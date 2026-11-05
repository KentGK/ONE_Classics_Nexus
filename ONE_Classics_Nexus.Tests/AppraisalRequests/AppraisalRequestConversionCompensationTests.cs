using FluentAssertions;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Domain.AppraisalRequests;

namespace ONE_Classics_Nexus.Tests.AppraisalRequests;

public sealed class AppraisalRequestConversionCompensationTests
{
    private static readonly DateTime Now = new(2026, 8, 31, 18, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task ConvertAsync_WhenSaveSucceeds_DoesNotDeleteAppraisal()
    {
        var request = AcceptedRequest();
        var gateway = new RecordingConversionGateway();
        var service = CreateService(request, gateway, saveSucceeds: true);

        await service.ConvertAsync(request.AppraisalRequestId, new ConvertAppraisalRequestCommand(request.RowVersion), CancellationToken.None);

        gateway.DeletedAppraisalIds.Should().BeEmpty();
    }

    [Fact]
    public async Task ConvertAsync_WhenSaveFails_DeletesProvisionalAppraisal()
    {
        var request = AcceptedRequest();
        var gateway = new RecordingConversionGateway();
        var service = CreateService(request, gateway, saveSucceeds: false);

        var action = () => service.ConvertAsync(request.AppraisalRequestId, new ConvertAppraisalRequestCommand(request.RowVersion), CancellationToken.None);

        await Assert.ThrowsAsync<AppraisalRequestApplicationException>(action);
        gateway.DeletedAppraisalIds.Should().Equal(gateway.CreatedAppraisalId);
    }

    [Fact]
    public async Task ConvertAsync_WhenSaveFails_PropagatesOriginalConflictCode()
    {
        var request = AcceptedRequest();
        var service = CreateService(request, new RecordingConversionGateway(), saveSucceeds: false);

        var action = () => service.ConvertAsync(request.AppraisalRequestId, new ConvertAppraisalRequestCommand(request.RowVersion), CancellationToken.None);

        var error = await Assert.ThrowsAsync<AppraisalRequestApplicationException>(action);
        error.Code.Should().Be("APPRAISAL_REQUEST_CONCURRENCY_CONFLICT");
    }

    [Fact]
    public async Task ConvertAsync_WhenCleanupAlsoFails_ReportsCleanupFailureWithAppraisalId()
    {
        var request = AcceptedRequest();
        var gateway = new RecordingConversionGateway { FailDelete = true };
        var service = CreateService(request, gateway, saveSucceeds: false);

        var action = () => service.ConvertAsync(request.AppraisalRequestId, new ConvertAppraisalRequestCommand(request.RowVersion), CancellationToken.None);

        var error = await Assert.ThrowsAsync<AppraisalRequestApplicationException>(action);
        error.Code.Should().Be("APPRAISAL_REQUEST_CONVERSION_CLEANUP_FAILED");
        error.Message.Should().Contain(gateway.CreatedAppraisalId.ToString());
    }

    private static AppraisalRequestService CreateService(AppraisalRequest request, RecordingConversionGateway gateway, bool saveSucceeds)
        => new(
            new StubRepository(request, saveSucceeds),
            new StubNumberGenerator(),
            new StubClock(),
            new StubCurrentUser(),
            gateway,
            new NullAuditWriter());

    private static AppraisalRequest AcceptedRequest()
    {
        var request = AppraisalRequest.Create(Guid.NewGuid(), "AR-2026-000001", Guid.NewGuid(), null,
            "Collector Vehicle Appraisal", "Insurance", null, null, null, null, "Physical", null, Guid.NewGuid(),
            1969, "Chevrolet", "Camaro", "123", null, null, "Portal", true, Now);
        request.Submit(Now);
        request.BeginReview(Now);
        request.MarkQualified(Guid.NewGuid(), null, Now);
        request.Accept(Now);
        return request;
    }

    private sealed class RecordingConversionGateway : IAppraisalRequestConversionGateway
    {
        public Guid CreatedAppraisalId { get; } = Guid.NewGuid();
        public List<Guid> DeletedAppraisalIds { get; } = [];
        public bool FailDelete { get; init; }

        public Task<CreatedAppraisal> CreateAppraisalFromRequestAsync(AppraisalSeed seed, CancellationToken ct)
            => Task.FromResult(new CreatedAppraisal(CreatedAppraisalId, "AN-000001"));

        public Task DeleteProvisionalAppraisalAsync(Guid appraisalId, CancellationToken ct)
        {
            if (FailDelete)
            {
                throw new InvalidOperationException("Cleanup failed.");
            }

            DeletedAppraisalIds.Add(appraisalId);
            return Task.CompletedTask;
        }
    }

    private sealed class StubRepository(AppraisalRequest request, bool saveSucceeds) : IAppraisalRequestRepository
    {
        public Task<AppraisalRequest?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult<AppraisalRequest?>(request);
        public Task<AppraisalRequest?> GetByRequestNumberAsync(string requestNumber, CancellationToken ct) => Task.FromResult<AppraisalRequest?>(request);
        public Task<PagedResult<AppraisalRequestSummary>> SearchAsync(AppraisalRequestSearchCriteria criteria, CancellationToken ct) => throw new NotImplementedException();
        public Task InsertAsync(AppraisalRequest appraisalRequest, CancellationToken ct) => Task.CompletedTask;
        public Task<bool> UpdateAsync(AppraisalRequest appraisalRequest, long expectedRowVersion, CancellationToken ct) => Task.FromResult(saveSucceeds);
    }

    private sealed class StubNumberGenerator : IAppraisalRequestNumberGenerator
    {
        public Task<string> NextAsync(DateTime utcNow, CancellationToken ct) => Task.FromResult("AR-2026-000001");
    }

    private sealed class StubClock : IAppraisalRequestClock
    {
        public DateTime UtcNow => Now;
    }

    private sealed class StubCurrentUser : ICurrentNexusUser
    {
        public Guid? UserId { get; } = Guid.NewGuid();
    }

    private sealed class NullAuditWriter : IAppraisalRequestAuditWriter
    {
        public Task WriteAsync(Guid requestId, Guid? actorUserId, string action, string? priorStatus, string newStatus,
            string? reason, Guid? appraisalId, CancellationToken ct) => Task.CompletedTask;
    }
}
