using ONE_Classics_Nexus.API.Contracts.Comparables;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IComparableApiService
{
    Task<ComparableResponse> CreateAsync(Guid appraisalId, CreateComparableRequest request, CancellationToken cancellationToken);

    Task<ComparableListResponse> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken);

    Task<bool> UpdateAsync(Guid comparableId, UpdateComparableRequest request, CancellationToken cancellationToken);
}