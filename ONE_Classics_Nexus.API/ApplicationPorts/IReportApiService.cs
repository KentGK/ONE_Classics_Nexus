using ONE_Classics_Nexus.API.Contracts.Reports;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IReportApiService
{
    Task<ReportResponse> CreateAsync(
        Guid appraisalId,
        CreateReportRequest request,
        CancellationToken cancellationToken);

    Task<ReportResponse?> GetLatestByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);
}
