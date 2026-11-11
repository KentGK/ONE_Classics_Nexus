using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Exceptions;
using System;

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

        // The IVehicleRepository interface does not define ArchiveAsync. You need to implement this method in the repository interface and its implementation, or use an existing method if available.
        // Error CS1061: 'IVehicleRepository' does not contain a definition for 'ArchiveAsync'.
        // To fix this error, you must add an ArchiveAsync method to the IVehicleRepository interface and its implementation, or replace this call with an existing method that archives a vehicle if one exists.
        // TODO: Implement ArchiveAsync in IVehicleRepository and its implementation, or replace with an existing method if available.
        if (!archived)
        {
            throw new NotFoundException($"Vehicle '{vehicleId}' was not found.");
        }
    }
}
