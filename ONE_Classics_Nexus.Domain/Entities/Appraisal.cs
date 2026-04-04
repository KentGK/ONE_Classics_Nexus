using System;

namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class Appraisal
{
    public Guid AppraisalId { get; set; }
    public Guid VehicleId { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public long RowVersion { get; set; }

    public static Appraisal Create(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("VehicleId is required.", nameof(vehicleId));

        var now = DateTime.UtcNow;

        return new Appraisal
        {
            AppraisalId = Guid.NewGuid(),
            VehicleId = vehicleId,
            Status = "Draft",
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            RowVersion = 1
        };
    }

    public void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
        RowVersion++;
    }

    public void SetStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status is required.", nameof(status));

        Status = status.Trim();
        Touch();
    }
}