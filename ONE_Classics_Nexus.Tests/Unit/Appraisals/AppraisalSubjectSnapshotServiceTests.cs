using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Services;
using ONE_Classics_Nexus.Domain.Entities;
using FluentAssertions;

namespace ONE_Classics_Nexus.Tests.Unit.Appraisals;

public sealed class AppraisalSubjectSnapshotServiceTests
{
    [Fact]
    public async Task CreateFromVehicleAsync_WhenValid_CreatesDraftSnapshot()
    {
        var appraisalId = Guid.NewGuid();
        var vehicle = CreateVehicle();

        var appraisalRepository = new FakeAppraisalRepository(new Appraisal
        {
            AppraisalId = appraisalId,
            VehicleId = vehicle.VehicleId,
            Status = "Draft",
            AppraisalType = "Market Value",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = 1
        });

        var vehicleRepository = new FakeVehicleRepository(vehicle);
        var snapshotRepository = new FakeSnapshotRepository();

        var sut = new AppraisalSubjectSnapshotService(appraisalRepository, vehicleRepository, snapshotRepository);

        var result = await sut.CreateFromVehicleAsync(appraisalId, vehicle.VehicleId, CancellationToken.None);

        result.AppraisalId.Should().Be(appraisalId);
        result.VehicleId.Should().Be(vehicle.VehicleId);
        result.Vin.Should().Be(vehicle.Vin);
        result.SnapshotStatus.Should().Be(AppraisalSubjectSnapshot.DraftStatus);
    }

    [Fact]
    public async Task RefreshDraftFromVehicleAsync_WhenDraft_UpdatesSnapshotFields()
    {
        var appraisalId = Guid.NewGuid();
        var vehicle = CreateVehicle();
        var snapshot = AppraisalSubjectSnapshot.CreateDraft(appraisalId, vehicle);

        var updatedVehicle = Vehicle.Rehydrate(
            vehicle.VehicleId,
            vehicle.VehicleTypeId,
            vehicle.Vin,
            vehicle.Year,
            vehicle.Make,
            vehicle.Marque,
            vehicle.Model,
            vehicle.Trim,
            vehicle.SeriesName,
            vehicle.BodyStyle,
            vehicle.ChassisNumber,
            vehicle.BodyNumber,
            vehicle.EngineNumber,
            vehicle.Engine,
            vehicle.Transmission,
            vehicle.Drivetrain,
            vehicle.Mileage,
            vehicle.OdometerUnit,
            vehicle.OdometerStatus,
            "Black",
            vehicle.InteriorColor,
            vehicle.TitleStatus,
            vehicle.AuthenticityProfile,
            vehicle.OriginalityProfile,
            vehicle.RestorationLevel,
            vehicle.DocumentationLevel,
            vehicle.OverallConditionScore,
            vehicle.HasRedFlags,
            vehicle.Notes,
            "Updated ownership",
            vehicle.VehicleStatus,
            vehicle.RowVersion,
            vehicle.CreatedAtUtc,
            vehicle.UpdatedAtUtc);

        var appraisalRepository = new FakeAppraisalRepository(new Appraisal
        {
            AppraisalId = appraisalId,
            VehicleId = vehicle.VehicleId,
            Status = "Draft",
            AppraisalType = "Market Value",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = 1
        });

        var vehicleRepository = new FakeVehicleRepository(updatedVehicle);
        var snapshotRepository = new FakeSnapshotRepository(snapshot);

        var sut = new AppraisalSubjectSnapshotService(appraisalRepository, vehicleRepository, snapshotRepository);

        var result = await sut.RefreshDraftFromVehicleAsync(appraisalId, CancellationToken.None);

        result.ExteriorColor.Should().Be("Black");
        result.OwnershipNotes.Should().Be("Updated ownership");
        result.RowVersion.Should().Be(2);
    }

    [Fact]
    public async Task ApproveAsync_WhenDraft_ApprovesSnapshot()
    {
        var appraisalId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var snapshot = AppraisalSubjectSnapshot.CreateDraft(appraisalId, CreateVehicle());

        var sut = new AppraisalSubjectSnapshotService(
            new FakeAppraisalRepository(null),
            new FakeVehicleRepository(null),
            new FakeSnapshotRepository(snapshot));

        await sut.ApproveAsync(appraisalId, userId, CancellationToken.None);

        snapshot.SnapshotStatus.Should().Be(AppraisalSubjectSnapshot.ApprovedStatus);
        snapshot.ApprovedByUserId.Should().Be(userId);
        snapshot.ApprovedAtUtc.Should().NotBeNull();
    }

    private static Vehicle CreateVehicle()
    {
        return Vehicle.Create(
            vehicleTypeId: Guid.NewGuid(),
            vin: "1FAFP45X1WF123456",
            year: 1967,
            make: "Ford",
            marque: null,
            model: "Mustang",
            trim: "GT",
            seriesName: "First Gen",
            bodyStyle: "Coupe",
            chassisNumber: "CHS-123",
            bodyNumber: "BDY-456",
            engineNumber: "ENG-789",
            engine: "V8",
            transmission: "Manual",
            drivetrain: "RWD",
            mileage: 12345,
            odometerUnit: "miles",
            odometerStatus: "Actual",
            exteriorColor: "Red",
            interiorColor: "Black",
            titleStatus: "Clean",
            authenticityProfile: "Unknown",
            originalityProfile: "Unknown",
            restorationLevel: "DriverRestoration",
            documentationLevel: "Good",
            overallConditionScore: 72m,
            hasRedFlags: false,
            notes: "Test vehicle",
            ownershipNotes: "Single owner");
    }

    private sealed class FakeAppraisalRepository : IAppraisalRepository
    {
        private readonly Appraisal? _appraisal;

        public FakeAppraisalRepository(Appraisal? appraisal)
        {
            _appraisal = appraisal;
        }

        public Task<Appraisal> InsertAsync(Appraisal appraisal, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Appraisal?> UpdateAsync(Appraisal appraisal, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Appraisal?> GetByIdAsync(Guid appraisalId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_appraisal);
        }

        public Task<PagedResult<Appraisal>> BrowseAsync(AppraisalBrowseFilter filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid appraisalId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class FakeVehicleRepository : IVehicleRepository
    {
        private readonly Vehicle? _vehicle;

        public FakeVehicleRepository(Vehicle? vehicle)
        {
            _vehicle = vehicle;
        }

        public Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken ct)
        {
            return Task.FromResult(_vehicle);
        }

        public Task<Vehicle?> UpdateAsync(Vehicle vehicle, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> ListAsync(
            int page,
            int pageSize,
            string? search = null,
            string? vin = null,
            int? yearFrom = null,
            int? yearTo = null,
            string? make = null,
            string? model = null,
            string? series = null,
            Guid? vehicleTypeId = null,
            string? bodyStyle = null,
            int? year = null,
            int? mileage = null,
            string? exteriorColor = null,
            bool? hasRedFlags = null,
            string? titleStatus = null,
            string? sortBy = null,
            string? sortDirection = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<(IReadOnlyList<Vehicle> items, int totalCount)> ListAsync(
            int page,
            int pageSize,
            string make,
            string model,
            int? year,
            string titleStatus,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> SearchAsync(ONE_Classics_Nexus.Application.Requests.SearchVehiclesRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ArchiveAsync(Guid vehicleId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MergeAsync(Guid sourceVehicleId, Guid targetVehicleId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class FakeSnapshotRepository : IAppraisalSubjectSnapshotRepository
    {
        private AppraisalSubjectSnapshot? _snapshot;

        public FakeSnapshotRepository(AppraisalSubjectSnapshot? snapshot = null)
        {
            _snapshot = snapshot;
        }

        public Task<AppraisalSubjectSnapshot?> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken)
        {
            return Task.FromResult<AppraisalSubjectSnapshot?>(_snapshot);
        }

        public Task<AppraisalSubjectSnapshot> InsertAsync(AppraisalSubjectSnapshot snapshot, CancellationToken cancellationToken)
        {
            _snapshot = snapshot;
            return Task.FromResult(snapshot);
        }

        public Task<AppraisalSubjectSnapshot?> UpdateAsync(AppraisalSubjectSnapshot snapshot, CancellationToken cancellationToken)
        {
            _snapshot = snapshot;
            return Task.FromResult(_snapshot);
        }

        public Task<AppraisalSubjectSnapshot?> GetByIdAsync(Guid appraisalSubjectSnapshotId, CancellationToken cancellationToken)
        {
            return Task.FromResult<AppraisalSubjectSnapshot?>(_snapshot);
        }

        public Task<bool> DeleteByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken)
        {
            var existed = _snapshot is not null;
            _snapshot = null;
            return Task.FromResult(existed);
        }
    }
}
