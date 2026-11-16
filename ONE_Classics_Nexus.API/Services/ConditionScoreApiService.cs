using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.ConditionScores;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.API.Services;

public sealed class ConditionScoreApiService : IConditionScoreApiService
{
    private readonly IInspectionRepository _inspectionRepository;

    public ConditionScoreApiService(IInspectionRepository inspectionRepository)
    {
        _inspectionRepository = inspectionRepository;
    }

    public async Task<ConditionScoreListResponse?> GetByInspectionIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(
            inspectionId,
            cancellationToken);

        if (inspection is null)
            return null;

        return new ConditionScoreListResponse
        {
            Scores = inspection.ConditionScores
                .Select(MapToResponse)
                .ToList()
        };
    }

    public async Task<ConditionScoreResponse?> GetByIdAsync(
        Guid inspectionId,
        Guid scoreId,
        CancellationToken cancellationToken)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(
            inspectionId,
            cancellationToken);

        if (inspection is null)
            return null;

        var score = inspection.ConditionScores
            .FirstOrDefault(s => s.ConditionScoreId == scoreId);

        return score is null ? null : MapToResponse(score);
    }

    public async Task<ConditionScoreResponse> CreateAsync(
        Guid inspectionId,
        CreateConditionScoreRequest request,
        CancellationToken cancellationToken)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(
            inspectionId,
            cancellationToken);

        if (inspection is null)
            throw new InvalidOperationException($"Inspection with ID {inspectionId} not found.");

        var score = ConditionScore.Create(
            inspectionId,
            request.SystemNumber,
            request.SystemKey,
            request.Score0To5,
            request.Weight,
            request.IsRedFlag,
            request.Notes);

        inspection.ConditionScores.Add(score);
        await _inspectionRepository.SaveScoresAsync(
            inspectionId,
            inspection.ConditionScores,
            cancellationToken);

        return MapToResponse(score);
    }

    public async Task<bool> UpdateAsync(
        Guid inspectionId,
        Guid scoreId,
        UpdateConditionScoreRequest request,
        CancellationToken cancellationToken)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(
            inspectionId,
            cancellationToken);

        if (inspection is null)
            return false;

        var score = inspection.ConditionScores
            .FirstOrDefault(s => s.ConditionScoreId == scoreId);

        if (score is null)
            return false;

        score.Update(
            request.Score0To5,
            request.Weight,
            request.IsRedFlag,
            request.Notes);

        await _inspectionRepository.SaveScoresAsync(
            inspectionId,
            inspection.ConditionScores,
            cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        Guid inspectionId,
        Guid scoreId,
        CancellationToken cancellationToken)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(
            inspectionId,
            cancellationToken);

        if (inspection is null)
            return false;

        var score = inspection.ConditionScores
            .FirstOrDefault(s => s.ConditionScoreId == scoreId);

        if (score is null)
            return false;

        inspection.ConditionScores.Remove(score);
        await _inspectionRepository.SaveScoresAsync(
            inspectionId,
            inspection.ConditionScores,
            cancellationToken);

        return true;
    }

    private static ConditionScoreResponse MapToResponse(ConditionScore score)
    {
        return new ConditionScoreResponse
        {
            ConditionScoreId = score.ConditionScoreId,
            InspectionId = score.InspectionId,
            SystemNumber = score.SystemNumber,
            SystemKey = score.SystemKey,
            Score0To5 = score.Score0To5,
            Weight = score.Weight,
            IsRedFlag = score.IsRedFlag,
            Notes = score.Notes,
            CreatedAtUtc = score.CreatedAtUtc,
            UpdatedAtUtc = score.UpdatedAtUtc
        };
    }
}
