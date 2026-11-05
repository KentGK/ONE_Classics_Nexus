using System;
namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public sealed record SubjectVehicle(
    Guid VehicleId,
    int Year,
    string Make,
    string Model,
    string? Trim,
    string? BodyStyle,
    string? Engine,
    string? Transmission,
    string? Drivetrain,
    string? Series,
    decimal? Mileage,
    string? Vin
);