using ONE_Classics_Nexus.Domain.Valuation.Models;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface IConfidenceEngine
{
    decimal CalculateConfidence0To1(
        IReadOnlyList<AdjustedComparable> adjustedComparables,
        IReadOnlyList<ConditionScore> conditionScores);
}