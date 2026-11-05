using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IInspectionRepository
{
    Task<Inspection> InsertAsync(Inspection inspection, CancellationToken cancellationToken = default);
    Task<Inspection?> GetByIdAsync(Guid inspectionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Inspection>> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken = default);
    Task<Inspection?> UpdateAsync(Inspection inspection, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid inspectionId, CancellationToken cancellationToken = default);
    Task<Inspection?> UpdateScoreSummaryAsync(
        Guid inspectionId,
        decimal completionPercentage,
        decimal overallConditionScore,
        bool hasRedFlags,
        CancellationToken cancellationToken = default);
}
