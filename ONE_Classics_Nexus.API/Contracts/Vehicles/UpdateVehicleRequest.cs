using System.ComponentModel.DataAnnotations;

namespace ONE_Classics_Nexus.API.Contracts.Vehicles
{
    public sealed class UpdateVehicleRequest
    {
        [StringLength(50)]
        public string? Vin { get; init; }

        [Range(1886, 2100)]
        public int Year { get; init; }

        [Required]
        [StringLength(100)]
        public string Make { get; init; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Model { get; init; } = string.Empty;

        [StringLength(100)]
        public string? Trim { get; init; }

        [StringLength(100)]
        public string? SeriesName { get; init; }

        [StringLength(100)]
        public string? BodyStyle { get; init; }

        [StringLength(100)]
        public string? Drivetrain { get; init; }

        [StringLength(150)]
        public string? Engine { get; init; }

        [StringLength(150)]
        public string? Transmission { get; init; }

        [Range(0, 10000000)]
        public int? Mileage { get; init; }

        [Required]
        [StringLength(20)]
        public string OdometerUnit { get; init; } = "miles";

        [StringLength(100)]
        public string? ExteriorColor { get; init; }

        [StringLength(100)]
        public string? InteriorColor { get; init; }

        [StringLength(50)]
        public string? TitleStatus { get; init; }

        [StringLength(50)]
        public string? AuthenticityProfile { get; init; }

        [StringLength(50)]
        public string? RestorationLevel { get; init; }

        [StringLength(50)]
        public string? DocumentationLevel { get; init; }

        public decimal? OverallConditionScore { get; init; }

        public bool HasRedFlags { get; init; }

        public string? OwnershipSummary { get; init; }

        public string? Notes { get; init; }
    }
}