using ONE_Classics_Nexus.API.Contracts.Inspections;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IInspectionApiService
{
    Task<InspectionResponse> CreateAsync(
        CreateInspectionRequest request,
        CancellationToken cancellationToken);

    Task<InspectionResponse?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken);

    Task<InspectionListResponse> GetByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid inspectionId,
        CancellationToken cancellationToken);

    Task<InspectionResponse?> SaveScoresAsync(
        Guid inspectionId,
        SaveInspectionScoresRequest request,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<InspectionResponse>> GetAllAsync(
        CancellationToken cancellationToken);
}