using ONE_Classics_Nexus.Application.Contracts.Vehicles;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
namespace ONE_Classics_Nexus.Application.Abstractions.Services
{
    public interface IVehicleService
    {
        Task<VehicleDto> CreateAsync(
            CreateVehicleCommand command, 
            CancellationToken cancellationToken = default);
        Task<VehicleDto?> GetByIdAsync(
            Guid vehicleId, 
            CancellationToken cancellationToken = default);
        Task<VehicleDto?> UpdateAsync(
            UpdateVehicleCommand command, 
            CancellationToken cancellationToken = default);
        Task ArchiveAsync(
            Guid vehicleId,
            long expectedRowVersion,
            CancellationToken cancellationToken);
        Task<PagedResult<VehicleListItemDto>> ListAsync(
            ListVehiclesQuery query,
            CancellationToken cancellationToken);
    }
}