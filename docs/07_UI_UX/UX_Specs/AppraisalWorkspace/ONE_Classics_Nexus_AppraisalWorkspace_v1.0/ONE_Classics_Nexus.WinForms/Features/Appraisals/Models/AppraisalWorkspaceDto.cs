using System.Text.Json.Serialization;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

public sealed class AppraisalWorkspaceDto
{
    public Guid AppraisalId { get; set; }
    public string? AppraisalNumber { get; set; }
    public Guid? AppraisalRequestId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? AppraisalTypeId { get; set; }
    public Guid? AppraisalStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string AppraisalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IntakeStatus { get; set; }
    public string? InspectionStatus { get; set; }
    public string? ValuationStatus { get; set; }
    public string? ReportStatus { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
    public long RowVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public VehicleSummaryDto? Vehicle { get; set; }
    public ClientSummaryDto? Client { get; set; }
    public UserSummaryDto? AssignedAppraiser { get; set; }
    public UserSummaryDto? AssignedReviewer { get; set; }
    public InspectionSummaryDto? LatestInspection { get; set; }
    public ValuationSummaryDto? LatestValuation { get; set; }
    public ReportSummaryDto? LatestReport { get; set; }
    public int ComparableCount { get; set; }

    [JsonIgnore]
    public string DisplayNumber => string.IsNullOrWhiteSpace(AppraisalNumber)
        ? AppraisalId.ToString("D")
        : AppraisalNumber;

    [JsonIgnore]
    public string VehicleDisplayName => Vehicle?.DisplayName
        ?? (VehicleId == Guid.Empty ? "Subject vehicle unavailable" : $"Vehicle {VehicleId:D}");
}

public sealed class VehicleSummaryDto
{
    public Guid VehicleId { get; set; }
    public Guid? VehicleTypeId { get; set; }
    public string? Vin { get; set; }
    public int? Year { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Trim { get; set; }
    public string? SeriesName { get; set; }
    public string? BodyStyle { get; set; }
    public string? Drivetrain { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public int? Mileage { get; set; }
    public string? OdometerUnit { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }

    [JsonIgnore]
    public string DisplayName
    {
        get
        {
            var identity = string.Join(" ", new[]
            {
                Year?.ToString(), Make, Model, Trim
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            return string.IsNullOrWhiteSpace(identity) ? "Subject vehicle" : identity;
        }
    }
}

public sealed class ClientSummaryDto
{
    public Guid ClientId { get; set; }
    public string? LegalName { get; set; }
    public string? PreferredName { get; set; }
    public string? Status { get; set; }

    [JsonIgnore]
    public string DisplayName => !string.IsNullOrWhiteSpace(PreferredName)
        ? PreferredName!
        : LegalName ?? ClientId.ToString("D");
}

public sealed class UserSummaryDto
{
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }

    [JsonIgnore]
    public string DisplayText => !string.IsNullOrWhiteSpace(DisplayName)
        ? DisplayName!
        : !string.IsNullOrWhiteSpace(Username)
            ? Username!
            : Email ?? UserId.ToString("D");
}

public sealed class InspectionSummaryDto
{
    public Guid InspectionId { get; set; }
    public string? InspectionType { get; set; }
    public string? Status { get; set; }
    public string? InspectionLocation { get; set; }
    public DateTime? InspectedOnUtc { get; set; }
    public decimal? ConditionIndex { get; set; }
    public int CompletedSystemCount { get; set; }
    public int TotalSystemCount { get; set; } = 12;
    public int RedFlagCount { get; set; }
}

public sealed class ValuationSummaryDto
{
    public Guid ValuationRunId { get; set; }
    public string? ValuationStatus { get; set; }
    public string? Status { get; set; }
    public string? EngineVersion { get; set; }
    public decimal? ConditionIndex { get; set; }
    public decimal? ConfidenceScore { get; set; }
    public string? ConfidenceBand { get; set; }
    public decimal? ValueLow { get; set; }
    public decimal? FinalValue { get; set; }
    public decimal? ValueHigh { get; set; }
    public bool ManualReviewRequired { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? ExecutedAtUtc { get; set; }
}

public sealed class ReportSummaryDto
{
    public Guid AppraisalReportId { get; set; }
    public Guid? ValuationRunId { get; set; }
    public string? ReportType { get; set; }
    public string? ReportVersion { get; set; }
    public string? ReportStatus { get; set; }
    public string? TemplateName { get; set; }
    public DateTime? GeneratedOnUtc { get; set; }
}
