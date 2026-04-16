using System.ComponentModel.DataAnnotations;

namespace ONE_Classics_Nexus.API.Contracts.Vehicles;

public sealed class CreateVehicleRequest
{
    [Required]
    [MinLength(1)]
    public string Vin { get; set; } = string.Empty;

    [Range(1886, 3000)]
    public int Year { get; set; }

    [Required]
    [MinLength(1)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Model { get; set; } = string.Empty;

    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }

    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }
}