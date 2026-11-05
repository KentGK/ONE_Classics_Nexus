namespace ONE_Classics_Nexus.API.Contracts.ConditionScores;

public sealed class CreateConditionScoreRequest
{
    public int SystemNumber { get; set; }
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
}