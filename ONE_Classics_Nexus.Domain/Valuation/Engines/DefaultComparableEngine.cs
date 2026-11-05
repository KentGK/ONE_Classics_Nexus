using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Domain.Valuation.Rules;

namespace ONE_Classics_Nexus.Domain.Valuation.Engines;

/// <summary>
/// Default deterministic comparable adjustment engine.
/// 
/// Responsibilities:
/// - Apply similarity scoring
/// - Normalize comparable prices (time, mileage)
/// - Prepare adjusted comparable set for valuation
/// 
/// Phase 2 Scope:
/// - Time adjustment
/// - Mileage adjustment
/// 
/// Phase 3 Extension Points:
/// - Condition adjustment
/// - Originality adjustment
/// - Equipment adjustments
/// - Venue/geography adjustments
/// </summary>
public sealed class DefaultComparableEngine : IComparableEngine
{
    private readonly ISimilarityEngine _similarityEngine;

    /// <summary>
    /// Constructor
    /// </summary>
    public DefaultComparableEngine(ISimilarityEngine similarityEngine)
    {
        _similarityEngine = similarityEngine;
    }

    /// <inheritdoc />
    public IReadOnlyList<AdjustedComparable> AdjustComparables(
        SubjectVehicle subject,
        IReadOnlyList<ComparableSale> comparables,
        decimal subjectConditionIndex0To100)
    {
        if (comparables is null || comparables.Count == 0)
        {
            return Array.Empty<AdjustedComparable>();
        }

        var results = new List<AdjustedComparable>();

        foreach (var comp in comparables)
        {
            // 1. Calculate similarity score
            decimal similarity = _similarityEngine.CalculateSimilarity0To1(subject, comp);

            // 2. Initialize adjustment tracking
            var adjustments = new List<ComparableAdjustment>();
            decimal adjustedPrice = comp.SalePrice;

            // 3. Apply Time Adjustment
            decimal timeAdjustment = CalculateTimeAdjustment(comp);
            if (timeAdjustment != 0m)
            {
                adjustments.Add(new ComparableAdjustment(
                    CategoryKey: "time",
                    Amount: timeAdjustment,
                    Reason: "Market time normalization",
                    EvidenceRef: comp.Url));

                adjustedPrice += timeAdjustment;
            }

            // 4. Apply Mileage Adjustment
            decimal mileageAdjustment = CalculateMileageAdjustment(subject, comp);
            if (mileageAdjustment != 0m)
            {
                adjustments.Add(new ComparableAdjustment(
                    CategoryKey: "mileage",
                    Amount: mileageAdjustment,
                    Reason: "Mileage normalization between subject and comparable",
                    EvidenceRef: comp.Url));

                adjustedPrice += mileageAdjustment;
            }

            // 🔥 Phase 3 Extensions (DO NOT REMOVE — intentional placeholders)
            // --------------------------------------------------------------
            // decimal conditionAdjustment = CalculateConditionAdjustment(...);
            // decimal originalityAdjustment = CalculateOriginalityAdjustment(...);
            // decimal equipmentAdjustment = CalculateEquipmentAdjustment(...);
            // decimal venueAdjustment = CalculateVenueAdjustment(...);
            // decimal geographyAdjustment = CalculateGeographyAdjustment(...);

            // 5. Clamp adjusted price (safety)
            adjustedPrice = Math.Max(0m, adjustedPrice);

            // 6. Build adjusted comparable
            results.Add(new AdjustedComparable(
                Comparable: comp,
                SimilarityScore0To1: similarity,
                Adjustments: adjustments,
                AdjustedPrice: adjustedPrice));
        }

        return results;
    }

    /// <summary>
    /// Applies time normalization based on sale date.
    /// </summary>
    private static decimal CalculateTimeAdjustment(ComparableSale comp)
    {
        DateTime asOf = DateTime.UtcNow;

        int monthsDiff =
            ((asOf.Year - comp.SaleDateUtc.Year) * 12) +
            (asOf.Month - comp.SaleDateUtc.Month);

        monthsDiff = Math.Max(0, monthsDiff);

        decimal adjustment =
            comp.SalePrice *
            ValuationRuleConstants.DefaultMonthlyTimeAdjustmentRate *
            monthsDiff;

        return adjustment;
    }

    /// <summary>
    /// Applies mileage normalization between subject and comparable.
    /// </summary>
    private static decimal CalculateMileageAdjustment(
        SubjectVehicle subject,
        ComparableSale comp)
    {
        if (!subject.Mileage.HasValue || !comp.Mileage.HasValue)
        {
            return 0m;
        }

        decimal mileageDelta = subject.Mileage.Value - comp.Mileage.Value;

        decimal adjustment =
            mileageDelta *
            ValuationRuleConstants.DefaultMileageAdjustmentRatePerMile *
            -1m;

        return adjustment;
    }
}