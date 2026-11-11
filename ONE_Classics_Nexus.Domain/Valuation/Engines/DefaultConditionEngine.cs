using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Domain.Valuation.Rules;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Valuation.Engines;

public sealed class DefaultConditionEngine : IConditionEngine
{
    public decimal CalculateConditionIndex0To100(IReadOnlyList<ConditionScore> scores)
    {
        if (scores is null || scores.Count == 0)
            return 0m;

        decimal totalWeight = scores.Sum(x => x.Weight0To1);
        if (totalWeight <= 0m)
            return 0m;

        decimal weightedScore = scores.Sum(x => x.Score0To10 * x.Weight0To1);
        decimal raw0To10 = weightedScore / totalWeight;
        decimal index0To100 = raw0To10 * 10m;

        int redFlagCount = scores.Count(x => x.IsRedFlag);
        decimal redFlagPenalty = Math.Min(
            ValuationRuleConstants.MaxRedFlagPenalty,
            redFlagCount * ValuationRuleConstants.RedFlagPenaltyPerItem);

        return Clamp(index0To100 - redFlagPenalty, 0m, 100m);
    }

    private static decimal Clamp(decimal value, decimal min, decimal max)
        => Math.Min(max, Math.Max(min, value));
}