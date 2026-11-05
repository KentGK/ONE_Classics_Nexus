---
title: "ONE Classics Nexus — Individual Client Workspace"
specification_version: "1.0"
document_type: "screenshot_transcription_and_ui_specification"
brand: "ONE Classics Nexus"
source_image: "ONE Classics Nexus Client Workspace.png"
source_conversation_id: "6ad650ae-34d8-83e8-a655-45de1fe2340e"
language: "en"
scope: "Visible desktop Clients workspace, Profile tab, selected Individual client"
---

# ONE Classics Nexus — Individual Client Workspace

## 1. Evidence and interpretation

This document transcribes the supplied screenshot into a structured specification. Read tables in their listed order to reproduce the screen's reading order. Quoted UI strings preserve visible wording and capitalization. Stable IDs in this document are specification identifiers, not observed implementation identifiers.

Evidence categories:

- **Observed:** A visible label, value, arrangement, control, selection, or message in the screenshot.
- **Explicit rule:** A constraint stated in visible interface text.
- **Inferred:** A likely component type or interaction suggested by visual appearance; runtime behavior has not been tested.
- **Unspecified:** Not established by the screenshot. Do not turn an unspecified behavior into a requirement without further input.

All personal details, organizations, statuses, selected filter values, the update timestamp, and pagination totals below are **screenshot sample data**, not application defaults, fixed production records, or universal business rules. Blank fields represent visible empty controls; they do not prove that values are optional. Asterisks explicitly mark First Name and Last Name as required.

## 2. Branding and desktop layout

| Region ID | Position and component | Observed content / appearance |
|---|---|---|
| `window_title_bar` | Full-width top title bar | `ONE Classics Nexus / Clients`; dark navy background, white title; minimize, maximize, and close glyphs at the far right |
| `client_browser` | Left pane, approximately 32% of workspace width | Search, two filters, Create Client action, client table, and pagination |
| `client_detail` | Right pane, approximately 68% of workspace width | Client summary header, tabs, Profile form, and bottom actions |
| `pane_divider` | Vertical separator | Thin gray line between browser and detail panes |
| `detail_footer` | Bottom of right pane | Light neutral action bar with Cancel and Save aligned right |

- The reference image is approximately 1487 × 1058 pixels.
- The title bar is about 52 pixels high; the pane boundary is near x=480.
- These describe this reference viewport, not required fixed dimensions or responsive breakpoints.

Use **ONE Classics Nexus** as the product name. Preserve the navy title bar and primary buttons, white content surfaces, pale gray separators and control borders, dark text, muted secondary text, pale blue selected table row, green status/availability indicators, and red required-field asterisks. Typography is a clean sans serif with bold headings. Exact font family, color tokens, spacing tokens, logo artwork, and responsive behavior are unspecified; no separate logo is visible.

## 3. Client browser

### 3.1 Search, filters, and creation

| ID | Component type | Label or visible text | Screenshot state | Rules / limits of evidence |
|---|---|---|---|---|
| `client_search` | Search input with magnifier icon | Placeholder: `Search clients by name, email, or organization...` | Empty | Search fields are described by the placeholder; matching algorithm and execution timing unspecified |
| `client_type_filter` | Select / dropdown | `Client Type` | `All Types` | Full option set unspecified |
| `status_filter` | Select / dropdown | `Status` | `All Statuses` | Full option set unspecified |
| `create_client` | Full-width primary button with plus icon | `Create Client` | Navy, enabled-looking | Creation destination, dialog, and defaults unspecified |

- The search occupies its own row.
- Client Type and Status filters sit side by side beneath it.
- Create Client spans the available pane width beneath the filters.

### 3.2 Client table — screenshot sample data

Columns appear in this order: `Name / Organization`, `Type`, `Status`. The first column uses a bold name on line one and a muted email on line two. Header separators divide the columns. Rows have horizontal separators. Jordan Lee is selected with a pale blue full-row background.

| Visible row | Name / Organization | Email (second line) | Type | Status | Selected |
|---:|---|---|---|---|---|
| 1 | Jordan Lee | jordan.lee@example.com | Individual | Active | true |
| 2 | Heritage Auto Collection | contact@heritageauto.com | Organization | Active | false |
| 3 | Taylor Morgan | taylor.morgan@example.com | Individual | Active | false |
| 4 | Riverside Classics LLC | info@riversideclassics.com | Organization | Active | false |
| 5 | Casey Dunn | casey.dunn@example.com | Individual | Active | false |
| 6 | Maple Grove Motors | sales@maplegrovemotors.com | Organization | Active | false |
| 7 | Alex Rivera | alex.rivera@example.com | Individual | Active | false |
| 8 | Pinecrest Auto Group | info@pinecrestauto.com | Organization | Active | false |
| 9 | Morgan Patel | morgan.patel@example.com | Individual | Active | false |
| 10 | Summit Classics | contact@summitclassics.com | Organization | Active | false |

- Row selection is suggested by the highlighted row and matching detail pane.
- Selection mechanics, sorting, multi-selection, keyboard support, and whether selection discards unsaved changes are unspecified.
- Organization rows are included because they are visible navigation context; their detail forms are not shown.

### 3.3 Pagination

- Below the table, controls appear left to right as: previous chevron, `1`, `2`, `3`, `4`, `5`, next chevron.
- Page `1` has a navy selected background; other page numbers have white backgrounds and borders.
- The previous chevron looks muted, but its disabled state is not confirmed.
- The next chevron looks enabled.

- The right-aligned range reads exactly `1–10 of 42`.
- The visible sample therefore has ten records on the first page, 42 total records, and five displayed page buttons.
- This does not establish a globally fixed page size or total.
- Page-size controls, additional-page overflow behavior, and pagination during filtering are unspecified.

### 3.4 Client browser Requirements


`client_search`         |   Initialize the search field to blank
                            If search results are found, then populate the Client List with searched client
`client_type_filter`    |   Default to "All Types"
                            If "Individual" selected, then display all clients of type individual in the Client List
                            If "Organization" selected, then display all clients of type organization in the Client List
`status_filter`         |   Default to "All Statuses"
`create_client`         |   Initialize Client Workspace to a new client  |

## 4. Selected client header and navigation

| ID | Component | Screenshot sample content / state |
|---|---|---|
| `client_heading` | Large bold heading, upper left | `Jordan Lee` |
| `client_summary` | Muted inline summary beneath heading | `Individual` \| `jordan.lee@example.com` \| `(312) 555-0187` |
| `client_status` | Rounded pale green badge, upper right | Green dot followed by `Active` |
| `last_updated` | Small muted text beneath status | `Last updated Apr 24, 2025 10:36 AM` |

- The status badge and timestamp are display elements; no edit controls for either are visible.
- The timestamp's timezone, source, and update triggers are unspecified.
- `Active` is a sample state, not evidence that all clients must be active.

- Tabs below the header appear left to right: `Profile`, `Contacts`, `History`. `Profile` is selected, bold/navy, with a navy underline.
- Contacts and History are unselected.
- Their contents, routes, and access rules are unspecified.

## 5. Profile form

The form is organized vertically into Client Information, Contact Information, Mailing Address, and Communication Preferences, separated by thin gray horizontal rules. Labels sit to the left of their controls. Input-like controls have white backgrounds, light gray borders, and subtly rounded corners. Editability below is an appearance-based inference unless an explicit rule is cited.

### 5.1 Client Information

| ID | Exact field label | Component type | Screenshot sample value | Required marker | Read-only / editability |
|---|---|---|---|---|---|
| `client_type` | Client Type | Disabled-looking select with chevron and adjacent lock icon | Individual | None | Locked after creation; explicit rule below |
| `first_name` | First Name | Single-line text input | Jordan | Red `*` | Editable-looking |
| `middle_name` | Middle Name | Single-line text input | Empty | None | Editable-looking |
| `last_name` | Last Name | Single-line text input | Lee | Red `*` | Editable-looking |
| `suffix` | Suffix | Select / dropdown | Empty | None | Editable-looking; options unspecified |
| `display_name` | Display Name | Single-line text input | Jordan Lee | None | Editable-looking |

**Explicit rule beneath Client Type:** `Client type cannot be changed after the client is created.`

- Preserve the disabled visual treatment and lock indicator for Client Type in this existing-client view.
- The screenshot proves the immutability rule, but does not establish whether the implementation uses a disabled select, a read-only presentation, or another mechanism.
- No other profile field visibly carries a lock or explicit read-only rule.

**Helper text beneath Display Name:** `This is how the client will appear in lists and communications.`

- Layout: Client Type is on its own row.
- First Name and Middle Name share the next row; Last Name and Suffix share the following row.
- Display Name spans the remaining form width on the next row.
- Display-name generation, automatic synchronization with first/last name, maximum lengths, and suffix choices are unspecified.

### 5.2 Contact Information

| ID | Exact field label | Component type | Screenshot sample value | Required marker | Editability |
|---|---|---|---|---|---|
| `email_address` | Email Address | Single-line input; email semantics inferred | jordan.lee@example.com | None | Editable-looking |
| `phone_number` | Phone Number | Single-line input; telephone semantics inferred | (312) 555-0187 | None | Editable-looking |

- Each field occupies a separate row and extends across the available control area.
- Validation rules, uniqueness, phone normalization, and permitted formats are unspecified.

### 5.3 Mailing Address

| ID | Exact field label | Component type | Screenshot sample value | Required marker | Editability |
|---|---|---|---|---|---|
| `address_line_1` | Address Line 1 | Single-line text input | 1234 North Michigan Avenue | None | Editable-looking |
| `address_line_2` | Address Line 2 | Single-line text input | Empty | None | Editable-looking |
| `city` | City | Single-line text input | Chicago | None | Editable-looking |
| `state` | State | Select / dropdown | IL | None | Editable-looking; option set unspecified |
| `zip_code` | ZIP Code | Single-line input | 60611 | None | Editable-looking |

- Address Line 1 and Address Line 2 each occupy a full control row.
- The final row contains City, State, and ZIP Code from left to right. 
- No Country field is visible. 
- Do not infer country restrictions or ZIP validation rules from this sample. 
- Treat ZIP Code as a string in machine-readable data to preserve possible leading zeroes.

### 5.4 Communication Preferences

| ID | Exact field label | Component type | Screenshot sample value | Required marker | Editability |
|---|---|---|---|---|---|
| `preferred_communication_method` | Preferred Communication Method | Select / dropdown | Email | None | Editable-looking; remaining options unspecified |

- Beneath the select, show a green circular check icon and the green message `Email address is available.` 
- This is a sample availability state. 
- The screenshot does not establish address deliverability, verified ownership, uniqueness, consent, or behavior when the email is absent.

- Beneath that message, preserve the muted italic note: `This preference is a routing convenience and does not constitute consent.` 
- This is an **explicit semantic rule**: communication preference must not be interpreted as consent. 
- No consent checkbox is visible.

## 6. Footer actions and read-only boundaries

| ID | Component type | Visible label | Appearance | Established / unspecified behavior |
|---|---|---|---|---|
| `cancel` | Secondary button | Cancel | White/light fill and gray border | Intended cancellation inferred; rollback, navigation, and confirmation behavior unspecified |
| `save` | Primary button | Save | Navy fill, white text | Intended save inferred; persistence, validation timing, success feedback, and enabled-state logic unspecified |

- Buttons appear in the order Cancel then Save, at the bottom right of the detail pane. 
- Their persistent/fixed positioning during scrolling is not established by one screenshot.

Read-only constraints and display boundaries:

- **Explicitly immutable:** Existing Client Type. It is shown as Individual, with muted text and a lock icon.
- **Display-only in this view:** Product title, summary heading, summary metadata, status badge, update timestamp, table record text, helper text, availability message, and pagination range. No inline editing controls are visible for these elements.
- **Editable-looking:** All other form fields listed above. Actual permissions, role-based restrictions, and server enforcement are unspecified.
- **Required:** First Name and Last Name show red asterisks. The screenshot does not establish additional mandatory fields or error messages.
- **Navigation/actions:** Filters, tabs, pagination, Create Client, Save, and Cancel are controls, not editable client data fields.

## 7. Machine-readable screenshot sample

The following JSON is a sample fixture transcribed from the screen. It is not a set of default values. Empty visible controls are represented by empty strings; unknown behavior elsewhere is described as unspecified rather than represented by invented values.

```json
{
  "brand": "ONE Classics Nexus",
  "workspace": "Clients",
  "activeTab": "Profile",
  "filters": {
    "searchText": "",
    "clientTypeDisplayed": "All Types",
    "statusDisplayed": "All Statuses"
  },
  "selectedClient": {
    "clientType": "Individual",
    "firstName": "Jordan",
    "middleName": "",
    "lastName": "Lee",
    "suffix": "",
    "displayName": "Jordan Lee",
    "emailAddress": "jordan.lee@example.com",
    "phoneNumber": "(312) 555-0187",
    "mailingAddress": {
      "addressLine1": "1234 North Michigan Avenue",
      "addressLine2": "",
      "city": "Chicago",
      "state": "IL",
      "zipCode": "60611"
    },
    "preferredCommunicationMethod": "Email",
    "statusDisplayed": "Active",
    "lastUpdatedDisplayed": "Last updated Apr 24, 2025 10:36 AM",
    "availabilityMessageDisplayed": "Email address is available."
  },
  "pagination": {
    "currentPage": 1,
    "visiblePageButtons": [1, 2, 3, 4, 5],
    "visibleRangeStart": 1,
    "visibleRangeEnd": 10,
    "totalRecords": 42,
    "rangeText": "1–10 of 42"
  }
}
```

## 8. Machine-readable explicit rules

These rules are separate from the sample fixture and are limited to what the visible interface states or marks.

```json
{
  "rules": [
    {
      "id": "client_type_immutable_after_creation",
      "evidence": "explicit_ui_text",
      "field": "clientType",
      "constraint": "cannot_change_after_creation",
      "sourceText": "Client type cannot be changed after the client is created."
    },
    {
      "id": "required_name_fields",
      "evidence": "visible_required_asterisks",
      "fields": ["firstName", "lastName"],
      "constraint": "required"
    },
    {
      "id": "display_name_purpose",
      "evidence": "explicit_ui_text",
      "field": "displayName",
      "purpose": "client_name_in_lists_and_communications",
      "sourceText": "This is how the client will appear in lists and communications."
    },
    {
      "id": "preference_is_not_consent",
      "evidence": "explicit_ui_text",
      "field": "preferredCommunicationMethod",
      "constraint": "routing_convenience_does_not_constitute_consent",
      "sourceText": "This preference is a routing convenience and does not constitute consent."
    }
  ]
}
```

## 9. Fidelity boundaries

- Only the displayed Profile screen is specified. 
- Do not invent Contacts or History content, additional fields, menu options, client identifiers, status transitions, endpoints, database schemas, permission models, validation messages, or save/cancel workflows. No loading, error, empty-list, confirmation, or success state is shown.
- Mobile layouts, scrolling behavior, exact dimensions, hover/focus states, and accessibility implementation details remain unspecified.

- The timestamp is preserved as displayed without assuming a timezone. 
- The Active badge, selected Email preference, ten visible rows, and 42-record total are sample data. 
- The immutable client-type rule, the two visible required markers, the display-name purpose, and the distinction between routing preference and consent are supported directly by the screenshot.
