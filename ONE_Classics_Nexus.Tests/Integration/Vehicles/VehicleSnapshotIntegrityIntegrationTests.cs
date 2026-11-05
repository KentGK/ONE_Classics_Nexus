using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ONE_Classics_Nexus.API.Contracts.Appraisals;
using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.Tests.Integration.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace ONE_Classics_Nexus.Tests.Integration.Vehicles;

public sealed class VehicleSnapshotIntegrityIntegrationTests : IClassFixture<NexusApiFactory>
{
    private readonly HttpClient _client;
    private readonly TestDatabaseHelper _db;

    public VehicleSnapshotIntegrityIntegrationTests(NexusApiFactory factory)
    {
        _client = factory.CreateClient();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Test.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Missing ConnectionStrings:DefaultConnection for integration tests.");

        _db = new TestDatabaseHelper(connectionString);
    }

    [Fact(DisplayName = "VSI-001 Create appraisal from vehicle creates subject snapshot")]
    public async Task CreateAppraisal_FromVehicle_CreatesSubjectSnapshot()
    {
        var vehicleTypeId = Guid.NewGuid();
        var vehicleRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = VehicleTestData.GetRequiredVin(vehicleRequest);

        Guid vehicleId = Guid.Empty;
        Guid appraisalId = Guid.Empty;

        try
        {
            var vehicle = await CreateVehicleAsync(vehicleRequest);
            vehicleId = vehicle.VehicleId;

            var appraisal = await CreateAppraisalAsync(vehicleId);
            appraisalId = appraisal.AppraisalId;

            var snapshot = await _db.GetSnapshotByAppraisalIdAsync(appraisalId);

            snapshot.Should().NotBeNull();
            snapshot!.Vehicle_Id.Should().Be(vehicleId.ToString());
            snapshot.Appraisal_Id.Should().Be(appraisalId.ToString());
            snapshot.Vin.Should().Be(vin);
            snapshot.Snapshot_Status.Should().Be("Draft");
        }
        finally
        {
            if (appraisalId != Guid.Empty)
            {
                await _db.DeleteInspectionsByAppraisalAsync(appraisalId);
                await _db.DeleteAppraisalAsync(appraisalId);
            }

            await CleanupVehicleAsync(vehicleId, vin);
        }
    }

    [Fact(DisplayName = "VSI-002 Vehicle update after snapshot approval does not change snapshot")]
    public async Task VehicleUpdate_AfterSnapshotApproval_DoesNotChangeSnapshot()
    {
        var vehicleTypeId = Guid.NewGuid();
        var vehicleRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = VehicleTestData.GetRequiredVin(vehicleRequest);

        Guid vehicleId = Guid.Empty;
        Guid appraisalId = Guid.Empty;

        try
        {
            var vehicle = await CreateVehicleAsync(vehicleRequest);
            vehicleId = vehicle.VehicleId;

            var appraisal = await CreateAppraisalAsync(vehicleId);
            appraisalId = appraisal.AppraisalId;

            var originalSnapshot = await _db.GetSnapshotByAppraisalIdAsync(appraisalId);
            originalSnapshot.Should().NotBeNull();

            await _db.SetSnapshotStatusAsync(appraisalId, "Approved");

            var current = await GetVehicleAsync(vehicleId);
            var updateRequest = BuildUpdateRequest(current, make: "UpdatedMasterMake");

            var updateResponse = await _client.PutAsJsonAsync($"/api/vehicles/{vehicleId}", updateRequest);
            updateResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

            var snapshotAfterVehicleUpdate = await _db.GetSnapshotByAppraisalIdAsync(appraisalId);
            snapshotAfterVehicleUpdate.Should().NotBeNull();
            snapshotAfterVehicleUpdate!.Make.Should().Be(originalSnapshot!.Make);
            snapshotAfterVehicleUpdate.Vin.Should().Be(originalSnapshot.Vin);
            snapshotAfterVehicleUpdate.Snapshot_Status.Should().Be("Approved");
        }
        finally
        {
            if (appraisalId != Guid.Empty)
            {
                await _db.DeleteInspectionsByAppraisalAsync(appraisalId);
                await _db.DeleteAppraisalAsync(appraisalId);
            }

            await CleanupVehicleAsync(vehicleId, vin);
        }
    }

    [Fact(DisplayName = "VSI-003 Snapshot refresh for draft appraisal requires explicit action")]
    public async Task SnapshotRefresh_DraftAppraisal_RequiresExplicitAction()
    {
        var vehicleTypeId = Guid.NewGuid();
        var vehicleRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = VehicleTestData.GetRequiredVin(vehicleRequest);

        Guid vehicleId = Guid.Empty;
        Guid appraisalId = Guid.Empty;

        try
        {
            var vehicle = await CreateVehicleAsync(vehicleRequest);
            vehicleId = vehicle.VehicleId;

            var appraisal = await CreateAppraisalAsync(vehicleId);
            appraisalId = appraisal.AppraisalId;

            var snapshotBeforeUpdate = await _db.GetSnapshotByAppraisalIdAsync(appraisalId);
            snapshotBeforeUpdate.Should().NotBeNull();

            var current = await GetVehicleAsync(vehicleId);
            var updateRequest = BuildUpdateRequest(current, make: "DraftMasterUpdate");

            var updateResponse = await _client.PutAsJsonAsync($"/api/vehicles/{vehicleId}", updateRequest);
            updateResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

            var snapshotAfterUpdate = await _db.GetSnapshotByAppraisalIdAsync(appraisalId);
            snapshotAfterUpdate.Should().NotBeNull();
            snapshotAfterUpdate!.Make.Should().Be(snapshotBeforeUpdate!.Make);
            snapshotAfterUpdate.Snapshot_Status.Should().Be("Draft");
        }
        finally
        {
            if (appraisalId != Guid.Empty)
            {
                await _db.DeleteInspectionsByAppraisalAsync(appraisalId);
                await _db.DeleteAppraisalAsync(appraisalId);
            }

            await CleanupVehicleAsync(vehicleId, vin);
        }
    }

    private static UpdateVehicleRequest BuildUpdateRequest(VehicleResponse vehicle, string make)
    {
        return new UpdateVehicleRequest
        {
            RowVersion = vehicle.RowVersion,
            VehicleTypeId = vehicle.VehicleTypeId,
            Vin = vehicle.Vin,
            Year = vehicle.Year,
            Make = make,
            Model = vehicle.Model,
            Trim = vehicle.Trim,
            SeriesName = vehicle.SeriesName,
            BodyStyle = vehicle.BodyStyle,
            Drivetrain = vehicle.Drivetrain,
            Engine = vehicle.Engine,
            Transmission = vehicle.Transmission,
            Mileage = vehicle.Mileage,
            OdometerUnit = vehicle.OdometerUnit,
            ExteriorColor = vehicle.ExteriorColor,
            InteriorColor = vehicle.InteriorColor,
            TitleStatus = vehicle.TitleStatus,
            AuthenticityProfile = vehicle.AuthenticityProfile,
            RestorationLevel = vehicle.RestorationLevel,
            DocumentationLevel = vehicle.DocumentationLevel,
            OverallConditionScore = vehicle.OverallConditionScore,
            HasRedFlags = vehicle.HasRedFlags,
            Notes = vehicle.Notes,
            OwnershipNotes = vehicle.OwnershipNotes
        };
    }

    private async Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request)
    {
        if (request.VehicleTypeId == Guid.Empty)
        {
            var vehicleTypeId = await _db.GetAnyVehicleTypeIdAsync();
            if (vehicleTypeId.HasValue)
            {
                request.VehicleTypeId = vehicleTypeId.Value;
            }
        }

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Match(
            status => status == HttpStatusCode.Created || status == HttpStatusCode.OK,
            $"POST /api/vehicles should succeed, but returned {(int)response.StatusCode}: {body}");

        var created = await response.Content.ReadAsAsync<VehicleResponse>();
        created.Should().NotBeNull();

        return created!;
    }

    private async Task<VehicleResponse> GetVehicleAsync(Guid vehicleId)
    {
        var response = await _client.GetAsync($"/api/vehicles/{vehicleId}");
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(
            HttpStatusCode.OK,
            $"GET /api/vehicles/{vehicleId} should succeed, but returned {(int)response.StatusCode}: {body}");

        var vehicle = await response.Content.ReadAsAsync<VehicleResponse>();
        vehicle.Should().NotBeNull();

        return vehicle!;
    }

    private async Task<AppraisalResponse> CreateAppraisalAsync(Guid vehicleId)
    {
        var request = new CreateAppraisalRequest(
            vehicleId,
            $"APR-{Guid.NewGuid():N}"[..12],
            null,
            null,
            null,
            null,
            "Standard",
            "Draft",
            null,
            null,
            null,
            null,
            null);

        var response = await _client.PostAsJsonAsync("/api/appraisals", request);
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(
            HttpStatusCode.Created,
            $"POST /api/appraisals should succeed, but returned {(int)response.StatusCode}: {body}");

        var appraisal = await response.Content.ReadAsAsync<AppraisalResponse>();
        appraisal.Should().NotBeNull();

        return appraisal!;
    }

    private async Task CleanupVehicleAsync(Guid vehicleId, string vin)
    {
        if (vehicleId != Guid.Empty)
        {
            await _db.DeleteInspectionsByVehicleAsync(vehicleId);
            await _db.DeleteAppraisalsByVehicleAsync(vehicleId);
            await _db.DeleteVehicleAsync(vehicleId);
            return;
        }

        await _db.DeleteVehicleByVinAsync(vin);
    }
}
