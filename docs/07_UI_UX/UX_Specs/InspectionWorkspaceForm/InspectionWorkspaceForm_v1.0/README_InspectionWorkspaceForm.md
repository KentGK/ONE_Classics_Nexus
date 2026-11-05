# ONE Classics Nexus — InspectionWorkspaceForm v1.0

## Deliverable

This package replaces the navigation placeholder with a production-oriented
`InspectionWorkspaceForm` based on the approved Inspection Overview wireframe.

The Inspection ID is implemented as a searchable WinForms `ComboBox`:

```csharp
cmbInspectionId.DropDownStyle = ComboBoxStyle.DropDown;
cmbInspectionId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
cmbInspectionId.AutoCompleteSource = AutoCompleteSource.ListItems;
```

Each item displays:

```text
{inspectionId} | {last updated date} | {status}
```

## Files

```text
Forms/
  Inspections/
    InspectionWorkspaceForm.cs
    InspectionWorkspaceForm.Designer.cs
    Contracts/
      InspectionWorkspaceContracts.cs
    Services/
      IInspectionWorkspaceApiClient.cs
      InspectionWorkspaceApiClient.cs
      MockInspectionWorkspaceApiClient.cs
  Shell/
    MainShellForm.InspectionWiring.cs
```

## Installation

1. Remove or replace the current placeholder files:

```text
Forms/Inspections/InspectionWorkspaceForm.cs
Forms/Inspections/InspectionWorkspaceForm.Designer.cs
```

Do not retain two declarations with the same class and namespace.

2. Copy the package folders into:

```text
ONE_Classics_Nexus.WinForms/
```

3. Ensure the existing theme package is present:

```text
ONE_Classics_Nexus.WinForms.Theming.NexusTheme
ONE_Classics_Nexus.WinForms.Theming.NexusControlStyles
```

4. Merge `MainShellForm.InspectionWiring.cs` into the shell project or keep it
as a partial class file.

## API base URL

The client reads:

```text
ONE_CLASSICS_NEXUS_API_BASE_URL
```

and falls back to:

```text
https://localhost:5001/
```

Example PowerShell configuration:

```powershell
$env:ONE_CLASSICS_NEXUS_API_BASE_URL = "https://localhost:7190/"
```

## As-built API routes used

```text
GET    /api/inspections
GET    /api/inspections/{inspectionId}
PUT    /api/inspections/{inspectionId}
DELETE /api/inspections/{inspectionId}
GET    /api/inspections/{inspectionId}/scores
PUT    /api/inspections/{inspectionId}/scores
GET    /api/appraisals/{appraisalId}
GET    /api/vehicles/{vehicleId}
```

## Immediate visual test

Use the mock client in the shell:

```csharp
var workspace = new InspectionWorkspaceForm(
    new MockInspectionWorkspaceApiClient(),
    disposeApiClient: true);

LoadChildForm(workspace);
```

This renders sample inspection, appraisal, vehicle, score, red-flag, media,
progress, and checklist data without requiring the API.

## Data-model alignment

The form supports:

- inspection header fields
- appraisal and vehicle context
- 12 canonical condition-score rows
- score validation from 0.00 through 5.00
- weights totaling 1.0000 before completion
- system-level red flags
- completion percentage
- overall condition index
- optimistic concurrency through `rowVersion`
- status-based deletion behavior

The UI displays:

```text
Condition Index: 0-100
Weighted score: 0-5
```

This preserves the wireframe's weighted-score presentation while aligning the
condition index to the inspection data model.

## Known API limitation

The current API exposes canonical `condition_scores` end-to-end. Dedicated API
routes for section scores, item scores, structured red flags, and media are not
currently documented as implemented. The form therefore:

- reads section summaries, red flags, and media when returned in the inspection
  detail response;
- otherwise derives section rows and system red flags from canonical condition
  scores;
- leaves media counts at zero until corresponding API support is added.

## Designer note

The form is a standard partial WinForms `Form` with:

```text
InspectionWorkspaceForm.cs
InspectionWorkspaceForm.Designer.cs
```

Navigation and API logic remain out of the Designer file.
