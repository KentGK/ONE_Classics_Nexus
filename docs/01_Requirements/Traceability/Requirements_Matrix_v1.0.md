# ONE Classics Nexus — Requirements Matrix

**Document:** Requirements_Matrix.md  
**Version:** 1.0  
**Purpose:** Map Nexus requirements to their primary system/module ownership.

| Requirement ID | Requirement Summary | Primary System / Module | Phase | Notes |
|---|---|---|---|---|
| FR-INS-001 | Implement 12-system model | Inspection Engine | Phase 1-3 | Canonical inspection taxonomy |
| FR-INS-002 | Create inspections | Inspection Engine | Phase 1 | Linked to appraisal |
| FR-INS-003 | Update scores, notes, and red flags | Inspection Engine / Red Flag Layer | Phase 1-3 | Feeds AVM |
| FR-INS-004 | Retrieve inspection | Inspection Engine / API | Phase 1 | Used by UI and reports |
| FR-INS-005 | Soft delete inspections | Inspection Engine / Audit | Phase 3 | Preserve history |
| FR-SCR-001 | Store scores DECIMAL(3,2) | Scoring Engine / Database | Phase 1 | 0-5 scale |
| FR-SCR-002 | Calculate system value = score x weight | Scoring Engine | Phase 1-2 | Deterministic math |
| FR-SCR-003 | Calculate condition index | Condition Engine | Phase 2 | AVM input |
| FR-SCR-004 | Enforce 0-5 score bounds | Validation / API | Phase 1 | Prevent invalid scoring |
| FR-WGT-001 | Support vehicle-type weights | Weight Configuration | Phase 2-3 | Standard, Custom, Classic |
| FR-WGT-002 | Validate weights total 1.00 | Weight Configuration / AVM | Phase 2 | Blocks invalid valuation |
| FR-WGT-003 | Apply weights by vehicle type | Condition Engine | Phase 2 | Deterministic |
| FR-TIER-001 | Derive condition tier | Tier Engine | Phase 2 | T1-T7 |
| FR-TIER-002 | Assign T1-T7 tier | Tier Engine / Reporting | Phase 2 | Used in valuation and report |
| FR-VAL-001 | Apply valuation formula | Valuation Engine | Phase 2 | Core AVM |
| FR-VAL-002 | Use median comparable value | Comparable / Valuation Engine | Phase 2 | Market anchor |
| FR-VAL-003 | Apply tier multiplier | Valuation Engine | Phase 2 | Value adjustment |
| FR-VAL-004 | Apply red flag modifiers | Red Flag / Valuation Engine | Phase 2-3 | Risk adjustment |
| FR-VAL-005 | Apply confidence modifier | Confidence / Valuation Engine | Phase 2 | Confidence-adjusted output |
| FR-VAL-006 | Generate value range | Valuation / Reporting | Phase 2-3 | Low/high value |
| FR-RPT-001 | Generate appraisal report | Reporting Engine | Phase 3 | Client-facing output |
| FR-RPT-002 | Include condition, value, and market evidence | Reporting Engine | Phase 3 | Report content |
| FR-RPT-003 | Draft to Final report lifecycle | Reporting Workflow | Phase 3 | Approval flow |
| FR-RPT-004 | Lock finalized reports | Reporting / Audit | Phase 3 | Immutability |
| FR-ADM-001 | Provide admin UI | Admin System | Phase 3 | Governance |
| FR-ADM-002 | Restrict config changes to admins | Admin / Security | Phase 3 | RBAC |
| FR-ADM-003 | Version configuration changes | Admin / Configuration | Phase 3 | Historical stability |
| FR-ADM-004 | Preserve configuration history | Admin / Audit | Phase 3 | Change control |
| FR-ADM-005 | Validate system weights | Admin / Weight Config | Phase 3 | Prevent invalid profile |
| NFR-PERF-001 | Valuation under 2 seconds | Performance / AVM | Phase 2-3 | Target SLA |
| NFR-PERF-002 | Save under 500ms | Performance / API | Phase 1-3 | Target SLA |
| NFR-SEC-001 | JWT authentication | Security | Phase 3 | API security |
| NFR-SEC-002 | Role-based access control | Security | Phase 3 | Role enforcement |
| NFR-DATA-001 | Maintain data integrity | Database / Application | Phase 1-3 | Referential integrity |
| NFR-AUD-001 | Maintain audit logging | Audit | Phase 3 | Compliance support |
