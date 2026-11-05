using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Tests.Integration.Vehicles
{
    internal static class VehicleTestData
    {
        public static Vehicle CreateNewVehicle()
        {
            var now = DateTime.UtcNow;

            return Vehicle.Create(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "TESTVIN1234567890",
                1967,
                "Ford",
                null,
                "Mustang",
                "GT",
                "First Series",
                "Coupe",
                null,
                null,
                null,
                "V8",
                "Manual",
                "RWD",
                12345,
                "miles",
                null,
                "Red",
                "Black",
                "Clean",
                "Original",
                null,
                "Older Restoration",
                "Good",
                8.5m,
                false,
                "Integration test vehicle",
                "Test ownership notes",
                now);
        }

        public static Vehicle CreateExistingVehicle(Guid vehicleId)
        {
            var now = DateTime.UtcNow;

            return Vehicle.Rehydrate(
                vehicleId,
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "TESTVIN1234567890",
                1967,
                "Ford",
                null,
                "Mustang",
                "GT",
                "First Series",
                "Coupe",
                null,
                null,
                null,
                "V8",
                "Manual",
                "RWD",
                12345,
                "miles",
                null,
                "Red",
                "Black",
                "Clean",
                "Original",
                null,
                "Older Restoration",
                "Good",
                8.5m,
                false,
                "Integration test vehicle",
                "Test ownership notes",
                Vehicle.DefaultVehicleStatus,
                0,
                now,
                now);
        }

        public static Vehicle CreateMinimalVehicle()
        {
            var now = DateTime.UtcNow;

            return Vehicle.Create(
                Guid.NewGuid(),
                null,
                2000,
                "Generic",
                null,
                "Model",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                false,
                null,
                null,
                now);
        }

        public static ONE_Classics_Nexus.API.Contracts.Vehicles.CreateVehicleRequest CreateValidRequest(Guid vehicleTypeId)
        {
            return new ONE_Classics_Nexus.API.Contracts.Vehicles.CreateVehicleRequest
            {
                VehicleTypeId = vehicleTypeId,
                Vin = GenerateVin(),
                Year = 2020,
                Make = "TestMake",
                Model = "TestModel",
                Trim = "Base",
                BodyStyle = "Sedan",
                Engine = "2.0L I4",
                Transmission = "Automatic",
                Drivetrain = "FWD",
                Mileage = 1000
            };
        }

        public static ONE_Classics_Nexus.API.Contracts.Vehicles.UpdateVehicleRequest CreateValidUpdate(string vin, long rowVersion = 1)
        {
            return new ONE_Classics_Nexus.API.Contracts.Vehicles.UpdateVehicleRequest
            {
                RowVersion = rowVersion,
                VehicleTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Vin = vin,
                Year = 2021,
                Make = "UpdatedMake",
                Model = "UpdatedModel",
                Trim = "Sport",
                BodyStyle = "Coupe",
                Engine = "3.0L V6",
                Transmission = "Manual",
                Drivetrain = "AWD",
                Mileage = 2000
            };
        }

        public static string GenerateVin()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 17).ToUpper();
        }

        public static string GetRequiredVin(ONE_Classics_Nexus.API.Contracts.Vehicles.CreateVehicleRequest request)
        {
            return request.Vin
                ?? throw new InvalidOperationException("The vehicle test request must contain a VIN.");
        }
    }
}