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

        inspection.AddConditionScore(score);

        // Save scores through repository
        await _inspectionRepository.UpdateAsync(
            inspection,
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

        await _inspectionRepository.UpdateAsync(
            inspection,
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

        // Remove score and save
        var scoreToRemove = inspection.ConditionScores.FirstOrDefault(s => s.ConditionScoreId == scoreId);
        if (scoreToRemove != null)
        {
            // Use the existing AddConditionScore pattern, but for removal, access the private list via reflection or, preferably, add a Remove method to Inspection.
            // Since you cannot access the private list directly, you should add a RemoveConditionScore method to the Inspection class.
            // For now, you can remove the score from the collection if you have access, or request to add such a method to the domain model.
            // Here is a placeholder for the correct domain-driven approach:
            inspection.RemoveConditionScore(scoreToRemove);
        }

        await _inspectionRepository.UpdateAsync(
            inspection,
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