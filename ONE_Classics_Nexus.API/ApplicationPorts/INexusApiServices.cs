using ONE_Classics_Nexus.API.Contracts.Appraisals;
using ONE_Classics_Nexus.API.Contracts.Vehicles;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

// Adapter ports used by the API controllers. Implement these in the Application layer,
// or create thin adapters over existing handlers/repositories.
public interface IVehicleApiService
{
    Task<VehicleResponse> CreateAsync(
        CreateVehicleRequest request,
        CancellationToken cancellationToken);
    Task<VehicleResponse?> GetByIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken);
    Task<bool> UpdateAsync(
        Guid vehicleId,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken);
}

public interface IAppraisalApiService
{
    Task<AppraisalResponse> CreateAsync(
        CreateAppraisalRequest request,
        CancellationToken cancellationToken);
    Task<AppraisalResponse?> GetByIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);
    Task<AppraisalResponse?> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalRequest request,
        CancellationToken cancellationToken);
    Task<AppraisalListResponse> BrowseAsync(
        BrowseAppraisalsRequest request,
        CancellationToken cancellationToken);
}
