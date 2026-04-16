namespace ONE_Classics_Nexus.Application.Valuation.Models;

public sealed record ComparableInput(
    Guid ComparableId,
    int Year,
    string Make,
    string Model,
    string? Trim,
    string Source,
    DateTime SaleDate,
    decimal SalePrice,
    string? BodyStyle,
    string? Drivetrain,
    string? Engine,
    string? Transmission,
    int? Mileage,
    string? OriginalityLevel,
    string? RestorationLevel,
    string? TitleStatus,
    decimal? OverallConditionScore,
    bool HasRedFlags,
    string? Notes
);