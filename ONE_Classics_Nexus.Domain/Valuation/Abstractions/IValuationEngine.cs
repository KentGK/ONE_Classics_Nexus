using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface IValuationEngine
{
    ValuationResult Run(ValuationInput input);
}