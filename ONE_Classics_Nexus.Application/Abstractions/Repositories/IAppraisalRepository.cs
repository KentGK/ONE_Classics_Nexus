using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IAppraisalRepository
{
    Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken ct);
    Task<Appraisal?> GetByIdAsync(Guid appraisalId, CancellationToken ct);
    Task<PagedResult<Appraisal>> BrowseAsync(AppraisalBrowseFilter filter, CancellationToken ct);
}