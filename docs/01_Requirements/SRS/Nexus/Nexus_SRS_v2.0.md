# ONE CLASSICS NEXUS
## SOFTWARE REQUIREMENTS SPECIFICATION (SRS)
### Version 2.0 — Reverse-Engineered from Implementation, Numbered & Traceable

---

# 1. INTRODUCTION

## 1.1 Purpose
This Software Requirements Specification (SRS) documents the functional and non-functional requirements for the ONE Classics Nexus platform, as reverse-engineered from the current state of the solution codebase. It reflects what is actually implemented today, and separately flags requirements that are planned/partial but not yet realized.

## 1.2 Scope
- Vehicle registration and management
- Appraisal lifecycle tracking (status fields, assignment)
- Structured vehicle inspections using a canonical 12-system condition model
- Deterministic, explainable vehicle valuation (Automated Valuation Model / AVM)
- Comparable sales management and adjustment
- Valuation run persistence with full audit trace
- Report generation (partial/stubbed)
- A WinForms desktop client and a REST API, sharing Application/Domain/Infrastructure layers

## 1.3 Solution Structure
The solution is organized using a layered/clean-architecture approach:

| Project | Responsibility |
|---|---|
| `ONE_Classics_Nexus.Domain` | Entities, valuation engines, domain rules/constants (no external dependencies) |
| `ONE_Classics_Nexus.Application` | Use case handlers, DTOs, repository/service abstractions, request/response contracts |
| `ONE_Classics_Nexus.Infrastructure` | Dapper-based MySQL/MariaDB repositories, DI registration, valuation persistence |
| `ONE_Classics_Nexus.API` | ASP.NET Core Web API controllers, Swagger, global exception handling |
| `ONE_Classics_Nexus.WinForms` | Windows Forms desktop client (appraisal workspace, dashboards, forms) |
| `ONE_Classics_Nexus.Tests` | Automated test project |
| `WorkSpaceApp` | Legacy/experimental .NET Framework 4.8 WinForms combo-box control sandbox, unrelated to the Nexus domain |

## 1.4 Target Frameworks
- API / Application / Domain / Infrastructure / WinForms: **.NET 8**
- `WorkSpaceApp`: **.NET Framework 4.8** (standalone UI control prototyping project, not part of the Nexus business workflow)

## 1.5 System Principles (as implemented)
- Deterministic, rule-based valuation (no ML/black-box scoring)
- Canonical 12-system inspection model with fixed default weights
- Every valuation run persists a full, ordered trace of calculation steps for auditability
- Valuation engines are decomposed into single-responsibility components (Condition, Similarity, Comparable, Confidence, Market) composed by a top-level `DefaultValuationEngine`
- Engine/ruleset/rubric versions are stamped on every valuation result

---

# 2. SYSTEM OVERVIEW

## 2.1 Process Flow (as implemented)
```
Vehicle (create/register)
   → Appraisal (created against a Vehicle; tracks Intake/Inspection/Valuation/Report sub-statuses)
      → Inspection (12-system condition scoring, red flags, notes)
         → Condition Scores (per-system score 0–5, weight, red-flag flag)
      → Comparable Sales (added per appraisal, similarity/adjustment inputs)
      → Valuation Run (executed on demand via API; produces Final/Low/High value + confidence + trace)
      → Report (generation is partially stubbed; core repository not yet implemented)
```

## 2.2 Appraisal Status Model
An `Appraisal` entity tracks four independent sub-status fields in addition to an overall status:
- `Status` (overall; default `Draft`)
- `IntakeStatus` (default `Open`)
- `InspectionStatus` (default `NotStarted`)
- `ValuationStatus` (default `NotStarted`)
- `ReportStatus` (default `NotStarted`)

Appraisals also support optional assignment of an appraiser and reviewer (`AssignedAppraiserId`, `AssignedReviewerId`), and use an in-memory `RowVersion` counter incremented on every mutation (`Touch()`).

---

# 3. FUNCTIONAL REQUIREMENTS

## 3.1 Vehicle Management
FR-VEH-001 The system shall allow creation of a Vehicle with make, model, year, VIN, trim, series, body style, drivetrain, engine, transmission, mileage, odometer unit, colors, title status, authenticity/restoration/documentation profile, and notes. *(Implemented: `CreateVehicleHandler`, `VehiclesController`)*

FR-VEH-002 The system shall allow retrieval of a single Vehicle by ID. *(Implemented: `GetVehicleHandler`)*

FR-VEH-003 The system shall allow paged listing of Vehicles with filter criteria. *(Implemented: `ListVehiclesHandler`, `ListVehiclesRequest`, `PagedResult`)*

FR-VEH-004 The system shall allow updating an existing Vehicle's attributes. *(Implemented: `VehiclesController.Update` via `IVehicleApiService`)*

## 3.2 Appraisal Management
FR-APR-001 The system shall allow creation of an Appraisal linked to a Vehicle, with optional appraisal type, client, appraiser, reviewer, and appraisal request reference. *(Implemented: `CreateAppraisalHandler`)*

FR-APR-002 The system shall allow retrieval of an Appraisal by ID. *(Implemented: `GetAppraisalHandler`)*

FR-APR-003 The system shall reject Appraisal creation with an empty VehicleId or blank AppraisalType. *(Implemented: `Appraisal.Create` validation)*

FR-APR-004 The system shall support independent tracking of Intake, Inspection, Valuation, and Report sub-statuses per Appraisal. *(Implemented: entity fields; full workflow transitions not fully enforced in API yet)*

FR-APR-005 The system shall support assigning an appraiser and/or reviewer to an Appraisal. *(Implemented: `AssignAppraiser`, `AssignReviewer`)*

## 3.3 Inspection Engine
FR-INS-001 The system shall implement a canonical 12-system inspection model with fixed system keys, numbers, and default weights:

| # | System | Default Weight |
|---|--------|----------------|
| 1 | Exterior Body & Paint | 1.00 |
| 2 | Glass, Trim & Lighting | 0.70 |
| 3 | Interior Cabin | 0.90 |
| 4 | Dashboard & Controls | 0.60 |
| 5 | Engine Compartment | 1.00 |
| 6 | Fuel, Exhaust & Cooling | 0.75 |
| 7 | Transmission & Driveline | 0.95 |
| 8 | Suspension, Steering & Brakes | 0.95 |
| 9 | Chassis & Undercarriage | 0.90 |
| 10 | Wheels & Tires | 0.55 |
| 11 | Electrical & Accessories | 0.65 |
| 12 | Documentation & Authenticity | 0.85 |

*(Implemented: `InspectionSystems` constants class)*

FR-INS-002 The system shall allow creation of an Inspection against an Appraisal, referencing an inspection template, inspector, type (Standard/Custom/Classic), status, location, and summary notes. *(Implemented: `Inspection.Create`, `CreateInspectionHandler`)*

FR-INS-003 The system shall allow saving/updating per-system condition scores (0–5 scale), weight, red-flag flag, and notes for an Inspection. *(Implemented: `SaveInspectionHandler`, `ConditionScore.Update`, `PUT /api/inspections/{id}/scores`)*

FR-INS-004 The system shall allow retrieval of a single Inspection, all Inspections for an Appraisal, or all Inspections in the system. *(Implemented: `InspectionsController`)*

FR-INS-005 The system shall allow deletion of an Inspection. *(Implemented: `InspectionsController.Delete`; soft-delete semantics not confirmed in repository layer)*

FR-INS-006 The system shall track and expose an Inspection's completion percentage, overall condition score, and red-flag indicator via `ApplyScoreSummary`. *(Implemented)*

FR-INS-007 The system shall allow CRUD operations on individual Condition Scores via a dedicated endpoint (`/api/inspections/{inspectionId}/scores/{scoreId}`), separate from the bulk score-save endpoint. *(Implemented: `ConditionScoresController`)*

FR-INS-008 The system shall reject a Condition Score with a score outside the 0–5 range or a negative weight. *(Implemented: `ConditionScore.Create`/`Update` validation)*

## 3.4 Scoring / Condition Index
FR-SCR-001 The system shall calculate a subject vehicle's Condition Index (0–100 scale) as the weighted average of per-system scores (0–5), scaled by 10:
```
raw0To10 = Σ(score × weight) / Σ(weight)
conditionIndex0To100 = raw0To10 × 10
```
*(Implemented: `DefaultConditionEngine.CalculateConditionIndex0To100`)*

FR-SCR-002 The system shall apply a red-flag penalty to the Condition Index: 5 points per red-flagged system, capped at a maximum of 25 points total. *(Implemented: `RedFlagPenaltyPerItem = 5m`, `MaxRedFlagPenalty = 25m`)*

FR-SCR-003 The Condition Index shall be clamped to the 0–100 range after all adjustments. *(Implemented)*

FR-SCR-004 The system shall return a Condition Index of 0 when no condition scores are supplied or total weight is zero. *(Implemented)*

## 3.5 Comparable Sales & Similarity
FR-CMP-001 The system shall allow adding Comparable Sales to an Appraisal (make, model, year, series, body style, drivetrain, engine, transmission, mileage, sale price, sale date, source URL). *(Implemented: `ComparablesController`, `CreateComparableRequest`)*

FR-CMP-002 The system shall calculate a Similarity Score (0–1) between the subject vehicle and each comparable, using weighted attribute matching:

| Attribute | Weight |
|---|---|
| Series | 0.20 |
| Body Style | 0.20 |
| Drivetrain | 0.15 |
| Engine | 0.15 |
| Transmission | 0.10 |
| Year proximity | 0.10 |
| Mileage proximity | 0.10 |

Exact attribute matches score 1.00, mismatches score 0.00, and missing data on either side scores 0.50 (neutral). Year and mileage proximity use tiered proximity bands rather than binary match. *(Implemented: `DefaultSimilarityEngine`)*

FR-CMP-003 The system shall adjust each comparable's sale price for market time (0.25%/month elapsed since sale, non-negative months only) and mileage delta (10¢ per mile of mileage difference between subject and comparable, applied inversely). *(Implemented: `DefaultComparableEngine.CalculateTimeAdjustment` / `CalculateMileageAdjustment`, constants in `ValuationRuleConstants`)*

FR-CMP-004 Adjusted comparable prices shall never be negative (clamped to 0). *(Implemented)*

FR-CMP-005 The system shall allow updating an existing Comparable Sale. *(Implemented: `ComparablesController.Update`)*

FR-CMP-006 *(Planned, not yet implemented)* Condition, originality, equipment, venue, and geography adjustments to comparables are reserved as extension points in `DefaultComparableEngine` but are not yet implemented (explicitly commented as "Phase 3 Extensions").

## 3.6 Valuation Engine (AVM)
FR-VAL-001 The system shall reject a valuation run and return an `InsufficientData` status if no comparables are supplied. *(Implemented)*

FR-VAL-002 The system shall exclude comparables with a Similarity Score of 0 or less from the valuation, and return `InsufficientData` if no comparables remain usable. *(Implemented)*

FR-VAL-003 The system shall calculate a similarity-weighted indicated value from usable, adjusted comparables:
```
weightedIndication = Σ(adjustedPrice × similarity) / Σ(similarity)
```
*(Implemented — this replaces a strict "median comps" approach; the current engine uses a similarity-weighted mean, not a median.)*

FR-VAL-004 The system shall apply a Market Layer adjustment to the weighted indication. *(Implemented as a pass-through identity function in `DefaultMarketEngine`; no market-level adjustment logic currently applied — reserved extension point.)*

FR-VAL-005 The system shall calculate a Confidence score (0–1) as a weighted blend of three signals:

| Signal | Weight |
|---|---|
| Comparable count sufficiency (target = 5 comparables) | 0.35 |
| Average comparable similarity | 0.40 |
| Inspection completeness (distinct weighted systems present ÷ 12) | 0.25 |

*(Implemented: `DefaultConfidenceEngine`)*

FR-VAL-006 The system shall derive a value range width percentage from confidence:
```
rangeWidthPct = BaseRangeWidthPct (5%) + ((1 − confidence) × VariableRangeWidthPct (15%))
```
clamped to 0–100%. Low/High values are computed as `marketAdjustedValue × (1 ∓ rangeWidthPct)`, with Low floored at 0 and High floored at Low. *(Implemented)*

FR-VAL-007 The system shall round the Final, Low, and High values to whole currency units (away-from-zero), and Confidence to 4 decimal places. *(Implemented)*

FR-VAL-008 Every valuation result shall be stamped with an Engine Version, Ruleset Version, and Rubric Version. Current versions: `OCN-VAL-1.0.0`, `OCN-RULES-1.0.0`, `OCN-RUBRIC-1.0.0`. *(Implemented: `ValuationVersions`)*

FR-VAL-009 The system shall produce a complete, ordered Valuation Trace for every run, recording condition index, each comparable's adjustment, weighted indication, market-adjusted value, confidence, range width, and low/high values, including error trace lines for insufficient-data scenarios. *(Implemented: `ValuationTraceLine`, `DefaultValuationEngine`)*

FR-VAL-010 The system shall expose an endpoint to run a valuation for an Appraisal (`POST /api/appraisals/{appraisalId}/valuation/run`), list prior runs (`GET /api/appraisals/{appraisalId}/valuation/runs`), and retrieve a specific run's detail including trace (`GET /api/valuation/runs/{valuationRunId}`). *(Implemented: `ValuationController`; note that `IValuationApiService` registration is currently commented out in `Program.cs`, meaning this endpoint set is not yet wired for dependency injection in the API host.)*

FR-VAL-011 Valuation runs and their trace lines shall be persisted immutably to `valuation_runs` and `valuation_trace_lines` tables. *(Implemented: `ValuationRunRepository`)*

FR-VAL-012 *(Not implemented)* There is no explicit tiering model (e.g., T1–T7) in the current codebase. Any tier-based multiplier concept referenced in earlier design drafts is superseded by the similarity-weighted comparable approach described in FR-VAL-003 and is not present in the Domain layer.

## 3.7 Reports
FR-RPT-001 The system shall expose endpoints to create a report for an Appraisal (`POST /api/appraisals/{appraisalId}/report`) and retrieve the latest report (`GET /api/appraisals/{appraisalId}/report`), requiring a template key and output format. *(Implemented at controller/contract level: `ReportsController`.)*

FR-RPT-002 *(Not implemented)* The underlying report generation service (`IReportApiService`) and repository (`AppraisalReportRepository`, `ReportExportRepository`) are present as stubs/empty classes or are commented out of DI registration in `Program.cs`. Report generation, Draft→Final lifecycle, and locking of finalized reports are not yet functional.

## 3.8 Admin / Configuration
FR-ADM-001 *(Not implemented)* No Admin UI, admin-only authorization checks, or system-configuration versioning endpoints currently exist in the API. `VehicleTypeSystemWeightRepository` is present as an empty stub, indicating vehicle-type-specific weight configuration is planned but not implemented — the system currently uses only the fixed default weights in `InspectionSystems`.

FR-ADM-002 *(Not implemented)* There is no persisted history of configuration changes, nor validation that vehicle-type weights sum to 1.00, since no vehicle-type weight override mechanism is implemented yet.

---

# 4. NON-FUNCTIONAL REQUIREMENTS

NFR-ARCH-001 The system shall follow a layered architecture (Domain → Application → Infrastructure → API/WinForms) with no circular dependencies. *(Implemented, as evidenced by project references and folder structure.)*

NFR-DATA-001 All persistence shall use Dapper against a MySQL/MariaDB-compatible database (`MySqlConnectionFactory`, `MariaDbConnectionFactory`). *(Implemented)*

NFR-DATA-002 Valuation runs shall be immutable once persisted (no update path exists in `ValuationRunRepository`, only insert). *(Implemented)*

NFR-ERR-001 The API shall return standardized `ProblemDetails` responses for unhandled exceptions via centralized middleware (`GlobalExceptionHandlingMiddleware`, `AddProblemDetails`). *(Implemented)*

NFR-DOC-001 The API shall expose Swagger/OpenAPI documentation with XML comments in Development and Testing environments. *(Implemented)*

NFR-SEC-001 *(Not implemented)* No authentication (JWT or otherwise) is currently configured in `Program.cs` — only `UseAuthorization()` is present with no corresponding `AddAuthentication`/`UseAuthentication` call, and no `[Authorize]` attributes appear on controllers. Security requirements (JWT auth, RBAC) are aspirational, not current state.

NFR-PERF-001 *(Unverified)* No explicit performance targets (e.g., valuation < 2s) are enforced or tested in the current codebase. Any such target should be treated as a design goal pending benchmarking, not a verified requirement.

NFR-TEST-001 A dedicated test project (`ONE_Classics_Nexus.Tests`) exists for automated testing of the solution.

---

# 5. DATA REQUIREMENTS

DATA-001 Condition Scores are stored per Inspection with system number/key, score (0–5), weight, red-flag flag, and notes. *(Table implied by `ConditionScoreRepository`; entity: `ConditionScore`.)*

DATA-002 Inspection System weights are currently hard-coded defaults (`InspectionSystems.All`) rather than persisted, configurable data. Per-vehicle-type weight overrides are planned (`VehicleTypeSystemWeightRepository` stub) but not implemented.

DATA-003 Valuation Runs are persisted with engine/rubric/ruleset versions, final/low/high value, confidence, and status (`valuation_runs` table).

DATA-004 Valuation Trace Lines are persisted per run with sort order, type, key, amount, factor, message, and evidence reference (`valuation_trace_lines` table).

DATA-005 Comparable Sales and their per-comparable adjustments are managed via `ComparableRepository` and `ComparableAdjustmentRepository`.

DATA-006 Appraisal, Vehicle, and Inspection entities each track `CreatedAtUtc`/`UpdatedAtUtc` timestamps; Appraisal and Inspection additionally track an in-memory optimistic-concurrency `RowVersion` counter.

---

# 6. API REQUIREMENTS (Implemented Endpoints)

## Vehicles
- `POST /api/vehicles`
- `GET /api/vehicles`
- `GET /api/vehicles/{VehicleId}`
- `PUT /api/vehicles/{vehicleId}`

## Appraisals
- `POST /api/appraisals`
- `GET /api/appraisals/{appraisalId}`

## Inspections
- `POST /api/inspections`
- `GET /api/inspections/{inspectionId}`
- `GET /api/appraisals/{appraisalId}/inspections`
- `PUT /api/inspections/{inspectionId}`
- `DELETE /api/inspections/{inspectionId}`
- `PUT /api/inspections/{inspectionId}/scores`
- `GET /api/inspections`

## Condition Scores
- `GET /api/inspections/{inspectionId}/scores`
- `GET /api/inspections/{inspectionId}/scores/{scoreId}`
- `POST /api/inspections/{inspectionId}/scores`
- `PUT /api/inspections/{inspectionId}/scores/{scoreId}`
- `DELETE /api/inspections/{inspectionId}/scores/{scoreId}`

## Comparables
- `POST /api/appraisals/{appraisalId}/comparables`
- `GET /api/appraisals/{appraisalId}/comparables`
- `PUT /api/comparables/{comparableId}`

## Valuation
- `POST /api/appraisals/{appraisalId}/valuation/run` *(controller present; service registration currently commented out in `Program.cs`)*
- `GET /api/appraisals/{appraisalId}/valuation/runs` *(same caveat)*
- `GET /api/valuation/runs/{valuationRunId}` *(same caveat)*

## Reports
- `POST /api/appraisals/{appraisalId}/report` *(controller/contract present; service registration commented out, backing repository is a stub)*
- `GET /api/appraisals/{appraisalId}/report` *(same caveat)*

## Not Yet Present
- No `/admin/*` endpoints exist in the API surface.

---

# 7. TRACEABILITY MATRIX

| Req ID | Module | Status |
|--------|--------|--------|
| FR-VEH-001..004 | Vehicles | Implemented |
| FR-APR-001..005 | Appraisals | Implemented |
| FR-INS-001..008 | Inspection Engine | Implemented |
| FR-SCR-001..004 | Scoring / Condition Index | Implemented |
| FR-CMP-001..005 | Comparables & Similarity | Implemented |
| FR-CMP-006 | Comparable Adjustment Extensions | Planned |
| FR-VAL-001..009, 011 | Valuation Engine | Implemented |
| FR-VAL-010 | Valuation API | Implemented (DI wiring incomplete) |
| FR-VAL-012 | Tiering Model | Not Implemented / Superseded |
| FR-RPT-001 | Report API Surface | Implemented (contracts only) |
| FR-RPT-002 | Report Generation | Not Implemented |
| FR-ADM-001..002 | Admin / Config Governance | Not Implemented |
| NFR-SEC-001 | Auth / RBAC | Not Implemented |
| NFR-PERF-001 | Performance Targets | Unverified |

---

# 8. KEY DIFFERENCES FROM PRIOR SRS (v2.0 draft)

1. **Valuation formula**: Prior draft specified "median comps" and a "tier multiplier." The implemented engine uses a **similarity-weighted mean** of adjusted comparables; there is no tiering (T1–T7) concept anywhere in the Domain layer.
2. **Security**: Prior draft specified JWT auth and RBAC as implemented (NFR-SEC-001/002). The current `Program.cs` has no authentication middleware or `[Authorize]` attributes — this is a gap, not a delivered feature.
3. **Admin governance**: Prior draft assumed a working Admin UI and versioned system configuration. The actual weight-configuration repository is an empty stub; only fixed default weights exist.
4. **Reports**: Prior draft assumed a full Draft→Final→Lock report lifecycle. Only the API contracts/controller exist; the service and repository are stubbed or commented out of DI.
5. **Scoring scale**: Condition scores are 0–5 (not "DECIMAL(3,2)" 0-per-item as ambiguously stated before); the condition index is derived as weighted-average × 10, not a literal "Σ(system_value) × 20" formula.

---

# END
