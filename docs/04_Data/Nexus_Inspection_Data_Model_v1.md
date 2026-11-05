# ONE CLASSICS NEXUS  
## INSPECTION DATA MODEL SPECIFICATION  
### Version 1.0 — Derived from SRS v2.0 and AVM Math Spec v1.0

---

# 1. PURPOSE

This document defines the **Inspection Data Model Specification** for the ONE Classics Nexus platform.  
It serves as the authoritative data contract for:

- Inspection capture
- Condition scoring
- Valuation input (AVM)
- Reporting

This specification is derived from:
- SRS v2.0 fileciteturn3file1
- AVM Math Specification v1.0 fileciteturn3file0

---

# 2. DESIGN PRINCIPLES

- Canonical 12-System Model (non-negotiable)
- Vehicle-type flexibility via weights/configuration
- Deterministic scoring inputs for AVM
- Version-controlled inspection schema
- Database-first alignment

---

# 3. CORE ENTITY: INSPECTION

## 3.1 inspections

| Column | Type | Description |
|--------|------|------------|
| inspection_id | CHAR(36) PK | Unique identifier |
| appraisal_id | CHAR(36) FK | Linked appraisal |
| vehicle_type | VARCHAR | STANDARD / CUSTOM / CLASSIC |
| status | VARCHAR | Draft / Complete |
| created_at_utc | DATETIME | Created timestamp |
| updated_at_utc | DATETIME | Updated timestamp |

---

# 4. CONDITION SCORES (CORE TABLE)

## 4.1 condition_scores

| Column | Type | Description |
|--------|------|------------|
| condition_score_id | CHAR(36) PK | Unique row ID |
| inspection_id | CHAR(36) FK | Parent inspection |
| appraisal_id | CHAR(36) FK | Linked appraisal |
| system_number | INT | 1–12 |
| system_code | VARCHAR | e.g. POWERTRAIN |
| system_key | VARCHAR | e.g. powertrain |
| score_0_to_5 | DECIMAL(3,2) | Score input |
| weight | DECIMAL(5,4) | Applied weight |
| weighted_value | DECIMAL(6,4) | score × weight |
| is_red_flag | BOOLEAN | Red flag indicator |
| notes | TEXT | Inspector notes |
| created_at_utc | DATETIME | Timestamp |
| updated_at_utc | DATETIME | Timestamp |

---

# 5. CANONICAL 12-SYSTEM MODEL

| # | Code | Key |
|---|------|------|
| 1 | BODY_EXT | body_exterior |
| 2 | FRAME_STRUCT | frame_structure |
| 3 | POWERTRAIN | powertrain |
| 4 | TRANS_DRIVE | transmission_driveline |
| 5 | SUS_STEER | suspension_steering |
| 6 | BRAKE_SYS | braking_system |
| 7 | ELEC_SYS | electrical_system |
| 8 | INTERIOR | interior |
| 9 | WHEEL_TIRE | wheels_tires |
|10 | TRIM_GLASS | trim_glass_details |
|11 | DOC_PROV | documentation_provenance |
|12 | ROAD_FUNC | road_test_functional |

---

# 6. SYSTEM WEIGHTS

## 6.1 system_weights

| Column | Type | Description |
|--------|------|------------|
| weight_id | CHAR(36) PK |
| vehicle_type | VARCHAR |
| system_number | INT |
| system_code | VARCHAR |
| system_key | VARCHAR |
| weight | DECIMAL(5,4) |
| version | INT |
| is_active | BOOLEAN |
| created_at_utc | DATETIME |

---

# 7. INSPECTION TEMPLATE MODEL

## 7.1 inspection_templates

| Column | Type |
|--------|------|
| template_id | CHAR(36) |
| vehicle_type | VARCHAR |
| version | INT |
| is_active | BOOLEAN |

## 7.2 inspection_template_items

| Column | Type |
|--------|------|
| item_id | CHAR(36) |
| template_id | CHAR(36) |
| system_code | VARCHAR |
| item_name | VARCHAR |
| description | TEXT |

---

# 8. RED FLAG MODEL

## 8.1 red_flag_rules

| Column | Type |
|--------|------|
| rule_id | CHAR(36) |
| system_code | VARCHAR |
| severity | VARCHAR |
| description | TEXT |
| modifier | DECIMAL(3,2) |

---

# 9. DERIVED FIELDS (NOT STORED)

Derived at runtime:

- total_weighted_value
- condition_index
- condition_tier

Formula (from AVM Spec):

```
condition_index = SUM(score × weight) × 20
```

---

# 10. VALIDATION RULES

- Score must be between 0 and 5
- All 12 systems must exist
- Weights must total 1.00
- No null required system scores

---

# 11. RELATIONSHIPS

- inspections → condition_scores (1:N)
- inspections → appraisal (N:1)
- system_weights → condition_scores (logical)
- inspection_templates → inspection_template_items (1:N)

---

# 12. TRACEABILITY

| Source | Mapping |
|--------|--------|
| SRS FR-INS | inspections |
| SRS FR-SCR | condition_scores |
| SRS FR-WGT | system_weights |
| AVM Section 7 | condition math |
| AVM Section 12 | red flags |

---

# 13. FINAL NOTES

This model is:

- Fully aligned with AVM math engine
- Normalized for scalability
- Deterministic for valuation integrity
- Extensible for future vehicle types

---

# END OF DOCUMENT
