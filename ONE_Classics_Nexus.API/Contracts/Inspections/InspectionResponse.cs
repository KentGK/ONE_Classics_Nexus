namespace ONE_Classics_Nexus.API.Contracts.Inspections;

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
    public decimal? CompletionPercentage { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }
    public string? SummaryNotes { get; set; }
    public long RowVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public IReadOnlyList<InspectionScoreDto> Scores { get; set; } = Array.Empty<InspectionScoreDto>();

    public string DisplayLabel =>
        $"{(InspectedOnUtc.HasValue ? InspectedOnUtc.Value.ToLocalTime().ToString("yyyy-MM-dd") : "Undated")} - " +
        $"{InspectionType} - {Status} - {InspectionId.ToString()[..8]}";
}

public sealed class ConditionScoreResponse
{
    public Guid ConditionScoreId { get; set; }
    public Guid InspectionId { get; set; }
    public int SystemNumber { get; set; }
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
}

public sealed class InspectionListResponse
{
    public IReadOnlyList<InspectionResponse> Items { get; set; } = Array.Empty<InspectionResponse>();
}
