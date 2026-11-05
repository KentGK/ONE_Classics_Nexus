using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Services;

public sealed class AppraisalSubjectSnapshotService : IAppraisalSubjectSnapshotService
{
    private readonly IAppraisalRepository _appraisalRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAppraisalSubjectSnapshotRepository _snapshotRepository;

    public AppraisalSubjectSnapshotService(
        IAppraisalRepository appraisalRepository,
        IVehicleRepository vehicleRepository,
        IAppraisalSubjectSnapshotRepository snapshotRepository)
    {
        ArgumentNullException.ThrowIfNull(appraisalRepository);
        ArgumentNullException.ThrowIfNull(vehicleRepository);
        ArgumentNullException.ThrowIfNull(snapshotRepository);

        _appraisalRepository = appraisalRepository;
        _vehicleRepository = vehicleRepository;
        _snapshotRepository = snapshotRepository;
    }

    public async Task<AppraisalSubjectSnapshot> CreateFromVehicleAsync(
        Guid appraisalId,
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        EnsureRequiredGuid(appraisalId, nameof(appraisalId));
        EnsureRequiredGuid(vehicleId, nameof(vehicleId));

        var appraisal = await _appraisalRepository.GetByIdAsync(appraisalId, cancellationToken);
        if (appraisal is null)
        {
            throw new NotFoundException("Appraisal not found.");
        }

        if (appraisal.VehicleId != vehicleId)
        {
            throw new ValidationException("Appraisal vehicle does not match the requested vehicle.");
        }

        var existing = await _snapshotRepository.GetByAppraisalIdAsync(appraisalId, cancellationToken);
        if (existing is not null)
        {
            throw new ConflictException("Subject snapshot already exists for this appraisal.");
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            throw new NotFoundException("Vehicle not found.");
        }

        var snapshot = AppraisalSubjectSnapshot.CreateDraft(appraisalId, vehicle);

        return await _snapshotRepository.InsertAsync(snapshot, cancellationToken);
    }

    public async Task<AppraisalSubjectSnapshot> RefreshDraftFromVehicleAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        EnsureRequiredGuid(appraisalId, nameof(appraisalId));

        var appraisal = await _appraisalRepository.GetByIdAsync(appraisalId, cancellationToken);
        if (appraisal is null)
        {
            throw new NotFoundException("Appraisal not found.");
        }

        var snapshot = await _snapshotRepository.GetByAppraisalIdAsync(appraisalId, cancellationToken);
        if (snapshot is null)
        {
            throw new NotFoundException("Subject snapshot not found.");
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(appraisal.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            throw new NotFoundException("Vehicle not found.");
        }

        snapshot.RefreshDraftFromVehicle(vehicle);

        var updated = await _snapshotRepository.UpdateAsync(snapshot, cancellationToken);
        if (updated is null)
        {
            throw new ConflictException("Subject snapshot update conflict.");
        }

        return updated;
    }

    public async Task ApproveAsync(
        Guid appraisalId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        EnsureRequiredGuid(appraisalId, nameof(appraisalId));
        EnsureRequiredGuid(userId, nameof(userId));

        var snapshot = await _snapshotRepository.GetByAppraisalIdAsync(appraisalId, cancellationToken);
        if (snapshot is null)
        {
            throw new NotFoundException("Subject snapshot not found.");
        }

        snapshot.Approve(userId);

        var updated = await _snapshotRepository.UpdateAsync(snapshot, cancellationToken);
        if (updated is null)
        {
            throw new ConflictException("Subject snapshot update conflict.");
        }
    }

    private static void EnsureRequiredGuid(Guid value, string fieldName)
    {
        if (value == Guid.Empty)
        {
            throw new ValidationException($"{fieldName} is required.");
        }
    }
}
