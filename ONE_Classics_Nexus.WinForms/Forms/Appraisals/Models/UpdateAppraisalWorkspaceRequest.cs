namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;

public sealed class UpdateAppraisalWorkspaceRequest
{
    public Guid AppraisalId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? AppraisalTypeId { get; set; }
    public Guid? AppraisalStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string AppraisalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IntakeStatus { get; set; }
    public string? InspectionStatus { get; set; }
    public string? ValuationStatus { get; set; }
    public string? ReportStatus { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
    public long RowVersion { get; set; }
}
