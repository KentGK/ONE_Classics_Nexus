using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Inspections;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/inspections")]
public sealed class InspectionsController : ControllerBase
{
    private readonly IInspectionApiService _service;

    public InspectionsController(IInspectionApiService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InspectionResponse>> Create(
        [FromBody] CreateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { inspectionId = response.InspectionId },
            response);
    }

    [HttpGet("{inspectionId:guid}")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionResponse>> GetById(
        [FromRoute] Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var response = await _service.GetByIdAsync(inspectionId, cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    [HttpGet("/api/appraisals/{appraisalId:guid}/inspections")]
    [ProducesResponseType(typeof(InspectionListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<InspectionListResponse>> GetByAppraisalId(
        [FromRoute] Guid appraisalId,
        CancellationToken cancellationToken)
    {
        var response = await _service.GetByAppraisalIdAsync(appraisalId, cancellationToken);

        return Ok(response);
    }

    [HttpPut("{inspectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid inspectionId,
        [FromBody] UpdateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(inspectionId, request, cancellationToken);

        return updated
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{inspectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid inspectionId,
        CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(inspectionId, cancellationToken);

        return deleted
            ? NoContent()
            : NotFound();
    }

    [HttpPut("{inspectionId:guid}/scores")]
    [ProducesResponseType(typeof(InspectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionResponse>> SaveScores(
        [FromRoute] Guid inspectionId,
        [FromBody] SaveInspectionScoresRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.SaveScoresAsync(inspectionId, request, cancellationToken);

        return response is null
            ? NotFound()
            : Ok(response);
    }
}
