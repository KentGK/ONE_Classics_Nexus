namespace ONE_Classics_Nexus.Tests.Integration.Vehicles;

public static class VehicleTestData
{
    public static CreateVehicleRequest CreateValidRequest(string? vin = null)
    {
        return new CreateVehicleRequest
        {
            Vin = vin ?? GenerateVin(),
            Year = 1970,
            Make = "Chevrolet",
            Model = "K10",
            Trim = "Custom Deluxe",
            BodyStyle = "Pickup",
            Engine = "350 V8",
            Transmission = "4-Speed Manual",
            Drivetrain = "4x4",
            Mileage = 85432
        };
    }

    public static UpdateVehicleRequest CreateValidUpdate(string vin)
    {
        return new UpdateVehicleRequest
        {
            Vin = vin,
            Year = 1971,
            Make = "Chevrolet",
            Model = "K10",
            Trim = "Cheyenne",
            BodyStyle = "Pickup",
            Engine = "402 Big Block",
            Transmission = "TH400 Automatic",
            Drivetrain = "4x4",
            Mileage = 91234
        };
    }

    public static string GenerateVin()
    {
        // Generates a simple unique 17-char VIN-safe test value.
        var raw = Guid.NewGuid().ToString("N")[..17].ToUpperInvariant();
        return raw.Replace("I", "1").Replace("O", "0").Replace("Q", "8");
    }
}

public sealed class CreateVehicleRequest
{
    public string Vin { get; set; } = default!;
    public int Year { get; set; }
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public int Mileage { get; set; }
}

public sealed class UpdateVehicleRequest
{
    public string Vin { get; set; } = default!;
    public int Year { get; set; }
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public int Mileage { get; set; }
}

public sealed class VehicleResponse
{
    public Guid VehicleId { get; set; }
    public string Vin { get; set; } = default!;
    public int Year { get; set; }
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public int Mileage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class ProblemDetailsResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
}