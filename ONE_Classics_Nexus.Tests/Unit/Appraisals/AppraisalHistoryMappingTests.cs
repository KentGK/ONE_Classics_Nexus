using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Vehicles;
using ONE_Classics_Nexus.Domain.Entities;
using Xunit;

namespace ONE_Classics_Nexus.Tests.Unit.Appraisals;

public sealed class AppraisalHistoryMappingTests
{
    private static readonly DateTime Created = new(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void DetailMappingPreservesAppraisalMetadataAndVehicle()
    {
        var vehicle = new Vehicle { VehicleId = Guid.NewGuid(), Vin = "TEST-VIN", Make = "Ford", Model = "Mustang", Year = 1967 };
        var appraisal = new Appraisal
        {
            AppraisalId = Guid.NewGuid(), VehicleId = vehicle.VehicleId,
            AppraisalNumber = "A-42", AppraisalRequestId = Guid.NewGuid(), ClientId = Guid.NewGuid(),
            AssignedAppraiserId = Guid.NewGuid(), AssignedReviewerId = Guid.NewGuid(),
            AppraisalType = "Insurance", Status = "Draft", IntakeStatus = "Complete",
            InspectionStatus = "Pending", ValuationStatus = "Pending", ReportStatus = "Draft",
            EffectiveDateUtc = Created, CreatedAtUtc = Created, UpdatedAtUtc = Created.AddDays(1), RowVersion = 7
        };

        var dto = appraisal.ToDetailDto(vehicle);

        Assert.Equal(appraisal.AppraisalId, dto.AppraisalId);
        Assert.Equal(appraisal.VehicleId, dto.VehicleId);
        Assert.Equal(appraisal.AppraisalNumber, dto.AppraisalNumber);
        Assert.Equal(appraisal.AppraisalRequestId, dto.AppraisalRequestId);
        Assert.Equal(appraisal.ClientId, dto.ClientId);
        Assert.Equal(appraisal.AssignedAppraiserId, dto.AssignedAppraiserId);
        Assert.Equal(appraisal.AssignedReviewerId, dto.AssignedReviewerId);
        Assert.Equal(appraisal.AppraisalType, dto.AppraisalType);
        Assert.Equal(appraisal.Status, dto.Status);
        Assert.Equal(appraisal.IntakeStatus, dto.IntakeStatus);
        Assert.Equal(appraisal.InspectionStatus, dto.InspectionStatus);
        Assert.Equal(appraisal.ValuationStatus, dto.ValuationStatus);
        Assert.Equal(appraisal.ReportStatus, dto.ReportStatus);
        Assert.Equal(appraisal.EffectiveDateUtc, dto.EffectiveDateUtc);
        Assert.Equal(appraisal.RowVersion, dto.RowVersion);
        Assert.Equal(appraisal.CreatedAtUtc, dto.CreatedAtUtc);
        Assert.Equal(appraisal.UpdatedAtUtc, dto.UpdatedAtUtc);
        Assert.Equal(vehicle.ToDto(), dto.Vehicle);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public async Task HistoryEmitsVehicleUpdatedOnlyAfterCreation(int updateDays, bool hasUpdate)
    {
        var vehicle = new Vehicle { VehicleId = Guid.NewGuid(), CreatedAtUtc = Created, UpdatedAtUtc = Created.AddDays(updateDays) };
        var handler = new GetVehicleHistoryHandler(new Vehicles(vehicle), new Appraisals(), new Inspections());

        var result = await handler.HandleAsync(vehicle.VehicleId, CancellationToken.None);

        Assert.Equal(vehicle.VehicleId, result.VehicleId);
        Assert.Single(result.Events, x => x.EventType == "VehicleCreated");
        var updates = result.Events.Where(x => x.EventType == "VehicleUpdated").ToArray();
        if (hasUpdate)
            Assert.Equal(vehicle.UpdatedAtUtc, Assert.Single(updates).OccurredAtUtc);
        else
            Assert.Empty(updates);
    }

    [Fact]
    public async Task HistoryLoadsAllPagesAndOrdersLinkedEventsNewestFirst()
    {
        var vehicle = new Vehicle { VehicleId = Guid.NewGuid(), CreatedAtUtc = Created, UpdatedAtUtc = Created };
        var first = new Appraisal { AppraisalId = Guid.NewGuid(), VehicleId = vehicle.VehicleId, CreatedAtUtc = Created.AddDays(1), UpdatedAtUtc = Created.AddDays(2) };
        var second = new Appraisal { AppraisalId = Guid.NewGuid(), VehicleId = vehicle.VehicleId, CreatedAtUtc = Created.AddDays(3), UpdatedAtUtc = Created.AddDays(3) };
        var inspection = new Inspection { InspectionId = Guid.NewGuid(), AppraisalId = first.AppraisalId, Status = "Complete", CreatedAtUtc = Created.AddDays(4), UpdatedAtUtc = Created.AddDays(5) };
        var appraisals = new Appraisals(first, second);
        var inspections = new Inspections(inspection);
        var handler = new GetVehicleHistoryHandler(new Vehicles(vehicle), appraisals, inspections);
        using var cancellation = new CancellationTokenSource();

        var result = await handler.HandleAsync(vehicle.VehicleId, cancellation.Token);

        Assert.Equal(new[] { 1, 2 }, appraisals.Filters.Select(x => x.Page));
        Assert.All(appraisals.Filters, filter => Assert.Equal(vehicle.VehicleId, filter.VehicleId));
        Assert.All(appraisals.Tokens, token => Assert.Equal(cancellation.Token, token));
        Assert.Equal(new[] { first.AppraisalId, second.AppraisalId }, inspections.RequestedIds);
        Assert.Equal(6, result.Events.Count);
        Assert.Equal(result.Events.OrderByDescending(x => x.OccurredAtUtc), result.Events);
        var update = Assert.Single(result.Events, x => x.EventType == "InspectionUpdated");
        Assert.Equal(inspection.UpdatedAtUtc, update.OccurredAtUtc);
        Assert.Equal(first.AppraisalId, update.AppraisalId);
        Assert.Equal(inspection.InspectionId, update.InspectionId);
        Assert.Contains("Complete", update.Description);
    }

    private sealed class Vehicles(Vehicle vehicle) : IVehicleRepository
    {
        public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult<Vehicle?>(id == vehicle.VehicleId ? vehicle : null);
        public Task<Vehicle> CreateAsync(Vehicle value, CancellationToken ct) => throw new NotSupportedException();
        public Task<Vehicle?> UpdateAsync(Vehicle value, CancellationToken ct) => throw new NotSupportedException();
        public Task<bool> ArchiveAsync(Guid id, CancellationToken ct) => throw new NotSupportedException();
        public Task<bool> MergeAsync(Guid source, Guid target, CancellationToken ct) => throw new NotSupportedException();
        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> ListAsync(int page, int pageSize, string make, string model, int? year, string titleStatus, CancellationToken ct) => throw new NotSupportedException();
        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> SearchAsync(SearchVehiclesRequest request, CancellationToken ct) => throw new NotSupportedException();
    }

    private sealed class Appraisals(params Appraisal[] items) : IAppraisalRepository
    {
        public List<AppraisalBrowseFilter> Filters { get; } = new();
        public List<CancellationToken> Tokens { get; } = new();
        public Task<PagedResult<Appraisal>> BrowseAsync(AppraisalBrowseFilter filter, CancellationToken ct)
        {
            Filters.Add(filter);
            Tokens.Add(ct);
            IReadOnlyList<Appraisal> page = items.Skip(filter.Page - 1).Take(1).ToArray();
            return Task.FromResult(new PagedResult<Appraisal>(page, filter.Page, filter.PageSize, items.Length == 0 ? 0 : 201));
        }
        public Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken ct) => throw new NotSupportedException();
        public Task<Appraisal?> GetByIdAsync(Guid id, CancellationToken ct) => throw new NotSupportedException();
    }

    private sealed class Inspections(params Inspection[] items) : IInspectionRepository
    {
        public List<Guid> RequestedIds { get; } = new();
        public Task<IReadOnlyList<Inspection>> GetByAppraisalIdAsync(Guid id, CancellationToken ct)
        {
            RequestedIds.Add(id);
            return Task.FromResult<IReadOnlyList<Inspection>>(items.Where(x => x.AppraisalId == id).ToArray());
        }
        public Task<Inspection> CreateAsync(Inspection inspection, CancellationToken ct) => throw new NotSupportedException();
        public Task<Inspection?> GetByIdAsync(Guid id, CancellationToken ct) => throw new NotSupportedException();
        public Task SaveScoresAsync(Guid id, IReadOnlyList<ConditionScore> scores, CancellationToken ct) => throw new NotSupportedException();
    }
}
