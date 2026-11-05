using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Services;

public interface IAppraisalSubjectSnapshotService
{
    Task<AppraisalSubjectSnapshot> CreateFromVehicleAsync(
        Guid appraisalId,
        Guid vehicleId,
        CancellationToken cancellationToken);

    Task<AppraisalSubjectSnapshot> RefreshDraftFromVehicleAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);

    Task ApproveAsync(
        Guid appraisalId,
        Guid userId,
        CancellationToken cancellationToken);
}
