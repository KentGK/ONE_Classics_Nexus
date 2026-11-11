namespace ONE_Classics_Nexus.Domain.Valuation.Models;
using System.Collections.Generic;

public sealed record ValuationInput(
    SubjectVehicle Subject,
    IReadOnlyList<ConditionScore> ConditionScores,
    IReadOnlyList<ComparableSale> Comparables,
    string SegmentCode,
    string EngineVersion,
    string RulesetVersion,
    string RubricVersion
);