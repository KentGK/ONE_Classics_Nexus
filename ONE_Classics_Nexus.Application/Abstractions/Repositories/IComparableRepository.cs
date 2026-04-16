using ONE_Classics_Nexus.Application.DTOs;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IComparableRepository
{
    Task<ComparableDto> CreateAsync(Guid appraisalId, CreateComparableRequest request, CancellationToken ct);
    Task<IReadOnlyList<ComparableDto>> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken ct);
}