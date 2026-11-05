using ONE_Classics_Nexus.API.Contracts.Valuation;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IValuationApiService
{
    Task<ValuationRunResponse> RunAsync(
        Guid appraisalId,
        RunValuationRequest request,
        CancellationToken cancellationToken);

    Task<ValuationRunListResponse> GetRunsByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);

    Task<ValuationRunDetailResponse?> GetRunDetailAsync(
        Guid valuationRunId,
        CancellationToken cancellationToken);
}
