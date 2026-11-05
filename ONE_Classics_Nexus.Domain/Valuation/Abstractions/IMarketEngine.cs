using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface IMarketEngine
{
    decimal ApplyMarketLayer(decimal baseValue, ValuationInput input);
}