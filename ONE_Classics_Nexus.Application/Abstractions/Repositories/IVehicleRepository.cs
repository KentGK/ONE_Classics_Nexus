using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> InsertAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
        Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
        Task<Vehicle?> UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    }
}