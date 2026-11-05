using ONE_Classics_Nexus.API.Contracts.Inspections;
using System.Net;
using System.Net.Http.Json;

namespace ONE_Classics_Nexus.WinForms.Services;

public sealed class InspectionApiService : IInspectionApiService
{
    private readonly HttpClient _httpClient;

    public InspectionApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<InspectionResponse?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"api/inspections/{inspectionId}",
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InspectionResponse>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to get inspection {inspectionId}", ex);
        }
    }

    public async Task<InspectionResponse> CreateAsync(
        CreateInspectionRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/inspections", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var inspection = await response.Content.ReadFromJsonAsync<InspectionResponse>(cancellationToken: cancellationToken);
            return inspection ?? throw new InvalidOperationException("Inspection create response was empty.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to create inspection", ex);
        }
    }

    public async Task<InspectionResponse?> SaveScoresAsync(
        Guid inspectionId,
        SaveInspectionScoresRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/inspections/{inspectionId}/scores", request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InspectionResponse>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to save inspection scores for {inspectionId}", ex);
        }
    }

    public async Task<InspectionListResponse> GetByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"api/appraisals/{appraisalId}/inspections",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<InspectionListResponse>(cancellationToken: cancellationToken);
            return result ?? new InspectionListResponse();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to get inspections for appraisal {appraisalId}", ex);
        }
    }

    public async Task<bool> UpdateAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/inspections/{inspectionId}", request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to update inspection {inspectionId}", ex);
        }
    }

    public async Task<bool> DeleteAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/inspections/{inspectionId}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to delete inspection {inspectionId}", ex);
        }
    }

    public async Task<IReadOnlyList<InspectionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/inspections", cancellationToken);
            response.EnsureSuccessStatusCode();

            var inspections = await response.Content
                .ReadFromJsonAsync<IReadOnlyList<InspectionResponse>>(cancellationToken: cancellationToken);

            return inspections ?? Array.Empty<InspectionResponse>();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to get inspections", ex);
        }
    }
}