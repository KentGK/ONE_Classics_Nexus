namespace ONE_Classics_Nexus.Application.Abstractions.Paging;

/// <summary>
/// Generic paged result wrapper used by repository browse/list queries
/// and by application use-case handlers.
/// </summary>
public sealed record PagedResult<T>
{
    public PagedResult()
    {
    }

    public PagedResult(IReadOnlyList<T> items, int page, int pageSize, long totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public long TotalCount { get; init; }

    /// <summary>
    /// Total number of pages, or zero when there are no results.
    /// </summary>
    public int TotalPages => TotalCount == 0 || PageSize <= 0
        ? 0
        : (int)Math.Ceiling(TotalCount / (double)PageSize);
}