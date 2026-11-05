# ONE Classics Nexus API Specification v1.0

This document is the Developer API Specification for the ONE Classics Nexus solution.

## Source Alignment

This OpenAPI contract is aligned to:
- SRS v2.0: vehicle appraisal lifecycle, inspection engine, scoring, weights, tiering, valuation, reporting, and admin governance.
- Inspection Data Model v1.0: `inspections`, `condition_scores`, `system_weights`, `inspection_templates`, and `red_flag_rules`.
- AVM Math Specification v1.0: deterministic AVM pipeline, condition index, tiering, red flags, confidence, final valuation, valuation runs, and trace lines.

## Included API Families

- Vehicles
- Appraisals
- Inspections
- Comparables
- Valuation
- Reports
- Admin configuration

## OpenAPI JSON

The full OpenAPI 3.0.3 JSON file is available here:

`ONE_Classics_Nexus_OpenAPI_Spec_v1.0.json`

## Core Inspection Contracts

```http
POST   /api/inspections
GET    /api/inspections/{inspectionId}
PUT    /api/inspections/{inspectionId}
DELETE /api/inspections/{inspectionId}
PUT    /api/inspections/{inspectionId}/scores
```

## Core Valuation Contracts

```http
POST /api/appraisals/{appraisalId}/valuation/run
GET  /api/appraisals/{appraisalId}/valuation/runs
GET  /api/valuation/runs/{valuationRunId}
```

## Admin Governance Contracts

```http
GET  /api/admin/system-config
POST /api/admin/system-config
GET  /api/admin/system-weights
POST /api/admin/system-weights
GET  /api/admin/inspection-templates
POST /api/admin/inspection-templates
GET  /api/admin/red-flag-rules
POST /api/admin/red-flag-rules
```

## Error Format

All API errors should return `application/problem+json`.

```json
{
  "type": "about:blank",
  "title": "Validation error",
  "status": 400,
  "detail": "Score must be between 0 and 5.",
  "instance": "/api/inspections/{inspectionId}/scores",
  "traceId": "..."
}
```

## Design Notes

- `condition_scores` is the authoritative inspection scoring contract.
- `score0To5` maps to database column `score_0_to_5`.
- Vehicle type values are `STANDARD`, `CUSTOM`, and `CLASSIC`.
- Valuation runs are immutable; corrections require a new run.
- Valuation trace lines are part of the public valuation detail response for auditability.
