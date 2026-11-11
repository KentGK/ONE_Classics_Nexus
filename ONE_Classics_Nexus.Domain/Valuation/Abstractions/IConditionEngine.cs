using ONE_Classics_Nexus.Domain.Valuation.Models;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface IConditionEngine
{
    decimal CalculateConditionIndex0To100(IReadOnlyList<ConditionScore> scores);
}