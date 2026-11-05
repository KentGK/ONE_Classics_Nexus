namespace ONE_Classics_Nexus.API.Contracts.ConditionScores;

public sealed class ConditionScoreListResponse
{
    public List<ConditionScoreResponse> Scores { get; set; } = new();
}