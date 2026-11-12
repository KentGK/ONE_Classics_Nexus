using ONE_Classics_Nexus.Domain.Entities;
using Xunit;

namespace ONE_Classics_Nexus.Tests.Domain.Inspections;

public sealed class ConditionScoreTimestampTests
{
    [Fact]
    public void CreateInitializesBothUtcTimestampsForPersistence()
    {
        var before = DateTime.UtcNow;

        var score = ConditionScore.Create(Guid.NewGuid(), 1, "Body", 3m, 1m);

        var after = DateTime.UtcNow;
        Assert.InRange(score.CreatedAtUtc, before, after);
        Assert.Equal(DateTimeKind.Utc, score.CreatedAtUtc.Kind);
        Assert.Equal(score.CreatedAtUtc, score.UpdatedAtUtc);
    }

    [Fact]
    public void UpdateAdvancesOnlyUpdatedAtUtc()
    {
        var score = ConditionScore.Create(Guid.NewGuid(), 1, "Body", 3m, 1m);
        var createdAtUtc = score.CreatedAtUtc;
        score.UpdatedAtUtc = createdAtUtc.AddMinutes(-1);

        score.Update(4m, 2m, true, "Reviewed");

        Assert.Equal(createdAtUtc, score.CreatedAtUtc);
        Assert.True(score.UpdatedAtUtc >= createdAtUtc);
        Assert.Equal(DateTimeKind.Utc, score.UpdatedAtUtc.Kind);
        Assert.Equal(4m, score.Score0To5);
    }

    [Fact]
    public void ReplacingInspectionScoresPreservesTheirTimestamps()
    {
        var inspection = Inspection.Create(Guid.NewGuid());
        var score = ConditionScore.Create(inspection.InspectionId, 1, "Body", 3m, 1m);
        var createdAtUtc = score.CreatedAtUtc;
        var updatedAtUtc = score.UpdatedAtUtc;

        inspection.ReplaceScores(new[] { score });

        var savedScore = Assert.Single(inspection.ConditionScores);
        Assert.Equal(createdAtUtc, savedScore.CreatedAtUtc);
        Assert.Equal(updatedAtUtc, savedScore.UpdatedAtUtc);
    }
}
