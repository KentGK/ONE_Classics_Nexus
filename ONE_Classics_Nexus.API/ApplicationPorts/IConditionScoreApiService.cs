using ONE_Classics_Nexus.API.Contracts.ConditionScores;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IConditionScoreApiService
{
    Task<ConditionScoreListResponse?> GetByInspectionIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken);

    Task<ConditionScoreResponse?> GetByIdAsync(
        Guid inspectionId,
        Guid scoreId,
        CancellationToken cancellationToken);

    Task<ConditionScoreResponse> CreateAsync(
        Guid inspectionId,
        CreateConditionScoreRequest request,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        Guid inspectionId,
        Guid scoreId,
        UpdateConditionScoreRequest request,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid inspectionId,
        Guid scoreId,
        CancellationToken cancellationToken);
}