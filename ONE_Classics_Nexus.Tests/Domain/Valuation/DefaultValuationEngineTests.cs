using ONE_Classics_Nexus.Domain.Valuation.Engines;
using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Tests.Domain.Valuation;

public sealed class DefaultValuationEngineTests
{
    [Fact]
    public void Run_Returns_InsufficientData_When_No_Comparables()
    {
        var sut = new DefaultValuationEngine(
            new DefaultConditionEngine(),
            new DefaultComparableEngine(new DefaultSimilarityEngine()),
            new DefaultMarketEngine(),
            new DefaultConfidenceEngine());

        var input = new ValuationInput(
            new SubjectVehicle(Guid.NewGuid(), 1970, "Chevrolet", "K10", null, null, null, null, null, null, null, null),
            Array.Empty<ConditionScore>(),
            Array.Empty<ComparableSale>(),
            "default",
            "1.0.0",
            "1.0.0",
            "1.0.0");

        ValuationResult result = sut.Run(input);

        Assert.Equal(ValuationStatus.InsufficientData, result.Status);
    }
}