using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Valuation.DTOs;
using ONE_Classics_Nexus.Application.Valuation.Interfaces;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/appraisals/{appraisalId:guid}/valuation")]
public sealed class ValuationController : ControllerBase
{
    private readonly IValuationApplicationService _valuationApplicationService;

    public ValuationController(IValuationApplicationService valuationApplicationService)
    {
        _valuationApplicationService = valuationApplicationService;
    }

    [HttpPost("run")]
    [ProducesResponseType(typeof(ValuationResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ValuationResultDto>> Run(Guid appraisalId, CancellationToken ct)
    {
        ValuationResultDto result = await _valuationApplicationService.RunAsync(appraisalId, ct);
        return Ok(result);
    }
}