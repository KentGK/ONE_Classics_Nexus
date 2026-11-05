using System;

namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class Inspection
{
    public Guid InspectionId { get; set; }
    public Guid AppraisalId { get; set; }
    public Guid InspectionTemplateId { get; set; }
    public Guid? InspectedByUserId { get; set; }
    public string InspectionType { get; set; } = "Standard";
    public string Status { get; set; } = "Draft";
    public string? InspectionLocation { get; set; }
    public DateTime? InspectedOnUtc { get; set; }
    public decimal? CompletionPercentage { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }
    public string? SummaryNotes { get; set; }
    public long RowVersion { get; set; } = 1;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public static Inspection Create(
        Guid appraisalId,
        Guid inspectionTemplateId,
        string inspectionType,
        Guid? inspectedByUserId = null,
        string status = "Draft",
        string? inspectionLocation = null,
        string? summaryNotes = null)
    {
        if (appraisalId == Guid.Empty)
            throw new ArgumentException("AppraisalId is required.", nameof(appraisalId));

        if (inspectionTemplateId == Guid.Empty)
            throw new ArgumentException("InspectionTemplateId is required.", nameof(inspectionTemplateId));

        if (string.IsNullOrWhiteSpace(inspectionType))
            throw new ArgumentException("InspectionType is required.", nameof(inspectionType));

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status is required.", nameof(status));

        var now = DateTime.UtcNow;

        return new Inspection
        {
            InspectionId = Guid.NewGuid(),
            AppraisalId = appraisalId,
            InspectionTemplateId = inspectionTemplateId,
            InspectedByUserId = inspectedByUserId,
            InspectionType = inspectionType.Trim(),
            Status = status.Trim(),
            InspectionLocation = Normalize(inspectionLocation),
            SummaryNotes = Normalize(summaryNotes),
            CompletionPercentage = 0m,
            OverallConditionScore = null,
            HasRedFlags = false,
            RowVersion = 1,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
    }

    public void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
        RowVersion++;
    }

    public void ApplyScoreSummary(decimal completionPercentage, decimal overallConditionScore, bool hasRedFlags)
    {
        CompletionPercentage = completionPercentage;
        OverallConditionScore = overallConditionScore;
        HasRedFlags = hasRedFlags;
        Touch();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
