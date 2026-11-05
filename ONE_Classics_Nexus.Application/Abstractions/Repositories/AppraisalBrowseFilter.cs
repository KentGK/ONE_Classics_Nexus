namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

/// <summary>
/// Filter/paging/sorting options for browsing appraisal records.
/// </summary>
public sealed class AppraisalBrowseFilter
{
    public Guid? VehicleId { get; init; }
    public Guid? ClientId { get; init; }
    public Guid? AssignedAppraiserId { get; init; }
    public Guid? AssignedReviewerId { get; init; }
    public string? Status { get; init; }
    public string? AppraisalType { get; init; }
    public string? Search { get; init; }
    public DateTime? EffectiveDateFromUtc { get; init; }
    public DateTime? EffectiveDateToUtc { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;

    public string? SortBy { get; init; }
    public bool SortDescending { get; init; } = true;
}