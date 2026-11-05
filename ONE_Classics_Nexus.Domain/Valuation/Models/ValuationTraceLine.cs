namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public sealed record ValuationTraceLine(
    int SortOrder,
    string Type,
    string Key,
    decimal? Amount,
    decimal? Factor,
    string Message,
    string? EvidenceRef
);