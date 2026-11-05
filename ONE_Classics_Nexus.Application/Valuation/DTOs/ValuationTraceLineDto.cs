namespace ONE_Classics_Nexus.Application.Valuation.DTOs;

public sealed record ValuationTraceLineDto(
    int SortOrder,
    string Type,
    string Key,
    decimal? Amount,
    decimal? Factor,
    string Message,
    string? EvidenceRef
);