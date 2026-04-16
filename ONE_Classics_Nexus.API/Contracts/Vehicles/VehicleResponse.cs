namespace ONE_Classics_Nexus.API.Contracts.Vehicles;

public sealed class VehicleResponse
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
    public int Mileage { get; set; }
}