# ONE Classics Nexus
# Repository Mapping Specification
**Version:** 1.1  
**Source Schema:** `Nexus_Full_Lock_Schema_v1.0.sql`  
**Prepared For:** Technical Lead, DBA, API Team, QA  
**Status:** Aligned to codebase (dev branch) — living document reflecting implemented repositories and API surface

---

## 1. Purpose

This specification maps the locked Nexus database schema to repository responsibilities, expected query operations, DTO/application dependencies, and validation checkpoints. It now reflects the current implementation status found in the `dev` branch of the ONE_Classics_Nexus codebase and is meant to prevent schema drift between MariaDB tables, Dapper repositories, API contracts, and QA test cases.

## 2. Repository Mapping Rules

- Repositories must use `snake_case` database columns exactly as defined in the lock schema whenever they build SQL by hand.
- Domain/Application models may use PascalCase properties; Dapper SQL must alias columns when required (application code uses DTO/entity mappers).
- GUID identifiers are stored as `CHAR(36)` and repositories generally pass GUIDs to SQL as `guid.ToString()` (many implemented repositories follow this pattern).
- Insert, update, select, and delete SQL must be aligned to this mapping before API endpoints are considered stable.
- API contracts remain DTO-only; repositories return domain entities or DTOs as defined by application abstractions.
- `condition_scores` is the canonical AVM 12-system condition input table — in the current codebase its management is performed inside `InspectionRepository` (see Implementation Notes).
- `valuation_runs` and `valuation_trace_lines` are the canonical valuation execution/audit tables and are implemented by `ValuationRunRepository` / `ValuationTraceRepository` (partial/implemented as noted below).

## 3. Repository Inventory by Entity / Workstream (code-aligned)

This inventory lists repository classes found in `ONE_Classics_Nexus.Infrastructure` and their current implementation status in `dev`.

- Vehicles
  - `vehicles` → `VehicleRepository` — Implemented (Create/Insert, GetById, Update, ExistsByVin). Delete not implemented in API or repository.
  - `vehicle_types` → `VehicleTypeRepository` — Implemented / lookup support (partial feature coverage).
  - `vehicle_type_system_weights` → `VehicleTypeSystemWeightRepository` — Placeholder/partial (exists, verify seed/upsert coverage)

- Appraisal
  - `appraisal_requests` → `AppraisalRequestRepository` — Present (implementation status: partial/needs validation)
  - `appraisals` → `AppraisalRepository` — Implemented (Insert/Create, GetById, Update with optimistic concurrency via `row_version`).
  - `appraisal_assignments` → `AppraisalAssignmentRepository` — Present (partial)

- Inspection
  - `inspection_templates` → `InspectionTemplateRepository` — Present (partial)
  - `inspection_template_sections` → `InspectionTemplateSectionRepository` — Present (partial)
  - `inspection_template_items` → `InspectionTemplateItemRepository` — Present (partial)
  - `inspections` → `InspectionRepository` — Implemented (Create, GetById, Update, SaveScores transactional). Note: `condition_scores` operations are executed inside this repository.
  - `inspection_section_scores` → `InspectionSectionScoreRepository` — Placeholder
  - `inspection_item_scores` → `InspectionItemScoreRepository` — Placeholder
  - `inspection_media` → `InspectionMediaRepository` — Placeholder/partial
  - `inspection_red_flags` → `InspectionRedFlagRepository` — Placeholder/partial
  - `condition_scores` → `ConditionScoreRepository` — Placeholder (explicit condition score table is managed by `InspectionRepository` currently).

- Comparables
  - `comparable_sales` → `ComparableRepository` — Implemented (Create, GetByAppraisalId). Repository does not currently provide an Update method even though API exposes a PUT for comparables (see Implementation Notes).
  - `comparable_adjustments` → `ComparableAdjustmentRepository` — Present (partial)

- Valuation
  - `valuation_runs` → `ValuationRunRepository` — Implemented/partial (run persistence and queries exist in `ValuationRunRepository` and `Valuation` subfolders; validate trace persistence coverage).
  - `valuation_comparable_links` → `ValuationComparableLinkRepository` — Implemented/partial
  - `valuation_trace_lines` → `ValuationTraceRepository` — Implemented/partial

- Reporting
  - `appraisal_reports` → `AppraisalReportRepository` — Present (partial)
  - `report_exports` → `ReportExportRepository` — Present (partial)

- Security / Admin
  - `users` → `UserRepository` — Placeholder (internal class stub present but not implemented)
  - `roles` → `RoleRepository` — Placeholder (internal class stub present)
  - `user_roles` → `UserRoleRepository` — Present (partial)

Notes:
- "Implemented" indicates repository methods are present and exercised by application/service code.
- "Partial/Placeholder" indicates the class exists but either contains limited behavior or is a stub. These should be validated and completed before relying on them for production features.
- Some repositories (e.g., `ConditionScoreRepository`) are intentionally not used because their table is managed inside another repository (inspection vertical).

## 4. Key Table-to-Repository Alignments (high level)

- `inspections` + `condition_scores` — canonical handling is implemented inside `InspectionRepository` using transactions. Any external caller should use `IInspectionRepository` and the API endpoints in `InspectionsController`.
- `appraisals` — `AppraisalRepository` implements optimistic concurrency. `row_version` is used in UPDATE WHERE clause and DB increments are mirrored in the entity.
- `vehicles` — `VehicleRepository` persists additional valuation-ready fields (e.g., `authenticity_profile`, `restoration_level`, `documentation_level`, `overall_condition_score`, `has_red_flags`, etc.). Ensure DB schema has matching columns prior to enabling those fields in the API.
- `comparable_sales` — `ComparableRepository` persists comparable sales and returns `ComparableDto`. The API has a PUT endpoint to update comparables but the repository currently only implements Create and Get. Implement repository-level Update/Delete to match API.

## 5. API Endpoints (as implemented in API controllers)

The current controllers and routes in `ONE_Classics_Nexus.API` include (non-exhaustive):

- Vehicles
  - POST `/api/vehicles` → create vehicle (validations in controller)
  - GET `/api/vehicles/{vehicleId}` → get by id
  - PUT `/api/vehicles/{vehicleId}` → update vehicle (No DELETE implemented)

- Appraisals
  - POST `/api/appraisals` → create appraisal (validates `VehicleId` exists at service level)
  - GET `/api/appraisals/{appraisalId}` → get appraisal
  - PUT `/api/appraisals/{appraisalId}` → update appraisal (status validation)

- Inspections
  - POST `/api/inspections` → create inspection (with initial scores)
  - GET `/api/inspections/{inspectionId}` → get inspection with scores
  - PUT `/api/inspections/{inspectionId}/scores` → replace the 12 canonical condition scores (controller validates 12 unique systems)
  - PUT `/api/inspections/{inspectionId}` → update inspection header and scores

- Comparables
  - POST `/api/appraisals/{appraisalId}/comparables` → create comparable
  - GET `/api/appraisals/{appraisalId}/comparables` → list comparables for an appraisal
  - PUT `/api/comparables/{comparableId}` → update comparable (API exists; repository update missing)

- Valuation
  - POST `/api/appraisals/{appraisalId}/valuation/run` → run valuation and persist `valuation_runs` and `valuation_trace_lines`
  - GET `/api/appraisals/{appraisalId}/valuation/runs` → list runs for appraisal
  - GET `/api/valuation/runs/{valuationRunId}` → get run detail

- Reporting
  - Controllers exist to manage report and export contracts; verify repository coverage for `appraisal_reports` and `report_exports` before enabling automated exports.

## 6. Repository Contracts (application layer interfaces)

Current repository interface signatures (present in `ONE_Classics_Nexus.Application`) should be considered authoritative for service integration. Examples:

- Vehicles (`IVehicleRepository`)
  - `Task<Vehicle?> InsertAsync(Vehicle vehicle, CancellationToken cancellationToken = default);`
  - `Task<Vehicle?> CreateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);` (adapter/compat)
  - `Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);`
  - `Task<Vehicle?> UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);`
  - `Task<bool> ExistsByVinAsync(string vin, CancellationToken cancellationToken = default);`

- Appraisals (`IAppraisalRepository`)
  - `Task<Appraisal> InsertAsync(Appraisal appraisal, CancellationToken cancellationToken = default);`
  - `Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken cancellationToken = default);`
  - `Task<Appraisal?> UpdateAsync(Appraisal appraisal, CancellationToken cancellationToken = default);`
  - `Task<Appraisal?> GetByIdAsync(Guid appraisalId, CancellationToken cancellationToken = default);`

- Inspections (`IInspectionRepository`)
  - `Task<Inspection> CreateAsync(Inspection inspection, CancellationToken ct);`
  - `Task<Inspection?> GetByIdAsync(Guid inspectionId, CancellationToken ct);`
  - `Task<Inspection> UpdateAsync(Inspection inspection, CancellationToken ct);`
  - `Task<Inspection?> InsertAsync(Guid inspectionId, CancellationToken ct);` (stub)
  - `Task SaveScoresAsync(Guid inspectionId, IReadOnlyList<ConditionScore> scores, CancellationToken ct);`

- Comparables (`IComparableRepository`)
  - `Task<ComparableDto> CreateAsync(Guid appraisalId, CreateComparableRequest request, CancellationToken ct);`
  - `Task<IReadOnlyList<ComparableDto>> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken ct);`
  - (No Update/Delete in interface — extend interface if API update/delete must be backed by repository methods.)

- Valuation run repository (`IValuationRunRepository` / `ValuationRunRepository`)
  - Typical methods: `InsertRunAsync`, `GetByIdAsync`, `GetByAppraisalIdAsync`, `MarkStatusAsync` (partial — consult implementation in `ONE_Classics_Nexus.Infrastructure.Valuation`).

## 7. Implementation Notes & Action Items

- Vehicle DELETE: Currently there is no DELETE endpoint or `DeleteAsync` in `IVehicleRepository`. Action: implement `DeleteAsync(Guid vehicleId)` in repository and expose DELETE `/api/vehicles/{id}` when audit policy is settled (hard delete vs soft delete).

- Comparable Update mismatch: `ComparablesController` exposes PUT `/api/comparables/{id}` and controller calls API service `UpdateAsync`, however `ComparableRepository` does not expose an Update method. Action: add an Update method to `IComparableRepository` and implement `ComparableRepository.UpdateAsync` to persist changes.

- Condition scores ownership: `ConditionScoreRepository` is a placeholder. `InspectionRepository` currently owns lifecycle of `condition_scores` (insert, replace, delete) using transactions. If a separate repository abstraction for condition scores is desired, refactor `InspectionRepository` to delegate to `ConditionScoreRepository` while preserving transactional integrity.

- Optimistic concurrency on appraisals: `AppraisalRepository.UpdateAsync` increments `row_version` at DB and checks `row_version` in WHERE clause. Services and API must pass the current `row_version` to detect conflicts. This behavior is already implemented; ensure UI/service layer surfaces 409/NotFound mapping as needed.

- Timestamps and defaults: Many repositories set `CreatedAtUtc` and `UpdatedAtUtc` in code before insert to ensure consistent values across environments. Continue this practice or standardize on DB defaults if desired.

- Repository naming vs tables: The code uses a mix of column names (`subsystem_key`, `score`) in the `condition_scores` SQL inside `InspectionRepository`. Verify SQL column names match lock schema or update SQL/DB to avoid runtime errors.

## 8. Validation and QA Checklist (code-aligned)

- Schema: Verify repository INSERT/UPDATE column lists match DB schema — run integration tests to catch `Unknown column` SQL errors.
- IDs: Ensure GUIDs are passed in the expected string format (`CHAR(36)`) where repositories use `ToString()`; tests should include invalid GUID / missing GUID cases.
- API: Controllers validate inputs and return ProblemDetails for 400, 404 where appropriate — ensure integration tests assert these responses.
- DB: Foreign key existence checks are enforced in service layer when necessary (e.g., validating `vehicle_id` before creating appraisal) — add service-level tests.
- Valuation: Ensure `valuation_runs` and `valuation_trace_lines` persist together; a partially committed run without trace lines is not acceptable.
- Inspection: `InspectionsController` requires exactly 12 canonical condition scores when saving — repository logic should enforce the same cardinality and mapping.

## 9. Recommended Next Actions (dev/prioritized)

1. Implement `DeleteAsync` in `IVehicleRepository` / `VehicleRepository` and add DELETE endpoint in `VehiclesController` if hard delete is approved.
2. Add `UpdateAsync` / `DeleteAsync` to comparable repository and implement in `ComparableRepository` to match API contract.
3. Decide repository ownership for `condition_scores`: either keep it inside `InspectionRepository` (transactional) or implement `ConditionScoreRepository` and refactor.
4. Fill in placeholder repository classes for critical admin/lookup tables (e.g., `users`, `roles`) or remove stubs if not needed.
5. Add integration tests to assert column-level SQL mapping and API -> repository end-to-end flows (Vehicle, Appraisal, Inspection, Comparables, Valuation).

---

This document is intentionally aligned to the `dev` branch implementation and should be updated whenever repository implementations change. Keep repository interfaces (`ONE_Classics_Nexus.Application.Abstractions.Repositories`) as the authoritative contract for service-level changes.