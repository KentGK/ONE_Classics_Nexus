using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace ONE_Classics_Nexus.WinForms.Services.Api;

/// <summary>
/// Query parameters for a Client List request.
/// </summary>
public sealed record ClientListQuery
{
    public string? Search { get; init; }

    public string? ClientTypeCode { get; init; }

    /// <summary>
    /// Status filter. Use "All" to return every status; leaving this null
    /// lets the API apply its Active default.
    /// </summary>
    public string? StatusCode { get; init; }

    public ClientSortBy SortBy { get; init; } = ClientSortBy.DisplayName;

    public ClientSortDirection SortDirection { get; init; } = ClientSortDirection.Ascending;

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}

/// <summary>
/// HTTP client for the Clients API.
/// </summary>
public sealed class ClientApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public ClientApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PagedResult<ClientListItemDto>> ListAsync(
        ClientListQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var values = new Dictionary<string, string?>
        {
            ["search"] = query.Search,
            ["clientTypeCode"] = query.ClientTypeCode,
            ["statusCode"] = query.StatusCode,
            ["sortBy"] = query.SortBy.ToString(),
            ["sortDirection"] = query.SortDirection.ToString(),
            ["page"] = query.Page.ToString(CultureInfo.InvariantCulture),
            ["pageSize"] = query.PageSize.ToString(CultureInfo.InvariantCulture)
        };

        var uri = $"api/clients{BuildQuery(values)}";

        using var response = await _httpClient
            .GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<PagedResult<ClientListItemDto>>(JsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return result ?? new PagedResult<ClientListItemDto>([], query.Page, query.PageSize, 0);
    }

    private static string BuildQuery(IReadOnlyDictionary<string, string?> values)
    {
        var query = values
            .Where(pair => !string.IsNullOrWhiteSpace(pair.Value))
            .Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value!)}");

        var joined = string.Join("&", query);
        return joined.Length == 0 ? string.Empty : $"?{joined}";
    }
}
