namespace ONE_Classics_Nexus.Domain.Valuation.Models;

/// <summary>
/// Represents a single adjustment applied to a comparable sale.
///
/// Each adjustment modifies the comparable's sale price to better align it
/// with the subject vehicle being appraised. Adjustments are cumulative and
/// are used to derive the final adjusted comparable value.
/// </summary>
/// <param name="CategoryKey">
/// A machine-readable category identifier for the adjustment.
/// Examples:
/// - "time"
/// - "mileage"
/// - "condition" (future)
/// - "originality" (future)
/// - "equipment" (future)
/// </param>
/// <param name="Amount">
/// The signed monetary adjustment applied to the comparable.
/// 
/// Positive value:
///   Increases the comparable price (subject is superior)
/// 
/// Negative value:
///   Decreases the comparable price (subject is inferior)
/// </param>
/// <param name="Reason">
/// A human-readable explanation of why the adjustment was applied.
/// This is critical for auditability and report generation.
/// </param>
/// <param name="EvidenceRef">
/// Optional supporting reference for the adjustment.
/// Examples:
/// - URL to listing
/// - internal note reference
/// - external data source
/// </param>
public sealed record ComparableAdjustment(
    string CategoryKey,
    decimal Amount,
    string Reason,
    string? EvidenceRef
);