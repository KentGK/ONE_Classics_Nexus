using ONE_Classics_Nexus.Domain.Valuation.Engines;
using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Tests.Domain.Valuation;

public sealed class DefaultConditionEngineTests
{
    [Fact]
    public void CalculateConditionIndex0To100_Returns_Zero_When_No_Scores()
    {
        var sut = new DefaultConditionEngine();

        decimal result = sut.CalculateConditionIndex0To100(Array.Empty<ConditionScore>());

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateConditionIndex0To100_Applies_Weighted_Average()
    {
        var sut = new DefaultConditionEngine();

        var scores = new[]
        {
            new ConditionScore("Body", 8m, 0.5m, false, null),
            new ConditionScore("Paint", 6m, 0.5m, false, null)
        };

        decimal result = sut.CalculateConditionIndex0To100(scores);

        Assert.Equal(70m, result);
    }
}