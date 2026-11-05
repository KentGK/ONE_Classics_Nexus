using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Exceptions;

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class MergeVehicleHandler
{
    private readonly IVehicleRepository _vehicleRepository;

    public MergeVehicleHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    /// <summary>
    /// Merges a source vehicle into a target vehicle by reassigning linked records and marking the source as merged.
    /// </summary>
    public async Task HandleAsync(
        Guid sourceVehicleId,
        Guid targetVehicleId,
        CancellationToken cancellationToken)
    {
        if (sourceVehicleId == Guid.Empty)
        {
            throw new ArgumentException("Source vehicle ID is required.", nameof(sourceVehicleId));
        }

        if (targetVehicleId == Guid.Empty)
        {
            throw new ArgumentException("Target vehicle ID is required.", nameof(targetVehicleId));
        }

        if (sourceVehicleId == targetVehicleId)
        {
            throw new ArgumentException("Source and target vehicle IDs must be different.", nameof(targetVehicleId));
        }

        var sourceVehicle = await _vehicleRepository.GetByIdAsync(sourceVehicleId, cancellationToken);
        if (sourceVehicle is null)
        {
            throw new NotFoundException($"Vehicle '{sourceVehicleId}' was not found.");
        }

        var targetVehicle = await _vehicleRepository.GetByIdAsync(targetVehicleId, cancellationToken);
        if (targetVehicle is null)
        {
            throw new NotFoundException($"Target vehicle '{targetVehicleId}' was not found.");
        }

        var merged = await _vehicleRepository.MergeAsync(sourceVehicleId, targetVehicleId, cancellationToken);
        if (!merged)
        {
            throw new NotFoundException($"Vehicle '{sourceVehicleId}' was not found.");
        }
    }
}
