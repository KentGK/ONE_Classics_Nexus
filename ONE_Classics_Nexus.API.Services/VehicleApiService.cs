using System;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.API.Services;

public sealed class VehicleApiService(IVehicleRepository vehicleRepository) : IVehicleApiService
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

    private static VehicleResponse MapToResponse(Vehicle vehicle)
    {
        return new VehicleResponse
        {
            VehicleId = vehicle.VehicleId,
            VehicleTypeId = vehicle.VehicleTypeId,
            Vin = vehicle.Vin ?? string.Empty,
            Year = vehicle.Year,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Trim = vehicle.Trim,
            SeriesName = vehicle.SeriesName,
            BodyStyle = vehicle.BodyStyle,
            Drivetrain = vehicle.Drivetrain,
            Engine = vehicle.Engine,
            Transmission = vehicle.Transmission,
            Mileage = vehicle.Mileage,
            OdometerUnit = vehicle.OdometerUnit,
            ExteriorColor = vehicle.ExteriorColor,
            InteriorColor = vehicle.InteriorColor,
            TitleStatus = vehicle.TitleStatus,
            AuthenticityProfile = vehicle.AuthenticityProfile,
            RestorationLevel = vehicle.RestorationLevel,
            DocumentationLevel = vehicle.DocumentationLevel,
            OverallConditionScore = vehicle.OverallConditionScore,
            HasRedFlags = vehicle.HasRedFlags,
            Notes = vehicle.Notes,
            OwnershipNotes = vehicle.OwnershipNotes,
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

        if (vehicle is null)
            return null;

        return MapToResponse(vehicle);
    }

    public async Task<bool> UpdateAsync(
        Guid vehicleId,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(
            vehicleId,
            cancellationToken);

        if (vehicle is null)
        {
            return false;
        }

        vehicle.VehicleTypeId = request.VehicleTypeId;
        vehicle.Vin = request.Vin;
        vehicle.Year = request.Year;
        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Trim = request.Trim;
        vehicle.SeriesName = request.SeriesName;
        vehicle.BodyStyle = request.BodyStyle;
        vehicle.Engine = request.Engine;
        vehicle.Transmission = request.Transmission;
        vehicle.Drivetrain = request.Drivetrain;
        vehicle.Mileage = request.Mileage;
        vehicle.OdometerUnit = request.OdometerUnit;
        vehicle.ExteriorColor = request.ExteriorColor;
        vehicle.InteriorColor = request.InteriorColor;
        vehicle.TitleStatus = request.TitleStatus;
        vehicle.AuthenticityProfile = request.AuthenticityProfile;
        vehicle.RestorationLevel = request.RestorationLevel;
        vehicle.DocumentationLevel = request.DocumentationLevel;
        vehicle.OverallConditionScore = request.OverallConditionScore;
        vehicle.HasRedFlags = request.HasRedFlags;
        vehicle.Notes = request.Notes;
        vehicle.OwnershipNotes = request.OwnershipNotes;
        vehicle.UpdatedAtUtc = DateTime.UtcNow;

        var updated = await _vehicleRepository.UpdateAsync(
            vehicle,
            cancellationToken);

        return updated is not null;
    }

    public async Task<VehicleResponse> CreateAsync(
        CreateVehicleRequest request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = new Vehicle
        {
            VehicleId = Guid.NewGuid(),
            VehicleTypeId = request.VehicleTypeId,
            Vin = request.Vin,
            Year = request.Year,
            Make = request.Make,
            Model = request.Model,
            Trim = request.Trim,
            SeriesName = request.SeriesName,
            BodyStyle = request.BodyStyle,
            Engine = request.Engine,
            Transmission = request.Transmission,
            Drivetrain = request.Drivetrain,
            Mileage = request.Mileage,
            OdometerUnit = request.OdometerUnit,
            ExteriorColor = request.ExteriorColor,
            InteriorColor = request.InteriorColor,
            TitleStatus = request.TitleStatus,
            AuthenticityProfile = request.AuthenticityProfile,
            RestorationLevel = request.RestorationLevel,
            DocumentationLevel = request.DocumentationLevel,
            OverallConditionScore = request.OverallConditionScore,
            HasRedFlags = request.HasRedFlags,
            Notes = request.Notes,
            OwnershipNotes = request.OwnershipNotes,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        var createdVehicle = await _vehicleRepository.InsertAsync(
            vehicle,
            cancellationToken);

        if (createdVehicle is null)
        {
            throw new InvalidOperationException("Failed to create vehicle.");
        }

        return MapToResponse(createdVehicle);
    }
}