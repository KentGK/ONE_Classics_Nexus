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

            var vehicle = new Vehicle
            {
                VehicleId = Guid.NewGuid(),
                Vin = Normalize(command.Vin),
                Year = command.Year,
                Make = NormalizeRequired(command.Make),
                Model = NormalizeRequired(command.Model),
                Trim = Normalize(command.Trim),
                SeriesName = Normalize(command.SeriesName),
                BodyStyle = Normalize(command.BodyStyle),
                Drivetrain = Normalize(command.Drivetrain),
                Engine = Normalize(command.Engine),
                Transmission = Normalize(command.Transmission),
                Mileage = command.Mileage,
                OdometerUnit = NormalizeRequired(command.OdometerUnit),
                ExteriorColor = Normalize(command.ExteriorColor),
                InteriorColor = Normalize(command.InteriorColor),
                TitleStatus = Normalize(command.TitleStatus),
                AuthenticityProfile = Normalize(command.AuthenticityProfile),
                RestorationLevel = Normalize(command.RestorationLevel),
                DocumentationLevel = Normalize(command.DocumentationLevel),
                OverallConditionScore = command.OverallConditionScore,
                HasRedFlags = command.HasRedFlags,
                OwnershipSummary = NormalizeLarge(command.OwnershipSummary),
                Notes = NormalizeLarge(command.Notes),
                CreatedAtUtc = utcNow,
                UpdatedAtUtc = utcNow
            };

            var inserted = await _vehicleRepository.InsertAsync(vehicle, cancellationToken);

            return MapToDto(inserted);
        }

        public async Task<VehicleDto?> GetByIdAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("VehicleId cannot be empty.", nameof(vehicleId));

            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

            return vehicle is null ? null : MapToDto(vehicle);
        }

        public async Task<VehicleDto?> UpdateAsync(
            UpdateVehicleCommand command,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (command.VehicleId == Guid.Empty)
                throw new ArgumentException("VehicleId cannot be empty.", nameof(command.VehicleId));

            ValidateUpdate(command);

            var existing = await _vehicleRepository.GetByIdAsync(command.VehicleId, cancellationToken);
            if (existing is null)
                return null;

            existing.Vin = Normalize(command.Vin);
            existing.Year = command.Year;
            existing.Make = NormalizeRequired(command.Make);
            existing.Model = NormalizeRequired(command.Model);
            existing.Trim = Normalize(command.Trim);
            existing.SeriesName = Normalize(command.SeriesName);
            existing.BodyStyle = Normalize(command.BodyStyle);
            existing.Drivetrain = Normalize(command.Drivetrain);
            existing.Engine = Normalize(command.Engine);
            existing.Transmission = Normalize(command.Transmission);
            existing.Mileage = command.Mileage;
            existing.OdometerUnit = NormalizeRequired(command.OdometerUnit);
            existing.ExteriorColor = Normalize(command.ExteriorColor);
            existing.InteriorColor = Normalize(command.InteriorColor);
            existing.TitleStatus = Normalize(command.TitleStatus);
            existing.AuthenticityProfile = Normalize(command.AuthenticityProfile);
            existing.RestorationLevel = Normalize(command.RestorationLevel);
            existing.DocumentationLevel = Normalize(command.DocumentationLevel);
            existing.OverallConditionScore = command.OverallConditionScore;
            existing.HasRedFlags = command.HasRedFlags;
            existing.OwnershipSummary = NormalizeLarge(command.OwnershipSummary);
            existing.Notes = NormalizeLarge(command.Notes);
            existing.UpdatedAtUtc = DateTime.UtcNow;

            var updated = await _vehicleRepository.UpdateAsync(existing, cancellationToken);
            if (updated is null)
                return null;

            return MapToDto(updated);
        }

        private static void ValidateCreate(CreateVehicleCommand command)
        {
            if (command.Year < 1886 || command.Year > 2100)
                throw new ArgumentOutOfRangeException(nameof(command.Year), "Year must be between 1886 and 2100.");

            if (string.IsNullOrWhiteSpace(command.Make))
                throw new ArgumentException("Make is required.", nameof(command.Make));

            if (string.IsNullOrWhiteSpace(command.Model))
                throw new ArgumentException("Model is required.", nameof(command.Model));

            if (string.IsNullOrWhiteSpace(command.OdometerUnit))
                throw new ArgumentException("OdometerUnit is required.", nameof(command.OdometerUnit));

            if (command.Mileage < 0)
                throw new ArgumentOutOfRangeException(nameof(command.Mileage), "Mileage cannot be negative.");
        }

        private static void ValidateUpdate(UpdateVehicleCommand command)
        {
            if (command.Year < 1886 || command.Year > 2100)
                throw new ArgumentOutOfRangeException(nameof(command.Year), "Year must be between 1886 and 2100.");

            if (string.IsNullOrWhiteSpace(command.Make))
                throw new ArgumentException("Make is required.", nameof(command.Make));

            if (string.IsNullOrWhiteSpace(command.Model))
                throw new ArgumentException("Model is required.", nameof(command.Model));

            if (string.IsNullOrWhiteSpace(command.OdometerUnit))
                throw new ArgumentException("OdometerUnit is required.", nameof(command.OdometerUnit));

            if (command.Mileage < 0)
                throw new ArgumentOutOfRangeException(nameof(command.Mileage), "Mileage cannot be negative.");
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                Vin = vehicle.Vin,
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
                OwnershipSummary = vehicle.OwnershipSummary,
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
    }
}