using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Exceptions;

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class ArchiveVehicleHandler
{
    private readonly IVehicleRepository _vehicleRepository;

    public ArchiveVehicleHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    /// <summary>
    /// Archives a vehicle by transitioning its status to Archived.
    /// </summary>
    public async Task HandleAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        if (vehicleId == Guid.Empty)
        {
            throw new ArgumentException("Vehicle ID is required.", nameof(vehicleId));
        }

        var archived = await _vehicleRepository.ArchiveAsync(vehicleId, cancellationToken);
        if (!archived)
        {
            throw new NotFoundException($"Vehicle '{vehicleId}' was not found.");
        }
    }
}
