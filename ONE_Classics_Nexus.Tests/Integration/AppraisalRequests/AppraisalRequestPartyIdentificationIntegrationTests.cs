using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Tests.Integration.Infrastructure;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;

namespace ONE_Classics_Nexus.Tests.Integration.AppraisalRequests;

public sealed class AppraisalRequestPartyIdentificationIntegrationTests
{
    [Fact(DisplayName = "CIU-I-001 GET party-identification returns workspace model")]
    public async Task Ciu_I_001_GetWorkspace_ShouldReturnOk()
    {
        var requestId = Guid.NewGuid();
        var workspace = BuildWorkspace(requestId);

        await using var factory = CreateFactory(new StubPartyService
        {
            OnGetAsync = (_, _) => Task.FromResult<PartyIdentificationResponse?>(workspace)
        });

        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/appraisal-requests/{requestId}/party-identification");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<PartyIdentificationResponse>();
        body.Should().NotBeNull();
        body!.AppraisalRequestId.Should().Be(requestId);
        body.Validation.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "CIU-I-002 PUT set-client maps application exception to 422 with stable code")]
    public async Task Ciu_I_002_SetClient_WithBusinessError_ShouldReturnProblemDetails()
    {
        var requestId = Guid.NewGuid();

        await using var factory = CreateFactory(new StubPartyService
        {
            OnSetClientAsync = (_, _, _, _) => throw new AppraisalRequestApplicationException("CIU-001", "Client is required.", 422)
        });

        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/api/appraisal-requests/{requestId}/party-identification/client",
            new SetAppraisalRequestClientRequest(Guid.Empty, 1UL));

        response.StatusCode.Should().Be((HttpStatusCode)422);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(422);

        var payload = await response.Content.ReadAsStringAsync();
        payload.Should().Contain("\"code\":\"CIU-001\"");
    }

    [Fact(DisplayName = "CIU-I-003 POST validate returns validation summary")]
    public async Task Ciu_I_003_Validate_ShouldReturnSummary()
    {
        var requestId = Guid.NewGuid();
        var validation = new ValidationSummary(
            false,
            1,
            1,
            new[]
            {
                new ValidationIssue("CIU-005", "error", "proposedIntendedUsers", "At least one proposed intended user is required before acceptance or conversion."),
                new ValidationIssue("CIU-014", "warning", "proposedIntendedUsers", "Client is not identified as a proposed intended user; review is required.")
            });

        await using var factory = CreateFactory(new StubPartyService
        {
            OnValidateAsync = (_, _) => Task.FromResult(validation)
        });

        using var client = factory.CreateClient();

        var response = await client.PostAsync($"/api/appraisal-requests/{requestId}/party-identification/validate", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ValidationSummary>();
        body.Should().NotBeNull();
        body!.BlockingIssueCount.Should().Be(1);
        body.WarningCount.Should().Be(1);
        body.Issues.Should().Contain(x => x.Code == "CIU-005");
    }

    private static WebApplicationFactory<Program> CreateFactory(StubPartyService stub)
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, cb) =>
            {
                cb.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=test;Uid=test;Pwd=test;",
                    ["ConnectionStrings:Nexus"] = "Server=localhost;Database=test;Uid=test;Pwd=test;"
                });
            });

            builder.ConfigureServices(services =>
            {
                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                        options.DefaultChallengeScheme = TestAuthHandler.Scheme;
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.Scheme, _ => { });

                services.RemoveAll<IAppraisalRequestPartyService>();
                services.AddSingleton<IAppraisalRequestPartyService>(stub);
            });
        });
    }

    private static PartyIdentificationResponse BuildWorkspace(Guid requestId)
    {
        var client = new ClientSnapshot(
            Guid.NewGuid(),
            "CL-100001",
            "PERSON",
            "Sample Client",
            null,
            "client@example.com",
            "555-0199",
            null,
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

        return new PartyIdentificationResponse(
            requestId,
            client.ClientId,
            client,
            null,
            null,
            intendedUse.IntendedUseId,
            intendedUse,
            Array.Empty<ProposedIntendedUserSnapshot>(),
            null,
            null,
            null,
            "Draft",
            1,
            new ValidationSummary(true, 0, 0, Array.Empty<ValidationIssue>()));
    }

    private sealed class StubPartyService : IAppraisalRequestPartyService
    {
        public Func<Guid, CancellationToken, Task<PartyIdentificationResponse?>>? OnGetAsync { get; init; }
        public Func<Guid, SetAppraisalRequestClientRequest, Guid, CancellationToken, Task<PartyIdentificationResponse>>? OnSetClientAsync { get; init; }
        public Func<Guid, CancellationToken, Task<ValidationSummary>>? OnValidateAsync { get; init; }

        public Task<PartyIdentificationResponse?> GetAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
            => OnGetAsync?.Invoke(appraisalRequestId, cancellationToken)
               ?? Task.FromResult<PartyIdentificationResponse?>(null);

        public Task<PartyIdentificationResponse> SetClientAsync(Guid appraisalRequestId, SetAppraisalRequestClientRequest request, Guid actorId, CancellationToken cancellationToken)
            => OnSetClientAsync?.Invoke(appraisalRequestId, request, actorId, cancellationToken)
               ?? Task.FromResult(BuildWorkspace(appraisalRequestId));

        public Task<ValidationSummary> ValidateAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
            => OnValidateAsync?.Invoke(appraisalRequestId, cancellationToken)
               ?? Task.FromResult(new ValidationSummary(true, 0, 0, Array.Empty<ValidationIssue>()));

        public Task<PartyIdentificationResponse> SetPrimaryClientContactAsync(Guid appraisalRequestId, SetPrimaryClientContactRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(BuildWorkspace(appraisalRequestId));

        public Task<PartyIdentificationResponse> SetProposedIntendedUseAsync(Guid appraisalRequestId, SetIntendedUseRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(BuildWorkspace(appraisalRequestId));

        public Task<ProposedIntendedUserSnapshot> AddProposedIntendedUserAsync(Guid appraisalRequestId, CreateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(new ProposedIntendedUserSnapshot(Guid.NewGuid(), request.UserTypeId, request.IsClient, request.DisplayName, request.OrganizationName, request.RelationshipToClient, request.Address, request.Email, request.Phone, request.UseRestrictions, request.SortOrder, true, 1));

        public Task<ProposedIntendedUserSnapshot> UpdateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, UpdateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(new ProposedIntendedUserSnapshot(intendedUserId, request.UserTypeId, request.IsClient, request.DisplayName, request.OrganizationName, request.RelationshipToClient, request.Address, request.Email, request.Phone, request.UseRestrictions, request.SortOrder, request.IsActive, request.ExpectedRowVersion + 1));

        public Task<PartyIdentificationResponse> RemoveProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, RemoveIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(BuildWorkspace(appraisalRequestId));

        public Task<PartyIdentificationResponse> SaveClientExclusionExplanationAsync(Guid appraisalRequestId, SaveClientExclusionRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(BuildWorkspace(appraisalRequestId));

        public Task<PartyIdentificationResponse> ConvertAsync(Guid appraisalRequestId, ConvertPartyIdentificationRequest request, Guid actorId, CancellationToken cancellationToken)
            => Task.FromResult(BuildWorkspace(appraisalRequestId) with { RequestStatus = "Converted" });
    }
}
