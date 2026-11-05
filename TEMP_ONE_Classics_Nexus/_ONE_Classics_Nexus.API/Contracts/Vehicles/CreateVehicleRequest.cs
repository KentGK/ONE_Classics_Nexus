using System;

namespace ONE_Classics_Nexus.API.Contracts.Vehicles
{
    public sealed class CreateVehicleRequest
    {
        public Guid VehicleTypeId { get; set; }
        public string Vin { get; set; } = default!;
        public int Year { get; set; }
        public string Make { get; set; } = default!;
        public string Model { get; set; } = default!;
        public string? Trim { get; set; }
        public string? SeriesName { get; set; }
        public string? BodyStyle { get; set; }
        public string? Drivetrain { get; set; }
        public string? Engine { get; set; }
        public string? Transmission { get; set; }
        public int? Mileage { get; set; }
        public string? OdometerUnit { get; set; }
        public string? ExteriorColor { get; set; }
        public string? InteriorColor { get; set; }
        public string? TitleStatus { get; set; }
        public string? AuthenticityProfile { get; set; }
        public string? RestorationLevel { get; set; }
        public string? DocumentationLevel { get; set; }
        public decimal? OverallConditionScore { get; set; }
        public bool HasRedFlags { get; set; } = false;
        public string? Notes { get; set; }
        public string? OwnershipNotes { get; set; }
    }
}