namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public sealed record ConditionScore(
    string SystemKey,
    decimal Score0To10,
    decimal Weight0To1,
    bool IsRedFlag,
    string? Notes
);