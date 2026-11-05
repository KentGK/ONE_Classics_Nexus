namespace ONE_Classics_Nexus.API.Contracts.Reports;

public sealed record CreateReportRequest(
    string TemplateKey,
    string OutputFormat,
    string? RequestedBy,
    string? Notes);

public sealed record ReportResponse(
    Guid ReportId,
    Guid AppraisalId,
    string TemplateKey,
    string OutputFormat,
    string Status,
    string? FileName,
    string? DownloadUrl,
    DateTime CreatedAtUtc);
