using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Paging;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard(
        [FromServices] IAppraisalRepository appraisalRepository,
        [FromServices] IVehicleRepository vehicleRepository,
        CancellationToken cancellationToken)
    {
        var appraisals = await appraisalRepository.BrowseAsync(
            new AppraisalBrowseFilter { Page = 1, PageSize = 200, SortBy = "updatedatutc" },
            cancellationToken);

        var workQueue = new List<DashboardWorkQueueItem>();
        foreach (var appraisal in appraisals.Items)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(appraisal.VehicleId, cancellationToken);
            var vehicleId = appraisal.VehicleId.ToString("N");
            var vehicleDisplay = vehicle is null
                ? vehicleId[..7]
                : $"{vehicle.Year} {vehicle.Make} {vehicle.Model} {vehicleId[..7]}";

            workQueue.Add(new DashboardWorkQueueItem(
                appraisal.AppraisalId,
                appraisal.VehicleId,
                appraisal.AppraisalNumber ?? appraisal.AppraisalId.ToString(),
                vehicleDisplay,
                appraisal.Status,
                appraisal.Status));
        }

        return Ok(new DashboardResponse(
            new DashboardSummary(
                workQueue.Count(item => !string.Equals(item.Stage, "Complete", StringComparison.OrdinalIgnoreCase)),
                0,
                0,
                workQueue.Count(item => string.Equals(item.Stage, "Needs Review", StringComparison.OrdinalIgnoreCase))),
            Array.Empty<DashboardActivity>(),
            workQueue));
    }

    private sealed record DashboardResponse(
        DashboardSummary Summary,
        IReadOnlyList<DashboardActivity> RecentActivity,
        IReadOnlyList<DashboardWorkQueueItem> WorkQueue);

    private sealed record DashboardSummary(
        int ActiveAppraisals,
        int PendingInspections,
        int ValuationsReady,
        int NeedsReview);

    private sealed record DashboardActivity(
        DateTime OccurredAtUtc,
        string ActivityType,
        string VehicleDisplay,
        string Status);

    private sealed record DashboardWorkQueueItem(
        Guid AppraisalId,
        Guid VehicleId,
        string AppraisalNumber,
        string VehicleDisplay,
        string Stage,
        string Priority);
}
