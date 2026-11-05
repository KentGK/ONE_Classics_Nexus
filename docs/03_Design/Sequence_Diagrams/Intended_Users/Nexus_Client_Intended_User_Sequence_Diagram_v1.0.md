# ONE Classics Nexus

## Client / Intended User Sequence Diagram

**Version:** 1.0  
**Source:** `Client_Intended_User_DDS_v2.0.md` and supporting MariaDB table definitions  
**Scope:** Client selection, primary contact selection, proposed intended use, proposed intended users, validation, submission, acceptance, and conversion to an Appraisal

## Sequence Diagram

```mermaid
sequenceDiagram
    autonumber
    actor Staff as Intake Staff / Appraiser
    participant UI as WinForms Workspace
    participant Svc as API + Application Service
    participant DB as MariaDB Repositories
    participant App as Appraisal Workflow

    Staff->>UI: Open Client / Intended User workspace
    UI->>Svc: GET party-identification workspace
    Svc->>DB: Load request, client/contact, reference data,<br/>proposed use/users, status, and row versions
    DB-->>Svc: Current aggregate and selectable options
    Svc-->>UI: Workspace model + validation state

    rect rgb(238, 244, 249)
        Note over Staff,DB: Establish the engaging client and primary contact
        Staff->>UI: Search for and select client
        UI->>Svc: SetAppraisalRequestClient(clientId, expectedRowVersion)
        Svc->>DB: Validate active client and row version
        alt Valid client and current version
            Svc->>DB: Link client, clear incompatible contact,<br/>refresh draft client snapshot, increment row version
            Svc->>DB: Write client-change audit event
            DB-->>Svc: Updated request
            Svc-->>UI: Client selected; ask whether client<br/>is a proposed intended user
        else Stale row version or invalid client
            Svc-->>UI: 409 concurrency conflict or 422 validation error
        end

        opt Select a primary client contact
            Staff->>UI: Select active contact
            UI->>Svc: SetPrimaryClientContact(contactId, expectedRowVersion)
            Svc->>DB: Verify contact belongs to selected client
            alt Contact is eligible
                Svc->>DB: Link contact, stage contact snapshot,<br/>increment row version, write audit event
                DB-->>Svc: Updated request
                Svc-->>UI: Contact selected
            else Contact is inactive or belongs to another client
                Svc-->>UI: 422 CIU-002 / CIU-010
            end
        end
    end

    rect rgb(246, 243, 235)
        Note over Staff,DB: Define proposed intended use and intended users
        Staff->>UI: Select intended-use category and definition
        UI->>Svc: SetProposedIntendedUse(useId, narrative,<br/>expectedRowVersion)
        Svc->>DB: Verify active/effective definition and category
        alt Custom narrative required but missing
            Svc-->>UI: 422 CIU-007
        else Proposed use is valid
            Svc->>DB: Link definition, stage name/description/category snapshot,<br/>increment row version, write audit event
            DB-->>Svc: Updated request
            Svc-->>UI: Proposed intended use saved
        end

        alt Client is a proposed intended user
            Staff->>UI: Choose Add client
            UI->>Svc: AddClientAsProposedIntendedUser(expectedRowVersion)
            Svc->>DB: Upsert single client-marked request snapshot
            Svc->>DB: Enforce unique client marker and relationship = Client
            DB-->>Svc: Proposed intended-user row
            Svc-->>UI: Client shown in proposed-user grid
        else Client is not a proposed intended user
            Staff->>UI: Choose No and provide explanation
            UI->>Svc: Save exclusion explanation
            Svc-->>UI: Require at least one additional proposed user
        end

        loop For each additional proposed intended user
            Staff->>UI: Enter identity, type, relationship,<br/>source links, and use restrictions
            UI->>Svc: AddProposedIntendedUser(data, expectedRowVersion)
            Svc->>DB: Validate type, identity, relationship,<br/>source consistency, duplicates, and restrictions
            alt Blocking validation error
                Svc-->>UI: 422 with stable CIU validation codes
            else Valid or warning-only
                Svc->>DB: Insert request-owned snapshot and audit event
                DB-->>Svc: Intended-user row + row version
                Svc-->>UI: Refresh proposed-user grid and warnings
            end
        end
    end

    rect rgb(238, 247, 239)
        Note over Staff,App: Validate, submit, accept, and convert
        Staff->>UI: Validate party identification
        UI->>Svc: ValidatePartyIdentification(requestId)
        Svc->>DB: Evaluate client, contact, proposed use/users,<br/>restrictions, lifecycle, and row versions
        DB-->>Svc: Errors and warnings
        Svc-->>UI: Validation summary

        alt Blocking errors remain
            UI-->>Staff: Show field errors and validation panel;<br/>remain editable in permitted state
        else Submission rules pass
            Staff->>UI: Submit request
            UI->>Svc: SubmitAppraisalRequest(expectedRowVersion)
            Svc->>DB: Capture submission snapshots,<br/>set Submitted, increment row version, audit
            DB-->>Svc: Submitted request
            Svc-->>UI: Status = Submitted

            Staff->>UI: Review and accept request
            UI->>Svc: AcceptAppraisalRequest(expectedRowVersion)
            Svc->>DB: Re-run CIU invariants and restriction checks
            alt Acceptance blocker or concurrency conflict
                Svc-->>UI: 422 blocker or 409 CONCURRENCY-001
            else Acceptance succeeds
                Svc->>DB: Freeze snapshots, set Accepted,<br/>record reviewer/time/version and audit event
                DB-->>Svc: Accepted read-only request
                Svc-->>UI: Status = Accepted

                Staff->>UI: Convert accepted request
                UI->>Svc: ConvertAppraisalRequest(expectedRowVersion)
                Svc->>DB: Verify not previously converted and begin transaction
                alt Existing conversion or unresolved blocker
                    Svc-->>UI: 422 CIU-012 or applicable validation code
                else Conversion succeeds atomically
                    Svc->>App: Create Appraisal and review-required<br/>party/use payload from frozen snapshots
                    App->>DB: Persist Appraisal and formal-review tasks
                    Svc->>DB: Set converted_appraisal_id,<br/>status = Converted, and conversion audit
                    DB-->>Svc: Commit one-to-one conversion
                    Svc-->>UI: Converted request + linked Appraisal ID
                    UI-->>Staff: Open Appraisal review
                    Note over Staff,App: Appraiser explicitly establishes authoritative<br/>Appraisal client, intended use, and intended users
                end
            end
        end
    end

    opt Authorized amendment after acceptance or conversion
        Staff->>UI: Start amendment with reason
        UI->>Svc: AmendPartyIdentification(before/after, reason,<br/>expectedRowVersion)
        Svc->>DB: Verify permission; preserve prior snapshots;<br/>record actor, approver, versions, and audit
        alt Request is already converted
            Svc->>App: Create review task; do not rewrite Appraisal facts
        end
        Svc-->>UI: Amendment result and current row version
    end
```

## Flow Guarantees

1. **Client and intended user are distinct concepts.** Selecting a client or contact never implicitly makes that party an intended user.
2. **Intended use is explicit.** It is selected from active, effective reference data and is not inferred from appraisal type, report format, value definition, client type, or recipient.
3. **Proposed intended users are request-owned snapshots.** Later edits to reusable Client or Client Contact records do not rewrite accepted or converted request history.
4. **At least one active proposed intended user is required** before acceptance or conversion.
5. **Only one intended-user row may represent the client** for a given Appraisal Request.
6. **Optimistic concurrency applies to every mutation.** A stale `expectedRowVersion` returns a conflict and does not overwrite another user's changes.
7. **Acceptance freezes the request snapshots.** Later changes require an authorized, audited amendment.
8. **Conversion is atomic and one-to-one.** It creates one Appraisal, a review-required conversion payload, the conversion link, and the audit event in one transaction.
9. **Appraisal facts remain authoritative only after formal review.** Conversion does not silently promote proposed request facts into final Appraisal problem-identification facts.
10. **Distribution is separate from intended-user status.** A report recipient is not automatically an intended user, and an intended user is not automatically authorized to receive the report electronically.

## Principal Validation Outcomes

| Condition | Result |
|---|---|
| Missing client | `422 CIU-001` |
| Contact is inactive or does not belong to client | `422 CIU-002` / `CIU-010` |
| Missing proposed intended use | `422 CIU-004` |
| No active proposed intended users | `422 CIU-005` |
| More than one client-marked intended user | `422 CIU-006` or unique-key conflict |
| Required custom-use narrative is missing | `422 CIU-007` |
| Intended-user identity, type, or relationship is incomplete | `422 CIU-008` / `CIU-009` |
| Use restriction conflicts with request or proposed user | `422 CIU-011` |
| Request is already converted | `422 CIU-012` |
| Possible duplicate intended user | Warning `CIU-013` |
| Client is not an intended user | Warning `CIU-014`; explicit review required |
| Stored row version differs from expected version | `409 CONCURRENCY-001` |

## Source Artifacts

- `Client_Intended_User_DDS_v2.0.md`
- `Clients_v2.0.sql`
- `Client_Contacts_Create_v1.0.sql`
- `Appraisal_Requests_Create_v1.0.sql`
- `Use_Categories_Create_v1.0.sql`
- `Intended_User_Types_Create_v1.0.sql`
- `Appraisal_Request_Intended_Users_Create_v1.0.sql`
