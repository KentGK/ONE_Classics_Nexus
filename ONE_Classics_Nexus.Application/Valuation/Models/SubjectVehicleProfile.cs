namespace ONE_Classics_Nexus.Application.Valuation.Models;

public sealed record SubjectVehicleProfile(
    Guid AppraisalId,
    int Year,
    string Make,
    string Model,
    string? Trim,
    string? BodyStyle,
    string? Drivetrain,
    string? Engine,
    string? Transmission,
    int? Mileage,
    string? OriginalityLevel,
    string? RestorationLevel,
    string? TitleStatus,
    decimal? OverallConditionScore,
    bool HasRedFlags
);