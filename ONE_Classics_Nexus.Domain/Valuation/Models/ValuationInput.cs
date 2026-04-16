using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public sealed record ValuationInput(
    SubjectVehicle Subject,
    IReadOnlyList<ConditionScore> ConditionScores,
    IReadOnlyList<ComparableSale> Comparables,
    string SegmentCode,
    string EngineVersion,
    string RulesetVersion,
    string RubricVersion
);