namespace ONE_Classics_Nexus.API.Contracts.Inspections;

public sealed record CreateInspectionRequest(
    Guid AppraisalId,
    Guid InspectionTemplateId,
    Guid? InspectedByUserId,
    string InspectionType,
    string Status,
    string? InspectionLocation,
    DateTime? InspectedOnUtc,
    decimal? CompletionPercentage,
    decimal? OverallConditionScore,
    bool HasRedFlags,
    string? SummaryNotes,
    long RowVersion,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record UpdateInspectionRequest(
    Guid InspectionTemplateId,
    Guid? InspectedByUserId,
    string InspectionType,
    string Status,
    string? InspectionLocation,
    DateTime? InspectedOnUtc,
    decimal? CompletionPercentage,
    decimal? OverallConditionScore,
    bool HasRedFlags,
    string? SummaryNotes,
    long RowVersion);

public sealed record SaveInspectionScoresRequest(
    IReadOnlyList<InspectionScoreDto> Scores);

public sealed record InspectionScoreDto(
    int SystemNumber,
    string SystemKey,
    decimal Score0To5,
    decimal Weight,
    bool IsRedFlag,
    string? Notes);
