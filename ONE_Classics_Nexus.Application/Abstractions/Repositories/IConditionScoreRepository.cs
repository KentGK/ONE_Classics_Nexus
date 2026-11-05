using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IConditionScoreRepository
{
    Task<IReadOnlyList<ConditionScore>> GetByInspectionIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default);

    Task ReplaceScoresAsync(
        Guid inspectionId,
        IReadOnlyList<ConditionScore> scores,
        CancellationToken cancellationToken = default);
}
