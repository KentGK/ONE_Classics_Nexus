using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Inspections;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.API.Services;

public sealed class InspectionApiService : IInspectionApiService
{
    private static readonly IReadOnlyDictionary<int, string> CanonicalSystemKeys =
        new Dictionary<int, string>
        {
            [1] = "body_exterior",
            [2] = "frame_structure",
            [3] = "powertrain",
            [4] = "transmission_driveline",
            [5] = "suspension_steering",
            [6] = "braking_system",
            [7] = "electrical_system",
            [8] = "interior",
            [9] = "wheels_tires",
            [10] = "trim_glass_details",
            [11] = "documentation_provenance",
            [12] = "road_test_functional"
        };

    private readonly IInspectionRepository _inspectionRepository;
    private readonly IConditionScoreRepository _conditionScoreRepository;

    public InspectionApiService(
        IInspectionRepository inspectionRepository,
        IConditionScoreRepository conditionScoreRepository)
    {
        _inspectionRepository = inspectionRepository;
        _conditionScoreRepository = conditionScoreRepository;
    }

    public async Task<InspectionResponse> CreateAsync(
        CreateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        ValidateCreate(request);

        var inspection = Inspection.Create(
            request.AppraisalId,
            request.InspectionTemplateId,
            request.InspectionType,
            request.InspectedByUserId,
            request.Status,
            request.InspectionLocation,
            request.SummaryNotes);

        var inserted = await _inspectionRepository.InsertAsync(inspection, cancellationToken);

        return await MapToResponseAsync(inserted, cancellationToken);
    }

    public async Task<InspectionResponse?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            throw new ArgumentException("InspectionId cannot be empty.", nameof(inspectionId));

        var inspection = await _inspectionRepository.GetByIdAsync(inspectionId, cancellationToken);

        return inspection is null
            ? null
            : await MapToResponseAsync(inspection, cancellationToken);
    }

    public async Task<InspectionListResponse> GetByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        if (appraisalId == Guid.Empty)
            throw new ArgumentException("AppraisalId cannot be empty.", nameof(appraisalId));

        var inspections = await _inspectionRepository.GetByAppraisalIdAsync(appraisalId, cancellationToken);
        var responses = new List<InspectionResponse>(inspections.Count);

        foreach (var inspection in inspections)
        {
            responses.Add(await MapToResponseAsync(inspection, cancellationToken));
        }

        return new InspectionListResponse { Items = responses };
    }

    public async Task<bool> UpdateAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            throw new ArgumentException("InspectionId cannot be empty.", nameof(inspectionId));

        ValidateUpdate(request);

        var existing = await _inspectionRepository.GetByIdAsync(inspectionId, cancellationToken);
        if (existing is null)
            return false;

        existing.InspectionTemplateId = request.InspectionTemplateId;
        existing.InspectedByUserId = request.InspectedByUserId;
        existing.InspectionType = request.InspectionType.Trim();
        existing.Status = request.Status.Trim();
        existing.InspectionLocation = Normalize(request.InspectionLocation);
        existing.InspectedOnUtc = request.InspectedOnUtc;
        existing.SummaryNotes = Normalize(request.SummaryNotes);
        existing.RowVersion = request.RowVersion;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        var updated = await _inspectionRepository.UpdateAsync(existing, cancellationToken);

        return updated is not null;
    }

    public Task<bool> DeleteAsync(Guid inspectionId, CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            throw new ArgumentException("InspectionId cannot be empty.", nameof(inspectionId));

        return _inspectionRepository.DeleteAsync(inspectionId, cancellationToken);
    }

    public async Task<InspectionResponse?> SaveScoresAsync(
        Guid inspectionId,
        SaveInspectionScoresRequest request,
        CancellationToken cancellationToken)
    {
        if (inspectionId == Guid.Empty)
            throw new ArgumentException("InspectionId cannot be empty.", nameof(inspectionId));

        var inspection = await _inspectionRepository.GetByIdAsync(inspectionId, cancellationToken);
        if (inspection is null)
            return null;

        ValidateScores(request);

        var now = DateTime.UtcNow;
        var scores = request.Scores
            .OrderBy(x => x.SystemNumber)
            .Select(score => new ConditionScore
            {
                ConditionScoreId = Guid.NewGuid(),
                InspectionId = inspectionId,
                SystemNumber = score.SystemNumber,
                SystemKey = score.SystemKey.Trim(),
                Score0To5 = score.Score0To5,
                Weight = score.Weight,
                IsRedFlag = score.IsRedFlag,
                Notes = Normalize(score.Notes),
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            })
            .ToArray();

        await _conditionScoreRepository.ReplaceScoresAsync(
            inspectionId,
            scores,
            cancellationToken);

        var completionPercentage = Math.Round(scores.Length / 12m * 100m, 2, MidpointRounding.AwayFromZero);
        var overallConditionScore = Math.Round(scores.Sum(x => x.Score0To5 * x.Weight) * 20m, 2, MidpointRounding.AwayFromZero);
        var hasRedFlags = scores.Any(x => x.IsRedFlag);

        var updatedInspection = await _inspectionRepository.UpdateScoreSummaryAsync(
            inspectionId,
            completionPercentage,
            overallConditionScore,
            hasRedFlags,
            cancellationToken);

        if (updatedInspection is null)
            return null;

        return await MapToResponseAsync(updatedInspection, cancellationToken);
    }

    public async Task<IReadOnlyList<InspectionResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var inspections = await _inspectionRepository.GetAllAsync(cancellationToken);
        var responses = new List<InspectionResponse>(inspections.Count);

        foreach (var inspection in inspections)
        {
            responses.Add(await MapToResponseAsync(inspection, cancellationToken));
        }

        return responses;
    }

    private async Task<InspectionResponse> MapToResponseAsync(
        Inspection inspection,
        CancellationToken cancellationToken)
    {
        var scores = await _conditionScoreRepository.GetByInspectionIdAsync(
            inspection.InspectionId,
            cancellationToken);

        return new InspectionResponse
        {
            InspectionId = inspection.InspectionId,
            AppraisalId = inspection.AppraisalId,
            InspectionTemplateId = inspection.InspectionTemplateId,
            InspectedByUserId = inspection.InspectedByUserId,
            InspectionType = inspection.InspectionType,
            Status = inspection.Status,
            InspectionLocation = inspection.InspectionLocation,
            InspectedOnUtc = inspection.InspectedOnUtc,
            CompletionPercentage = inspection.CompletionPercentage,
            OverallConditionScore = inspection.OverallConditionScore,
            HasRedFlags = inspection.HasRedFlags,
            SummaryNotes = inspection.SummaryNotes,
            RowVersion = inspection.RowVersion,
            CreatedAtUtc = inspection.CreatedAtUtc,
            UpdatedAtUtc = inspection.UpdatedAtUtc,
            Scores = scores
                .OrderBy(x => x.SystemNumber)
                .Select(x => new InspectionScoreDto(
                    x.SystemNumber,
                    x.SystemKey,
                    x.Score0To5,
                    x.Weight,
                    x.IsRedFlag,
                    x.Notes))
                .ToArray()
        };
    }

    private static void ValidateCreate(CreateInspectionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.AppraisalId == Guid.Empty)
            throw new ArgumentException("AppraisalId is required.", nameof(request.AppraisalId));

        if (request.InspectionTemplateId == Guid.Empty)
            throw new ArgumentException("InspectionTemplateId is required.", nameof(request.InspectionTemplateId));

        ValidateInspectionType(request.InspectionType);
        ValidateStatus(request.Status);
    }

    private static void ValidateUpdate(UpdateInspectionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.InspectionTemplateId == Guid.Empty)
            throw new ArgumentException("InspectionTemplateId is required.", nameof(request.InspectionTemplateId));

        ValidateInspectionType(request.InspectionType);
        ValidateStatus(request.Status);
    }

    private static void ValidateScores(SaveInspectionScoresRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Scores is null)
            throw new ArgumentException("Scores are required.", nameof(request.Scores));

        if (request.Scores.Count != 12)
            throw new ArgumentException("Exactly 12 condition scores are required.", nameof(request.Scores));

        var seen = new HashSet<int>();

        foreach (var score in request.Scores)
        {
            if (score.SystemNumber < 1 || score.SystemNumber > 12)
                throw new ArgumentOutOfRangeException(nameof(score.SystemNumber), "SystemNumber must be between 1 and 12.");

            if (!seen.Add(score.SystemNumber))
                throw new ArgumentException($"Duplicate system_number found: {score.SystemNumber}", nameof(request.Scores));

            if (!CanonicalSystemKeys.TryGetValue(score.SystemNumber, out var expectedKey))
                throw new ArgumentException($"Invalid system_number: {score.SystemNumber}", nameof(request.Scores));

            if (!string.Equals(score.SystemKey, expectedKey, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"SystemKey for system {score.SystemNumber} must be '{expectedKey}'.", nameof(request.Scores));

            if (score.Score0To5 < 0m || score.Score0To5 > 5m)
                throw new ArgumentOutOfRangeException(nameof(score.Score0To5), "Score0To5 must be between 0.00 and 5.00.");

            if (score.Weight < 0m || score.Weight > 1m)
                throw new ArgumentOutOfRangeException(nameof(score.Weight), "Weight must be between 0.0000 and 1.0000.");
        }
    }

    private static void ValidateInspectionType(string inspectionType)
    {
        if (string.IsNullOrWhiteSpace(inspectionType))
            throw new ArgumentException("InspectionType is required.", nameof(inspectionType));

        var allowed = new[] { "Standard", "Custom", "VintageClassic" };
        if (!allowed.Contains(inspectionType.Trim(), StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("InspectionType must be Standard, Custom, or VintageClassic.", nameof(inspectionType));
    }

    private static void ValidateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status is required.", nameof(status));

        var allowed = new[] { "Draft", "Scheduled", "InProgress", "PendingReview", "Completed", "Cancelled", "Superseded" };
        if (!allowed.Contains(status.Trim(), StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid inspection status.", nameof(status));
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

