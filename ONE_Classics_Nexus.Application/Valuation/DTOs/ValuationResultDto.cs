namespace ONE_Classics_Nexus.Application.Valuation.DTOs;

using ONE_Classics_Nexus.Domain.Valuation.Models;
using System.Collections.Generic;

public sealed record ValuationResultDto(
    ValuationStatus Status,
    decimal FinalValue,
    decimal LowValue,
    decimal HighValue,
    decimal Confidence0To1,
    string EngineVersion,
    string RubricVersion,
    string RulesetVersion,
    IReadOnlyList<ValuationTraceLineDto> Trace
);