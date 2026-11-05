using ONE_Classics_Nexus.Application.Valuation.DTOs;
using ONE_Classics_Nexus.Application.Valuation.Interfaces;
using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Domain.Valuation.Versioning;

namespace ONE_Classics_Nexus.Application.Valuation.Services;

/// <summary>
/// Application service responsible for orchestrating valuation execution.
/// </summary>
public sealed class ValuationApplicationService : IValuationApplicationService
{
    private readonly IAppraisalQueryService _queryService;
    private readonly IValuationEngine _valuationEngine;
    private readonly IValuationRunRepository _repository;

    public ValuationApplicationService(
        IAppraisalQueryService queryService,
        IValuationEngine valuationEngine,
        IValuationRunRepository repository)
    {
        _queryService = queryService;
        _valuationEngine = valuationEngine;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<ValuationResultDto> RunAsync(
        Guid appraisalId,
        CancellationToken ct)
    {
        SubjectVehicle subject = await _queryService.GetSubjectVehicleAsync(appraisalId, ct);
        IReadOnlyList<ConditionScore> conditionScores = await _queryService.GetConditionScoresAsync(appraisalId, ct);
        IReadOnlyList<ComparableSale> comparables = await _queryService.GetComparableSalesAsync(appraisalId, ct);

        if (subject is null)
        {
            throw new InvalidOperationException($"Subject vehicle not found for appraisal {appraisalId}.");
        }

        conditionScores ??= Array.Empty<ConditionScore>();
        comparables ??= Array.Empty<ComparableSale>();

        var input = new ValuationInput(
            Subject: subject,
            ConditionScores: conditionScores,
            Comparables: comparables,
            SegmentCode: "default",
            EngineVersion: ValuationVersions.EngineVersion,
            RulesetVersion: ValuationVersions.RulesetVersion,
            RubricVersion: ValuationVersions.RubricVersion);

        ValuationResult result = _valuationEngine.Run(input);

        await _repository.SaveAsync(appraisalId, result, ct);

        return new ValuationResultDto(
            Status: result.Status,
            FinalValue: result.FinalValue,
            LowValue: result.LowValue,
            HighValue: result.HighValue,
            Confidence0To1: result.Confidence0To1,
            EngineVersion: result.EngineVersion,
            RubricVersion: result.RubricVersion,
            RulesetVersion: result.RulesetVersion,
            Trace: result.Trace
                .OrderBy(x => x.SortOrder)
                .Select(x => new ValuationTraceLineDto(
                    SortOrder: x.SortOrder,
                    Type: x.Type,
                    Key: x.Key,
                    Amount: x.Amount,
                    Factor: x.Factor,
                    Message: x.Message,
                    EvidenceRef: x.EvidenceRef))
                .ToList());
    }
}