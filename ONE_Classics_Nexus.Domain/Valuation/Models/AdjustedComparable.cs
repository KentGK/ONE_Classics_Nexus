using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Valuation.Models;

/// <summary>
/// Represents a comparable sale after similarity scoring and valuation adjustments
/// have been applied.
///
/// This is the normalized working object used by the valuation engine when
/// calculating weighted comparable indications.
/// </summary>
/// <param name="Comparable">
/// The original comparable sale record.
/// </param>
/// <param name="SimilarityScore0To1">
/// The similarity score between the subject vehicle and the comparable,
/// normalized to a 0.00 to 1.00 range.
/// </param>
/// <param name="Adjustments">
/// The ordered list of adjustments applied to the comparable.
/// </param>
/// <param name="AdjustedPrice">
/// The final adjusted price after all comparable adjustments are applied.
/// </param>
public sealed record AdjustedComparable(
    ComparableSale Comparable,
    decimal SimilarityScore0To1,
    IReadOnlyList<ComparableAdjustment> Adjustments,
    decimal AdjustedPrice
);