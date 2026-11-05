using System;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Tests.Integration.Vehicles
{
    internal static class VehicleTestData
    {
        public static Vehicle CreateNewVehicle()
        {
            var now = DateTime.UtcNow;

            return new Vehicle
            {
                VehicleId = Guid.NewGuid(),
                VehicleTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Vin = "TESTVIN1234567890",
                Year = 1967,
                Make = "Ford",
                Model = "Mustang",
                Trim = "GT",
                SeriesName = "First Series",
                BodyStyle = "Coupe",
                Drivetrain = "RWD",
                Engine = "V8",
                Transmission = "Manual",
                Mileage = 12345,
                OdometerUnit = "miles",
                ExteriorColor = "Red",
                InteriorColor = "Black",
                TitleStatus = "Clean",
                AuthenticityProfile = "Original",
                RestorationLevel = "Partial",
                DocumentationLevel = "Good",
                OverallConditionScore = 8.5m,
                HasRedFlags = false,
                Notes = "Integration test vehicle",
                OwnershipNotes = "Test ownership notes",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };
        }

        public static Vehicle CreateExistingVehicle(Guid vehicleId)
        {
            var v = CreateNewVehicle();
            v.VehicleId = vehicleId;
            return v;
        }

        public static Vehicle CreateMinimalVehicle()
        {
            var now = DateTime.UtcNow;

            return new Vehicle
            {
                VehicleId = Guid.NewGuid(),
                VehicleTypeId = Guid.NewGuid(),
                Year = 2000,
                Make = "Generic",
                Model = "Model",
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };
        }
    }
}