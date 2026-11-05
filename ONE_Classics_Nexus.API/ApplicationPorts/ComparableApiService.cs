using ONE_Classics_Nexus.API.Contracts.Comparables;
using System.Collections.Concurrent;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

// NOTE: This is a minimal in-memory implementation to unblock compilation.
// Replace with a real implementation backed by an IComparableRepository
// (following the same pattern as IAppraisalRepository / IInspectionRepository)
// once the Comparable domain entity and persistence are added.
public sealed class ComparableApiService : IComparableApiService
{
    private readonly ConcurrentDictionary<Guid, List<ComparableResponse>> _byAppraisal = new();
    private readonly ConcurrentDictionary<Guid, Guid> _appraisalIdByComparableId = new();

    public Task<ComparableResponse> CreateAsync(Guid appraisalId, CreateComparableRequest request, CancellationToken cancellationToken)
    {
        var response = new ComparableResponse(
            Guid.NewGuid(),
            appraisalId,
            request.Year,
            request.Make,
            request.Model,
            request.Trim,
            request.SalePrice,
            request.AskingPrice,
            request.SaleDate,
            request.Source,
            null,
            request.Notes);

        var list = _byAppraisal.GetOrAdd(appraisalId, _ => new List<ComparableResponse>());
        list.Add(response);
        _appraisalIdByComparableId[response.ComparableId] = appraisalId;

        return Task.FromResult(response);
    }

    public Task<ComparableListResponse> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken)
    {
        var items = _byAppraisal.TryGetValue(appraisalId, out var list)
            ? list.AsReadOnly()
            : (IReadOnlyList<ComparableResponse>)Array.Empty<ComparableResponse>();

        return Task.FromResult(new ComparableListResponse(items));
    }

    public Task<bool> UpdateAsync(Guid comparableId, UpdateComparableRequest request, CancellationToken cancellationToken)
    {
        if (!_appraisalIdByComparableId.TryGetValue(comparableId, out var appraisalId)
            || !_byAppraisal.TryGetValue(appraisalId, out var list))
        {
            return Task.FromResult(false);
        }

        var index = list.FindIndex(c => c.ComparableId == comparableId);
        if (index < 0) return Task.FromResult(false);

        list[index] = list[index] with
        {
            Year = request.Year,
            Make = request.Make,
            Model = request.Model,
            Trim = request.Trim,
            SalePrice = request.SalePrice,
            AskingPrice = request.AskingPrice,
            SaleDate = request.SaleDate,
            Source = request.Source,
            Notes = request.Notes
        };

        return Task.FromResult(true);
    }
}