namespace ONE_Classics_Nexus.Application.Contracts.Vehicles
{
    public sealed class VehicleDto
    {
        public Guid VehicleId { get; init; }
        public Guid? VehicleTypeId { get; init; }
        public string? Vin { get; init; }
        public string? ChassisNumber { get; init; }
        public string? BodyNumber { get; init; }
        public string? EngineNumber { get; init; }
        public int Year { get; init; }
        public string Make { get; init; } = string.Empty;
        public string? Marque { get; init; }
        public string Model { get; init; } = string.Empty;
        public string? Trim { get; init; }
        public string? SeriesName { get; init; }
        public string? BodyStyle { get; init; }
        public string? Drivetrain { get; init; }
        public string? Engine { get; init; }
        public string? Transmission { get; init; }
        public int? Mileage { get; init; }
        public string OdometerUnit { get; init; } = "miles";
        public string? OdometerStatus { get; init; }
        public string? ExteriorColor { get; init; }
        public string? InteriorColor { get; init; }
        public string? TitleStatus { get; init; }
        public string? AuthenticityProfile { get; init; }
        public string? OriginalityProfile { get; init; }
        public string? RestorationLevel { get; init; }
        public string? DocumentationLevel { get; init; }
        public decimal? OverallConditionScore { get; init; }
        public bool HasRedFlags { get; init; }
        public string VehicleStatus { get; init; } = string.Empty;
        public long RowVersion { get; init; }
        public string? OwnershipNotes { get; init; }
        public string? Notes { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public DateTime UpdatedAtUtc { get; init; }
    }
}