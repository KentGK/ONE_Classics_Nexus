using FluentAssertions;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Vehicles;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Tests.Unit.Vehicles;

public sealed class VehicleHandlersTests
{
    [Fact]
    public async Task CreateVehicleHandler_ValidRequest_ReturnsVehicle()
    {
        var vehicleTypeId = Guid.NewGuid();
        var repository = new InMemoryVehicleRepository();
        var vehicleTypeRepository = new StubVehicleTypeRepository(vehicleTypeId, exists: true);
        var handler = new CreateVehicleHandler(repository, vehicleTypeRepository);

        var request = new CreateVehicleRequest
        {
            VehicleTypeId = vehicleTypeId,
            Vin = "1FAFP45X1WF123456",
            Year = 1967,
            Make = "Ford",
            Model = "Mustang",
            Mileage = 1000,
            OdometerUnit = "miles"
        };

        var result = await handler.HandleAsync(request, CancellationToken.None);

        result.VehicleId.Should().NotBeEmpty();
        result.Make.Should().Be("Ford");
        result.Model.Should().Be("Mustang");
    }

    [Fact]
    public async Task GetVehicleHandler_MissingVehicle_ReturnsNotFound()
    {
        var repository = new InMemoryVehicleRepository();
        var handler = new GetVehicleHandler(repository);

        Func<Task> act = async () => await handler.HandleAsync(Guid.NewGuid(), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ListVehiclesHandler_FilterByMake_ReturnsMatches()
    {
        var repository = new InMemoryVehicleRepository();
        repository.Seed(new[]
        {
            BuildVehicle("Ford", "Mustang", 1967),
            BuildVehicle("Chevrolet", "Camaro", 1969)
        });

        var handler = new ListVehiclesHandler(repository);
        var request = new ListVehiclesRequest { Make = "Ford", Page = 1, PageSize = 20 };

        var result = await handler.HandleAsync(request, CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.Items[0].Make.Should().Be("Ford");
    }

    [Fact]
    public async Task ListVehiclesHandler_PaginatesCorrectly()
    {
        var repository = new InMemoryVehicleRepository();
        repository.Seed(new[]
        {
            BuildVehicle("Ford", "Mustang", 1967),
            BuildVehicle("Ford", "Bronco", 1970),
            BuildVehicle("Ford", "Galaxie", 1965)
        });

        var handler = new ListVehiclesHandler(repository);
        var request = new ListVehiclesRequest { Page = 2, PageSize = 2 };

        var result = await handler.HandleAsync(request, CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(3);
    }

    private static Vehicle BuildVehicle(string make, string model, int year)
    {
        return Vehicle.Create(
            vehicleTypeId: Guid.NewGuid(),
            vin: Guid.NewGuid().ToString("N")[..17],
            year: year,
            make: make,
            marque: null,
            model: model,
            trim: null,
            seriesName: null,
            bodyStyle: null,
            chassisNumber: null,
            bodyNumber: null,
            engineNumber: null,
            engine: null,
            transmission: null,
            drivetrain: null,
            mileage: null,
            odometerUnit: "miles",
            odometerStatus: null,
            exteriorColor: null,
            interiorColor: null,
            titleStatus: null,
            authenticityProfile: null,
            originalityProfile: null,
            restorationLevel: null,
            documentationLevel: null,
            overallConditionScore: null,
            hasRedFlags: false,
            notes: null,
            ownershipNotes: null);
    }

    private sealed class StubVehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly Guid _vehicleTypeId;
        private readonly bool _exists;

        public StubVehicleTypeRepository(Guid vehicleTypeId, bool exists)
        {
            _vehicleTypeId = vehicleTypeId;
            _exists = exists;
        }

        public Task<bool> ExistsByIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_exists && vehicleTypeId == _vehicleTypeId);
        }

        public Task<VehicleTypeRecord?> GetByIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default)
        {
            var result = _exists && vehicleTypeId == _vehicleTypeId
                ? new VehicleTypeRecord(vehicleTypeId, "STD", "Standard", null, true, DateTime.UtcNow, DateTime.UtcNow)
                : null;

            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<VehicleTypeRecord>> ListActiveAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<VehicleTypeRecord> result = _exists
                ? new[] { new VehicleTypeRecord(_vehicleTypeId, "STD", "Standard", null, true, DateTime.UtcNow, DateTime.UtcNow) }
                : Array.Empty<VehicleTypeRecord>();

            return Task.FromResult(result);
        }
    }

    private sealed class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new();

        public void Seed(IEnumerable<Vehicle> vehicles)
        {
            _vehicles.Clear();
            _vehicles.AddRange(vehicles);
        }

        public Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct)
        {
            _vehicles.Add(vehicle);
            return Task.FromResult(vehicle);
        }

        public Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken ct)
        {
            return Task.FromResult(_vehicles.FirstOrDefault(v => v.VehicleId == vehicleId));
        }

        public Task<Vehicle?> UpdateAsync(Vehicle vehicle, CancellationToken ct)
        {
            var index = _vehicles.FindIndex(v => v.VehicleId == vehicle.VehicleId);
            if (index < 0)
            {
                return Task.FromResult<Vehicle?>(null);
            }

            _vehicles[index] = vehicle;
            return Task.FromResult<Vehicle?>(vehicle);
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
            IEnumerable<Vehicle> query = _vehicles;

            if (!string.IsNullOrWhiteSpace(make))
            {
                query = query.Where(v => string.Equals(v.Make, make, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                query = query.Where(v => string.Equals(v.Model, model, StringComparison.OrdinalIgnoreCase));
            }

            if (year.HasValue)
            {
                query = query.Where(v => v.Year == year.Value);
            }

            var filtered = query.ToList();
            var totalCount = filtered.Count;
            var pageItems = filtered
                .Skip((Math.Max(page, 1) - 1) * Math.Max(pageSize, 1))
                .Take(Math.Max(pageSize, 1))
                .ToList();

            return Task.FromResult(((IReadOnlyList<Vehicle>)pageItems, totalCount));
        }

        public Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> SearchAsync(SearchVehiclesRequest request, CancellationToken cancellationToken = default)
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

        public Task<(IReadOnlyList<Vehicle> items, int totalCount)> ListAsync(int page, int pageSize, string make, string model, int? year, string titleStatus, CancellationToken cancellationToken)
        {
            IEnumerable<Vehicle> query = _vehicles;

            if (!string.IsNullOrWhiteSpace(make))
            {
                query = query.Where(v => string.Equals(v.Make, make, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                query = query.Where(v => string.Equals(v.Model, model, StringComparison.OrdinalIgnoreCase));
            }

            if (year.HasValue)
            {
                query = query.Where(v => v.Year == year.Value);
            }

            if (!string.IsNullOrWhiteSpace(titleStatus))
            {
                query = query.Where(v => string.Equals(v.TitleStatus, titleStatus, StringComparison.OrdinalIgnoreCase));
            }

            var filtered = query.ToList();
            var totalCount = filtered.Count;
            var pageItems = filtered
                .Skip((Math.Max(page, 1) - 1) * Math.Max(pageSize, 1))
                .Take(Math.Max(pageSize, 1))
                .ToList();

            return Task.FromResult(((IReadOnlyList<Vehicle>)pageItems, totalCount));
        }
    }
}
