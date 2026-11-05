namespace ONE_Classics_Nexus.Application.AppraisalRequests;

public sealed class AppraisalRequestPartyService(
    IAppraisalRequestPartyRepository repository,
    IPartyReferenceDataRepository referenceData,
    IAppraisalRequestClock clock) : IAppraisalRequestPartyService
{
    public Task<PartyIdentificationResponse?> GetAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
    {
        if (appraisalRequestId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("CIU-001", "Client is required.", 422);
        }

        return repository.GetAsync(appraisalRequestId, cancellationToken);
    }

    public async Task<PartyIdentificationResponse> SetClientAsync(
        Guid appraisalRequestId,
        SetAppraisalRequestClientRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        if (request.ClientId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("CIU-001", "Client is required.", 422);
        }

        if (!await referenceData.ClientIsActiveAsync(request.ClientId, cancellationToken).ConfigureAwait(false))
        {
            throw new AppraisalRequestApplicationException("CIU-010", "Selected client is inactive or unavailable.", 422);
        }

        var saved = await repository
            .SetClientAsync(appraisalRequestId, request.ClientId, request.ExpectedRowVersion, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        return await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse> SetPrimaryClientContactAsync(
        Guid appraisalRequestId,
        SetPrimaryClientContactRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);

        if (request.ClientContactId.HasValue)
        {
            var contactId = request.ClientContactId.Value;

            if (!await referenceData.ContactBelongsToClientAsync(workspace.ClientId, contactId, cancellationToken).ConfigureAwait(false))
            {
                throw new AppraisalRequestApplicationException("CIU-002", "Selected client contact does not belong to the request client.", 422);
            }

            if (!await referenceData.ContactIsActiveAsync(contactId, cancellationToken).ConfigureAwait(false))
            {
                throw new AppraisalRequestApplicationException("CIU-010", "Selected client contact is inactive.", 422);
            }
        }

        var saved = await repository
            .SetPrimaryClientContactAsync(appraisalRequestId, request.ClientContactId, request.ExpectedRowVersion, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        return await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse> SetProposedIntendedUseAsync(
        Guid appraisalRequestId,
        SetIntendedUseRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        if (request.IntendedUseId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("CIU-004", "Proposed intended use is required before acceptance or conversion.", 422);
        }

        if (!await referenceData.IntendedUseIsSelectableAsync(request.IntendedUseId, clock.UtcNow, cancellationToken).ConfigureAwait(false))
        {
            throw new AppraisalRequestApplicationException("CIU-010", "Selected intended-use definition is inactive or not effective.", 422);
        }

        var saved = await repository
            .SetProposedIntendedUseAsync(appraisalRequestId, request.IntendedUseId, Clean(request.CustomNarrative), request.ExpectedRowVersion, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        return EnsureCustomNarrative(workspace);
    }

    public async Task<ProposedIntendedUserSnapshot> AddProposedIntendedUserAsync(
        Guid appraisalRequestId,
        CreateIntendedUserRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        await EnsureIntendedUserRequestAsync(appraisalRequestId, request, cancellationToken).ConfigureAwait(false);

        var createdId = await repository
            .AddProposedIntendedUserAsync(appraisalRequestId, request with { DisplayName = request.DisplayName.Trim(), RelationshipToClient = request.RelationshipToClient.Trim() }, actorId, cancellationToken)
            .ConfigureAwait(false);

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        var created = workspace.ProposedIntendedUsers.FirstOrDefault(x => x.IntendedUserId == createdId);

        return created ?? throw new AppraisalRequestApplicationException("CIU-008", "Unable to load the created proposed intended user.", 500);
    }

    public async Task<ProposedIntendedUserSnapshot> UpdateProposedIntendedUserAsync(
        Guid appraisalRequestId,
        Guid intendedUserId,
        UpdateIntendedUserRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        if (intendedUserId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("CIU-008", "Proposed intended user id is required.", 422);
        }

        await EnsureIntendedUserRequestAsync(appraisalRequestId, request, cancellationToken).ConfigureAwait(false);

        var saved = await repository
            .UpdateProposedIntendedUserAsync(appraisalRequestId, intendedUserId, request with { DisplayName = request.DisplayName.Trim(), RelationshipToClient = request.RelationshipToClient.Trim() }, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        var updated = workspace.ProposedIntendedUsers.FirstOrDefault(x => x.IntendedUserId == intendedUserId);

        return updated ?? throw new AppraisalRequestApplicationException("CIU-008", "Proposed intended user was not found.", 404);
    }

    public async Task<PartyIdentificationResponse> RemoveProposedIntendedUserAsync(
        Guid appraisalRequestId,
        Guid intendedUserId,
        RemoveIntendedUserRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        var activeCount = workspace.ProposedIntendedUsers.Count(x => x.IsActive);

        if (activeCount <= 1 && !string.Equals(workspace.RequestStatus, "Draft", StringComparison.OrdinalIgnoreCase))
        {
            throw new AppraisalRequestApplicationException("CIU-005", "At least one proposed intended user is required before acceptance or conversion.", 422);
        }

        var saved = await repository
            .DeactivateProposedIntendedUserAsync(appraisalRequestId, intendedUserId, request.ExpectedRowVersion, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        return await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse> SaveClientExclusionExplanationAsync(
        Guid appraisalRequestId,
        SaveClientExclusionRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        if (string.IsNullOrWhiteSpace(request.Explanation))
        {
            throw new AppraisalRequestApplicationException("CIU-009", "Relationship or explanation is required.", 422);
        }

        var saved = await repository
            .SaveClientExclusionExplanationAsync(appraisalRequestId, request.Explanation.Trim(), request.ExpectedRowVersion, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        return await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ValidationSummary> ValidateAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
    {
        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        return ValidateWorkspace(workspace);
    }

    public async Task<PartyIdentificationResponse> ConvertAsync(
        Guid appraisalRequestId,
        ConvertPartyIdentificationRequest request,
        Guid actorId,
        CancellationToken cancellationToken)
    {
        ValidateActor(actorId);

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        var validation = ValidateWorkspace(workspace);

        if (!validation.IsValid)
        {
            var first = validation.Issues.First(x => string.Equals(x.Severity, "error", StringComparison.OrdinalIgnoreCase));
            throw new AppraisalRequestApplicationException(first.Code, first.Message, 422);
        }

        var saved = await repository
            .MarkConvertedAsync(appraisalRequestId, request.ExpectedRowVersion, actorId, cancellationToken)
            .ConfigureAwait(false);

        if (!saved)
        {
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        return await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
    }

    private async Task EnsureIntendedUserRequestAsync(Guid appraisalRequestId, CreateIntendedUserRequest request, CancellationToken cancellationToken)
    {
        if (request.UserTypeId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("CIU-008", "Proposed intended-user type is required.", 422);
        }

        if (!await referenceData.IntendedUserTypeIsActiveAsync(request.UserTypeId, cancellationToken).ConfigureAwait(false))
        {
            throw new AppraisalRequestApplicationException("CIU-010", "Selected intended-user type is inactive.", 422);
        }

        if (string.IsNullOrWhiteSpace(request.DisplayName) && string.IsNullOrWhiteSpace(request.OrganizationName))
        {
            throw new AppraisalRequestApplicationException("CIU-008", "Proposed intended-user name or organization is required.", 422);
        }

        if (string.IsNullOrWhiteSpace(request.RelationshipToClient))
        {
            throw new AppraisalRequestApplicationException("CIU-009", "Relationship to client is required.", 422);
        }

        if (!request.IsClient)
        {
            return;
        }

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        if (workspace.ProposedIntendedUsers.Any(x => x.IsActive && x.IsClient))
        {
            throw new AppraisalRequestApplicationException("CIU-006", "Only one proposed intended user may represent the client.", 422);
        }
    }

    private async Task EnsureIntendedUserRequestAsync(Guid appraisalRequestId, UpdateIntendedUserRequest request, CancellationToken cancellationToken)
    {
        await EnsureIntendedUserTypeAndIdentityAsync(request.UserTypeId, request.DisplayName, request.OrganizationName, request.RelationshipToClient, cancellationToken).ConfigureAwait(false);

        if (!request.IsClient)
        {
            return;
        }

        var workspace = await GetRequiredAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        var otherClientCount = workspace.ProposedIntendedUsers.Count(x => x.IsActive && x.IsClient && x.IntendedUserId != Guid.Empty);

        if (otherClientCount > 1)
        {
            throw new AppraisalRequestApplicationException("CIU-006", "Only one proposed intended user may represent the client.", 422);
        }
    }

    private async Task EnsureIntendedUserTypeAndIdentityAsync(Guid userTypeId, string displayName, string? organizationName, string relationshipToClient, CancellationToken cancellationToken)
    {
        if (userTypeId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("CIU-008", "Proposed intended-user type is required.", 422);
        }

        if (!await referenceData.IntendedUserTypeIsActiveAsync(userTypeId, cancellationToken).ConfigureAwait(false))
        {
            throw new AppraisalRequestApplicationException("CIU-010", "Selected intended-user type is inactive.", 422);
        }

        if (string.IsNullOrWhiteSpace(displayName) && string.IsNullOrWhiteSpace(organizationName))
        {
            throw new AppraisalRequestApplicationException("CIU-008", "Proposed intended-user name or organization is required.", 422);
        }

        if (string.IsNullOrWhiteSpace(relationshipToClient))
        {
            throw new AppraisalRequestApplicationException("CIU-009", "Relationship to client is required.", 422);
        }
    }

    private static PartyIdentificationResponse EnsureCustomNarrative(PartyIdentificationResponse workspace)
    {
        if (workspace.ProposedIntendedUse?.RequiresCustomNarrative == true && string.IsNullOrWhiteSpace(workspace.ProposedIntendedUse.CustomNarrative))
        {
            throw new AppraisalRequestApplicationException("CIU-007", "Custom proposed-use narrative is required.", 422);
        }

        return workspace;
    }

    private static ValidationSummary ValidateWorkspace(PartyIdentificationResponse workspace)
    {
        var issues = new List<ValidationIssue>();

        if (workspace.ClientId == Guid.Empty || workspace.Client is null)
        {
            issues.Add(new ValidationIssue("CIU-001", "error", "client", "Client is required."));
        }

        if (workspace.PrimaryClientContactId.HasValue && workspace.PrimaryClientContact is null)
        {
            issues.Add(new ValidationIssue("CIU-002", "error", "primaryClientContact", "Selected client contact does not belong to the request client or is inactive."));
        }

        if (workspace.ProposedIntendedUse is null)
        {
            issues.Add(new ValidationIssue("CIU-004", "error", "proposedIntendedUse", "Proposed intended use is required before acceptance or conversion."));
        }

        if (workspace.ProposedIntendedUse?.RequiresCustomNarrative == true && string.IsNullOrWhiteSpace(workspace.ProposedIntendedUse.CustomNarrative))
        {
            issues.Add(new ValidationIssue("CIU-007", "error", "proposedIntendedUse.customNarrative", "Custom proposed-use narrative is required."));
        }

        var activeUsers = workspace.ProposedIntendedUsers.Where(x => x.IsActive).ToList();

        if (activeUsers.Count == 0)
        {
            issues.Add(new ValidationIssue("CIU-005", "error", "proposedIntendedUsers", "At least one proposed intended user is required before acceptance or conversion."));
        }

        if (activeUsers.Count(x => x.IsClient) > 1)
        {
            issues.Add(new ValidationIssue("CIU-006", "error", "proposedIntendedUsers", "Only one proposed intended user may represent the client."));
        }

        foreach (var user in activeUsers)
        {
            if (user.UserTypeId == Guid.Empty || (string.IsNullOrWhiteSpace(user.DisplayName) && string.IsNullOrWhiteSpace(user.OrganizationName)))
            {
                issues.Add(new ValidationIssue("CIU-008", "error", "proposedIntendedUsers", "Proposed intended-user name/organization and type are required."));
                break;
            }

            if (string.IsNullOrWhiteSpace(user.RelationshipToClient))
            {
                issues.Add(new ValidationIssue("CIU-009", "error", "proposedIntendedUsers.relationshipToClient", "Relationship to client is required."));
                break;
            }
        }

        if (activeUsers.Count > 0 && activeUsers.All(x => !x.IsClient))
        {
            issues.Add(new ValidationIssue("CIU-014", "warning", "proposedIntendedUsers", "Client is not identified as a proposed intended user; review is required."));
        }

        var duplicates = activeUsers
            .GroupBy(x => $"{x.DisplayName.Trim().ToUpperInvariant()}|{(x.OrganizationName ?? string.Empty).Trim().ToUpperInvariant()}")
            .Any(g => g.Count() > 1);

        if (duplicates)
        {
            issues.Add(new ValidationIssue("CIU-013", "warning", "proposedIntendedUsers", "Possible duplicate proposed intended user."));
        }

        var blockingCount = issues.Count(x => string.Equals(x.Severity, "error", StringComparison.OrdinalIgnoreCase));
        var warningCount = issues.Count - blockingCount;

        return new ValidationSummary(blockingCount == 0, blockingCount, warningCount, issues);
    }

    private async Task<PartyIdentificationResponse> GetRequiredAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
    {
        if (appraisalRequestId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_NOT_FOUND", "Appraisal request was not found.", 404);
        }

        var workspace = await repository.GetAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
        if (workspace is null)
        {
            throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_NOT_FOUND", "Appraisal request was not found.", 404);
        }

        return workspace with { Validation = ValidateWorkspace(workspace) };
    }

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static void ValidateActor(Guid actorId)
    {
        if (actorId == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_FORBIDDEN", "Authenticated actor id is required.", 403);
        }
    }
}
