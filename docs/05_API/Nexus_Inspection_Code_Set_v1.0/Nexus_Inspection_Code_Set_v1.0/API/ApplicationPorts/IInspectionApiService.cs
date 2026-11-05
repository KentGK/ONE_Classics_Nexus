using ONE_Classics_Nexus.API.Contracts.Inspections;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IInspectionApiService
{
    Task<IReadOnlyList<InspectionResponse>> GetAllAsync(
        CancellationToken 
        cancellationToken = default);

    Task<InspectionResponse?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default);

    Task<InspectionResponse> CreateAsync(
        CreateInspectionRequest request, 
        CancellationToken cancellationToken = default);

    Task<bool> SaveScoresAsync(
        Guid inspectionId, 
        SaveInspectionScoresRequest request, 
        CancellationToken cancellationToken = default);
}
