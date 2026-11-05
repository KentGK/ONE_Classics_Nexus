using System;
using System.Collections.Generic;
using System.Drawing;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections.Contracts;

public sealed class InspectionListItemResponse
{
    public Guid InspectionId { get; set; }
    public Guid AppraisalId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string InspectionType { get; set; } = string.Empty;
    public string? VehicleDisplayName { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public string DisplayText =>
        $"{InspectionId:D}  |  {(UpdatedAtUtc ?? CreatedAtUtc).ToLocalTime():d}  |  {Status}";
}

public sealed class InspectionWorkspaceResponse
{
    public Guid InspectionId { get; set; }
    public Guid AppraisalId { get; set; }
    public Guid InspectionTemplateId { get; set; }
    public Guid? InspectedByUserId { get; set; }
    public string? InspectedByDisplayName { get; set; }
    public string InspectionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? TemplateName { get; set; }
    public string? InspectionLocation { get; set; }
    public DateTime? InspectedOnUtc { get; set; }
    public string? SummaryNotes { get; set; }
    public decimal? CompletionPercentage { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }
    public long RowVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public int? ItemsScored { get; set; }
    public int? TotalItems { get; set; }
    public AppraisalSummaryResponse? Appraisal { get; set; }
    public VehicleSummaryResponse? Vehicle { get; set; }
    public IReadOnlyCollection<ConditionScoreResponse> ConditionScores { get; set; } =
        Array.Empty<ConditionScoreResponse>();
    public IReadOnlyCollection<SectionSummaryResponse> SectionSummaries { get; set; } =
        Array.Empty<SectionSummaryResponse>();
    public IReadOnlyCollection<RedFlagSummaryResponse> RedFlags { get; set; } =
        Array.Empty<RedFlagSummaryResponse>();
    public MediaSummaryResponse? MediaSummary { get; set; }
}

public sealed class AppraisalSummaryResponse
{
    public Guid AppraisalId { get; set; }
    public string? AppraisalNumber { get; set; }
    public Guid VehicleId { get; set; }
    public string? Status { get; set; }
    public string? AppraisalType { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
    public string? ClientName { get; set; }
}

public sealed class VehicleSummaryResponse
{
    public Guid VehicleId { get; set; }
    public string? Vin { get; set; }
    public int Year { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Drivetrain { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public int? Mileage { get; set; }
    public string? OdometerUnit { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Image? Image { get; set; }
}

public sealed class ConditionScoreResponse
{
    public Guid? ConditionScoreId { get; set; }
    public Guid InspectionId { get; set; }
    public int SystemNumber { get; set; }
    public string? SystemName { get; set; }
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
}

public sealed class SectionSummaryResponse
{
    public int SystemNumber { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public int ItemsScored { get; set; }
    public int TotalItems { get; set; }
    public int RedFlagCount { get; set; }
}

public sealed class RedFlagSummaryResponse
{
    public Guid RedFlagId { get; set; }
    public int? SystemNumber { get; set; }
    public string? SystemName { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? ItemName { get; set; }
    public string? Description { get; set; }
}

public sealed class MediaSummaryResponse
{
    public int PhotoCount { get; set; }
    public int VideoCount { get; set; }
    public int DocumentCount { get; set; }
    public int OtherFileCount { get; set; }
}

public sealed class UpdateInspectionRequest
{
    public Guid InspectionTemplateId { get; set; }
    public Guid? InspectedByUserId { get; set; }
    public string InspectionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? InspectionLocation { get; set; }
    public DateTime? InspectedOnUtc { get; set; }
    public string? SummaryNotes { get; set; }
    public long RowVersion { get; set; }
}

public sealed class SaveConditionScoresRequest
{
    public List<ConditionScoreItemRequest> Scores { get; set; } = new();
}

public sealed class ConditionScoreItemRequest
{
    public Guid? ConditionScoreId { get; set; }
    public int SystemNumber { get; set; }
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
}

public sealed class ProblemDetailsResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
}
