# ONE Classics Nexus — AppraisalWorkspace v1.0

Production-oriented WinForms workspace code set for `ONE_Classics_Nexus.WinForms` targeting .NET 8.

## Design basis

The implementation reconciles these supplied sources:

- `CREATE_appraisals.sql` — newest appraisal DDL, including normalized `appraisal_type_id` and `appraisal_status_id` alongside legacy text fields.
- `CREATE TABLE appraisals (.txt` — canonical appraisal lifecycle columns and foreign keys.
- `individual_erds.puml` — appraisal parent relationships to vehicle, client, assignments, inspection, comparables, valuation, and report records.
- `Nexus_Appraisal_Sequence_Diagram.md` — API-first create/load/update flow and `row_version` optimistic concurrency.
- `ONE Classics Nexus Design.txt` — WinForms dynamic content shell, API-first architecture, and Nexus UI module boundaries.

## What is included

- `AppraisalWorkspaceForm` and Designer code.
- Locked Nexus primary theme `#0E2841`.
- Header, status badge, summary strip, and workflow navigation controls.
- Overview, Intake, Vehicle, Inspection, Comparables, Valuation, Review, and Report pages.
- API-only `IAppraisalWorkspaceService` / `AppraisalWorkspaceService`.
- Tolerant direct-or-envelope JSON response parsing.
- `ProblemDetails` display support.
- `409 Conflict` handling for optimistic concurrency.
- View model and explicit workspace state.
- DI registration with typed `HttpClient`.
- Shell-neutral `OpenInAsync` launcher.
- xUnit view-model tests.

## Required project packages

The WinForms project should reference:

```xml
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.1" />
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.1" />
```

Use the package versions already standardized by the solution when they differ.

## 1. Copy files

Copy the package folders into the matching solution projects:

```text
ONE_Classics_Nexus.WinForms/
ONE_Classics_Nexus.Tests/
```

SDK-style projects automatically include `*.cs` files unless compile items were explicitly disabled.

## 2. Register the workspace in Program.cs

Add:

```csharp
using ONE_Classics_Nexus.WinForms.DependencyInjection;
```

Then register before building the service provider:

```csharp
services.AddAppraisalWorkspace(new Uri("https://localhost:5001/"));
```

Prefer the existing configuration value instead of hard-coding the URL:

```csharp
var apiBaseUrl = configuration["NexusApi:BaseUrl"]
    ?? throw new InvalidOperationException("NexusApi:BaseUrl is required.");

services.AddAppraisalWorkspace(new Uri(apiBaseUrl, UriKind.Absolute));
```

## 3. Open inside the current MainShellForm content panel

The launcher does not assume a shell field name. Given the shell content panel and an appraisal id:

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals;

private readonly IServiceProvider _services;

private async Task OpenAppraisalAsync(Guid appraisalId)
{
    var workspace = await contentPanel.OpenInAsync(
        _services,
        appraisalId,
        CancellationToken.None);

    workspace.NavigationRequested += Workspace_NavigationRequested;
}
```

Replace `contentPanel` only with the actual current shell content host control.

## 4. Wire downstream navigation

```csharp
private void Workspace_NavigationRequested(
    object? sender,
    AppraisalWorkspaceNavigationEventArgs e)
{
    switch (e.Target)
    {
        case AppraisalWorkspaceSection.Vehicle:
            // Open vehicle detail using e.RelatedEntityId.
            break;

        case AppraisalWorkspaceSection.Inspection:
            // e.Action: "open" or "create"
            // Keep e.AppraisalId as workflow context.
            break;

        case AppraisalWorkspaceSection.Comparables:
            // Open comparables filtered by e.AppraisalId.
            break;

        case AppraisalWorkspaceSection.Valuation:
            // e.Action: "open" or "run"
            break;

        case AppraisalWorkspaceSection.Report:
            // e.Action: "open" or "generate"
            break;
    }
}
```

## 5. Expected API routes

```http
GET /api/appraisals/{appraisalId}
PUT /api/appraisals/{appraisalId}
```

The update payload contains:

```text
appraisalId
vehicleId
appraisalTypeId
appraisalStatusId
clientId
assignedAppraiserId
assignedReviewerId
appraisalType
status
intakeStatus
inspectionStatus
valuationStatus
reportStatus
effectiveDateUtc
rowVersion
```

### Response compatibility

The service accepts either:

1. A direct appraisal JSON object; or
2. An envelope with an `appraisal` object and optional nested context such as `vehicle`, `latestInspection`, `latestValuation`, and `latestReport`.

This makes the UI usable with the current CRUD response while remaining ready for the richer `AppraisalDetailResponse` described by the supplied sequence diagram.

## 6. Concurrency behavior

`PUT` always uses the currently loaded `rowVersion`. An API `409 Conflict` raises `AppraisalConcurrencyException`; the form offers to reload the latest record and explicitly warns that unsaved edits will be discarded.

The API/repository remains responsible for the atomic database rule:

```sql
UPDATE appraisals
SET ..., row_version = row_version + 1
WHERE appraisal_id = @AppraisalId
  AND row_version = @RowVersion;
```

## 7. Important implementation boundary

The workspace intentionally performs no SQL and owns no downstream business logic. It coordinates the appraisal lifecycle and delegates vehicle, inspection, comparables, valuation, and report operations through navigation events. This keeps the WinForms layer aligned with the Nexus API-first architecture.

## Validation note

The generated package was structurally checked for file completeness and delimiter balance in this environment. A .NET SDK/compiler was not available in the execution container, so a real `dotnet build` could not be run here. Build the package in the existing Visual Studio 2022 / .NET 8 solution and resolve only project-specific naming differences (for example, the actual MainShell content panel field or existing package versions).
