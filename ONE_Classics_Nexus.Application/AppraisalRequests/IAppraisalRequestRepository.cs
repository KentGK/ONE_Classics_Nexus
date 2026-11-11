using ONE_Classics_Nexus.Domain.AppraisalRequests;
using System;

namespace ONE_Classics_Nexus.Application.AppraisalRequests;

public interface IAppraisalRequestRepository
{
    Task<AppraisalRequest?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<AppraisalRequest?> GetByRequestNumberAsync(string requestNumber, CancellationToken ct);
    Task<PagedResult<AppraisalRequestSummary>> SearchAsync(AppraisalRequestSearchCriteria criteria, CancellationToken ct);
    Task InsertAsync(AppraisalRequest request, CancellationToken ct);
    Task<bool> UpdateAsync(AppraisalRequest request, long expectedRowVersion, CancellationToken ct);
}

public interface IAppraisalRequestNumberGenerator { Task<string> NextAsync(DateTime utcNow, CancellationToken ct); }
public interface IAppraisalRequestClock { DateTime UtcNow { get; } }
public interface ICurrentNexusUser { Guid? UserId { get; } }

public sealed record AppraisalSeed(Guid AppraisalRequestId, Guid ClientId, Guid? VehicleId, Guid? AssignedAppraiserId,
    string ProposedAppraisalType, string? ProposedIntendedUse, string? ProposedValueType, DateOnly? ProposedEffectiveDate,
    string? ProposedInspectionScope, string? IntakeHistory, string? InternalWorkfileContext);

public sealed record CreatedAppraisal(Guid AppraisalId, string AppraisalNumber);

public interface IAppraisalRequestConversionGateway
{
    Task<CreatedAppraisal> CreateAppraisalFromRequestAsync(AppraisalSeed seed, CancellationToken ct);

    /// <summary>
    /// Compensating action that removes a provisional appraisal created during a conversion
    /// that could not be completed. Must be safe to call when the appraisal no longer exists.
    /// </summary>
    Task DeleteProvisionalAppraisalAsync(Guid appraisalId, CancellationToken ct);
}

public interface IAppraisalRequestAuditWriter
{
    Task WriteAsync(Guid requestId, Guid? actorUserId, string action, string? priorStatus, string newStatus,
        string? reason, Guid? appraisalId, CancellationToken ct);
}
