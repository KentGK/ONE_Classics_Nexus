namespace ONE_Classics_Nexus.Application.Valuation.Models;

public sealed record RankedComparable(
    ComparableInput Comparable,
    decimal SimilarityScore,
    decimal AdjustedPrice,
    IReadOnlyList<string> Reasons
);