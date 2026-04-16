using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Domain.Valuation.Engines;

public sealed class DefaultMarketEngine : IMarketEngine
{
    public decimal ApplyMarketLayer(decimal baseValue, ValuationInput input)
    {
        return baseValue;
    }
}