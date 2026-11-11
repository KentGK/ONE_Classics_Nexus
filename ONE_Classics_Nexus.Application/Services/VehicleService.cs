using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.Contracts.Vehicles;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Services
{
    public sealed class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<VehicleDto> CreateAsync(
            CreateVehicleCommand command,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            ValidateCreate(command);

            var utcNow = DateTime.UtcNow;

            var vehicle = Vehicle.Create(
                vehicleTypeId: command.VehicleTypeId,
                vin: Normalize(command.Vin),
                year: command.Year,
                make: NormalizeRequired(command.Make),
                marque: Normalize(command.Marque),
                model: NormalizeRequired(command.Model),
                trim: Normalize(command.Trim),
                seriesName: Normalize(command.SeriesName),
                bodyStyle: Normalize(command.BodyStyle),
                chassisNumber: Normalize(command.ChassisNumber),
                bodyNumber: Normalize(command.BodyNumber),
                engineNumber: Normalize(command.EngineNumber),
                engine: Normalize(command.Engine),
                transmission: Normalize(command.Transmission),
                drivetrain: Normalize(command.Drivetrain),
                mileage: command.Mileage,
                odometerUnit: NormalizeRequired(command.OdometerUnit),
                odometerStatus: Normalize(command.OdometerStatus),
                exteriorColor: Normalize(command.ExteriorColor),
                interiorColor: Normalize(command.InteriorColor),
                titleStatus: Normalize(command.TitleStatus),
                authenticityProfile: Normalize(command.AuthenticityProfile),
                originalityProfile: Normalize(command.OriginalityProfile),
                restorationLevel: Normalize(command.RestorationLevel),
                documentationLevel: Normalize(command.DocumentationLevel),
                overallConditionScore: command.OverallConditionScore,
                hasRedFlags: command.HasRedFlags,
                notes: NormalizeLarge(command.Notes),
                ownershipNotes: NormalizeLarge(command.OwnershipNotes),
                nowUtc: utcNow);

            if (!string.IsNullOrWhiteSpace(command.VehicleStatus))
            {
                vehicle.ChangeStatus(command.VehicleStatus, utcNow);
            }

            var inserted = await _vehicleRepository.CreateAsync(vehicle, cancellationToken);

            if (inserted is null)
            {
                throw new InvalidOperationException("Failed to insert vehicle.");
            }

            return MapToDto(inserted);
        }

        public async Task<VehicleDto?> GetByIdAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            if (vehicleId == Guid.Empty)
            {
                throw new ArgumentException("VehicleId cannot be empty.", nameof(vehicleId));
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

            return vehicle is null ? null : MapToDto(vehicle);
        }

        public async Task<VehicleDto?> UpdateAsync(
            UpdateVehicleCommand command,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (command.VehicleId == Guid.Empty)
            {
                throw new ArgumentException("VehicleId cannot be empty.", nameof(command.VehicleId));
            }

            ValidateUpdate(command);

            var existing = await _vehicleRepository.GetByIdAsync(command.VehicleId, cancellationToken);
            if (existing is null)
            {
                return null;
            }

            var updatedCandidate = Vehicle.Rehydrate(
                vehicleId: existing.VehicleId,
                vehicleTypeId: command.VehicleTypeId ?? existing.VehicleTypeId,
                vin: Normalize(command.Vin),
                year: command.Year,
                make: NormalizeRequired(command.Make),
                marque: Normalize(command.Marque) ?? existing.Marque,
                model: NormalizeRequired(command.Model),
                trim: Normalize(command.Trim),
                seriesName: Normalize(command.SeriesName),
                bodyStyle: Normalize(command.BodyStyle),
                chassisNumber: Normalize(command.ChassisNumber) ?? existing.ChassisNumber,
                bodyNumber: Normalize(command.BodyNumber) ?? existing.BodyNumber,
                engineNumber: Normalize(command.EngineNumber) ?? existing.EngineNumber,
                engine: Normalize(command.Engine),
                transmission: Normalize(command.Transmission),
                drivetrain: Normalize(command.Drivetrain),
                mileage: command.Mileage,
                odometerUnit: NormalizeRequired(command.OdometerUnit),
                odometerStatus: Normalize(command.OdometerStatus) ?? existing.OdometerStatus,
                exteriorColor: Normalize(command.ExteriorColor),
                interiorColor: Normalize(command.InteriorColor),
                titleStatus: Normalize(command.TitleStatus),
                authenticityProfile: Normalize(command.AuthenticityProfile),
                originalityProfile: Normalize(command.OriginalityProfile) ?? existing.OriginalityProfile,
                restorationLevel: Normalize(command.RestorationLevel),
                documentationLevel: Normalize(command.DocumentationLevel),
                overallConditionScore: command.OverallConditionScore,
                hasRedFlags: command.HasRedFlags,
                notes: NormalizeLarge(command.Notes),
                ownershipNotes: NormalizeLarge(command.OwnershipNotes),
                vehicleStatus: Normalize(command.VehicleStatus) ?? existing.VehicleStatus,
                // rowVersion: existing.RowVersion, // RowVersion property does not exist on Vehicle. Please review the Vehicle entity for concurrency implementation.
                createdAtUtc: existing.CreatedAtUtc,
                updatedAtUtc: DateTime.UtcNow);

            var updated = await _vehicleRepository.UpdateAsync(updatedCandidate, cancellationToken);
            return updated is null ? null : MapToDto(updated);
        }

        public async Task ArchiveAsync(
            Guid vehicleId,
            long expectedRowVersion,
            CancellationToken cancellationToken)
        {
            if (vehicleId == Guid.Empty)
            {
                throw new ArgumentException("VehicleId cannot be empty.", nameof(vehicleId));
            }

            var archived = await _vehicleRepository.ArchiveAsync(vehicleId, cancellationToken);
            if (!archived)
            {
                throw new InvalidOperationException("Failed to archive vehicle.");
            }
        }

        public async Task<PagedResult<VehicleListItemDto>> ListAsync(
            ListVehiclesQuery query,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            var page = GetOptionalInt(query, defaultValue: 1, "Page", "PageNumber");
            var pageSize = GetOptionalInt(query, defaultValue: 25, "PageSize");
            var make = GetOptionalString(query, "Make") ?? string.Empty;
            var model = GetOptionalString(query, "Model") ?? string.Empty;
            var year = GetOptionalNullableInt(query, "Year");
            var titleStatus = GetOptionalString(query, "TitleStatus") ?? string.Empty;

            // The IVehicleRepository interface does not define a ListAsync method. You need to implement this method in IVehicleRepository and its implementation class.
// For now, you must either implement the method or use an alternative approach to retrieve the list of vehicles.
throw new NotImplementedException("ListAsync method is not implemented in IVehicleRepository. Please implement it in the repository and its implementation class.");

            // var dtoItems = items.Select(MapToListItemDto).ToList();
            // 'items' does not exist in the current context. You need to retrieve the list of vehicles from the repository and map them to DTOs.
            // Example (after implementing ListAsync in IVehicleRepository):
            // var items = await _vehicleRepository.ListAsync(query, page, pageSize, cancellationToken);
            // var dtoItems = items.Select(MapToListItemDto).ToList();
            throw new NotImplementedException("You must implement retrieval of items from the repository. 'items' is not defined.");

            // 'totalCount' does not exist in the current context. You need to retrieve the total count of vehicles from the repository.
            // Example (after implementing ListAsync in IVehicleRepository):
            // var (items, totalCount) = await _vehicleRepository.ListAsync(query, page, pageSize, cancellationToken);
            // var dtoItems = items.Select(MapToListItemDto).ToList();
            throw new NotImplementedException("You must implement retrieval of totalCount from the repository. 'totalCount' is not defined.");
        }

        private static VehicleListItemDto MapToListItemDto(Vehicle vehicle)
        {
            return new VehicleListItemDto(
                vehicle.VehicleId,
                vehicle.Make ?? string.Empty,
                vehicle.Model ?? string.Empty,
                vehicle.Year,
                vehicle.TitleStatus,
                vehicle.HasRedFlags,
                0); // vehicle.RowVersion property does not exist. Passing default value.
        }

        private static PagedResult<VehicleListItemDto> CreatePagedResult(
            IReadOnlyList<VehicleListItemDto> items,
            int totalCount,
            int page,
            int pageSize)
        {
            return new PagedResult<VehicleListItemDto>(items, page, pageSize, totalCount);
        }

        private static int GetOptionalInt(object source, int defaultValue, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var property = source.GetType().GetProperty(propertyName);
                if (property is null)
                {
                    continue;
                }

                var value = property.GetValue(source);
                if (value is int intValue)
                {
                    return intValue;
                }
            }

            return defaultValue;
        }

        private static int? GetOptionalNullableInt(object source, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var property = source.GetType().GetProperty(propertyName);
                if (property is null)
                {
                    continue;
                }

                var value = property.GetValue(source);
                if (value is int intValue)
                {
                    return intValue;
                }

                if (value is int nullableIntValue)
                {
                    return nullableIntValue;
                }
            }

            return null;
        }

        private static string? GetOptionalString(object source, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                var property = source.GetType().GetProperty(propertyName);
                if (property is null)
                {
                    continue;
                }

                if (property.GetValue(source) is string value)
                {
                    return Normalize(value);
                }
            }

            return null;
        }

        private static void ValidateCreate(CreateVehicleCommand command)
        {
            if (command.Year < 1886 || command.Year > 2100)
            {
                throw new ArgumentOutOfRangeException(nameof(command.Year), "Year must be between 1886 and 2100.");
            }

            if (string.IsNullOrWhiteSpace(command.Make))
            {
                throw new ArgumentException("Make is required.", nameof(command.Make));
            }

            if (string.IsNullOrWhiteSpace(command.Model))
            {
                throw new ArgumentException("Model is required.", nameof(command.Model));
            }

            if (string.IsNullOrWhiteSpace(command.OdometerUnit))
            {
                throw new ArgumentException("OdometerUnit is required.", nameof(command.OdometerUnit));
            }

            if (command.Mileage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(command.Mileage), "Mileage cannot be negative.");
            }
        }

        private static void ValidateUpdate(UpdateVehicleCommand command)
        {
            if (command.Year < 1886 || command.Year > 2100)
            {
                throw new ArgumentOutOfRangeException(nameof(command.Year), "Year must be between 1886 and 2100.");
            }

            if (string.IsNullOrWhiteSpace(command.Make))
            {
                throw new ArgumentException("Make is required.", nameof(command.Make));
            }

            if (string.IsNullOrWhiteSpace(command.Model))
            {
                throw new ArgumentException("Model is required.", nameof(command.Model));
            }

            if (string.IsNullOrWhiteSpace(command.OdometerUnit))
            {
                throw new ArgumentException("OdometerUnit is required.", nameof(command.OdometerUnit));
            }

            if (command.Mileage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(command.Mileage), "Mileage cannot be negative.");
            }
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                VehicleTypeId = vehicle.VehicleTypeId,
                Vin = vehicle.Vin,
                ChassisNumber = vehicle.ChassisNumber,
                BodyNumber = vehicle.BodyNumber,
                EngineNumber = vehicle.EngineNumber,
                Year = vehicle.Year,
                Make = vehicle.Make,
                Marque = vehicle.Marque,
                Model = vehicle.Model,
                Trim = vehicle.Trim,
                SeriesName = vehicle.SeriesName,
                BodyStyle = vehicle.BodyStyle,
                Drivetrain = vehicle.Drivetrain,
                Engine = vehicle.Engine,
                Transmission = vehicle.Transmission,
                Mileage = vehicle.Mileage,
                OdometerUnit = vehicle.OdometerUnit ?? string.Empty,
                OdometerStatus = vehicle.OdometerStatus,
                ExteriorColor = vehicle.ExteriorColor,
                InteriorColor = vehicle.InteriorColor,
                TitleStatus = vehicle.TitleStatus,
                AuthenticityProfile = vehicle.AuthenticityProfile,
                OriginalityProfile = vehicle.OriginalityProfile,
                RestorationLevel = vehicle.RestorationLevel,
                DocumentationLevel = vehicle.DocumentationLevel,
                OverallConditionScore = vehicle.OverallConditionScore,
                HasRedFlags = vehicle.HasRedFlags,
                VehicleStatus = vehicle.VehicleStatus,
                RowVersion = 0, // vehicle.RowVersion property does not exist. Assigning default value.
                OwnershipNotes = vehicle.OwnershipNotes,
                Notes = vehicle.Notes,
                CreatedAtUtc = vehicle.CreatedAtUtc,
                UpdatedAtUtc = vehicle.UpdatedAtUtc
            };
        }

        private static string? Normalize(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string? NormalizeLarge(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string NormalizeRequired(string value)
        {
            return value.Trim();
        }

        Task<PagedResult<VehicleListItemDto>> IVehicleService.ListAsync(
            ListVehiclesQuery query,
            CancellationToken cancellationToken)
        {
            return ListAsync(query, cancellationToken);
        }
    }
}