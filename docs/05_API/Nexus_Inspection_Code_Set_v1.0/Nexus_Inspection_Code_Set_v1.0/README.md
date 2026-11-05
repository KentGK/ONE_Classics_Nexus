# Nexus Inspection Code Set v1.0

This package implements the Inspection vertical slice aligned to `Nexus_04_Data_Inspections_v1.3.md`.

## Included

- Domain entities:
  - `Inspection`
  - `ConditionScore`
- API contracts:
  - Create/Update inspection requests
  - Save 12-system scores request
  - Inspection responses
- API service:
  - `InspectionApiService`
- API controller:
  - `InspectionsController`
- Repository abstractions:
  - `IInspectionRepository`
  - `IConditionScoreRepository`
- Infrastructure repositories:
  - `InspectionRepository`
  - `ConditionScoreRepository`
- DI registration helpers
- SQL DDL baseline
- Swagger/HTTP test script

## Routes

```text
POST   /api/inspections
GET    /api/inspections/{inspectionId}
PUT    /api/inspections/{inspectionId}
DELETE /api/inspections/{inspectionId}
PUT    /api/inspections/{inspectionId}/scores
GET    /api/appraisals/{appraisalId}/inspections
```

## Program.cs registration

Add:

```csharp
builder.Services.AddInspectionRepositories();
builder.Services.AddInspectionApiServices();
```

And using statements as needed:

```csharp
using ONE_Classics_Nexus.Infrastructure.DependencyInjection;
using ONE_Classics_Nexus.API.DependencyInjection;
```

## Notes

- `PUT /api/inspections/{id}/scores` replaces all condition scores for the inspection.
- Score validation enforces exactly 12 scores, system numbers 1-12, canonical `system_key`, score 0-5, and weight 0-1.
- Condition Index = `SUM(score_0_to_5 * weight) * 20`.
