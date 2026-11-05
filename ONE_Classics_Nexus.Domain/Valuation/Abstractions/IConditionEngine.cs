using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface IConditionEngine
{
    decimal CalculateConditionIndex0To100(IReadOnlyList<ConditionScore> scores);
}