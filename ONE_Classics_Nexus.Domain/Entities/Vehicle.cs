using System;

namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class Vehicle
{
    public Guid VehicleId { get; set; }
    public string Vin { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public string? TitleStatus { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public static Vehicle Create(
        string vin,
        int year,
        string make,
        string model,
        string? trim = null,
        string? bodyStyle = null,
        string? engine = null,
        string? transmission = null,
        string? drivetrain = null,
        string? titleStatus = null)
    {
        if (string.IsNullOrWhiteSpace(vin))
            throw new ArgumentException("VIN is required.", nameof(vin));

        if (year < 1886 || year > DateTime.UtcNow.Year + 1)
            throw new ArgumentOutOfRangeException(nameof(year), "Year is out of valid range.");

        if (string.IsNullOrWhiteSpace(make))
            throw new ArgumentException("Make is required.", nameof(make));

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.", nameof(model));

        return new Vehicle
        {
            VehicleId = Guid.NewGuid(),
            Vin = vin.Trim(),
            Year = year,
            Make = make.Trim(),
            Model = model.Trim(),
            Trim = Normalize(trim),
            BodyStyle = Normalize(bodyStyle),
            Engine = Normalize(engine),
            Transmission = Normalize(transmission),
            Drivetrain = Normalize(drivetrain),
            TitleStatus = Normalize(titleStatus),
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}