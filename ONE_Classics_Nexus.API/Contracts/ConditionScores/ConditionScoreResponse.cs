namespace ONE_Classics_Nexus.API.Contracts.ConditionScores;

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
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}