using ONE_Classics_Nexus.Domain.AppraisalRequests;

namespace ONE_Classics_Nexus.Application.AppraisalRequests;

public interface IAppraisalRequestService
{
    Task<AppraisalRequestResponse> CreateAsync(CreateAppraisalRequestRequest request, CancellationToken ct);
    Task<AppraisalRequestResponse> GetAsync(Guid id, CancellationToken ct);
    Task<PagedResult<AppraisalRequestSummary>> SearchAsync(AppraisalRequestSearchCriteria criteria, CancellationToken ct);
    Task<AppraisalRequestResponse> UpdateAsync(Guid id, UpdateAppraisalRequestRequest request, CancellationToken ct);
    Task<AppraisalRequestResponse> SubmitAsync(Guid id, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> BeginReviewAsync(Guid id, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> NeedsInformationAsync(Guid id, string? notes, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> QualifyAsync(Guid id, string? notes, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> AcceptAsync(Guid id, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> DeclineAsync(Guid id, string reason, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> CancelAsync(Guid id, string reason, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> WithdrawAsync(Guid id, string reason, long rowVersion, CancellationToken ct);
    Task<AppraisalRequestResponse> AssignAsync(Guid id, Guid? userId, long rowVersion, CancellationToken ct);
    Task<ConvertAppraisalRequestResult> ConvertAsync(Guid id, ConvertAppraisalRequestCommand command, CancellationToken ct);
}

public sealed class AppraisalRequestService : IAppraisalRequestService
{
    private readonly IAppraisalRequestRepository _repo; private readonly IAppraisalRequestNumberGenerator _numbers;
    private readonly IAppraisalRequestClock _clock; private readonly ICurrentNexusUser _currentUser;
    private readonly IAppraisalRequestConversionGateway _conversion; private readonly IAppraisalRequestAuditWriter _audit;

    public AppraisalRequestService(IAppraisalRequestRepository repo, IAppraisalRequestNumberGenerator numbers,
        IAppraisalRequestClock clock, ICurrentNexusUser currentUser, IAppraisalRequestConversionGateway conversion,
        IAppraisalRequestAuditWriter audit)
    { _repo = repo; _numbers = numbers; _clock = clock; _currentUser = currentUser; _conversion = conversion; _audit = audit; }

    public async Task<AppraisalRequestResponse> CreateAsync(CreateAppraisalRequestRequest r, CancellationToken ct)
    {
        var now = _clock.UtcNow; var n = await _numbers.NextAsync(now, ct);
        var x = AppraisalRequest.Create(Guid.NewGuid(), n, r.ClientId, r.ClientContactId, r.RequestedAppraisalType, r.RequestedPurpose,
            r.RequestedIntendedUse, r.RequestedValueType, r.RequestedEffectiveDate, r.RequestedCompletionDate, r.InspectionPreference,
            r.InspectionLocation, r.VehicleId, r.SubjectYear, r.SubjectMake, r.SubjectModel, r.SubjectVin, r.SubjectDescription,
            r.ClientComments, r.RequestSource, r.RequesterAcknowledgedVerification, now);
        await _repo.InsertAsync(x, ct); await _audit.WriteAsync(x.AppraisalRequestId, _currentUser.UserId, "AppraisalRequestCreated", null, x.RequestStatus.ToString(), null, null, ct); return x.ToResponse();
    }
    public async Task<AppraisalRequestResponse> GetAsync(Guid id, CancellationToken ct) => (await GetEntity(id, ct)).ToResponse();
    public Task<PagedResult<AppraisalRequestSummary>> SearchAsync(AppraisalRequestSearchCriteria c, CancellationToken ct) => _repo.SearchAsync(c, ct);
    public async Task<AppraisalRequestResponse> UpdateAsync(Guid id, UpdateAppraisalRequestRequest r, CancellationToken ct)
    {
        var x = await GetEntity(id, ct); EnsureVersion(x, r.RowVersion); var prior = x.RequestStatus.ToString();
        x.UpdateIntake(r.ClientId, r.ClientContactId, r.RequestedAppraisalType, r.RequestedPurpose, r.RequestedIntendedUse, r.RequestedValueType,
            r.RequestedEffectiveDate, r.RequestedCompletionDate, r.InspectionPreference, r.InspectionLocation, r.VehicleId, r.SubjectYear,
            r.SubjectMake, r.SubjectModel, r.SubjectVin, r.SubjectDescription, r.ClientComments, r.RequestSource, r.RequesterAcknowledgedVerification, _clock.UtcNow);
        await Save(x, r.RowVersion, ct); await Audit(x, "AppraisalRequestUpdated", prior, null, null, ct); return x.ToResponse();
    }
    public Task<AppraisalRequestResponse> SubmitAsync(Guid id, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestSubmitted", x => x.Submit(_clock.UtcNow), null, ct);
    public Task<AppraisalRequestResponse> BeginReviewAsync(Guid id, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestReviewStarted", x => x.BeginReview(_clock.UtcNow), null, ct);
    public Task<AppraisalRequestResponse> NeedsInformationAsync(Guid id, string? notes, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestInformationRequested", x => x.RequestAdditionalInformation(notes, _clock.UtcNow), notes, ct);
    public Task<AppraisalRequestResponse> QualifyAsync(Guid id, string? notes, long v, CancellationToken ct)
    { var uid = _currentUser.UserId ?? throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_QUALIFIER_REQUIRED", "Authenticated user id is required.", 403); return Mutate(id, v, "AppraisalRequestQualified", x => x.MarkQualified(uid, notes, _clock.UtcNow), notes, ct); }
    public Task<AppraisalRequestResponse> AcceptAsync(Guid id, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestAccepted", x => x.Accept(_clock.UtcNow), null, ct);
    public Task<AppraisalRequestResponse> DeclineAsync(Guid id, string reason, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestDeclined", x => x.Decline(reason, _clock.UtcNow), reason, ct);
    public Task<AppraisalRequestResponse> CancelAsync(Guid id, string reason, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestCancelled", x => x.Cancel(reason, _clock.UtcNow), reason, ct);
    public Task<AppraisalRequestResponse> WithdrawAsync(Guid id, string reason, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestWithdrawn", x => x.Withdraw(reason, _clock.UtcNow), reason, ct);
    public Task<AppraisalRequestResponse> AssignAsync(Guid id, Guid? userId, long v, CancellationToken ct) => Mutate(id, v, "AppraisalRequestAssigned", x => x.Assign(userId, _clock.UtcNow), userId?.ToString(), ct);

    public async Task<ConvertAppraisalRequestResult> ConvertAsync(Guid id, ConvertAppraisalRequestCommand c, CancellationToken ct)
    {
        var x = await GetEntity(id, ct); EnsureVersion(x, c.RowVersion);
        if (!x.CanConvert) throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_NOT_QUALIFIED", "The request must be Qualified or Accepted before conversion.", 422);
        var seed = new AppraisalSeed(x.AppraisalRequestId, x.ClientId, c.VehicleIdOverride ?? x.VehicleId, c.AssignedAppraiserId,
            c.ProposedAppraisalType ?? x.RequestedAppraisalType, c.ProposedIntendedUse, c.ProposedValueType, c.ProposedEffectiveDate,
            c.ProposedInspectionScope, x.ClientComments, x.InternalNotes);
        var created = await _conversion.CreateAppraisalFromRequestAsync(seed, ct);

        // The appraisal lives in a separate store from the request, so the two writes cannot share
        // a transaction. If linking the request fails, roll the provisional appraisal back so a
        // retry does not leave an orphan behind.
        var prior = x.RequestStatus.ToString();
        try
        {
            x.Convert(created.AppraisalId, _clock.UtcNow);
            await Save(x, c.RowVersion, ct);
        }
        catch (Exception ex)
        {
            await CompensateFailedConversionAsync(created.AppraisalId, ex);
            throw;
        }

        // Past this point the request durably references the appraisal, so an audit failure
        // must not delete it; the write is allowed to bubble up instead.
        await Audit(x, "AppraisalRequestConverted", prior, null, created.AppraisalId, ct);
        return new(x.AppraisalRequestId, x.RequestNumber, x.RequestStatus.ToString(), created.AppraisalId, created.AppraisalNumber);
    }

    private async Task CompensateFailedConversionAsync(Guid appraisalId, Exception cause)
    {
        try
        {
            // Cleanup must still run when the caller cancelled, otherwise the orphan survives.
            await _conversion.DeleteProvisionalAppraisalAsync(appraisalId, CancellationToken.None);
        }
        catch (Exception cleanupFailure)
        {
            // Surface both problems: the orphaned appraisal id must stay visible for manual repair.
            throw new AppraisalRequestApplicationException(
                "APPRAISAL_REQUEST_CONVERSION_CLEANUP_FAILED",
                $"Conversion failed and the provisional appraisal {appraisalId} could not be removed. " +
                "It must be reconciled manually.",
                500,
                new AggregateException(cause, cleanupFailure));
        }
    }

    private async Task<AppraisalRequestResponse> Mutate(Guid id, long v, string action, Action<AppraisalRequest> mutation, string? reason, CancellationToken ct)
    { var x = await GetEntity(id, ct); EnsureVersion(x, v); var prior = x.RequestStatus.ToString(); mutation(x); await Save(x, v, ct); await Audit(x, action, prior, reason, null, ct); return x.ToResponse(); }
    private async Task<AppraisalRequest> GetEntity(Guid id, CancellationToken ct) => await _repo.GetByIdAsync(id, ct) ?? throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_NOT_FOUND", "Appraisal request was not found.", 404);
    private static void EnsureVersion(AppraisalRequest x, long v) { if (x.RowVersion != v) throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_CONCURRENCY_CONFLICT", "The appraisal request has changed. Reload and retry.", 409); }
    private async Task Save(AppraisalRequest x, long expected, CancellationToken ct) { if (!await _repo.UpdateAsync(x, expected, ct)) throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_CONCURRENCY_CONFLICT", "The appraisal request has changed. Reload and retry.", 409); }
    private Task Audit(AppraisalRequest x, string action, string prior, string? reason, Guid? appraisalId, CancellationToken ct) => _audit.WriteAsync(x.AppraisalRequestId, _currentUser.UserId, action, prior, x.RequestStatus.ToString(), reason, appraisalId, ct);
}

public sealed class SystemAppraisalRequestClock : IAppraisalRequestClock { public DateTime UtcNow => DateTime.UtcNow; }
