using ONE_Classics_Nexus.WinForms.Models.AppraisalRequests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.WinForms.Services.Api;

public sealed partial class AppraisalRequestApiClient : IAppraisalRequestApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public AppraisalRequestApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public Task<PagedResult<AppraisalRequestListItemDto>> ListAsync(
        AppraisalRequestListQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var values = new Dictionary<string, string?>
        {
            ["query"] = query.Query,
            ["status"] = query.Status,
            ["clientId"] = query.ClientId?.ToString("D"),
            ["requestedFromUtc"] = FormatUtc(query.RequestedFromUtc),
            ["requestedToUtc"] = FormatUtc(query.RequestedToUtc),
            ["sort"] = query.Sort,
            ["page"] = query.Page.ToString(CultureInfo.InvariantCulture),
            ["pageSize"] = query.PageSize.ToString(CultureInfo.InvariantCulture)
        };

        return GetAsync<PagedResult<AppraisalRequestListItemDto>>(
            $"api/appraisal-requests{BuildQuery(values)}",
            cancellationToken);
    }

    public Task<AppraisalRequestDetailDto> GetAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken = default) =>
        GetAsync<AppraisalRequestDetailDto>(
            $"api/appraisal-requests/{appraisalRequestId:D}",
            cancellationToken);

    public Task<AppraisalRequestDetailDto> CreateAsync(
        CreateAppraisalRequestRequest request,
        CancellationToken cancellationToken = default) =>
        SendAsync<AppraisalRequestDetailDto>(
            HttpMethod.Post,
            "api/appraisal-requests",
            request,
            cancellationToken);

    public Task<AppraisalRequestDetailDto> UpdateAsync(
        Guid appraisalRequestId,
        UpdateAppraisalRequestRequest request,
        CancellationToken cancellationToken = default) =>
        SendAsync<AppraisalRequestDetailDto>(
            HttpMethod.Put,
            $"api/appraisal-requests/{appraisalRequestId:D}",
            request,
            cancellationToken);

    public Task<ValidationResponse> ValidateAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken = default) =>
        SendAsync<ValidationResponse>(
            HttpMethod.Post,
            $"api/appraisal-requests/{appraisalRequestId:D}/validate",
            body: null,
            cancellationToken: cancellationToken);

    public Task<AppraisalRequestDetailDto> SubmitAsync(
        Guid appraisalRequestId,
        ExpectedRowVersionRequest request,
        CancellationToken cancellationToken = default) =>
        SendAsync<AppraisalRequestDetailDto>(
            HttpMethod.Post,
            $"api/appraisal-requests/{appraisalRequestId:D}/submit",
            request,
            cancellationToken);

    public Task<AppraisalRequestDetailDto> CancelAsync(
        Guid appraisalRequestId,
        DispositionRequest request,
        CancellationToken cancellationToken = default) =>
        SendAsync<AppraisalRequestDetailDto>(
            HttpMethod.Post,
            $"api/appraisal-requests/{appraisalRequestId:D}/cancel",
            request,
            cancellationToken);

    public Task<AppraisalRequestDetailDto> WithdrawAsync(
        Guid appraisalRequestId,
        DispositionRequest request,
        CancellationToken cancellationToken = default) =>
        SendAsync<AppraisalRequestDetailDto>(
            HttpMethod.Post,
            $"api/appraisal-requests/{appraisalRequestId:D}/withdraw",
            request,
            cancellationToken);

    public async Task DeleteDraftAsync(
        Guid appraisalRequestId,
        ExpectedRowVersionRequest request,
        CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Delete,
            $"api/appraisal-requests/{appraisalRequestId:D}")
        {
            Content = JsonContent.Create(request, options: JsonOptions)
        };

        using var response = await _httpClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.OK)
            return;

        await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public Task<IReadOnlyList<ReferenceOptionDto>> ListClientsAsync(
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<ReferenceOptionDto>>(
            "api/clients/options?status=Active",
            cancellationToken);

    public Task<IReadOnlyList<ReferenceOptionDto>> ListClientContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<ReferenceOptionDto>>(
            $"api/clients/{clientId:D}/contacts/options?status=Active",
            cancellationToken);

    public Task<IReadOnlyList<ReferenceOptionDto>> ListIntendedUsesAsync(
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<ReferenceOptionDto>>(
            "api/reference/intended-uses?activeOnly=true",
            cancellationToken);

    public Task<IReadOnlyList<ReferenceOptionDto>> ListValueDefinitionsAsync(
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<ReferenceOptionDto>>(
            "api/reference/value-definitions?activeOnly=true",
            cancellationToken);

    public Task<IReadOnlyList<ReferenceOptionDto>> ListAppraisalTypesAsync(
        CancellationToken cancellationToken = default) =>
        GetAsync<IReadOnlyList<ReferenceOptionDto>>(
            "api/reference/appraisal-types?activeOnly=true",
            cancellationToken);

    private async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken)
    {
        using var response = await _httpClient
            .GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        return await ReadResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> SendAsync<T>(
        HttpMethod method,
        string requestUri,
        object? body,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, requestUri);
        if (body is not null)
            request.Content = JsonContent.Create(body, options: JsonOptions);

        using var response = await _httpClient
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        return await ReadResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<T> ReadResponseAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);

        var result = await response.Content
            .ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return result ?? throw new AppraisalRequestApiException(
            response.StatusCode,
            "The API returned an empty response.",
            problem: null);
    }

    private static async Task ThrowApiExceptionAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ApiProblemDetails? problem = null;
        try
        {
            problem = await response.Content
                .ReadFromJsonAsync<ApiProblemDetails>(JsonOptions, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException)
        {
            // Preserve the status code even when the server does not return ProblemDetails JSON.
        }

        var message = problem?.Detail
            ?? problem?.Title
            ?? $"The Appraisal Request API returned {(int)response.StatusCode} {response.ReasonPhrase}.";

        var isConcurrencyConflict = response.StatusCode == HttpStatusCode.Conflict &&
            (problem?.Issues?.Any(issue =>
                string.Equals(issue.Code, "CONCURRENCY-001", StringComparison.OrdinalIgnoreCase)) == true ||
             message.Contains("row version", StringComparison.OrdinalIgnoreCase));

        if (isConcurrencyConflict)
            throw new AppraisalRequestConcurrencyException(message, problem);

        throw new AppraisalRequestApiException(response.StatusCode, message, problem);
    }

    private static string? FormatUtc(DateTime? value) =>
        value?.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);

    private static string BuildQuery(IReadOnlyDictionary<string, string?> values)
    {
        var query = values
            .Where(pair => !string.IsNullOrWhiteSpace(pair.Value))
            .Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value!)}");

        var joined = string.Join("&", query);
        return joined.Length == 0 ? string.Empty : $"?{joined}";
    }
}

public class AppraisalRequestApiException : Exception
{
    public AppraisalRequestApiException(
        HttpStatusCode statusCode,
        string message,
        ApiProblemDetails? problem)
        : base(message)
    {
        StatusCode = statusCode;
        Problem = problem;
    }

    public HttpStatusCode StatusCode { get; }
    public ApiProblemDetails? Problem { get; }
}

public sealed class AppraisalRequestConcurrencyException : AppraisalRequestApiException
{
    public AppraisalRequestConcurrencyException(string message, ApiProblemDetails? problem)
        : base(HttpStatusCode.Conflict, message, problem)
    {
    }
}
