namespace ONE_Classics_Nexus.WinForms.Models.Ui
{
    public sealed class DashboardSummaryDto
    {
        public int ActiveAppraisals { get; set; }
        public int PendingInspections { get; set; }
        public int ValuationsReady { get; set; }
        public int NeedsReview { get; set; }
    }

    public sealed class DashboardActivityDto
    {
        public DateTime OccurredAtUtc { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string VehicleDisplay { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public sealed class DashboardWorkQueueItemDto
    {
        public Guid AppraisalId { get; set; }
        public Guid VehicleId { get; set; }
        public string AppraisalNumber { get; set; } = string.Empty;
        public string VehicleDisplay { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    public sealed class DashboardResponseDto
    {
        public DashboardSummaryDto Summary { get; set; } = new();
        public List<DashboardActivityDto> RecentActivity { get; set; } = new();
        public List<DashboardWorkQueueItemDto> WorkQueue { get; set; } = new();
    }
}