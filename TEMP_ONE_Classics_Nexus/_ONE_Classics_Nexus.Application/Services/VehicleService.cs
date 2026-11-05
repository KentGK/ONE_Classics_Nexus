using System;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Application.Mappings;

namespace ONE_Classics_Nexus.Application.Services;

public sealed class VehicleService
{
    private readonly IVehicleRepository _repository;

    public VehicleService(IVehicleRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<VehicleDto?> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(vehicleId, cancellationToken);
        return entity is null ? null : entity.ToDto();
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var now = DateTime.UtcNow;
        var entity = new Vehicle
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
            Drivetrain = request.Drivetrain,
            Engine = request.Engine,
            Transmission = request.Transmission,
            Mileage = request.Mileage,
            OdometerUnit = request.OdometerUnit ?? "miles",
            ExteriorColor = request.ExteriorColor,
            InteriorColor = request.InteriorColor,
            TitleStatus = request.TitleStatus,
            AuthenticityProfile = request.AuthenticityProfile,
            RestorationLevel = request.RestorationLevel,
            DocumentationLevel = request.DocumentationLevel,
            OverallConditionScore = request.OverallConditionScore,
            HasRedFlags = request.HasRedFlags,
            Notes = request.Notes,
            // domain property is OwnershipSummary, DB column is ownership_notes
            OwnershipSummary = request.OwnershipNotes,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        // Use InsertAsync which persists the full column set (matches vehicles SQL)
        var created = await _repository.InsertAsync(entity, cancellationToken)
                      ?? throw new InvalidOperationException("Failed to create vehicle.");

        return created.ToDto();
    }
}