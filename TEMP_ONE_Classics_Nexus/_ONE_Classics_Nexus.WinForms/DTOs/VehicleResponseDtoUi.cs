using System;

namespace ONE_Classics_Nexus.WinForms.DTOs;

public sealed class VehicleResponseDtoUi
{
    public Guid VehicleId { get; init; }
    public Guid VehicleTypeId { get; init; }
    public string Vin { get; init; } = string.Empty;
    public int Year { get; init; }
    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string? Trim { get; init; }
    public string? SeriesName { get; init; }
    public string? BodyStyle { get; init; }
    public string? Drivetrain { get; init; }
    public string? Engine { get; init; }
    public string? Transmission { get; init; }
    public int? Mileage { get; init; }
    public string? OdometerUnit { get; init; }
    public string? ExteriorColor { get; init; }
    public string? InteriorColor { get; init; }
    public string? TitleStatus { get; init; }
    public string? AuthenticityProfile { get; init; }
    public string? RestorationLevel { get; init; }
    public string? DocumentationLevel { get; init; }
    public decimal? OverallConditionScore { get; init; }
    public bool HasRedFlags { get; init; }
    public string? Notes { get; init; }
    public string? OwnershipNotes { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }

    // Optional convenience factory to create UI DTO from application DTO or domain model
    public static VehicleResponseDtoUi FromApplicationDto(ONE_Classics_Nexus.Application.DTOs.VehicleDto dto) =>
        new()
        {
            VehicleId = dto.VehicleId,
            VehicleTypeId = dto.VehicleTypeId,
            Vin = dto.Vin,
            Year = dto.Year,
            Make = dto.Make,
            Model = dto.Model,
            Trim = dto.Trim,
            SeriesName = dto.SeriesName,
            BodyStyle = dto.BodyStyle,
            Drivetrain = dto.Drivetrain,
            Engine = dto.Engine,
            Transmission = dto.Transmission,
            Mileage = dto.Mileage,
            OdometerUnit = dto.OdometerUnit,
            ExteriorColor = dto.ExteriorColor,
            InteriorColor = dto.InteriorColor,
            TitleStatus = dto.TitleStatus,
            AuthenticityProfile = dto.AuthenticityProfile,
            RestorationLevel = dto.RestorationLevel,
            DocumentationLevel = dto.DocumentationLevel,
            OverallConditionScore = dto.OverallConditionScore,
            HasRedFlags = dto.HasRedFlags,
            Notes = dto.Notes,
            OwnershipNotes = dto.OwnershipNotes,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc
        };
}