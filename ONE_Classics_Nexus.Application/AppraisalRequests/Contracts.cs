using ONE_Classics_Nexus.Domain.AppraisalRequests;
using System;

namespace ONE_Classics_Nexus.Application.AppraisalRequests;

public sealed record CreateAppraisalRequestRequest(Guid ClientId, Guid? ClientContactId, string RequestedAppraisalType,
    string RequestedPurpose, string? RequestedIntendedUse, string? RequestedValueType, DateOnly? RequestedEffectiveDate,
    DateOnly? RequestedCompletionDate, string? InspectionPreference, string? InspectionLocation, Guid? VehicleId,
    int? SubjectYear, string? SubjectMake, string? SubjectModel, string? SubjectVin, string? SubjectDescription,
    string? ClientComments, string? RequestSource, bool RequesterAcknowledgedVerification);

public sealed record UpdateAppraisalRequestRequest(Guid ClientId, Guid? ClientContactId, string RequestedAppraisalType,
    string RequestedPurpose, string? RequestedIntendedUse, string? RequestedValueType, DateOnly? RequestedEffectiveDate,
    DateOnly? RequestedCompletionDate, string? InspectionPreference, string? InspectionLocation, Guid? VehicleId,
    int? SubjectYear, string? SubjectMake, string? SubjectModel, string? SubjectVin, string? SubjectDescription,
    string? ClientComments, string? RequestSource, bool RequesterAcknowledgedVerification, long RowVersion);

public sealed record AppraisalRequestResponse(Guid AppraisalRequestId, string RequestNumber, Guid ClientId, Guid? ClientContactId,
    Guid? VehicleId, Guid? ConvertedAppraisalId, string RequestStatus, string? RequestSource, string RequestedAppraisalType,
    string RequestedPurpose, string? RequestedIntendedUse, string? RequestedValueType, DateOnly? RequestedEffectiveDate,
    DateOnly? RequestedCompletionDate, string? InspectionPreference, string? InspectionLocation, int? SubjectYear,
    string? SubjectMake, string? SubjectModel, string? SubjectVin, string? SubjectDescription, string? ClientComments,
    string QualificationStatus, Guid? AssignedToUserId, DateTime ReceivedAtUtc, DateTime? ConvertedAtUtc, long RowVersion);

public sealed record InternalAppraisalRequestResponse(AppraisalRequestResponse Request, string? InternalNotes,
    string? QualificationNotes, Guid? QualifiedByUserId, DateTime? ReviewedAtUtc, DateTime? QualifiedAtUtc, DateTime? ClosedAtUtc);

public sealed record AppraisalRequestSummary(Guid AppraisalRequestId, string RequestNumber, Guid ClientId, string RequestStatus,
    string QualificationStatus, Guid? AssignedToUserId, DateTime ReceivedAtUtc, DateOnly? RequestedCompletionDate,
    int? SubjectYear, string? SubjectMake, string? SubjectModel, string? SubjectVin, string RequestedAppraisalType, string RequestedPurpose, long RowVersion);

public sealed record AppraisalRequestSearchCriteria(string? Status = null, Guid? ClientId = null, Guid? AssignedToUserId = null,
    DateTime? ReceivedFrom = null, DateTime? ReceivedTo = null, string? RequestNumber = null, string? Vin = null,
    string? Make = null, string? Model = null, int Page = 1, int PageSize = 50);

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, long TotalCount);
public sealed record RowVersionRequest(long RowVersion);
public sealed record ReasonRequest(string Reason, long RowVersion);
public sealed record QualifyRequest(string? Notes, long RowVersion);
public sealed record NeedsInformationRequest(string? Notes, long RowVersion);
public sealed record AssignRequest(Guid? AssignedToUserId, long RowVersion);
public sealed record ConvertAppraisalRequestCommand(long RowVersion, Guid? VehicleIdOverride = null, Guid? AssignedAppraiserId = null,
    string? ProposedAppraisalType = null, string? ProposedIntendedUse = null, string? ProposedValueType = null,
    DateOnly? ProposedEffectiveDate = null, string? ProposedInspectionScope = null);
public sealed record ConvertAppraisalRequestResult(Guid AppraisalRequestId, string RequestNumber, string RequestStatus,
    Guid AppraisalId, string AppraisalNumber);

public static class AppraisalRequestMappings
{
    public static AppraisalRequestResponse ToResponse(this AppraisalRequest x) => new(x.AppraisalRequestId, x.RequestNumber,
        x.ClientId, x.ClientContactId, x.VehicleId, x.ConvertedAppraisalId, x.RequestStatus.ToString(), x.RequestSource,
        x.RequestedAppraisalType, x.RequestedPurpose, x.RequestedIntendedUse, x.RequestedValueType, x.RequestedEffectiveDate,
        x.RequestedCompletionDate, x.InspectionPreference, x.InspectionLocation, x.SubjectYear, x.SubjectMake, x.SubjectModel,
        x.SubjectVin, x.SubjectDescription, x.ClientComments, x.QualificationStatus.ToString(), x.AssignedToUserId,
        x.ReceivedAtUtc, x.ConvertedAtUtc, x.RowVersion);
}
