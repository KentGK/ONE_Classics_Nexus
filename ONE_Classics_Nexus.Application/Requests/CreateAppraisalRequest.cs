using System;

namespace ONE_Classics_Nexus.Application.Requests;

public sealed class CreateAppraisalRequest
{
    public Guid VehicleId { get; set; }
    public string? AppraisalNumber { get; set; }
    public Guid? AppraisalRequestId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string? AppraisalType { get; set; }
    public string Status { get; set; } = "Draft";
    public string? IntakeStatus { get; set; }
    public string? InspectionStatus { get; set; }
    public string? ValuationStatus { get; set; }
    public string? ReportStatus { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
}