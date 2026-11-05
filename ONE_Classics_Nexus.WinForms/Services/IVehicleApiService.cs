using ONE_Classics_Nexus.API.Contracts.Vehicles;

namespace ONE_Classics_Nexus.WinForms.Services;

public interface IVehicleApiService
{
    Task<IReadOnlyList<VehicleResponse>> ListAsync(
        CancellationToken cancellationToken = default);
    Task<VehicleResponse?> GetByIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
    Task<VehicleResponse> CreateAsync(
        CreateVehicleRequest request,
        CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(
        Guid vehicleId,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
}