using System;
using System.ComponentModel.DataAnnotations;

namespace ONE_Classics_Nexus.Application.Requests;

public sealed class CreateVehicleRequest
{
    // DB: vehicle_type_id CHAR(36) NOT NULL
    [Required]
    public Guid VehicleTypeId { get; set; }

    // DB: vin VARCHAR(50) NOT NULL
    [Required, MaxLength(50)]
    public string Vin { get; set; } = string.Empty;

    // DB: year INT NOT NULL
    [Required]
    public int Year { get; set; }

    // DB: make VARCHAR(100) NOT NULL
    [Required, MaxLength(100)]
    public string Make { get; set; } = string.Empty;

    // DB: model VARCHAR(100) NOT NULL
    [Required, MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    // Optional / nullable DB columns
    [MaxLength(100)]
    public string? Trim { get; set; }

    [MaxLength(100)]
    public string? SeriesName { get; set; }

    [MaxLength(100)]
    public string? BodyStyle { get; set; }

    [MaxLength(100)]
    public string? Drivetrain { get; set; }

    [MaxLength(150)]
    public string? Engine { get; set; }

    [MaxLength(150)]
    public string? Transmission { get; set; }

    public int? Mileage { get; set; }

    [MaxLength(20)]
    public string? OdometerUnit { get; set; } = "miles";

    [MaxLength(100)]
    public string? ExteriorColor { get; set; }

    [MaxLength(100)]
    public string? InteriorColor { get; set; }

    [MaxLength(50)]
    public string? TitleStatus { get; set; }

    [MaxLength(100)]
    public string? AuthenticityProfile { get; set; }

    [MaxLength(100)]
    public string? RestorationLevel { get; set; }

    [MaxLength(100)]
    public string? DocumentationLevel { get; set; }

    public decimal? OverallConditionScore { get; set; }

    public bool HasRedFlags { get; set; }

    public string? Notes { get; set; }

    // DB: ownership_notes TEXT
    public string? OwnershipNotes { get; set; }
}