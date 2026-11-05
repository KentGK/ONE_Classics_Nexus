namespace ONE_Classics_Nexus.API.Contracts.Comparables;

public sealed record CreateComparableRequest(
    int? Year,
    string Make,
    string Model,
    string? Trim,
    string? BodyStyle,
    string? Engine,
    string? Transmission,
    string? Drivetrain,
    decimal? SalePrice,
    decimal? AskingPrice,
    DateTime? SaleDate,
    string? Source,
    string? SourceListingUrl,
    int? Mileage,
    decimal? OverallConditionScore,
    string? ConditionSummary,
    decimal? AuthenticityScore,
    bool HasRedFlags,
    string? Notes);

public sealed record UpdateComparableRequest(
    int? Year,
    string Make,
    string Model,
    string? Trim,
    string? BodyStyle,
    string? Engine,
    string? Transmission,
    string? Drivetrain,
    decimal? SalePrice,
    decimal? AskingPrice,
    DateTime? SaleDate,
    string? Source,
    string? SourceListingUrl,
    int? Mileage,
    decimal? OverallConditionScore,
    string? ConditionSummary,
    decimal? AuthenticityScore,
    bool HasRedFlags,
    string? Notes);

public sealed record ComparableResponse(
    Guid ComparableId,
    Guid AppraisalId,
    int? Year,
    string Make,
    string Model,
    string? Trim,
    decimal? SalePrice,
    decimal? AskingPrice,
    DateTime? SaleDate,
    string? Source,
    decimal? SimilarityScore,
    string? Notes);

public sealed record ComparableListResponse(IReadOnlyList<ComparableResponse> Items);
