# ONE Classics Nexus — API ↔ DB ↔ Repository Mapping Matrix (v1.0)

## Purpose
This matrix enforces alignment between:
- API Endpoints (Controllers / DTOs)
- Application Services
- Repositories
- Database Tables (Canonical Schema)

This is the **anti-drift control layer** for Nexus.

---

## Conventions
- UUIDs: Guid (C#) ↔ CHAR(36) (MariaDB)
- Scores: DECIMAL(3,2) in `score_0_to_5`
- 12-System keys: fixed `system_number` (1–12) and `system_key` (snake_case)
- No `SELECT *` in repositories; explicit column lists only

---

# 🧩 Core Mapping Matrix

| Domain | API Endpoint | Controller | Service | Repository | Database Tables | Key Columns |
|---|---|---|---|---|---|---|
| Vehicle | POST /api/vehicles | VehiclesController | IVehicleService | IVehicleRepository | vehicles | vehicle_id, vin, year, make, model |
| Vehicle | GET /api/vehicles/{id} | VehiclesController | IVehicleService | IVehicleRepository | vehicles | vehicle_id |
| Appraisal | POST /api/appraisals | AppraisalsController | IAppraisalService | IAppraisalRepository | appraisals, vehicles | appraisal_id, vehicle_id, status |
| Appraisal | GET /api/appraisals/{id} | AppraisalsController | IAppraisalService | IAppraisalRepository | appraisals | appraisal_id, status |
| Inspection | POST /api/inspections | InspectionsController | IInspectionService | IInspectionRepository | inspections | inspection_id, appraisal_id, template_id |
| Inspection Scores | PUT /api/inspections/{id}/section-scores | InspectionsController | IInspectionService | IInspectionRepository | inspection_section_scores | inspection_id, system_number, system_key, score_0_to_5 |
| Red Flags | POST /api/inspections/{id}/red-flags | InspectionsController | IInspectionService | IInspectionRepository | inspection_red_flags | severity, system_number, item_key |
| Templates | POST /api/inspection-templates | InspectionTemplatesController | IInspectionTemplateService | IInspectionTemplateRepository | inspection_templates, inspection_template_sections, inspection_template_items | template_id, version_label, status |
| Comparables | POST /api/comparables | ComparablesController | IComparableService | IComparableRepository | comparable_sales | comparable_id, sale_price, sale_date |
| Valuation Run | POST /api/appraisals/{id}/valuation/run | ValuationRunsController | IValuationService | IValuationRunRepository | valuation_runs, valuation_comparable_links, valuation_trace_lines | valuation_run_id, final_value, confidence_score |
| Reports | POST /api/appraisals/{id}/reports | ReportsController | IReportService | IReportRepository | appraisal_reports | report_id, report_status, finalized_at_utc |
| Admin Config | POST /api/admin/system-config | AdminSystemConfigController | ISystemConfigService | ISystemConfigRepository | system_config_versions, vehicle_type_weight_profiles, tier_rules | config_version_id, effective_from_utc |

---

# 🔗 Detailed Domain Mapping

## Inspection Domain

| Component | Mapping |
|---|---|
| Controller | InspectionsController |
| Service | IInspectionService |
| Repository | IInspectionRepository |
| Tables | inspections, inspection_section_scores, inspection_item_scores, inspection_red_flags |
| Critical Columns | system_number, system_key, score_0_to_5 |

---

## Valuation Domain

| Component | Mapping |
|---|---|
| Controller | ValuationRunsController |
| Service | IValuationService |
| Repository | IValuationRunRepository |
| Tables | valuation_runs, valuation_comparable_links, valuation_trace_lines |
| Critical Columns | final_value, value_low, value_high, confidence_score |

---

## Admin Configuration Domain

| Component | Mapping |
|---|---|
| Controller | AdminSystemConfigController |
| Service | ISystemConfigService |
| Repository | ISystemConfigRepository |
| Tables | system_config_versions, vehicle_type_weight_profiles, tier_rules |
| Critical Columns | version_label, effective_from_utc |

---

# 🚨 Anti-Drift Rules

1. API contracts must match DTOs exactly.
2. DTOs must match repository projections.
3. Repository SQL must match database schema.
4. Schema changes require:
   - Migration script
   - DDS update
   - Mapping matrix update

---

# ✅ Definition of Done (Mapping)

- Every API endpoint maps to:
  - Controller
  - Service
  - Repository
  - Database table(s)
- No unknown-column runtime errors
- No route conflicts in Swagger
- All mappings covered by integration tests

---

# 🚀 Next Step

Use this matrix to generate:
- Integration test coverage
- Repository SQL verification tests
- Swagger contract validation

This document must be updated **every time**:
- A new endpoint is added
- A table changes
- A DTO changes
