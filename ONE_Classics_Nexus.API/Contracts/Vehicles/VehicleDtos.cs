namespace ONE_Classics_Nexus.API.Contracts.Vehicles;

public sealed class CreateVehicleRequest
{
    public Guid? VehicleTypeId { get; set; }

    public string? Vin { get; set; }

    public string? ChassisNumber { get; set; }

    public string? BodyNumber { get; set; }

    public string? EngineNumber { get; set; }

    public int Year { get; set; }

    public string Make { get; set; } = string.Empty;

    public string? Marque { get; set; }

    public string Model { get; set; } = string.Empty;

    public string? Trim { get; set; }

    public string? SeriesName { get; set; }

    public string? BodyStyle { get; set; }

    public string? Drivetrain { get; set; }

    public string? Engine { get; set; }

    public string? Transmission { get; set; }

    public int? Mileage { get; set; }

    public string? OdometerUnit { get; set; }

    public string? OdometerStatus { get; set; }

    public string? ExteriorColor { get; set; }

    public string? InteriorColor { get; set; }

    public string? TitleStatus { get; set; }

    public string? AuthenticityProfile { get; set; }

    public string? OriginalityProfile { get; set; }

    public string? RestorationLevel { get; set; }

    public string? DocumentationLevel { get; set; }

    public decimal? OverallConditionScore { get; set; }

    public bool HasRedFlags { get; set; }

    public string? Notes { get; set; }

    public string? OwnershipNotes { get; set; }

    public string? VehicleStatus { get; set; }
}

public sealed class UpdateVehicleRequest
{
    public long RowVersion { get; set; }

    public Guid? VehicleTypeId { get; set; }

    public string? Vin { get; set; }

    public string? ChassisNumber { get; set; }

    public string? BodyNumber { get; set; }

    public string? EngineNumber { get; set; }

    public int Year { get; set; }

    public string Make { get; set; } = string.Empty;

    public string? Marque { get; set; }

    public string Model { get; set; } = string.Empty;

    public string? Trim { get; set; }

    public string? SeriesName { get; set; }

    public string? BodyStyle { get; set; }

    public string? Drivetrain { get; set; }

    public string? Engine { get; set; }

    public string? Transmission { get; set; }

    public int? Mileage { get; set; }

    public string? OdometerUnit { get; set; }

    public string? OdometerStatus { get; set; }

    public string? ExteriorColor { get; set; }

    public string? InteriorColor { get; set; }

    public string? TitleStatus { get; set; }

    public string? AuthenticityProfile { get; set; }

    public string? OriginalityProfile { get; set; }

    public string? RestorationLevel { get; set; }

    public string? DocumentationLevel { get; set; }

    public decimal? OverallConditionScore { get; set; }

    public bool HasRedFlags { get; set; }

    public string? VehicleStatus { get; set; }

    public string? Notes { get; set; }

    public string? OwnershipNotes { get; set; }
}

public sealed class MergeVehicleRequest
{
    public Guid TargetVehicleId { get; set; }
}

public sealed class VehicleResponse
{
    public Guid VehicleId { get; set; }

    public Guid? VehicleTypeId { get; set; }

    public string? Vin { get; set; }

    public string? ChassisNumber { get; set; }

    public string? BodyNumber { get; set; }

    public string? EngineNumber { get; set; }

    public int Year { get; set; }

    public string Make { get; set; } = string.Empty;

    public string? Marque { get; set; }

    public string Model { get; set; } = string.Empty;

    public string? Trim { get; set; }

    public string? SeriesName { get; set; }

    public string? BodyStyle { get; set; }

    public string? Drivetrain { get; set; }

    public string? Engine { get; set; }

    public string? Transmission { get; set; }

    public int? Mileage { get; set; }

    public string? OdometerUnit { get; set; }

    public string? OdometerStatus { get; set; }

    public string? ExteriorColor { get; set; }

    public string? InteriorColor { get; set; }

    public string? TitleStatus { get; set; }

    public string? AuthenticityProfile { get; set; }

    public string? OriginalityProfile { get; set; }

    public string? RestorationLevel { get; set; }

    public string? DocumentationLevel { get; set; }

    public decimal? OverallConditionScore { get; set; }

    public bool HasRedFlags { get; set; }

    public string VehicleStatus { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public string? OwnershipNotes { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public long RowVersion { get; set; }
}

public sealed class VehicleHistoryResponse
{
    public Guid VehicleId { get; set; }
    public IReadOnlyList<VehicleHistoryEventResponse> Events { get; set; } = Array.Empty<VehicleHistoryEventResponse>();
}
   
public sealed class VehicleHistoryEventResponse
{
    public string EventType { get; set; } = string.Empty;
    public DateTime OccurredAtUtc { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? AppraisalId { get; set; }
    public Guid? InspectionId { get; set; }
}
