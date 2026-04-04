using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class CreateVehicleHandler
{
    private readonly IVehicleRepository _vehicleRepository;

    public CreateVehicleHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleDto> HandleAsync(CreateVehicleRequest request, CancellationToken ct)
    {
        Validate(request);

        var vehicle = Vehicle.Create(
            request.Vin,
            request.Year,
            request.Make,
            request.Model,
            request.Trim,
            request.BodyStyle,
            request.Engine,
            request.Transmission,
            request.Drivetrain,
            request.TitleStatus);

        try
        {
            var created = await _vehicleRepository.CreateAsync(vehicle, ct);
            return created.ToDto();
        }
        catch (InvalidOperationException ex)
        {
            throw new ConflictException(ex.Message);
        }
    }

    private static void Validate(CreateVehicleRequest request)
    {
        if (request is null)
            throw new ValidationException("Request is required.");

        if (string.IsNullOrWhiteSpace(request.Vin))
            throw new ValidationException("VIN is required.");

        if (request.Year < 1886 || request.Year > DateTime.UtcNow.Year + 1)
            throw new ValidationException("Year is out of range.");

        if (string.IsNullOrWhiteSpace(request.Make))
            throw new ValidationException("Make is required.");

        if (string.IsNullOrWhiteSpace(request.Model))
            throw new ValidationException("Model is required.");
    }
}