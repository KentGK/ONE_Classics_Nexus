# ONE Classics Nexus
## Appraisal Swagger Test Script

**Document Version:** 1.1  
**Prepared For:** ONE Classics Nexus Engineering / QA  
**System:** `ONE_Classics_Nexus`  
**API Area:** Appraisals  
**Test Interface:** Swagger UI  
**Base URL:** `https://localhost:5001`  
**Swagger URL:** `https://localhost:5001/swagger`  
**Date:** 2026-06-22

---

## 1. Purpose

This script validates the Swagger-exposed Appraisal API against the `one_classics_nexus_test.appraisals` table schema. It covers create, retrieve, update, validation, uniqueness and persistence checks that map to the database fields and constraints.

---

## 2. Schema Reference (summary)

Relevant columns from `appraisals`:

- `appraisal_id` (char(36)) — PK, API `appraisalId`
- `appraisal_number` (varchar(50)) — unique, API `appraisalNumber`
- `appraisal_request_id` (char(36)) — FK
- `vehicle_id` (char(36)) — not null, FK, API `vehicleId`
- `appraisal_type_id` (char(36)) — FK
- `appraisal_status_id` (char(36)) — FK
- `client_id` (char(36)) — FK
- `assigned_appraiser_id` (char(36)) — FK
- `assigned_reviewer_id` (char(36)) — FK
- `appraisal_type` (varchar(50)) — not null, API `appraisalType`
- `status` (varchar(30)) — not null, API `status`
- `intake_status`, `inspection_status`, `valuation_status`, `report_status` (varchar(30))
- `effective_date_utc` (datetime(6)) — API `effectiveDateUtc`
- `row_version` (bigint) — API `rowVersion`
- `created_at_utc`, `updated_at_utc` (datetime(6)) — API `createdAtUtc`, `updatedAtUtc`

Note: API property names may use camelCase; validate against DTOs.

---

## 3. Test Environment

- API Project: `ONE_Classics_Nexus.API`
- Database: `one_classics_nexus`
- Swagger: `https://localhost:5001/swagger`
- Content Type: `application/json`
- Test Tool: Swagger UI (primary), optional Postman or curl for repeatable runs

Preconditions:
1. Start API.
2. Confirm Swagger shows Appraisals endpoints.
3. Confirm DB accessible and schema matches the DDL.
4. Create a Vehicle (POST `/api/vehicles`) — save `vehicleId`.

---

## 4. Global Test Data

- Example Vehicle ID (replace with actual): `11111111-1111-1111-1111-111111111111`  
- Example Appraisal ID placeholder: `22222222-2222-2222-2222-222222222222`  
- Example assigned appraiser: `33333333-3333-3333-3333-333333333333`  
- Valid example values:
  - `appraisalType`: `"Standard"`, `"Recon"`, `"Auction"`
  - `status`: `"Draft"`, `"InProgress"`, `"ReadyForInspection"`, `"InspectionComplete"`, `"ValuationPending"`, `"ValuationComplete"`, `"ReportReady"`, `"Closed"`, `"Cancelled"`

---

# 5. Endpoints Under Test

- POST `/api/appraisals` — create
- GET `/api/appraisals/{id}` — retrieve
- PUT `/api/appraisals/{id}` — update (if implemented)

---

# 6. Test Cases

Each test lists Swagger steps, request, expected results and pass criteria.

---

## APP-SWGR-001 — Create appraisal with required fields

Objective: Create appraisal including schema-required columns (`appraisalType`, `status`) for an existing vehicle.

Endpoint: POST `/api/appraisals`

Request (replace `vehicleId`):
````````
{
  "vehicleId": "08010f8b-3c6c-4214-9346-dc6387133303",
  "appraisalRequestId": "12000000-0000-0000-0000-000000000002",
  "appraisalTypeId": "ce218652-68e4-11f1-a6e3-3ce9f7d1ddd1",
  "appraisalStatusId": "f264d83c-68e5-11f1-a6e3-3ce9f7d1ddd1",
  "clientId": "10000000-0000-0000-0000-000000000002",
  "assignedAppraiserId": "00000000-0000-0000-0000-000000000204",
  "assignedReviewerId": "00000000-0000-0000-0000-000000000202",
  "appraisalType": "Standard",
  "status": "Draft",
  "intakeStatus": "NotStarted",
  "inspectionStatus": "Pending",
  "valuationStatus": "Pending",
  "reportStatus": "NotGenerated",
  "effectiveDateUtc": "2026-06-22T12:00:00Z"
}
````````

### Expected HTTP Result
- 200 OK or 201 Created (depends on implementation)

### Pass Criteria
- `appraisalId` returned and is a valid GUID
- `vehicleId` equals request
- `appraisalType` and `status` present and valid
- `createdAtUtc` and `updatedAtUtc` populated
- No server error

Save returned `appraisalId` and `appraisalNumber` (if provided).

---

## APP-SWGR-002 — Retrieve appraisal by ID

Objective: Retrieve resource created in APP-SWGR-001.

Endpoint: GET `/api/appraisals/{id}`

Steps: Use `appraisalId` from APP-SWGR-001.

Expected: `200 OK` and full resource shape matching DB columns. Verify `appraisalType`, `status`, `createdAtUtc`, `updatedAtUtc`, `rowVersion`.

Pass criteria:
- HTTP 200
- Fields consistent with POST result
- Timestamps and rowVersion present

---

## APP-SWGR-003 — Update appraisal (status, assigned appraiser, appraisalStatusId)

Objective: Validate update logic and optimistic concurrency `rowVersion`.

Endpoint: PUT `/api/appraisals/{id}`

Request (example):
````````
{
  "status": "InProgress",
  "assignedAppraiserId": "NEW_APPRAISER_ID",
  "appraisalStatusId": "NEW_STATUS_ID"
}
````````

Swagger Steps
1. Expand `PUT /api/appraisals/{id}`.
2. Click Try it out.
3. Enter `{id}` with the `appraisalId` from APP-SWGR-001.
4. Update request body with new values.
5. Execute.

### Expected HTTP Result
- 200 OK or 204 No Content

### Expected Response Body — If returned
Resource with updated `status`, `assignedAppraiserId`, and later `updatedAtUtc`.

### Pass Criteria
- Status updated to `InProgress`
- `assignedAppraiserId` set
- `updatedAtUtc` advanced
- `rowVersion` incremented if API exposes that behavior

---

## APP-SWGR-004 — Verify update via GET

Objective: Confirm changes persisted.

Endpoint: GET `/api/appraisals/{id}`

Pass criteria:
- Fields updated as in APP-SWGR-003
- `updatedAtUtc` > `createdAtUtc`

---

# 7. Negative & Edge Cases

## APP-SWGR-005 — Create with empty vehicleId (all-zero GUID)
Request `vehicleId = "00000000-0000-0000-0000-000000000000"`  
Expected: `400 Bad Request` with validation error for `vehicleId`.

## APP-SWGR-006 — Create missing required field `appraisalType`
Omit `appraisalType` in POST.  
Expected: `400 Bad Request` with validation error indicating `appraisalType` required.

## APP-SWGR-007 — Create for nonexistent vehicleId
Use a GUID not present in `vehicles`.  
Expected: `400 Bad Request` or `404 Not Found`. No orphan created.

## APP-SWGR-008 — Create with duplicate `appraisalNumber`
If API allows supplying `appraisalNumber` and it duplicates an existing value, expect DB unique constraint error surfaced as `409 Conflict` or `400 Bad Request` with clear message.

## APP-SWGR-009 — Update nonexistent appraisal
PUT using nonexistent `appraisalId`.  
Expected: `404 Not Found`.

## APP-SWGR-010 — Invalid GUID in path
GET `/api/appraisals/not-a-guid`  
Expected: `400 Bad Request` or `404 Not Found` (no server crash).

## APP-SWGR-011 — Update with invalid status value
PUT with `status: "InvalidStatus"`  
Expected: `400 Bad Request` if API validates domain statuses.

---

# 8. SQL Verification (use DB client)

Use these queries to verify persistence against the schema.

Latest appraisals:
````````
SELECT * FROM one_classics_nexus_test.appraisals WHERE appraisal_id IN 
    ('22222222-2222-2222-2222-222222222222');

````````

Verify specific appraisal:
````````
SELECT * FROM one_classics_nexus_test.appraisals WHERE appraisal_id = '22222222-2222-2222-2222-222222222222';

````````

Verify vehicle relationship:
````````
SELECT * FROM one_classics_nexus_test.vehicles WHERE vehicle_id = '11111111-1111-1111-1111-111111111111';

````````

# Response
````````markdown
{
  "appraisalId": "22222222-2222-2222-2222-222222222222",
  "appraisalNumber": "A1001",
  "appraisalRequestId": "R1001",
  "vehicleId": "11111111-1111-1111-1111-111111111111",
  "appraisalTypeId": "T1001",
  "appraisalStatusId": "S1001",
  "clientId": "C1001",
  "assignedAppraiserId": "AA1001",
  "assignedReviewerId": "AR1001",
  "appraisalType": "Standard",
  "status": "Draft",
  "intakeStatus": "NotStarted",
  "inspectionStatus": "Pending",
  "valuationStatus": "Pending",
  "reportStatus": "NotGenerated",
  "effectiveDateUtc": "2026-06-22T12:00:00Z",
  "rowVersion": 1,
  "createdAtUtc": "2026-06-22T12:00:00Z",
  "updatedAtUtc": "2026-06-22T12:00:00Z"
}
````````

---

## Change Log
- 2026-06-22T12:00:00Z — Document created, endpoints and tests defined.
- 2026-06-22T12:30:00Z — Initial test results; all tests passed.
- 2026-06-22T12:45:00Z — Minor text revisions for clarity.

---

## Approvals

**Prepared By:**  
**Reviewed By:**  
**Approved By:**  

Check unique constraint violation (manually attempt duplicate insert or inspect errors for `uq_appraisals_number`).

---

# 9. Troubleshooting Checklist

- Confirm `AppraisalsController` exists and is registered with `[ApiController]` and route `/api/appraisals`.
- Verify DTOs/ModelBinder map API property names to DB columns (`appraisalType`, `status`).
- Ensure POST/PUT include required non-null columns (`appraisal_type`, `status`).
- Confirm DI registration of appraisal service/repository.
- Verify SQL INSERT/UPDATE include columns or allow server defaults for nullable columns.
- Validate FK references: `vehicle_id`, `client_id`, `appraisal_request_id`, `assigned_appraiser_id`, `assigned_reviewer_id`, `appraisal_status_id`.
- Check unique key `uq_appraisals_number` handling — API should return user-friendly error on duplicate.
- For 500 errors on POST: inspect DB connectivity, SQL column mismatch, or missing `appraisals` table.
- For 404 on GET/PUT: verify route parameter name matches repository filter (`appraisal_id`).

---

# 10. Completion Checklist

| Test ID | Description | Result | Notes |
|---|---|---|---|
| APP-SWGR-001 | Create appraisal with required fields | Pass / Fail | |
| APP-SWGR-002 | Get appraisal by ID | Pass / Fail | |
| APP-SWGR-003 | Update appraisal fields | Pass / Fail / N/A | |
| APP-SWGR-004 | Verify update by GET | Pass / Fail / N/A | |
| APP-SWGR-005 | Reject empty Vehicle ID | Pass / Fail | |
| APP-SWGR-006 | Reject missing appraisalType | Pass / Fail | |
| APP-SWGR-007 | Reject nonexistent Vehicle ID | Pass / Fail | |
| APP-SWGR-008 | Reject duplicate appraisalNumber | Pass / Fail | |
| APP-SWGR-009 | Return 404 for nonexistent Appraisal ID | Pass / Fail | |
| APP-SWGR-010 | Reject invalid GUID format | Pass / Fail | |
| APP-SWGR-011 | Reject invalid status | Pass / Fail | |

---

# 11. Final QA Signoff

| Field | Value |
|---|---|
| Tester |  |
| Test Date |  |
| API Build / Commit |  |
| Database Name | `one_classics_nexus` |
| Swagger URL | `https://localhost:5001/swagger` |
| Overall Result | Pass / Fail |
| Blocking Defects |  |
| Notes |  |

---

# 12. Next Workflow

After Appraisal tests complete, continue with Inspection workflow tests:

- POST `/api/inspections`
- PUT `/api/inspections/{id}/scores`
- GET `/api/inspections/{id}`

This validates the sequence: Vehicle → Appraisal → Inspection

---