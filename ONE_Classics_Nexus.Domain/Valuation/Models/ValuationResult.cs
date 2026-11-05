namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public sealed record ValuationResult(
    ValuationStatus Status,
    decimal FinalValue,
    decimal LowValue,
    decimal HighValue,
    decimal Confidence0To1,
    string EngineVersion,
    string RubricVersion,
    string RulesetVersion,
    IReadOnlyList<ValuationTraceLine> Trace
);