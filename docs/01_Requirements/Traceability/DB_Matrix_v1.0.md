# ONE Classics Nexus — Database Traceability Matrix

**Document:** DB_Traceability.md  
**Version:** 1.0  
**Purpose:** Map Nexus requirements to database tables and key fields.

| Requirement ID | Requirement Summary | Primary Table(s) | Key Field(s) | Notes |
|---|---|---|---|---|
| FR-INS-001 | Implement 12-system model | `inspection_templates`, `inspection_template_items`, `system_weights` | `vehicle_type`, `system_code`, `system_key`, `version` | Template-driven structure |
| FR-INS-002 | Create inspections | `inspections` | `inspection_id`, `appraisal_id`, `vehicle_type`, `status` | Parent inspection record |
| FR-INS-003 | Update scores, notes, red flags | `condition_scores`, `red_flag_rules` | `score_0_to_5`, `is_red_flag`, `notes`, `system_code` | Captures AVM inputs |
| FR-INS-004 | Retrieve inspection | `inspections`, `condition_scores` | `inspection_id`, `appraisal_id` | Inspection read model |
| FR-INS-005 | Soft delete inspections | `inspections` | `status`, `updated_at_utc` | Archive only |
| FR-SCR-001 | Store scores DECIMAL(3,2) | `condition_scores` | `score_0_to_5` | Canonical score field |
| FR-SCR-002 | Calculate system value | `condition_scores` | `score_0_to_5`, `weight`, `weighted_value` | score x weight |
| FR-SCR-003 | Calculate condition index | `condition_scores`, `valuation_runs` | `weighted_value`, `condition_score` | Derived then persisted |
| FR-SCR-004 | Enforce 0-5 bounds | `condition_scores` | `score_0_to_5` | DB/API validation |
| FR-WGT-001 | Support vehicle-type weights | `system_weights` | `vehicle_type`, `system_code`, `weight` | Standard, Custom, Classic |
| FR-WGT-002 | Validate weights total 1.00 | `system_weights` | `weight`, `version`, `is_active` | Weight profile validation |
| FR-WGT-003 | Apply weights by vehicle type | `system_weights`, `condition_scores` | `vehicle_type`, `weight` | Selected by vehicle type |
| FR-TIER-001 | Derive condition tier | `valuation_runs` | `condition_score`, `condition_tier` | Tier stored with run |
| FR-TIER-002 | Assign T1-T7 tier | `valuation_runs` | `condition_tier`, `tier_multiplier` | Persisted result |
| FR-VAL-001 | Apply valuation formula | `valuation_runs` | `final_value`, `value_low`, `value_high`, `status` | Immutable output |
| FR-VAL-002 | Use median comparable value | `comparable_sales`, `valuation_runs` | `sale_price`, `median_comparable_value` | Market anchor |
| FR-VAL-003 | Apply tier multiplier | `valuation_runs` | `tier_multiplier` | AVM factor |
| FR-VAL-004 | Apply red flag modifiers | `red_flag_rules`, `condition_scores`, `valuation_runs` | `modifier`, `is_red_flag`, `red_flag_modifier` | MIN modifier rule |
| FR-VAL-005 | Apply confidence modifier | `valuation_runs` | `confidence_score`, `confidence_modifier` | Confidence adjusted |
| FR-VAL-006 | Generate value range | `valuation_runs` | `value_low`, `value_high`, `final_value` | Reportable range |
| FR-RPT-001 | Generate appraisal report | `appraisal_reports` | `report_id`, `appraisal_id`, `valuation_run_id` | Report record |
| FR-RPT-002 | Include condition/value/market evidence | `appraisal_reports`, `valuation_runs`, `comparable_sales` | `condition_score`, `final_value`, `comparable_id` | Report data sources |
| FR-RPT-003 | Draft to Final lifecycle | `appraisal_reports` | `status`, `version`, `updated_at_utc` | Report workflow |
| FR-RPT-004 | Lock finalized reports | `appraisal_reports` | `status`, `locked_at_utc` | Immutable final report |
| FR-ADM-001 | Provide admin UI | `system_weights`, `inspection_templates`, `red_flag_rules` | `is_active`, `version` | Admin config data |
| FR-ADM-002 | Admin-only changes | `users`, `roles`, `user_roles` | `user_id`, `role_id` | RBAC enforcement |
| FR-ADM-003 | Version config changes | `system_weights`, `inspection_templates`, `red_flag_rules` | `version`, `created_at_utc` | Change-controlled config |
| FR-ADM-004 | Preserve config history | `system_weights`, `inspection_templates`, `red_flag_rules` | `version`, `is_active` | Historical stability |
| FR-ADM-005 | Validate system weights | `system_weights` | `vehicle_type`, `weight`, `version` | Total must equal 1.00 |
| NFR-DATA-001 | Maintain data integrity | All core tables | PK/FK fields | Relational consistency |
| NFR-AUD-001 | Maintain audit logging | `valuation_trace_lines`, config/history tables | `created_at_utc`, `trace_key`, `message` | Explainability and audit |
