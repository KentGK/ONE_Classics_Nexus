# ONE Classics Nexus AVM Math Specification
## AVM_MATH_SPEC_v1.0
### Core Engine Contract for Inspection, Tiering, Red Flags, Confidence, and Valuation

**Document Status:** Draft Lock Candidate  
**Prepared For:** ONE Classics Nexus  
**Version:** 1.0  
**Purpose:** Define deterministic AVM math rules for the ONE Classics Nexus valuation pipeline.

---

## 1. Purpose

This specification defines the mathematical contract for the ONE Classics Nexus Automated Valuation Model (AVM). It formalizes how inspection scores, vehicle-type weights, condition tiers, valuation multipliers, red flags, confidence scoring, and comparable market data are transformed into a final appraised value.

The primary goal is deterministic valuation behavior:

> Same inputs must always produce the same outputs.

---

## 2. Scope

This specification covers:

1. Canonical 12-system inspection scoring
2. Vehicle-type-specific weight profiles
3. 0–5 score normalization into 0–100 condition score
4. Weighted condition index calculation
5. Condition tier mapping
6. Tier-to-valuation multiplier logic
7. Red flag adjustment logic
8. Confidence scoring framework
9. Comparable value anchoring
10. Final valuation formula
11. Traceability output requirements
12. Edge-case handling rules

---

## 3. Canonical 12-System Inspection Model

| System # | System Name | System Code | System Key |
|---:|---|---|---|
| 1 | Body & Exterior | BODY_EXT | body_exterior |
| 2 | Frame & Structure | FRAME_STRUCT | frame_structure |
| 3 | Engine / Powertrain | POWERTRAIN | powertrain |
| 4 | Transmission & Driveline | TRANS_DRIVE | transmission_driveline |
| 5 | Suspension & Steering | SUS_STEER | suspension_steering |
| 6 | Braking System | BRAKE_SYS | braking_system |
| 7 | Electrical System | ELEC_SYS | electrical_system |
| 8 | Interior | INTERIOR | interior |
| 9 | Wheels & Tires | WHEEL_TIRE | wheels_tires |
| 10 | Trim, Glass & Details | TRIM_GLASS | trim_glass_details |
| 11 | Documentation & Provenance | DOC_PROV | documentation_provenance |
| 12 | Road Test & Functional | ROAD_FUNC | road_test_functional |

### 3.1 Canonical Rule

The system number, system code, and system key must not change between vehicle types. Vehicle-type differences must be handled by weight profile, checklist wording, scoring guidance, red flag rules, valuation modifiers, and collector value logic.

---

## 4. Vehicle Type Classification

| Vehicle Type | Description |
|---|---|
| STANDARD | Standard vehicle; reliability and safety prioritized |
| CUSTOM | Modified/custom vehicle; build quality, integration, and craftsmanship prioritized |
| CLASSIC | Vintage/Classic vehicle; authenticity, originality, provenance, and collector value prioritized |

The AVM must allow future vehicle types without changing the canonical 12-system model.

---

## 5. Score Scale

| Score | Label | General Interpretation |
|---:|---|---|
| 0 | Failed / Unsafe | Inoperable, unsafe, or materially failed |
| 1 | Poor | Major defects or high repair exposure |
| 2 | Fair | Operational but degraded; notable repair needs |
| 3 | Good | Normal condition for age/use |
| 4 | Very Good | Above average, well maintained |
| 5 | Excellent | Exceptional, near-new, show-level, or top-tier condition |

Scores must be stored as `score_0_to_5 DECIMAL(3,2)`.

---

## 6. Vehicle-Type Weight Profiles

Weights must total exactly 1.00 per vehicle type.

### 6.1 Standard Vehicle Weights

| System Code | Weight |
|---|---:|
| BODY_EXT | 0.07 |
| FRAME_STRUCT | 0.10 |
| POWERTRAIN | 0.18 |
| TRANS_DRIVE | 0.15 |
| SUS_STEER | 0.12 |
| BRAKE_SYS | 0.12 |
| ELEC_SYS | 0.08 |
| INTERIOR | 0.05 |
| WHEEL_TIRE | 0.05 |
| TRIM_GLASS | 0.03 |
| DOC_PROV | 0.02 |
| ROAD_FUNC | 0.03 |

**Total = 1.00**

### 6.2 Custom Vehicle Weights

| System Code | Weight |
|---|---:|
| BODY_EXT | 0.12 |
| FRAME_STRUCT | 0.10 |
| POWERTRAIN | 0.13 |
| TRANS_DRIVE | 0.11 |
| SUS_STEER | 0.10 |
| BRAKE_SYS | 0.08 |
| ELEC_SYS | 0.08 |
| INTERIOR | 0.10 |
| WHEEL_TIRE | 0.08 |
| TRIM_GLASS | 0.06 |
| DOC_PROV | 0.05 |
| ROAD_FUNC | 0.09 |

**Total = 1.00**

### 6.3 Vintage / Classic Vehicle Weights

| System Code | Weight |
|---|---:|
| BODY_EXT | 0.12 |
| FRAME_STRUCT | 0.10 |
| POWERTRAIN | 0.14 |
| TRANS_DRIVE | 0.12 |
| SUS_STEER | 0.10 |
| BRAKE_SYS | 0.08 |
| ELEC_SYS | 0.07 |
| INTERIOR | 0.10 |
| WHEEL_TIRE | 0.06 |
| TRIM_GLASS | 0.06 |
| DOC_PROV | 0.05 |
| ROAD_FUNC | 0.00 |

**Total = 1.00**

### 6.4 Weight Validation Rule

For each vehicle type:

```text
SUM(system_weight) = 1.00
```

If the sum does not equal 1.00, the valuation engine must reject the weight profile and return `INVALID_WEIGHT_PROFILE`.

---

## 7. Core Condition Math

### 7.1 System Weighted Value

```text
system_weighted_value = score_0_to_5 × system_weight
```

### 7.2 Total Weighted Value

```text
total_weighted_value = SUM(system_weighted_value)
```

Because all weights total 1.00 and scores range from 0 to 5:

```text
0.00 <= total_weighted_value <= 5.00
```

### 7.3 Condition Index

```text
condition_index_0_to_100 = total_weighted_value × 20
```

---

## 8. Weighted Score Range Bands

| Rubric Score | Label | Weighted Value Range | Condition Score Range |
|---:|---|---:|---:|
| 0 | Failed / Unsafe | 0.00 – 0.49 | 0 – 9 |
| 1 | Poor | 0.50 – 1.49 | 10 – 29 |
| 2 | Fair | 1.50 – 2.49 | 30 – 49 |
| 3 | Good | 2.50 – 3.49 | 50 – 69 |
| 4 | Very Good | 3.50 – 4.49 | 70 – 89 |
| 5 | Excellent | 4.50 – 5.00 | 90 – 100 |

---

## 9. Condition Tier Model

| Tier Code | Tier Name | Condition Score Range |
|---|---|---:|
| T1 | Investment / Show Tier | 90 – 100 |
| T2 | Premium Retail Tier | 80 – 89 |
| T3 | Strong Driver Tier | 70 – 79 |
| T4 | Average Driver Tier | 60 – 69 |
| T5 | Driver / Needs Attention | 50 – 59 |
| T6 | Project Vehicle Tier | 30 – 49 |
| T7 | Non-Operational / Salvage Tier | 0 – 29 |

### 9.1 Tier Logic

```text
IF condition_index >= 90 THEN T1
ELSE IF condition_index >= 80 THEN T2
ELSE IF condition_index >= 70 THEN T3
ELSE IF condition_index >= 60 THEN T4
ELSE IF condition_index >= 50 THEN T5
ELSE IF condition_index >= 30 THEN T6
ELSE T7
```

---

## 10. Tier-to-Valuation Multipliers

| Tier Code | Multiplier |
|---|---:|
| T1 | 1.20 |
| T2 | 1.10 |
| T3 | 1.00 |
| T4 | 0.90 |
| T5 | 0.75 |
| T6 | 0.55 |
| T7 | 0.30 |

```text
tier_adjusted_value = median_comparable_value × tier_multiplier
```

---

## 11. Comparable Market Anchor

Required comparable statistics:

```text
low_comparable_value
high_comparable_value
median_comparable_value
comparable_count
```

### 11.1 Comparable Count Rules

| Comparable Count | Status |
|---:|---|
| 0 | Valuation blocked |
| 1–2 | Low confidence |
| 3–5 | Medium confidence |
| 6+ | High confidence |

The primary market anchor is:

```text
base_market_value = median_comparable_value
```

If no median can be calculated, valuation status must be `INSUFFICIENT_DATA`.

---

## 12. Red Flag Adjustment Model

Red flags apply after tier adjustment.

### 12.1 Red Flag Modifier Table

| Red Flag Type | Modifier |
|---|---:|
| Engine failure | 0.70 |
| Transmission failure | 0.75 |
| Structural / frame damage | 0.60 |
| Salvage / branded title | 0.65 |
| Severe rust | 0.70 |
| Brake safety failure | 0.65 |
| Electrical fire / unsafe wiring | 0.70 |
| VIN / identity inconsistency | 0.50 |

### 12.2 Severity Modifier Table

| Severity | Modifier |
|---|---:|
| LOW | 0.95 |
| MEDIUM | 0.85 |
| HIGH | 0.70 |
| CRITICAL | 0.50 |

### 12.3 Aggregation Rule

Initial deterministic rule:

```text
red_flag_modifier_total = MIN(all_applicable_red_flag_modifiers)
```

This avoids excessive compounding while still applying the strongest material risk.

---

## 13. Confidence Score Model

Confidence score ranges from 0.00 to 1.00.

| Input | Weight |
|---|---:|
| Comparable Count Quality | 0.35 |
| Comparable Similarity Quality | 0.30 |
| Inspection Completeness | 0.20 |
| Documentation / Data Reliability | 0.15 |

### 13.1 Confidence Formula

```text
confidence_score =
(comparable_count_quality × 0.35)
+ (comparable_similarity_quality × 0.30)
+ (inspection_completeness × 0.20)
+ (documentation_reliability × 0.15)
```

### 13.2 Comparable Count Quality

| Comparable Count | Quality Score |
|---:|---:|
| 0 | 0.00 |
| 1 | 0.25 |
| 2 | 0.40 |
| 3 | 0.60 |
| 4–5 | 0.80 |
| 6+ | 1.00 |

### 13.3 Inspection Completeness

```text
inspection_completeness = completed_required_fields / total_required_fields
```

### 13.4 Confidence Modifier

| Confidence Score | Confidence Level | Modifier |
|---:|---|---:|
| 0.90 – 1.00 | High | 1.00 |
| 0.70 – 0.89 | Medium | 0.95 |
| 0.00 – 0.69 | Low | 0.85 |

---

## 14. Final Valuation Formula

```text
final_value =
median_comparable_value
× tier_multiplier
× red_flag_modifier_total
× confidence_modifier
```

Final value should be rounded to the nearest $100 for reports.

```text
reported_final_value = ROUND(final_value, nearest 100)
```

### 14.1 Value Range

Standard confidence range:

```text
value_low = reported_final_value × 0.90
value_high = reported_final_value × 1.10
```

Low-confidence range:

```text
value_low = reported_final_value × 0.85
value_high = reported_final_value × 1.15
```

---

## 15. Calculation Order

The AVM must calculate in this order:

1. Validate inputs
2. Validate weight profile total is 1.00
3. Calculate system weighted values
4. Calculate total weighted value
5. Calculate condition index
6. Determine condition tier
7. Determine tier multiplier
8. Determine median comparable value
9. Apply tier multiplier
10. Determine red flag modifier
11. Determine confidence score
12. Determine confidence modifier
13. Calculate final value
14. Calculate value range
15. Generate valuation trace lines
16. Persist immutable valuation run

---

## 16. Edge Case Rules

| Edge Case | Required Behavior |
|---|---|
| Missing required system score | Status = INCOMPLETE_INSPECTION |
| Inspection completeness < 80% | Confidence score capped at 0.69 |
| Inspection completeness < 50% | Status = INCOMPLETE_INSPECTION |
| Comparable count = 0 | Status = INSUFFICIENT_COMPARABLES |
| Score outside 0–5 | Status = INVALID_INPUT |
| Weight outside 0–1 | Status = INVALID_WEIGHT_PROFILE |
| Weight total not 1.00 | Status = INVALID_WEIGHT_PROFILE |
| Median comparable <= 0 | Status = INVALID_COMPARABLE_DATA |
| Critical red flag exists | Tier cannot exceed T6 without reviewer override |

---

## 17. Valuation Status Codes

| Status Code | Meaning |
|---|---|
| SUCCESS | Final valuation completed |
| INCOMPLETE_INSPECTION | Required inspection data missing |
| INSUFFICIENT_COMPARABLES | Not enough comparable market data |
| INVALID_INPUT | Score or other input failed validation |
| INVALID_WEIGHT_PROFILE | Weight profile invalid or does not total 1.00 |
| INVALID_COMPARABLE_DATA | Comparable data invalid |
| NEEDS_REVIEW | Valuation produced but requires reviewer attention |
| ERROR | Unexpected engine error |

---

## 18. Valuation Trace Requirements

Every valuation run must produce trace lines.

| Sort Order | Trace Key | Message |
|---:|---|---|
| 10 | BASE_MEDIAN | Median comparable value selected |
| 20 | CONDITION_SCORE | Condition index calculated |
| 30 | CONDITION_TIER | Tier assigned |
| 40 | TIER_MULTIPLIER | Tier multiplier applied |
| 50 | RED_FLAG_MODIFIER | Red flag modifier applied |
| 60 | CONFIDENCE_SCORE | Confidence score calculated |
| 70 | CONFIDENCE_MODIFIER | Confidence modifier applied |
| 80 | FINAL_VALUE | Final value calculated |
| 90 | VALUE_RANGE | Value range calculated |

Trace line fields:

```text
trace_id
valuation_run_id
sort_order
type
trace_key
amount
factor
message
evidence_ref
```

---

## 19. Database Alignment Requirements

### 19.1 condition_scores

```text
condition_score_id
inspection_id
appraisal_id
system_number
system_key
system_code
score_0_to_5
weight
weighted_value
is_red_flag
notes
created_at_utc
updated_at_utc
```

### 19.2 system_weights

```text
weight_id
vehicle_type
system_number
system_code
system_key
weight
version
is_active
created_at_utc
```

### 19.3 valuation_runs

```text
valuation_run_id
appraisal_id
inspection_id
engine_version
rubric_version
ruleset_version
condition_score
condition_tier
tier_multiplier
median_comparable_value
red_flag_modifier
confidence_score
confidence_modifier
final_value
value_low
value_high
status
created_at_utc
```

### 19.4 valuation_trace_lines

```text
trace_id
valuation_run_id
sort_order
type
trace_key
amount
factor
message
evidence_ref
created_at_utc
```

---

## 20. C# Engine Contract

```csharp
public interface IConditionEngine
{
    ConditionResult CalculateConditionIndex(InspectionScoreInput input);
}

public interface ITierEngine
{
    TierResult DetermineTier(decimal conditionIndex);
}

public interface IRedFlagEngine
{
    RedFlagResult CalculateRedFlagModifier(IEnumerable<RedFlagInput> redFlags);
}

public interface IConfidenceEngine
{
    ConfidenceResult CalculateConfidence(ConfidenceInput input);
}

public interface IValuationEngine
{
    ValuationResult Run(ValuationInput input);
}
```

No valuation engine method may depend on current system time except for persisted run timestamps, random numbers, non-versioned external data, or unordered enumeration where output may vary.

---

## 21. Golden Test Scenario

### 21.1 Input

Vehicle Type: STANDARD

| System Code | Score | Weight | Weighted Value |
|---|---:|---:|---:|
| BODY_EXT | 3 | 0.07 | 0.21 |
| FRAME_STRUCT | 4 | 0.10 | 0.40 |
| POWERTRAIN | 4 | 0.18 | 0.72 |
| TRANS_DRIVE | 3 | 0.15 | 0.45 |
| SUS_STEER | 3 | 0.12 | 0.36 |
| BRAKE_SYS | 4 | 0.12 | 0.48 |
| ELEC_SYS | 3 | 0.08 | 0.24 |
| INTERIOR | 3 | 0.05 | 0.15 |
| WHEEL_TIRE | 4 | 0.05 | 0.20 |
| TRIM_GLASS | 3 | 0.03 | 0.09 |
| DOC_PROV | 2 | 0.02 | 0.04 |
| ROAD_FUNC | 4 | 0.03 | 0.12 |

### 21.2 Expected Condition Math

```text
total_weighted_value = 3.46
condition_index = 3.46 × 20 = 69.20
tier = T4
tier_multiplier = 0.90
```

### 21.3 Comparable and Modifier Inputs

```text
median_comparable_value = 25000
red_flag_modifier_total = 1.00
confidence_score = 0.82
confidence_modifier = 0.95
```

### 21.4 Expected Valuation

```text
final_value = 25000 × 0.90 × 1.00 × 0.95 = 21375
reported_final_value = 21400
value_low = 19300
value_high = 23500
```

---

## 22. Acceptance Criteria

The AVM math implementation is acceptable when:

1. All weight profiles total 1.00.
2. All scores are validated within 0–5.
3. Condition index matches formula exactly.
4. Tier assignment matches tier table exactly.
5. Tier multiplier matches tier table exactly.
6. Red flag modifier follows deterministic aggregation rule.
7. Confidence score follows weighted formula.
8. Final value follows required formula.
9. Trace lines are generated for all major calculation steps.
10. Golden test scenario passes exactly within configured rounding tolerance.

---

## 23. Versioning

Valuation runs must store:

```text
engine_version
rubric_version
ruleset_version
```

Once persisted, a valuation run must not be modified. Corrections require a new valuation run.

---

## 24. Open Items for v1.1

1. Segment-specific comparable similarity models
2. Market momentum layer
3. Market liquidity layer
4. Cost-to-cure adjustment layer
5. Retail readiness adjustment layer
6. ML-assisted valuation layer
7. Reviewer override workflow
8. Dynamic weighting by vehicle subcategory

---

## 25. Final Lock Statement

This document establishes the AVM mathematical source of truth for ONE Classics Nexus v1.0. All API, database, C# engine, test, and report generation work must align to this specification unless formally revised through versioned change control.

---

# End of Document
