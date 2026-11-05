# ONE Classics Nexus  
## Appraisal Swagger Test Script  

**Document Version:** 1.0  
**Prepared For:** ONE Classics Nexus Engineering / QA  
**System:** `ONE_Classics_Nexus`  
**API Area:** Appraisals  
**Test Interface:** Swagger UI  
**Base URL:** `https://localhost:5001`  
**Swagger URL:** `https://localhost:5001/swagger`  
**Date:** 2026-06-11  

---

## 1. Purpose

This test script validates the Swagger-exposed Appraisal API workflow for ONE Classics Nexus.

The script verifies that the Appraisal API can:

1. Create an appraisal for an existing vehicle.
2. Retrieve an appraisal by ID.
3. Update appraisal status, if the PUT endpoint is implemented.
4. Return expected errors for invalid, missing, or nonexistent IDs.
5. Persist appraisal records correctly in the database.

---

## 2. Scope

### In Scope

| Area | Included |
|---|---:|
| Swagger request validation | Yes |
| Appraisal POST | Yes |
| Appraisal GET by ID | Yes |
| Appraisal PUT by ID | Yes, if implemented |
| Database verification | Yes |
| Negative testing | Yes |
| Vehicle creation dependency | Referenced as prerequisite |
| Inspection workflow continuation | Referenced only |

### Out of Scope

| Area | Reason |
|---|---|
| Full Vehicle API testing | Covered by Vehicle test script |
| Full Inspection API testing | Covered by Inspection test script |
| Valuation run testing | Phase 2 / valuation test script |
| Report generation | Reporting test script |

---

## 3. Design Alignment

The Appraisal API participates in the primary Nexus workflow:

```text
Vehicle → Appraisal → Inspection → Condition Scores → Valuation → Report
```

The Appraisal API is expected to expose the following endpoints:

```text
POST   /api/appraisals
GET    /api/appraisals/{id}
PUT    /api/appraisals/{id}
```

The Appraisal database record is expected to include at minimum:

```text
appraisal_id
vehicle_id
status
created_at
updated_at
row_version
```

> Note: If the current implementation uses `created_at_utc` and `updated_at_utc` instead of `created_at` and `updated_at`, verify against the locked database schema and repository SQL.

---

## 4. Test Environment

| Item | Value |
|---|---|
| API Project | `ONE_Classics_Nexus.API` |
| Database | `one_classics_nexus` |
| Local HTTPS URL | `https://localhost:5001` |
| Swagger URL | `https://localhost:5001/swagger` |
| Content Type | `application/json` |
| Test Tool | Swagger UI |

---

## 5. Preconditions

Before executing this script:

1. Start the API project.
2. Open Swagger at:

```text
https://localhost:5001/swagger
```

3. Confirm the Appraisals section appears in Swagger.
4. Confirm the database is available.
5. Create a Vehicle first using `POST /api/vehicles`.
6. Copy the returned `vehicleId` for use in Appraisal tests.

---

## 6. Test Data

### 6.1 Existing Vehicle ID

Replace this placeholder with a real `vehicleId` returned from Vehicle POST.

```text
11111111-1111-1111-1111-111111111111
```

### 6.2 Expected Appraisal ID

The API should generate this value. The following value is only a placeholder for examples.

```text
22222222-2222-2222-2222-222222222222
```

### 6.3 Valid Appraisal Status Values

Use only the values currently supported by the API/domain model.

```text
Draft
InProgress
ReadyForInspection
InspectionComplete
ValuationPending
ValuationComplete
ReportReady
Closed
Cancelled
```

If the current implementation only supports `Draft`, treat all other status values as future workflow values.

---

# 7. Test Cases

---

## APP-SWGR-001 — Create Appraisal with Valid Vehicle ID

### Objective

Verify that Swagger can create a new appraisal for an existing vehicle.

### Endpoint

```text
POST /api/appraisals
```

### Swagger Steps

1. Open Swagger.
2. Expand `POST /api/appraisals`.
3. Click **Try it out**.
4. Enter the request body below.
5. Replace `vehicleId` with a real Vehicle ID.
6. Click **Execute**.

### Request Body

```json
{
  "vehicleId": "11111111-1111-1111-1111-111111111111"
}
```

### Expected HTTP Result

```text
200 OK
```

or

```text
201 Created
```

The accepted success code depends on controller implementation.

### Expected Response Body

```json
{
  "appraisalId": "22222222-2222-2222-2222-222222222222",
  "vehicleId": "11111111-1111-1111-1111-111111111111",
  "status": "Draft",
  "createdAtUtc": "2026-06-11T15:30:00Z",
  "updatedAtUtc": "2026-06-11T15:30:00Z"
}
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Appraisal ID returned | Yes |
| Vehicle ID matches request | Yes |
| Status defaults to `Draft` | Yes |
| Created timestamp populated | Yes |
| Updated timestamp populated | Yes |
| Response has no server error | Yes |

### Save for Later Tests

Copy the returned value:

```text
appraisalId
```

---

## APP-SWGR-002 — Retrieve Existing Appraisal by ID

### Objective

Verify that Swagger can retrieve an appraisal created in `APP-SWGR-001`.

### Endpoint

```text
GET /api/appraisals/{id}
```

### Swagger Steps

1. Expand `GET /api/appraisals/{id}`.
2. Click **Try it out**.
3. Paste the `appraisalId` returned from `APP-SWGR-001` into the `id` field.
4. Click **Execute**.

### Path Parameter

```text
22222222-2222-2222-2222-222222222222
```

### Expected HTTP Result

```text
200 OK
```

### Expected Response Body

```json
{
  "appraisalId": "22222222-2222-2222-2222-222222222222",
  "vehicleId": "11111111-1111-1111-1111-111111111111",
  "status": "Draft",
  "createdAtUtc": "2026-06-11T15:30:00Z",
  "updatedAtUtc": "2026-06-11T15:30:00Z"
}
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| HTTP status is success | `200 OK` |
| Appraisal ID matches path parameter | Yes |
| Vehicle ID is present | Yes |
| Status is present | Yes |
| Created timestamp is present | Yes |
| Updated timestamp is present | Yes |

---

## APP-SWGR-003 — Update Appraisal Status

### Objective

Verify that Swagger can update an existing appraisal status.

> Execute this test only if `PUT /api/appraisals/{id}` is implemented in the current API build.

### Endpoint

```text
PUT /api/appraisals/{id}
```

### Swagger Steps

1. Expand `PUT /api/appraisals/{id}`.
2. Click **Try it out**.
3. Paste the `appraisalId` returned from `APP-SWGR-001` into the `id` field.
4. Enter the request body below.
5. Click **Execute**.

### Path Parameter

```text
22222222-2222-2222-2222-222222222222
```

### Request Body

```json
{
  "status": "InProgress"
}
```

### Expected HTTP Result

```text
200 OK
```

or

```text
204 No Content
```

The accepted success code depends on controller implementation.

### Expected Response Body — If API Returns Updated Resource

```json
{
  "appraisalId": "22222222-2222-2222-2222-222222222222",
  "vehicleId": "11111111-1111-1111-1111-111111111111",
  "status": "InProgress",
  "createdAtUtc": "2026-06-11T15:30:00Z",
  "updatedAtUtc": "2026-06-11T15:45:00Z"
}
```

### Expected Response Body — If API Returns No Content

```text
No response body
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| API accepts valid status | Yes |
| Status changes to `InProgress` | Yes |
| Updated timestamp changes | Yes |
| Record remains linked to same vehicle | Yes |

---

## APP-SWGR-004 — Verify Updated Appraisal with GET

### Objective

Verify that the appraisal update from `APP-SWGR-003` persisted correctly.

### Endpoint

```text
GET /api/appraisals/{id}
```

### Path Parameter

```text
22222222-2222-2222-2222-222222222222
```

### Expected HTTP Result

```text
200 OK
```

### Expected Response Body

```json
{
  "appraisalId": "22222222-2222-2222-2222-222222222222",
  "vehicleId": "11111111-1111-1111-1111-111111111111",
  "status": "InProgress",
  "createdAtUtc": "2026-06-11T15:30:00Z",
  "updatedAtUtc": "2026-06-11T15:45:00Z"
}
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Status reflects update | `InProgress` |
| Appraisal ID unchanged | Yes |
| Vehicle ID unchanged | Yes |
| Updated timestamp is later than created timestamp | Yes |

---

# 8. Negative Test Cases

---

## APP-SWGR-005 — Create Appraisal with Empty Vehicle ID

### Objective

Verify API validation rejects an empty GUID.

### Endpoint

```text
POST /api/appraisals
```

### Request Body

```json
{
  "vehicleId": "00000000-0000-0000-0000-000000000000"
}
```

### Expected HTTP Result

```text
400 Bad Request
```

### Expected Error Shape

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "vehicleId": [
      "VehicleId is required."
    ]
  }
}
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Empty GUID rejected | Yes |
| No appraisal created | Yes |
| Error response uses ProblemDetails or validation error format | Yes |

---

## APP-SWGR-006 — Create Appraisal with Nonexistent Vehicle ID

### Objective

Verify API rejects or cannot create an appraisal for a vehicle that does not exist.

### Endpoint

```text
POST /api/appraisals
```

### Request Body

```json
{
  "vehicleId": "99999999-9999-9999-9999-999999999999"
}
```

### Expected HTTP Result

```text
400 Bad Request
```

or

```text
404 Not Found
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Nonexistent Vehicle ID rejected | Yes |
| No orphan appraisal created | Yes |
| Error message is clear | Yes |

---

## APP-SWGR-007 — Get Appraisal with Nonexistent Appraisal ID

### Objective

Verify API returns `404 Not Found` for an appraisal ID that does not exist.

### Endpoint

```text
GET /api/appraisals/{id}
```

### Path Parameter

```text
99999999-9999-9999-9999-999999999999
```

### Expected HTTP Result

```text
404 Not Found
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Nonexistent ID does not return fake data | Yes |
| HTTP status is 404 | Yes |
| No server error | Yes |

---

## APP-SWGR-008 — Get Appraisal with Invalid GUID Format

### Objective

Verify API rejects invalid GUID route values.

### Endpoint

```text
GET /api/appraisals/{id}
```

### Path Parameter

```text
not-a-guid
```

### Expected HTTP Result

```text
400 Bad Request
```

or

```text
404 Not Found
```

The exact behavior depends on route constraint configuration.

### Pass Criteria

| Check | Expected Result |
|---|---|
| Invalid GUID does not crash API | Yes |
| Response is client error, not server error | Yes |

---

## APP-SWGR-009 — Update Appraisal with Invalid Status

### Objective

Verify API rejects unsupported appraisal status values.

> Execute this test only if `PUT /api/appraisals/{id}` validates status values.

### Endpoint

```text
PUT /api/appraisals/{id}
```

### Path Parameter

```text
22222222-2222-2222-2222-222222222222
```

### Request Body

```json
{
  "status": "InvalidStatus"
}
```

### Expected HTTP Result

```text
400 Bad Request
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Invalid status rejected | Yes |
| Existing appraisal status remains unchanged | Yes |
| API returns clear validation message | Yes |

---

## APP-SWGR-010 — Update Nonexistent Appraisal

### Objective

Verify API returns `404 Not Found` when trying to update a nonexistent appraisal.

### Endpoint

```text
PUT /api/appraisals/{id}
```

### Path Parameter

```text
99999999-9999-9999-9999-999999999999
```

### Request Body

```json
{
  "status": "InProgress"
}
```

### Expected HTTP Result

```text
404 Not Found
```

### Pass Criteria

| Check | Expected Result |
|---|---|
| Nonexistent ID rejected | Yes |
| No new appraisal created | Yes |
| No server error | Yes |

---

# 9. Database Verification

Run the following SQL after successful POST/PUT tests.

## 9.1 Latest Appraisals

```sql
SELECT
    appraisal_id,
    vehicle_id,
    status,
    created_at_utc,
    updated_at_utc
FROM appraisals
ORDER BY created_at_utc DESC
LIMIT 10;
```

If the schema uses `created_at` and `updated_at` instead, run:

```sql
SELECT
    appraisal_id,
    vehicle_id,
    status,
    created_at,
    updated_at,
    row_version
FROM appraisals
ORDER BY created_at DESC
LIMIT 10;
```

## 9.2 Verify Specific Appraisal

```sql
SELECT
    appraisal_id,
    vehicle_id,
    status,
    created_at_utc,
    updated_at_utc
FROM appraisals
WHERE appraisal_id = '22222222-2222-2222-2222-222222222222';
```

## 9.3 Verify Vehicle Relationship

```sql
SELECT
    a.appraisal_id,
    a.vehicle_id,
    v.vin,
    v.year,
    v.make,
    v.model,
    a.status,
    a.created_at_utc,
    a.updated_at_utc
FROM appraisals a
JOIN vehicles v
    ON v.vehicle_id = a.vehicle_id
WHERE a.appraisal_id = '22222222-2222-2222-2222-222222222222';
```

---

# 10. Swagger Troubleshooting Checklist

## 10.1 Appraisal Endpoint Missing from Swagger

Check:

- `AppraisalsController` exists.
- Controller has `[ApiController]`.
- Controller has `[Route("api/appraisals")]`.
- Project rebuild succeeded.
- Swagger is enabled in `Program.cs`.

## 10.2 POST Returns 500

Check:

- `IAppraisalApiService` is registered in DI.
- Appraisal repository is registered in DI.
- DB connection string is correct.
- `appraisals` table exists.
- Repository INSERT columns match database columns.
- FK relationship to `vehicles.vehicle_id` is valid.

## 10.3 POST Returns 400

Check:

- `vehicleId` is not empty.
- `vehicleId` exists in `vehicles`.
- Request body property name is exactly `vehicleId`.
- JSON body is valid.

## 10.4 GET Returns 404

Check:

- The copied `appraisalId` is correct.
- The appraisal exists in DB.
- Controller route parameter name matches method argument.
- Repository `GetByIdAsync` filters by `appraisal_id`.

## 10.5 PUT Returns 404

Check:

- Appraisal exists before update.
- The route path is `/api/appraisals/{id}`.
- The controller passes the route ID into service/repository.
- The repository `WHERE appraisal_id = @AppraisalId` clause is correct.

---

# 11. Completion Checklist

| Test ID | Description | Result | Notes |
|---|---|---|---|
| APP-SWGR-001 | Create appraisal with valid vehicle ID | Pass / Fail |  |
| APP-SWGR-002 | Get appraisal by ID | Pass / Fail |  |
| APP-SWGR-003 | Update appraisal status | Pass / Fail / N/A |  |
| APP-SWGR-004 | Verify update by GET | Pass / Fail / N/A |  |
| APP-SWGR-005 | Reject empty Vehicle ID | Pass / Fail |  |
| APP-SWGR-006 | Reject nonexistent Vehicle ID | Pass / Fail |  |
| APP-SWGR-007 | Return 404 for nonexistent Appraisal ID | Pass / Fail |  |
| APP-SWGR-008 | Reject invalid GUID format | Pass / Fail |  |
| APP-SWGR-009 | Reject invalid status | Pass / Fail / N/A |  |
| APP-SWGR-010 | Reject update for nonexistent Appraisal ID | Pass / Fail / N/A |  |

---

# 12. Final QA Signoff

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

# 13. Expected Next Workflow

After Appraisal API tests pass, continue with:

```text
POST /api/inspections
PUT  /api/inspections/{id}/scores
GET  /api/inspections/{id}
```

This validates the next workflow step:

```text
Vehicle → Appraisal → Inspection
```
