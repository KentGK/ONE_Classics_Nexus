using System;

namespace ONE_Classics_Nexus.WinForms.Models.Ui
{
    public sealed class VehicleResponseDto
    {
        public Guid VehicleId { get; set; }
        public string? Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Trim { get; set; }
        public string? SeriesName { get; set; }
        public string? BodyStyle { get; set; }
        public string? Drivetrain { get; set; }
        public string? Engine { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
        public string OdometerUnit { get; set; } = "miles";
        public string? ExteriorColor { get; set; }
        public string? InteriorColor { get; set; }
        public string? TitleStatus { get; set; }
        public string? AuthenticityProfile { get; set; }
        public string? RestorationLevel { get; set; }
        public string? DocumentationLevel { get; set; }
        public decimal? OverallConditionScore { get; set; }
        public bool HasRedFlags { get; set; }
        public string? OwnershipSummary { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }

    public sealed class CreateVehicleRequestDto
    {
        public string? Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Trim { get; set; }
        public string? SeriesName { get; set; }
        public string? BodyStyle { get; set; }
        public string? Drivetrain { get; set; }
        public string? Engine { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
        public string OdometerUnit { get; set; } = "miles";
        public string? ExteriorColor { get; set; }
        public string? InteriorColor { get; set; }
        public string? TitleStatus { get; set; }
        public string? AuthenticityProfile { get; set; }
        public string? RestorationLevel { get; set; }
        public string? DocumentationLevel { get; set; }
        public decimal? OverallConditionScore { get; set; }
        public bool HasRedFlags { get; set; }
        public string? OwnershipSummary { get; set; }
        public string? Notes { get; set; }
    }

    public sealed class UpdateVehicleRequestDto
    {
        public string? Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Trim { get; set; }
        public string? SeriesName { get; set; }
        public string? BodyStyle { get; set; }
        public string? Drivetrain { get; set; }
        public string? Engine { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
        public string OdometerUnit { get; set; } = "miles";
        public string? ExteriorColor { get; set; }
        public string? InteriorColor { get; set; }
        public string? TitleStatus { get; set; }
        public string? AuthenticityProfile { get; set; }
        public string? RestorationLevel { get; set; }
        public string? DocumentationLevel { get; set; }
        public decimal? OverallConditionScore { get; set; }
        public bool HasRedFlags { get; set; }
        public string? OwnershipSummary { get; set; }
        public string? Notes { get; set; }
    }
}