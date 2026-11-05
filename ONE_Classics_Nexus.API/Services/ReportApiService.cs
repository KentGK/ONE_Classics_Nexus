using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Reports;
using System.Collections.Concurrent;

namespace ONE_Classics_Nexus.API.Services;

// NOTE: This is a minimal in-memory implementation to unblock compilation.
// Replace with a real implementation backed by an IReportRepository
// (following the same pattern as IAppraisalRepository / IInspectionRepository)
// once the Report domain entity and persistence are added.
public sealed class ReportApiService : IReportApiService
{
    private readonly ConcurrentDictionary<Guid, ReportResponse> _latestByAppraisal = new();

    public Task<ReportResponse> CreateAsync(
        Guid appraisalId,
        CreateReportRequest request,
        CancellationToken cancellationToken)
    {
        var response = new ReportResponse(
            Guid.NewGuid(),
            appraisalId,
            request.TemplateKey,
            request.OutputFormat,
            "Pending",
            null,
            null,
            DateTime.UtcNow);

        _latestByAppraisal[appraisalId] = response;

        return Task.FromResult(response);
    }

    public Task<ReportResponse?> GetLatestByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_latestByAppraisal.TryGetValue(appraisalId, out var response)
            ? response
            : null);
    }
}
