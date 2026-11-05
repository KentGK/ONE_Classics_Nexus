namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record VehicleSummaryDto(
    Guid VehicleId,
    string Vin,
    int Year,
    string Make,
    string Model,
    string? Trim,
    string? BodyStyle,
    string? TitleStatus);