using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Valuation;
using System.Collections.Concurrent;

namespace ONE_Classics_Nexus.API.Services;

// NOTE: This is a minimal in-memory implementation to unblock compilation.
// Replace with a real implementation backed by an IValuationRunRepository
// (following the same pattern as IAppraisalRepository / IInspectionRepository)
// once the ValuationRun domain entity and persistence are added.
public sealed class ValuationApiService : IValuationApiService
{
    private readonly ConcurrentDictionary<Guid, List<ValuationRunResponse>> _byAppraisal = new();
    private readonly ConcurrentDictionary<Guid, ValuationRunResponse> _byRunId = new();

    public Task<ValuationRunResponse> RunAsync(
        Guid appraisalId,
        RunValuationRequest request,
        CancellationToken cancellationToken)
    {
        var response = new ValuationRunResponse(
            Guid.NewGuid(),
            appraisalId,
            "Completed",
            request.EngineVersion,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            request.Notes,
            DateTime.UtcNow);

        var list = _byAppraisal.GetOrAdd(appraisalId, _ => new List<ValuationRunResponse>());
        list.Add(response);
        _byRunId[response.ValuationRunId] = response;

        return Task.FromResult(response);
    }

    public Task<ValuationRunListResponse> GetRunsByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        var items = _byAppraisal.TryGetValue(appraisalId, out var list)
            ? list.AsReadOnly()
            : (IReadOnlyList<ValuationRunResponse>)Array.Empty<ValuationRunResponse>();

        return Task.FromResult(new ValuationRunListResponse(items));
    }

    public Task<ValuationRunDetailResponse?> GetRunDetailAsync(
        Guid valuationRunId,
        CancellationToken cancellationToken)
    {
        if (!_byRunId.TryGetValue(valuationRunId, out var run))
        {
            return Task.FromResult<ValuationRunDetailResponse?>(null);
        }

        return Task.FromResult<ValuationRunDetailResponse?>(
            new ValuationRunDetailResponse(run, Array.Empty<TraceLineDto>()));
    }
}
