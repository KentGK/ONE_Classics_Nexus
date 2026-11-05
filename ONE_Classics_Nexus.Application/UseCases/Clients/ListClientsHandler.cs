using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.UseCases.Clients;

/// <summary>
/// Returns the filtered, sorted, paged Client List.
/// </summary>
public sealed class ListClientsHandler
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 200;

    private readonly IClientRepository _repository;

    public ListClientsHandler(IClientRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PagedResult<ClientListItemDto>> HandleAsync(
        ListClientsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var filter = new ClientBrowseFilter
        {
            Search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim(),
            ClientTypeCode = NormalizeFilterValue(request.ClientTypeCode),
            StatusCode = ResolveStatusFilter(request.StatusCode),
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
            Page = request.Page < 1 ? 1 : request.Page,
            PageSize = request.PageSize is < 1 or > MaxPageSize ? DefaultPageSize : request.PageSize
        };

        var result = await _repository
            .ListAsync(filter, cancellationToken)
            .ConfigureAwait(false);

        var items = result.Items
            .Select(client => new ClientListItemDto(
                client.ClientId,
                client.DisplayName,
                client.Email,
                client.ClientTypeCode,
                client.StatusCode))
            .ToList();

        return new PagedResult<ClientListItemDto>(
            items,
            result.Page,
            result.PageSize,
            result.TotalCount);
    }

    /// <summary>
    /// Maps the request's status filter to a repository filter value. An
    /// omitted value keeps the Active default; the AllStatuses sentinel clears
    /// the filter so every status is returned.
    /// </summary>
    private static string? ResolveStatusFilter(string? statusCode)
    {
        if (string.IsNullOrWhiteSpace(statusCode))
        {
            return ClientStatusCodes.Active;
        }

        return string.Equals(
            statusCode.Trim(),
            ListClientsRequest.AllStatuses,
            StringComparison.OrdinalIgnoreCase)
            ? null
            : statusCode.Trim();
    }

    private static string? NormalizeFilterValue(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
