namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record AppraisalDetailDto(
    Guid AppraisalId,
    Guid VehicleId,
    string? AppraisalNumber,
    Guid? AppraisalRequestId,
    Guid? ClientId,
    Guid? AssignedAppraiserId,
    Guid? AssignedReviewerId,
    string AppraisalType,
    string Status,
    string? IntakeStatus,
    string? InspectionStatus,
    string? ValuationStatus,
    string? ReportStatus,
    DateTime? EffectiveDateUtc,
    long RowVersion,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    VehicleDto Vehicle);