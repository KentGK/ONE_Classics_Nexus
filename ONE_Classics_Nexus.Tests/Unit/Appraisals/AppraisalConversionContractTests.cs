using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Appraisals;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.AppraisalRequests;
using Xunit;

namespace ONE_Classics_Nexus.Tests.Unit.Appraisals;

public sealed class AppraisalConversionContractTests
{
    [Fact]
    public async Task ConversionPersistsEveryProvisionalFieldAndReturnsGeneratedNumber()
    {
        var vehicleId = Guid.NewGuid();
        var repository = new Appraisals();
        var numbers = new Numbers();
        var gateway = new ExistingAppraisalConversionGateway(
            new CreateAppraisalHandler(new Vehicles(vehicleId), repository, numbers),
            repository, new Snapshots(repository.Events));
        var seed = new AppraisalSeed(Guid.NewGuid(), Guid.NewGuid(), vehicleId, Guid.NewGuid(),
            "Insurance", null, null, new DateOnly(2025, 6, 12), null, null, null);

        var result = await gateway.CreateAppraisalFromRequestAsync(seed, CancellationToken.None);

        var saved = Assert.IsType<Appraisal>(repository.Created);
        Assert.Equal(saved.AppraisalId, result.AppraisalId);
        Assert.Equal("AN_000001", result.AppraisalNumber);
        Assert.Equal(result.AppraisalNumber, saved.AppraisalNumber);
        Assert.Equal(seed.AppraisalRequestId, saved.AppraisalRequestId);
        Assert.Equal(seed.ClientId, saved.ClientId);
        Assert.Equal(seed.AssignedAppraiserId, saved.AssignedAppraiserId);
        Assert.Equal("Insurance", saved.AppraisalType);
        Assert.Equal("Draft", saved.Status);
        Assert.Equal("Open", saved.IntakeStatus);
        Assert.Equal("Not Started", saved.InspectionStatus);
        Assert.Equal("Not Started", saved.ValuationStatus);
        Assert.Equal("Not Started", saved.ReportStatus);
        Assert.True(seed.ProposedEffectiveDate.HasValue);
        Assert.Equal(seed.ProposedEffectiveDate.GetValueOrDefault().ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc), saved.EffectiveDateUtc);
        Assert.Equal(1, numbers.Calls);
    }

    [Fact]
    public async Task ExplicitNumberAndReviewerArePreservedWithoutGeneratingAnotherNumber()
    {
        var vehicleId = Guid.NewGuid();
        var reviewerId = Guid.NewGuid();
        var repository = new Appraisals();
        var numbers = new Numbers();
        var handler = new CreateAppraisalHandler(new Vehicles(vehicleId), repository, numbers);

        var result = await handler.HandleAsync(new CreateAppraisalRequest
        {
            VehicleId = vehicleId, AppraisalNumber = "A-42", AssignedReviewerId = reviewerId,
            AppraisalType = "Insurance", Status = "Draft", IntakeStatus = "Open",
            InspectionStatus = "Pending", ValuationStatus = "Pending", ReportStatus = "Draft"
        }, CancellationToken.None);

        Assert.Equal("A-42", result.AppraisalNumber);
        Assert.Equal(reviewerId, repository.Created!.AssignedReviewerId);
        Assert.Equal("Pending", repository.Created.InspectionStatus);
        Assert.Equal("Pending", repository.Created.ValuationStatus);
        Assert.Equal("Draft", repository.Created.ReportStatus);
        Assert.Equal(0, numbers.Calls);
    }

    [Fact]
    public async Task RollbackDeletesSnapshotBeforeAppraisalAndIgnoresEmptyId()
    {
        var repository = new Appraisals();
        var snapshots = new Snapshots(repository.Events);
        var gateway = new ExistingAppraisalConversionGateway(
            new CreateAppraisalHandler(new Vehicles(Guid.NewGuid()), repository, new Numbers()),
            repository, snapshots);
        var appraisalId = Guid.NewGuid();
        using var cancellation = new CancellationTokenSource();

        await gateway.DeleteProvisionalAppraisalAsync(appraisalId, cancellation.Token);
        await gateway.DeleteProvisionalAppraisalAsync(Guid.Empty, cancellation.Token);

        Assert.Equal(new[] { "snapshot", "appraisal" }, repository.Events);
        Assert.Equal(appraisalId, snapshots.DeletedId);
        Assert.Equal(appraisalId, repository.DeletedId);
        Assert.Equal(cancellation.Token, repository.DeleteToken);
    }

    private sealed class Numbers : IAutoNumberGenerator
    {
        public int Calls { get; private set; }
        public Task<string> NextAsync(string sequenceKey, string prefix, int width, CancellationToken cancellationToken)
        {
            Calls++;
            Assert.Equal("appraisal", sequenceKey);
            Assert.Equal("AN", prefix);
            Assert.Equal(6, width);
            return Task.FromResult("AN_000001");
        }
    }

    private sealed class Appraisals : IAppraisalRepository
    {
        public Appraisal? Created { get; private set; }
        public Guid? DeletedId { get; private set; }
        public CancellationToken DeleteToken { get; private set; }
        public List<string> Events { get; } = new();
        public Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken ct)
        {
            Created = appraisal;
            return Task.FromResult(appraisal);
        }
        public Task<bool> DeleteAsync(Guid appraisalId, CancellationToken ct)
        {
            DeletedId = appraisalId;
            DeleteToken = ct;
            Events.Add("appraisal");
            return Task.FromResult(true);
        }
        public Task<Appraisal?> GetByIdAsync(Guid id, CancellationToken ct) => throw new NotSupportedException();
        public Task<ONE_Classics_Nexus.Application.Abstractions.Paging.PagedResult<Appraisal>> BrowseAsync(
            AppraisalBrowseFilter filter, CancellationToken ct) => throw new NotSupportedException();
    }

    private sealed class Vehicles(Guid vehicleId) : IVehicleRepository
    {
        public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct) =>
            Task.FromResult<Vehicle?>(id == vehicleId ? new Vehicle { VehicleId = id } : null);
        public Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct) => throw new NotSupportedException();
        public Task<Vehicle?> UpdateAsync(Vehicle vehicle, CancellationToken ct) => throw new NotSupportedException();
        public Task<bool> ArchiveAsync(Guid id, CancellationToken ct) => throw new NotSupportedException();
        public Task<bool> MergeAsync(Guid source, Guid target, CancellationToken ct) => throw new NotSupportedException();
        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> ListAsync(int page, int pageSize, string make, string model, int? year, string status, CancellationToken ct) => throw new NotSupportedException();
        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> SearchAsync(SearchVehiclesRequest request, CancellationToken ct) => throw new NotSupportedException();
    }

    private sealed class Snapshots(List<string> events) : IAppraisalSubjectSnapshotRepository
    {
        public Guid? DeletedId { get; private set; }
        public Task<bool> DeleteByAppraisalIdAsync(Guid appraisalId, CancellationToken ct)
        {
            DeletedId = appraisalId;
            events.Add("snapshot");
            return Task.FromResult(true);
        }
        public Task<AppraisalSubjectSnapshot?> GetByAppraisalIdAsync(Guid id, CancellationToken ct) => throw new NotSupportedException();
        public Task<AppraisalSubjectSnapshot> InsertAsync(AppraisalSubjectSnapshot snapshot, CancellationToken ct) => throw new NotSupportedException();
        public Task<AppraisalSubjectSnapshot?> UpdateAsync(AppraisalSubjectSnapshot snapshot, CancellationToken ct) => throw new NotSupportedException();
    }
}
