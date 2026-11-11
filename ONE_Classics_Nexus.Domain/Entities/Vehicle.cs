using System;
using ONE_Classics_Nexus.Domain.Constants;

namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class Vehicle
{
    public const string DefaultVehicleStatus = VehicleStatuses.Active;
    public Guid VehicleId { get; set; }
    public Guid? VehicleTypeId { get; set; }
    public string Vin { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public string? TitleStatus { get; set; }
    public string? ChassisNumber { get; set; }
    public string? BodyNumber { get; set; }
    public string? EngineNumber { get; set; }
    public string? Marque { get; set; }
    public string? SeriesName { get; set; }
    public int? Mileage { get; set; }
    public string? OdometerUnit { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public string? AuthenticityProfile { get; set; }
    public string? OriginalityProfile { get; set; }
    public string? RestorationLevel { get; set; }
    public string? DocumentationLevel { get; set; }
    public string? OwnershipNotes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string? OdometerStatus { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }
    public string VehicleStatus { get; set; } = DefaultVehicleStatus;
    public long RowVersion { get; set; }
    public string? Notes { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public static Vehicle Create(
        string vin,
        int year,
        string make,
        string model,
        string? trim = null,
        string? bodyStyle = null,
        string? engine = null,
        string? transmission = null,
        string? drivetrain = null,
        string? titleStatus = null)
    {
        if (string.IsNullOrWhiteSpace(vin))
            throw new ArgumentException("VIN is required.", nameof(vin));

        if (year < 1886 || year > DateTime.UtcNow.Year + 1)
            throw new ArgumentOutOfRangeException(nameof(year), "Year is out of valid range.");

        if (string.IsNullOrWhiteSpace(make))
            throw new ArgumentException("Make is required.", nameof(make));

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.", nameof(model));

        return new Vehicle
        {
            VehicleId = Guid.NewGuid(),
            Vin = vin.Trim(),
            Year = year,
            Make = make.Trim(),
            Model = model.Trim(),
            Trim = Normalize(trim),
            BodyStyle = Normalize(bodyStyle),
            Engine = Normalize(engine),
            Transmission = Normalize(transmission),
            Drivetrain = Normalize(drivetrain),
            TitleStatus = Normalize(titleStatus),
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = 1
        };
    }

    public static Vehicle Create(
        Guid? vehicleTypeId, string? vin, int year, string make, string? marque,
        string model, string? trim, string? seriesName, string? bodyStyle,
        string? chassisNumber, string? bodyNumber, string? engineNumber,
        string? engine, string? transmission, string? drivetrain, int? mileage,
        string odometerUnit, string? odometerStatus, string? exteriorColor,
        string? interiorColor, string? titleStatus, string? authenticityProfile,
        string? originalityProfile, string? restorationLevel, string? documentationLevel,
        decimal? overallConditionScore, bool hasRedFlags, string? notes,
        string? ownershipNotes, DateTime? nowUtc = null)
    {
        if (year < 1886 || year > 2100)
            throw new ArgumentOutOfRangeException(nameof(year));
        if (string.IsNullOrWhiteSpace(make))
            throw new ArgumentException("Make is required.", nameof(make));
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.", nameof(model));
        if (mileage < 0)
            throw new ArgumentOutOfRangeException(nameof(mileage));

        var now = nowUtc ?? DateTime.UtcNow;
        return new Vehicle
        {
            VehicleId = Guid.NewGuid(), VehicleTypeId = vehicleTypeId,
            Vin = Normalize(vin) ?? string.Empty, Year = year, Make = make.Trim(),
            Marque = Normalize(marque), Model = model.Trim(), Trim = Normalize(trim),
            SeriesName = Normalize(seriesName), BodyStyle = Normalize(bodyStyle),
            ChassisNumber = Normalize(chassisNumber), BodyNumber = Normalize(bodyNumber),
            EngineNumber = Normalize(engineNumber), Engine = Normalize(engine),
            Transmission = Normalize(transmission), Drivetrain = Normalize(drivetrain),
            Mileage = mileage,
            OdometerUnit = Required(odometerUnit, OdometerUnits.Normalize, nameof(odometerUnit)),
            OdometerStatus = Optional(odometerStatus, OdometerStatuses.Normalize, nameof(odometerStatus)),
            ExteriorColor = Normalize(exteriorColor), InteriorColor = Normalize(interiorColor),
            TitleStatus = Optional(titleStatus, TitleStatuses.Normalize, nameof(titleStatus)),
            AuthenticityProfile = Optional(authenticityProfile, AuthenticityProfiles.Normalize, nameof(authenticityProfile)),
            OriginalityProfile = Optional(originalityProfile, OriginalityProfiles.Normalize, nameof(originalityProfile)),
            RestorationLevel = Optional(restorationLevel, RestorationLevels.Normalize, nameof(restorationLevel)),
            DocumentationLevel = Optional(documentationLevel, DocumentationLevels.Normalize, nameof(documentationLevel)),
            OverallConditionScore = overallConditionScore, HasRedFlags = hasRedFlags,
            Notes = Normalize(notes), OwnershipNotes = Normalize(ownershipNotes),
            VehicleStatus = DefaultVehicleStatus, RowVersion = 1,
            CreatedAtUtc = now, UpdatedAtUtc = now
        };
    }

    public static Vehicle Rehydrate(
        Guid vehicleId, Guid? vehicleTypeId, string? vin, int year, string make,
        string? marque, string model, string? trim, string? seriesName,
        string? bodyStyle, string? chassisNumber, string? bodyNumber,
        string? engineNumber, string? engine, string? transmission,
        string? drivetrain, int? mileage, string? odometerUnit,
        string? odometerStatus, string? exteriorColor, string? interiorColor,
        string? titleStatus, string? authenticityProfile, string? originalityProfile,
        string? restorationLevel, string? documentationLevel,
        decimal? overallConditionScore, bool hasRedFlags, string? notes,
        string? ownershipNotes, string? vehicleStatus, long rowVersion,
        DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        return new Vehicle
        {
            VehicleId = vehicleId, VehicleTypeId = vehicleTypeId, Vin = Normalize(vin) ?? string.Empty,
            Year = year, Make = make, Marque = Normalize(marque), Model = model,
            Trim = Normalize(trim), SeriesName = Normalize(seriesName), BodyStyle = Normalize(bodyStyle),
            ChassisNumber = Normalize(chassisNumber), BodyNumber = Normalize(bodyNumber),
            EngineNumber = Normalize(engineNumber), Engine = Normalize(engine),
            Transmission = Normalize(transmission), Drivetrain = Normalize(drivetrain),
            Mileage = mileage,
            OdometerUnit = Optional(odometerUnit, OdometerUnits.Normalize, nameof(odometerUnit)),
            OdometerStatus = Optional(odometerStatus, OdometerStatuses.Normalize, nameof(odometerStatus)),
            ExteriorColor = Normalize(exteriorColor), InteriorColor = Normalize(interiorColor),
            TitleStatus = Optional(titleStatus, TitleStatuses.Normalize, nameof(titleStatus)),
            AuthenticityProfile = Optional(authenticityProfile, AuthenticityProfiles.Normalize, nameof(authenticityProfile)),
            OriginalityProfile = Optional(originalityProfile, OriginalityProfiles.Normalize, nameof(originalityProfile)),
            RestorationLevel = Optional(restorationLevel, RestorationLevels.Normalize, nameof(restorationLevel)),
            DocumentationLevel = Optional(documentationLevel, DocumentationLevels.Normalize, nameof(documentationLevel)),
            OverallConditionScore = overallConditionScore, HasRedFlags = hasRedFlags,
            Notes = Normalize(notes), OwnershipNotes = Normalize(ownershipNotes),
            VehicleStatus = Optional(vehicleStatus, VehicleStatuses.Normalize, nameof(vehicleStatus)) ?? DefaultVehicleStatus,
            RowVersion = rowVersion, CreatedAtUtc = createdAtUtc, UpdatedAtUtc = updatedAtUtc
        };
    }

    public void ChangeStatus(string status, DateTime nowUtc)
    {
        VehicleStatus = Required(status, VehicleStatuses.Normalize, nameof(status));
        UpdatedAtUtc = nowUtc;
        RowVersion++;
    }

    public void UpdateConditionSummary(decimal? score, bool hasRedFlags, DateTime nowUtc)
    {
        OverallConditionScore = score;
        HasRedFlags = hasRedFlags;
        UpdatedAtUtc = nowUtc;
        RowVersion++;
    }

    private static string Required(string? value, Func<string?, string?> normalize, string name) =>
        normalize(value) ?? throw new ArgumentException($"Invalid {name}.", name);

    private static string? Optional(string? value, Func<string?, string?> normalize, string name) =>
        string.IsNullOrWhiteSpace(value) ? null : Required(value, normalize, name);

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}