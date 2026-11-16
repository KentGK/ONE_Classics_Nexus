using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IInspectionRepository
{
    Task<Inspection> CreateAsync(Inspection inspection, CancellationToken ct);
    Task<Inspection?> GetByIdAsync(Guid inspectionId, CancellationToken ct);
    Task<IReadOnlyList<Inspection>> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken ct);
    Task<IReadOnlyList<Inspection>> GetAllAsync(CancellationToken cancellationToken);
    Task<Inspection?> UpdateAsync(Inspection inspection, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid inspectionId, CancellationToken cancellationToken);
    Task<Inspection?> UpdateScoreSummaryAsync(
        Guid inspectionId,
        decimal completionPercentage,
        decimal overallConditionScore,
        bool hasRedFlags,
        CancellationToken cancellationToken);
    Task SaveScoresAsync(Guid inspectionId, IReadOnlyList<ConditionScore> scores, CancellationToken ct);
}
