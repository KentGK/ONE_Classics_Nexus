using ONE_Classics_Nexus.API.Contracts.Appraisals;
using System.Net;
using System.Net.Http.Json;

namespace ONE_Classics_Nexus.WinForms.Services;

public sealed class AppraisalApiService : IAppraisalApiService
{
    private readonly HttpClient _httpClient;

    public AppraisalApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<AppraisalResponse?> GetByIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/appraisals/{appraisalId}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AppraisalResponse>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to get appraisal {appraisalId}", ex);
        }
    }

    public async Task<AppraisalResponse> CreateAsync(
        CreateAppraisalRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/appraisals", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var appraisal = await response.Content.ReadFromJsonAsync<AppraisalResponse>(cancellationToken: cancellationToken);
            return appraisal ?? throw new InvalidOperationException("Appraisal create response was empty.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to create appraisal", ex);
        }
    }

    public async Task<AppraisalResponse> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/appraisals/{appraisalId}", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var appraisal = await response.Content.ReadFromJsonAsync<AppraisalResponse>(cancellationToken: cancellationToken);
            return appraisal ?? throw new InvalidOperationException("Appraisal update response was empty.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to update appraisal {appraisalId}", ex);
        }
    }
}