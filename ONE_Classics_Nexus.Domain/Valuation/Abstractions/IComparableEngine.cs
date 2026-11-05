using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface IComparableEngine
{
    IReadOnlyList<AdjustedComparable> AdjustComparables(
        SubjectVehicle subject,
        IReadOnlyList<ComparableSale> comparables,
        decimal subjectConditionIndex0To100);
}