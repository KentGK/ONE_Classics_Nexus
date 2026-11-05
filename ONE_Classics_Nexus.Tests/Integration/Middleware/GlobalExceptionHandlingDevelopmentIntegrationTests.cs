using FluentAssertions;
using Microsoft.AspNetCore.Hosting; // Add this using directive
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using ONE_Classics_Nexus.Tests.Integration.Infrastructure;
using ONE_Classics_Nexus.Tests.Integration.Vehicles;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ONE_Classics_Nexus.Tests.Integration.Middleware;

public sealed class GlobalExceptionHandlingDevelopmentIntegrationTests
{
    private readonly TestDatabaseHelper _db;

    public GlobalExceptionHandlingDevelopmentIntegrationTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Test.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection for integration tests.");

        _db = new TestDatabaseHelper(connectionString);
    }

    [Fact(DisplayName = "MW-D-001 Development mode includes full exception detail for Conflict")]
    public async Task MW_D_001_DevelopmentMode_ConflictReturnsFullDetail()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Test.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection for integration tests.");

        var vehicleTypeId = await _db.GetAnyVehicleTypeIdAsync();
        vehicleTypeId.Should().NotBeNull();

        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((ctx, cb) =>
            {
                cb.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = connectionString
                });
            });
        });

        var client = factory.CreateClient();

        var request = VehicleTestData.CreateValidRequest(vehicleTypeId!.Value);

        try
        {
            var created = await client.PostAsJsonAsync("/api/vehicles", request);
            created.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

            var duplicate = await client.PostAsJsonAsync("/api/vehicles", request);
            duplicate.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var body = await duplicate.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            root.TryGetProperty("traceId", out var traceEl).Should().BeTrue();
            traceEl.GetString().Should().NotBeNullOrWhiteSpace();

            root.TryGetProperty("correlationId", out var corrEl).Should().BeTrue();
            corrEl.GetString().Should().NotBeNullOrWhiteSpace();

            root.TryGetProperty("detail", out var detailEl).Should().BeTrue();
            var detail = detailEl.GetString() ?? string.Empty;
            detail.Should().Contain("ConflictException");
            detail.Should().Contain(request.Vin);
            detail.Should().Contain(" at ");
        }
        finally
        {
            if (!string.IsNullOrEmpty(request.Vin))
            {
                await _db.DeleteVehicleByVinAsync(request.Vin);
            }
        }
    }
}