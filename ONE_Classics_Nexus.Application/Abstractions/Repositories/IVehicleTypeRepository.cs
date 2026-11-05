namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IVehicleTypeRepository
{
    Task<bool> ExistsByIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default);
    Task<VehicleTypeRecord?> GetByIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VehicleTypeRecord>> ListActiveAsync(CancellationToken cancellationToken = default);
}

public sealed record VehicleTypeRecord(
    Guid VehicleTypeId,
    string TypeCode,
    string TypeName,
    string? Description,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
