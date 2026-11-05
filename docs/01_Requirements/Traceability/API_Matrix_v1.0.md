# ONE Classics Nexus — API Traceability Matrix

**Document:** API_Traceability.md  
**Version:** 1.0  
**Purpose:** Map Nexus requirements to API endpoints.

| Requirement ID | Requirement Summary | API Endpoint(s) | Method(s) | Notes |
|---|---|---|---|---|
| FR-INS-001 | Implement 12-system model | `/api/inspection-templates`, `/api/admin/system-config` | GET | Retrieve active template/config |
| FR-INS-002 | Create inspections | `/api/inspections` | POST | Create inspection for appraisal |
| FR-INS-003 | Update scores, notes, red flags | `/api/inspections/{id}/scores` | PATCH/PUT | Save score, notes, red flag state |
| FR-INS-004 | Retrieve inspection | `/api/inspections/{id}` | GET | Read inspection detail |
| FR-INS-005 | Soft delete inspections | `/api/inspections/{id}` | DELETE | Soft delete/archive |
| FR-SCR-001 | Store scores DECIMAL(3,2) | `/api/inspections/{id}/scores` | PATCH/PUT | Validate 0-5 decimal |
| FR-SCR-002 | Calculate system value | `/api/inspections/{id}/scores`, `/api/appraisals/{id}/valuation/run` | PATCH/POST | Save score; valuation recalculates |
| FR-SCR-003 | Calculate condition index | `/api/appraisals/{id}/valuation/run` | POST | Run condition math |
| FR-SCR-004 | Enforce 0-5 bounds | `/api/inspections/{id}/scores` | PATCH/PUT | Return validation error |
| FR-WGT-001 | Support vehicle-type weights | `/api/admin/system-config`, `/api/inspection-templates` | GET/POST | Admin-controlled config |
| FR-WGT-002 | Validate weights total 1.00 | `/api/admin/system-config/validate`, `/api/appraisals/{id}/valuation/run` | POST | Reject invalid profile |
| FR-WGT-003 | Apply weights by vehicle type | `/api/appraisals/{id}/valuation/run` | POST | Vehicle type selects profile |
| FR-TIER-001 | Derive condition tier | `/api/appraisals/{id}/valuation/run` | POST | Calculated during valuation |
| FR-TIER-002 | Assign T1-T7 tier | `/api/appraisals/{id}/valuation/run`, `/api/valuation/runs/{id}` | POST/GET | Persist and retrieve tier |
| FR-VAL-001 | Apply valuation formula | `/api/appraisals/{id}/valuation/run` | POST | Execute AVM |
| FR-VAL-002 | Use median comparable value | `/api/appraisals/{id}/comparables`, `/api/appraisals/{id}/valuation/run` | GET/POST | Comps feed AVM |
| FR-VAL-003 | Apply tier multiplier | `/api/appraisals/{id}/valuation/run` | POST | AVM output |
| FR-VAL-004 | Apply red flag modifiers | `/api/inspections/{id}/scores`, `/api/appraisals/{id}/valuation/run` | PATCH/POST | Capture then apply red flags |
| FR-VAL-005 | Apply confidence modifier | `/api/appraisals/{id}/valuation/run` | POST | Confidence engine |
| FR-VAL-006 | Generate value range | `/api/appraisals/{id}/valuation/run`, `/api/valuation/runs/{id}` | POST/GET | Low/high value output |
| FR-RPT-001 | Generate appraisal report | `/api/reports`, `/api/appraisals/{id}/report` | POST | Create report |
| FR-RPT-002 | Include condition/value/market evidence | `/api/reports/{id}` | GET | Retrieve full report |
| FR-RPT-003 | Draft to Final lifecycle | `/api/reports/{id}` | PATCH/PUT | Update report status/version |
| FR-RPT-004 | Lock finalized reports | `/api/reports/{id}/finalize` | POST | Finalize and lock |
| FR-ADM-001 | Provide admin UI | `/api/admin/system-config` | GET | UI configuration source |
| FR-ADM-002 | Admin-only changes | `/api/admin/system-config` | POST/PUT | Requires admin role |
| FR-ADM-003 | Version config changes | `/api/admin/system-config` | POST/PUT | Creates new version |
| FR-ADM-004 | Preserve config history | `/api/admin/system-config/history` | GET | Read history |
| FR-ADM-005 | Validate system weights | `/api/admin/system-config/validate` | POST | Validate before activation |
| NFR-SEC-001 | JWT authentication | All secured endpoints | All | Auth middleware |
| NFR-SEC-002 | RBAC | Admin/report/inspection endpoints | All | Role enforcement |
