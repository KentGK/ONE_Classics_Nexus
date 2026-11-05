namespace ONE_Classics_Nexus.API.Contracts.Appraisals;

/// <summary>
/// Query parameters used to browse/filter/paginate appraisal records.
/// </summary>
public sealed class BrowseAppraisalsRequest
{
    public Guid? VehicleId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string? Status { get; set; }
    public string? AppraisalType { get; set; }
    public string? Search { get; set; }
    public DateTime? EffectiveDateFromUtc { get; set; }
    public DateTime? EffectiveDateToUtc { get; set; }

    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    private int _pageSize = 25;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            < 1 => 25,
            > 200 => 200,
            _ => value
        };
    }

    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}