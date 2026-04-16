using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Domain.Valuation.Rules;
using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Valuation.Engines;

/// <summary>
/// Default deterministic valuation engine.
///
/// Responsibilities:
/// 1. Calculate subject condition index
/// 2. Adjust and score comparables
/// 3. Compute weighted indicated value
/// 4. Apply market layer
/// 5. Calculate confidence
/// 6. Produce a value range
/// 7. Emit a full valuation trace
/// </summary>
public sealed class DefaultValuationEngine : IValuationEngine
{
    private readonly IConditionEngine _conditionEngine;
    private readonly IComparableEngine _comparableEngine;
    private readonly IMarketEngine _marketEngine;
    private readonly IConfidenceEngine _confidenceEngine;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultValuationEngine"/> class.
    /// </summary>
    public DefaultValuationEngine(
        IConditionEngine conditionEngine,
        IComparableEngine comparableEngine,
        IMarketEngine marketEngine,
        IConfidenceEngine confidenceEngine)
    {
        _conditionEngine = conditionEngine;
        _comparableEngine = comparableEngine;
        _marketEngine = marketEngine;
        _confidenceEngine = confidenceEngine;
    }

    /// <inheritdoc />
    public ValuationResult Run(ValuationInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var trace = new List<ValuationTraceLine>();
        int order = 1;

        if (input.Comparables is null || input.Comparables.Count == 0)
        {
            trace.Add(CreateTraceLine(
                order++,
                type: "Error",
                key: "Comparables",
                amount: null,
                factor: null,
                message: "No comparable sales were supplied.",
                evidenceRef: null));

            return CreateInsufficientDataResult(input, trace);
        }

        decimal conditionIndex = _conditionEngine.CalculateConditionIndex0To100(
            input.ConditionScores ?? Array.Empty<ConditionScore>());

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "ConditionIndex",
            amount: conditionIndex,
            factor: null,
            message: $"Condition index calculated at {conditionIndex:0.00}/100.",
            evidenceRef: null));

        IReadOnlyList<AdjustedComparable> adjustedComparables = _comparableEngine.AdjustComparables(
            input.Subject,
            input.Comparables,
            conditionIndex);

        List<AdjustedComparable> usableComparables = adjustedComparables
            .Where(x => x.SimilarityScore0To1 > 0m)
            .OrderByDescending(x => x.SimilarityScore0To1)
            .ToList();

        if (usableComparables.Count == 0)
        {
            trace.Add(CreateTraceLine(
                order++,
                type: "Error",
                key: "UsableComparables",
                amount: null,
                factor: null,
                message: "No usable comparables remained after similarity scoring.",
                evidenceRef: null));

            return CreateInsufficientDataResult(input, trace);
        }

        foreach (AdjustedComparable comparable in usableComparables)
        {
            trace.Add(CreateTraceLine(
                order++,
                type: "Comparable",
                key: comparable.Comparable.ComparableId.ToString(),
                amount: comparable.AdjustedPrice,
                factor: comparable.SimilarityScore0To1,
                message: $"{comparable.Comparable.Year} {comparable.Comparable.Make} {comparable.Comparable.Model} adjusted to {comparable.AdjustedPrice:C}.",
                evidenceRef: comparable.Comparable.Url));
        }

        decimal totalWeight = usableComparables.Sum(x => x.SimilarityScore0To1);
        if (totalWeight <= 0m)
        {
            trace.Add(CreateTraceLine(
                order++,
                type: "Error",
                key: "ComparableWeights",
                amount: null,
                factor: null,
                message: "Comparable weights summed to zero.",
                evidenceRef: null));

            return CreateInsufficientDataResult(input, trace);
        }

        decimal weightedIndication = usableComparables.Sum(x => x.AdjustedPrice * x.SimilarityScore0To1) / totalWeight;

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "WeightedIndication",
            amount: weightedIndication,
            factor: null,
            message: "Weighted comparable indication calculated.",
            evidenceRef: null));

        decimal marketAdjustedValue = _marketEngine.ApplyMarketLayer(weightedIndication, input);
        marketAdjustedValue = Math.Max(0m, marketAdjustedValue);

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "MarketAdjustedValue",
            amount: marketAdjustedValue,
            factor: null,
            message: "Market layer applied.",
            evidenceRef: null));

        decimal confidence = _confidenceEngine.CalculateConfidence0To1(
            usableComparables,
            input.ConditionScores ?? Array.Empty<ConditionScore>());

        confidence = Clamp(confidence, 0m, 1m);

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "Confidence",
            amount: null,
            factor: confidence,
            message: $"Confidence calculated at {confidence:0.0000}.",
            evidenceRef: null));

        decimal rangeWidthPct =
            ValuationRuleConstants.BaseRangeWidthPct +
            ((1m - confidence) * ValuationRuleConstants.VariableRangeWidthPct);

        rangeWidthPct = Clamp(rangeWidthPct, 0m, 1m);

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "RangeWidthPct",
            amount: null,
            factor: rangeWidthPct,
            message: $"Value range width percentage = {rangeWidthPct:P2}.",
            evidenceRef: null));

        decimal lowValue = Math.Max(0m, marketAdjustedValue * (1m - rangeWidthPct));
        decimal highValue = Math.Max(lowValue, marketAdjustedValue * (1m + rangeWidthPct));

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "LowValue",
            amount: lowValue,
            factor: null,
            message: $"Low value calculated at {lowValue:C}.",
            evidenceRef: null));

        trace.Add(CreateTraceLine(
            order++,
            type: "Info",
            key: "HighValue",
            amount: highValue,
            factor: null,
            message: $"High value calculated at {highValue:C}.",
            evidenceRef: null));

        return new ValuationResult(
            Status: ValuationStatus.Success,
            FinalValue: decimal.Round(marketAdjustedValue, 0, MidpointRounding.AwayFromZero),
            LowValue: decimal.Round(lowValue, 0, MidpointRounding.AwayFromZero),
            HighValue: decimal.Round(highValue, 0, MidpointRounding.AwayFromZero),
            Confidence0To1: decimal.Round(confidence, 4, MidpointRounding.AwayFromZero),
            EngineVersion: input.EngineVersion,
            RubricVersion: input.RubricVersion,
            RulesetVersion: input.RulesetVersion,
            Trace: trace);
    }

    /// <summary>
    /// Creates a standardized trace line.
    /// </summary>
    private static ValuationTraceLine CreateTraceLine(
        int sortOrder,
        string type,
        string key,
        decimal? amount,
        decimal? factor,
        string message,
        string? evidenceRef)
    {
        return new ValuationTraceLine(
            SortOrder: sortOrder,
            Type: type,
            Key: key,
            Amount: amount,
            Factor: factor,
            Message: message,
            EvidenceRef: evidenceRef);
    }

    /// <summary>
    /// Creates a standard insufficient-data result.
    /// </summary>
    private static ValuationResult CreateInsufficientDataResult(
        ValuationInput input,
        IReadOnlyList<ValuationTraceLine> trace)
    {
        return new ValuationResult(
            Status: ValuationStatus.InsufficientData,
            FinalValue: 0m,
            LowValue: 0m,
            HighValue: 0m,
            Confidence0To1: 0m,
            EngineVersion: input.EngineVersion,
            RubricVersion: input.RubricVersion,
            RulesetVersion: input.RulesetVersion,
            Trace: trace);
    }

    /// <summary>
    /// Clamps a decimal value to the supplied minimum and maximum.
    /// </summary>
    private static decimal Clamp(decimal value, decimal min, decimal max)
        => Math.Min(max, Math.Max(min, value));
}