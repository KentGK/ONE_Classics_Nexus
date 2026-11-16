using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IAppraisalRepository
{
    Task<Appraisal> InsertAsync(Appraisal appraisal, CancellationToken cancellationToken);
    Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken ct);
    Task<Appraisal?> GetByIdAsync(Guid appraisalId, CancellationToken ct);
    Task<Appraisal?> UpdateAsync(Appraisal appraisal, CancellationToken cancellationToken);
    Task<PagedResult<Appraisal>> BrowseAsync(AppraisalBrowseFilter filter, CancellationToken ct);
    Task<bool> DeleteAsync(Guid appraisalId, CancellationToken ct);
}
