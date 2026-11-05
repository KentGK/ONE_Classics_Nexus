# ONE Classics Nexus — AppraisalWorkspace Engineering Notes

**Version:** 1.0  
**Target:** `ONE_Classics_Nexus.WinForms` / .NET 8 / WinForms

## 1. Purpose

Provide a shell-embeddable appraisal lifecycle workspace that coordinates the subject vehicle, intake, inspection, comparable evidence, valuation, review, and report workflow without bypassing Nexus API boundaries.

## 2. Source reconciliation decisions

### 2.1 Appraisal schema

The implementation uses all lifecycle fields present in the supplied appraisal DDL:

- `appraisal_id`
- `appraisal_number`
- `appraisal_request_id`
- `vehicle_id`
- `appraisal_type_id` when present
- `appraisal_status_id` when present
- `client_id`
- `assigned_appraiser_id`
- `assigned_reviewer_id`
- `appraisal_type`
- `status`
- `intake_status`
- `inspection_status`
- `valuation_status`
- `report_status`
- `effective_date_utc`
- `row_version`
- timestamps

The newer DDL contains normalized type/status IDs while retaining text fields. v1.0 therefore carries both and does not remove the current text contract.

### 2.2 ERD relationships

The workspace treats `appraisals` as the workflow parent and exposes navigation context for:

- `vehicles`
- `inspections`
- `comparable_sales`
- `valuation_runs`
- `appraisal_reports`

Assignments and client relationships are displayed through the appraisal foreign keys and optional nested API summaries.

### 2.3 Sequence behavior

- `GET /api/appraisals/{id}` loads the workspace.
- `PUT /api/appraisals/{id}` saves editable appraisal lifecycle fields.
- `rowVersion` is always sent from the currently loaded response.
- `409 Conflict` is handled as optimistic-concurrency failure.
- downstream actions preserve `appraisalId`.
- API errors are surfaced from `ProblemDetails` when present.

## 3. UI composition

```text
AppraisalWorkspaceForm
├── AppraisalHeaderControl
├── AppraisalSummaryControl
├── AppraisalWorkflowControl
├── ContentHost
│   ├── Overview
│   ├── Intake
│   ├── Vehicle
│   ├── Inspection
│   ├── Comparables
│   ├── Valuation
│   ├── Review
│   └── Report
└── StatusStrip
```

## 4. Ownership boundaries

The Appraisal Workspace owns:

- lifecycle context display
- appraisal lifecycle field edits
- workflow navigation
- save/refresh state
- concurrency user experience

It does not own:

- vehicle persistence
- inspection scoring
- comparable adjustment logic
- AVM execution logic
- report generation logic
- SQL access

## 5. API response evolution

For compatibility, the service parser accepts direct and envelope responses. The preferred richer response shape is:

```json
{
  "appraisal": { },
  "vehicle": { },
  "client": { },
  "assignedAppraiser": { },
  "assignedReviewer": { },
  "latestInspection": { },
  "latestValuation": { },
  "latestReport": { },
  "comparableCount": 5
}
```

The current CRUD API may continue returning the direct appraisal object; the form remains functional and shows graceful placeholders for missing nested summaries.
