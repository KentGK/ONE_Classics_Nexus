# ONE Classics Nexus  
# Inspection Test Plan v1.0

---

## 1. Document Control

| Item | Value |
|---|---|
| Document Name | Nexus Inspection Test Plan |
| Version | 1.0 |
| Area | 08_Testing_Quality_Inspection |
| Primary Table | `inspections` |
| Database | `one_classics_nexus_test` |
| Prepared For | ONE Classics Nexus |
| Purpose | Validate Inspection API, database persistence, workflow integrity, schema alignment, deletion behavior, section scoring, and red flag handling |

---

## 2. Purpose

This test plan defines the required validation coverage for the Nexus Inspection feature.

The plan is based on the current `inspections` table schema and expands Inspection QA coverage to include:

- Inspection header creation, retrieval, update, and deletion
- Inspection workflow and status behavior
- Foreign key validation
- Inspection section scoring
- Inspection red flag handling
- Database persistence
- API and Swagger contract validation
- Integration and regression coverage

---

## 3. Schema Under Test

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

---

## 4. Scope

### 4.1 In Scope

This test plan covers:

- Inspection record creation
- Inspection record retrieval
- Inspection record update
- Inspection record deletion
- Inspection workflow/status validation
- Required field validation
- Foreign key validation
- Status validation
- Inspection type validation
- Timestamp behavior
- Nullable field behavior
- Database persistence
- API response schema validation
- Error handling
- Referential integrity
- Index-supported query behavior
- Inspection section scoring
- Inspection red flags

### 4.2 Out of Scope

The following are not fully covered by this test plan unless separately added:

- Inspection item-level scoring beyond section-score validation
- Inspection media uploads
- Report generation
- Valuation engine execution
- Authentication provider implementation
- Full role/permission matrix beyond basic Inspection authorization tests

---

## 5. Assumptions

The Inspection API is expected to expose endpoints similar to:

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/inspections` | Create inspection |
| GET | `/api/inspections/{inspectionId}` | Get inspection by ID |
| PUT | `/api/inspections/{inspectionId}` | Update inspection header |
| DELETE | `/api/inspections/{inspectionId}` | Delete inspection |
| GET | `/api/appraisals/{appraisalId}/inspections` | Get inspections for appraisal |
| PUT | `/api/inspections/{inspectionId}/section-scores` | Save inspection section scores |
| GET | `/api/inspections/{inspectionId}/section-scores` | Get inspection section scores |
| PUT | `/api/inspections/{inspectionId}/red-flags` | Save inspection red flags |
| GET | `/api/inspections/{inspectionId}/red-flags` | Get inspection red flags |

If the implemented routes differ, the test IDs and expected behavior remain valid, but the endpoint paths should be adjusted.

---

## 6. Required Test Data

### 6.1 Parent Records Required

Because the `inspections` table contains foreign keys, test setup must include valid parent records.

| Table | Required? | Reason |
|---|---:|---|
| `appraisals` | Yes | Required by `appraisal_id` |
| `inspection_templates` | Yes | Required by `inspection_template_id` |
| `users` | Optional | Required only when `inspected_by_user_id` is supplied |
| `inspection_template_sections` | Yes for section scoring | Required to validate section-level scoring |
| `inspection_red_flags` or equivalent red flag table | Yes for red flag tests | Required to validate red flag behavior |

### 6.2 Suggested Test IDs

| Entity | Test ID |
|---|---|
| Appraisal | `11111111-1111-1111-1111-111111111111` |
| Inspection Template | `22222222-2222-2222-2222-222222222222` |
| User | `33333333-3333-3333-3333-333333333333` |
| Inspection | `44444444-4444-4444-4444-444444444444` |
| Inspection Section | `55555555-5555-5555-5555-555555555555` |
| Red Flag | `66666666-6666-6666-6666-666666666666` |

---

## 7. Recommended API Request Contracts

### 7.1 CreateInspectionRequest

```json
{
  "appraisalId": "11111111-1111-1111-1111-111111111111",
  "inspectionTemplateId": "22222222-2222-2222-2222-222222222222",
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "Draft",
  "inspectionLocation": "Meridian, Idaho",
  "inspectedOnUtc": "2026-06-12T15:30:00.000000Z",
  "summaryNotes": "Initial inspection record created for QA validation."
}
```

### 7.2 UpdateInspectionRequest

```json
{
  "inspectedByUserId": "33333333-3333-3333-3333-333333333333",
  "inspectionType": "Standard",
  "status": "InProgress",
  "inspectionLocation": "Meridian, Idaho - Field Inspection",
  "inspectedOnUtc": "2026-06-12T16:15:00.000000Z",
  "summaryNotes": "Inspection moved to in-progress status after field review started."
}
```

### 7.3 InspectionResponse

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
  "summaryNotes": "Initial inspection record created for QA validation.",
  "createdAtUtc": "2026-06-12T15:30:00.000000Z",
  "updatedAtUtc": "2026-06-12T15:30:00.000000Z"
}
```

### 7.4 SectionScoreRequest

```json
{
  "sectionScores": [
    {
      "sectionNumber": 1,
      "sectionKey": "body_exterior",
      "sectionName": "Body & Exterior",
      "score0To5": 4.25,
      "weight": 0.1200,
      "notes": "Minor paint defects observed; no structural body concerns."
    },
    {
      "sectionNumber": 2,
      "sectionKey": "frame_structure",
      "sectionName": "Frame & Structure",
      "score0To5": 4.75,
      "weight": 0.0900,
      "notes": "Frame appears sound with no visible deformation."
    }
  ]
}
```

### 7.5 RedFlagRequest

```json
{
  "redFlags": [
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "redFlagCode": "FRAME_DAMAGE",
      "severity": "High",
      "isActive": true,
      "notes": "Evidence of prior frame repair requires review."
    }
  ]
}
```

---

## 8. Inspection Type Validation

The database column is `varchar(30)` and does not enforce values by constraint. Therefore, the API/domain layer must validate allowed inspection types.

Recommended allowed values:

| Value | Description |
|---|---|
| `Standard` | Standard vehicle inspection |
| `Custom` | Modified or custom vehicle inspection |
| `VintageClassic` | Vintage or classic vehicle inspection |

### Required Behavior

Invalid inspection types should be rejected by the API with `400 Bad Request`.

---

## 9. Inspection Status Validation

The database column is `varchar(30)` and does not enforce values by constraint. Therefore, the API/domain layer must validate allowed statuses.

Recommended allowed values:

| Status | Description |
|---|---|
| `Draft` | Inspection created but not started |
| `InProgress` | Inspection work has started |
| `Completed` | Inspection finalized |
| `Cancelled` | Inspection intentionally cancelled |

### Required Behavior

Invalid status values should be rejected by the API with `400 Bad Request`.

---

## 10. Functional Test Cases

### 10.1 Create Inspection Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-POST-001 | Create valid Standard inspection | `201 Created`; inspection persisted |
| INSP-POST-002 | Create valid Custom inspection | `201 Created`; inspection persisted |
| INSP-POST-003 | Create valid VintageClassic inspection | `201 Created`; inspection persisted |
| INSP-POST-004 | Create inspection without optional user | `201 Created`; `inspected_by_user_id` is null |
| INSP-POST-005 | Create inspection without optional location | `201 Created`; `inspection_location` is null |
| INSP-POST-006 | Create inspection without optional inspected date | `201 Created`; `inspected_on_utc` is null |
| INSP-POST-007 | Create inspection with summary notes | `201 Created`; notes persisted |
| INSP-POST-008 | Create inspection with Unicode notes/location | `201 Created`; UTF-8 content persisted |

---

### 10.2 Required Field Validation Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-VAL-001 | Missing `appraisalId` | `400 Bad Request` |
| INSP-VAL-002 | Missing `inspectionTemplateId` | `400 Bad Request` |
| INSP-VAL-003 | Missing `inspectionType` | `400 Bad Request` |
| INSP-VAL-004 | Missing `status` | `400 Bad Request` |
| INSP-VAL-005 | Empty GUID for `appraisalId` | `400 Bad Request` |
| INSP-VAL-006 | Empty GUID for `inspectionTemplateId` | `400 Bad Request` |
| INSP-VAL-007 | Empty GUID for `inspectedByUserId` when supplied | `400 Bad Request` |
| INSP-VAL-008 | Blank `inspectionType` | `400 Bad Request` |
| INSP-VAL-009 | Blank `status` | `400 Bad Request` |

---

### 10.3 Foreign Key Validation Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-FK-001 | Create inspection with nonexistent `appraisalId` | `400 Bad Request` or `409 Conflict` |
| INSP-FK-002 | Create inspection with nonexistent `inspectionTemplateId` | `400 Bad Request` or `409 Conflict` |
| INSP-FK-003 | Create inspection with nonexistent `inspectedByUserId` | `400 Bad Request` or `409 Conflict` |
| INSP-FK-004 | Create inspection with valid appraisal/template and null user | Success |
| INSP-FK-005 | Delete parent appraisal with existing inspection | Database should reject unless cascade is intentionally configured |
| INSP-FK-006 | Delete parent template with existing inspection | Database should reject unless cascade is intentionally configured |
| INSP-FK-007 | Delete user referenced by inspection | Database should reject unless user deletion rules are handled separately |

---

### 10.4 Inspection Type Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-TYPE-001 | `inspectionType = Standard` | Success |
| INSP-TYPE-002 | `inspectionType = Custom` | Success |
| INSP-TYPE-003 | `inspectionType = VintageClassic` | Success |
| INSP-TYPE-004 | `inspectionType = Classic` | Reject unless alias is explicitly supported |
| INSP-TYPE-005 | `inspectionType = Vintage/Classic` | Reject unless alias is explicitly supported |
| INSP-TYPE-006 | `inspectionType = Unknown` | `400 Bad Request` |
| INSP-TYPE-007 | `inspectionType` longer than 30 characters | `400 Bad Request` |

---

### 10.5 Status Workflow Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-STAT-001 | Create inspection with `Draft` status | Success |
| INSP-STAT-002 | Update `Draft` to `InProgress` | Success |
| INSP-STAT-003 | Update `InProgress` to `Completed` | Success |
| INSP-STAT-004 | Update `Draft` to `Cancelled` | Success |
| INSP-STAT-005 | Update `Completed` back to `Draft` | Reject unless reopening is explicitly allowed |
| INSP-STAT-006 | Update `Cancelled` to `Completed` | Reject unless restoration is explicitly allowed |
| INSP-STAT-007 | Use invalid status `Submitted` | `400 Bad Request` |
| INSP-STAT-008 | Use status longer than 30 characters | `400 Bad Request` |

---

### 10.6 Get Inspection Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-GET-001 | Get existing inspection by ID | `200 OK`; full inspection returned |
| INSP-GET-002 | Get nonexistent inspection by ID | `404 Not Found` |
| INSP-GET-003 | Get inspection with invalid GUID format | `400 Bad Request` |
| INSP-GET-004 | Get inspections by appraisal ID | `200 OK`; list returned |
| INSP-GET-005 | Get inspections for appraisal with no inspections | `200 OK`; empty list |
| INSP-GET-006 | Get inspections for nonexistent appraisal | `404 Not Found` or empty list, depending on API design |

---

### 10.7 Update Inspection Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-PUT-001 | Update location | `200 OK` or `204 No Content`; DB updated |
| INSP-PUT-002 | Update status | Success; status persisted |
| INSP-PUT-003 | Update inspected date | Success; `inspected_on_utc` persisted |
| INSP-PUT-004 | Update summary notes | Success; notes persisted |
| INSP-PUT-005 | Update assigned inspector user | Success if user exists |
| INSP-PUT-006 | Update with null user | Success if unassignment is allowed |
| INSP-PUT-007 | Update nonexistent inspection | `404 Not Found` |
| INSP-PUT-008 | Update with invalid status | `400 Bad Request` |
| INSP-PUT-009 | Update with invalid inspection type | `400 Bad Request` |
| INSP-PUT-010 | Update with nonexistent user | `400 Bad Request` or `409 Conflict` |

---

### 10.8 Delete Inspection Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-DEL-001 | Delete existing Draft inspection | `204 No Content` or `200 OK`; inspection removed or marked deleted |
| INSP-DEL-002 | Delete nonexistent inspection | `404 Not Found` |
| INSP-DEL-003 | Delete inspection with invalid GUID format | `400 Bad Request` |
| INSP-DEL-004 | Delete InProgress inspection | Allowed only if business rules permit |
| INSP-DEL-005 | Delete Completed inspection | Reject unless administrative deletion is explicitly allowed |
| INSP-DEL-006 | Delete inspection with section scores | Child records handled according to delete strategy |
| INSP-DEL-007 | Delete inspection with red flags | Child records handled according to delete strategy |
| INSP-DEL-008 | Delete inspection and verify GET by ID | `404 Not Found` or deleted-state response, depending on strategy |
| INSP-DEL-009 | Delete inspection and verify appraisal inspection list | Deleted inspection no longer appears unless audit view is used |
| INSP-DEL-010 | Unauthorized user attempts delete | `401 Unauthorized` or `403 Forbidden` |

### Deletion Rule

The current `inspections` table does not include an `is_deleted` or `deleted_at_utc` column. Therefore, deletion must be one of the following:

| Strategy | Expected Behavior |
|---|---|
| Hard delete | Row is physically removed from `inspections` |
| Soft delete | Requires schema extension such as `is_deleted`, `deleted_at_utc`, and `deleted_by_user_id` |
| Restricted delete | Completed or child-linked inspections cannot be deleted |

For Nexus auditability, restricted delete or soft delete is preferred over unrestricted hard delete.

---

### 10.9 Inspection Section Scoring Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-SEC-SCORE-001 | Save valid section scores for inspection | Scores persisted successfully |
| INSP-SEC-SCORE-002 | Get section scores for inspection | `200 OK`; scores returned |
| INSP-SEC-SCORE-003 | Save score below 0 | `400 Bad Request` |
| INSP-SEC-SCORE-004 | Save score above 5 | `400 Bad Request` |
| INSP-SEC-SCORE-005 | Save decimal score within 0–5 | Success |
| INSP-SEC-SCORE-006 | Save score for nonexistent inspection | `404 Not Found` |
| INSP-SEC-SCORE-007 | Save score for section not in template | `400 Bad Request` |
| INSP-SEC-SCORE-008 | Save duplicate section score | Update existing score or reject based on repository rule |
| INSP-SEC-SCORE-009 | Save notes with section score | Notes persisted |
| INSP-SEC-SCORE-010 | Complete inspection with missing required section score | Reject completion |
| INSP-SEC-SCORE-011 | Complete inspection with all required section scores | Success |
| INSP-SEC-SCORE-012 | Validate section score contributes to inspection rollup | Calculated rollup matches expected value |

---

### 10.10 Inspection Red Flag Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-RF-001 | Add valid red flag to inspection | Red flag persisted |
| INSP-RF-002 | Get red flags for inspection | `200 OK`; red flags returned |
| INSP-RF-003 | Add red flag for nonexistent inspection | `404 Not Found` |
| INSP-RF-004 | Add red flag for invalid system number | `400 Bad Request` |
| INSP-RF-005 | Add red flag with invalid severity | `400 Bad Request` |
| INSP-RF-006 | Add duplicate active red flag | Reject or update existing flag based on repository rule |
| INSP-RF-007 | Deactivate red flag | Red flag no longer active |
| INSP-RF-008 | Complete inspection with active critical red flag | Manual review required |
| INSP-RF-009 | Red flag appears in inspection response summary | Response includes red flag indicator |
| INSP-RF-010 | Red flag affects downstream valuation readiness | Valuation readiness reflects active critical flag |
| INSP-RF-011 | Red flag notes support UTF-8 content | Notes persisted safely |
| INSP-RF-012 | Delete inspection with red flags | Red flags handled according to delete strategy |

---

## 11. Database Persistence Test Cases

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-DB-001 | Insert inspection through API | Row exists in `inspections` |
| INSP-DB-002 | Verify `inspection_id` is UUID char(36) | Valid UUID format |
| INSP-DB-003 | Verify `created_at_utc` default is populated | Non-null timestamp |
| INSP-DB-004 | Verify `updated_at_utc` default is populated | Non-null timestamp |
| INSP-DB-005 | Update inspection and verify `updated_at_utc` changes | Timestamp is greater than original |
| INSP-DB-006 | Verify `created_at_utc` does not change on update | Original timestamp preserved |
| INSP-DB-007 | Verify nullable fields allow null | Null values persisted correctly |
| INSP-DB-008 | Verify `inspection_location` max length | Reject or truncate prevention at API |
| INSP-DB-009 | Verify `inspection_type` max length | Reject at API before DB error |
| INSP-DB-010 | Verify `status` max length | Reject at API before DB error |
| INSP-DB-011 | Delete inspection and verify persistence result | Row removed or soft-delete fields updated |
| INSP-DB-012 | Delete inspection with child section scores | Referential integrity preserved |
| INSP-DB-013 | Delete inspection with red flags | Referential integrity preserved |

---

## 12. Index and Query Behavior Tests

| Test ID | Index | Scenario | Expected Result |
|---|---|---|---|
| INSP-IDX-001 | `idx_inspections_appraisal` | Query inspections by appraisal | Uses indexed lookup |
| INSP-IDX-002 | `idx_inspections_template` | Query inspections by template | Uses indexed lookup |
| INSP-IDX-003 | `idx_inspections_user` | Query inspections by inspector | Uses indexed lookup |
| INSP-IDX-004 | `idx_inspections_status` | Query inspections by status | Uses indexed lookup |
| INSP-IDX-005 | Primary key | Query inspection by ID | Uses primary key lookup |

---

## 13. Negative Test Cases

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-NEG-001 | Submit malformed JSON | `400 Bad Request` |
| INSP-NEG-002 | Submit invalid GUID format | `400 Bad Request` |
| INSP-NEG-003 | Submit SQL injection text in location | Value rejected or safely stored as text |
| INSP-NEG-004 | Submit script tags in notes | Value sanitized or safely encoded by UI |
| INSP-NEG-005 | Submit excessive notes text | API handles safely without failure |
| INSP-NEG-006 | Submit future `inspectedOnUtc` | Reject unless future scheduling is allowed |
| INSP-NEG-007 | Submit local time instead of UTC | Reject or normalize to UTC |
| INSP-NEG-008 | Submit duplicate `inspection_id` directly at DB layer | Primary key violation |
| INSP-NEG-009 | Delete completed inspection without authorization | `403 Forbidden` or business-rule rejection |
| INSP-NEG-010 | Save section score without inspection ID | `400 Bad Request` |
| INSP-NEG-011 | Save red flag without required red flag code | `400 Bad Request` |

---

## 14. Security and Authorization Tests

| Test ID | Scenario | Expected Result |
|---|---|---|
| INSP-SEC-001 | Unauthenticated user creates inspection | `401 Unauthorized` |
| INSP-SEC-002 | Unauthorized role creates inspection | `403 Forbidden` |
| INSP-SEC-003 | Authorized inspector creates inspection | Success |
| INSP-SEC-004 | Authorized admin updates inspection | Success |
| INSP-SEC-005 | Unauthorized user updates completed inspection | `403 Forbidden` |
| INSP-SEC-006 | User attempts to access another tenant/client inspection | Access denied if tenancy rules exist |
| INSP-SEC-007 | Unauthorized user deletes inspection | `403 Forbidden` |
| INSP-SEC-008 | Authorized admin deletes eligible inspection | Success |
| INSP-SEC-009 | Authorized inspector saves section scores | Success |
| INSP-SEC-010 | Unauthorized user saves red flags | `403 Forbidden` |

---

## 15. Swagger Validation Checklist

Use Swagger to manually validate the following:

| Step | Action | Expected Result |
|---|---|---|
| 1 | Open Swagger | Inspection endpoints are visible |
| 2 | Expand POST `/api/inspections` | Request schema matches create contract |
| 3 | Submit valid Standard inspection | `201 Created` |
| 4 | Copy returned `inspectionId` | ID is valid UUID |
| 5 | Execute GET by ID | Response matches created record |
| 6 | Execute PUT by ID | Update succeeds |
| 7 | Execute GET again | Updated values are returned |
| 8 | Submit invalid status | `400 Bad Request` |
| 9 | Submit invalid appraisal ID | Error returned |
| 10 | Submit null optional fields | API accepts valid nullable fields |
| 11 | Execute section score save | Scores persist |
| 12 | Execute red flag save | Red flags persist |
| 13 | Execute DELETE for eligible inspection | Inspection removed or marked deleted |
| 14 | Execute GET after DELETE | Deleted inspection is not returned as active |

---

## 16. Integration Test Coverage

Recommended xUnit integration test classes:

```text
Tests/
└── Integration/
    └── Inspections/
        ├── InspectionCreateTests.cs
        ├── InspectionGetTests.cs
        ├── InspectionUpdateTests.cs
        ├── InspectionDeleteTests.cs
        ├── InspectionValidationTests.cs
        ├── InspectionForeignKeyTests.cs
        ├── InspectionWorkflowTests.cs
        ├── InspectionSectionScoreTests.cs
        └── InspectionRedFlagTests.cs
```

### Required Integration Tests

| Test Class | Coverage |
|---|---|
| `InspectionCreateTests` | Valid POST scenarios |
| `InspectionGetTests` | GET by ID and appraisal |
| `InspectionUpdateTests` | PUT behavior |
| `InspectionDeleteTests` | DELETE behavior and referential integrity |
| `InspectionValidationTests` | Required fields, invalid values |
| `InspectionForeignKeyTests` | Appraisal/template/user FK validation |
| `InspectionWorkflowTests` | Status transitions |
| `InspectionSectionScoreTests` | Section score persistence and validation |
| `InspectionRedFlagTests` | Red flag persistence and validation |

---

## 17. Repository Test Coverage

The Inspection repository should be tested for:

| Test ID | Repository Method | Expected Result |
|---|---|---|
| INSP-REPO-001 | `CreateAsync` | Inserts row |
| INSP-REPO-002 | `GetByIdAsync` | Returns matching inspection |
| INSP-REPO-003 | `GetByAppraisalIdAsync` | Returns appraisal inspections |
| INSP-REPO-004 | `UpdateAsync` | Updates mutable fields |
| INSP-REPO-005 | `ExistsAsync` | Returns true for existing inspection |
| INSP-REPO-006 | `ExistsAsync` | Returns false for missing inspection |
| INSP-REPO-007 | `DeleteAsync` | Deletes or marks inspection deleted |
| INSP-REPO-008 | `GetSectionScoresAsync` | Returns inspection section scores |
| INSP-REPO-009 | `SaveSectionScoresAsync` | Persists section scores |
| INSP-REPO-010 | `GetRedFlagsAsync` | Returns inspection red flags |
| INSP-REPO-011 | `SaveRedFlagsAsync` | Persists red flags |

---

## 18. Data Quality Rules

The Inspection feature must enforce the following rules:

| Rule ID | Rule |
|---|---|
| INSP-DQ-001 | `inspection_id` must be generated by the system |
| INSP-DQ-002 | `appraisal_id` must reference an existing appraisal |
| INSP-DQ-003 | `inspection_template_id` must reference an existing template |
| INSP-DQ-004 | `inspected_by_user_id` must be null or reference an existing user |
| INSP-DQ-005 | `inspection_type` must be one of the approved Nexus inspection types |
| INSP-DQ-006 | `status` must be one of the approved Nexus inspection statuses |
| INSP-DQ-007 | `created_at_utc` must be system generated |
| INSP-DQ-008 | `updated_at_utc` must be system maintained |
| INSP-DQ-009 | Date/time values must be stored in UTC |
| INSP-DQ-010 | Text fields must safely support UTF-8 content |
| INSP-DQ-011 | Deleted inspections must not appear in active workflows |
| INSP-DQ-012 | Section scores must remain within the 0–5 Nexus scoring range |
| INSP-DQ-013 | Required inspection sections must be scored before completion |
| INSP-DQ-014 | Active critical red flags must be visible to downstream workflows |
| INSP-DQ-015 | Red flag severity values must be validated by the API/domain layer |

---

## 19. Defect Severity Guidance

| Severity | Example |
|---|---|
| Critical | Inspection cannot be created |
| Critical | FK validation allows orphan inspection records |
| Critical | Deleting an inspection corrupts child section score or red flag data |
| High | Invalid status accepted |
| High | Inspection updates wrong appraisal record |
| High | Completed inspection can be deleted without authorization |
| High | Critical red flag does not trigger manual review indicator |
| Medium | Optional fields mishandled |
| Medium | Timestamp not updated |
| Medium | Section score rollup is incorrect |
| Low | Error message wording issue |
| Low | Swagger example data incomplete |

---

## 20. Acceptance Criteria

The Inspection feature is considered test-complete when:

1. Valid inspections can be created for Standard, Custom, and VintageClassic types.
2. Inspections cannot be created without required appraisal and template references.
3. Invalid foreign keys are rejected.
4. Invalid status values are rejected.
5. Invalid inspection type values are rejected.
6. GET by inspection ID returns the correct inspection.
7. GET by appraisal ID returns only inspections for that appraisal.
8. PUT updates allowed inspection fields.
9. DELETE handles eligible inspections according to the approved deletion strategy.
10. Deleted inspections do not appear in active appraisal inspection workflows.
11. `created_at_utc` and `updated_at_utc` behave correctly.
12. Nullable fields persist correctly.
13. Inspection section scores can be saved, retrieved, validated, and used for completion readiness.
14. Section scores reject values outside the 0–5 Nexus scoring range.
15. Inspection red flags can be saved, retrieved, activated, deactivated, and surfaced to downstream workflows.
16. Active critical red flags trigger manual review or workflow-blocking behavior where required.
17. Swagger request and response schemas match implemented DTOs.
18. Integration tests pass against `one_classics_nexus_test`.
19. No orphan inspection records can exist in the database.
20. API errors return clear and consistent responses.
21. The feature remains aligned with the Nexus inspection workflow.

---

## 21. Recommended Next Test Artifacts

The following artifacts should be created next:

1. `Inspection_Swagger_Test_Data_v1.0.md`
2. `Inspection_API_Test_Cases_v1.0.md`
3. `Inspection_SQL_Seed_Data_v1.0.sql`
4. `Inspection_Integration_Test_Checklist_v1.0.md`
5. `Inspection_Debugging_Checklist_v1.0.md`
6. `Inspection_Delete_Test_Cases_v1.0.md`
7. `Inspection_Section_Score_Test_Cases_v1.0.md`
8. `Inspection_Red_Flag_Test_Cases_v1.0.md`

---

## 22. Summary

The `inspections` table is structurally sound and supports the Nexus inspection workflow through strong foreign key relationships to appraisals, templates, and users.

This regenerated v1.0 test plan expands Inspection QA coverage to include:

- Inspection record deletion
- Inspection section scoring
- Inspection red flags

The main QA risks are:

- `inspection_type` and `status` are unconstrained database `varchar` fields.
- The deletion strategy is not directly represented in the current `inspections` schema.
- Section scoring and red flag behavior require related child tables and API/domain validation.
- Critical red flags must remain visible to downstream valuation and manual review workflows.

Therefore, the API, domain, repository, and integration test layers must enforce the Nexus business rules for inspection lifecycle, scoring, red flags, and data integrity.
