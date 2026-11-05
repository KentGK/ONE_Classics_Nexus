namespace ONE_Classics_Nexus.API.Contracts.Appraisals;

public sealed record CreateAppraisalRequest(
    Guid VehicleId,
    string AppraisalType,
    string? AppraisalNumber = null,
    Guid? AppraisalRequestId = null,
    Guid? ClientId = null,
    Guid? AssignedAppraiserId = null,
    Guid? AssignedReviewerId = null,
    string? IntakeStatus = null,
    string? InspectionStatus = null,
    string? ValuationStatus = null,
    string? ReportStatus = null,
    DateTime? EffectiveDateUtc = null);

public sealed record UpdateAppraisalRequest(
    Guid AppraisalId,
    Guid VehicleId,
    string AppraisalType,
    string Status,
    long RowVersion,
    string? AppraisalNumber = null,
    Guid? AppraisalRequestId = null,
    Guid? ClientId = null,
    Guid? AssignedAppraiserId = null,
    Guid? AssignedReviewerId = null,
    string? IntakeStatus = null,
    string? InspectionStatus = null,
    string? ValuationStatus = null,
    string? ReportStatus = null,
    DateTime? EffectiveDateUtc = null);