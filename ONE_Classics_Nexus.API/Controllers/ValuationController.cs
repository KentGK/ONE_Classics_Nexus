using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Valuation;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
public sealed class ValuationController : ControllerBase
{
    private readonly IValuationApiService _service;

    public ValuationController(IValuationApiService service) => _service = service;

    /// <summary>
    /// Run a valuation for an appraisal
    /// </summary>
    [HttpPost("api/appraisals/{appraisalId:guid}/valuation/run")]
    [ProducesResponseType(typeof(ValuationRunResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ValuationRunResponse>> Run(Guid appraisalId, [FromBody] RunValuationRequest? request, CancellationToken cancellationToken)
    {
        if (appraisalId == Guid.Empty) return ValidationProblem("AppraisalId is required.");
        var result = await _service.RunAsync(appraisalId, request ?? new RunValuationRequest(null, null, null, null), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get all valuation runs for an appraisal
    /// </summary>
    [HttpGet("api/appraisals/{appraisalId:guid}/valuation/runs")]
    [ProducesResponseType(typeof(ValuationRunListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ValuationRunListResponse>> GetRuns(Guid appraisalId, CancellationToken cancellationToken)
    {
        var result = await _service.GetRunsByAppraisalIdAsync(appraisalId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get valuation run details by ID
    /// </summary>
    [HttpGet("api/valuation/runs/{valuationRunId:guid}")]
    [ProducesResponseType(typeof(ValuationRunDetailResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ValuationRunDetailResponse>> GetRunDetail(Guid valuationRunId, CancellationToken cancellationToken)
    {
        var result = await _service.GetRunDetailAsync(valuationRunId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
