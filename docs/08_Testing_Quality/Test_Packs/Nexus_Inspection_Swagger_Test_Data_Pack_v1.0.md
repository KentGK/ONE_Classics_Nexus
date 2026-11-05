# ONE Classics Nexus
# Inspection Swagger Test Data Pack v1.0

---

## 1. Document Control

| Item | Value |
|---|---|
| Document Name | Nexus Inspection Swagger Test Data Pack |
| Version | 1.0 |
| Area | 08_Testing_Quality_Inspection |
| Primary Table | `inspections` |
| Database | `one_classics_nexus_test` |
| Purpose | Provide Swagger-ready request payloads and expected responses for Inspection API testing |

---

## 2. Source Schema

This Swagger test data pack is based on the current Inspection SQL schema:

```sql
CREATE TABLE `inspections` (
  `inspection_id` char(36) NOT NULL,
  `appraisal_id` char(36) NOT NULL,
  `inspection_template_id` char(36) NOT NULL,
  `inspected_by_user_id` char(36) DEFAULT NULL,
  `inspection_type` varchar(30) NOT NULL,
  `status` varchar(30) NOT NULL,
  `inspection_location` varchar(255) DEFAULT NULL,
  `inspected_on_utc` datetime(6) DEFAULT NULL,
  `summary_notes` text DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`inspection_id`),
  KEY `idx_inspections_appraisal` (`appraisal_id`),
  KEY `idx_inspections_template` (`inspection_template_id`),
  KEY `idx_inspections_user` (`inspected_by_user_id`),
  KEY `idx_inspections_status` (`status`),
  CONSTRAINT `fk_inspections_appraisal` FOREIGN KEY (`appraisal_id`) REFERENCES `appraisals` (`appraisal_id`),
  CONSTRAINT `fk_inspections_template` FOREIGN KEY (`inspection_template_id`) REFERENCES `inspection_templates` (`inspection_template_id`),
  CONSTRAINT `fk_inspections_user` FOREIGN KEY (`inspected_by_user_id`) REFERENCES `users` (`user_id`)
);
```

---

## 3. Swagger Endpoint Assumptions

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/inspections` | Create inspection |
| GET | `/api/inspections/{inspectionId}` | Get inspection by ID |
| PUT | `/api/inspections/{inspectionId}` | Update inspection |
| DELETE | `/api/inspections/{inspectionId}` | Delete inspection if supported |
| GET | `/api/appraisals/{appraisalId}/inspections` | Get inspections by appraisal if supported |

If the implemented controller uses a different route name, keep the payloads and expected results but adjust the route.

---

## 4. Test Data IDs

Use these fixed IDs for repeatable Swagger testing.

### 4.1 Valid Parent Records

These records must exist before POST tests are executed.

| Entity | ID |
|---|---|
| Appraisal | `11111111-1111-1111-1111-111111111111` |
| Inspection Template | `22222222-2222-2222-2222-222222222222` |
| User / Inspector | `33333333-3333-3333-3333-333333333333` |

### 4.2 Invalid Foreign Key IDs

Use these to verify FK validation behavior.

| Entity | Invalid ID |
|---|---|
| Missing Appraisal | `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` |
| Missing Inspection Template | `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` |
| Missing User | `cccccccc-cccc-cccc-cccc-cccccccccccc` |

---

## 5. Recommended Allowed Values

The database uses `varchar(30)` for `inspection_type` and `status`, so the API/domain layer should validate allowed values.

### 5.1 Inspection Types

| Value | Purpose |
|---|---|
| `Standard` | Standard production vehicle inspection |
| `Custom` | Modified/custom vehicle inspection |
| `VintageClassic` | Vintage or classic vehicle inspection |

### 5.2 Inspection Statuses

| Value | Purpose |
|---|---|
| `Draft` | Created but not started |
| `InProgress` | Inspection work is underway |
| `Completed` | Inspection is complete |
| `Cancelled` | Inspection was cancelled |

---

# 6. POST `/api/inspections` Test Payloads

---

## 6.1 INSP-SW-POST-001 — Create Standard Draft Inspection

### Request Body

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Initial Standard inspection created from Swagger test data."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| `inspectionId` | Generated UUID |
| `appraisalId` | Matches request |
| `inspectionTemplateId` | Matches request |
| `inspectedByUserId` | Matches request |
| `inspectionType` | `Standard` |
| `status` | `Draft` |
| `createdAtUtc` | System generated |
| `updatedAtUtc` | System generated |

---

## 6.2 INSP-SW-POST-002 — Create Custom InProgress Inspection

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Custom",
  "status": "InProgress",
  "inspectionLocation": "Boise, Idaho - Client Garage",
  "inspectedOnUtc": "2026-06-12T16:00:00.000000Z",
  "summaryNotes": "Custom inspection started for modified vehicle validation."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| `inspectionType` | `Custom` |
| `status` | `InProgress` |

---

## 6.3 INSP-SW-POST-003 — Create VintageClassic Draft Inspection

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "VintageClassic",
  "status": "Draft",
  "inspectionLocation": "Nampa, Idaho",
  "inspectedOnUtc": "2026-06-12T17:15:00.000000Z",
  "summaryNotes": "Vintage/classic inspection created for collector vehicle workflow."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| `inspectionType` | `VintageClassic` |
| `status` | `Draft` |

---

## 6.4 INSP-SW-POST-004 — Create Inspection Without Optional User

The SQL schema allows `inspected_by_user_id` to be null.

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": null,
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T18:00:00.000000Z",
  "summaryNotes": "Inspection created before inspector assignment."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| `inspectedByUserId` | `null` |

---

## 6.5 INSP-SW-POST-005 — Create Minimal Valid Inspection

The SQL schema allows `inspected_by_user_id`, `inspection_location`, `inspected_on_utc`, and `summary_notes` to be null.

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectionType": "Standard",
  "status": "Draft"
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| Optional fields | Null or omitted in response |
| Timestamps | System generated |

---

## 6.6 INSP-SW-POST-006 — Create Inspection With Nullable Fields Explicitly Null

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": null,
  "inspectionType": "Custom",
  "status": "Draft",
  "inspectionLocation": null,
  "inspectedOnUtc": null,
  "summaryNotes": null
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| Nullable columns | Persist as `NULL` |

---

## 6.7 INSP-SW-POST-007 — Create Inspection With Unicode Text

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "VintageClassic",
  "status": "InProgress",
  "inspectionLocation": "Méridian, Idaho – Field Bay #2",
  "inspectedOnUtc": "2026-06-12T19:30:00.000000Z",
  "summaryNotes": "Unicode test: café, résumé, em dash —, and classic vehicle notes."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `201 Created` |
| Text encoding | UTF-8 content persists correctly |

---

# 7. GET `/api/inspections/{inspectionId}` Test Data

---

## 7.1 INSP-SW-GET-001 — Get Existing Inspection

### Path Parameter

```text
inspectionId = {inspectionId returned from POST}
```

### Expected Response

```json
{
  "inspectionId": "44444444-4444-4444-4444-444444444444",
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Initial Standard inspection created from Swagger test data.",
  "createdAtUtc": "2026-06-12T15:30:00.000000Z",
  "updatedAtUtc": "2026-06-12T15:30:00.000000Z"
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `200 OK` |
| Response Body | Full inspection response |
| Timestamps | Present |

---

## 7.2 INSP-SW-GET-002 — Get Missing Inspection

```text
inspectionId = dddddddd-dddd-dddd-dddd-dddddddddddd
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `404 Not Found` |
| Error Body | ProblemDetails |

---

## 7.3 INSP-SW-GET-003 — Invalid GUID Format

```text
inspectionId = not-a-guid
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error Body | ProblemDetails or route model validation error |

---

# 8. GET `/api/appraisals/{appraisalId}/inspections` Test Data

---

## 8.1 INSP-SW-GET-BY-APP-001 — Get Inspections By Appraisal

```text
appraisalId = 11111111-1111-1111-1111-111111111111
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `200 OK` |
| Response Body | Array of inspections |
| Rule | Only inspections for the requested appraisal are returned |

### Example Response

```json
[
  {
    "inspectionId": "44444444-4444-4444-4444-444444444444",
    "appraisalId": "11111111-1111-1111-1111-111111111111",
    "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
    "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
    "inspectionType": "Standard",
    "status": "Draft",
    "inspectionLocation": "Meridian, Idaho",
    "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
    "summaryNotes": "Initial Standard inspection created from Swagger test data.",
    "createdAtUtc": "2026-06-12T15:30:00.000000Z",
    "updatedAtUtc": "2026-06-12T15:30:00.000000Z"
  }
]
```

---

# 9. PUT `/api/inspections/{inspectionId}` Test Payloads

---

## 9.1 INSP-SW-PUT-001 — Update Draft To InProgress

### Path Parameter

```text
inspectionId = {inspectionId returned from POST}
```

### Request Body

```json
{
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "InProgress",
  "inspectionLocation": "Meridian, Idaho - Field Inspection",
  "inspectedOnUtc": "2026-06-12T20:00:00.000000Z",
  "summaryNotes": "Inspection moved to InProgress after field work started."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `200 OK` or `204 No Content` |
| `status` | `InProgress` |
| `updatedAtUtc` | Later than original value |

---

## 9.2 INSP-SW-PUT-002 — Update InProgress To Completed

```json
{
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Completed",
  "inspectionLocation": "Meridian, Idaho - Field Inspection",
  "inspectedOnUtc": "2026-06-12T21:15:00.000000Z",
  "summaryNotes": "Inspection completed. All required inspection sections reviewed."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `200 OK` or `204 No Content` |
| `status` | `Completed` |
| `updatedAtUtc` | Updated |

---

## 9.3 INSP-SW-PUT-003 — Update Optional User To Null

```json
{
  "inspectedByUserId": null,
  "inspectionType": "Custom",
  "status": "Draft",
  "inspectionLocation": "Boise, Idaho - Pending Assignment",
  "inspectedOnUtc": null,
  "summaryNotes": "Inspector unassigned for reassignment workflow."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `200 OK` or `204 No Content` |
| `inspectedByUserId` | `null` |

---

## 9.4 INSP-SW-PUT-004 — Cancel Inspection

```json
{
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "VintageClassic",
  "status": "Cancelled",
  "inspectionLocation": "Nampa, Idaho",
  "inspectedOnUtc": "2026-06-12T22:00:00.000000Z",
  "summaryNotes": "Inspection cancelled by appraiser due to unavailable vehicle access."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `200 OK` or `204 No Content` |
| `status` | `Cancelled` |

---

# 10. DELETE `/api/inspections/{inspectionId}` Test Data

The current SQL schema does not contain `is_deleted`, `deleted_at_utc`, or `deleted_by_user_id`. Therefore, Swagger deletion testing should confirm the implemented strategy.

---

## 10.1 INSP-SW-DEL-001 — Delete Eligible Draft Inspection

```text
inspectionId = {inspectionId returned from POST}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `204 No Content` or `200 OK` |
| Follow-up GET | `404 Not Found`, unless soft-delete strategy returns deleted metadata |
| Notes | If hard delete is used, row should be physically removed |

---

## 10.2 INSP-SW-DEL-002 — Delete Missing Inspection

```text
inspectionId = eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `404 Not Found` |
| Error Body | ProblemDetails |

---

## 10.3 INSP-SW-DEL-003 — Delete Completed Inspection

```text
inspectionId = {completedInspectionId}
```

### Expected Result

| Item | Expected |
|---|---|
| Preferred Result | `400 Bad Request` or `409 Conflict` |
| Reason | Completed inspections should normally be preserved for auditability |
| Alternate Result | `204 No Content` only if admin hard-delete is intentionally allowed |

---

# 11. Negative POST Test Payloads

---

## 11.1 INSP-SW-NEG-001 — Missing Appraisal ID

```json
{
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Missing appraisal ID test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | `appraisalId` is required |

---

## 11.2 INSP-SW-NEG-002 — Missing Inspection Template ID

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Missing template ID test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | `inspectionTemplateId` is required |

---

## 11.3 INSP-SW-NEG-003 — Missing Inspection Type

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Missing inspection type test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | `inspectionType` is required |

---

## 11.4 INSP-SW-NEG-004 — Missing Status

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Missing status test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | `status` is required |

---

## 11.5 INSP-SW-NEG-005 — Invalid Appraisal Foreign Key

```json
{
  "appraisalId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Invalid appraisal FK test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` or `409 Conflict` |
| Error | Appraisal does not exist |

---

## 11.6 INSP-SW-NEG-006 — Invalid Template Foreign Key

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Invalid inspection template FK test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` or `409 Conflict` |
| Error | Inspection template does not exist |

---

## 11.7 INSP-SW-NEG-007 — Invalid User Foreign Key

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "cccccccc-cccc-cccc-cccc-cccccccccccc",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Invalid inspector user FK test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` or `409 Conflict` |
| Error | User does not exist |

---

## 11.8 INSP-SW-NEG-008 — Invalid Inspection Type

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Classic",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Invalid inspection type test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | Invalid inspection type |

---

## 11.9 INSP-SW-NEG-009 — Invalid Status

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Submitted",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Invalid status test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | Invalid status |

---

## 11.10 INSP-SW-NEG-010 — Inspection Location Exceeds 255 Characters

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "This inspection location intentionally exceeds the database column size limit of 255 characters. This should be rejected by the API validation layer before it reaches MariaDB so that the user receives a clear validation message instead of a database truncation or persistence error from the repository layer.",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Location length validation test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | `inspectionLocation` exceeds max length |

---

## 11.11 INSP-SW-NEG-011 — Malformed Date

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "not-a-date",
  "summaryNotes": "Malformed date test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | Invalid date format |

---

## 11.12 INSP-SW-NEG-012 — Empty GUID Values

```json
{
  "appraisalId": "00000000-0000-0000-0000-000000000000",
  "inspectionTemplateId": "00000000-0000-0000-0000-000000000000",
  "inspectedByUserId": "00000000-0000-0000-0000-000000000000",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Empty GUID validation test."
}
```

### Expected Result

| Item | Expected |
|---|---|
| HTTP Status | `400 Bad Request` |
| Error | Empty GUID values are not allowed |

---

# 12. ProblemDetails Error Shape

Swagger should return consistent error responses for validation and business rule failures.

## 12.1 Example Validation Error

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "inspectionType": [
      "Inspection type is required."
    ],
    "status": [
      "Status is required."
    ]
  },
  "traceId": "00-00000000000000000000000000000000-0000000000000000-00"
}
```

## 12.2 Example Conflict Error

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.10",
  "title": "Inspection could not be created.",
  "status": 409,
  "detail": "The referenced appraisal, inspection template, or user does not exist.",
  "traceId": "00-00000000000000000000000000000000-0000000000000000-00"
}
```

---

# 13. Swagger Manual Test Script

Use this order to validate the Inspection API through Swagger.

| Step | Test ID | Action | Expected Result |
|---:|---|---|---|
| 1 | INSP-SW-POST-001 | POST valid Standard inspection | `201 Created` |
| 2 | INSP-SW-GET-001 | GET returned inspection ID | `200 OK` |
| 3 | INSP-SW-PUT-001 | PUT status to `InProgress` | `200 OK` or `204 No Content` |
| 4 | INSP-SW-GET-001 | GET again | Updated status returned |
| 5 | INSP-SW-PUT-002 | PUT status to `Completed` | Success if workflow allows |
| 6 | INSP-SW-POST-004 | POST with null user | Success |
| 7 | INSP-SW-POST-005 | POST minimal valid request | Success |
| 8 | INSP-SW-NEG-001 | POST missing appraisal ID | `400 Bad Request` |
| 9 | INSP-SW-NEG-005 | POST invalid appraisal FK | `400` or `409` |
| 10 | INSP-SW-NEG-008 | POST invalid inspection type | `400 Bad Request` |
| 11 | INSP-SW-NEG-009 | POST invalid status | `400 Bad Request` |
| 12 | INSP-SW-NEG-010 | POST oversized location | `400 Bad Request` |
| 13 | INSP-SW-DEL-001 | DELETE eligible draft inspection | `204 No Content` or approved strategy result |
| 14 | INSP-SW-GET-002 | GET deleted/missing inspection | `404 Not Found` |

---

# 14. SQL Verification Queries

Use these queries after Swagger requests to confirm database persistence.

## 14.1 Get Inspection By ID

```sql
SELECT
    inspection_id,
    appraisal_id,
    inspection_template_id,
    inspected_by_user_id,
    inspection_type,
    status,
    inspection_location,
    inspected_on_utc,
    summary_notes,
    created_at_utc,
    updated_at_utc
FROM inspections
WHERE inspection_id = 'REPLACE_WITH_INSPECTION_ID';
```

---

## 14.2 Get Inspections By Appraisal

```sql
SELECT
    inspection_id,
    appraisal_id,
    inspection_template_id,
    inspected_by_user_id,
    inspection_type,
    status,
    inspection_location,
    inspected_on_utc,
    created_at_utc,
    updated_at_utc
FROM inspections
WHERE appraisal_id = '11111111-1111-1111-1111-111111111111'
ORDER BY created_at_utc DESC;
```

---

## 14.3 Verify Status Index Scenario

```sql
SELECT
    inspection_id,
    appraisal_id,
    inspection_type,
    status,
    inspected_on_utc
FROM inspections
WHERE status = 'Draft'
ORDER BY created_at_utc DESC;
```

---

## 14.4 Verify Timestamp Update

```sql
SELECT
    inspection_id,
    created_at_utc,
    updated_at_utc,
    TIMESTAMPDIFF(MICROSECOND, created_at_utc, updated_at_utc) AS update_delta_microseconds
FROM inspections
WHERE inspection_id = 'REPLACE_WITH_INSPECTION_ID';
```

---

## 14.5 Verify No Orphaned Inspection Records

```sql
SELECT i.inspection_id
FROM inspections i
LEFT JOIN appraisals a
    ON i.appraisal_id = a.appraisal_id
LEFT JOIN inspection_templates t
    ON i.inspection_template_id = t.inspection_template_id
LEFT JOIN users u
    ON i.inspected_by_user_id = u.user_id
WHERE a.appraisal_id IS NULL
   OR t.inspection_template_id IS NULL
   OR (
        i.inspected_by_user_id IS NOT NULL
        AND u.user_id IS NULL
   );
```

Expected result:

```text
No rows returned.
```

---

# 15. Field Mapping

| JSON Property | DB Column | Required? | Nullable? | Notes |
|---|---|---:|---:|---|
| `inspectionId` | `inspection_id` | Response only | No | Generated by system |
| `appraisalId` | `appraisal_id` | Yes | No | Must exist in `appraisals` |
| `inspectionTemplateId` | `inspection_template_id` | Yes | No | Must exist in `inspection_templates` |
| `inspectedByUserId` | `inspected_by_user_id` | No | Yes | Must exist in `users` when supplied |
| `inspectionType` | `inspection_type` | Yes | No | Max 30 chars |
| `status` | `status` | Yes | No | Max 30 chars |
| `inspectionLocation` | `inspection_location` | No | Yes | Max 255 chars |
| `inspectedOnUtc` | `inspected_on_utc` | No | Yes | UTC datetime |
| `summaryNotes` | `summary_notes` | No | Yes | Text |
| `createdAtUtc` | `created_at_utc` | Response only | No | DB default |
| `updatedAtUtc` | `updated_at_utc` | Response only | No | DB default/on update |

---

# 16. Pass/Fail Rules

| Rule ID | Rule |
|---|---|
| SW-INSP-RULE-001 | Swagger POST must not require `inspectionId` from the caller |
| SW-INSP-RULE-002 | Swagger POST must require `appraisalId` |
| SW-INSP-RULE-003 | Swagger POST must require `inspectionTemplateId` |
| SW-INSP-RULE-004 | Swagger POST must require `inspectionType` |
| SW-INSP-RULE-005 | Swagger POST must require `status` |
| SW-INSP-RULE-006 | Swagger POST must allow `inspectedByUserId` to be null |
| SW-INSP-RULE-007 | Swagger POST must allow `inspectionLocation` to be null |
| SW-INSP-RULE-008 | Swagger POST must allow `inspectedOnUtc` to be null |
| SW-INSP-RULE-009 | Swagger POST must allow `summaryNotes` to be null |
| SW-INSP-RULE-010 | Swagger response must include `inspectionId` |
| SW-INSP-RULE-011 | Swagger response must include `createdAtUtc` |
| SW-INSP-RULE-012 | Swagger response must include `updatedAtUtc` |
| SW-INSP-RULE-013 | Invalid foreign keys must not create rows |
| SW-INSP-RULE-014 | Invalid status values must not create rows |
| SW-INSP-RULE-015 | Invalid inspection type values must not create rows |
| SW-INSP-RULE-016 | API should return ProblemDetails for validation/business errors |

---

# 17. Summary

This Swagger Test Data Pack validates the Inspection API directly against the `inspections` SQL schema.

The highest-value checks are:

1. Required foreign keys are enforced.
2. Nullable fields behave correctly.
3. `inspection_type` and `status` are validated by the API/domain layer.
4. Timestamps are generated and maintained by the database.
5. Swagger request and response DTOs match the database contract.
6. Invalid requests return predictable ProblemDetails responses.
7. Deletion behavior is confirmed against the selected Nexus audit strategy.

This pack is ready for manual Swagger validation and can also be converted into integration test fixtures.
