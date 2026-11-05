using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Application.Valuation.Interfaces;

public interface IAppraisalQueryService
{
    Task<SubjectVehicle> GetSubjectVehicleAsync(Guid appraisalId, CancellationToken ct);
    Task<IReadOnlyList<ConditionScore>> GetConditionScoresAsync(Guid appraisalId, CancellationToken ct);
    Task<IReadOnlyList<ComparableSale>> GetComparableSalesAsync(Guid appraisalId, CancellationToken ct);
}