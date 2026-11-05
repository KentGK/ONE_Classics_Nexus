using ONE_Classics_Nexus.Domain.Valuation.Engines;
using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Tests.Domain.Valuation;

public sealed class DefaultConfidenceEngineTests
{
    [Fact]
    public void CalculateConfidence0To1_Returns_Clamped_Value()
    {
        var sut = new DefaultConfidenceEngine();

        var adjusted = new[]
        {
            new AdjustedComparable(
                new ComparableSale(Guid.NewGuid(), 1970, "Chevrolet", "K10", null, null, null, null, null, null, 50000m, DateTime.UtcNow, "BAT", null, null, null, null),
                0.90m,
                Array.Empty<ComparableAdjustment>(),
                50000m)
        };

        var conditionScores = new[]
        {
            new ConditionScore("Body", 8m, 1m, false, null)
        };

        decimal result = sut.CalculateConfidence0To1(adjusted, conditionScores);

        Assert.InRange(result, 0m, 1m);
    }
}