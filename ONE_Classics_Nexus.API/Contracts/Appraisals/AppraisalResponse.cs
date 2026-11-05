namespace ONE_Classics_Nexus.API.Contracts.Appraisals;

public sealed class AppraisalResponse
{
    public Guid VehicleId { get; set; }
    public Guid AppraisalId { get; set; }
    public string? AppraisalNumber { get; set; }
    public Guid? AppraisalRequestId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string? AppraisalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IntakeStatus { get; set; }
    public string? InspectionStatus { get; set; }
    public string? ValuationStatus { get; set; }
    public string? ReportStatus { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
    public long RowVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}