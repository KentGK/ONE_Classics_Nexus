// modify CreateVehicleAsync to ensure a valid VehicleTypeId is set before posting
private async Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request)
{
    // ensure the test database has a default vehicle type and assign its id if none provided
    if (request.VehicleTypeId == Guid.Empty)
    {
        var defaultTypeId = await _db.EnsureDefaultVehicleTypeAsync();
        request.VehicleTypeId = defaultTypeId;
    }

    var response = await _client.PostAsJsonAsync("/api/vehicles", request);

    response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

    var created = await response.Content.ReadAsAsync<VehicleResponse>();
    created.Should().NotBeNull();
    created!.VehicleId.Should().NotBeEmpty();

    return created;
}