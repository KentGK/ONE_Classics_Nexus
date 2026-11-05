# ONE Classics Nexus  
# Developer Design Specification v2.0 (UPDATED to repository state)

---

## 1. PURPOSE
This DDS defines the implementation blueprint for the ONE Classics Nexus platform.

It translates:
- SRS v2.0
- Inspection Data Model Specification v1.0
- Nexus AVM Math Specification v2.0 (LOCKED)

into:
→ API, database, domain, and engine implementation.

---

## 2. SYSTEM OVERVIEW

### Workflow
Client → Vehicle → Appraisal → Inspection → Condition Scores → Condition Index → Tier → Valuation → Report

### Architecture
WinForms UI → API → Application → Domain → Infrastructure → MariaDB

---

## 3. REQUIREMENTS → IMPLEMENTATION

### Inspection
- POST /api/inspections → CreateInspectionHandler
- PUT /api/inspections/{id}/scores → SaveInspectionHandler
- GET /api/inspections/{id}
- PUT /api/inspections/{id} → Update inspection metadata

### Scoring
- domain score range: 0–5 (decimal)
- persistence: stored as 0–10 in DB (repository multiplies domain value × 2)
- condition_index = Σ(score × weight) × 20 (computed in engine; domain uses 0–5)
- weights must equal 1.00 (validation required)

### Valuation
- ComparableEngine → comparable data retrieval
- SimilarityEngine → similarity scoring and weighting
- ComparableEngine → base_market_value selection
- TierEngine → multiplier
- RedFlagEngine → penalties
- CollectorValueEngine → collector adjustment
- ConfidenceEngine → reliability
- ValuationEngine → final value
- TraceEngine → audit/explainability output

---

## 4. DOMAIN MODEL (current repository types)

### Vehicle
Guid VehicleId  
Guid VehicleTypeId  
string? Vin  
int Year  
string Make  
string Model  
string? Trim  
string? SeriesName  
string? BodyStyle  
string? Drivetrain  
string? Engine  
string? Transmission  
int? Mileage  
string OdometerUnit (default "miles")  
string? ExteriorColor  
string? InteriorColor  
string? TitleStatus  
string? AuthenticityProfile  
string? RestorationLevel  
string? DocumentationLevel  
decimal? OverallConditionScore  
bool HasRedFlags  
string? OwnershipNotes  
string? Notes  
DateTime CreatedAtUtc  
DateTime UpdatedAtUtc

### Appraisal
Guid AppraisalId  
string? AppraisalNumber  
Guid? AppraisalRequestId  
Guid VehicleId  
Guid? ClientId  
Guid? AssignedAppraiserId  
Guid? AssignedReviewerId  
string AppraisalType (default "Standard")  
string Status (default "Draft")  
string? IntakeStatus  
string? InspectionStatus  
string? ValuationStatus  
string? ReportStatus  
DateTime? EffectiveDateUtc  
long RowVersion (optimistic concurrency)  
DateTime CreatedAtUtc  
DateTime UpdatedAtUtc

### Inspection
Guid InspectionId  
Guid AppraisalId  
string InspectionType (default "Standard")  
DateTime CreatedAtUtc  
DateTime UpdatedAtUtc  
List<ConditionScore> ConditionScores

### ConditionScore
Guid ConditionScoreId  
Guid InspectionId  
int SystemNumber (domain may use SystemNumber but DB orders by subsystem key)  
string SystemKey (domain)  
decimal Score0To5 (domain, 0–5)  
decimal Weight (domain)  
bool IsRedFlag  
string? Notes

---

## 5. DATABASE DESIGN (current repository schema / Phase 2 notes)

### inspections
- inspection_id CHAR(36) PK  
- appraisal_id CHAR(36)  
- inspection_type VARCHAR  
- created_at_utc DATETIME  
- updated_at_utc DATETIME

### condition_scores (AUTHORITATIVE, Phase 2 schema)
- condition_score_id CHAR(36) PK  
- appraisal_id CHAR(36)  
- inspection_id CHAR(36)  
- subsystem_key VARCHAR  -- maps to domain `SystemKey`
- score DECIMAL(4,2)     -- stored as 0–10 scale (domain 0–5 is multiplied by 2 on write, divided by 2 on read)
- weight DECIMAL(5,4)
- is_red_flag BOOLEAN
- notes TEXT
- created_at DATETIME
- updated_at DATETIME

Notes:
- The repository layer performs a conversion: domain `Score0To5` → DB `score = Score0To5 * 2`. When reading, DB `score / 2.0` maps back to `Score0To5`.
- Column names differ from earlier spec (e.g., `subsystem_key` vs `system_key`, `score` stored 0–10). DDS must treat `condition_scores` (Phase 2 schema) as authoritative for persistence and ETL.

### Valuation Status Codes (current set)
- SUCCESS  
- INCOMPLETE_INSPECTION  
- INSUFFICIENT_COMPARABLES  
- INVALID_INPUT  
- INVALID_WEIGHT_PROFILE  
- INVALID_COMPARABLE_DATA  
- NEEDS_REVIEW  
- ERROR

LOCK RULE:
- `condition_scores` is the authoritative scoring table; the repository enforces the Phase 2 column naming and the 0–10 storage convention.

---

## 6. API DESIGN (current handlers / routes)

Inspections
- POST /api/inspections → create inspection
- GET /api/inspections/{id} → retrieve inspection + scores (repository converts DB → domain)
- PUT /api/inspections/{id} → update inspection metadata (inspection_type, timestamps)
- PUT /api/inspections/{id}/scores → replace/save scores (SaveInspectionHandler)

Vehicles
- POST /api/vehicles
- GET /api/vehicles/{id}
- PUT /api/vehicles/{id}
- DELETE /api/vehicles/{id}

Appraisals
- POST /api/appraisals
- GET /api/appraisals/{id}
- PUT /api/appraisals/{id}
- DELETE /api/appraisals/{id}

Comparables & Valuation
- POST /api/appraisals/{id}/comparables
- GET /api/appraisals/{id}/comparables
- PUT /api/comparables/{id}
- POST /api/appraisals/{id}/valuation/run
- GET /api/appraisals/{id}/valuation/runs
- GET /api/valuation/runs/{id}
- GET /api/valuation/runs/{id}/trace
- DELETE /api/comparables/{id}

---

## 7. APPLICATION LAYER

Handlers implemented in repository:
- `CreateInspectionHandler` (creates Inspection domain entity and persists via `InspectionRepository.CreateAsync`)
- `SaveInspectionHandler` (replaces scores via `InspectionRepository.SaveScoresAsync`)
- `RunValuationHandler` (valuation orchestration)

---

## 8. ENGINE DESIGN

Pipeline (current implementation intent):
ConditionEngine → TierEngine → ComparableEngine → SimilarityEngine → RedFlagEngine → CollectorValueEngine → ConfidenceEngine → ValuationEngine → TraceEngine

Formulas:
- condition_index = Σ(score × weight) × 20  (score here is domain 0–5)
- weights must sum to 1.00
- result range: 0–100
- base_market_value = similarity_weighted_base_value when accepted similarity-weighted comps ≥ 3, otherwise median_comparable_value when valid; else INSUFFICIENT_COMPARABLES
- final_value = base_market_value × tier_multiplier × red_flag_modifier_total × collector_adjustment × confidence_modifier
- reported_final_value = ROUND(final_value, nearest 100)

---

## 9. VALIDATION RULES (enforced in application/domain)
- score must be 0–5 (domain validation)
- 12 systems required (domain/validation layer)
- weights must sum to 1.00 (application validation before persist)
- no null scores
- no duplicate system_number or system_key
- repository-level expectations: `ConditionScore` writes must populate `ConditionScoreId`, `SystemKey`, `Score0To5`, `Weight`, `IsRedFlag`, and `Notes`; repository maps to DB columns and scaling.

---

## 10. TRACEABILITY

Minimum Trace Keys:
- INPUT_VALIDATION
- CONDITION_SCORE
- CONDITION_TIER
- BASE_MARKET_VALUE
- RED_FLAG_MODIFIER
- COLLECTOR_VALUE
- CONFIDENCE_SCORE
- FINAL_VALUE
- VALUE_RANGE
- STATUS

---

## 11. EXECUTION FLOW (current)
Inspection → Validate → Scores → Condition Index → Tier → Comps → Similarity → Red Flags → Collector Value → Confidence → Final Value → Trace → Persist

Notes:
- Persisted scores use Phase 2 schema; application layer and repository are responsible for conversion between domain model and DB representation.

---

## 12. PERFORMANCE

- Save: < 500ms (target)
- Valuation: < 2s (target)

---

## 13. ERROR HANDLING

- application/problem+json
- 400 validation  
- 404 not found  
- 409 conflict  
- 500 server error  

---

## 14. RISKS

- schema mismatch between domain and DB (conversion already present for score scale)
- Dapper mapping issues (explicit SQL and DTO mapping used)
- weight imbalance
- missing systems
- API route conflicts
- optimistic concurrency expectations (Appraisal.RowVersion)

---

## 15. ACCEPTANCE

- deterministic valuation
- golden test passes
- full trace output
- API + DB aligned (repository is authoritative for persistence mapping)

---

## 16. FINAL

This DDS reflects the repository state (Phase 2 schema for `condition_scores`, DB scaling of scores, expanded `Vehicle` and `Appraisal` models). Treat this file as the authoritative design document to be updated in-sync with code changes.

---

END