using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.API.Services;

public sealed class VehicleApiService(
    IVehicleRepository vehicleRepository,
    IVehicleTypeRepository vehicleTypeRepository) : IVehicleApiService
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository = vehicleTypeRepository;

    private static VehicleResponse MapToResponse(Vehicle vehicle)
    {
        return new VehicleResponse
        {
            VehicleId = vehicle.VehicleId,
            VehicleTypeId = vehicle.VehicleTypeId,
            RowVersion = vehicle.RowVersion,
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
            Engine = vehicle.Engine,
            Transmission = vehicle.Transmission,
            Drivetrain = vehicle.Drivetrain,
            Mileage = vehicle.Mileage,
            OdometerUnit = vehicle.OdometerUnit,
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
            OwnershipNotes = vehicle.OwnershipNotes,
            Notes = vehicle.Notes,
            CreatedAtUtc = vehicle.CreatedAtUtc,
            UpdatedAtUtc = vehicle.UpdatedAtUtc
        };
    }

    public async Task<VehicleResponse?> GetByIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(
            vehicleId,
            cancellationToken);

        return vehicle is null ? null : MapToResponse(vehicle);
    }

    // Replace direct property sets with a new Vehicle instance using the Rehydrate method
    public async Task<bool> UpdateAsync(
        Guid vehicleId,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedVehicleTypeId = request.VehicleTypeId == Guid.Empty
            ? null
            : request.VehicleTypeId;

        if (request.RowVersion <= 0)
        {
            throw new ValidationException("RowVersion is required.");
        }

        if (normalizedVehicleTypeId.HasValue)
        {
            var updateVehicleTypeExists = await _vehicleTypeRepository.ExistsByIdAsync(normalizedVehicleTypeId.Value, cancellationToken);
            if (!updateVehicleTypeExists)
            {
                throw new ValidationException("VehicleTypeId is invalid.");
            }
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

        if (vehicle is null)
        {
            return false;
        }

        var updatedVehicle = Vehicle.Rehydrate(
            vehicle.VehicleId,
            normalizedVehicleTypeId,
            request.Vin,
            request.Year,
            request.Make ?? string.Empty,
            request.Marque,
            request.Model ?? string.Empty,
            request.Trim,
            request.SeriesName,
            request.BodyStyle,
            request.ChassisNumber,
            request.BodyNumber,
            request.EngineNumber,
            request.Engine,
            request.Transmission,
            request.Drivetrain,
            request.Mileage,
            string.IsNullOrWhiteSpace(request.OdometerUnit) ? vehicle.OdometerUnit ?? "miles" : request.OdometerUnit,
            request.OdometerStatus,
            request.ExteriorColor,
            request.InteriorColor,
            request.TitleStatus,
            request.AuthenticityProfile,
            request.OriginalityProfile,
            request.RestorationLevel,
            request.DocumentationLevel,
            request.OverallConditionScore,
            request.HasRedFlags,
            request.Notes,
            request.OwnershipNotes,
            string.IsNullOrWhiteSpace(request.VehicleStatus) ? vehicle.VehicleStatus : request.VehicleStatus,
            request.RowVersion,
            vehicle.CreatedAtUtc,
            DateTime.UtcNow
        );

        var result = await _vehicleRepository.UpdateAsync(updatedVehicle, cancellationToken);

        return result is not null;
    }

    public async Task<VehicleResponse> CreateAsync(
        CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedVehicleTypeId = request.VehicleTypeId == Guid.Empty
            ? null
            : request.VehicleTypeId;

        if (normalizedVehicleTypeId.HasValue)
        {
            var createVehicleTypeExists = await _vehicleTypeRepository.ExistsByIdAsync(normalizedVehicleTypeId.Value, cancellationToken);
            if (!createVehicleTypeExists)
            {
                throw new ValidationException("VehicleTypeId is invalid.");
            }
        }

        // Normalize VIN for checks and storage
        var normalizedVin = request.Vin is null ? string.Empty : request.Vin.Trim().ToUpperInvariant();

        // Early existence check to provide friendly 409 response (race conditions still handled at DB level)
        if (!string.IsNullOrWhiteSpace(normalizedVin))
        {
            // Fallback: ListAsync to check for VIN existence
            var (items, _) = await _vehicleRepository.ListAsync(
                page: 1,
                pageSize: 1,
                make: string.Empty,
                model: string.Empty,
                year: null,
                titleStatus: string.Empty,
                cancellationToken: cancellationToken);

            var exists = items.Any(v => v.Vin != null && v.Vin.Equals(normalizedVin, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                throw new ConflictException($"A vehicle with VIN '{normalizedVin}' already exists.");
            }
        }

        var now = DateTime.UtcNow;

        var vehicle = Vehicle.Create(
            normalizedVehicleTypeId,
            normalizedVin,
            request.Year,
            request.Make ?? string.Empty,
            request.Marque,
            request.Model ?? string.Empty,
            request.Trim,
            request.SeriesName,
            request.BodyStyle,
            request.ChassisNumber,
            request.BodyNumber,
            request.EngineNumber,
            request.Engine,
            request.Transmission,
            request.Drivetrain,
            request.Mileage,
            request.OdometerUnit,
            request.OdometerStatus,
            request.ExteriorColor,
            request.InteriorColor,
            request.TitleStatus,
            request.AuthenticityProfile,
            request.OriginalityProfile,
            request.RestorationLevel,
            request.DocumentationLevel,
            request.OverallConditionScore,
            request.HasRedFlags,
            request.Notes,
            request.OwnershipNotes,
            now
        );

        if (!string.IsNullOrWhiteSpace(request.VehicleStatus))
        {
            vehicle.ChangeStatus(request.VehicleStatus, now);
        }

        var updatedVehicle = await _vehicleRepository.CreateAsync(
            vehicle,
            cancellationToken);

        if (updatedVehicle is null)
        {
            throw new InvalidOperationException("Failed to create vehicle.");
        }

        return MapToResponse(updatedVehicle);
    }
}