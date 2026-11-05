using FluentAssertions;
using ONE_Classics_Nexus.Application.AppraisalRequests;

namespace ONE_Classics_Nexus.Tests.AppraisalRequests;

public sealed class AppraisalRequestPartyServiceTests
{
    [Fact]
    public async Task ValidateAsync_WhenProposedIntendedUseMissing_ReturnsCiu004()
    {
        var requestId = Guid.NewGuid();
        var repository = new InMemoryPartyRepository(BuildWorkspace(requestId) with { ProposedIntendedUse = null });
        var service = CreateService(repository);

        var result = await service.ValidateAsync(requestId, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Issues.Should().Contain(x => x.Code == "CIU-004");
    }

    [Fact]
    public async Task SetClientAsync_WhenClientIdEmpty_ThrowsCiu001()
    {
        var requestId = Guid.NewGuid();
        var repository = new InMemoryPartyRepository(BuildWorkspace(requestId));
        var service = CreateService(repository);

        var action = () => service.SetClientAsync(
            requestId,
            new SetAppraisalRequestClientRequest(Guid.Empty, 1),
            Guid.NewGuid(),
            CancellationToken.None);

        var error = await Assert.ThrowsAsync<AppraisalRequestApplicationException>(action);
        error.Code.Should().Be("CIU-001");
    }

    [Fact]
    public async Task RemoveProposedIntendedUserAsync_WhenRemovingLastActiveOutsideDraft_ThrowsCiu005()
    {
        var requestId = Guid.NewGuid();
        var singleUser = new ProposedIntendedUserSnapshot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            "John Doe",
            null,
            "Advisor",
            null,
            null,
            null,
            null,
            0,
            true,
            1);

        var repository = new InMemoryPartyRepository(BuildWorkspace(requestId) with
        {
            RequestStatus = "Submitted",
            ProposedIntendedUsers = new[] { singleUser }
        });

        var service = CreateService(repository);

        var action = () => service.RemoveProposedIntendedUserAsync(
            requestId,
            singleUser.IntendedUserId,
            new RemoveIntendedUserRequest(1),
            Guid.NewGuid(),
            CancellationToken.None);

        var error = await Assert.ThrowsAsync<AppraisalRequestApplicationException>(action);
        error.Code.Should().Be("CIU-005");
    }

    [Fact]
    public async Task AddProposedIntendedUserAsync_WhenValid_ReturnsCreatedUser()
    {
        var requestId = Guid.NewGuid();
        var repository = new InMemoryPartyRepository(BuildWorkspace(requestId));
        var service = CreateService(repository);

        var created = await service.AddProposedIntendedUserAsync(
            requestId,
            new CreateIntendedUserRequest(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                false,
                "Jane Smith",
                null,
                "Counsel",
                null,
                "jane@example.com",
                "555-0100",
                null,
                1,
                1,
                null,
                null),
            Guid.NewGuid(),
            CancellationToken.None);

        created.DisplayName.Should().Be("Jane Smith");
        repository.Current.ProposedIntendedUsers.Should().HaveCount(2);
    }

    private static AppraisalRequestPartyService CreateService(InMemoryPartyRepository repository)
    {
        return new AppraisalRequestPartyService(repository, repository, new StubClock());
    }

    private static PartyIdentificationResponse BuildWorkspace(Guid requestId)
    {
        var user = new ProposedIntendedUserSnapshot(
            Guid.NewGuid(),
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            true,
            "Client User",
            null,
            "Client",
            null,
            "client@example.com",
            "555-0111",
            null,
            0,
            true,
            1);

        var intendedUse = new IntendedUseSnapshot(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "INSURANCE",
            "Insurance placement",
            "Insurance placement",
            "Insurance",
            false,
            null,
            1);

        var client = new ClientSnapshot(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "CL-000001",
            "PERSON",
            "Client User",
            null,
            "client@example.com",
            "555-0111",
            null,
            1);

        return new PartyIdentificationResponse(
            requestId,
            client.ClientId,
            client,
            null,
            null,
            intendedUse.IntendedUseId,
            intendedUse,
            new[] { user },
            null,
            null,
            null,
            "Draft",
            1,
            new ValidationSummary(true, 0, 0, Array.Empty<ValidationIssue>()));
    }

    private sealed class StubClock : IAppraisalRequestClock
    {
        public DateTime UtcNow => new(2026, 9, 15, 12, 0, 0, DateTimeKind.Utc);
    }

    private sealed class InMemoryPartyRepository : IAppraisalRequestPartyRepository, IPartyReferenceDataRepository
    {
        public InMemoryPartyRepository(PartyIdentificationResponse current)
        {
            Current = current;
        }

        public PartyIdentificationResponse Current { get; private set; }

        public Task<PartyIdentificationResponse?> GetAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
        {
            return Task.FromResult<PartyIdentificationResponse?>(Current.AppraisalRequestId == appraisalRequestId ? Current : null);
        }

        public Task<bool> SetClientAsync(Guid appraisalRequestId, Guid clientId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, expectedRowVersion))
            {
                return Task.FromResult(false);
            }

            Current = Current with
            {
                ClientId = clientId,
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1
            };

            return Task.FromResult(true);
        }

        public Task<bool> SetPrimaryClientContactAsync(Guid appraisalRequestId, Guid? clientContactId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, expectedRowVersion))
            {
                return Task.FromResult(false);
            }

            Current = Current with
            {
                PrimaryClientContactId = clientContactId,
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1
            };

            return Task.FromResult(true);
        }

        public Task<bool> SetProposedIntendedUseAsync(Guid appraisalRequestId, Guid intendedUseId, string? customNarrative, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, expectedRowVersion))
            {
                return Task.FromResult(false);
            }

            var use = Current.ProposedIntendedUse;
            if (use is null)
            {
                use = new IntendedUseSnapshot(intendedUseId, Guid.NewGuid(), "OTHER", "Other", "Other", "Other", true, customNarrative, 1);
            }
            else
            {
                use = use with { IntendedUseId = intendedUseId, CustomNarrative = customNarrative };
            }

            Current = Current with
            {
                IntendedUseId = intendedUseId,
                ProposedIntendedUse = use,
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1
            };

            return Task.FromResult(true);
        }

        public Task<Guid> AddProposedIntendedUserAsync(Guid appraisalRequestId, CreateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, request.ExpectedRowVersion))
            {
                throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
            }

            var id = Guid.NewGuid();
            var created = new ProposedIntendedUserSnapshot(
                id,
                request.UserTypeId,
                request.IsClient,
                request.DisplayName,
                request.OrganizationName,
                request.RelationshipToClient,
                request.Address,
                request.Email,
                request.Phone,
                request.UseRestrictions,
                request.SortOrder,
                true,
                1);

            Current = Current with
            {
                ProposedIntendedUsers = Current.ProposedIntendedUsers.Concat(new[] { created }).ToArray(),
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1
            };

            return Task.FromResult(id);
        }

        public Task<bool> UpdateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, UpdateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, request.ExpectedRowVersion))
            {
                return Task.FromResult(false);
            }

            var list = Current.ProposedIntendedUsers.ToList();
            var index = list.FindIndex(x => x.IntendedUserId == intendedUserId);
            if (index < 0)
            {
                return Task.FromResult(false);
            }

            list[index] = list[index] with
            {
                UserTypeId = request.UserTypeId,
                IsClient = request.IsClient,
                DisplayName = request.DisplayName,
                OrganizationName = request.OrganizationName,
                RelationshipToClient = request.RelationshipToClient,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                UseRestrictions = request.UseRestrictions,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive,
                RowVersion = list[index].RowVersion + 1
            };

            Current = Current with
            {
                ProposedIntendedUsers = list,
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1
            };

            return Task.FromResult(true);
        }

        public Task<bool> DeactivateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, expectedRowVersion))
            {
                return Task.FromResult(false);
            }

            var list = Current.ProposedIntendedUsers.ToList();
            var index = list.FindIndex(x => x.IntendedUserId == intendedUserId);
            if (index < 0)
            {
                return Task.FromResult(false);
            }

            list[index] = list[index] with { IsActive = false, RowVersion = list[index].RowVersion + 1 };
            Current = Current with
            {
                ProposedIntendedUsers = list,
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1
            };

            return Task.FromResult(true);
        }

        public Task<bool> SaveClientExclusionExplanationAsync(Guid appraisalRequestId, string explanation, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
        {
            return Task.FromResult(MatchesRequestAndVersion(appraisalRequestId, expectedRowVersion));
        }

        public Task<bool> MarkConvertedAsync(Guid appraisalRequestId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
        {
            if (!MatchesRequestAndVersion(appraisalRequestId, expectedRowVersion))
            {
                return Task.FromResult(false);
            }

            Current = Current with
            {
                RequestStatus = "Converted",
                AppraisalRequestRowVersion = Current.AppraisalRequestRowVersion + 1,
                ReviewedAt = DateTimeOffset.UtcNow,
                ReviewedByUserId = actorId
            };

            return Task.FromResult(true);
        }

        public Task<bool> ClientIsActiveAsync(Guid clientId, CancellationToken cancellationToken)
        {
            return Task.FromResult(clientId != Guid.Empty);
        }

        public Task<bool> ContactBelongsToClientAsync(Guid clientId, Guid clientContactId, CancellationToken cancellationToken)
        {
            return Task.FromResult(clientContactId != Guid.Empty && clientId == Current.ClientId);
        }

        public Task<bool> ContactIsActiveAsync(Guid clientContactId, CancellationToken cancellationToken)
        {
            return Task.FromResult(clientContactId != Guid.Empty);
        }

        public Task<bool> IntendedUseIsSelectableAsync(Guid intendedUseId, DateTime utcNow, CancellationToken cancellationToken)
        {
            return Task.FromResult(intendedUseId != Guid.Empty);
        }

        public Task<bool> IntendedUserTypeIsActiveAsync(Guid userTypeId, CancellationToken cancellationToken)
        {
            return Task.FromResult(userTypeId != Guid.Empty);
        }

        private bool MatchesRequestAndVersion(Guid appraisalRequestId, ulong expectedVersion)
        {
            return Current.AppraisalRequestId == appraisalRequestId && Current.AppraisalRequestRowVersion == expectedVersion;
        }
    }
}
