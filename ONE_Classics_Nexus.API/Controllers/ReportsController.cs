using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Reports;
namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/appraisals/{appraisalId:guid}/report")]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportApiService _service;

    public ReportsController(IReportApiService service) => _service = service;

    /// <summary>
    /// Create a new report for an appraisal
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReportResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ReportResponse>> Create(Guid appraisalId, [FromBody] CreateReportRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.TemplateKey)) return ValidationProblem("TemplateKey is required.");
        if (string.IsNullOrWhiteSpace(request.OutputFormat)) return ValidationProblem("OutputFormat is required.");

        var created = await _service.CreateAsync(appraisalId, request, cancellationToken);
        return CreatedAtAction(nameof(GetLatest), new { appraisalId }, created);
    }

    /// <summary>
    /// Get the latest report for an appraisal
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ReportResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ReportResponse>> GetLatest(Guid appraisalId, CancellationToken cancellationToken)
    {
        var result = await _service.GetLatestByAppraisalIdAsync(appraisalId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
