using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Domain.Valuation.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ONE_Classics_Nexus.Domain.Valuation.Engines;

/// <summary>
/// Default deterministic confidence engine.
/// 
/// Confidence is derived from three primary signals:
/// 1. Comparable count sufficiency
/// 2. Average comparable similarity
/// 3. Inspection completeness
/// 
/// Phase 2 goal:
/// Produce a transparent, explainable confidence score on a 0.00 to 1.00 scale.
/// </summary>
public sealed class DefaultConfidenceEngine : IConfidenceEngine
{
    private const decimal ComparableCountWeight = 0.35m;
    private const decimal AverageSimilarityWeight = 0.40m;
    private const decimal InspectionCompletenessWeight = 0.25m;

    private const decimal TargetComparableCount = 5m;

    /// <inheritdoc />
    public decimal CalculateConfidence0To1(
        IReadOnlyList<AdjustedComparable> adjustedComparables,
        IReadOnlyList<ConditionScore> conditionScores)
    {
        adjustedComparables ??= Array.Empty<AdjustedComparable>();
        conditionScores ??= Array.Empty<ConditionScore>();

        decimal comparableCountScore = CalculateComparableCountScore(adjustedComparables);
        decimal averageSimilarityScore = CalculateAverageSimilarityScore(adjustedComparables);
        decimal inspectionCompletenessScore = CalculateInspectionCompletenessScore(conditionScores);

        decimal confidence =
            (comparableCountScore * ComparableCountWeight) +
            (averageSimilarityScore * AverageSimilarityWeight) +
            (inspectionCompletenessScore * InspectionCompletenessWeight);

        return Clamp(confidence, 0m, 1m);
    }

    /// <summary>
    /// Scores the sufficiency of the comparable count.
    /// Full credit is reached at the target comparable count.
    /// </summary>
    private static decimal CalculateComparableCountScore(
        IReadOnlyList<AdjustedComparable> adjustedComparables)
    {
        if (adjustedComparables.Count == 0)
        {
            return 0m;
        }

        decimal score = adjustedComparables.Count / TargetComparableCount;
        return Clamp(score, 0m, 1m);
    }

    /// <summary>
    /// Scores the average similarity of the comparable set.
    /// </summary>
    private static decimal CalculateAverageSimilarityScore(
        IReadOnlyList<AdjustedComparable> adjustedComparables)
    {
        if (adjustedComparables.Count == 0)
        {
            return 0m;
        }

        decimal averageSimilarity = adjustedComparables.Average(x => x.SimilarityScore0To1);
        return Clamp(averageSimilarity, 0m, 1m);
    }

    /// <summary>
    /// Scores inspection completeness based on distinct weighted systems present.
    /// </summary>
    private static decimal CalculateInspectionCompletenessScore(
        IReadOnlyList<ConditionScore> conditionScores)
    {
        if (conditionScores.Count == 0)
        {
            return 0m;
        }

        int distinctSystemsWithWeight = conditionScores
            .Where(x => x.Weight0To1 > 0m && !string.IsNullOrWhiteSpace(x.SystemKey))
            .Select(x => x.SystemKey.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        decimal score = distinctSystemsWithWeight / (decimal)ValuationRuleConstants.ExpectedSystemCount;
        return Clamp(score, 0m, 1m);
    }

    /// <summary>
    /// Clamps a decimal value to the supplied minimum and maximum.
    /// </summary>
    private static decimal Clamp(decimal value, decimal min, decimal max)
        => Math.Min(max, Math.Max(min, value));
}