using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ONE_Classics_Nexus.Tests.Integration.Infrastructure;
using Xunit;

namespace ONE_Classics_Nexus.Tests.Integration.Vehicles;

public sealed class VehicleCrudIntegrationTests : IClassFixture<NexusApiFactory>
{
    private readonly HttpClient _client;
    private readonly TestDatabaseHelper _db;

    public VehicleCrudIntegrationTests(NexusApiFactory factory)
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

    [Fact(DisplayName = "VC-C-001 Create vehicle with valid payload returns success and persists row")]
    public async Task VC_C_001_CreateVehicle_WithValidPayload_ShouldPersistAndReturnVehicle()
    {
        var request = VehicleTestData.CreateValidRequest();

        try
        {
            var created = await CreateVehicleAsync(request);

            created.VehicleId.Should().NotBeEmpty();
            created.Vin.Should().Be(request.Vin);
            created.Year.Should().Be(request.Year);
            created.Make.Should().Be(request.Make);
            created.Model.Should().Be(request.Model);
            created.Trim.Should().Be(request.Trim);
            created.BodyStyle.Should().Be(request.BodyStyle);
            created.Engine.Should().Be(request.Engine);
            created.Transmission.Should().Be(request.Transmission);
            created.Drivetrain.Should().Be(request.Drivetrain);
            created.Mileage.Should().Be(request.Mileage);

            var dbRow = await _db.GetVehicleRowAsync(created.VehicleId);

            dbRow.Should().NotBeNull("the API returned a vehicle ID, so the row should exist in the test database");

            dbRow!.Vin.Should().Be(request.Vin);
            dbRow.Year.Should().Be(request.Year);
            dbRow.Make.Should().Be(request.Make);
            dbRow.Model.Should().Be(request.Model);
            dbRow.Trim.Should().Be(request.Trim);
            dbRow.Engine.Should().Be(request.Engine);
            dbRow.Transmission.Should().Be(request.Transmission);
            dbRow.Drivetrain.Should().Be(request.Drivetrain);
            dbRow.Mileage.Should().Be(request.Mileage);
        }
        finally
        {
            await _db.DeleteVehicleByVinAsync(request.Vin);
        }
    }

    [Fact(DisplayName = "VC-C-002 Create vehicle missing VIN returns validation error")]
    public async Task VC_C_002_CreateVehicle_MissingVin_ShouldReturnBadRequest()
    {
        var request = VehicleTestData.CreateValidRequest();
        request.Vin = string.Empty;

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "VC-C-003 Create vehicle duplicate VIN is rejected")]
    public async Task VC_C_003_CreateVehicle_DuplicateVin_ShouldBeRejected()
    {
        var request = VehicleTestData.CreateValidRequest();

        try
        {
            var first = await _client.PostAsJsonAsync("/api/vehicles", request);
            first.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

            var second = await _client.PostAsJsonAsync("/api/vehicles", request);

            second.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);

            var count = await _db.CountByVinAsync(request.Vin);
            count.Should().Be(1);
        }
        finally
        {
            await _db.DeleteVehicleByVinAsync(request.Vin);
        }
    }

    [Fact(DisplayName = "VC-C-005 Create vehicle with negative mileage returns validation error")]
    public async Task VC_C_005_CreateVehicle_NegativeMileage_ShouldReturnBadRequest()
    {
        var request = VehicleTestData.CreateValidRequest();
        request.Mileage = -1;

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(request.Vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-R-001 Get existing vehicle by id returns exact data")]
    public async Task VC_R_001_GetVehicle_ById_ShouldReturnExistingVehicle()
    {
        var request = VehicleTestData.CreateValidRequest();
        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(request);
            vehicleId = created.VehicleId;

            var vehicle = await GetVehicleAsync(vehicleId);

            vehicle.VehicleId.Should().Be(vehicleId);
            vehicle.Vin.Should().Be(request.Vin);
            vehicle.Year.Should().Be(request.Year);
            vehicle.Make.Should().Be(request.Make);
            vehicle.Model.Should().Be(request.Model);
            vehicle.Trim.Should().Be(request.Trim);
            vehicle.BodyStyle.Should().Be(request.BodyStyle);
            vehicle.Engine.Should().Be(request.Engine);
            vehicle.Transmission.Should().Be(request.Transmission);
            vehicle.Drivetrain.Should().Be(request.Drivetrain);
            vehicle.Mileage.Should().Be(request.Mileage);
        }
        finally
        {
            await CleanupVehicleAsync(vehicleId, request.Vin);
        }
    }

    [Fact(DisplayName = "VC-R-002 Get non-existent vehicle returns 404")]
    public async Task VC_R_002_GetVehicle_NonExistent_ShouldReturnNotFound()
    {
        var randomId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/vehicles/{randomId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "VC-R-003 Get invalid id format returns 404")]
    public async Task VC_R_003_GetVehicle_InvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/vehicles/not-a-guid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "VC-U-001 Update existing vehicle with valid payload persists changes")]
    public async Task VC_U_001_UpdateVehicle_WithValidPayload_ShouldPersistChanges()
    {
        var createRequest = VehicleTestData.CreateValidRequest();
        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(createRequest);
            vehicleId = created.VehicleId;

            var updateRequest = VehicleTestData.CreateValidUpdate(createRequest.Vin);

            var updateResponse = await _client.PutAsJsonAsync($"/api/vehicles/{vehicleId}", updateRequest);
            updateResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

            var updated = await GetVehicleAsync(vehicleId);

            updated.VehicleId.Should().Be(vehicleId);
            updated.Vin.Should().Be(updateRequest.Vin);
            updated.Year.Should().Be(updateRequest.Year);
            updated.Make.Should().Be(updateRequest.Make);
            updated.Model.Should().Be(updateRequest.Model);
            updated.Trim.Should().Be(updateRequest.Trim);
            updated.BodyStyle.Should().Be(updateRequest.BodyStyle);
            updated.Engine.Should().Be(updateRequest.Engine);
            updated.Transmission.Should().Be(updateRequest.Transmission);
            updated.Drivetrain.Should().Be(updateRequest.Drivetrain);
            updated.Mileage.Should().Be(updateRequest.Mileage);

            var dbRow = await _db.GetVehicleRowAsync(vehicleId);

            dbRow.Should().NotBeNull("the updated vehicle row should exist in the test database");

            dbRow!.Vin.Should().Be(updateRequest.Vin);
            dbRow.Year.Should().Be(updateRequest.Year);
            dbRow.Make.Should().Be(updateRequest.Make);
            dbRow.Model.Should().Be(updateRequest.Model);
            dbRow.Trim.Should().Be(updateRequest.Trim);
            dbRow.Engine.Should().Be(updateRequest.Engine);
            dbRow.Transmission.Should().Be(updateRequest.Transmission);
            dbRow.Drivetrain.Should().Be(updateRequest.Drivetrain);
            dbRow.Mileage.Should().Be(updateRequest.Mileage);
        }
        finally
        {
            await CleanupVehicleAsync(vehicleId, createRequest.Vin);
        }
    }

    [Fact(DisplayName = "VC-U-003 Update non-existent vehicle returns 404")]
    public async Task VC_U_003_UpdateVehicle_NonExistent_ShouldReturnNotFound()
    {
        var request = VehicleTestData.CreateValidUpdate(VehicleTestData.GenerateVin());
        var randomId = Guid.NewGuid();

        var response = await _client.PutAsJsonAsync($"/api/vehicles/{randomId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "VC-U-004 Update VIN to duplicate existing VIN is rejected")]
    public async Task VC_U_004_UpdateVehicle_ToDuplicateVin_ShouldBeRejected()
    {
        var firstRequest = VehicleTestData.CreateValidRequest();
        var secondRequest = VehicleTestData.CreateValidRequest();

        Guid firstId = Guid.Empty;
        Guid secondId = Guid.Empty;

        try
        {
            var first = await CreateVehicleAsync(firstRequest);
            firstId = first.VehicleId;

            var second = await CreateVehicleAsync(secondRequest);
            secondId = second.VehicleId;

            var duplicateUpdate = VehicleTestData.CreateValidUpdate(firstRequest.Vin);

            var response = await _client.PutAsJsonAsync($"/api/vehicles/{secondId}", duplicateUpdate);

            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);

            var verified = await GetVehicleAsync(secondId);
            verified.Vin.Should().Be(secondRequest.Vin);
        }
        finally
        {
            await CleanupVehicleAsync(firstId, firstRequest.Vin);
            await CleanupVehicleAsync(secondId, secondRequest.Vin);
        }
    }

    [Fact(DisplayName = "VC-I-001 POST then GET returns same persisted vehicle")]
    public async Task VC_I_001_PostThenGet_ShouldRoundTripSuccessfully()
    {
        var request = VehicleTestData.CreateValidRequest();
        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(request);
            vehicleId = created.VehicleId;

            var fetched = await GetVehicleAsync(vehicleId);

            fetched.VehicleId.Should().Be(created.VehicleId);
            fetched.Vin.Should().Be(created.Vin);
            fetched.Year.Should().Be(created.Year);
            fetched.Make.Should().Be(created.Make);
            fetched.Model.Should().Be(created.Model);
            fetched.Trim.Should().Be(created.Trim);
            fetched.BodyStyle.Should().Be(created.BodyStyle);
            fetched.Engine.Should().Be(created.Engine);
            fetched.Transmission.Should().Be(created.Transmission);
            fetched.Drivetrain.Should().Be(created.Drivetrain);
            fetched.Mileage.Should().Be(created.Mileage);
        }
        finally
        {
            await CleanupVehicleAsync(vehicleId, request.Vin);
        }
    }

    [Fact(DisplayName = "VC-I-002 PUT then GET returns updated vehicle")]
    public async Task VC_I_002_PutThenGet_ShouldReturnUpdatedState()
    {
        var createRequest = VehicleTestData.CreateValidRequest();
        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(createRequest);
            vehicleId = created.VehicleId;

            var updateRequest = VehicleTestData.CreateValidUpdate(createRequest.Vin);

            var updateResponse = await _client.PutAsJsonAsync($"/api/vehicles/{vehicleId}", updateRequest);
            updateResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

            var updated = await GetVehicleAsync(vehicleId);

            updated.VehicleId.Should().Be(vehicleId);
            updated.Vin.Should().Be(updateRequest.Vin);
            updated.Year.Should().Be(updateRequest.Year);
            updated.Make.Should().Be(updateRequest.Make);
            updated.Model.Should().Be(updateRequest.Model);
            updated.Trim.Should().Be(updateRequest.Trim);
            updated.BodyStyle.Should().Be(updateRequest.BodyStyle);
            updated.Engine.Should().Be(updateRequest.Engine);
            updated.Transmission.Should().Be(updateRequest.Transmission);
            updated.Drivetrain.Should().Be(updateRequest.Drivetrain);
            updated.Mileage.Should().Be(updateRequest.Mileage);
        }
        finally
        {
            await CleanupVehicleAsync(vehicleId, createRequest.Vin);
        }
    }

    [Fact(DisplayName = "VC-ERR-001 Validation errors return ProblemDetails")]
    public async Task VC_ERR_001_ValidationError_ShouldReturnProblemDetails()
    {
        var request = new CreateVehicleRequest
        {
            Vin = "",
            Year = 0,
            Make = "",
            Model = "",
            Mileage = -1
        };

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Title.Should().NotBeNullOrWhiteSpace();
        problem.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    private async Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request)
    {
        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

        var created = await response.Content.ReadAsAsync<VehicleResponse>();
        created.Should().NotBeNull();
        created!.VehicleId.Should().NotBeEmpty();

        return created;
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

    private async Task CleanupVehicleAsync(Guid vehicleId, string vin)
    {
        if (vehicleId != Guid.Empty)
            await _db.DeleteVehicleAsync(vehicleId);
        else
            await _db.DeleteVehicleByVinAsync(vin);
    }
}