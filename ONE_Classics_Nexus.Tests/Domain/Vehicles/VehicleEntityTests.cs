using FluentAssertions;
using ONE_Classics_Nexus.Domain.Constants;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Tests.Domain.Vehicles;

public sealed class VehicleEntityTests
{
    [Fact]
    public void Vehicle_Create_ValidData_Succeeds()
    {
        var vehicle = CreateVehicle();

        vehicle.VehicleId.Should().NotBeEmpty();
        vehicle.Make.Should().Be("Ford");
        vehicle.Model.Should().Be("Mustang");
        vehicle.RowVersion.Should().Be(1);
    }

    [Fact]
    public void Vehicle_Create_EmptyMake_Fails()
    {
        Action act = () => BuildVehicle(make: " ");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Vehicle_Create_EmptyModel_Fails()
    {
        Action act = () => BuildVehicle(model: "");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Vehicle_Create_NegativeMileage_Fails()
    {
        Action act = () => BuildVehicle(mileage: -1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("miles", OdometerUnits.Miles)]
    [InlineData("kilometers", OdometerUnits.Kilometers)]
    [InlineData("hours", OdometerUnits.Hours)]
    [InlineData("unknown", OdometerUnits.Unknown)]
    [InlineData("not_applicable", OdometerUnits.NotApplicable)]
    [InlineData("MILES", OdometerUnits.Miles)]
    public void Vehicle_Create_AllowedOdometerUnits_Succeeds(string inputUnit, string expectedUnit)
    {
        var vehicle = BuildVehicle(odometerUnit: inputUnit);

        vehicle.OdometerUnit.Should().Be(expectedUnit);
    }

    [Theory]
    [InlineData("km", OdometerUnits.Kilometers)]
    [InlineData("KMS", OdometerUnits.Kilometers)]
    [InlineData("kilometres", OdometerUnits.Kilometers)]
    [InlineData("mi", OdometerUnits.Miles)]
    public void Vehicle_Rehydrate_LegacyOdometerUnit_Normalizes(string legacyUnit, string expectedUnit)
    {
        var vehicle = Vehicle.Rehydrate(
            vehicleId: Guid.NewGuid(),
            vehicleTypeId: Guid.NewGuid(),
            vin: "1FAFP45X1WF123456",
            year: 1967,
            make: "Ford",
            marque: null,
            model: "Mustang",
            trim: "GT",
            seriesName: "First Gen",
            bodyStyle: "Coupe",
            chassisNumber: "CHS-123",
            bodyNumber: "BDY-456",
            engineNumber: "ENG-789",
            engine: "V8",
            transmission: "Manual",
            drivetrain: "RWD",
            mileage: 12345,
            odometerUnit: legacyUnit,
            odometerStatus: "Actual",
            exteriorColor: "Red",
            interiorColor: "Black",
            titleStatus: "Clean",
            authenticityProfile: "Unknown",
            originalityProfile: "Unknown",
            restorationLevel: "Driver Restoration",
            documentationLevel: "Good",
            overallConditionScore: 72m,
            hasRedFlags: false,
            notes: "Test vehicle",
            ownershipNotes: "Single owner",
            vehicleStatus: Vehicle.DefaultVehicleStatus,
            rowVersion: 1,
            createdAtUtc: DateTime.UtcNow,
            updatedAtUtc: DateTime.UtcNow);

        vehicle.OdometerUnit.Should().Be(expectedUnit);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedOdometerUnit_Fails()
    {
        Action act = () => BuildVehicle(odometerUnit: "league");

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Actual", OdometerStatuses.Actual)]
    [InlineData("Exceeds Mechanical Limits", OdometerStatuses.ExceedsMechanicalLimits)]
    [InlineData("Not Actual", OdometerStatuses.NotActual)]
    [InlineData("TMU", OdometerStatuses.Tmu)]
    [InlineData("Unknown", OdometerStatuses.Unknown)]
    [InlineData("Replaced", OdometerStatuses.Replaced)]
    [InlineData("actual", OdometerStatuses.Actual)]
    public void Vehicle_Create_AllowedOdometerStatuses_Succeeds(string inputStatus, string expectedStatus)
    {
        var vehicle = BuildVehicle(odometerStatus: inputStatus);

        vehicle.OdometerStatus.Should().Be(expectedStatus);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedOdometerStatus_Fails()
    {
        Action act = () => BuildVehicle(odometerStatus: "Estimated");

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Clean", TitleStatuses.Clean)]
    [InlineData("Salvage", TitleStatuses.Salvage)]
    [InlineData("Rebuilt", TitleStatuses.Rebuilt)]
    [InlineData("Flood", TitleStatuses.Flood)]
    [InlineData("Bonded", TitleStatuses.Bonded)]
    [InlineData("Parts Only", TitleStatuses.PartsOnly)]
    [InlineData("NoTitle", TitleStatuses.NoTitle)]
    [InlineData("Bill Of Sale", TitleStatuses.BillOfSale)]
    [InlineData("Foreign Registration", TitleStatuses.ForeignRegistration)]
    [InlineData("Unknown", TitleStatuses.Unknown)]
    [InlineData("clean", TitleStatuses.Clean)]
    public void Vehicle_Create_AllowedTitleStatuses_Succeeds(string inputStatus, string expectedStatus)
    {
        var vehicle = BuildVehicle(titleStatus: inputStatus);

        vehicle.TitleStatus.Should().Be(expectedStatus);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedTitleStatus_Fails()
    {
        Action act = () => BuildVehicle(titleStatus: "Lien");

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Unknown", AuthenticityProfiles.Unknown)]
    [InlineData("Original", AuthenticityProfiles.Original)]
    [InlineData("Restored", AuthenticityProfiles.Restored)]
    [InlineData("Survivor", AuthenticityProfiles.Survivor)]
    [InlineData("Restomod", AuthenticityProfiles.Restomod)]
    [InlineData("Custom", AuthenticityProfiles.Custom)]
    [InlineData("Tribute", AuthenticityProfiles.Tribute)]
    [InlineData("Industrial", AuthenticityProfiles.Industrial)]
    [InlineData("Industrial Standardized", AuthenticityProfiles.IndustrialStandardized)]
    [InlineData("Industry Standardized", AuthenticityProfiles.IndustrialStandardized)]
    [InlineData("original", AuthenticityProfiles.Original)]
    public void Vehicle_Create_AllowedAuthenticityProfiles_Succeeds(string inputProfile, string expectedProfile)
    {
        var vehicle = BuildVehicle(authenticityProfile: inputProfile);

        vehicle.AuthenticityProfile.Should().Be(expectedProfile);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedAuthenticityProfile_Fails()
    {
        Action act = () => BuildVehicle(authenticityProfile: "Factory Certified");

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Unknown", OriginalityProfiles.Unknown)]
    [InlineData("Original", OriginalityProfiles.Original)]
    [InlineData("Predominantly Original", OriginalityProfiles.PredominantlyOriginal)]
    [InlineData("Restored", OriginalityProfiles.Restored)]
    [InlineData("Partially Restored", OriginalityProfiles.PartiallyRestored)]
    [InlineData("Modified", OriginalityProfiles.Modified)]
    [InlineData("Highly Modified", OriginalityProfiles.HighlyModified)]
    [InlineData("Reconstructed", OriginalityProfiles.Reconstructed)]
    [InlineData("original", OriginalityProfiles.Original)]
    public void Vehicle_Create_AllowedOriginalityProfiles_Succeeds(string inputProfile, string expectedProfile)
    {
        var vehicle = BuildVehicle(originalityProfile: inputProfile);

        vehicle.OriginalityProfile.Should().Be(expectedProfile);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedOriginalityProfile_Fails()
    {
        Action act = () => BuildVehicle(originalityProfile: "Survivor");

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Unknown", RestorationLevels.Unknown)]
    [InlineData("Unrestored", RestorationLevels.Unrestored)]
    [InlineData("Preserved", RestorationLevels.Preserved)]
    [InlineData("Driver Restoration", RestorationLevels.DriverRestoration)]
    [InlineData("Older Restoration", RestorationLevels.OlderRestoration)]
    [InlineData("Full Restoration", RestorationLevels.FullRestoration)]
    [InlineData("Concours Restoration", RestorationLevels.ConcoursRestoration)]
    [InlineData("Restomod", RestorationLevels.Restomod)]
    [InlineData("Reconstruction", RestorationLevels.Reconstruction)]
    [InlineData("unknown", RestorationLevels.Unknown)]
    public void Vehicle_Create_AllowedRestorationLevels_Succeeds(string inputLevel, string expectedLevel)
    {
        var vehicle = BuildVehicle(restorationLevel: inputLevel);

        vehicle.RestorationLevel.Should().Be(expectedLevel);
    }

    [Theory]
    [InlineData("DriverRestoration", RestorationLevels.DriverRestoration)]
    [InlineData("DriverRestorationLevel", RestorationLevels.DriverRestoration)]
    [InlineData("Recent Restoration", RestorationLevels.FullRestoration)]
    [InlineData("Show Quality", RestorationLevels.ConcoursRestoration)]
    public void Vehicle_Rehydrate_LegacyRestorationLevel_Normalizes(string legacyValue, string expectedValue)
    {
        var vehicle = Vehicle.Rehydrate(
            vehicleId: Guid.NewGuid(),
            vehicleTypeId: Guid.NewGuid(),
            vin: "1FAFP45X1WF123456",
            year: 1967,
            make: "Ford",
            marque: null,
            model: "Mustang",
            trim: "GT",
            seriesName: "First Gen",
            bodyStyle: "Coupe",
            chassisNumber: "CHS-123",
            bodyNumber: "BDY-456",
            engineNumber: "ENG-789",
            engine: "V8",
            transmission: "Manual",
            drivetrain: "RWD",
            mileage: 12345,
            odometerUnit: "miles",
            odometerStatus: "Actual",
            exteriorColor: "Red",
            interiorColor: "Black",
            titleStatus: "Clean",
            authenticityProfile: "Unknown",
            originalityProfile: "Unknown",
            restorationLevel: legacyValue,
            documentationLevel: "Good",
            overallConditionScore: 72m,
            hasRedFlags: false,
            notes: "Test vehicle",
            ownershipNotes: "Single owner",
            vehicleStatus: Vehicle.DefaultVehicleStatus,
            rowVersion: 1,
            createdAtUtc: DateTime.UtcNow,
            updatedAtUtc: DateTime.UtcNow);

        vehicle.RestorationLevel.Should().Be(expectedValue);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedRestorationLevel_Fails()
    {
        Action act = () => BuildVehicle(restorationLevel: "Frame Off");

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Unknown", DocumentationLevels.Unknown)]
    [InlineData("Minimal", DocumentationLevels.Minimal)]
    [InlineData("Partial", DocumentationLevels.Partial)]
    [InlineData("Good", DocumentationLevels.Good)]
    [InlineData("Extensive", DocumentationLevels.Extensive)]
    [InlineData("Exceptional", DocumentationLevels.Exceptional)]
    [InlineData("good", DocumentationLevels.Good)]
    public void Vehicle_Create_AllowedDocumentationLevels_Succeeds(string inputLevel, string expectedLevel)
    {
        var vehicle = BuildVehicle(documentationLevel: inputLevel);

        vehicle.DocumentationLevel.Should().Be(expectedLevel);
    }

    [Theory]
    [InlineData("None", DocumentationLevels.Unknown)]
    [InlineData("Moderate", DocumentationLevels.Good)]
    [InlineData("Complete", DocumentationLevels.Exceptional)]
    public void Vehicle_Rehydrate_LegacyDocumentationLevel_Normalizes(string legacyValue, string expectedValue)
    {
        var vehicle = Vehicle.Rehydrate(
            vehicleId: Guid.NewGuid(),
            vehicleTypeId: Guid.NewGuid(),
            vin: "1FAFP45X1WF123456",
            year: 1967,
            make: "Ford",
            marque: null,
            model: "Mustang",
            trim: "GT",
            seriesName: "First Gen",
            bodyStyle: "Coupe",
            chassisNumber: "CHS-123",
            bodyNumber: "BDY-456",
            engineNumber: "ENG-789",
            engine: "V8",
            transmission: "Manual",
            drivetrain: "RWD",
            mileage: 12345,
            odometerUnit: "miles",
            odometerStatus: "Actual",
            exteriorColor: "Red",
            interiorColor: "Black",
            titleStatus: "Clean",
            authenticityProfile: "Unknown",
            originalityProfile: "Unknown",
            restorationLevel: "Driver Restoration",
            documentationLevel: legacyValue,
            overallConditionScore: 72m,
            hasRedFlags: false,
            notes: "Test vehicle",
            ownershipNotes: "Single owner",
            vehicleStatus: Vehicle.DefaultVehicleStatus,
            rowVersion: 1,
            createdAtUtc: DateTime.UtcNow,
            updatedAtUtc: DateTime.UtcNow);

        vehicle.DocumentationLevel.Should().Be(expectedValue);
    }

    [Fact]
    public void Vehicle_Create_UnsupportedDocumentationLevel_Fails()
    {
        Action act = () => BuildVehicle(documentationLevel: "Archive Binder");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Vehicle_ApplyConditionSummary_UpdatesSummary()
    {
        var vehicle = CreateVehicle();

        vehicle.UpdateConditionSummary(81.5m, true, new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc));

        vehicle.OverallConditionScore.Should().Be(81.5m);
        vehicle.HasRedFlags.Should().BeTrue();
    }

    [Fact]
    public void Vehicle_UpdateConfiguration_UpdatesTimestamp()
    {
        var createdAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var updatedAt = new DateTime(2025, 1, 2, 11, 0, 0, DateTimeKind.Utc);

        var vehicle = BuildVehicle(nowUtc: createdAt);

        vehicle.ChangeStatus("Archived", updatedAt);

        vehicle.UpdatedAtUtc.Should().Be(updatedAt);
    }

    [Fact]
    public void Vehicle_UpdateIdentity_UpdatesRowVersion()
    {
        var vehicle = CreateVehicle();

        vehicle.ChangeStatus("Archived", new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc));

        vehicle.RowVersion.Should().Be(2);
    }

    [Fact]
    public void Vehicle_MasterChange_DoesNotMutateSnapshot()
    {
        var appraisalId = Guid.NewGuid();
        var vehicle = CreateVehicle();
        var snapshot = AppraisalSubjectSnapshot.CreateDraft(appraisalId, vehicle);

        var updatedVehicle = Vehicle.Rehydrate(
            vehicle.VehicleId,
            vehicle.VehicleTypeId,
            vehicle.Vin,
            vehicle.Year,
            vehicle.Make,
            vehicle.Marque,
            vehicle.Model,
            vehicle.Trim,
            vehicle.SeriesName,
            vehicle.BodyStyle,
            vehicle.ChassisNumber,
            vehicle.BodyNumber,
            vehicle.EngineNumber,
            vehicle.Engine,
            vehicle.Transmission,
            vehicle.Drivetrain,
            vehicle.Mileage,
            vehicle.OdometerUnit,
            vehicle.OdometerStatus,
            "Black",
            vehicle.InteriorColor,
            vehicle.TitleStatus,
            vehicle.AuthenticityProfile,
            vehicle.OriginalityProfile,
            vehicle.RestorationLevel,
            vehicle.DocumentationLevel,
            vehicle.OverallConditionScore,
            vehicle.HasRedFlags,
            vehicle.Notes,
            "Updated ownership",
            vehicle.VehicleStatus,
            vehicle.RowVersion,
            vehicle.CreatedAtUtc,
            DateTime.UtcNow);

        updatedVehicle.ExteriorColor.Should().Be("Black");
        snapshot.ExteriorColor.Should().Be("Red");
    }

    private static Vehicle CreateVehicle()
    {
        return BuildVehicle();
    }

    private static Vehicle BuildVehicle(
        string make = "Ford",
        string model = "Mustang",
        int? mileage = 12345,
        string odometerUnit = "miles",
        string odometerStatus = "Actual",
        string titleStatus = "Clean",
        string authenticityProfile = "Unknown",
        string originalityProfile = "Unknown",
        string restorationLevel = "Driver Restoration",
        string documentationLevel = "Good",
        DateTime? nowUtc = null)
    {
        return Vehicle.Create(
            vehicleTypeId: Guid.NewGuid(),
            vin: "1FAFP45X1WF123456",
            year: 1967,
            make: make,
            marque: null,
            model: model,
            trim: "GT",
            seriesName: "First Gen",
            bodyStyle: "Coupe",
            chassisNumber: "CHS-123",
            bodyNumber: "BDY-456",
            engineNumber: "ENG-789",
            engine: "V8",
            transmission: "Manual",
            drivetrain: "RWD",
            mileage: mileage,
            odometerUnit: odometerUnit,
            odometerStatus: odometerStatus,
            exteriorColor: "Red",
            interiorColor: "Black",
            titleStatus: titleStatus,
            authenticityProfile: authenticityProfile,
            originalityProfile: originalityProfile,
            restorationLevel: restorationLevel,
            documentationLevel: documentationLevel,
            overallConditionScore: 72m,
            hasRedFlags: false,
            notes: "Test vehicle",
            ownershipNotes: "Single owner",
            nowUtc: nowUtc);
    }
}
