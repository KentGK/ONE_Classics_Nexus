using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ONE_Classics_Nexus.API.Contracts.Appraisals;
using ONE_Classics_Nexus.API.Contracts.Inspections;
using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Tests.Integration.Infrastructure;
using ONE_Classics_Nexus.Tests.Integration.Vehicles;
using System.Net;
using System.Net.Http.Json;


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
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);

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
            if (!string.IsNullOrEmpty(request.Vin))
            {
                await _db.DeleteVehicleByVinAsync(request.Vin);
            }
        }
    }

    [Fact(DisplayName = "VC-C-002 Create vehicle missing VIN returns validation error")]
    public async Task VC_C_002_CreateVehicle_MissingVin_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
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
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");

        try
        {
            var first = await _client.PostAsJsonAsync("/api/vehicles", request);
            first.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

            var second = await _client.PostAsJsonAsync("/api/vehicles", request);

            second.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);

            var count = await _db.CountByVinAsync(vin);
            count.Should().Be(1);
        }
        finally
        {
            await _db.DeleteVehicleByVinAsync(vin);
        }
    }

    [Fact(DisplayName = "VC-C-004 Create duplicate VIN returns 409 Conflict")]
    public async Task VC_C_004_CreateVehicle_DuplicateVin_ShouldReturnConflict()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException(
                "The generated vehicle test request must contain a VIN.");

        try
        {
            var first = await _client.PostAsJsonAsync("/api/vehicles", request);
            first.StatusCode.Should().BeOneOf(
                HttpStatusCode.Created,
                HttpStatusCode.OK);

            var second = await _client.PostAsJsonAsync("/api/vehicles", request);

            second.StatusCode.Should().Be(
                HttpStatusCode.Conflict,
                "duplicate VIN should be rejected with 409 Conflict");
        }
        finally
        {
            await _db.DeleteVehicleByVinAsync(vin);
        }
    }

    [Fact(DisplayName = "VC-C-005 Create vehicle with negative mileage returns validation error")]
    public async Task VC_C_005_CreateVehicle_NegativeMileage_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.Mileage = -1;

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-011 Create vehicle with unsupported documentation level returns validation error")]
    public async Task VC_C_011_CreateVehicle_UnsupportedDocumentationLevel_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.DocumentationLevel = "Complete";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-012 Create vehicle with unsupported authenticity profile returns validation error")]
    public async Task VC_C_012_CreateVehicle_UnsupportedAuthenticityProfile_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.AuthenticityProfile = "Factory Certified";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-010 Create vehicle with unsupported restoration level returns validation error")]
    public async Task VC_C_010_CreateVehicle_UnsupportedRestorationLevel_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.RestorationLevel = "Partial";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-009 Create vehicle with unsupported originality profile returns validation error")]
    public async Task VC_C_009_CreateVehicle_UnsupportedOriginalityProfile_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.OriginalityProfile = "Survivor";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-008 Create vehicle with unsupported title status returns validation error")]
    public async Task VC_C_008_CreateVehicle_UnsupportedTitleStatus_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.TitleStatus = "Lien";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-007 Create vehicle with unsupported odometer status returns validation error")]
    public async Task VC_C_007_CreateVehicle_UnsupportedOdometerStatus_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.OdometerStatus = "Estimated";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-C-006 Create vehicle with unsupported odometer unit returns validation error")]
    public async Task VC_C_006_CreateVehicle_UnsupportedOdometerUnit_ShouldReturnBadRequest()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        request.OdometerUnit = "km";

        var response = await _client.PostAsJsonAsync("/api/vehicles", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var count = await _db.CountByVinAsync(vin);
        count.Should().Be(0);
    }

    [Fact(DisplayName = "VC-R-001 Get existing vehicle by id returns exact data")]
    public async Task VC_R_001_GetVehicle_ById_ShouldReturnExistingVehicle()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = request.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");

        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(request);
            vehicleId = created.VehicleId;

            var vehicle = await GetVehicleAsync(vehicleId);

            vehicle.VehicleId.Should().Be(vehicleId);
        }
        finally
        {
            if (!string.IsNullOrEmpty(request.Vin))
            {
                await _db.DeleteVehicleByVinAsync(request.Vin);
            }
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

    [Fact(DisplayName = "VC-VI-001 Get inspections by vehicle id returns only that vehicle inspections")]
    public async Task VC_VI_001_GetInspectionsByVehicleId_ShouldReturnAssociatedInspections()
    {
        var vehicleTypeId = Guid.NewGuid();
        var firstVehicleRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var secondVehicleRequest = VehicleTestData.CreateValidRequest(Guid.NewGuid());

        var firstVin = VehicleTestData.GetRequiredVin(firstVehicleRequest);
        var secondVin = VehicleTestData.GetRequiredVin(secondVehicleRequest);

        Guid firstVehicleId = Guid.Empty;
        Guid secondVehicleId = Guid.Empty;
        Guid firstAppraisalId = Guid.Empty;
        Guid secondAppraisalId = Guid.Empty;
        Guid firstInspectionId = Guid.Empty;
        Guid secondInspectionId = Guid.Empty;

        try
        {
            var firstVehicle = await CreateVehicleAsync(firstVehicleRequest);
            firstVehicleId = firstVehicle.VehicleId;

            var secondVehicle = await CreateVehicleAsync(secondVehicleRequest);
            secondVehicleId = secondVehicle.VehicleId;

            var firstAppraisal = await CreateAppraisalAsync(firstVehicleId);
            firstAppraisalId = firstAppraisal.AppraisalId;

            var secondAppraisal = await CreateAppraisalAsync(secondVehicleId);
            secondAppraisalId = secondAppraisal.AppraisalId;

            var firstInspection = await CreateInspectionAsync(firstAppraisalId);
            firstInspectionId = firstInspection.InspectionId;

            var secondInspection = await CreateInspectionAsync(secondAppraisalId);
            secondInspectionId = secondInspection.InspectionId;

            var response = await _client.GetAsync($"/api/vehicles/{firstVehicleId}/inspections");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadAsAsync<InspectionListResponse>();
            payload.Should().NotBeNull();
            payload!.Items.Should().Contain(item => item.InspectionId == firstInspectionId);
            payload.Items.Should().NotContain(item => item.InspectionId == secondInspectionId);
            payload.Items.Should().OnlyContain(item => item.AppraisalId == firstAppraisalId);
        }
        finally
        {
            if (firstInspectionId != Guid.Empty)
            {
                await _db.DeleteInspectionAsync(firstInspectionId);
            }

            if (secondInspectionId != Guid.Empty)
            {
                await _db.DeleteInspectionAsync(secondInspectionId);
            }

            if (firstAppraisalId != Guid.Empty)
            {
                await _db.DeleteAppraisalAsync(firstAppraisalId);
            }

            if (secondAppraisalId != Guid.Empty)
            {
                await _db.DeleteAppraisalAsync(secondAppraisalId);
            }

            await CleanupVehicleAsync(firstVehicleId, firstVin);
            await CleanupVehicleAsync(secondVehicleId, secondVin);
        }
    }

    [Fact(DisplayName = "VC-VI-002 Get inspections for non-existent vehicle returns 404")]
    public async Task VC_VI_002_GetInspectionsByVehicleId_NonExistentVehicle_ShouldReturnNotFound()
    {
        var vehicleId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/vehicles/{vehicleId}/inspections");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "VC-H-001 Get vehicle history returns timeline for the requested vehicle")]
    public async Task VC_H_001_GetVehicleHistory_ShouldReturnVehicleTimeline()
    {
        var vehicleTypeId = Guid.NewGuid();
        var firstVehicleRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var secondVehicleRequest = VehicleTestData.CreateValidRequest(Guid.NewGuid());

        var firstVin = VehicleTestData.GetRequiredVin(firstVehicleRequest);
        var secondVin = VehicleTestData.GetRequiredVin(secondVehicleRequest);

        Guid firstVehicleId = Guid.Empty;
        Guid secondVehicleId = Guid.Empty;
        Guid firstAppraisalId = Guid.Empty;
        Guid secondAppraisalId = Guid.Empty;
        Guid firstInspectionId = Guid.Empty;
        Guid secondInspectionId = Guid.Empty;

        try
        {
            var firstVehicle = await CreateVehicleAsync(firstVehicleRequest);
            firstVehicleId = firstVehicle.VehicleId;

            var secondVehicle = await CreateVehicleAsync(secondVehicleRequest);
            secondVehicleId = secondVehicle.VehicleId;

            var firstAppraisal = await CreateAppraisalAsync(firstVehicleId);
            firstAppraisalId = firstAppraisal.AppraisalId;

            var secondAppraisal = await CreateAppraisalAsync(secondVehicleId);
            secondAppraisalId = secondAppraisal.AppraisalId;

            var firstInspection = await CreateInspectionAsync(firstAppraisalId);
            firstInspectionId = firstInspection.InspectionId;

            var secondInspection = await CreateInspectionAsync(secondAppraisalId);
            secondInspectionId = secondInspection.InspectionId;

            var response = await _client.GetAsync($"/api/vehicles/{firstVehicleId}/history");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadAsAsync<VehicleHistoryResponse>();
            payload.Should().NotBeNull();
            payload!.VehicleId.Should().Be(firstVehicleId);

            payload.Events.Should().Contain(e => e.EventType == "VehicleCreated");
            payload.Events.Should().Contain(e => e.AppraisalId == firstAppraisalId);
            payload.Events.Should().Contain(e => e.InspectionId == firstInspectionId);
            payload.Events.Should().NotContain(e => e.AppraisalId == secondAppraisalId);
            payload.Events.Should().NotContain(e => e.InspectionId == secondInspectionId);
        }
        finally
        {
            if (firstInspectionId != Guid.Empty)
            {
                await _db.DeleteInspectionAsync(firstInspectionId);
            }

            if (secondInspectionId != Guid.Empty)
            {
                await _db.DeleteInspectionAsync(secondInspectionId);
            }

            if (firstAppraisalId != Guid.Empty)
            {
                await _db.DeleteAppraisalAsync(firstAppraisalId);
            }

            if (secondAppraisalId != Guid.Empty)
            {
                await _db.DeleteAppraisalAsync(secondAppraisalId);
            }

            await CleanupVehicleAsync(firstVehicleId, firstVin);
            await CleanupVehicleAsync(secondVehicleId, secondVin);
        }
    }

    [Fact(DisplayName = "VC-H-002 Get vehicle history for non-existent vehicle returns 404")]
    public async Task VC_H_002_GetVehicleHistory_NonExistentVehicle_ShouldReturnNotFound()
    {
        var vehicleId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/vehicles/{vehicleId}/history");

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
        var vehicleTypeId = Guid.NewGuid();
        var createRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = createRequest.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");
        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(createRequest);
            vehicleId = created.VehicleId;

            var current = await GetVehicleAsync(vehicleId);

            var updateRequest = VehicleTestData.CreateValidUpdate(vin, current.RowVersion);

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
            await CleanupVehicleAsync(vehicleId, vin);
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
        var vehicleTypeId = Guid.NewGuid();
        var firstRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var secondRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);

        var firstVin = firstRequest.Vin
            ?? throw new InvalidOperationException("The generated first vehicle request must contain a VIN.");

        var secondVin = secondRequest.Vin
            ?? throw new InvalidOperationException("The generated second vehicle request must contain a VIN.");

        Guid firstId = Guid.Empty;
        Guid secondId = Guid.Empty;

        try
        {
            var first = await CreateVehicleAsync(firstRequest);
            firstId = first.VehicleId;

            var second = await CreateVehicleAsync(secondRequest);
            secondId = second.VehicleId;

            var duplicateUpdate = VehicleTestData.CreateValidUpdate(firstVin);

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
            await CleanupVehicleAsync(firstId, firstVin);
            await CleanupVehicleAsync(secondId, secondVin);
        }
    }

    [Fact(DisplayName = "VC-AR-001 Archive existing vehicle updates status and returns 204")]
    public async Task VC_AR_001_ArchiveVehicle_Existing_ShouldReturnNoContentAndPersistArchivedStatus()
    {
        var vehicleTypeId = Guid.NewGuid();
        var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var vin = VehicleTestData.GetRequiredVin(request);
        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(request);
            vehicleId = created.VehicleId;

            var response = await _client.PostAsync($"/api/vehicles/{vehicleId}/archive", content: null);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var dbRow = await _db.GetVehicleRowAsync(vehicleId);
            dbRow.Should().NotBeNull();
            dbRow!.Vehicle_Status.Should().Be("Archived");
        }
        finally
        {
            await CleanupVehicleAsync(vehicleId, vin);
        }
    }

    [Fact(DisplayName = "VC-AR-002 Archive non-existent vehicle returns 404")]
    public async Task VC_AR_002_ArchiveVehicle_NonExistent_ShouldReturnNotFound()
    {
        var vehicleId = Guid.NewGuid();

        var response = await _client.PostAsync($"/api/vehicles/{vehicleId}/archive", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "VC-MG-001 Merge vehicle moves appraisals and marks source as merged")]
    public async Task VC_MG_001_MergeVehicle_WithValidTarget_ShouldReturnNoContentAndPersistMerge()
    {
        var vehicleTypeId = Guid.NewGuid();
        var sourceRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var targetRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);

        var sourceVin = VehicleTestData.GetRequiredVin(sourceRequest);
        var targetVin = VehicleTestData.GetRequiredVin(targetRequest);

        Guid sourceVehicleId = Guid.Empty;
        Guid targetVehicleId = Guid.Empty;
        Guid appraisalId = Guid.Empty;

        try
        {
            var sourceVehicle = await CreateVehicleAsync(sourceRequest);
            sourceVehicleId = sourceVehicle.VehicleId;

            var targetVehicle = await CreateVehicleAsync(targetRequest);
            targetVehicleId = targetVehicle.VehicleId;

            var appraisal = await CreateAppraisalAsync(sourceVehicleId);
            appraisalId = appraisal.AppraisalId;

            var mergeRequest = new MergeVehicleRequest
            {
                TargetVehicleId = targetVehicleId
            };

            var response = await _client.PostAsJsonAsync($"/api/vehicles/{sourceVehicleId}/merge", mergeRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var sourceDbRow = await _db.GetVehicleRowAsync(sourceVehicleId);
            sourceDbRow.Should().NotBeNull();
            sourceDbRow!.Vehicle_Status.Should().Be("Merged");

            var sourceAppraisalCount = await _db.CountAppraisalsByVehicleAsync(sourceVehicleId);
            sourceAppraisalCount.Should().Be(0);

            var targetAppraisalCount = await _db.CountAppraisalsByVehicleAsync(targetVehicleId);
            targetAppraisalCount.Should().BeGreaterThan(0);
        }
        finally
        {
            if (appraisalId != Guid.Empty)
            {
                await _db.DeleteInspectionsByAppraisalAsync(appraisalId);
            }

            await CleanupVehicleAsync(sourceVehicleId, sourceVin);
            await CleanupVehicleAsync(targetVehicleId, targetVin);
        }
    }

    [Fact(DisplayName = "VC-MG-002 Merge vehicle with missing target returns 404")]
    public async Task VC_MG_002_MergeVehicle_MissingTarget_ShouldReturnNotFound()
    {
        var vehicleTypeId = Guid.NewGuid();
        var sourceRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var sourceVin = VehicleTestData.GetRequiredVin(sourceRequest);
        Guid sourceVehicleId = Guid.Empty;

        try
        {
            var sourceVehicle = await CreateVehicleAsync(sourceRequest);
            sourceVehicleId = sourceVehicle.VehicleId;

            var mergeRequest = new MergeVehicleRequest
            {
                TargetVehicleId = Guid.NewGuid()
            };

            var response = await _client.PostAsJsonAsync($"/api/vehicles/{sourceVehicleId}/merge", mergeRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
            problem.Should().NotBeNull();
            problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
        }
        finally
        {
            await CleanupVehicleAsync(sourceVehicleId, sourceVin);
        }
    }

    [Fact(DisplayName = "VC-MG-003 Merge vehicle with issued appraisal returns 409 and preserves appraisal link")]
    public async Task VC_MG_003_MergeVehicle_WithIssuedAppraisal_ShouldReturnConflictAndKeepSourceLink()
    {
        var vehicleTypeId = Guid.NewGuid();
        var sourceRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var targetRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);

        var sourceVin = VehicleTestData.GetRequiredVin(sourceRequest);
        var targetVin = VehicleTestData.GetRequiredVin(targetRequest);

        Guid sourceVehicleId = Guid.Empty;
        Guid targetVehicleId = Guid.Empty;
        Guid appraisalId = Guid.Empty;

        try
        {
            var sourceVehicle = await CreateVehicleAsync(sourceRequest);
            sourceVehicleId = sourceVehicle.VehicleId;

            var targetVehicle = await CreateVehicleAsync(targetRequest);
            targetVehicleId = targetVehicle.VehicleId;

            var appraisal = await CreateAppraisalAsync(sourceVehicleId);
            appraisalId = appraisal.AppraisalId;

            await _db.SetAppraisalStatusAsync(appraisalId, "Issued");

            var mergeRequest = new MergeVehicleRequest
            {
                TargetVehicleId = targetVehicleId
            };

            var response = await _client.PostAsJsonAsync($"/api/vehicles/{sourceVehicleId}/merge", mergeRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var sourceAppraisalCount = await _db.CountAppraisalsByVehicleAsync(sourceVehicleId);
            sourceAppraisalCount.Should().Be(1);

            var targetAppraisalCount = await _db.CountAppraisalsByVehicleAsync(targetVehicleId);
            targetAppraisalCount.Should().Be(0);
        }
        finally
        {
            if (appraisalId != Guid.Empty)
            {
                await _db.DeleteInspectionsByAppraisalAsync(appraisalId);
                await _db.DeleteAppraisalAsync(appraisalId);
            }

            await CleanupVehicleAsync(sourceVehicleId, sourceVin);
            await CleanupVehicleAsync(targetVehicleId, targetVin);
        }
    }

    [Fact(DisplayName = "VC-I-001 POST then GET returns same persisted vehicle")]
    public async Task VC_I_001_PostThenGet_ShouldRoundTripSuccessfully()
    {
    var vehicleTypeId = Guid.NewGuid();
    var request = VehicleTestData.CreateValidRequest(vehicleTypeId);
    var vin = request.Vin
        ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");

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
        var vehicleTypeId = Guid.NewGuid();
        var createRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var createVin = createRequest.Vin
            ?? throw new InvalidOperationException("The generated vehicle test request must contain a VIN.");

        Guid vehicleId = Guid.Empty;

        try
        {
            var created = await CreateVehicleAsync(createRequest);
            vehicleId = created.VehicleId;

            var current = await GetVehicleAsync(vehicleId);

            var updateRequest = VehicleTestData.CreateValidUpdate(createVin, current.RowVersion);

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

            var staleUpdateRequest = VehicleTestData.CreateValidUpdate(createVin, current.RowVersion);
            staleUpdateRequest.Make = "StaleUpdate";

            var staleUpdateResponse = await _client.PutAsJsonAsync($"/api/vehicles/{vehicleId}", staleUpdateRequest);
            staleUpdateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var staleProblem = await staleUpdateResponse.Content.ReadAsAsync<ProblemDetailsResponse>();
            staleProblem.Should().NotBeNull();
            staleProblem!.Status.Should().Be((int)HttpStatusCode.Conflict);
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

    [Fact(DisplayName = "VC-S-001 Search vehicles by partial term returns matching rows")]
    public async Task VC_S_001_SearchVehicles_ByPartialTerm_ShouldReturnMatches()
    {
        var vehicleTypeId = Guid.NewGuid();
        var firstRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        firstRequest.Make = "SearchableMake";
        firstRequest.Model = "AlphaModel";

        var secondRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        secondRequest.Make = "AnotherMake";
        secondRequest.Model = "DifferentModel";

        var firstVin = VehicleTestData.GetRequiredVin(firstRequest);
        var secondVin = VehicleTestData.GetRequiredVin(secondRequest);
        Guid firstVehicleId = Guid.Empty;
        Guid secondVehicleId = Guid.Empty;

        try
        {
            var firstVehicle = await CreateVehicleAsync(firstRequest);
            firstVehicleId = firstVehicle.VehicleId;

            var secondVehicle = await CreateVehicleAsync(secondRequest);
            secondVehicleId = secondVehicle.VehicleId;

            var response = await _client.GetAsync("/api/vehicles/search?search=Searchable&page=1&pageSize=20");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadFromJsonAsync<PagedResult<VehicleSummaryDto>>();
            payload.Should().NotBeNull();
            payload!.Items.Should().Contain(item => item.VehicleId == firstVehicleId);
            payload.Items.Should().NotContain(item => item.VehicleId == secondVehicleId);
        }
        finally
        {
            await CleanupVehicleAsync(firstVehicleId, firstVin);
            await CleanupVehicleAsync(secondVehicleId, secondVin);
        }
    }

    [Fact(DisplayName = "VC-S-002 Search vehicles by year range returns only in-range rows")]
    public async Task VC_S_002_SearchVehicles_ByYearRange_ShouldFilterRows()
    {
        var vehicleTypeId = Guid.NewGuid();
        var firstRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        firstRequest.Year = 1965;
        firstRequest.Make = "RangeMake";
        firstRequest.Model = "OutOfRange";

        var secondRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        secondRequest.Year = 1988;
        secondRequest.Make = "RangeMake";
        secondRequest.Model = "InRange";

        var firstVin = VehicleTestData.GetRequiredVin(firstRequest);
        var secondVin = VehicleTestData.GetRequiredVin(secondRequest);
        Guid firstVehicleId = Guid.Empty;
        Guid secondVehicleId = Guid.Empty;

        try
        {
            var firstVehicle = await CreateVehicleAsync(firstRequest);
            firstVehicleId = firstVehicle.VehicleId;

            var secondVehicle = await CreateVehicleAsync(secondRequest);
            secondVehicleId = secondVehicle.VehicleId;

            var response = await _client.GetAsync("/api/vehicles/search?make=RangeMake&yearFrom=1980&yearTo=1990&page=1&pageSize=20");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadFromJsonAsync<PagedResult<VehicleSummaryDto>>();
            payload.Should().NotBeNull();
            payload!.Items.Should().Contain(item => item.VehicleId == secondVehicleId);
            payload.Items.Should().NotContain(item => item.VehicleId == firstVehicleId);
        }
        finally
        {
            await CleanupVehicleAsync(firstVehicleId, firstVin);
            await CleanupVehicleAsync(secondVehicleId, secondVin);
        }
    }

    [Fact(DisplayName = "VC-A-001 Get appraisals by vehicle id returns only that vehicle's appraisals")]
    public async Task VC_A_001_GetAppraisalsByVehicleId_ShouldReturnAssociatedAppraisals()
    {
        var vehicleTypeId = Guid.NewGuid();
        var firstVehicleRequest = VehicleTestData.CreateValidRequest(vehicleTypeId);
        var secondVehicleRequest = VehicleTestData.CreateValidRequest(Guid.NewGuid());

        var firstVin = VehicleTestData.GetRequiredVin(firstVehicleRequest);
        var secondVin = VehicleTestData.GetRequiredVin(secondVehicleRequest);

        Guid firstVehicleId = Guid.Empty;
        Guid secondVehicleId = Guid.Empty;
        Guid firstAppraisalId = Guid.Empty;
        Guid secondAppraisalId = Guid.Empty;

        try
        {
            var firstVehicle = await CreateVehicleAsync(firstVehicleRequest);
            firstVehicleId = firstVehicle.VehicleId;

            var secondVehicle = await CreateVehicleAsync(secondVehicleRequest);
            secondVehicleId = secondVehicle.VehicleId;

            var firstAppraisal = await CreateAppraisalAsync(firstVehicleId);
            firstAppraisalId = firstAppraisal.AppraisalId;

            var secondAppraisal = await CreateAppraisalAsync(secondVehicleId);
            secondAppraisalId = secondAppraisal.AppraisalId;

            var response = await _client.GetAsync($"/api/vehicles/{firstVehicleId}/appraisals?page=1&pageSize=20");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadAsAsync<AppraisalListResponse>();
            payload.Should().NotBeNull();
            payload!.Items.Should().Contain(item => item.AppraisalId == firstAppraisalId);
            payload.Items.Should().NotContain(item => item.AppraisalId == secondAppraisalId);
            payload.Items.Should().OnlyContain(item => item.VehicleId == firstVehicleId);
        }
        finally
        {
            if (firstAppraisalId != Guid.Empty)
            {
                await _db.DeleteAppraisalAsync(firstAppraisalId);
            }

            if (secondAppraisalId != Guid.Empty)
            {
                await _db.DeleteAppraisalAsync(secondAppraisalId);
            }

            await CleanupVehicleAsync(firstVehicleId, firstVin);
            await CleanupVehicleAsync(secondVehicleId, secondVin);
        }
    }

    [Fact(DisplayName = "VC-A-002 Get appraisals for non-existent vehicle returns 404")]
    public async Task VC_A_002_GetAppraisalsByVehicleId_NonExistentVehicle_ShouldReturnNotFound()
    {
        var vehicleId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/vehicles/{vehicleId}/appraisals?page=1&pageSize=20");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadAsAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    private async Task<VehicleResponse> CreateVehicleAsync(ONE_Classics_Nexus.API.Contracts.Vehicles.CreateVehicleRequest request)
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

    private async Task<InspectionResponse> CreateInspectionAsync(Guid appraisalId)
    {
        var now = DateTime.UtcNow;

        var request = new CreateInspectionRequest(
            appraisalId,
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            null,
            "Standard",
            "Draft",
            "Test Facility",
            now,
            null,
            null,
            false,
            "Vehicle inspections endpoint test",
            1,
            now,
            now);

        var response = await _client.PostAsJsonAsync("/api/inspections", request);
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(
            HttpStatusCode.Created,
            $"POST /api/inspections should succeed, but returned {(int)response.StatusCode}: {body}");

        var inspection = await response.Content.ReadAsAsync<InspectionResponse>();
        inspection.Should().NotBeNull();

        return inspection!;
    }

    private async Task CleanupVehicleAsync(Guid vehicleId, string vin)
    {
        if (vehicleId != Guid.Empty)
        {
            await _db.DeleteInspectionsByVehicleAsync(vehicleId);
            await _db.DeleteAppraisalsByVehicleAsync(vehicleId);
            await _db.DeleteVehicleAsync(vehicleId);
        }
        else
            await _db.DeleteVehicleByVinAsync(vin);
    }
}