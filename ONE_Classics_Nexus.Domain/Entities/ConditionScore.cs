using System;

namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class ConditionScore
{
    public Guid ConditionScoreId { get; set; }
    public Guid InspectionId { get; set; }
    public int SystemNumber { get; set; }
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public static ConditionScore Create(
        Guid inspectionId,
        int systemNumber,
        string systemKey,
        decimal score0To5,
        decimal weight,
        bool isRedFlag = false,
        string? notes = null)
    {
        if (inspectionId == Guid.Empty)
            throw new ArgumentException("InspectionId is required.", nameof(inspectionId));

        if (systemNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(systemNumber), "SystemNumber must be greater than zero.");

        if (string.IsNullOrWhiteSpace(systemKey))
            throw new ArgumentException("SystemKey is required.", nameof(systemKey));

        if (score0To5 < 0m || score0To5 > 5m)
            throw new ArgumentOutOfRangeException(nameof(score0To5), "Score must be between 0 and 5.");

        if (weight < 0m)
            throw new ArgumentOutOfRangeException(nameof(weight), "Weight cannot be negative.");

        var nowUtc = DateTime.UtcNow;
        return new ConditionScore
        {
            ConditionScoreId = Guid.NewGuid(),
            InspectionId = inspectionId,
            SystemNumber = systemNumber,
            SystemKey = systemKey.Trim(),
            Score0To5 = score0To5,
            Weight = weight,
            IsRedFlag = isRedFlag,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
            CreatedAtUtc = nowUtc,
            UpdatedAtUtc = nowUtc
        };
    }

    public void Update(decimal score0To5, decimal weight, bool isRedFlag, string? notes)
    {
        if (score0To5 < 0m || score0To5 > 5m)
            throw new ArgumentOutOfRangeException(nameof(score0To5), "Score must be between 0 and 5.");

        if (weight < 0m)
            throw new ArgumentOutOfRangeException(nameof(weight), "Weight cannot be negative.");

        Score0To5 = score0To5;
        Weight = weight;
        IsRedFlag = isRedFlag;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        UpdatedAtUtc = DateTime.UtcNow;
    }
}