using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Appraisals;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;

/// <summary>Adapter to the existing Appraisal creation use case. Proposed intake values stay provisional.</summary>
public sealed class ExistingAppraisalConversionGateway : IAppraisalRequestConversionGateway
{
    private readonly CreateAppraisalHandler _createAppraisal;
    private readonly IAppraisalRepository _appraisals;
    private readonly IAppraisalSubjectSnapshotRepository _snapshots;

    public ExistingAppraisalConversionGateway(
        CreateAppraisalHandler createAppraisal,
        IAppraisalRepository appraisals,
        IAppraisalSubjectSnapshotRepository snapshots)
    {
        ArgumentNullException.ThrowIfNull(createAppraisal);
        ArgumentNullException.ThrowIfNull(appraisals);
        ArgumentNullException.ThrowIfNull(snapshots);
        _createAppraisal = createAppraisal;
        _appraisals = appraisals;
        _snapshots = snapshots;
    }

    /// <summary>Creates a new Draft appraisal from an accepted/qualified appraisal request seed.</summary>
    public async Task<CreatedAppraisal> CreateAppraisalFromRequestAsync(AppraisalSeed seed, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(seed);

        if (!seed.VehicleId.HasValue || seed.VehicleId.Value == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException(
                "APPRAISAL_REQUEST_SUBJECT_REQUIRED",
                "A subject vehicle must be identified on the request before it can be converted to an appraisal.",
                422);
        }

        var request = new CreateAppraisalRequest
        {
            VehicleId = seed.VehicleId.Value,
            AppraisalRequestId = seed.AppraisalRequestId,
            ClientId = seed.ClientId,
            AssignedAppraiserId = seed.AssignedAppraiserId,
            AppraisalType = seed.ProposedAppraisalType,
            // Conversion never approves scope, value definition or effective date; the
            // appraiser confirms them inside the appraisal workfile.
            Status = "Draft",
            IntakeStatus = "Open",
            InspectionStatus = "Not Started",
            ValuationStatus = "Not Started",
            ReportStatus = "Not Started",
            EffectiveDateUtc = seed.ProposedEffectiveDate?.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
        };

        var created = await _createAppraisal.HandleAsync(request, ct).ConfigureAwait(false);

        return new CreatedAppraisal(created.AppraisalId, created.AppraisalNumber ?? string.Empty);
    }

    /// <summary>Removes the appraisal and its subject snapshot after a failed conversion.</summary>
    public async Task DeleteProvisionalAppraisalAsync(Guid appraisalId, CancellationToken ct)
    {
        if (appraisalId == Guid.Empty)
        {
            return;
        }

        // Delete the child snapshot first so the appraisal delete cannot be blocked by it.
        await _snapshots.DeleteByAppraisalIdAsync(appraisalId, ct).ConfigureAwait(false);
        await _appraisals.DeleteAsync(appraisalId, ct).ConfigureAwait(false);
    }
}
