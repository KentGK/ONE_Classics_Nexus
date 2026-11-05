using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Comparables;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
public sealed class ComparablesController : ControllerBase
{
    private readonly IComparableApiService _service;

    public ComparablesController(IComparableApiService service) => _service = service;

    /// <summary>
    /// Create a new comparable for an appraisal
    /// </summary>
    [HttpPost("api/appraisals/{appraisalId:guid}/comparables")]
    [ProducesResponseType(typeof(ComparableResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ComparableResponse>> Create(Guid appraisalId, [FromBody] CreateComparableRequest request, CancellationToken cancellationToken)
    {
        if (appraisalId == Guid.Empty) return ValidationProblem("AppraisalId is required.");
        if (string.IsNullOrWhiteSpace(request.Make)) return ValidationProblem("Make is required.");
        if (string.IsNullOrWhiteSpace(request.Model)) return ValidationProblem("Model is required.");

        var created = await _service.CreateAsync(appraisalId, request, cancellationToken);
        return CreatedAtAction(nameof(GetByAppraisalId), new { appraisalId }, created);
    }

    /// <summary>
    /// Get all comparables for an appraisal
    /// </summary>
    [HttpGet("api/appraisals/{appraisalId:guid}/comparables")]
    [ProducesResponseType(typeof(ComparableListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ComparableListResponse>> GetByAppraisalId(Guid appraisalId, CancellationToken cancellationToken)
    {
        var result = await _service.GetByAppraisalIdAsync(appraisalId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing comparable
    /// </summary>
    [HttpPut("api/comparables/{comparableId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(Guid comparableId, [FromBody] UpdateComparableRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(comparableId, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }
}
