<div align="center">

# ONE Classics Nexus

## Client - Requirements Definition (RD)

</div>

**Document ID:** Nexus-RD-CLIENT  
**Version:** 1.2  
**Status:** Draft for review  
**Domain:** Client master data and client contacts  
**Primary standard context:** 2024 Uniform Standards of Professional Appraisal Practice (USPAP), Standard 7: Personal Property Appraisal, Development

## 1. Purpose

This Requirements Definition specifies how ONE Classics Nexus shall create, maintain, identify, and protect reusable Client records and their Client Contacts. It defines the Client domain boundary and requirements for Client master data. It is a software requirements artifact, not legal advice or a substitute for the appraiser’s professional judgment.

## 2. Scope

This document covers:

- Reusable identity and ordinary contact information for individual and organization clients.
- Client Contact records associated with an organization Client.
- Client record lifecycle operations: create, view, update, search, deactivate, and controlled correction or merge.
- Distinguishing an organization Client from its contacts and preserving the client’s stable identity and history.
- Providing a stable Client reference for other Nexus domains to use when they independently capture a client relationship.
- Access control, validation, and audit requirements for Client and Client Contact data.

This document does **not** define Appraisal Request scope, including request intake, request status, requester/submitting contact, or request CRUD. It does not define appraisal/assignment scope, assignment acceptance, engagement terms or evidence, assignment-time client snapshots, intended use, intended users, value type or definition, effective date, subject-property characteristics, scope of work, valuation, or report generation. Those requirements belong to their respective RDs/DDSs. Other domains may reference a Client by its stable identifier; they own the meaning, validation, and history of that relationship.

## 3. Normative basis and interpretation

The 2024 USPAP definition describes the client as the party or parties who engage an appraiser by employment or contract in a specific assignment. Standard 7 SR 7-2(a) requires the appraiser to identify the client and other intended users when developing an appraisal. These are assignment-specific appraisal determinations.

**Client-domain interpretation:** The Client domain maintains reusable party identity. A master Client record does not establish that the party is the client for any particular assignment, does not make a party an intended user, and does not prove that an engagement exists. An appraisal or other consuming domain is responsible for explicitly identifying and preserving its own relationship to a Client. Client data and profile attributes must not imply or dictate appraisal conclusions.

## 4. Definitions

| Term | Definition for Nexus |
|---|---|
| Client | A reusable person or organization record that may be referenced by other Nexus domains. Its role as client is established in the context of a specific engagement by that domain. |
| Client record | The master record for a person or organization, containing stable identity, ordinary party details, lifecycle status, and audit metadata. |
| Client Contact | A natural person associated with an organization Client who acts for, works for, or communicates on behalf of that organization. A contact is a separate record and is not automatically the Client. |
| Client identifier | A stable unique key assigned to a Client and independent of name, email, or address. |
| Deactivation | A lifecycle action that prevents routine new use of a Client record while retaining the record and its history for authorized reference. |
| Consuming domain | A separate Nexus domain that references a Client and owns the purpose-specific relationship and related business rules. |

## 5. Business rules

1. Each Client shall have a stable unique identifier that does not change when its name or contact details change.
2. A Client shall be classified as an individual or organization. Organization contacts shall be represented as Client Contact records linked to that organization.
3. A Client Contact shall not be treated as a separate Client, client representative with legal authority, or other business role unless the relevant consuming domain explicitly records that role.
4. Client master data describes a party; it does not establish a client relationship for an appraisal, request, or other transaction. Consuming domains must explicitly create and maintain their own relationships.
5. Assignment-specific engagement terms, client capacity, intended use, intended users, or other appraisal problem-identification data shall not be stored as Client master attributes.
6. Updating Client or Contact master data shall not rewrite historical values or snapshots owned by a consuming domain.
7. Deactivation, correction, or merge shall not delete a Client or Contact referenced by existing records. Historical references must remain resolvable to authorized users.
8. A merge or identity correction shall preserve the source record, target record, actor, timestamp, reason, and reference mapping needed to interpret existing links.
9. The system shall collect only Client and Contact information needed for party identification, ordinary communications, and authorized business operations. Sensitive data shall have restricted access.
10. Client-provided preferences or requested outcomes, if collected by another domain, shall remain source information and shall not be represented as professional conclusions.

## 6. Functional requirements

### 6.1 Client master record

| ID | Requirement | Priority |
|---|---|---|
| FR-CLIENT-001 | The system shall create, view, update, search, and deactivate Client master records for individual and organization Clients. | Must |
| FR-CLIENT-002 | The system shall assign each Client a stable unique identifier independent of name, email, or address. | Must |
| FR-CLIENT-003 | The system shall record Client type (individual or organization), legal/display name, lifecycle status, and created/updated audit metadata. | Must |
| FR-CLIENT-004 | The system shall support ordinary party details appropriate to Client type, such as mailing address, phone, email, and organization details, subject to the approved Client data model. | Must |
| FR-CLIENT-005 | The system shall detect likely duplicate Clients using configurable matching indicators and require user review before creating a likely duplicate. | Should |
| FR-CLIENT-006 | The system shall retain inactive Clients for historical reference and distinguish them from Clients available for routine new selection. | Must |
| FR-CLIENT-007 | The system shall support controlled correction or merge of duplicate Client records while preserving identifiers, prior values, links, reason, actor, and timestamp. | Must |
| FR-CLIENT-008 | The system shall validate required identity fields by Client type and validate contact data formats where applicable. | Must |

### 6.2 Client Contacts

| ID | Requirement | Priority |
|---|---|---|
| FR-CLIENT-010 | The system shall create, view, update, search, and deactivate Client Contact records associated with an organization Client. | Must |
| FR-CLIENT-011 | Each Client Contact shall have a stable unique identifier and a required link to its organization Client. | Must |
| FR-CLIENT-012 | The system shall record the contact’s name, ordinary communication details, role/title when provided, lifecycle status, and audit metadata. | Must |
| FR-CLIENT-013 | The system shall permit more than one Client Contact for an organization and distinguish active from inactive contacts. | Must |
| FR-CLIENT-014 | The system shall preserve historical contact references when a contact is deactivated or its details change. | Must |
| FR-CLIENT-015 | The system shall not infer authority, engagement capacity, or a consuming-domain role solely from a contact’s title, relationship, or communication with Nexus users. | Must |

### 6.3 Access, audit, and integration boundary

| ID | Requirement | Priority |
|---|---|---|
| FR-CLIENT-020 | The system shall log Client and Client Contact creation, material updates, deactivation, correction, merge, and reactivation with actor, timestamp, and before/after values. | Must |
| FR-CLIENT-021 | The system shall enforce role-based access to Client and Contact personal or confidential information. | Must |
| FR-CLIENT-022 | The system shall allow consuming domains to reference a Client using its stable identifier without giving those domains ownership of Client master identity. | Must |
| FR-CLIENT-023 | The system shall make Client lifecycle status and sufficient identifying information available to consuming domains for selection and validation. | Must |
| FR-CLIENT-024 | The system shall prevent deactivation or merge operations from silently orphaning or deleting existing consuming-domain references. | Must |
| FR-CLIENT-025 | The system shall provide a searchable Client list with filters for Client type, status, and identifying attributes, subject to access permissions. | Must |

## 7. Data requirements

The logical Client domain shall support, at minimum:

- `clients`: stable Client identifier; Client type; legal/display name; lifecycle status; ordinary individual or organization details; created/updated timestamps and actors as applicable.
- `client_contacts`: stable Contact identifier; organization Client identifier; contact name; role/title where available; ordinary communication details; lifecycle/effective dates where required; audit fields.
- Client and Contact history or audit records sufficient to preserve material changes, deactivation, correction, and merge mappings.

Other domains may store foreign-key references to `clients.client_id` and, where appropriate, `client_contacts.client_contact_id`. Such references do not expand the Client domain’s ownership to request or assignment workflow. Exact table and column definitions belong in the Client DDS and database schema and must be reconciled against the current database source of truth before implementation.

## 8. Validation and acceptance criteria

1. An authorized user can create an individual Client and an organization Client with type-appropriate required identity fields.
2. Each created Client receives a stable identifier that remains unchanged when ordinary identifying or communication details are updated.
3. An organization can have multiple separately identifiable Client Contacts.
4. A Client Contact cannot exist without its organization Client, and deactivating a contact does not delete its history.
5. A Client can be deactivated while existing references remain resolvable to authorized users.
6. A likely duplicate is surfaced for review; a merge preserves the source-to-target mapping and audit history.
7. Updating Client or Contact master data does not rewrite historical data maintained by a consuming domain.
8. A consuming domain can reference a Client by stable identifier and separately record the meaning and history of that relationship.
9. User permissions restrict access to sensitive Client and Contact fields, and material changes are attributable to an actor and timestamp.
10. Client master data alone cannot be interpreted as proof of engagement, assignment acceptance, intended use, intended user status, or appraisal findings.

## 9. Traceability to USPAP 2024 Standard 7

| USPAP reference | Relevant obligation/context | Client-domain treatment |
|---|---|---|
| Definition of Client | Client is the party or parties who engage an appraiser for a specific assignment. | Client master data identifies reusable parties; a consuming assignment domain establishes the assignment-specific client relationship. |
| SR 7-2(a) | Identify the client and other intended users during appraisal development. | The Client domain supplies party identity only. Assignment-specific identification and intended-user rules are out of scope and belong to appraisal requirements. |
| SR 7-2(b) and Comment | Identify intended use; do not allow intended use or client objectives to bias results. | No intended-use or appraisal-bias workflow is specified here. Client records shall not encode or drive appraisal conclusions. |

## 10. Dependencies and related Nexus artifacts

- Nexus appraisal-centric SRS and USPAP Standards 7 & 8 requirements.
- Client and Client Contact data model and DDL.
- Separate Appraisal Request RD/DDS for request intake and request-specific parties.
- Separate Appraisal/Assignment RD/DDS for engagement and assignment-specific client identification.
- Intended Use, Intended User, reporting, and audit/history requirements owned by their respective domains.

## 11. Assumptions and open design items

1. A Client may be an individual or organization; an organization may have multiple contacts.
2. Client master data is reusable across Nexus workflows, but the specific role of a Client in a request, assignment, or other record is owned by that consuming domain.
3. Individual contacts shall not be modeled as organization contacts unless associated with an organization Client; whether the application also supports individual-client representatives is a separate data-model decision.
4. Key types, field lengths, address model, retention policy, and audit implementation are to be confirmed against the current schema before the Client DDS is locked.
5. This RD does not define Appraisal Request or appraisal assignment requirements. Those must remain in their own specifications and traceability matrices.

## 12. Source

The Appraisal Foundation. *Uniform Standards of Professional Appraisal Practice (USPAP), 2024 Edition*, Definitions (Client) and Standard 7, especially SR 7-2(a)–(b). Refer to the official USPAP source page: [The Appraisal Foundation — USPAP](https://appraisalfoundation.org/pages/uspap/).

This document paraphrases the relevant standard context for requirements analysis. Verify final application against the licensed 2024 USPAP edition held by the appraisal organization.