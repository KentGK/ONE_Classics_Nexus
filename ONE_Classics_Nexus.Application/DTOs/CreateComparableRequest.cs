namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record CreateComparableRequest(
    int Year,
    string Make,
    string Model,
    string? Trim,
    string Source,
    DateOnly SaleDate,
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