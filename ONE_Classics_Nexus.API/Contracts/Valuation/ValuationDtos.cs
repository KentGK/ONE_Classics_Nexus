namespace ONE_Classics_Nexus.API.Contracts.Valuation;

public sealed record RunValuationRequest(
    string? EngineVersion,
    string? RubricVersion,
    string? RulesetVersion,
    string? Notes);

public sealed record ValuationRunResponse(
    Guid ValuationRunId,
    Guid AppraisalId,
    string ValuationStatus,
    string? EngineVersion,
    decimal? ConditionIndex,
    decimal? MarketFactor,
    decimal? RedFlagFactor,
    decimal? ConfidenceScore,
    string? ConfidenceBand,
    decimal? ValueLow,
    decimal? FinalValue,
    decimal? ValueHigh,
    bool ManualReviewRequired,
    string? RunNotes,
    DateTime CreatedAtUtc);

public sealed record ValuationRunListResponse(IReadOnlyList<ValuationRunResponse> Items);

public sealed record ValuationRunDetailResponse(
    ValuationRunResponse Run,
    IReadOnlyList<TraceLineDto> TraceLines);

public sealed record TraceLineDto(
    Guid ValuationTraceLineId,
    Guid ValuationRunId,
    int SortOrder,
    string TraceType,
    string TraceKey,
    decimal? Amount,
    decimal? Factor,
    string Message,
    string? EvidenceRef,
    DateTime CreatedAtUtc);
