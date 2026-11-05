namespace ONE_Classics_Nexus.API.Contracts.Appraisals;

/// <summary>
/// Paged result of appraisal records returned by the browse/list endpoint.
/// </summary>
public sealed class AppraisalListResponse
{
    public IReadOnlyList<AppraisalResponse> Items { get; set; } = Array.Empty<AppraisalResponse>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}