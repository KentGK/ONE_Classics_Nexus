using ONE_Classics_Nexus.Domain.Valuation.Engines;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using Xunit;

namespace ONE_Classics_Nexus.Tests.Domain.Valuation;

public sealed class DefaultComparableEngineTests
{
    [Fact]
    public void AdjustComparables_Returns_Adjusted_Comparables()
    {
        // Arrange
        var similarityEngine = new DefaultSimilarityEngine();
        var engine = new DefaultComparableEngine(similarityEngine);

        var subject = new SubjectVehicle(
            VehicleId: Guid.NewGuid(),
            Year: 1970,
            Make: "Chevrolet",
            Model: "K10",
            Trim: "Custom",
            BodyStyle: "Pickup",
            Engine: "350 V8",
            Transmission: "Automatic",
            Drivetrain: "4x4",
            Series: "K10",
            Mileage: 55000,
            Vin: "TESTVIN123"
        );

        var comps = new List<ComparableSale>
        {
            new ComparableSale(
                comparableId: Guid.NewGuid(),
                year: 1970,
                make: "Chevrolet",
                model: "K10",
                trim: "Custom",
                bodyStyle: "Pickup",
                engine: "350 V8",
                transmission: "Automatic",
                drivetrain: "4x4",
                series: "K10",
                salePrice: 45000m,
                saleDateUtc: DateTime.UtcNow.AddDays(-30),
                source: "Mecum",
                url: "https://example.com/1",
                mileage: 50000,
                venue: "Auction",
                notes: "Clean example"
            )
        };

        // Act
        var result = engine.AdjustComparables(
            subject,
            comps,
            subjectConditionIndex0To100: 85m);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var first = result.First();

        Assert.NotNull(first.Comparable);
        Assert.True(first.AdjustedPrice >= 0);
        Assert.InRange(first.SimilarityScore0To1, 0m, 1m);
    }
}