using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Mappings;

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class GetVehicleHandler
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleDto> HandleAsync(Guid vehicleId, CancellationToken ct)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, ct);

        if (vehicle is null)
            throw new NotFoundException($"Vehicle '{vehicleId}' was not found.");

        return vehicle.ToDto();
    }
}
