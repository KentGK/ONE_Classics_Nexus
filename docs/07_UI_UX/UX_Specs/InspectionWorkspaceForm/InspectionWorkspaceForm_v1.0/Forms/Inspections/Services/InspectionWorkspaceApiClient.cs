using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.WinForms.Forms.Inspections.Contracts;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections.Services;

public sealed class InspectionWorkspaceApiClient : IInspectionWorkspaceApiClient
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

    private readonly HttpClient _httpClient;
    private readonly bool _ownsClient;

    public InspectionWorkspaceApiClient(
        HttpClient httpClient,
        bool ownsClient = false)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ownsClient = ownsClient;
    }

    public async Task<IReadOnlyList<InspectionListItemResponse>> GetInspectionListAsync(
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            "api/inspections",
            cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(json))
            return Array.Empty<InspectionListItemResponse>();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (root.ValueKind == JsonValueKind.Array)
        {
            return JsonSerializer.Deserialize<List<InspectionListItemResponse>>(
                       root.GetRawText(),
                       JsonOptions)
                   ?? new List<InspectionListItemResponse>();
        }

        foreach (var propertyName in new[] { "items", "data", "results", "value" })
        {
            if (root.TryGetProperty(propertyName, out var items) &&
                items.ValueKind == JsonValueKind.Array)
            {
                return JsonSerializer.Deserialize<List<InspectionListItemResponse>>(
                           items.GetRawText(),
                           JsonOptions)
                       ?? new List<InspectionListItemResponse>();
            }
        }

        return Array.Empty<InspectionListItemResponse>();
    }

    public async Task<InspectionWorkspaceResponse> GetWorkspaceAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        var inspectionTask = GetRequiredAsync<InspectionWorkspaceResponse>(
            $"api/inspections/{inspectionId:D}",
            cancellationToken);

        var scoresTask = GetOptionalAsync<List<ConditionScoreResponse>>(
            $"api/inspections/{inspectionId:D}/scores",
            cancellationToken);

        await Task.WhenAll(inspectionTask, scoresTask);

        var workspace = await inspectionTask;
        workspace.ConditionScores =
            await scoresTask ?? new List<ConditionScoreResponse>();

        workspace.Appraisal ??= await GetOptionalAsync<AppraisalSummaryResponse>(
            $"api/appraisals/{workspace.AppraisalId:D}",
            cancellationToken);

        if (workspace.Vehicle is null &&
            workspace.Appraisal?.VehicleId is Guid vehicleId &&
            vehicleId != Guid.Empty)
        {
            workspace.Vehicle = await GetOptionalAsync<VehicleSummaryResponse>(
                $"api/vehicles/{vehicleId:D}",
                cancellationToken);
        }

        return workspace;
    }

    public async Task UpdateInspectionAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(
            $"api/inspections/{inspectionId:D}",
            request,
            JsonOptions,
            cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task SaveConditionScoresAsync(
        Guid inspectionId,
        SaveConditionScoresRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(
            $"api/inspections/{inspectionId:D}/scores",
            request,
            JsonOptions,
            cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task DeleteInspectionAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(
            $"api/inspections/{inspectionId:D}",
            cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
    }

    public void Dispose()
    {
        if (_ownsClient)
            _httpClient.Dispose();
    }

    private async Task<T> GetRequiredAsync<T>(
        string path,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(path, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        return await response.Content.ReadFromJsonAsync<T>(
                   JsonOptions,
                   cancellationToken)
               ?? throw new InvalidOperationException(
                   $"The API returned an empty response for '{path}'.");
    }

    private async Task<T?> GetOptionalAsync<T>(
        string path,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(path, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;

        await EnsureSuccessAsync(response, cancellationToken);

        if (response.Content.Headers.ContentLength == 0)
            return default;

        return await response.Content.ReadFromJsonAsync<T>(
            JsonOptions,
            cancellationToken);
    }

    private static async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        ProblemDetailsResponse problem;

        try
        {
            problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>(
                          JsonOptions,
                          cancellationToken)
                      ?? new ProblemDetailsResponse();
        }
        catch
        {
            problem = new ProblemDetailsResponse
            {
                Status = (int)response.StatusCode,
                Title = response.ReasonPhrase,
                Detail = await response.Content.ReadAsStringAsync(cancellationToken)
            };
        }

        throw new ApiProblemException((int)response.StatusCode, problem);
    }
}

public static class InspectionWorkspaceApiClientFactory
{
    public static IInspectionWorkspaceApiClient CreateDefault()
    {
        var baseUrl =
            Environment.GetEnvironmentVariable("ONE_CLASSICS_NEXUS_API_BASE_URL")
            ?? "https://localhost:5001/";

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl, UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(30)
        };

        return new InspectionWorkspaceApiClient(
            httpClient,
            ownsClient: true);
    }
}

public sealed class ApiProblemException : Exception
{
    public ApiProblemException(
        int statusCode,
        ProblemDetailsResponse problem)
        : base(
            problem.Detail
            ?? problem.Title
            ?? $"API request failed with status {statusCode}.")
    {
        StatusCode = statusCode;
        Problem = problem;
    }

    public int StatusCode { get; }
    public ProblemDetailsResponse Problem { get; }
}
