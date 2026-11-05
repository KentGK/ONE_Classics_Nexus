using System;
namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class AppraisalSubjectSnapshot
{
    public const string DraftStatus = "Draft";
    public const string ApprovedStatus = "Approved";

    public Guid AppraisalSubjectSnapshotId { get; private set; }
    public Guid AppraisalId { get; private set; }
    public Guid VehicleId { get; private set; }

    public string? Vin { get; private set; }
    public string? ChassisNumber { get; private set; }
    public string? BodyNumber { get; private set; }
    public string? EngineNumber { get; private set; }

    public int Year { get; private set; }
    public string Make { get; private set; } = string.Empty;
    public string? Marque { get; private set; }
    public string Model { get; private set; } = string.Empty;
    public string? SeriesName { get; private set; }
    public string? Trim { get; private set; }
    public string? BodyStyle { get; private set; }

    public string? Engine { get; private set; }
    public string? Transmission { get; private set; }
    public string? Drivetrain { get; private set; }

    public int? Mileage { get; private set; }
    public string? OdometerUnit { get; private set; }

    public string? ExteriorColor { get; private set; }
    public string? InteriorColor { get; private set; }

    public string? TitleStatus { get; private set; }
    public string? AuthenticityProfile { get; private set; }
    public string? OriginalityProfile { get; private set; }
    public string? RestorationLevel { get; private set; }
    public string? DocumentationLevel { get; private set; }

    public string? OwnershipNotes { get; private set; }

    public string SnapshotStatus { get; private set; } = DraftStatus;
    public Guid? ApprovedByUserId { get; private set; }
    public DateTime? ApprovedAtUtc { get; private set; }

    public long RowVersion { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    public static AppraisalSubjectSnapshot CreateDraft(
        Guid appraisalId,
        Vehicle vehicle,
        DateTime? nowUtc = null)
    {
        if (appraisalId == Guid.Empty)
        {
            throw new ArgumentException("AppraisalId is required.", nameof(appraisalId));
        }

        ArgumentNullException.ThrowIfNull(vehicle);

        var utcNow = nowUtc ?? DateTime.UtcNow;

        var snapshot = new AppraisalSubjectSnapshot
        {
            AppraisalSubjectSnapshotId = Guid.NewGuid(),
            AppraisalId = appraisalId,
            VehicleId = vehicle.VehicleId,
            SnapshotStatus = DraftStatus,
            RowVersion = 1,
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow
        };

        snapshot.CopyVehicleFields(vehicle);

        return snapshot;
    }

    public static AppraisalSubjectSnapshot Rehydrate(
        Guid appraisalSubjectSnapshotId,
        Guid appraisalId,
        Guid vehicleId,
        string? vin,
        string? chassisNumber,
        string? bodyNumber,
        string? engineNumber,
        int year,
        string make,
        string? marque,
        string model,
        string? seriesName,
        string? trim,
        string? bodyStyle,
        string? engine,
        string? transmission,
        string? drivetrain,
        int? mileage,
        string? odometerUnit,
        string? exteriorColor,
        string? interiorColor,
        string? titleStatus,
        string? authenticityProfile,
        string? originalityProfile,
        string? restorationLevel,
        string? documentationLevel,
        string? ownershipNotes,
        string snapshotStatus,
        Guid? approvedByUserId,
        DateTime? approvedAtUtc,
        long rowVersion,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        if (appraisalSubjectSnapshotId == Guid.Empty)
        {
            throw new ArgumentException("AppraisalSubjectSnapshotId is required.", nameof(appraisalSubjectSnapshotId));
        }

        if (appraisalId == Guid.Empty)
        {
            throw new ArgumentException("AppraisalId is required.", nameof(appraisalId));
        }

        if (vehicleId == Guid.Empty)
        {
            throw new ArgumentException("VehicleId is required.", nameof(vehicleId));
        }

        if (string.IsNullOrWhiteSpace(make))
        {
            throw new ArgumentException("Make is required.", nameof(make));
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("Model is required.", nameof(model));
        }

        if (string.IsNullOrWhiteSpace(snapshotStatus))
        {
            throw new ArgumentException("SnapshotStatus is required.", nameof(snapshotStatus));
        }

        return new AppraisalSubjectSnapshot
        {
            AppraisalSubjectSnapshotId = appraisalSubjectSnapshotId,
            AppraisalId = appraisalId,
            VehicleId = vehicleId,
            Vin = vin,
            ChassisNumber = chassisNumber,
            BodyNumber = bodyNumber,
            EngineNumber = engineNumber,
            Year = year,
            Make = make,
            Marque = marque,
            Model = model,
            SeriesName = seriesName,
            Trim = trim,
            BodyStyle = bodyStyle,
            Engine = engine,
            Transmission = transmission,
            Drivetrain = drivetrain,
            Mileage = mileage,
            OdometerUnit = odometerUnit,
            ExteriorColor = exteriorColor,
            InteriorColor = interiorColor,
            TitleStatus = titleStatus,
            AuthenticityProfile = authenticityProfile,
            OriginalityProfile = originalityProfile,
            RestorationLevel = restorationLevel,
            DocumentationLevel = documentationLevel,
            OwnershipNotes = ownershipNotes,
            SnapshotStatus = snapshotStatus,
            ApprovedByUserId = approvedByUserId,
            ApprovedAtUtc = approvedAtUtc,
            RowVersion = rowVersion,
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = updatedAtUtc
        };
    }

    public void RefreshDraftFromVehicle(
        Vehicle vehicle,
        DateTime? nowUtc = null)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (!string.Equals(SnapshotStatus, DraftStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only draft snapshots can be refreshed.");
        }

        if (vehicle.VehicleId != VehicleId)
        {
            throw new InvalidOperationException("Vehicle does not match snapshot vehicle.");
        }

        CopyVehicleFields(vehicle);
        RowVersion++;
        UpdatedAtUtc = nowUtc ?? DateTime.UtcNow;
    }

    public void Approve(
        Guid userId,
        DateTime? nowUtc = null)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (!string.Equals(SnapshotStatus, DraftStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only draft snapshots can be approved.");
        }

        SnapshotStatus = ApprovedStatus;
        ApprovedByUserId = userId;
        ApprovedAtUtc = nowUtc ?? DateTime.UtcNow;
        RowVersion++;
        UpdatedAtUtc = ApprovedAtUtc.Value;
    }

    private void CopyVehicleFields(Vehicle vehicle)
    {
        Vin = vehicle.Vin;
        ChassisNumber = vehicle.ChassisNumber;
        BodyNumber = vehicle.BodyNumber;
        EngineNumber = vehicle.EngineNumber;

        Year = vehicle.Year;
        Make = vehicle.Make;
        Marque = vehicle.Marque;
        Model = vehicle.Model;
        SeriesName = vehicle.SeriesName;
        Trim = vehicle.Trim;
        BodyStyle = vehicle.BodyStyle;

        Engine = vehicle.Engine;
        Transmission = vehicle.Transmission;
        Drivetrain = vehicle.Drivetrain;

        Mileage = vehicle.Mileage;
        OdometerUnit = vehicle.OdometerUnit;

        ExteriorColor = vehicle.ExteriorColor;
        InteriorColor = vehicle.InteriorColor;

        TitleStatus = vehicle.TitleStatus;
        AuthenticityProfile = vehicle.AuthenticityProfile;
        OriginalityProfile = vehicle.OriginalityProfile;
        RestorationLevel = vehicle.RestorationLevel;
        DocumentationLevel = vehicle.DocumentationLevel;

        OwnershipNotes = vehicle.OwnershipNotes;
    }
}
