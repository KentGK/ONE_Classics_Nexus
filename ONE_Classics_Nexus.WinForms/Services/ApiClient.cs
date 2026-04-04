using System.Net.Http.Json;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Requests;

namespace ONE_Classics_Nexus.WinForms.Services;

public sealed class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VehicleDto?> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/vehicles", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VehicleDto>(cancellationToken: ct);
    }

    public async Task<AppraisalDto?> CreateAppraisalAsync(CreateAppraisalRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/appraisals", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AppraisalDto>(cancellationToken: ct);
    }

    public async Task<InspectionDto?> CreateInspectionAsync(CreateInspectionRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/inspections", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<InspectionDto>(cancellationToken: ct);
    }

    public async Task<InspectionDto?> SaveInspectionScoresAsync(SaveInspectionRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/inspections/{request.InspectionId}/scores", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<InspectionDto>(cancellationToken: ct);
    }
}