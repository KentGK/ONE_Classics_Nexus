using System;

namespace ONE_Classics_Nexus.Domain.AppraisalRequests;

public sealed class AppraisalRequest
{
    private AppraisalRequest() { }

    public Guid AppraisalRequestId { get; private set; }
    public string RequestNumber { get; private set; } = string.Empty;
    public Guid ClientId { get; private set; }
    public Guid? ClientContactId { get; private set; }
    public Guid? VehicleId { get; private set; }
    public Guid? ConvertedAppraisalId { get; private set; }
    public AppraisalRequestStatus RequestStatus { get; private set; }
    public string? RequestSource { get; private set; }
    public string RequestedAppraisalType { get; private set; } = string.Empty;
    public string RequestedPurpose { get; private set; } = string.Empty;
    public string? RequestedIntendedUse { get; private set; }
    public string? RequestedValueType { get; private set; }
    public DateOnly? RequestedEffectiveDate { get; private set; }
    public DateOnly? RequestedCompletionDate { get; private set; }
    public string? InspectionPreference { get; private set; }
    public string? InspectionLocation { get; private set; }
    public Guid? VehicleIdSeed => VehicleId;
    public int? SubjectYear { get; private set; }
    public string? SubjectMake { get; private set; }
    public string? SubjectModel { get; private set; }
    public string? SubjectVin { get; private set; }
    public string? SubjectDescription { get; private set; }
    public string? ClientComments { get; private set; }
    public string? InternalNotes { get; private set; }
    public bool RequesterAcknowledgedVerification { get; private set; }
    public AppraisalRequestQualificationStatus QualificationStatus { get; private set; }
    public string? QualificationNotes { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public Guid? QualifiedByUserId { get; private set; }
    public DateTime ReceivedAtUtc { get; private set; }
    public DateTime? ReviewedAtUtc { get; private set; }
    public DateTime? QualifiedAtUtc { get; private set; }
    public DateTime? ConvertedAtUtc { get; private set; }
    public DateTime? ClosedAtUtc { get; private set; }
    public long RowVersion { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    public static AppraisalRequest Create(Guid id, string requestNumber, Guid clientId,
        Guid? clientContactId, string requestedAppraisalType, string requestedPurpose,
        string? requestedIntendedUse, string? requestedValueType, DateOnly? requestedEffectiveDate,
        DateOnly? requestedCompletionDate, string? inspectionPreference, string? inspectionLocation,
        Guid? vehicleId, int? subjectYear, string? subjectMake, string? subjectModel, string? subjectVin,
        string? subjectDescription, string? clientComments, string? requestSource,
        bool requesterAcknowledgedVerification, DateTime utcNow)
    {
        if (id == Guid.Empty) throw Error("APPRAISAL_REQUEST_INVALID_ID", "AppraisalRequestId is required.");
        if (clientId == Guid.Empty) throw Error("APPRAISAL_REQUEST_CLIENT_REQUIRED", "ClientId is required.");
        if (string.IsNullOrWhiteSpace(requestNumber)) throw Error("APPRAISAL_REQUEST_NUMBER_REQUIRED", "RequestNumber is required.");

        return new AppraisalRequest
        {
            AppraisalRequestId = id,
            RequestNumber = requestNumber.Trim(),
            ClientId = clientId,
            ClientContactId = clientContactId,
            RequestedAppraisalType = CleanRequired(requestedAppraisalType, nameof(requestedAppraisalType)),
            RequestedPurpose = CleanRequired(requestedPurpose, nameof(requestedPurpose)),
            RequestedIntendedUse = Clean(requestedIntendedUse),
            RequestedValueType = Clean(requestedValueType),
            RequestedEffectiveDate = requestedEffectiveDate,
            RequestedCompletionDate = requestedCompletionDate,
            InspectionPreference = Clean(inspectionPreference),
            InspectionLocation = Clean(inspectionLocation),
            VehicleId = vehicleId,
            SubjectYear = subjectYear,
            SubjectMake = Clean(subjectMake),
            SubjectModel = Clean(subjectModel),
            SubjectVin = Clean(subjectVin),
            SubjectDescription = Clean(subjectDescription),
            ClientComments = Clean(clientComments),
            RequestSource = Clean(requestSource),
            RequesterAcknowledgedVerification = requesterAcknowledgedVerification,
            RequestStatus = AppraisalRequestStatus.Draft,
            QualificationStatus = AppraisalRequestQualificationStatus.NotReviewed,
            ReceivedAtUtc = utcNow,
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow,
            RowVersion = 1
        };
    }

    public void UpdateIntake(Guid clientId, Guid? clientContactId, string requestedAppraisalType, string requestedPurpose,
        string? requestedIntendedUse, string? requestedValueType, DateOnly? requestedEffectiveDate,
        DateOnly? requestedCompletionDate, string? inspectionPreference, string? inspectionLocation, Guid? vehicleId,
        int? subjectYear, string? subjectMake, string? subjectModel, string? subjectVin, string? subjectDescription,
        string? clientComments, string? requestSource, bool requesterAcknowledgedVerification, DateTime utcNow)
    {
        EnsureMutable();
        if (clientId == Guid.Empty) throw Error("APPRAISAL_REQUEST_CLIENT_REQUIRED", "ClientId is required.");
        ClientId = clientId; ClientContactId = clientContactId; RequestedAppraisalType = CleanRequired(requestedAppraisalType, nameof(requestedAppraisalType));
        RequestedPurpose = CleanRequired(requestedPurpose, nameof(requestedPurpose)); RequestedIntendedUse = Clean(requestedIntendedUse);
        RequestedValueType = Clean(requestedValueType); RequestedEffectiveDate = requestedEffectiveDate; RequestedCompletionDate = requestedCompletionDate;
        InspectionPreference = Clean(inspectionPreference); InspectionLocation = Clean(inspectionLocation); VehicleId = vehicleId; SubjectYear = subjectYear;
        SubjectMake = Clean(subjectMake); SubjectModel = Clean(subjectModel); SubjectVin = Clean(subjectVin); SubjectDescription = Clean(subjectDescription);
        ClientComments = Clean(clientComments); RequestSource = Clean(requestSource); RequesterAcknowledgedVerification = requesterAcknowledgedVerification;
        Touch(utcNow);
    }

    public void SetInternalNotes(string? notes, DateTime utcNow) { InternalNotes = Clean(notes); Touch(utcNow); }
    public void Assign(Guid? userId, DateTime utcNow) { EnsureNonTerminal(); AssignedToUserId = userId; Touch(utcNow); }

    public void Submit(DateTime utcNow)
    {
        RequireStatus(AppraisalRequestStatus.Draft);
        if (ClientId == Guid.Empty) throw Error("APPRAISAL_REQUEST_CLIENT_REQUIRED", "A client is required before submission.");
        if (string.IsNullOrWhiteSpace(RequestedAppraisalType)) throw Error("APPRAISAL_REQUEST_INCOMPLETE", "Requested appraisal type is required.");
        if (string.IsNullOrWhiteSpace(RequestedPurpose)) throw Error("APPRAISAL_REQUEST_INCOMPLETE", "Requested purpose is required.");
        if (!HasSufficientSubjectIdentity()) throw Error("APPRAISAL_REQUEST_SUBJECT_REQUIRED", "Sufficient preliminary subject identification is required.");
        if (!RequesterAcknowledgedVerification) throw Error("APPRAISAL_REQUEST_INCOMPLETE", "Requester verification acknowledgement is required.");
        RequestStatus = AppraisalRequestStatus.Submitted; Touch(utcNow);
    }

    public void BeginReview(DateTime utcNow)
    {
        if (RequestStatus is not (AppraisalRequestStatus.Submitted or AppraisalRequestStatus.NeedsInformation))
            throw InvalidState("begin review");
        RequestStatus = AppraisalRequestStatus.UnderReview; ReviewedAtUtc ??= utcNow; Touch(utcNow);
    }

    public void RequestAdditionalInformation(string? notes, DateTime utcNow)
    {
        RequireStatus(AppraisalRequestStatus.UnderReview); RequestStatus = AppraisalRequestStatus.NeedsInformation;
        QualificationStatus = AppraisalRequestQualificationStatus.PendingInformation; QualificationNotes = Clean(notes); Touch(utcNow);
    }

    public void MarkQualified(Guid qualifiedByUserId, string? notes, DateTime utcNow)
    {
        RequireStatus(AppraisalRequestStatus.UnderReview);
        if (qualifiedByUserId == Guid.Empty) throw Error("APPRAISAL_REQUEST_QUALIFIER_REQUIRED", "QualifiedByUserId is required.");
        RequestStatus = AppraisalRequestStatus.Qualified; QualificationStatus = AppraisalRequestQualificationStatus.Qualified;
        QualificationNotes = Clean(notes); QualifiedByUserId = qualifiedByUserId; QualifiedAtUtc = utcNow; Touch(utcNow);
    }

    public void Accept(DateTime utcNow) { RequireStatus(AppraisalRequestStatus.Qualified); RequestStatus = AppraisalRequestStatus.Accepted; Touch(utcNow); }
    public void Decline(string reason, DateTime utcNow) { RequireStatus(AppraisalRequestStatus.UnderReview); RequireReason(reason); RequestStatus = AppraisalRequestStatus.Declined; QualificationStatus = AppraisalRequestQualificationStatus.NotQualified; QualificationNotes = reason.Trim(); ClosedAtUtc = utcNow; Touch(utcNow); }
    public void Withdraw(string reason, DateTime utcNow) { EnsureNonTerminal(); RequireReason(reason); RequestStatus = AppraisalRequestStatus.Withdrawn; QualificationNotes = reason.Trim(); ClosedAtUtc = utcNow; Touch(utcNow); }
    public void Cancel(string reason, DateTime utcNow)
    {
        if (RequestStatus is not (AppraisalRequestStatus.Draft or AppraisalRequestStatus.Submitted or AppraisalRequestStatus.UnderReview or AppraisalRequestStatus.Qualified or AppraisalRequestStatus.Accepted)) throw InvalidState("cancel");
        RequireReason(reason); RequestStatus = AppraisalRequestStatus.Cancelled; QualificationNotes = reason.Trim(); ClosedAtUtc = utcNow; Touch(utcNow);
    }

    public void Convert(Guid appraisalId, DateTime utcNow)
    {
        if (ConvertedAppraisalId.HasValue || RequestStatus == AppraisalRequestStatus.Converted) throw Error("APPRAISAL_REQUEST_ALREADY_CONVERTED", "The request has already been converted.");
        if (RequestStatus is not (AppraisalRequestStatus.Qualified or AppraisalRequestStatus.Accepted)) throw Error("APPRAISAL_REQUEST_NOT_QUALIFIED", "The request must be Qualified or Accepted before conversion.");
        if (appraisalId == Guid.Empty) throw Error("APPRAISAL_REQUEST_CONVERSION_FAILED", "A resulting appraisal id is required.");
        ConvertedAppraisalId = appraisalId; ConvertedAtUtc = utcNow; RequestStatus = AppraisalRequestStatus.Converted; Touch(utcNow);
    }

    public void Close(DateTime utcNow)
    {
        if (RequestStatus != AppraisalRequestStatus.Converted) throw InvalidState("close");
        RequestStatus = AppraisalRequestStatus.Closed; ClosedAtUtc = utcNow; Touch(utcNow);
    }

    public bool CanConvert => (RequestStatus is AppraisalRequestStatus.Qualified or AppraisalRequestStatus.Accepted) && !ConvertedAppraisalId.HasValue;
    public bool HasSufficientSubjectIdentity() => VehicleId.HasValue || !string.IsNullOrWhiteSpace(SubjectVin) ||
        (SubjectYear.HasValue && !string.IsNullOrWhiteSpace(SubjectMake) && !string.IsNullOrWhiteSpace(SubjectModel)) ||
        !string.IsNullOrWhiteSpace(SubjectDescription);

    private void EnsureMutable() { if (RequestStatus is AppraisalRequestStatus.Converted or AppraisalRequestStatus.Closed) throw Error("APPRAISAL_REQUEST_INVALID_STATE", "Converted request facts are historically preserved and cannot be edited."); }
    private void EnsureNonTerminal() { if (RequestStatus is AppraisalRequestStatus.Converted or AppraisalRequestStatus.Closed or AppraisalRequestStatus.Declined or AppraisalRequestStatus.Cancelled or AppraisalRequestStatus.Withdrawn) throw InvalidState("change terminal request"); }
    private void RequireStatus(AppraisalRequestStatus s) { if (RequestStatus != s) throw InvalidState($"transition from {RequestStatus}"); }
    private AppraisalRequestDomainException InvalidState(string action) => Error("APPRAISAL_REQUEST_INVALID_STATE", $"Cannot {action} while request is {RequestStatus}.");
    private static void RequireReason(string reason) { if (string.IsNullOrWhiteSpace(reason)) throw Error("APPRAISAL_REQUEST_REASON_REQUIRED", "A reason is required."); }
    private static string CleanRequired(string value, string name) => string.IsNullOrWhiteSpace(value) ? throw Error("APPRAISAL_REQUEST_INCOMPLETE", $"{name} is required.") : value.Trim();
    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    private void Touch(DateTime utcNow) { UpdatedAtUtc = utcNow; RowVersion++; }
    private static InvalidOperationException Error(string code, string message) => new AppraisalRequestDomainException(code, message);

    public static AppraisalRequest Rehydrate(AppraisalRequestState s) => new()
    {
        AppraisalRequestId = s.AppraisalRequestId,
        RequestNumber = s.RequestNumber,
        ClientId = s.ClientId,
        ClientContactId = s.ClientContactId,
        VehicleId = s.VehicleId,
        ConvertedAppraisalId = s.ConvertedAppraisalId,
        RequestStatus = s.RequestStatus,
        RequestSource = s.RequestSource,
        RequestedAppraisalType = s.RequestedAppraisalType,
        RequestedPurpose = s.RequestedPurpose,
        RequestedIntendedUse = s.RequestedIntendedUse,
        RequestedValueType = s.RequestedValueType,
        RequestedEffectiveDate = s.RequestedEffectiveDate,
        RequestedCompletionDate = s.RequestedCompletionDate,
        InspectionPreference = s.InspectionPreference,
        InspectionLocation = s.InspectionLocation,
        SubjectYear = s.SubjectYear,
        SubjectMake = s.SubjectMake,
        SubjectModel = s.SubjectModel,
        SubjectVin = s.SubjectVin,
        SubjectDescription = s.SubjectDescription,
        ClientComments = s.ClientComments,
        InternalNotes = s.InternalNotes,
        RequesterAcknowledgedVerification = s.RequesterAcknowledgedVerification,
        QualificationStatus = s.QualificationStatus,
        QualificationNotes = s.QualificationNotes,
        AssignedToUserId = s.AssignedToUserId,
        QualifiedByUserId = s.QualifiedByUserId,
        ReceivedAtUtc = s.ReceivedAtUtc,
        ReviewedAtUtc = s.ReviewedAtUtc,
        QualifiedAtUtc = s.QualifiedAtUtc,
        ConvertedAtUtc = s.ConvertedAtUtc,
        ClosedAtUtc = s.ClosedAtUtc,
        RowVersion = s.RowVersion,
        CreatedAtUtc = s.CreatedAtUtc,
        UpdatedAtUtc = s.UpdatedAtUtc
    };
}

public sealed record AppraisalRequestState(
    Guid AppraisalRequestId, 
    string RequestNumber, 
    Guid ClientId, 
    Guid? ClientContactId,
    Guid? VehicleId, 
    Guid? ConvertedAppraisalId, 
    AppraisalRequestStatus RequestStatus, 
    string? RequestSource,
    string RequestedAppraisalType, 
    string RequestedPurpose, 
    string? RequestedIntendedUse, 
    string? RequestedValueType,
    DateOnly? RequestedEffectiveDate, 
    DateOnly? RequestedCompletionDate, 
    string? InspectionPreference, 
    string? InspectionLocation,
    int? SubjectYear, 
    string? SubjectMake, 
    string? SubjectModel, 
    string? SubjectVin, 
    string? SubjectDescription, 
    string? ClientComments,
    string? InternalNotes, 
    bool RequesterAcknowledgedVerification, 
    AppraisalRequestQualificationStatus QualificationStatus,
    string? QualificationNotes, 
    Guid? AssignedToUserId, 
    Guid? QualifiedByUserId, 
    DateTime ReceivedAtUtc, 
    DateTime? ReviewedAtUtc,
    DateTime? QualifiedAtUtc, 
    DateTime? ConvertedAtUtc, 
    DateTime? ClosedAtUtc, long RowVersion, 
    DateTime CreatedAtUtc, 
    DateTime UpdatedAtUtc
    );
