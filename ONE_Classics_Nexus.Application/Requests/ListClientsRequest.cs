using ONE_Classics_Nexus.Application.Abstractions.Repositories;

namespace ONE_Classics_Nexus.Application.Requests;

/// <summary>
/// Query parameters for the Client List.
/// </summary>
public sealed class ListClientsRequest
{
    /// <summary>
    /// Partial, case-insensitive search across Display Name and Email.
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Client Type filter; null or empty returns all types.
    /// </summary>
    public string? ClientTypeCode { get; set; }

    /// <summary>
    /// Client Status filter. Defaults to Active so the list opens on active
    /// records; pass <see cref="AllStatuses"/> to return every status.
    /// </summary>
    public string? StatusCode { get; set; } = Domain.Entities.ClientStatusCodes.Active;

    public ClientSortBy SortBy { get; set; } = ClientSortBy.DisplayName;

    public ClientSortDirection SortDirection { get; set; } = ClientSortDirection.Ascending;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Sentinel for "no status filter". A null StatusCode cannot carry this
    /// meaning because null is indistinguishable from an omitted query string
    /// value, which must fall back to the Active default.
    /// </summary>
    public const string AllStatuses = "All";
}
