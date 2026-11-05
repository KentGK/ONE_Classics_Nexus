using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.WinForms.Models.AppraisalRequests;

public static class AppraisalRequestStatuses
{
    public const string Draft = "Draft";
    public const string Submitted = "Submitted";
    public const string UnderReview = "UnderReview";
    public const string Accepted = "Accepted";
    public const string Converted = "Converted";
    public const string Declined = "Declined";
    public const string Cancelled = "Cancelled";
    public const string Withdrawn = "Withdrawn";

    public static readonly IReadOnlyList<string> All =
    [
        Draft,
        Submitted,
        UnderReview,
        Accepted,
        Converted,
        Declined,
        Cancelled,
        Withdrawn
    ];
}

public sealed record AppraisalRequestWorkflowModel(
    string Phase,
    string DataCollectionAreaPath);

public static class AppraisalRequestWorkflow
{
    public const string DataCollectionFormsPath = @"ONE_Classics_Nexus_App\ONE_Classics_Nexus.WinForms\Forms\AppraisalRequests\";

    public static AppraisalRequestWorkflowModel FromStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return new AppraisalRequestWorkflowModel("Data Collection", DataCollectionFormsPath);

        return status switch
        {
            AppraisalRequestStatuses.Accepted => new AppraisalRequestWorkflowModel("Assignment Review", DataCollectionFormsPath),
            AppraisalRequestStatuses.Converted => new AppraisalRequestWorkflowModel("Converted", DataCollectionFormsPath),
            AppraisalRequestStatuses.Declined => new AppraisalRequestWorkflowModel("Closed", DataCollectionFormsPath),
            AppraisalRequestStatuses.Cancelled => new AppraisalRequestWorkflowModel("Closed", DataCollectionFormsPath),
            AppraisalRequestStatuses.Withdrawn => new AppraisalRequestWorkflowModel("Closed", DataCollectionFormsPath),
            _ => new AppraisalRequestWorkflowModel("Data Collection", DataCollectionFormsPath)
        };
    }
}

public sealed record AppraisalRequestListQuery(
    string? Query,
    string? Status,
    Guid? ClientId,
    DateTime? RequestedFromUtc,
    DateTime? RequestedToUtc,
    string Sort,
    int Page,
    int PageSize);

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
}

public sealed record ReferenceOptionDto(
    Guid Id,
    string Code,
    string Name,
    bool IsActive = true);

public sealed record AppraisalRequestListItemDto(
    Guid AppraisalRequestId,
    string RequestNumber,
    string ClientDisplayName,
    string VehicleSummary,
    string Status,
    DateTime? RequestedEffectiveDateUtc,
    DateTime UpdatedAtUtc,
    long RowVersion);

public sealed record IntendedUserDto(
    Guid IntendedUserId,
    Guid IntendedUserTypeId,
    string IntendedUserTypeName,
    string DisplayName,
    string? OrganizationName,
    string RelationshipToClient,
    bool IsClient,
    string? UseRestrictions,
    bool IsActive,
    int SortOrder,
    long RowVersion);

public sealed record AttachmentDto(
    Guid AttachmentId,
    string FileName,
    string ContentType,
    long SizeBytes,
    DateTime CreatedAtUtc,
    string CreatedByDisplayName);

public sealed record AuditEntryDto(
    Guid AuditEntryId,
    DateTime OccurredAtUtc,
    string EventType,
    string ActorDisplayName,
    string Description,
    long? PriorRowVersion,
    long? ResultingRowVersion);

public sealed record AppraisalRequestDetailDto(
    Guid AppraisalRequestId,
    string RequestNumber,
    string Status,
    Guid? ClientId,
    string? ClientDisplayName,
    Guid? PrimaryClientContactId,
    string? PrimaryClientContactName,
    Guid? IntendedUseId,
    string? IntendedUseName,
    string? IntendedUseNarrative,
    DateTime? RequestedEffectiveDateUtc,
    string? RequestedEffectiveDatePrecision,
    string? RequestedEffectiveDateTimeZone,
    Guid? RequestedValueDefinitionId,
    string? RequestedValueDefinitionName,
    string? AppraisalTypeCode,
    string? AppraisalTypeName,
    string? PriorityCode,
    string? RequestedScopeSummary,
    Guid? VehicleId,
    string? VehicleSummary,
    Guid? ConvertedAppraisalId,
    IReadOnlyList<IntendedUserDto> IntendedUsers,
    IReadOnlyList<AttachmentDto> Attachments,
    IReadOnlyList<AuditEntryDto> History,
    long RowVersion,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    bool CanEdit,
    bool CanSubmit,
    bool CanCancel,
    bool CanWithdraw,
    bool CanHardDelete);

public sealed record CreateAppraisalRequestRequest(
    Guid? ClientId,
    Guid? PrimaryClientContactId,
    Guid? IntendedUseId,
    string? IntendedUseNarrative,
    DateTime? RequestedEffectiveDateUtc,
    string? RequestedEffectiveDatePrecision,
    string? RequestedEffectiveDateTimeZone,
    Guid? RequestedValueDefinitionId,
    string? AppraisalTypeCode,
    string? PriorityCode,
    string? RequestedScopeSummary,
    Guid? VehicleId);

public sealed record UpdateAppraisalRequestRequest(
    Guid? ClientId,
    Guid? PrimaryClientContactId,
    Guid? IntendedUseId,
    string? IntendedUseNarrative,
    DateTime? RequestedEffectiveDateUtc,
    string? RequestedEffectiveDatePrecision,
    string? RequestedEffectiveDateTimeZone,
    Guid? RequestedValueDefinitionId,
    string? AppraisalTypeCode,
    string? PriorityCode,
    string? RequestedScopeSummary,
    Guid? VehicleId,
    long ExpectedRowVersion);

public sealed record ExpectedRowVersionRequest(long ExpectedRowVersion);

public sealed record DispositionRequest(
    string Reason,
    long ExpectedRowVersion);

public sealed record ValidationIssueDto(
    string Code,
    string Severity,
    string FieldPath,
    string Message);

public sealed record ValidationResponse(
    IReadOnlyList<ValidationIssueDto> Issues)
{
    public int BlockingCount => Count("ERROR");
    public int WarningCount => Count("WARNING");

    private int Count(string severity)
    {
        var count = 0;
        foreach (var issue in Issues)
        {
            if (string.Equals(issue.Severity, severity, StringComparison.OrdinalIgnoreCase))
                count++;
        }

        return count;
    }
}

public sealed record ApiProblemDetails(
    string? Title,
    string? Detail,
    int? Status,
    IReadOnlyDictionary<string, string[]>? Errors,
    IReadOnlyList<ValidationIssueDto>? Issues);
