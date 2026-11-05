using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.ConditionScores;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/inspections/{inspectionId:guid}/scores")]
public sealed class ConditionScoresController : ControllerBase
{
    private readonly IConditionScoreApiService _service;

    public ConditionScoresController(IConditionScoreApiService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all condition scores for an inspection
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ConditionScoreListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConditionScoreListResponse>> GetByInspectionId(
        [FromRoute] Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var response = await _service.GetByInspectionIdAsync(
            inspectionId,
            cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    /// <summary>
    /// Get a specific condition score by ID
    /// </summary>
    [HttpGet("{scoreId:guid}")]
    [ProducesResponseType(typeof(ConditionScoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConditionScoreResponse>> GetById(
        [FromRoute] Guid inspectionId,
        [FromRoute] Guid scoreId,
        CancellationToken cancellationToken)
    {
        var response = await _service.GetByIdAsync(
            inspectionId,
            scoreId,
            cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    /// <summary>
    /// Create a new condition score for an inspection
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConditionScoreResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConditionScoreResponse>> Create(
        [FromRoute] Guid inspectionId,
        [FromBody] CreateConditionScoreRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(
            inspectionId,
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { inspectionId, scoreId = response.ConditionScoreId },
            response);
    }

    /// <summary>
    /// Update an existing condition score
    /// </summary>
    [HttpPut("{scoreId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid inspectionId,
        [FromRoute] Guid scoreId,
        [FromBody] UpdateConditionScoreRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(
            inspectionId,
            scoreId,
            request,
            cancellationToken);

        return updated
            ? NoContent()
            : NotFound();
    }

    /// <summary>
    /// Delete a condition score
    /// </summary>
    [HttpDelete("{scoreId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid inspectionId,
        [FromRoute] Guid scoreId,
        CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(
            inspectionId,
            scoreId,
            cancellationToken);

        return deleted
            ? NoContent()
            : NotFound();
    }
}