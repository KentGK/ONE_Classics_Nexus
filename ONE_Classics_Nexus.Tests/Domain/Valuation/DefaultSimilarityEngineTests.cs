using ONE_Classics_Nexus.Domain.Valuation.Engines;
using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Tests.Domain.Valuation;

public sealed class DefaultSimilarityEngineTests
{
    [Fact]
    public void CalculateSimilarity0To1_Returns_High_Score_For_Close_Match()
    {
        var sut = new DefaultSimilarityEngine();

        var subject = new SubjectVehicle(Guid.NewGuid(), 1970, "Chevrolet", "K10", null, "Pickup", "6.0 LS", "4L80E", "4x4", "K10", 12000m, null);
        var comp = new ComparableSale(Guid.NewGuid(), 1970, "Chevrolet", "K10", null, "Pickup", "6.0 LS", "4L80E", "4x4", "K10", 60000m, DateTime.UtcNow, "BAT", null, 11000, null, null);

        decimal result = sut.CalculateSimilarity0To1(subject, comp);

        Assert.True(result >= 0.90m);
    }
}