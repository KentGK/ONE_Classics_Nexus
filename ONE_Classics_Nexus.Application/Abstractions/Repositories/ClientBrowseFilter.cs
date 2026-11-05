using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

/// <summary>
/// Sortable Client List columns. Mirrors the Display Name, Type and Status
/// column headings shown in the client browser.
/// </summary>
public enum ClientSortBy
{
    DisplayName = 0,
    ClientTypeCode = 1,
    StatusCode = 2
}

public enum ClientSortDirection
{
    Ascending = 0,
    Descending = 1
}

/// <summary>
/// Filter, sort and paging criteria for a Client List query.
/// </summary>
public sealed record ClientBrowseFilter
{
    /// <summary>
    /// Partial, case-insensitive search applied to display_name and email.
    /// </summary>
    public string? Search { get; init; }

    /// <summary>
    /// Exact client_type_code match; null returns all types.
    /// </summary>
    public string? ClientTypeCode { get; init; }

    /// <summary>
    /// Exact Status_Code match. Defaults to Active so the Client List opens on
    /// active records only; set to null explicitly to return all statuses.
    /// </summary>
    public string? StatusCode { get; init; } = ClientStatusCodes.Active;

    public ClientSortBy SortBy { get; init; } = ClientSortBy.DisplayName;

    public ClientSortDirection SortDirection { get; init; } = ClientSortDirection.Ascending;

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
