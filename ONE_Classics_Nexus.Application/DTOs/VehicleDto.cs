using System;

namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record VehicleDto(
    Guid VehicleId,
    string Vin,
    int Year,
    string Make,
    string Model,
    string? Trim,
    string? BodyStyle,
    string? Engine,
    string? Transmission,
    string? Drivetrain,
    string? TitleStatus,
    DateTime CreatedAtUtc);