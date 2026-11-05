# AVM_Math_Specification_v2.0.md
## ONE Classics Nexus – AVM Math Specification
### Core Engine Contract for Inspection, Condition, Similarity, Tiering, Red Flags, Collector Value, Confidence, Traceability, and Valuation

**Document Status:** Reconciled Engineering Lock Candidate  
**Prepared For:** ONE Classics Nexus  
**Version:** 2.0 Reconciled  
**Merged From:** `Nexus_AVM_Math_Spec_v1.0.md` + `AVM_Math_Specification_v3.0.md`  
**Primary Owners:** Tech Analyst (TA), Business Analyst (BA), Database Architect (DBA)  
**Target Folder:** `/docs/06_Engine/`  
**Purpose:** Define deterministic AVM math rules for the ONE Classics Nexus valuation pipeline.

---

# 1. Purpose

This specification defines the mathematical contract for the ONE Classics Nexus Automated Valuation Model (AVM). It formalizes how inspection scores, vehicle-type weights, condition tiers, valuation multipliers, red flags, confidence scoring, comparable market data, similarity weighting, and collector value modifiers are transformed into a final appraised value.

This reconciled version restores the execution detail from v1.0 and incorporates the enhanced formulas from v3.0, including:

- Normalized condition formulas
- Similarity-weighted comparable valuation
- Critical-system floor rule
- Collector Value Layer
- Mandatory traceability
- Deterministic output contract

The primary goal is deterministic valuation behavior:

> Same inputs must always produce the same outputs.

---

# 2. Scope

This specification covers:

1. Canonical 12-system inspection scoring
2. Vehicle-type classification
3. Vehicle-type-specific weight profiles
4. 0–5 score normalization into a 0–100 condition score
5. Weighted condition index calculation
6. Critical system floor logic
7. Condition tier mapping
8. Tier-to-valuation multiplier logic
9. Comparable market anchoring
10. Similarity-weighted comparable valuation
11. Red flag adjustment logic
12. Collector Value Layer
13. Confidence scoring framework
14. Final valuation formula
15. Value range calculation
16. Traceability output requirements
17. Edge-case handling rules
18. Database alignment requirements
19. C# engine contracts
20. Golden test scenarios
21. Acceptance criteria
22. Versioning and immutability rules

---

# 3. Design Principles

| Principle | Description |
|---|---|
| Deterministic | Same inputs must always produce the same outputs. |
| Inspection-first | The inspection and 12-system condition model are core AVM inputs. |
| Explainable | Every valuation run must produce valuation trace lines. |
| DB-aligned | Math inputs and outputs must align with locked database structures. |
| Versioned | Engine, rubric, and ruleset versions must be stored with each valuation run. |
| Extensible | Future vehicle types and weighting profiles must be supported without changing the canonical 12-system mapping. |
| Reviewer-ready | High-risk or insufficient-data valuations must return status codes that support review workflows. |

---

# 4. Core AVM Pipeline

The AVM pipeline is locked as:

```text
Vehicle + Appraisal
→ Inspection
→ Condition Engine
→ Critical System Floor Rule
→ Condition Tier Engine
→ Comparable Engine
→ Similarity Engine
→ Red Flag Engine
→ Collector Value Layer
→ Confidence Engine
→ Valuation Engine
→ Trace Engine
→ Persisted Valuation Run
→ Report Output
```

The engine may be implemented internally as separate components, but the output must be traceable to the calculation order defined in this specification.

---

# 5. Canonical 12-System Inspection Model

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

## 5.1 Canonical Rule

The system number, system code, and system key must not change between vehicle types.

Vehicle-type differences must be handled by:

- Weight profile
- Checklist wording
- Scoring guidance
- Red flag rules
- Comparable weighting
- Valuation modifiers
- Collector Value Layer behavior

The canonical 12-system map is the shared inspection and AVM backbone.

---

# 6. Vehicle Type Classification

| Vehicle Type | Description | Valuation Emphasis |
|---|---|---|
| STANDARD | Standard vehicle; reliability and safety prioritized | Reliability, safety, drivability, repair exposure |
| CUSTOM | Modified/custom vehicle; build quality, integration, and craftsmanship prioritized | Build quality, design intent, custom integration, workmanship |
| CLASSIC | Vintage/Classic vehicle; authenticity, originality, provenance, and collector value prioritized | Originality, provenance, market desirability, authenticity |

The AVM must allow future vehicle types without changing the canonical 12-system model.

---

# 7. Score Scale

Scores must be stored as `score_0_to_5 DECIMAL(3,2)`.

| Score | Label | General Interpretation |
|---:|---|---|
| 0 | Failed / Unsafe | Inoperable, unsafe, or materially failed |
| 1 | Poor | Major defects or high repair exposure |
| 2 | Fair | Operational but degraded; notable repair needs |
| 3 | Good | Normal condition for age/use |
| 4 | Very Good | Above average, well maintained |
| 5 | Excellent | Exceptional, near-new, show-level, or top-tier condition |

## 7.1 Score Validation

```text
0.00 <= score_0_to_5 <= 5.00
```

If any score falls outside this range:

```text
status = INVALID_INPUT
```

---

# 8. Vehicle-Type Weight Profiles

Weights must total exactly 1.00 per vehicle type.

```text
SUM(system_weight) = 1.00
```

If the sum does not equal 1.00:

```text
status = INVALID_WEIGHT_PROFILE
```

## 8.1 Standard Vehicle Weights — Locked Reliability-First Profile

| System # | System Code | Weight | Rationale |
|---:|---|---:|---|
| 1 | BODY_EXT | 0.06 | Cosmetic; limited direct drivability impact |
| 2 | FRAME_STRUCT | 0.10 | Structural integrity and safety |
| 3 | POWERTRAIN | 0.18 | Primary reliability driver |
| 4 | TRANS_DRIVE | 0.14 | Critical operational reliability |
| 5 | SUS_STEER | 0.10 | Stability, handling, safe drivability |
| 6 | BRAKE_SYS | 0.10 | Core safety system |
| 7 | ELEC_SYS | 0.10 | Failure-prone; affects operability |
| 8 | INTERIOR | 0.04 | Lower reliability impact |
| 9 | WHEEL_TIRE | 0.06 | Roadworthiness and safety |
| 10 | TRIM_GLASS | 0.02 | Primarily cosmetic/supporting |
| 11 | DOC_PROV | 0.02 | Supporting documentation value |
| 12 | ROAD_FUNC | 0.08 | Real-world functional validation |

**Total = 1.00**

## 8.2 Custom Vehicle Weights

| System # | System Code | Weight | Rationale |
|---:|---|---:|---|
| 1 | BODY_EXT | 0.12 | Paint/body quality and visual integration |
| 2 | FRAME_STRUCT | 0.10 | Structural safety and build foundation |
| 3 | POWERTRAIN | 0.13 | Performance and mechanical quality |
| 4 | TRANS_DRIVE | 0.11 | Driveline integration and usability |
| 5 | SUS_STEER | 0.10 | Ride quality and handling |
| 6 | BRAKE_SYS | 0.08 | Safety and performance |
| 7 | ELEC_SYS | 0.08 | Custom wiring and electronics reliability |
| 8 | INTERIOR | 0.10 | Craftsmanship and finished appearance |
| 9 | WHEEL_TIRE | 0.08 | Fitment, stance, and roadworthiness |
| 10 | TRIM_GLASS | 0.06 | Finish details |
| 11 | DOC_PROV | 0.05 | Builder documentation and provenance |
| 12 | ROAD_FUNC | 0.09 | Functional integration |

**Total = 1.00**

## 8.3 Classic Vehicle Weights

| System # | System Code | Weight | Rationale |
|---:|---|---:|---|
| 1 | BODY_EXT | 0.12 | Body quality and preservation |
| 2 | FRAME_STRUCT | 0.10 | Structural authenticity and safety |
| 3 | POWERTRAIN | 0.14 | Mechanical originality and function |
| 4 | TRANS_DRIVE | 0.12 | Driveline originality and operation |
| 5 | SUS_STEER | 0.10 | Originality and safe operation |
| 6 | BRAKE_SYS | 0.08 | Safety and operational status |
| 7 | ELEC_SYS | 0.07 | Correctness and reliability |
| 8 | INTERIOR | 0.10 | Preservation, correctness, and trim |
| 9 | WHEEL_TIRE | 0.06 | Correct fitment and condition |
| 10 | TRIM_GLASS | 0.06 | Detail authenticity |
| 11 | DOC_PROV | 0.05 | Provenance and documentation |
| 12 | ROAD_FUNC | 0.00 | Optional for non-road-tested collector vehicles |

**Total = 1.00**

## 8.4 Weight Validation Rule

For each vehicle type:

```text
SUM(system_weight) = 1.00
```

Tolerance:

```text
ABS(SUM(system_weight) - 1.00) <= 0.0001
```

If the profile fails validation:

```text
status = INVALID_WEIGHT_PROFILE
```

---

# 9. Core Condition Math

This reconciled spec preserves the v1.0 weighted-value formula and explicitly maps it to the v3.0 normalized formula.

## 9.1 v1.0 Weighted Value Formula

For each system:

```text
system_weighted_value = score_0_to_5 × system_weight
```

Total weighted value:

```text
total_weighted_value = SUM(system_weighted_value)
```

Because scores range from 0 to 5 and weights total 1.00:

```text
0.00 <= total_weighted_value <= 5.00
```

Condition index:

```text
condition_index_0_to_100 = total_weighted_value × 20
```

## 9.2 v3.0 Normalized Formula

For each system:

```text
S_i = score_0_to_5_i / 5
```

Then:

```text
C = SUM(S_i × W_i)
```

Condition index:

```text
ConditionIndex = C × 100
```

## 9.3 Formula Reconciliation

The v1.0 and v3.0 formulas are mathematically equivalent:

```text
SUM(score_i × weight_i) × 20
=
SUM((score_i / 5) × weight_i) × 100
```

Therefore, the engine may use either implementation internally, but must produce the same rounded result.

## 9.4 Required Condition Trace Detail

For each system, trace must include:

```text
system_code
system_key
score_0_to_5
weight
weighted_value
normalized_score
weighted_normalized_contribution
```

---

# 10. Critical System Floor Rule

The v3.0 critical-system floor rule is added as a mandatory post-condition guard.

Critical systems:

| System Code | Reason |
|---|---|
| POWERTRAIN | Primary operational reliability |
| TRANS_DRIVE | Driveline usability and failure exposure |
| BRAKE_SYS | Core safety |
| ELEC_SYS | Operability and fire/electrical risk |
| ROAD_FUNC | Real-world functional validation |

## 10.1 Floor Rule

If any critical system has:

```text
score_0_to_5 <= 1.00
```

Then:

```text
condition_index_after_floor = MIN(condition_index_0_to_100, 40.00)
```

## 10.2 Trace Requirement

If the floor rule is triggered, emit:

```text
trace_key = CRITICAL_SYSTEM_FLOOR
message = Critical system score <= 1.00 capped condition index at 40.00
```

---

# 11. Weighted Score Range Bands

| Rubric Score | Label | Weighted Value Range | Condition Score Range |
|---:|---|---:|---:|
| 0 | Failed / Unsafe | 0.00 – 0.49 | 0 – 9 |
| 1 | Poor | 0.50 – 1.49 | 10 – 29 |
| 2 | Fair | 1.50 – 2.49 | 30 – 49 |
| 3 | Good | 2.50 – 3.49 | 50 – 69 |
| 4 | Very Good | 3.50 – 4.49 | 70 – 89 |
| 5 | Excellent | 4.50 – 5.00 | 90 – 100 |

---

# 12. Condition Tier Model

| Tier Code | Tier Name | Condition Score Range |
|---|---|---:|
| T1 | Investment / Show Tier | 90 – 100 |
| T2 | Premium Retail Tier | 80 – 89 |
| T3 | Strong Driver Tier | 70 – 79 |
| T4 | Average Driver Tier | 60 – 69 |
| T5 | Driver / Needs Attention | 50 – 59 |
| T6 | Project Vehicle Tier | 30 – 49 |
| T7 | Non-Operational / Salvage Tier | 0 – 29 |

## 12.1 Tier Logic

```text
IF condition_index >= 90 THEN T1
ELSE IF condition_index >= 80 THEN T2
ELSE IF condition_index >= 70 THEN T3
ELSE IF condition_index >= 60 THEN T4
ELSE IF condition_index >= 50 THEN T5
ELSE IF condition_index >= 30 THEN T6
ELSE T7
```

## 12.2 Critical Red Flag Tier Cap

If a critical red flag exists:

```text
tier cannot exceed T6 unless reviewer override is applied
```

Reviewer override must be traceable and must not modify the original valuation run. A new valuation run must be created.

---

# 13. Tier-to-Valuation Multipliers

| Tier Code | Multiplier |
|---|---:|
| T1 | 1.20 |
| T2 | 1.10 |
| T3 | 1.00 |
| T4 | 0.90 |
| T5 | 0.75 |
| T6 | 0.55 |
| T7 | 0.30 |

Tier-adjusted value:

```text
tier_adjusted_value = base_market_value × tier_multiplier
```

---

# 14. Comparable Market Anchor

Required comparable statistics:

```text
low_comparable_value
high_comparable_value
median_comparable_value
average_comparable_value
comparable_count
```

## 14.1 Comparable Count Rules

| Comparable Count | Status / Confidence Meaning |
|---:|---|
| 0 | Valuation blocked |
| 1–2 | Low confidence |
| 3–5 | Medium confidence |
| 6+ | High confidence |

If no median can be calculated:

```text
status = INSUFFICIENT_DATA
```

or, if comparable rows exist but values are invalid:

```text
status = INVALID_COMPARABLE_DATA
```

## 14.2 Median Comparable Fallback

The v1.0 primary market anchor remains:

```text
base_market_value = median_comparable_value
```

This remains the fallback when the similarity engine is unavailable or when similarity weighting cannot be calculated reliably.

---

# 15. Similarity Engine

The v3.0 similarity model is added as the preferred comparable weighting method.

## 15.1 Similarity Inputs

The similarity engine may evaluate:

| Factor | Description |
|---|---|
| Year similarity | Closeness of model year or generation |
| Make/model match | Exact or near-exact model relevance |
| Trim/body style | Comparable body configuration |
| Drivetrain | 2WD/4WD/AWD, drivetrain relevance |
| Engine/transmission | Powertrain comparability |
| Mileage | Usage similarity |
| Condition | Comparable condition vs subject condition |
| Restoration level | Original/restored/custom/project state |
| Documentation | Provenance and evidence quality |
| Sale recency | Date relevance |
| Market source quality | Auction, dealer, verified sale, listing quality |

## 15.2 Default Similarity Factor Weights

| Factor | Weight |
|---|---:|
| Year / generation | 0.15 |
| Make/model/body relevance | 0.20 |
| Drivetrain | 0.10 |
| Engine/transmission | 0.15 |
| Condition | 0.20 |
| Restoration/authenticity | 0.10 |
| Sale recency/source quality | 0.10 |

**Total = 1.00**

## 15.3 Similarity Formula

```text
similarity_score =
SUM(factor_score_i × factor_weight_i)
```

Where:

```text
0.00 <= factor_score_i <= 1.00
0.00 <= similarity_score <= 1.00
```

## 15.4 Similarity Rejection Threshold

Reject comparable if:

```text
similarity_score < 0.50
```

Rejected comparables must be traceable, but excluded from the weighted comparable value.

## 15.5 Similarity-Weighted Comparable Formula

For accepted comparable sales:

```text
comparable_weight_c = similarity_score_c / SUM(similarity_score_all_accepted)
```

```text
similarity_weighted_base_value =
SUM(sale_price_c × comparable_weight_c)
```

## 15.6 Comparable Anchor Selection Rule

Use this deterministic selection rule:

```text
IF accepted_similarity_weighted_comparable_count >= 3
    base_market_value = similarity_weighted_base_value
ELSE IF median_comparable_value > 0
    base_market_value = median_comparable_value
    status may be NEEDS_REVIEW or LOW_CONFIDENCE depending on confidence score
ELSE
    status = INSUFFICIENT_COMPARABLES
```

## 15.7 Trace Requirements

Trace must capture:

```text
comparable_id
sale_price
similarity_score
accepted_or_rejected
comparable_weight
weighted_contribution
rejection_reason
```

---

# 16. Red Flag Adjustment Model

Red flags apply after the base market value and condition/tier calculations.

## 16.1 Red Flag Modifier Table

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

## 16.2 Severity Modifier Table

| Severity | Modifier | Equivalent Penalty |
|---|---:|---:|
| LOW | 0.95 | 5% |
| MEDIUM | 0.85 | 15% |
| HIGH | 0.70 | 30% |
| CRITICAL | 0.50 | 50% |

## 16.3 v3.0 Multiplicative Penalty Formula

For red flag penalties expressed as penalty percentages:

```text
red_flag_factor_product = PRODUCT(1 - penalty_i)
```

## 16.4 Reconciled Red Flag Aggregation Rule

To avoid excessive compounding while preserving deterministic risk impact, the locked rule is:

```text
red_flag_modifier_total = MIN(all_applicable_red_flag_type_modifiers, all_applicable_severity_modifiers)
```

Use multiplicative penalty detail only for secondary trace analysis unless a future ruleset version explicitly enables compounding.

## 16.5 Red Flag Tier Cap

If any CRITICAL red flag exists:

```text
condition_tier_max = T6
status = NEEDS_REVIEW
```

unless formally overridden by reviewer workflow.

## 16.6 Trace Requirements

For each red flag, trace must include:

```text
red_flag_code
red_flag_type
severity
type_modifier
severity_modifier
selected_modifier
is_tier_cap_triggered
message
```

---

# 17. Collector Value Layer

The Collector Value Layer is added after condition/tier adjustment and before final confidence reporting. It does not replace the 12-system condition model.

## 17.1 Collector Inputs

| Input | Range | Description |
|---|---:|---|
| rarity_score | 0.00–1.00 | Scarcity, limited production, unusual trim, one-off build |
| originality_score | 0.00–1.00 | OEM correctness or build-intent authenticity |
| provenance_score | 0.00–1.00 | Documentation, ownership history, builder reputation |
| desirability_score | 0.00–1.00 | Market demand and buyer preference |

## 17.2 Collector Index Formula

```text
collector_index =
(rarity_score + originality_score + provenance_score + desirability_score) / 4
```

## 17.3 Vehicle-Type Collector Influence

| Vehicle Type | Collector Influence | Type Multiplier |
|---|---|---:|
| STANDARD | Low | 0.05 |
| CUSTOM | Moderate | 0.15 |
| CLASSIC | High | 0.30 |

## 17.4 Collector Adjustment Formula

```text
collector_adjustment =
1 + (collector_index × vehicle_type_collector_multiplier)
```

```text
collector_adjusted_value =
pre_collector_value × collector_adjustment
```

## 17.5 Collector Layer Interpretation by Vehicle Type

### STANDARD

- Rarity applies only to unusual trims, limited editions, or unusually desirable configurations.
- Originality means OEM correctness.
- Provenance is usually minor unless unusually well documented.
- Desirability is used as a market sanity adjustment.

### CUSTOM

- Rarity means one-off or limited custom build.
- Originality is reinterpreted as build authenticity and design intent.
- Provenance includes builder reputation, show history, magazine features, or documented build receipts.
- Desirability reflects style, segment demand, craftsmanship, and market acceptance.

### CLASSIC

- Rarity, originality, provenance, and desirability are high-impact valuation drivers.
- Authenticity and historical significance may materially increase value.
- Documentation and provenance may influence both final value and confidence.

## 17.6 Collector Trace Requirements

Trace must include:

```text
rarity_score
originality_score
provenance_score
desirability_score
collector_index
vehicle_type_collector_multiplier
collector_adjustment
collector_value_delta
```

---

# 18. Confidence Score Model

Confidence score ranges from 0.00 to 1.00.

## 18.1 v1.0 Confidence Formula

```text
confidence_score =
(comparable_count_quality × 0.35)
+ (comparable_similarity_quality × 0.30)
+ (inspection_completeness × 0.20)
+ (documentation_reliability × 0.15)
```

## 18.2 Confidence Inputs

| Input | Weight |
|---|---:|
| Comparable Count Quality | 0.35 |
| Comparable Similarity Quality | 0.30 |
| Inspection Completeness | 0.20 |
| Documentation / Data Reliability | 0.15 |

## 18.3 Comparable Count Quality

| Comparable Count | Quality Score |
|---:|---:|
| 0 | 0.00 |
| 1 | 0.25 |
| 2 | 0.40 |
| 3 | 0.60 |
| 4–5 | 0.80 |
| 6+ | 1.00 |

## 18.4 Comparable Similarity Quality

Default calculation:

```text
comparable_similarity_quality = average_similarity_score_of_accepted_comparables
```

If similarity engine is unavailable:

```text
comparable_similarity_quality = 0.50
```

and trace must show:

```text
message = Similarity engine unavailable; default similarity quality applied
```

## 18.5 Inspection Completeness

```text
inspection_completeness =
completed_required_fields / total_required_fields
```

If inspection completeness is less than 80%:

```text
confidence_score = MIN(confidence_score, 0.69)
```

If inspection completeness is less than 50%:

```text
status = INCOMPLETE_INSPECTION
```

## 18.6 Documentation Reliability

Documentation reliability may consider:

| Score | Interpretation |
|---:|---|
| 1.00 | Strong documentation, verified records, clear provenance |
| 0.75 | Good documentation, minor gaps |
| 0.50 | Basic documentation only |
| 0.25 | Minimal or questionable documentation |
| 0.00 | No reliable documentation |

## 18.7 Confidence Modifier

| Confidence Score | Confidence Level | Modifier |
|---:|---|---:|
| 0.90 – 1.00 | High | 1.00 |
| 0.70 – 0.89 | Medium | 0.95 |
| 0.00 – 0.69 | Low | 0.85 |

## 18.8 v3.0 Simplified Formula Compatibility

The v3.0 simplified formula:

```text
Confidence = (Comp × 0.4) + (Sim × 0.3) + (Inspect × 0.3)
```

is retained only as a simplified conceptual expression. The engineering implementation must use the v1.0 four-factor formula in Section 18.1 because it includes documentation reliability and aligns better to appraisal traceability.

---

# 19. Final Valuation Formula

This reconciled spec retains the v1.0 tier-based valuation contract and adds v3.0 optional similarity and collector layers.

## 19.1 Base Formula

```text
final_value =
base_market_value
× tier_multiplier
× red_flag_modifier_total
× collector_adjustment
× confidence_modifier
```

Where:

```text
base_market_value =
similarity_weighted_base_value when accepted similarity-weighted comps >= 3
otherwise median_comparable_value
```

## 19.2 Important Reconciliation Note

The v3.0 streamlined formula included:

```text
Value_cond = BaseValue × (ConditionIndex / 100)
```

The v1.0 formula already applies condition through:

```text
condition_tier → tier_multiplier
```

To avoid double-counting condition, the locked v3.0 reconciled implementation uses the v1.0 tier multiplier as the primary condition valuation adjustment. The raw `ConditionIndex` remains a stored and traced output, and it determines the condition tier.

Therefore:

```text
DO NOT multiply final value by both tier_multiplier and ConditionIndex / 100
```

unless a future ruleset explicitly introduces a calibrated condition-delta factor.

## 19.3 Reported Final Value

Final value should be rounded to the nearest $100 for reports.

```text
reported_final_value = ROUND(final_value, nearest 100)
```

Database may store both unrounded and rounded values if supported.

---

# 20. Value Range Calculation

## 20.1 Standard Confidence Range

For medium or high confidence valuations:

```text
value_low = reported_final_value × 0.90
value_high = reported_final_value × 1.10
```

## 20.2 Low Confidence Range

For low-confidence valuations:

```text
value_low = reported_final_value × 0.85
value_high = reported_final_value × 1.15
```

## 20.3 Rounding

Report range values should be rounded to the nearest $100:

```text
reported_value_low = ROUND(value_low, nearest 100)
reported_value_high = ROUND(value_high, nearest 100)
```

---

# 21. Calculation Order

The AVM must calculate in this order:

1. Validate input envelope.
2. Validate vehicle type.
3. Validate canonical 12-system score set.
4. Validate all score values are within 0–5.
5. Validate weight profile total is 1.00.
6. Calculate system weighted values.
7. Calculate total weighted value.
8. Calculate condition index.
9. Apply critical system floor rule.
10. Determine condition tier.
11. Apply critical red flag tier cap if required.
12. Determine tier multiplier.
13. Validate comparable data.
14. Calculate comparable statistics.
15. Calculate similarity scores if similarity engine data is available.
16. Reject low-similarity comps.
17. Select base market value using similarity-weighted value or median fallback.
18. Determine red flag modifier.
19. Calculate collector index.
20. Calculate collector adjustment.
21. Determine confidence score.
22. Determine confidence modifier.
23. Calculate final value.
24. Round reported final value.
25. Calculate value range.
26. Generate valuation trace lines.
27. Persist immutable valuation run.

---

# 22. Edge Case Rules

| Edge Case | Required Behavior |
|---|---|
| Missing required system score | Status = INCOMPLETE_INSPECTION |
| Fewer than 12 condition score systems | Status = INCOMPLETE_INSPECTION |
| Duplicate system number/key | Status = INVALID_INPUT |
| Inspection completeness < 80% | Confidence score capped at 0.69 |
| Inspection completeness < 50% | Status = INCOMPLETE_INSPECTION |
| Comparable count = 0 | Status = INSUFFICIENT_COMPARABLES |
| Accepted similarity comp count = 0 | Fallback to median if valid; otherwise INSUFFICIENT_COMPARABLES |
| Score outside 0–5 | Status = INVALID_INPUT |
| Weight outside 0–1 | Status = INVALID_WEIGHT_PROFILE |
| Weight total not 1.00 | Status = INVALID_WEIGHT_PROFILE |
| Median comparable <= 0 | Status = INVALID_COMPARABLE_DATA |
| Critical red flag exists | Tier cannot exceed T6 without reviewer override |
| Critical system score <= 1.00 | Condition index capped at 40 |
| Collector input missing | Default collector scores to 0.00 and trace missing layer |
| Confidence score below 0.70 | Use low-confidence value range |
| Unexpected engine exception | Status = ERROR |

---

# 23. Valuation Status Codes

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

## 23.1 Needs Review Triggers

The valuation may complete with `NEEDS_REVIEW` when:

- Critical red flag exists
- Comparable count is low
- Similarity fallback was required
- Collector adjustment is unusually high
- Documentation reliability is low
- Manual reviewer override is needed

---

# 24. Valuation Trace Requirements

Every valuation run must produce trace lines.

## 24.1 Minimum Trace Lines

| Sort Order | Trace Key | Message |
|---:|---|---|
| 10 | INPUT_VALIDATION | Input validation completed |
| 20 | WEIGHT_PROFILE | Weight profile validated |
| 30 | SYSTEM_WEIGHTED_VALUES | System weighted values calculated |
| 40 | CONDITION_SCORE | Condition index calculated |
| 50 | CRITICAL_SYSTEM_FLOOR | Critical system floor evaluated |
| 60 | CONDITION_TIER | Tier assigned |
| 70 | TIER_MULTIPLIER | Tier multiplier applied |
| 80 | COMPARABLE_STATS | Comparable statistics calculated |
| 90 | SIMILARITY_SCORING | Comparable similarity scores calculated |
| 100 | BASE_MARKET_VALUE | Base market value selected |
| 110 | RED_FLAG_MODIFIER | Red flag modifier applied |
| 120 | COLLECTOR_VALUE_LAYER | Collector adjustment applied |
| 130 | CONFIDENCE_SCORE | Confidence score calculated |
| 140 | CONFIDENCE_MODIFIER | Confidence modifier applied |
| 150 | FINAL_VALUE | Final value calculated |
| 160 | VALUE_RANGE | Value range calculated |
| 170 | STATUS | Final valuation status assigned |

## 24.2 Trace Line Fields

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

## 24.3 Trace Types

Suggested trace `type` values:

```text
VALIDATION
CONDITION
TIER
COMPARABLE
SIMILARITY
RED_FLAG
COLLECTOR
CONFIDENCE
VALUATION
STATUS
ERROR
```

## 24.4 Trace Immutability

Trace lines must not be edited after valuation persistence. Corrections require a new valuation run.

---

# 25. Database Alignment Requirements

This section defines the logical fields expected by the engine. Physical schema may split these across core tables, lookup tables, or views, but repository outputs must provide this shape to the engine.

## 25.1 condition_scores

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

Minimum locked implementation fields:

```text
condition_score_id
inspection_id
system_number
system_key
score_0_to_5
weight
is_red_flag
notes
created_at_utc
updated_at_utc
```

## 25.2 system_weights

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

## 25.3 valuation_runs

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
base_market_value
median_comparable_value
similarity_weighted_base_value
red_flag_modifier
collector_index
collector_adjustment
confidence_score
confidence_modifier
final_value
reported_final_value
value_low
value_high
status
created_at_utc
```

## 25.4 valuation_trace_lines

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

## 25.5 comparable_sales Inputs

```text
comparable_id
appraisal_id
year
make
model
trim
body_style
engine
transmission
drivetrain
sale_price
sale_date
source
source_listing_url
mileage
condition_score
restoration_level
originality_level
documentation_level
title_status
notes
```

---

# 26. C# Engine Contract

## 26.1 Required Interfaces

```csharp
public interface IConditionEngine
{
    ConditionResult CalculateConditionIndex(InspectionScoreInput input);
}

public interface ITierEngine
{
    TierResult DetermineTier(decimal conditionIndex, IEnumerable<RedFlagInput> redFlags);
}

public interface IComparableEngine
{
    ComparableResult CalculateComparableAnchor(ComparableInput input);
}

public interface ISimilarityEngine
{
    SimilarityResult CalculateSimilarity(SimilarityInput input);
}

public interface IRedFlagEngine
{
    RedFlagResult CalculateRedFlagModifier(IEnumerable<RedFlagInput> redFlags);
}

public interface ICollectorValueEngine
{
    CollectorValueResult CalculateCollectorAdjustment(CollectorValueInput input);
}

public interface IConfidenceEngine
{
    ConfidenceResult CalculateConfidence(ConfidenceInput input);
}

public interface IValuationEngine
{
    ValuationResult Run(ValuationInput input);
}

public interface ITraceEngine
{
    IReadOnlyList<ValuationTraceLine> BuildTrace(ValuationTraceContext context);
}
```

## 26.2 Deterministic Implementation Rule

No valuation engine method may depend on:

- Current system time except persisted run timestamps
- Random numbers
- Non-versioned external data
- Unordered enumeration where output may vary
- Mutable global state

If input ordering matters, order explicitly by stable fields such as:

```text
system_number
sort_order
comparable_id
sale_date
```

---

# 27. Output Contract

## 27.1 Valuation Result JSON

```json
{
  "valuationRunId": "guid",
  "appraisalId": "guid",
  "inspectionId": "guid",
  "engineVersion": "3.0",
  "rubricVersion": "3.0",
  "rulesetVersion": "3.0",
  "conditionIndex": 69.20,
  "conditionTier": "T4",
  "tierMultiplier": 0.90,
  "baseMarketValue": 25000.00,
  "redFlagModifier": 1.00,
  "collectorIndex": 0.00,
  "collectorAdjustment": 1.00,
  "confidenceScore": 0.82,
  "confidenceModifier": 0.95,
  "finalValue": 21375.00,
  "reportedFinalValue": 21400.00,
  "valueLow": 19300.00,
  "valueHigh": 23500.00,
  "status": "SUCCESS"
}
```

---

# 28. Golden Test Scenario 1 — v1.0 Compatibility Baseline

## 28.1 Input

Vehicle Type: STANDARD

> This scenario preserves the v1.0 test case for backward validation.

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

## 28.2 Expected Condition Math

```text
total_weighted_value = 3.46
condition_index = 3.46 × 20 = 69.20
tier = T4
tier_multiplier = 0.90
```

## 28.3 Comparable and Modifier Inputs

```text
median_comparable_value = 25000
red_flag_modifier_total = 1.00
collector_adjustment = 1.00
confidence_score = 0.82
confidence_modifier = 0.95
```

## 28.4 Expected Valuation

```text
final_value = 25000 × 0.90 × 1.00 × 1.00 × 0.95 = 21375
reported_final_value = 21400
value_low = 19300
value_high = 23500
```

---

# 29. Golden Test Scenario 2 — Locked Standard Weights

## 29.1 Input

Vehicle Type: STANDARD using locked reliability-first weights.

| System Code | Score | Weight | Weighted Value |
|---|---:|---:|---:|
| BODY_EXT | 3 | 0.06 | 0.18 |
| FRAME_STRUCT | 4 | 0.10 | 0.40 |
| POWERTRAIN | 4 | 0.18 | 0.72 |
| TRANS_DRIVE | 3 | 0.14 | 0.42 |
| SUS_STEER | 3 | 0.10 | 0.30 |
| BRAKE_SYS | 4 | 0.10 | 0.40 |
| ELEC_SYS | 3 | 0.10 | 0.30 |
| INTERIOR | 3 | 0.04 | 0.12 |
| WHEEL_TIRE | 4 | 0.06 | 0.24 |
| TRIM_GLASS | 3 | 0.02 | 0.06 |
| DOC_PROV | 2 | 0.02 | 0.04 |
| ROAD_FUNC | 4 | 0.08 | 0.32 |

## 29.2 Expected Condition Math

```text
total_weighted_value = 3.50
condition_index = 3.50 × 20 = 70.00
tier = T3
tier_multiplier = 1.00
```

## 29.3 Comparable and Modifier Inputs

```text
base_market_value = 25000
red_flag_modifier_total = 1.00
collector_adjustment = 1.00
confidence_score = 0.82
confidence_modifier = 0.95
```

## 29.4 Expected Valuation

```text
final_value = 25000 × 1.00 × 1.00 × 1.00 × 0.95 = 23750
reported_final_value = 23800
value_low = 21400
value_high = 26200
```

---

# 30. Acceptance Criteria

The AVM math implementation is acceptable when:

1. All supported weight profiles total 1.00.
2. All scores are validated within 0–5.
3. All 12 canonical systems are represented unless a versioned rule explicitly permits omission.
4. Condition index matches the formula exactly.
5. Critical system floor rule is applied exactly.
6. Tier assignment matches the tier table exactly.
7. Tier multiplier matches the tier table exactly.
8. Comparable statistics are calculated deterministically.
9. Similarity scores are calculated and low-similarity comps are rejected deterministically.
10. Base market value selection follows the locked selection rule.
11. Red flag modifier follows deterministic aggregation rule.
12. Collector Value Layer follows the vehicle-type multiplier table.
13. Confidence score follows the four-factor weighted formula.
14. Confidence modifier follows the confidence table.
15. Final value follows the required merged formula.
16. Value range follows confidence-based range rules.
17. Trace lines are generated for all major calculation steps.
18. Golden test scenarios pass exactly within configured rounding tolerance.
19. Valuation runs store engine, rubric, and ruleset versions.
20. Persisted valuation runs are immutable.

---

# 31. Versioning

Valuation runs must store:

```text
engine_version
rubric_version
ruleset_version
```

Once persisted, a valuation run must not be modified. Corrections require a new valuation run.

---

# 32. Open Items for Future Versions

1. Segment-specific comparable similarity models
2. Market momentum layer
3. Market liquidity layer
4. Cost-to-cure adjustment layer
5. Retail readiness adjustment layer
6. ML-assisted valuation layer
7. Reviewer override workflow
8. Dynamic weighting by vehicle subcategory
9. Regional market adjustment
10. Auction venue normalization
11. Verified sale vs listing-price confidence weighting
12. Insurance/appraisal compliance formatting

---

# 33. Final Lock Statement

This reconciled AVM Math Specification establishes the mathematical source of truth for ONE Classics Nexus AVM v3.0.

It supersedes separate standalone use of:

- `Nexus_AVM_Math_Spec_v1.0.md`
- the simplified `AVM_Math_Specification_v3.0.md`

All API, database, C# engine, test, valuation trace, and report generation work must align to this specification unless formally revised through versioned change control.

---

# End of Document
