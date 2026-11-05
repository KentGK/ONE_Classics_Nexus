# Nexus 05 Inspection API Code v1.0

This package is the Inspection API implementation slice for ONE Classics Nexus.

## Source-of-truth alignment

The SQL schema remains the source of truth. This package targets the current inspection table family:

- `inspections`
- `inspection_section_scores`
- `inspection_item_scores`
- `inspection_red_flags`
- `inspection_media` for deletion cleanup
- `inspection_templates`, `inspection_template_sections`, and `inspection_template_items` as parent/reference tables

The code intentionally avoids legacy `condition_scores` in the API contract and repository layer.

## Endpoint surface

| Method | Route | Purpose |
|---|---|---|
| POST | `/api/inspections` | Create an inspection |
| GET | `/api/inspections/{inspectionId}` | Get an inspection aggregate |
| PUT | `/api/inspections/{inspectionId}` | Update inspection header fields |
| PUT | `/api/inspections/{inspectionId}/section-scores` | Replace section scores |
| PUT | `/api/inspections/{inspectionId}/item-scores` | Replace item scores |
| POST | `/api/inspections/{inspectionId}/red-flags` | Add a red flag |
| DELETE | `/api/inspections/{inspectionId}` | Delete inspection and inspection child records |

## Merge notes

1. Keep DTOs in `ONE_Classics_Nexus.API.Contracts.Inspections`.
2. Keep controller thin. Do not define DTOs inline in `InspectionsController`.
3. Register both:
   - `IInspectionRepository -> InspectionRepository`
   - `IInspectionApiService -> InspectionApiService`
4. If your project already has `INexusApiServices.cs`, merge the `IInspectionApiService` interface into that existing file instead of creating a duplicate interface file.
5. If your `IDbConnectionFactory` exposes a differently named method, update `_connectionFactory.CreateConnection()` in `InspectionRepository.cs` to your existing factory method.

## Schema check

The repository assumes these optional-but-recommended columns exist on `inspections`:

- `overall_condition_score`
- `has_red_flags`
- `notes`
- `row_version`
- `created_at_utc`
- `updated_at_utc`

If your current `CREATE_inspections.sql` differs, update the SQL projections and INSERT/UPDATE lists before building.


---


## `src/ONE_Classics_Nexus.API/Contracts/Inspections/InspectionDtos.cs`

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ONE_Classics_Nexus.API.Contracts.Inspections;

public sealed class CreateInspectionRequest
{
    [Required]
    public Guid AppraisalId { get; set; }

    [Required]
    public Guid InspectionTemplateId { get; set; }

    public Guid? InspectedByUserId { get; set; }

    [Required, StringLength(30)]
    public string InspectionType { get; set; } = "Standard";

    [StringLength(30)]
    public string? Status { get; set; } = "Draft";

    [StringLength(255)]
    public string? InspectionLocation { get; set; }

    public DateTime? InspectedOnUtc { get; set; }

    public string? Notes { get; set; }
}

public sealed class UpdateInspectionRequest
{
    public Guid? InspectionTemplateId { get; set; }

    public Guid? InspectedByUserId { get; set; }

    [StringLength(30)]
    public string? InspectionType { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    [StringLength(255)]
    public string? InspectionLocation { get; set; }

    public DateTime? InspectedOnUtc { get; set; }

    public string? Notes { get; set; }
}

public sealed class SaveInspectionSectionScoresRequest
{
    [Required, MinLength(1)]
    public IReadOnlyList<InspectionSectionScoreUpsertRequest> Scores { get; set; } =
        Array.Empty<InspectionSectionScoreUpsertRequest>();
}

public sealed class InspectionSectionScoreUpsertRequest
{
    [Required]
    public Guid InspectionTemplateSectionId { get; set; }

    [Range(0, 5)]
    public decimal Score0To5 { get; set; }

    [Range(0, 1)]
    public decimal Weight { get; set; }

    public string? Notes { get; set; }
}

public sealed class SaveInspectionItemScoresRequest
{
    [Required, MinLength(1)]
    public IReadOnlyList<InspectionItemScoreUpsertRequest> Scores { get; set; } =
        Array.Empty<InspectionItemScoreUpsertRequest>();
}

public sealed class InspectionItemScoreUpsertRequest
{
    [Required]
    public Guid InspectionTemplateItemId { get; set; }

    [Range(0, 5)]
    public decimal Score0To5 { get; set; }

    public string? Notes { get; set; }
}

public sealed class CreateInspectionRedFlagRequest
{
    public Guid? InspectionTemplateSectionId { get; set; }

    public Guid? InspectionTemplateItemId { get; set; }

    [Required, StringLength(50)]
    public string RedFlagType { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Severity { get; set; } = "Medium";

    [Required]
    public string Description { get; set; } = string.Empty;
}

public sealed class InspectionResponse
{
    public Guid InspectionId { get; set; }
    public Guid AppraisalId { get; set; }
    public Guid InspectionTemplateId { get; set; }
    public Guid? InspectedByUserId { get; set; }
    public string InspectionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? InspectionLocation { get; set; }
    public DateTime? InspectedOnUtc { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }
    public string? Notes { get; set; }
    public long RowVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public IReadOnlyList<InspectionSectionScoreResponse> SectionScores { get; set; } =
        Array.Empty<InspectionSectionScoreResponse>();

    public IReadOnlyList<InspectionItemScoreResponse> ItemScores { get; set; } =
        Array.Empty<InspectionItemScoreResponse>();

    public IReadOnlyList<InspectionRedFlagResponse> RedFlags { get; set; } =
        Array.Empty<InspectionRedFlagResponse>();
}

public sealed class InspectionSectionScoreResponse
{
    public Guid InspectionSectionScoreId { get; set; }
    public Guid InspectionId { get; set; }
    public Guid InspectionTemplateSectionId { get; set; }
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public decimal WeightedScore { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class InspectionItemScoreResponse
{
    public Guid InspectionItemScoreId { get; set; }
    public Guid InspectionId { get; set; }
    public Guid InspectionTemplateItemId { get; set; }
    public decimal Score0To5 { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}

public sealed class InspectionRedFlagResponse
{
    public Guid InspectionRedFlagId { get; set; }
    public Guid InspectionId { get; set; }
    public Guid? InspectionTemplateSectionId { get; set; }
    public Guid? InspectionTemplateItemId { get; set; }
    public string RedFlagType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsResolved { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
```


## `src/ONE_Classics_Nexus.API/ApplicationPorts/IInspectionApiService.cs`

```csharp
using ONE_Classics_Nexus.API.Contracts.Inspections;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IInspectionApiService
{
    Task<InspectionResponse> CreateAsync(CreateInspectionRequest request, CancellationToken cancellationToken);
    Task<InspectionResponse?> GetByIdAsync(Guid inspectionId, CancellationToken cancellationToken);
    Task<InspectionResponse?> UpdateAsync(Guid inspectionId, UpdateInspectionRequest request, CancellationToken cancellationToken);
    Task<InspectionResponse?> SaveSectionScoresAsync(Guid inspectionId, SaveInspectionSectionScoresRequest request, CancellationToken cancellationToken);
    Task<InspectionResponse?> SaveItemScoresAsync(Guid inspectionId, SaveInspectionItemScoresRequest request, CancellationToken cancellationToken);
    Task<InspectionResponse?> CreateRedFlagAsync(Guid inspectionId, CreateInspectionRedFlagRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid inspectionId, CancellationToken cancellationToken);
}
```


## `src/ONE_Classics_Nexus.API/Controllers/InspectionsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Inspections;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/inspections")]
public sealed class InspectionsController : ControllerBase
{
    private readonly IInspectionApiService _service;

    public InspectionsController(IInspectionApiService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InspectionResponse>> Create(
        [FromBody] CreateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        if (request.AppraisalId == Guid.Empty)
            return ValidationProblem("AppraisalId is required.");

        if (request.InspectionTemplateId == Guid.Empty)
            return ValidationProblem("InspectionTemplateId is required.");

        var response = await _service.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { inspectionId = response.InspectionId },
            response);
    }

    [HttpGet("{inspectionId:guid}")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionResponse>> GetById(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var response = await _service.GetByIdAsync(inspectionId, cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    [HttpPut("{inspectionId:guid}")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InspectionResponse>> Update(
        Guid inspectionId,
        [FromBody] UpdateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            return ValidationProblem("InspectionId is required.");

        var response = await _service.UpdateAsync(
            inspectionId, 
            request, 
            cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    [HttpPut("{inspectionId:guid}/section-scores")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<InspectionResponse>> SaveSectionScores(
        Guid inspectionId,
        [FromBody] SaveInspectionSectionScoresRequest request,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            return ValidationProblem("InspectionId is required.");

        var response = await _service.SaveSectionScoresAsync(
            inspectionId, 
            request, 
            cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    [HttpPut("{inspectionId:guid}/item-scores")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<InspectionResponse>> SaveItemScores(
        Guid inspectionId,
        [FromBody] SaveInspectionItemScoresRequest request,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            return ValidationProblem("InspectionId is required.");

        var response = await _service.SaveItemScoresAsync(inspectionId, request, cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    [HttpPost("{inspectionId:guid}/red-flags")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<InspectionResponse>> CreateRedFlag(
        Guid inspectionId,
        [FromBody] CreateInspectionRedFlagRequest request,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            return ValidationProblem("InspectionId is required.");

        var response = await _service.CreateRedFlagAsync(inspectionId, request, cancellationToken);

        return response is null
            ? NotFound()
            : CreatedAtAction(nameof(GetById), new { inspectionId }, response);
    }

    [HttpDelete("{inspectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Delete(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(inspectionId, cancellationToken);

        return deleted
            ? NoContent()
            : NotFound();
    }
}
```


## `src/ONE_Classics_Nexus.API/Services/InspectionApiService.cs`

```csharp
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Inspections;
using ONE_Classics_Nexus.Application.Abstractions;

namespace ONE_Classics_Nexus.API.Services;

public sealed class InspectionApiService : IInspectionApiService
{
    private static readonly HashSet<string> ValidInspectionTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Standard",
        "Custom",
        "VintageClassic"
    };

    private readonly IInspectionRepository _repository;

    public InspectionApiService(IInspectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<InspectionResponse> CreateAsync(
        CreateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        ValidateInspectionType(request.InspectionType);

        var now = DateTime.UtcNow;

        var record = new InspectionRecord(
            InspectionId: Guid.NewGuid(),
            AppraisalId: request.AppraisalId,
            InspectionTemplateId: request.InspectionTemplateId,
            InspectedByUserId: request.InspectedByUserId,
            InspectionType: NormalizeInspectionType(request.InspectionType),
            Status: string.IsNullOrWhiteSpace(request.Status) ? "Draft" : request.Status.Trim(),
            InspectionLocation: NullIfWhiteSpace(request.InspectionLocation),
            InspectedOnUtc: request.InspectedOnUtc,
            OverallConditionScore: null,
            HasRedFlags: false,
            Notes: NullIfWhiteSpace(request.Notes),
            RowVersion: 1,
            CreatedAtUtc: now,
            UpdatedAtUtc: now);

        await _repository.CreateAsync(record, cancellationToken);

        var aggregate = await _repository.GetByIdAsync(record.InspectionId, cancellationToken);

        return MapToResponse(aggregate ?? new InspectionAggregate(
            record,
            Array.Empty<InspectionSectionScoreRecord>(),
            Array.Empty<InspectionItemScoreRecord>(),
            Array.Empty<InspectionRedFlagRecord>()));
    }

    public async Task<InspectionResponse?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var aggregate = await _repository.GetByIdAsync(inspectionId, cancellationToken);
        return aggregate is null ? null : MapToResponse(aggregate);
    }

    public async Task<InspectionResponse?> UpdateAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.InspectionType))
            ValidateInspectionType(request.InspectionType);

        var update = new InspectionUpdate(
            InspectionTemplateId: request.InspectionTemplateId,
            InspectedByUserId: request.InspectedByUserId,
            InspectionType: string.IsNullOrWhiteSpace(request.InspectionType) ? null : NormalizeInspectionType(request.InspectionType),
            Status: NullIfWhiteSpace(request.Status),
            InspectionLocation: NullIfWhiteSpace(request.InspectionLocation),
            InspectedOnUtc: request.InspectedOnUtc,
            Notes: request.Notes);

        var updated = await _repository.UpdateAsync(inspectionId, update, cancellationToken);
        if (!updated)
            return null;

        var aggregate = await _repository.GetByIdAsync(inspectionId, cancellationToken);
        return aggregate is null ? null : MapToResponse(aggregate);
    }

    public async Task<InspectionResponse?> SaveSectionScoresAsync(
        Guid inspectionId,
        SaveInspectionSectionScoresRequest request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var scores = request.Scores.Select(score =>
        {
            var weightedScore = decimal.Round(score.Score0To5 * score.Weight, 4, MidpointRounding.AwayFromZero);

            return new InspectionSectionScoreRecord(
                InspectionSectionScoreId: Guid.NewGuid(),
                InspectionId: inspectionId,
                InspectionTemplateSectionId: score.InspectionTemplateSectionId,
                Score0To5: score.Score0To5,
                Weight: score.Weight,
                WeightedScore: weightedScore,
                Notes: NullIfWhiteSpace(score.Notes),
                CreatedAtUtc: now,
                UpdatedAtUtc: now);
        }).ToArray();

        var saved = await _repository.SaveSectionScoresAsync(inspectionId, scores, cancellationToken);
        if (!saved)
            return null;

        var aggregate = await _repository.GetByIdAsync(inspectionId, cancellationToken);
        return aggregate is null ? null : MapToResponse(aggregate);
    }

    public async Task<InspectionResponse?> SaveItemScoresAsync(
        Guid inspectionId,
        SaveInspectionItemScoresRequest request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var scores = request.Scores.Select(score => new InspectionItemScoreRecord(
            InspectionItemScoreId: Guid.NewGuid(),
            InspectionId: inspectionId,
            InspectionTemplateItemId: score.InspectionTemplateItemId,
            Score0To5: score.Score0To5,
            Notes: NullIfWhiteSpace(score.Notes),
            CreatedAtUtc: now,
            UpdatedAtUtc: now)).ToArray();

        var saved = await _repository.SaveItemScoresAsync(inspectionId, scores, cancellationToken);
        if (!saved)
            return null;

        var aggregate = await _repository.GetByIdAsync(inspectionId, cancellationToken);
        return aggregate is null ? null : MapToResponse(aggregate);
    }

    public async Task<InspectionResponse?> CreateRedFlagAsync(
        Guid inspectionId,
        CreateInspectionRedFlagRequest request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _repository.GetByIdAsync(inspectionId, cancellationToken);
        if (aggregate is null)
            return null;

        var now = DateTime.UtcNow;

        var redFlag = new InspectionRedFlagRecord(
            InspectionRedFlagId: Guid.NewGuid(),
            InspectionId: inspectionId,
            InspectionTemplateSectionId: request.InspectionTemplateSectionId,
            InspectionTemplateItemId: request.InspectionTemplateItemId,
            RedFlagType: request.RedFlagType.Trim(),
            Severity: request.Severity.Trim(),
            Description: request.Description.Trim(),
            IsResolved: false,
            CreatedAtUtc: now,
            UpdatedAtUtc: now);

        var created = await _repository.CreateRedFlagAsync(redFlag, cancellationToken);
        if (!created)
            return null;

        var updatedAggregate = await _repository.GetByIdAsync(inspectionId, cancellationToken);
        return updatedAggregate is null ? null : MapToResponse(updatedAggregate);
    }

    public Task<bool> DeleteAsync(Guid inspectionId, CancellationToken cancellationToken)
    {
        return _repository.DeleteAsync(inspectionId, cancellationToken);
    }

    private static InspectionResponse MapToResponse(InspectionAggregate aggregate)
    {
        return new InspectionResponse
        {
            InspectionId = aggregate.Inspection.InspectionId,
            AppraisalId = aggregate.Inspection.AppraisalId,
            InspectionTemplateId = aggregate.Inspection.InspectionTemplateId,
            InspectedByUserId = aggregate.Inspection.InspectedByUserId,
            InspectionType = aggregate.Inspection.InspectionType,
            Status = aggregate.Inspection.Status,
            InspectionLocation = aggregate.Inspection.InspectionLocation,
            InspectedOnUtc = aggregate.Inspection.InspectedOnUtc,
            OverallConditionScore = aggregate.Inspection.OverallConditionScore,
            HasRedFlags = aggregate.Inspection.HasRedFlags,
            Notes = aggregate.Inspection.Notes,
            RowVersion = aggregate.Inspection.RowVersion,
            CreatedAtUtc = aggregate.Inspection.CreatedAtUtc,
            UpdatedAtUtc = aggregate.Inspection.UpdatedAtUtc,
            SectionScores = aggregate.SectionScores.Select(MapSectionScore).ToArray(),
            ItemScores = aggregate.ItemScores.Select(MapItemScore).ToArray(),
            RedFlags = aggregate.RedFlags.Select(MapRedFlag).ToArray()
        };
    }

    private static InspectionSectionScoreResponse MapSectionScore(InspectionSectionScoreRecord score)
    {
        return new InspectionSectionScoreResponse
        {
            InspectionSectionScoreId = score.InspectionSectionScoreId,
            InspectionId = score.InspectionId,
            InspectionTemplateSectionId = score.InspectionTemplateSectionId,
            Score0To5 = score.Score0To5,
            Weight = score.Weight,
            WeightedScore = score.WeightedScore,
            Notes = score.Notes,
            CreatedAtUtc = score.CreatedAtUtc,
            UpdatedAtUtc = score.UpdatedAtUtc
        };
    }

    private static InspectionItemScoreResponse MapItemScore(InspectionItemScoreRecord score)
    {
        return new InspectionItemScoreResponse
        {
            InspectionItemScoreId = score.InspectionItemScoreId,
            InspectionId = score.InspectionId,
            InspectionTemplateItemId = score.InspectionTemplateItemId,
            Score0To5 = score.Score0To5,
            Notes = score.Notes,
            CreatedAtUtc = score.CreatedAtUtc,
            UpdatedAtUtc = score.UpdatedAtUtc
        };
    }

    private static InspectionRedFlagResponse MapRedFlag(InspectionRedFlagRecord redFlag)
    {
        return new InspectionRedFlagResponse
        {
            InspectionRedFlagId = redFlag.InspectionRedFlagId,
            InspectionId = redFlag.InspectionId,
            InspectionTemplateSectionId = redFlag.InspectionTemplateSectionId,
            InspectionTemplateItemId = redFlag.InspectionTemplateItemId,
            RedFlagType = redFlag.RedFlagType,
            Severity = redFlag.Severity,
            Description = redFlag.Description,
            IsResolved = redFlag.IsResolved,
            CreatedAtUtc = redFlag.CreatedAtUtc,
            UpdatedAtUtc = redFlag.UpdatedAtUtc
        };
    }

    private static void ValidateInspectionType(string? inspectionType)
    {
        if (string.IsNullOrWhiteSpace(inspectionType) || !ValidInspectionTypes.Contains(inspectionType.Trim()))
            throw new ArgumentException("InspectionType must be one of: Standard, Custom, VintageClassic.");
    }

    private static string NormalizeInspectionType(string inspectionType)
    {
        var value = inspectionType.Trim();

        return ValidInspectionTypes.First(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    private static string? NullIfWhiteSpace(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
```


## `src/ONE_Classics_Nexus.Application/Abstractions/IInspectionRepository.cs`

```csharp
namespace ONE_Classics_Nexus.Application.Abstractions;

public interface IInspectionRepository
{
    Task<InspectionAggregate?> GetByIdAsync(Guid inspectionId, CancellationToken cancellationToken);
    Task<InspectionRecord> CreateAsync(InspectionRecord inspection, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid inspectionId, InspectionUpdate update, CancellationToken cancellationToken);
    Task<bool> SaveSectionScoresAsync(Guid inspectionId, IReadOnlyList<InspectionSectionScoreRecord> scores, CancellationToken cancellationToken);
    Task<bool> SaveItemScoresAsync(Guid inspectionId, IReadOnlyList<InspectionItemScoreRecord> scores, CancellationToken cancellationToken);
    Task<bool> CreateRedFlagAsync(InspectionRedFlagRecord redFlag, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid inspectionId, CancellationToken cancellationToken);
}

public sealed record InspectionAggregate(
    InspectionRecord Inspection,
    IReadOnlyList<InspectionSectionScoreRecord> SectionScores,
    IReadOnlyList<InspectionItemScoreRecord> ItemScores,
    IReadOnlyList<InspectionRedFlagRecord> RedFlags);

public sealed record InspectionRecord(
    Guid InspectionId,
    Guid AppraisalId,
    Guid InspectionTemplateId,
    Guid? InspectedByUserId,
    string InspectionType,
    string Status,
    string? InspectionLocation,
    DateTime? InspectedOnUtc,
    decimal? OverallConditionScore,
    bool HasRedFlags,
    string? Notes,
    long RowVersion,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record InspectionUpdate(
    Guid? InspectionTemplateId,
    Guid? InspectedByUserId,
    string? InspectionType,
    string? Status,
    string? InspectionLocation,
    DateTime? InspectedOnUtc,
    string? Notes);

public sealed record InspectionSectionScoreRecord(
    Guid InspectionSectionScoreId,
    Guid InspectionId,
    Guid InspectionTemplateSectionId,
    decimal Score0To5,
    decimal Weight,
    decimal WeightedScore,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record InspectionItemScoreRecord(
    Guid InspectionItemScoreId,
    Guid InspectionId,
    Guid InspectionTemplateItemId,
    decimal Score0To5,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record InspectionRedFlagRecord(
    Guid InspectionRedFlagId,
    Guid InspectionId,
    Guid? InspectionTemplateSectionId,
    Guid? InspectionTemplateItemId,
    string RedFlagType,
    string Severity,
    string Description,
    bool IsResolved,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
```


## `src/ONE_Classics_Nexus.Infrastructure/Repositories/InspectionRepository.cs`

```csharp
using System.Data;
using Dapper;
using ONE_Classics_Nexus.Application.Abstractions;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class InspectionRepository : IInspectionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public InspectionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<InspectionAggregate?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var inspection = await connection.QuerySingleOrDefaultAsync<InspectionRecord>(
            new CommandDefinition(
                """
                SELECT
                    inspection_id AS InspectionId,
                    appraisal_id AS AppraisalId,
                    inspection_template_id AS InspectionTemplateId,
                    inspected_by_user_id AS InspectedByUserId,
                    inspection_type AS InspectionType,
                    status AS Status,
                    inspection_location AS InspectionLocation,
                    inspected_on_utc AS InspectedOnUtc,
                    overall_condition_score AS OverallConditionScore,
                    has_red_flags AS HasRedFlags,
                    notes AS Notes,
                    row_version AS RowVersion,
                    created_at_utc AS CreatedAtUtc,
                    updated_at_utc AS UpdatedAtUtc
                FROM inspections
                WHERE inspection_id = @InspectionId;
                """,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: cancellationToken));

        if (inspection is null)
            return null;

        var sectionScores = (await connection.QueryAsync<InspectionSectionScoreRecord>(
            new CommandDefinition(
                """
                SELECT
                    inspection_section_score_id AS InspectionSectionScoreId,
                    inspection_id AS InspectionId,
                    inspection_template_section_id AS InspectionTemplateSectionId,
                    score_0_to_5 AS Score0To5,
                    weight AS Weight,
                    weighted_score AS WeightedScore,
                    notes AS Notes,
                    created_at_utc AS CreatedAtUtc,
                    updated_at_utc AS UpdatedAtUtc
                FROM inspection_section_scores
                WHERE inspection_id = @InspectionId
                ORDER BY created_at_utc, inspection_section_score_id;
                """,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: cancellationToken))).ToArray();

        var itemScores = (await connection.QueryAsync<InspectionItemScoreRecord>(
            new CommandDefinition(
                """
                SELECT
                    inspection_item_score_id AS InspectionItemScoreId,
                    inspection_id AS InspectionId,
                    inspection_template_item_id AS InspectionTemplateItemId,
                    score_0_to_5 AS Score0To5,
                    notes AS Notes,
                    created_at_utc AS CreatedAtUtc,
                    updated_at_utc AS UpdatedAtUtc
                FROM inspection_item_scores
                WHERE inspection_id = @InspectionId
                ORDER BY created_at_utc, inspection_item_score_id;
                """,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: cancellationToken))).ToArray();

        var redFlags = (await connection.QueryAsync<InspectionRedFlagRecord>(
            new CommandDefinition(
                """
                SELECT
                    inspection_red_flag_id AS InspectionRedFlagId,
                    inspection_id AS InspectionId,
                    inspection_template_section_id AS InspectionTemplateSectionId,
                    inspection_template_item_id AS InspectionTemplateItemId,
                    red_flag_type AS RedFlagType,
                    severity AS Severity,
                    description AS Description,
                    is_resolved AS IsResolved,
                    created_at_utc AS CreatedAtUtc,
                    updated_at_utc AS UpdatedAtUtc
                FROM inspection_red_flags
                WHERE inspection_id = @InspectionId
                ORDER BY created_at_utc, inspection_red_flag_id;
                """,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: cancellationToken))).ToArray();

        return new InspectionAggregate(inspection, sectionScores, itemScores, redFlags);
    }

    public async Task<InspectionRecord> CreateAsync(
        InspectionRecord inspection,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            new CommandDefinition(
                """
                INSERT INTO inspections
                (
                    inspection_id,
                    appraisal_id,
                    inspection_template_id,
                    inspected_by_user_id,
                    inspection_type,
                    status,
                    inspection_location,
                    inspected_on_utc,
                    overall_condition_score,
                    has_red_flags,
                    notes,
                    row_version,
                    created_at_utc,
                    updated_at_utc
                )
                VALUES
                (
                    @InspectionId,
                    @AppraisalId,
                    @InspectionTemplateId,
                    @InspectedByUserId,
                    @InspectionType,
                    @Status,
                    @InspectionLocation,
                    @InspectedOnUtc,
                    @OverallConditionScore,
                    @HasRedFlags,
                    @Notes,
                    @RowVersion,
                    @CreatedAtUtc,
                    @UpdatedAtUtc
                );
                """,
                ToDbParameters(inspection),
                cancellationToken: cancellationToken));

        return inspection;
    }

    public async Task<bool> UpdateAsync(
        Guid inspectionId,
        InspectionUpdate update,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var affected = await connection.ExecuteAsync(
            new CommandDefinition(
                """
                UPDATE inspections
                SET
                    inspection_template_id = COALESCE(@InspectionTemplateId, inspection_template_id),
                    inspected_by_user_id = @InspectedByUserId,
                    inspection_type = COALESCE(@InspectionType, inspection_type),
                    status = COALESCE(@Status, status),
                    inspection_location = @InspectionLocation,
                    inspected_on_utc = @InspectedOnUtc,
                    notes = @Notes,
                    row_version = row_version + 1,
                    updated_at_utc = @UpdatedAtUtc
                WHERE inspection_id = @InspectionId;
                """,
                new
                {
                    InspectionId = inspectionId.ToString(),
                    InspectionTemplateId = update.InspectionTemplateId?.ToString(),
                    InspectedByUserId = update.InspectedByUserId?.ToString(),
                    update.InspectionType,
                    update.Status,
                    update.InspectionLocation,
                    update.InspectedOnUtc,
                    update.Notes,
                    UpdatedAtUtc = DateTime.UtcNow
                },
                cancellationToken: cancellationToken));

        return affected == 1;
    }

    public async Task<bool> SaveSectionScoresAsync(
        Guid inspectionId,
        IReadOnlyList<InspectionSectionScoreRecord> scores,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        EnsureOpen(connection);

        using var transaction = connection.BeginTransaction();

        try
        {
            var exists = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    """
                    SELECT COUNT(1)
                    FROM inspections
                    WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = inspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            if (exists == 0)
            {
                transaction.Rollback();
                return false;
            }

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    DELETE FROM inspection_section_scores
                    WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = inspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            foreach (var score in scores)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition(
                        """
                        INSERT INTO inspection_section_scores
                        (
                            inspection_section_score_id,
                            inspection_id,
                            inspection_template_section_id,
                            score_0_to_5,
                            weight,
                            weighted_score,
                            notes,
                            created_at_utc,
                            updated_at_utc
                        )
                        VALUES
                        (
                            @InspectionSectionScoreId,
                            @InspectionId,
                            @InspectionTemplateSectionId,
                            @Score0To5,
                            @Weight,
                            @WeightedScore,
                            @Notes,
                            @CreatedAtUtc,
                            @UpdatedAtUtc
                        );
                        """,
                        ToDbParameters(score),
                        transaction,
                        cancellationToken: cancellationToken));
            }

            var overallConditionScore = scores.Count == 0
                ? (decimal?)null
                : decimal.Round(scores.Sum(x => x.WeightedScore) * 20m, 2, MidpointRounding.AwayFromZero);

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    UPDATE inspections
                    SET
                        overall_condition_score = @OverallConditionScore,
                        row_version = row_version + 1,
                        updated_at_utc = @UpdatedAtUtc
                    WHERE inspection_id = @InspectionId;
                    """,
                    new
                    {
                        InspectionId = inspectionId.ToString(),
                        OverallConditionScore = overallConditionScore,
                        UpdatedAtUtc = DateTime.UtcNow
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> SaveItemScoresAsync(
        Guid inspectionId,
        IReadOnlyList<InspectionItemScoreRecord> scores,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        EnsureOpen(connection);

        using var transaction = connection.BeginTransaction();

        try
        {
            var exists = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    """
                    SELECT COUNT(1)
                    FROM inspections
                    WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = inspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            if (exists == 0)
            {
                transaction.Rollback();
                return false;
            }

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    DELETE FROM inspection_item_scores
                    WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = inspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            foreach (var score in scores)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition(
                        """
                        INSERT INTO inspection_item_scores
                        (
                            inspection_item_score_id,
                            inspection_id,
                            inspection_template_item_id,
                            score_0_to_5,
                            notes,
                            created_at_utc,
                            updated_at_utc
                        )
                        VALUES
                        (
                            @InspectionItemScoreId,
                            @InspectionId,
                            @InspectionTemplateItemId,
                            @Score0To5,
                            @Notes,
                            @CreatedAtUtc,
                            @UpdatedAtUtc
                        );
                        """,
                        ToDbParameters(score),
                        transaction,
                        cancellationToken: cancellationToken));
            }

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    UPDATE inspections
                    SET
                        row_version = row_version + 1,
                        updated_at_utc = @UpdatedAtUtc
                    WHERE inspection_id = @InspectionId;
                    """,
                    new
                    {
                        InspectionId = inspectionId.ToString(),
                        UpdatedAtUtc = DateTime.UtcNow
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> CreateRedFlagAsync(
        InspectionRedFlagRecord redFlag,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        EnsureOpen(connection);

        using var transaction = connection.BeginTransaction();

        try
        {
            var exists = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    """
                    SELECT COUNT(1)
                    FROM inspections
                    WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = redFlag.InspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            if (exists == 0)
            {
                transaction.Rollback();
                return false;
            }

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    INSERT INTO inspection_red_flags
                    (
                        inspection_red_flag_id,
                        inspection_id,
                        inspection_template_section_id,
                        inspection_template_item_id,
                        red_flag_type,
                        severity,
                        description,
                        is_resolved,
                        created_at_utc,
                        updated_at_utc
                    )
                    VALUES
                    (
                        @InspectionRedFlagId,
                        @InspectionId,
                        @InspectionTemplateSectionId,
                        @InspectionTemplateItemId,
                        @RedFlagType,
                        @Severity,
                        @Description,
                        @IsResolved,
                        @CreatedAtUtc,
                        @UpdatedAtUtc
                    );
                    """,
                    ToDbParameters(redFlag),
                    transaction,
                    cancellationToken: cancellationToken));

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    UPDATE inspections
                    SET
                        has_red_flags = 1,
                        row_version = row_version + 1,
                        updated_at_utc = @UpdatedAtUtc
                    WHERE inspection_id = @InspectionId;
                    """,
                    new
                    {
                        InspectionId = redFlag.InspectionId.ToString(),
                        UpdatedAtUtc = DateTime.UtcNow
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        EnsureOpen(connection);

        using var transaction = connection.BeginTransaction();

        try
        {
            var exists = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    """
                    SELECT COUNT(1)
                    FROM inspections
                    WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = inspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            if (exists == 0)
            {
                transaction.Rollback();
                return false;
            }

            await connection.ExecuteAsync(
                new CommandDefinition(
                    """
                    DELETE FROM inspection_media WHERE inspection_id = @InspectionId;
                    DELETE FROM inspection_red_flags WHERE inspection_id = @InspectionId;
                    DELETE FROM inspection_item_scores WHERE inspection_id = @InspectionId;
                    DELETE FROM inspection_section_scores WHERE inspection_id = @InspectionId;
                    DELETE FROM inspections WHERE inspection_id = @InspectionId;
                    """,
                    new { InspectionId = inspectionId.ToString() },
                    transaction,
                    cancellationToken: cancellationToken));

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static object ToDbParameters(InspectionRecord inspection)
    {
        return new
        {
            InspectionId = inspection.InspectionId.ToString(),
            AppraisalId = inspection.AppraisalId.ToString(),
            InspectionTemplateId = inspection.InspectionTemplateId.ToString(),
            InspectedByUserId = inspection.InspectedByUserId?.ToString(),
            inspection.InspectionType,
            inspection.Status,
            inspection.InspectionLocation,
            inspection.InspectedOnUtc,
            inspection.OverallConditionScore,
            inspection.HasRedFlags,
            inspection.Notes,
            inspection.RowVersion,
            inspection.CreatedAtUtc,
            inspection.UpdatedAtUtc
        };
    }

    private static object ToDbParameters(InspectionSectionScoreRecord score)
    {
        return new
        {
            InspectionSectionScoreId = score.InspectionSectionScoreId.ToString(),
            InspectionId = score.InspectionId.ToString(),
            InspectionTemplateSectionId = score.InspectionTemplateSectionId.ToString(),
            score.Score0To5,
            score.Weight,
            score.WeightedScore,
            score.Notes,
            score.CreatedAtUtc,
            score.UpdatedAtUtc
        };
    }

    private static object ToDbParameters(InspectionItemScoreRecord score)
    {
        return new
        {
            InspectionItemScoreId = score.InspectionItemScoreId.ToString(),
            InspectionId = score.InspectionId.ToString(),
            InspectionTemplateItemId = score.InspectionTemplateItemId.ToString(),
            score.Score0To5,
            score.Notes,
            score.CreatedAtUtc,
            score.UpdatedAtUtc
        };
    }

    private static object ToDbParameters(InspectionRedFlagRecord redFlag)
    {
        return new
        {
            InspectionRedFlagId = redFlag.InspectionRedFlagId.ToString(),
            InspectionId = redFlag.InspectionId.ToString(),
            InspectionTemplateSectionId = redFlag.InspectionTemplateSectionId?.ToString(),
            InspectionTemplateItemId = redFlag.InspectionTemplateItemId?.ToString(),
            redFlag.RedFlagType,
            redFlag.Severity,
            redFlag.Description,
            redFlag.IsResolved,
            redFlag.CreatedAtUtc,
            redFlag.UpdatedAtUtc
        };
    }

    private static void EnsureOpen(IDbConnection connection)
    {
        if (connection.State != ConnectionState.Open)
            connection.Open();
    }
}
```


## `src/ONE_Classics_Nexus.Infrastructure/DependencyInjection/InspectionDependencyInjection.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Services;
using ONE_Classics_Nexus.Application.Abstractions;
using ONE_Classics_Nexus.Infrastructure.Repositories;

namespace ONE_Classics_Nexus.Infrastructure.DependencyInjection;

public static class InspectionDependencyInjection
{
    public static IServiceCollection AddInspectionApi(this IServiceCollection services)
    {
        services.AddScoped<IInspectionRepository, InspectionRepository>();
        services.AddScoped<IInspectionApiService, InspectionApiService>();

        return services;
    }
}
```
