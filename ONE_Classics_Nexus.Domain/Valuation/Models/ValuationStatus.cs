namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public enum ValuationStatus
{
    Pending = 0,
    Success = 1,
    InsufficientData = 2,
    NeedsReview = 3,
    Failed = 4,
    Error = 5
}