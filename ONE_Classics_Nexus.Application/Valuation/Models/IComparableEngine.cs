using ONE_Classics_Nexus.Application.Valuation.Models;

namespace ONE_Classics_Nexus.Application.Abstractions.Valuation;

public interface IComparableEngine
{
    IReadOnlyList<RankedComparable> Rank(
        SubjectVehicleProfile subject,
        IReadOnlyList<ComparableInput> comparables);
}