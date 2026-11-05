namespace ONE_Classics_Nexus.Application.AppraisalRequests;

public sealed record PostalAddress(
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? StateRegion,
    string? PostalCode,
    string? CountryCode);

public sealed record ClientSnapshot(
    Guid ClientId,
    string ClientNumber,
    string ClientTypeCode,
    string DisplayName,
    string? OrganizationName,
    string? Email,
    string? Phone,
    PostalAddress? Address,
    ulong RowVersion);

public sealed record ClientContactSnapshot(
    Guid ClientContactId,
    string ContactName,
    string? Title,
    string? ContactRole,
    string? Email,
    string? Phone,
    PostalAddress? Address,
    ulong RowVersion);

public sealed record IntendedUseSnapshot(
    Guid IntendedUseId,
    Guid UseCategoryId,
    string IntendedUseCode,
    string IntendedUseName,
    string Description,
    string UseCategoryName,
    bool RequiresCustomNarrative,
    string? CustomNarrative,
    ulong RowVersion);

public sealed record ProposedIntendedUserSnapshot(
    Guid IntendedUserId,
    Guid UserTypeId,
    bool IsClient,
    string DisplayName,
    string? OrganizationName,
    string RelationshipToClient,
    PostalAddress? Address,
    string? Email,
    string? Phone,
    string? UseRestrictions,
    int SortOrder,
    bool IsActive,
    ulong RowVersion);

public sealed record ValidationIssue(string Code, string Severity, string Field, string Message);

public sealed record ValidationSummary(
    bool IsValid,
    int BlockingIssueCount,
    int WarningCount,
    IReadOnlyList<ValidationIssue> Issues);

public sealed record PartyIdentificationResponse(
    Guid AppraisalRequestId,
    Guid ClientId,
    ClientSnapshot? Client,
    Guid? PrimaryClientContactId,
    ClientContactSnapshot? PrimaryClientContact,
    Guid? IntendedUseId,
    IntendedUseSnapshot? ProposedIntendedUse,
    IReadOnlyList<ProposedIntendedUserSnapshot> ProposedIntendedUsers,
    DateTimeOffset? ReviewedAt,
    Guid? ReviewedByUserId,
    Guid? ConvertedAppraisalId,
    string RequestStatus,
    ulong AppraisalRequestRowVersion,
    ValidationSummary Validation);

public sealed record SetAppraisalRequestClientRequest(Guid ClientId, ulong ExpectedRowVersion);

public sealed record SetPrimaryClientContactRequest(Guid? ClientContactId, ulong ExpectedRowVersion);

public sealed record SetIntendedUseRequest(Guid IntendedUseId, string? CustomNarrative, ulong ExpectedRowVersion);

public sealed record CreateIntendedUserRequest(
    Guid UserTypeId,
    bool IsClient,
    string DisplayName,
    string? OrganizationName,
    string RelationshipToClient,
    PostalAddress? Address,
    string? Email,
    string? Phone,
    string? UseRestrictions,
    int SortOrder,
    ulong ExpectedRowVersion,
    Guid? SourceClientId,
    Guid? SourceClientContactId);

public sealed record UpdateIntendedUserRequest(
    Guid UserTypeId,
    bool IsClient,
    string DisplayName,
    string? OrganizationName,
    string RelationshipToClient,
    PostalAddress? Address,
    string? Email,
    string? Phone,
    string? UseRestrictions,
    int SortOrder,
    ulong ExpectedRowVersion,
    bool IsActive,
    Guid? SourceClientId,
    Guid? SourceClientContactId);

public sealed record RemoveIntendedUserRequest(ulong ExpectedRowVersion);

public sealed record SaveClientExclusionRequest(string Explanation, ulong ExpectedRowVersion);

public sealed record ConvertPartyIdentificationRequest(ulong ExpectedRowVersion);

public interface IAppraisalRequestPartyRepository
{
    Task<PartyIdentificationResponse?> GetAsync(Guid appraisalRequestId, CancellationToken cancellationToken);
    Task<bool> SetClientAsync(Guid appraisalRequestId, Guid clientId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken);
    Task<bool> SetPrimaryClientContactAsync(Guid appraisalRequestId, Guid? clientContactId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken);
    Task<bool> SetProposedIntendedUseAsync(Guid appraisalRequestId, Guid intendedUseId, string? customNarrative, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken);
    Task<Guid> AddProposedIntendedUserAsync(Guid appraisalRequestId, CreateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<bool> UpdateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, UpdateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<bool> DeactivateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken);
    Task<bool> SaveClientExclusionExplanationAsync(Guid appraisalRequestId, string explanation, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken);
    Task<bool> MarkConvertedAsync(Guid appraisalRequestId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken);
}

public interface IPartyReferenceDataRepository
{
    Task<bool> ClientIsActiveAsync(Guid clientId, CancellationToken cancellationToken);
    Task<bool> ContactBelongsToClientAsync(Guid clientId, Guid clientContactId, CancellationToken cancellationToken);
    Task<bool> ContactIsActiveAsync(Guid clientContactId, CancellationToken cancellationToken);
    Task<bool> IntendedUseIsSelectableAsync(Guid intendedUseId, DateTime utcNow, CancellationToken cancellationToken);
    Task<bool> IntendedUserTypeIsActiveAsync(Guid userTypeId, CancellationToken cancellationToken);
}

public interface IAppraisalRequestPartyService
{
    Task<PartyIdentificationResponse?> GetAsync(Guid appraisalRequestId, CancellationToken cancellationToken);
    Task<PartyIdentificationResponse> SetClientAsync(Guid appraisalRequestId, SetAppraisalRequestClientRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<PartyIdentificationResponse> SetPrimaryClientContactAsync(Guid appraisalRequestId, SetPrimaryClientContactRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<PartyIdentificationResponse> SetProposedIntendedUseAsync(Guid appraisalRequestId, SetIntendedUseRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<ProposedIntendedUserSnapshot> AddProposedIntendedUserAsync(Guid appraisalRequestId, CreateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<ProposedIntendedUserSnapshot> UpdateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, UpdateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<PartyIdentificationResponse> RemoveProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, RemoveIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<PartyIdentificationResponse> SaveClientExclusionExplanationAsync(Guid appraisalRequestId, SaveClientExclusionRequest request, Guid actorId, CancellationToken cancellationToken);
    Task<ValidationSummary> ValidateAsync(Guid appraisalRequestId, CancellationToken cancellationToken);
    Task<PartyIdentificationResponse> ConvertAsync(Guid appraisalRequestId, ConvertPartyIdentificationRequest request, Guid actorId, CancellationToken cancellationToken);
}
