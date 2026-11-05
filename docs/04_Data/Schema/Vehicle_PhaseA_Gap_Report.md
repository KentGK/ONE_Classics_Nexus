# ONE Classics Nexus — Vehicle Phase A Gap Report

**Scope:** DDS v1.0 Section `73` — *Phase A: Reconcile Existing Vehicle Schema*  
**Date:** 2026-09-14  
**Source Basis:** Repository artifacts only (no live DB inspection in this report)

---

## 1) Artifacts Compared

- `docs/04_Data/SQL/CREATE_Vehicles.sql` (legacy SQL script)
- `docs/04_Data/Schema/Vehicles_DDL.txt` (current schema documentation)
- `ONE_Classics_Nexus.Domain/Entities/Vehicle.cs` (domain model)
- `ONE_Classics_Nexus.Infrastructure/Repositories/Vehicle/VehicleRepository.cs` (persistence mapping)

---

## 2) Phase A Baseline (DDS Section 73)

Phase A requires reconciliation of the existing `vehicles` schema for the baseline fields:

`vehicle_id`, `vin`, `year`, `make`, `model`, `trim`, `series_name`, `body_style`, `drivetrain`, `engine`, `transmission`, `mileage`, `odometer_unit`, `exterior_color`, `interior_color`, `title_status`, `authenticity_profile`, `restoration_level`, `documentation_level`, `overall_condition_score`, `has_red_flags`, `ownership_notes`, `notes`, `created_at_utc`, `updated_at_utc`.

---

## 3) Reconciliation Summary

### 3.1 Overall Assessment

- `Vehicles_DDL.txt` is substantially aligned with current code mappings.
- `CREATE_Vehicles.sql` is **not aligned** with current code and DDS direction.
- Schema authority is currently split across conflicting artifacts.

### 3.2 Critical Conflicts

1. `vin` nullability conflict
   - `CREATE_Vehicles.sql`: `vin NOT NULL`
   - `Vehicles_DDL.txt`: `vin NULL`
   - Code behavior supports nullable VIN.

2. VIN uniqueness policy conflict
   - `CREATE_Vehicles.sql`: `UNIQUE KEY uq_vehicles_vin (vin)`
   - DDS Section `30`: blanket DB-level unique VIN is not recommended at this stage.

3. Missing target columns in legacy SQL artifact
   - Not present in `CREATE_Vehicles.sql` but present in code and `Vehicles_DDL.txt`:
   - `chassis_number`, `body_number`, `engine_number`, `marque`, `odometer_status`, `originality_profile`, `vehicle_status`, `row_version`.

---

## 4) Gap Matrix (Repository Artifacts)

| Column | `CREATE_Vehicles.sql` | `Vehicles_DDL.txt` | Domain/Repository | Gap | Action |
|---|---|---|---|---|---|
| `vehicle_id` | present PK | present PK | mapped | none | keep |
| `vehicle_type_id` | present nullable | present nullable FK | mapped | none | keep |
| `vin` | `NOT NULL`, unique index | nullable, non-unique index | nullable behavior in code | conflict | align to nullable + non-unique policy |
| `chassis_number` | missing | present | mapped | legacy missing | add/standardize |
| `body_number` | missing | present | mapped | legacy missing | add/standardize |
| `engine_number` | missing | present | mapped | legacy missing | add/standardize |
| `year` | present | present | mapped | none | keep |
| `make` | present | present | mapped | none | keep |
| `marque` | missing | present | mapped | legacy missing | add/standardize |
| `model` | present | present | mapped | none | keep |
| `trim` | present | present | mapped | none | keep |
| `series_name` | present | present | mapped | none | keep |
| `body_style` | present | present | mapped | none | keep |
| `drivetrain` | present | present | mapped | none | keep |
| `engine` | present | present | mapped | none | keep |
| `transmission` | present | present | mapped | none | keep |
| `mileage` | `int(11) NULL` | `int(11) NULL` | `int?` mapped | none | keep |
| `odometer_unit` | present | present | mapped | none | keep |
| `odometer_status` | missing | present | mapped | legacy missing | add/standardize |
| `exterior_color` | present | present | mapped | none | keep |
| `interior_color` | present | present | mapped | none | keep |
| `title_status` | present | present | mapped | none | keep |
| `authenticity_profile` | present | present | mapped | none | keep |
| `originality_profile` | missing | present | mapped | legacy missing | add/standardize |
| `restoration_level` | present | present | mapped | none | keep |
| `documentation_level` | present | present | mapped | none | keep |
| `overall_condition_score` | `decimal(4,2)` | `decimal(4,2)` | mapped | none | keep |
| `has_red_flags` | present | present | mapped | none | keep |
| `notes` | present | present | mapped | none | keep |
| `ownership_notes` | present | present | mapped | none | keep |
| `vehicle_status` | missing | present default `Active` | mapped | legacy missing | add/standardize |
| `row_version` | missing | present default `1` | mapped + concurrency SQL | legacy missing | add/standardize |
| `created_at_utc` | present | present | mapped | none | keep |
| `updated_at_utc` | present | present | mapped | none | keep |

---

## 5) Index and Constraint Reconciliation Findings (Phase A input to Phase C)

| Item | `CREATE_Vehicles.sql` | `Vehicles_DDL.txt` | Gap |
|---|---|---|---|
| `idx_vehicles_make` | yes | yes | none |
| `idx_vehicles_year` | yes | yes | none |
| `idx_vehicles_vehicle_type` | yes | yes | none |
| `idx_vehicles_vin` | no | yes | legacy missing |
| `idx_vehicles_chassis_number` | no | yes | legacy missing |
| `fk_vehicles_vehicle_type` | no | yes | legacy missing |
| `uq_vehicles_vin` | yes | no | policy conflict with DDS `30` |

---

## 6) Overlaps and Duplicates Affecting Schema Reconciliation

1. Multiple schema definitions exist for `vehicles` with conflicting rules.
2. Repository contains overlapping insert paths (`InsertAsync` and `CreateAsync`) that assume slightly different insert column sets.
3. Current tests and code behavior align better with `Vehicles_DDL.txt` than with `CREATE_Vehicles.sql`.

---

## 7) Phase A Decisions Required

1. **Declare schema authority**
   - Set `docs/04_Data/Schema/Vehicles_DDL.txt` as authoritative until a migration system is introduced.

2. **Retire or relabel legacy SQL**
   - Mark `docs/04_Data/SQL/CREATE_Vehicles.sql` as legacy/non-authoritative.

3. **Adopt DDS VIN policy**
   - Remove/avoid blanket unique VIN constraint at DB level until duplicate-handling workflow is fully defined.

4. **Lock baseline column set**
   - Freeze Phase A field list and confirm all layers (Domain/Application/Infrastructure/API/WinForms) reference the same names/types/nullability semantics.

---

## 8) Exit Criteria for Phase A

Phase A is complete when:

- one authoritative `vehicles` schema artifact is declared,
- legacy conflicting DDL is retired or clearly marked,
- all baseline fields are reconciled with code mappings,
- unresolved VIN nullability/uniqueness conflicts are formally decided,
- a Phase B migration backlog is generated from this report.
