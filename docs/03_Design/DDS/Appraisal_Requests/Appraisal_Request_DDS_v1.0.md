# ONE CLASSICS NEXUS
## Detailed Design Specification
### Appraisal Request Entity
### Version 1.0 — Appraisal-Centric / USPAP Aligned

**Document Type:** Entity Detailed Design Specification  
**Parent DDS:** Nexus DDS v3.0 — Appraisal-Centric Platform Architecture  
**System:** `ONE_Classics_Nexus`  
**Target Platform:** .NET 8 / ASP.NET Core / WinForms / MariaDB  
**Architecture:** Layered / Clean Architecture  
**Entity:** `AppraisalRequest`  
**Physical Table:** `appraisal_requests`  
**Status:** Proposed Implementation Baseline

---

# 1. PURPOSE

The `AppraisalRequest` entity represents the **pre-appraisal intake record** through which a prospective appraisal engagement is captured, qualified, reviewed, and either:

- converted into a formal `Appraisal`;
- declined;
- cancelled;
- withdrawn; or
- otherwise closed without creating an appraisal.

The entity exists immediately upstream of the Appraisal within the Nexus appraisal-centric architecture:

```text
Client
   ↓
Appraisal Request
   ↓
Appraisal
   ↓
Assignment / Problem Identification
   ↓
Scope of Work
   ↓
Appraisal Development
```

The governing Nexus DDS explicitly places `Client / Appraisal Request` before the `Appraisal`, with the latter serving as the governing aggregate for appraisal development and reporting.

The USPAP-aligned SRS separately establishes the appraisal lifecycle beginning with Assignment and identifies the `Appraisal` as the central entity for USPAP workflow and data.

Accordingly:

> **An Appraisal Request is not itself an appraisal assignment and shall not be treated as the authoritative record of USPAP problem identification.**

Information entered during intake may become proposed assignment data, but such information must be reviewed and formally established within the resulting Appraisal.

---

# 2. DESIGN OBJECTIVES

The `AppraisalRequest` design shall:

1. capture sufficient intake information to determine whether ONE Classics will accept the requested work;
2. associate the request with a client or prospective client;
3. capture preliminary information concerning the subject vehicle;
4. capture the requester's stated purpose for seeking an appraisal;
5. capture requested timing, contact, location, and service information;
6. preserve intake information independently from later appraisal conclusions;
7. support qualification, review, assignment, decline, and conversion workflows;
8. prevent intake statements from automatically becoming approved appraisal assumptions or conclusions;
9. support traceability from request to resulting Appraisal;
10. prevent one request from silently creating multiple unrelated appraisals;
11. preserve the original request after conversion;
12. provide a clean handoff into Standard 7 problem identification;
13. support auditability and optimistic concurrency; and
14. fit the existing Nexus Domain → Application → Infrastructure → API → WinForms architecture.

---

# 3. DOMAIN BOUNDARY

## 3.1 Appraisal Request Versus Appraisal

The two entities serve different purposes.

| Appraisal Request | Appraisal |
|---|---|
| Intake record | Formal appraisal workflow |
| Captures requested service | Establishes assignment |
| May contain unverified client statements | Contains reviewed appraisal facts |
| May be incomplete | Subject to compliance gates |
| Can be declined | Proceeds through development |
| Does not contain final opinions | Owns appraisal conclusion workflow |
| Does not satisfy Standard 7 | Supports Standard 7 development |
| Mutable during intake | Increasingly controlled as appraisal progresses |

The distinction is required because Standard 7 problem identification requires the formal appraisal to establish the client, intended users, intended use, subject property, property interest, value definition, effective date, assignment conditions, and scope. 
---

# 4. AGGREGATE DESIGN

## 4.1 Aggregate Root

`AppraisalRequest` shall be an independent aggregate root.

```text
AppraisalRequest
    │
    ├── Client
    ├── ClientContact
    ├── Preliminary Subject Information
    ├── Requested Service
    ├── Requested Purpose
    ├── Requested Timing
    ├── Intake Notes
    ├── Qualification Decision
    └── Conversion Reference
             ↓
          Appraisal
```

It shall not be modeled as a child entity of `Appraisal`, because the request exists before an Appraisal is created.

## 4.2 Principal Relationships

```text
Client
  1
  │
  └──────────< AppraisalRequest
                       │
                       │ 0..1
                       ▼
                   Appraisal
                       │
                       ▼
                    Vehicle
```

An Appraisal Request may exist without a completed appraisal.

A converted Appraisal Request shall reference the resulting Appraisal.

---

# 5. ENTITY RESPONSIBILITIES

The entity shall own rules concerning:

- intake identity;
- request status;
- preliminary request details;
- client/requester association;
- preliminary subject identification;
- intake qualification;
- requested appraisal purpose;
- requested service/type;
- preferred schedule;
- conversion eligibility;
- conversion tracking;
- cancellation;
- withdrawal;
- decline;
- request history;
- concurrency.

The entity shall **not** own:

- formal intended-user determination;
- formal intended-use approval;
- final value definition;
- approved effective date;
- scope of work;
- extraordinary assumptions;
- hypothetical conditions;
- appraisal evidence conclusions;
- valuation;
- reconciliation;
- final opinion of value;
- report generation.

Those belong to the Appraisal workflow.

---

# 6. DOMAIN ENTITY

Recommended Domain entity:

```csharp
public sealed class AppraisalRequest
{
    public Guid AppraisalRequestId { get; private set; }

    public string RequestNumber { get; private set; }

    public Guid ClientId { get; private set; }
    public Guid? ClientContactId { get; private set; }

    public Guid? VehicleId { get; private set; }
    public Guid? ConvertedAppraisalId { get; private set; }

    public string RequestStatus { get; private set; }
    public string RequestSource { get; private set; }

    public string RequestedAppraisalType { get; private set; }
    public string RequestedPurpose { get; private set; }

    public string? RequestedIntendedUseNarrative { get; private set; }
    public string? RequestedValueType { get; private set; }

    public DateTime? RequestedEffectiveDate { get; private set; }
    public DateTime? RequestedCompletionDate { get; private set; }

    public string? InspectionPreference { get; private set; }
    public string? InspectionLocation { get; private set; }

    public string? SubjectYear { get; private set; }
    public string? SubjectMake { get; private set; }
    public string? SubjectModel { get; private set; }
    public string? SubjectVin { get; private set; }

    public string? SubjectDescription { get; private set; }

    public string? ClientComments { get; private set; }
    public string? InternalNotes { get; private set; }

    public string? QualificationStatus { get; private set; }
    public string? QualificationNotes { get; private set; }

    public Guid? AssignedToUserId { get; private set; }

    public DateTime ReceivedAtUtc { get; private set; }
    public DateTime? ReviewedAtUtc { get; private set; }
    public DateTime? ConvertedAtUtc { get; private set; }
    public DateTime? ClosedAtUtc { get; private set; }

    public long RowVersion { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
}
```

---

# 7. IDENTIFIER DESIGN

## 7.1 Primary Identifier

```text
appraisal_request_id CHAR(36)
```

shall store a GUID.

The identifier shall:

- be system generated;
- be immutable;
- never be reused;
- remain valid after conversion or closure.

## 7.2 Human-Readable Request Number

A separate request number shall be generated.

Recommended format:

```text
AR-YYYY-NNNNNN
```

Example:

```text
AR-2026-000184
```

`request_number` shall have a unique database constraint.

Request numbers shall never be recycled.

---

# 8. REQUEST STATUS MODEL

Recommended canonical statuses:

```text
Draft
Submitted
UnderReview
NeedsInformation
Qualified
Accepted
Declined
Cancelled
Withdrawn
Converted
Closed
```

## 8.1 Status Definitions

### Draft

Request exists but has not yet been formally submitted.

### Submitted

Minimum intake data is present and the request is awaiting review.

### UnderReview

An internal user is actively evaluating the request.

### NeedsInformation

Additional information is required from the client/requester.

### Qualified

The request has sufficient information and appears suitable for acceptance.

### Accepted

ONE Classics has accepted the engagement request, but the formal Appraisal may not yet have been created.

### Declined

ONE Classics will not accept the requested engagement.

### Cancelled

The request was administratively cancelled.

### Withdrawn

The client/requester withdrew the request.

### Converted

A formal `Appraisal` has been created from the request.

### Closed

The request is complete and requires no further intake processing.

---

# 9. REQUEST STATE MACHINE

```text
Draft
  ↓
Submitted
  ↓
UnderReview
  ├──────────────→ NeedsInformation
  │                       │
  │                       └────→ UnderReview
  │
  ├──────────────→ Declined
  │
  ├──────────────→ Withdrawn
  │
  └──────────────→ Qualified
                           ↓
                       Accepted
                           ↓
                       Converted
                           ↓
                         Closed
```

Alternative terminal transitions:

```text
Draft → Cancelled
Submitted → Cancelled
UnderReview → Cancelled
Qualified → Cancelled
Accepted → Cancelled
```

`Converted` shall be treated as a business lock state for material intake facts.

---

# 10. STATUS TRANSITION RULES

Domain methods shall be used instead of directly setting `RequestStatus`.

Recommended methods:

```csharp
Submit()
BeginReview()
RequestAdditionalInformation()
MarkQualified()
Accept()
Decline(string reason)
Withdraw(string reason)
Cancel(string reason)
Convert(Guid appraisalId)
Close()
```

Invalid transitions shall throw a domain exception.

Example:

```text
Converted → UnderReview
```

shall not be allowed.

---

# 11. MINIMUM SUBMISSION REQUIREMENTS

A request shall not transition from `Draft` to `Submitted` unless it contains at minimum:

- client;
- contact mechanism or client contact;
- requested appraisal/service type;
- requested purpose or reason for appraisal;
- sufficient preliminary subject information to distinguish the vehicle/property;
- requester acknowledgment that submitted information may require verification.

For a collector vehicle, preliminary subject identification should normally include some combination of:

```text
VIN
year
make
model
series
trim
body style
subject description
```

The formal appraisal will later require sufficient subject identification and value-relevant characteristics.

---

# 12. PRELIMINARY SUBJECT INFORMATION

## 12.1 Design Principle

Appraisal Request subject information is **preliminary**.

It shall not automatically overwrite the `Vehicle` master record.

Example:

```text
Request says:
1969 Camaro Z/28
VIN XYZ...

Existing Vehicle says:
1969 Camaro SS
VIN XYZ...
```

The system shall surface the conflict for review rather than silently updating the Vehicle.

## 12.2 Vehicle Link

`VehicleId` shall be optional.

Three intake scenarios shall be supported:

```text
Known Vehicle
    → Link existing VehicleId

New Vehicle
    → Store preliminary request subject information
    → Vehicle created during qualification/conversion

Unknown / incomplete identity
    → Store available preliminary description
    → Resolve before Appraisal Assignment Ready
```

---

# 13. CLIENT DESIGN

Each submitted request shall be associated with a `Client`.

Optional:

```text
ClientContactId
```

shall identify the specific person handling the request.

The Appraisal Request may capture statements supplied by the client, but the resulting Appraisal must still establish the formal client for the assignment.

The USPAP requirements specifically require the Appraisal to identify the responsible client and distinguish that client from intended users, owner, seller, buyer, recipients, and other participants.

---

# 14. REQUESTED PURPOSE VERSUS INTENDED USE

This distinction shall be explicit.

The request may store:

```text
RequestedPurpose
RequestedIntendedUseNarrative
```

Examples:

```text
"Need value for insurance."
"Estate attorney asked for appraisal."
"Considering sale."
"Need documentation for divorce proceedings."
```

These values are **client-supplied intake statements**.

They do not automatically become the formal `AppraisalIntendedUse`.

During conversion:

```text
Requested Purpose
       ↓
Proposed Intended Use
       ↓
Appraiser Review
       ↓
Established Intended Use
```

USPAP-aligned requirements require the Appraisal itself to establish the intended use.

---

# 15. REQUESTED VALUE TYPE

A request may optionally capture a client-stated requested value concept such as:

```text
insurance value
fair market value
replacement value
market value
```

This field shall be treated as a **request**, not the authoritative appraisal value definition.

Formal value definition requires:

- value type;
- definition;
- source;
- jurisdiction where applicable;
- citation/reference; and
- supplemental narrative.

Therefore:

```text
AppraisalRequest.RequestedValueType
```

shall never be directly substituted for:

```text
AppraisalValueDefinition
```

without appraiser review.

---

# 16. REQUESTED EFFECTIVE DATE

The Appraisal Request may capture:

```text
RequestedEffectiveDate
```

Examples:

- current date;
- date of loss;
- date of death;
- historical transaction date;
- litigation-relevant date.

It shall remain provisional.

The resulting Appraisal must formally establish its own effective date, because the SRS requires the Appraisal to maintain the effective date independently from engagement, inspection, valuation, report, certification, and delivery dates.

---

# 17. INSPECTION REQUEST DATA

The request may capture a client's desired inspection arrangement.

Recommended values:

```text
Physical
Remote
ThirdParty
RecordsOnly
Undetermined
```

Optional:

```text
inspection_location
preferred_inspection_date
inspection_notes
```

This is scheduling/intake information only.

The actual Appraisal shall independently identify the inspection extent relied upon, which may be physical, limited physical, remote, third-party, records-only, or no inspection.

---

# 18. QUALIFICATION DESIGN

The intake process shall support a distinct qualification decision.

Recommended fields:

```text
qualification_status
qualification_notes
qualified_by_user_id
qualified_at_utc
```

Values:

```text
NotReviewed
PendingInformation
Qualified
NotQualified
ConflictReview
```

Qualification may consider operational matters such as:

- sufficient subject information;
- service type supported;
- geographic feasibility;
- timing;
- available appraiser;
- requested use;
- required expertise;
- possible conflicts;
- availability of evidence;
- condition/access considerations.

The attached sources do not prescribe the business rules for engagement qualification; therefore the exact qualification criteria shall remain configurable business rules rather than being represented as USPAP requirements.

---

# 19. ASSIGNMENT TO INTERNAL USER

An Appraisal Request may be assigned to an internal user for intake review:

```text
assigned_to_user_id
```

This is an **intake assignment**.

It is not necessarily the same as:

```text
Appraisal.AssignedAppraiserId
```

Conversion shall allow either:

```text
carry proposed appraiser forward
```

or:

```text
select another responsible appraiser
```

before Assignment Ready validation.

---

# 20. CONVERSION TO APPRAISAL

## 20.1 Conversion Rule

Only a request in:

```text
Qualified
or
Accepted
```

shall normally be convertible.

Conversion shall be implemented through an application service.

Recommended interface:

```csharp
public interface IAppraisalRequestConversionService
{
    Task<ConvertAppraisalRequestResult> ConvertAsync(
        Guid appraisalRequestId,
        ConvertAppraisalRequestCommand command,
        CancellationToken cancellationToken);
}
```

## 20.2 Conversion Transaction

The operation shall be atomic:

```text
Load Appraisal Request
        ↓
Validate convertible status
        ↓
Resolve / create Vehicle
        ↓
Create Appraisal
        ↓
Associate Client
        ↓
Set AppraisalRequestId
        ↓
Copy proposed intake values
        ↓
Create initial assignment/problem-identification drafts
        ↓
Set ConvertedAppraisalId
        ↓
Set Request Status = Converted
        ↓
Write audit event
        ↓
COMMIT
```

If any step fails:

```text
ROLLBACK
```

No partially converted request shall remain.

---

# 21. CONVERSION MAPPING

Recommended mapping:

| Appraisal Request | Resulting Appraisal | Treatment |
|---|---|---|
| `AppraisalRequestId` | `Appraisal.AppraisalRequestId` | Direct |
| `ClientId` | `Appraisal.ClientId` | Direct, subject to review |
| `VehicleId` | `Appraisal.VehicleId` | Direct if resolved |
| requested type | appraisal type | Proposed/default |
| requested purpose | intended use draft | Requires confirmation |
| requested value type | value definition draft | Requires confirmation |
| requested effective date | effective date draft | Requires confirmation |
| subject details | Vehicle/snapshot seed | Requires verification |
| inspection preference | scope/inspection seed | Not authoritative |
| requester notes | intake history | Preserve verbatim |
| internal qualification notes | workfile/admin context | Do not report automatically |

---

# 22. CRITICAL CONVERSION PRINCIPLE

Conversion shall follow:

```text
Copy as Proposed
      ≠
Approve as Appraisal Fact
```

Specifically, conversion shall not automatically establish:

- intended users;
- intended use;
- property interest;
- value definition;
- effective date;
- extraordinary assumptions;
- hypothetical conditions;
- scope appropriateness;
- final subject characteristics.

Those remain Standard 7 appraisal-development responsibilities.

The formal Assignment Ready gate evaluates client, intended use, intended users, subject identification, property interest, value definition, effective-date handling, responsible appraiser, and scope of work.

---

# 23. DATABASE DESIGN

Recommended table:

```sql
CREATE TABLE appraisal_requests
(
    appraisal_request_id          CHAR(36)      NOT NULL,
    request_number                VARCHAR(30)   NOT NULL,

    client_id                     CHAR(36)      NOT NULL,
    client_contact_id             CHAR(36)      NULL,

    vehicle_id                    CHAR(36)      NULL,
    converted_appraisal_id        CHAR(36)      NULL,

    request_status                VARCHAR(30)   NOT NULL,
    request_source                VARCHAR(50)   NULL,

    requested_appraisal_type      VARCHAR(100)  NOT NULL,
    requested_purpose             VARCHAR(255)  NOT NULL,

    requested_intended_use        LONGTEXT      NULL,
    requested_value_type          VARCHAR(100)  NULL,

    requested_effective_date      DATE          NULL,
    requested_completion_date     DATE          NULL,

    inspection_preference         VARCHAR(50)   NULL,
    inspection_location           VARCHAR(500)  NULL,

    subject_year                  SMALLINT      NULL,
    subject_make                  VARCHAR(100)  NULL,
    subject_model                 VARCHAR(100)  NULL,
    subject_vin                   VARCHAR(50)   NULL,
    subject_description           LONGTEXT      NULL,

    client_comments               LONGTEXT      NULL,
    internal_notes                LONGTEXT      NULL,

    qualification_status          VARCHAR(30)   NOT NULL,
    qualification_notes           LONGTEXT      NULL,

    assigned_to_user_id           CHAR(36)      NULL,
    qualified_by_user_id          CHAR(36)      NULL,

    received_at_utc               DATETIME(6)   NOT NULL,
    reviewed_at_utc               DATETIME(6)   NULL,
    qualified_at_utc              DATETIME(6)   NULL,
    converted_at_utc              DATETIME(6)   NULL,
    closed_at_utc                 DATETIME(6)   NULL,

    row_version                   BIGINT        NOT NULL DEFAULT 1,

    created_at_utc                DATETIME(6)   NOT NULL,
    updated_at_utc                DATETIME(6)   NOT NULL,

    PRIMARY KEY (appraisal_request_id),

    UNIQUE KEY uq_appraisal_requests_number
        (request_number),

    UNIQUE KEY uq_appraisal_requests_converted_appraisal
        (converted_appraisal_id),

    KEY ix_appraisal_requests_client
        (client_id),

    KEY ix_appraisal_requests_status
        (request_status),

    KEY ix_appraisal_requests_assignee_status
        (assigned_to_user_id, request_status),

    KEY ix_appraisal_requests_vehicle
        (vehicle_id),

    KEY ix_appraisal_requests_received
        (received_at_utc)
);
```

---

# 24. FOREIGN KEYS

Recommended constraints:

```text
appraisal_requests.client_id
    → clients.client_id

appraisal_requests.client_contact_id
    → client_contacts.client_contact_id

appraisal_requests.vehicle_id
    → vehicles.vehicle_id

appraisal_requests.converted_appraisal_id
    → appraisals.appraisal_id

appraisal_requests.assigned_to_user_id
    → users.user_id

appraisal_requests.qualified_by_user_id
    → users.user_id
```

Deletion behavior should generally be restrictive.

Recommended:

```text
Client               RESTRICT
Vehicle              SET NULL only if legally safe before conversion
Converted Appraisal  RESTRICT
User                 SET NULL or preserve inactive user record
```

Hard deletion of converted requests shall not be supported.

---

# 25. ONE REQUEST → ONE APPRAISAL RULE

Default business invariant:

```text
AppraisalRequest
      0..1
       │
       ▼
    Appraisal
```

A converted request shall normally generate one Appraisal.

The unique constraint on:

```text
converted_appraisal_id
```

protects the reverse association.

The conversion service shall also prevent multiple Appraisals from being created from the same request.

If later business requirements permit multiple assignments from one inquiry, that shall require an explicit redesign such as:

```text
AppraisalRequest
      ↓
AppraisalEngagement
      ↓
Appraisal(s)
```

That relationship is **not established by the attached source documents** and is therefore outside this DDS.

---

# 26. REPOSITORY DESIGN

Application abstraction:

```csharp
public interface IAppraisalRequestRepository
{
    Task<AppraisalRequest?> GetByIdAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken);

    Task<AppraisalRequest?> GetByRequestNumberAsync(
        string requestNumber,
        CancellationToken cancellationToken);

    Task<PagedResult<AppraisalRequestSummary>> SearchAsync(
        AppraisalRequestSearchCriteria criteria,
        CancellationToken cancellationToken);

    Task InsertAsync(
        AppraisalRequest request,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        AppraisalRequest request,
        long expectedRowVersion,
        CancellationToken cancellationToken);
}
```

Infrastructure implementation:

```text
Infrastructure/
└── Persistence/
    └── Repositories/
        └── AppraisalRequestRepository.cs
```

Dapper shall remain the persistence mechanism consistent with the governing DDS architecture.

---

# 27. APPLICATION LAYER DESIGN

Recommended feature layout:

```text
Application/
└── AppraisalRequests/
    ├── Create/
    ├── Get/
    ├── Search/
    ├── Update/
    ├── Workflow/
    ├── Qualification/
    ├── Assignment/
    └── Conversion/
```

Recommended handlers/services:

```text
CreateAppraisalRequestHandler
GetAppraisalRequestHandler
SearchAppraisalRequestsHandler
UpdateAppraisalRequestHandler

SubmitAppraisalRequestHandler
BeginAppraisalRequestReviewHandler
RequestAdditionalInformationHandler
QualifyAppraisalRequestHandler
DeclineAppraisalRequestHandler
CancelAppraisalRequestHandler
WithdrawAppraisalRequestHandler

AssignAppraisalRequestHandler
ConvertAppraisalRequestHandler
```

---

# 28. CREATE REQUEST CONTRACT

```csharp
public sealed record CreateAppraisalRequestRequest
(
    Guid ClientId,
    Guid? ClientContactId,

    string RequestedAppraisalType,
    string RequestedPurpose,

    string? RequestedIntendedUse,
    string? RequestedValueType,

    DateOnly? RequestedEffectiveDate,
    DateOnly? RequestedCompletionDate,

    string? InspectionPreference,
    string? InspectionLocation,

    Guid? VehicleId,

    int? SubjectYear,
    string? SubjectMake,
    string? SubjectModel,
    string? SubjectVin,
    string? SubjectDescription,

    string? ClientComments,
    string? RequestSource
);
```

Server-generated fields shall include:

```text
AppraisalRequestId
RequestNumber
RequestStatus
QualificationStatus
ReceivedAtUtc
RowVersion
CreatedAtUtc
UpdatedAtUtc
```

---

# 29. REQUEST RESPONSE CONTRACT

```csharp
public sealed record AppraisalRequestResponse
(
    Guid AppraisalRequestId,
    string RequestNumber,

    Guid ClientId,
    Guid? ClientContactId,

    Guid? VehicleId,
    Guid? ConvertedAppraisalId,

    string RequestStatus,
    string? RequestSource,

    string RequestedAppraisalType,
    string RequestedPurpose,

    string? RequestedIntendedUse,
    string? RequestedValueType,

    DateOnly? RequestedEffectiveDate,
    DateOnly? RequestedCompletionDate,

    string? InspectionPreference,
    string? InspectionLocation,

    int? SubjectYear,
    string? SubjectMake,
    string? SubjectModel,
    string? SubjectVin,
    string? SubjectDescription,

    string? ClientComments,

    string QualificationStatus,
    Guid? AssignedToUserId,

    DateTime ReceivedAtUtc,
    DateTime? ConvertedAtUtc,

    long RowVersion
);
```

Internal notes should only be included in internal/admin contracts.

---

# 30. API DESIGN

Base resource:

```text
/api/appraisal-requests
```

## 30.1 Create

```http
POST /api/appraisal-requests
```

## 30.2 Retrieve

```http
GET /api/appraisal-requests/{appraisalRequestId}
```

## 30.3 Search

```http
GET /api/appraisal-requests
```

Filters may include:

```text
status
clientId
assignedToUserId
receivedFrom
receivedTo
requestNumber
VIN
make
model
```

## 30.4 Update Draft/Intake Data

```http
PUT /api/appraisal-requests/{appraisalRequestId}
```

## 30.5 Workflow

```http
POST /api/appraisal-requests/{id}/submit
POST /api/appraisal-requests/{id}/begin-review
POST /api/appraisal-requests/{id}/needs-information
POST /api/appraisal-requests/{id}/qualify
POST /api/appraisal-requests/{id}/accept
POST /api/appraisal-requests/{id}/decline
POST /api/appraisal-requests/{id}/cancel
POST /api/appraisal-requests/{id}/withdraw
```

## 30.6 Conversion

```http
POST /api/appraisal-requests/{id}/convert
```

---

# 31. CONVERSION API RESPONSE

Successful conversion:

```json
{
  "appraisalRequestId": "guid",
  "requestNumber": "AR-2026-000184",
  "requestStatus": "Converted",
  "appraisalId": "guid",
  "appraisalNumber": "APP-2026-000092"
}
```

Recommended response:

```http
201 Created
```

with the resulting Appraisal resource location.

---

# 32. ERROR CONTRACT

Domain/application errors shall use the existing Nexus `ProblemDetails` strategy.

Recommended codes:

```text
APPRAISAL_REQUEST_NOT_FOUND
APPRAISAL_REQUEST_INVALID_STATE
APPRAISAL_REQUEST_ALREADY_CONVERTED
APPRAISAL_REQUEST_INCOMPLETE
APPRAISAL_REQUEST_NOT_QUALIFIED
APPRAISAL_REQUEST_CLIENT_REQUIRED
APPRAISAL_REQUEST_SUBJECT_REQUIRED
APPRAISAL_REQUEST_CONCURRENCY_CONFLICT
APPRAISAL_REQUEST_CONVERSION_FAILED
```

Example:

```json
{
  "title": "Appraisal request cannot be converted.",
  "status": 422,
  "code": "APPRAISAL_REQUEST_NOT_QUALIFIED",
  "detail": "The request must be qualified or accepted before conversion."
}
```

---

# 33. OPTIMISTIC CONCURRENCY

`row_version` shall be persisted.

Update:

```sql
UPDATE appraisal_requests
SET
    request_status = @RequestStatus,
    qualification_status = @QualificationStatus,
    updated_at_utc = @UpdatedAtUtc,
    row_version = row_version + 1
WHERE appraisal_request_id = @AppraisalRequestId
  AND row_version = @ExpectedRowVersion;
```

If:

```text
affected rows = 0
```

return:

```http
409 Conflict
```

This follows the persisted optimistic-concurrency strategy established by the parent DDS.

---

# 34. IMMUTABILITY AFTER CONVERSION

After `RequestStatus = Converted`:

These fields shall not normally be editable:

```text
client_id
vehicle_id
requested_appraisal_type
requested_purpose
requested_intended_use
requested_value_type
requested_effective_date
subject identity fields
converted_appraisal_id
```

Administrative annotations may still be appended where needed.

Correction of appraisal data shall occur in the Appraisal, not by rewriting the originating request.

This preserves:

```text
What the client originally requested
           ↓
What the appraiser ultimately established
```

as two distinguishable historical facts.

---

# 35. AUDIT DESIGN

Material Appraisal Request events shall generate audit records.

Recommended event types:

```text
AppraisalRequestCreated
AppraisalRequestSubmitted
AppraisalRequestReviewStarted
AppraisalRequestInformationRequested
AppraisalRequestUpdated
AppraisalRequestQualified
AppraisalRequestAccepted
AppraisalRequestDeclined
AppraisalRequestAssigned
AppraisalRequestWithdrawn
AppraisalRequestCancelled
AppraisalRequestConverted
AppraisalRequestClosed
```

Audit metadata should include:

```text
appraisal_request_id
actor_user_id
timestamp
action
prior_status
new_status
reason
correlation_id
```

Once converted, the conversion event shall also identify:

```text
appraisal_id
```

---

# 36. SECURITY AND AUTHORIZATION

Recommended permissions:

| Action | Intake/User | Appraiser | Reviewer | Admin |
|---|---:|---:|---:|---:|
| Create request | Yes | Yes | Yes | Yes |
| Edit draft request | Yes | Yes | Yes | Yes |
| Begin review | Conditional | Yes | Yes | Yes |
| Qualify | No/Conditional | Yes | Yes | Yes |
| Accept | No | Yes | Yes | Yes |
| Decline | No | Yes | Yes | Yes |
| Convert to appraisal | No | Yes | Yes | Yes |
| Edit converted request | No | No | No | Restricted |
| View internal notes | Conditional | Yes | Yes | Yes |

API authorization shall be authoritative.

UI visibility shall not replace API enforcement.

---

# 37. WINFORMS DESIGN

Recommended module:

```text
Appraisal Requests
```

Primary screen:

```text
AppraisalRequestListForm
```

Detail/workspace:

```text
AppraisalRequestForm
```

## 37.1 List View

Columns:

```text
Request #
Received
Client
Vehicle
Requested Type
Purpose
Status
Qualification
Assigned To
Requested Completion
```

Filters:

```text
Open Requests
My Requests
Needs Information
Qualified
Accepted
Converted
Declined
All
```

---

# 38. APPRAISAL REQUEST FORM

Suggested sections:

```text
┌───────────────────────────────────────────────────────┐
│ Appraisal Request AR-2026-000184                     │
│ Status: Under Review       Qualification: Pending    │
├───────────────────────────────────────────────────────┤
│ Client                                                │
│ Request Details                                       │
│ Subject Vehicle                                       │
│ Requested Purpose / Use                               │
│ Timing                                                │
│ Inspection Preference                                 │
│ Client Comments                                       │
│ Internal Review                                       │
│ Conversion                                            │
└───────────────────────────────────────────────────────┘
```

Primary actions:

```text
Save
Submit
Begin Review
Request Information
Qualify
Accept
Decline
Convert to Appraisal
Cancel
```

Buttons shall be enabled according to valid workflow transitions.

---

# 39. CONVERSION REVIEW UI

Before conversion, Nexus should display a review screen:

```text
REQUEST DATA                     APPRAISAL SEED

Client                ───────→   Client
Vehicle               ───────→   Subject Vehicle
Requested Type        ───────→   Proposed Appraisal Type
Requested Purpose     ───────→   Proposed Intended Use
Requested Value Type  ───────→   Proposed Value Definition
Requested Date        ───────→   Proposed Effective Date
Inspection Preference ───────→   Proposed Scope Input
```

Each proposed appraisal field should support:

```text
Accept
Modify
Leave Blank
```

for fields requiring professional determination.

---

# 40. STANDARD 7 HANDOFF

The Appraisal Request → Appraisal handoff shall populate the beginning of the Standard 7 workflow.

The SRS requires appraisal problem identification to ultimately establish:

- client;
- intended users;
- intended use;
- subject property;
- relevant property characteristics;
- property interest;
- type and definition of value;
- effective date;
- assignment conditions. 
The request may seed several of these.

It cannot finalize them.

---

# 41. ASSIGNMENT READY INTERACTION

Conversion does **not** mean:

```text
Assignment Ready
```

Instead:

```text
Appraisal Request
      ↓
Convert
      ↓
Appraisal / AssignmentDefinition
      ↓
Problem Identification
      ↓
Scope of Work
      ↓
Gate A Validation
      ↓
AssignmentReady
```

The parent DDS specifies that Gate A evaluates client, intended use, intended users, subject identification, property interest, value definition, effective-date handling, responsible appraiser, and scope of work.

Therefore:

```text
Converted ≠ AssignmentReady
```

This shall be a locked design rule.

---

# 42. VALIDATION RULES

## AR-VAL-001

`AppraisalRequestId` shall not be empty.

## AR-VAL-002

`RequestNumber` shall be unique.

## AR-VAL-003

Submitted requests shall have a Client.

## AR-VAL-004

Submitted requests shall have a nonblank Requested Appraisal Type.

## AR-VAL-005

Submitted requests shall have a nonblank Requested Purpose.

## AR-VAL-006

Submitted requests shall contain sufficient preliminary subject identification.

## AR-VAL-007

A converted request shall have `ConvertedAppraisalId`.

## AR-VAL-008

A non-converted request shall not normally have `ConvertedAppraisalId`.

## AR-VAL-009

A request shall not be converted more than once.

## AR-VAL-010

A Declined, Cancelled, or Withdrawn request shall not be converted without an explicit reopen workflow.

## AR-VAL-011

`RequestedEffectiveDate` shall remain distinguishable from `Appraisal.EffectiveDate`.

## AR-VAL-012

Client-provided statements shall not automatically become verified appraisal facts.

## AR-VAL-013

Internal qualification notes shall not automatically flow into client-facing reports.

## AR-VAL-014

Updates shall enforce optimistic concurrency.

---

# 43. DOMAIN INVARIANTS

The following invariants shall hold:

```text
RequestNumber is immutable after creation.

ConvertedAppraisalId may be assigned only once.

Converted request cannot return to ordinary intake workflow.

Request facts remain historically preserved after conversion.

Appraisal facts may differ from request facts.

Request does not contain appraisal conclusions.

Request does not satisfy Standard 7 completion requirements.

Conversion is transactional.

Every converted request remains traceable to its Appraisal.
```

---

# 44. TEST DESIGN

## 44.1 Domain Tests

```text
CreateRequest_WithValidInputs_Succeeds

CreateRequest_MissingClient_FailsSubmission

SubmitRequest_FromDraft_Succeeds

SubmitRequest_MissingPurpose_Fails

BeginReview_FromSubmitted_Succeeds

QualifyRequest_FromUnderReview_Succeeds

AcceptRequest_FromQualified_Succeeds

ConvertRequest_FromAccepted_Succeeds

ConvertRequest_FromDraft_Fails

ConvertRequest_Twice_Fails

DeclineRequest_FromUnderReview_Succeeds

ConvertedRequest_CannotReturnToReview

ConvertedRequest_PreservesOriginalIntakeData
```

## 44.2 Persistence Tests

```text
Insert_AppraisalRequest_PersistsAllFields

RequestNumber_IsUnique

ConvertedAppraisalId_IsUnique

Update_WithCorrectRowVersion_Succeeds

Update_WithStaleRowVersion_Fails

Search_ByClient_ReturnsExpectedRows

Search_ByStatus_ReturnsExpectedRows
```

## 44.3 API Tests

```text
POST_AppraisalRequest_Returns201

GET_AppraisalRequest_Returns200

GET_UnknownRequest_Returns404

PUT_WithStaleRowVersion_Returns409

POST_Submit_ValidRequest_Returns200

POST_Qualify_InvalidState_Returns422

POST_Convert_QualifiedRequest_Returns201

POST_Convert_AlreadyConverted_Returns422
```

## 44.4 Conversion Integration Test

Golden workflow:

```text
Create Client
      ↓
Create Appraisal Request
      ↓
Submit
      ↓
Review
      ↓
Qualify
      ↓
Accept
      ↓
Convert
      ↓
Verify Appraisal created
      ↓
Verify AppraisalRequestId copied
      ↓
Verify Client linked
      ↓
Verify Vehicle linked/created
      ↓
Verify requested values copied only as proposed data
      ↓
Verify request marked Converted
      ↓
Verify audit event
```

---

# 45. TRACEABILITY MATRIX

| DDS Requirement Area | Source Requirement / Architecture | Domain | Data | Service | API | UI | Test |
|---|---|---|---|---|---|---|---|
| Request precedes Appraisal | Parent DDS appraisal hierarchy | `AppraisalRequest` | `appraisal_requests` | RequestService | `/appraisal-requests` | Request Workspace | Creation tests |
| Client capture | S7-REQ-001 precursor | ClientId | client_id | RequestService | POST request | Client section | Missing-client test |
| Requested purpose | S7-REQ-003 precursor | RequestedPurpose | requested_purpose | RequestService | POST/PUT | Purpose section | Purpose validation |
| Preliminary subject | S7-REQ-004 precursor | Subject fields | subject_* | RequestService | POST/PUT | Vehicle section | Subject validation |
| Requested value type | S7-REQ-007 precursor | RequestedValueType | requested_value_type | RequestService | POST/PUT | Value section | Mapping test |
| Requested effective date | S7-REQ-008 precursor | RequestedEffectiveDate | requested_effective_date | RequestService | POST/PUT | Date field | Date mapping test |
| Conversion | Parent DDS appraisal flow | Convert() | converted_appraisal_id | ConversionService | `/convert` | Convert action | Conversion integration |
| Historical trace | CROSS requirements architecture | immutable request | audit linkage | AuditService | internal | History | Audit test |
| Assignment gate separation | Parent DDS Gate A | status rule | request/appraisal link | ConversionService | `/convert` | Review screen | Converted-not-ready test |

---

# 46. REQUIREMENT CLASSIFICATION

The Appraisal Request entity is primarily a **Nexus intake/domain design requirement** rather than a direct USPAP reporting entity.

Its relationship to USPAP is transitional:

```text
Appraisal Request
      ↓
captures preliminary information
      ↓
Appraisal created
      ↓
appraiser establishes assignment facts
      ↓
Standard 7 requirements apply
```

USPAP requirements begin governing the formal appraisal problem identification once the Appraisal assignment is established.

The system shall therefore preserve the distinction between:

```text
Client requested
```

and:

```text
Appraiser identified / established / concluded
```

That distinction supports the broader Nexus traceability requirement connecting what was identified, relied upon, analyzed, calculated, reconciled, concluded, reported, and certified.

---

# 47. RECOMMENDED PROJECT STRUCTURE

```text
ONE_Classics_Nexus.Domain/
└── AppraisalRequests/
    ├── AppraisalRequest.cs
    ├── AppraisalRequestStatus.cs
    ├── AppraisalRequestQualificationStatus.cs
    └── AppraisalRequestErrors.cs

ONE_Classics_Nexus.Application/
└── AppraisalRequests/
    ├── Contracts/
    ├── Create/
    ├── Get/
    ├── Search/
    ├── Update/
    ├── Workflow/
    ├── Qualification/
    └── Conversion/

ONE_Classics_Nexus.Infrastructure/
└── Persistence/
    └── Repositories/
        └── AppraisalRequestRepository.cs

ONE_Classics_Nexus.API/
└── Controllers/
    └── AppraisalRequestsController.cs

ONE_Classics_Nexus.WinForms/
├── Forms/
│   └── AppraisalRequests/
│       ├── AppraisalRequestListForm.cs
│       └── AppraisalRequestForm.cs
└── Services/
    └── Api/
        └── AppraisalRequestApiClient.cs

ONE_Classics_Nexus.Tests/
└── AppraisalRequests/
    ├── Domain/
    ├── Application/
    ├── Infrastructure/
    └── Api/
```

---

# 48. IMPLEMENTATION SEQUENCE

Recommended implementation order:

```text
AR-1
Create domain entity + statuses + invariants

AR-2
Create appraisal_requests DDL

AR-3
Implement repository

AR-4
Create CRUD application handlers

AR-5
Implement workflow transitions

AR-6
Implement qualification workflow

AR-7
Create API controller/contracts

AR-8
Create Appraisal Request WinForms list/detail UI

AR-9
Implement Vehicle resolution

AR-10
Implement Appraisal conversion service

AR-11
Integrate Appraisal Assignment seed data

AR-12
Implement audit events

AR-13
Add RBAC

AR-14
Complete domain/API/integration tests
```

---

# 49. DESIGN DECISIONS

## DDS-AR-001 — Appraisal Request Is a Separate Aggregate

The request exists before the formal Appraisal and therefore shall not be embedded in the Appraisal aggregate.

## DDS-AR-002 — Request Information Is Preliminary

Information supplied during intake shall not automatically become an approved appraisal fact.

## DDS-AR-003 — Conversion Is Explicit

Creating an Appraisal from a Request shall require a deliberate conversion operation.

## DDS-AR-004 — Conversion Does Not Satisfy Gate A

A converted request creates an Appraisal in an assignment-definition state. The formal Assignment Ready gate must still be passed.

## DDS-AR-005 — Converted Requests Are Historically Preserved

The original request shall remain available after conversion and shall not be rewritten to match later appraisal conclusions.

## DDS-AR-006 — One Request Produces At Most One Appraisal Under This Design

Multiple Appraisals from one request are outside the relationship established by the attached design sources and require a future explicit model change.

## DDS-AR-007 — Requested Intended Use Is Not Established Intended Use

The appraiser must formally establish intended use during problem identification.

## DDS-AR-008 — Requested Value Type Is Not the Value Definition

The formal Appraisal must separately establish the type and definition of value.

## DDS-AR-009 — Requested Effective Date Is Provisional

The Appraisal owns the authoritative effective date.

## DDS-AR-010 — Appraisal Request Is Never a Source of Final Value

No valuation, reconciliation, final opinion, or report conclusion shall originate from the Appraisal Request entity.

---

# 50. FINAL ENTITY MODEL

```text
                         CLIENT
                           │
                           │ 1
                           ▼
                 ┌─────────────────────┐
                 │  APPRAISAL REQUEST  │
                 │                     │
                 │ Request Number      │
                 │ Requested Service   │
                 │ Requested Purpose   │
                 │ Preliminary Subject │
                 │ Requested Date      │
                 │ Intake Notes        │
                 │ Qualification       │
                 │ Status              │
                 └──────────┬──────────┘
                            │
                    qualify / accept
                            │
                            ▼
                       CONVERSION
                            │
            ┌───────────────┼────────────────┐
            │               │                │
            ▼               ▼                ▼
         CLIENT          VEHICLE         PROPOSED
          LINK             LINK        ASSIGNMENT DATA
            │               │                │
            └───────────────┼────────────────┘
                            ▼
                    ┌───────────────┐
                    │   APPRAISAL   │
                    │ Assignment    │
                    │ Definition    │
                    └───────┬───────┘
                            │
                            ▼
                STANDARD 7 PROBLEM
                   IDENTIFICATION
                            │
                            ▼
                   ASSIGNMENT READY
                         GATE A
```

---

# 51. ACCEPTANCE CRITERIA

The Appraisal Request implementation shall be considered complete when:

1. a request can be created independently of an Appraisal;
2. every submitted request is associated with a client;
3. preliminary vehicle information can be stored without corrupting the Vehicle master record;
4. requested purpose, value type, date, and inspection preferences are clearly identified as provisional;
5. request workflow transitions are domain-controlled;
6. qualification is distinct from appraisal development;
7. only eligible requests can be converted;
8. conversion atomically creates and links an Appraisal;
9. one request cannot be silently converted more than once;
10. converted request data remains historically preserved;
11. conversion seeds but does not approve Standard 7 problem-identification information;
12. the resulting Appraisal must independently pass Gate A;
13. optimistic concurrency prevents lost updates;
14. material actions generate audit history;
15. API, Domain, persistence, UI, and tests trace to this DDS; and
16. no Appraisal Request field can substitute directly for an approved valuation, final opinion, or report conclusion.

---

# 52. GOVERNING DESIGN STATEMENT

The `AppraisalRequest` shall serve as the controlled intake boundary between the prospective client's request and the professional appraisal process.

The governing lifecycle shall be:

```text
INQUIRY
   ↓
APPRAISAL REQUEST
   ↓
QUALIFICATION
   ↓
ACCEPTANCE
   ↓
CONVERSION
   ↓
APPRAISAL
   ↓
PROBLEM IDENTIFICATION
   ↓
SCOPE OF WORK
   ↓
ASSIGNMENT READY
   ↓
STANDARD 7 DEVELOPMENT
```

The entity's principal architectural responsibility is therefore to preserve:

```text
What was requested
        ↓
What was accepted
        ↓
What was converted
```

while the resulting `Appraisal` preserves:

```text
What was established
        ↓
What was relied upon
        ↓
What was analyzed
        ↓
What was concluded
        ↓
What was reported
```

This separation protects both intake history and appraisal integrity while fitting directly into the appraisal-centric ONE Classics Nexus architecture.

**End of Appraisal Request DDS v1.0**