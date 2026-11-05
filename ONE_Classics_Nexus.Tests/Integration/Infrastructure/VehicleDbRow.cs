namespace ONE_Classics_Nexus.Tests.Integration.Infrastructure;

public sealed class VehicleDbRow
{
    public Guid Vehicle_Id { get; set; }
    public string Vin { get; set; } = default!;
    public int Year { get; set; }
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string? Trim { get; set; }
    public string? Body_Style { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public int Mileage { get; set; }
    public string? Vehicle_Status { get; set; }
}