using ONE_Classics_Nexus.API.Contracts.Vehicles;
using System.Net;
using System.Net.Http.Json;

namespace ONE_Classics_Nexus.WinForms.Services;

public sealed class VehicleApiService : IVehicleApiService
{
    private readonly HttpClient _httpClient;

    public VehicleApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IReadOnlyList<VehicleResponse>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<PagedVehicleResponse>(
                "api/vehicles?page=1&pageSize=200",
                cancellationToken);

            return result?.Items ?? new List<VehicleResponse>();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to list vehicles", ex);
        }
    }

    private sealed class PagedVehicleResponse
    {
        public List<VehicleResponse> Items { get; set; } = new();
    }

    public async Task<VehicleResponse?> GetByIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"api/vehicles/{vehicleId}";
            System.Diagnostics.Debug.WriteLine($"GET Request: {_httpClient.BaseAddress}{url}");

            var response = await _httpClient.GetAsync(url, cancellationToken);

            System.Diagnostics.Debug.WriteLine($"Response Status: {response.StatusCode}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                System.Diagnostics.Debug.WriteLine("Vehicle not found (404)");
                return null;
            }

            response.EnsureSuccessStatusCode();

            var vehicle = await response.Content.ReadFromJsonAsync<VehicleResponse>(cancellationToken: cancellationToken);

            if (vehicle == null)
            {
                throw new InvalidOperationException("API returned null vehicle response");
            }

            System.Diagnostics.Debug.WriteLine($"Vehicle loaded: {vehicle.VehicleId}");
            return vehicle;
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"HTTP Request failed: {ex.Message}");
            throw new InvalidOperationException(
                $"Failed to get vehicle {vehicleId}. API may not be running or reachable.",
                ex);
        }
    }

    public async Task<VehicleResponse> CreateAsync(
        CreateVehicleRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/vehicles", request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new InvalidOperationException("409: A vehicle with this VIN already exists.");
            }

            response.EnsureSuccessStatusCode();

            var vehicle = await response.Content.ReadFromJsonAsync<VehicleResponse>(cancellationToken: cancellationToken);
            return vehicle ?? throw new InvalidOperationException("Vehicle create response was empty.");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            throw new InvalidOperationException("A vehicle with this VIN already exists.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to create vehicle", ex);
        }
    }

    public async Task<bool> UpdateAsync(
        Guid vehicleId,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/vehicles/{vehicleId}", request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true; // API returns 204 NoContent on success
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to update vehicle {vehicleId}", ex);
        }
    }

    public async Task<bool> DeleteAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        // Not yet implemented in API
        await Task.Yield();
        throw new NotImplementedException("DELETE endpoint not yet implemented in API.");
    }
}