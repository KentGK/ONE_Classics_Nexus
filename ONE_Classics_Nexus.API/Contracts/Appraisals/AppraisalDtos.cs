namespace ONE_Classics_Nexus.API.Contracts.Appraisals;

public sealed record CreateAppraisalRequest(
    Guid VehicleId,
    string? AppraisalNumber,
    Guid? AppraisalRequestId,
    Guid? ClientId,
    Guid? AssignedAppraiserId,
    Guid? AssignedReviewerId,
    string? AppraisalType,
    string Status,
    string? IntakeStatus,
    string? InspectionStatus,
    string? ValuationStatus,
    string? ReportStatus,
    DateTime? EffectiveDateUtc);

public sealed record UpdateAppraisalRequest(
    Guid VehicleID,
    string? AppraisalNumber,
    Guid? AppraisalRequestId,
    Guid? ClientId,
    Guid? AssignedAppraiserId,
    Guid? AssignedReviewerId,
    string? AppraisalType,
    string Status,
    string? IntakeStatus,
    string? InspectionStatus,
    string? ValuationStatus,
    string? ReportStatus,
    DateTime? EffectiveDateUtc,
    long RowVersion);