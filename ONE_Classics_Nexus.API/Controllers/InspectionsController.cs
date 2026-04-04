using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Inspections;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/inspections")]
public sealed class InspectionsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateInspectionHandler handler,
        [FromBody] CreateInspectionRequest request,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(request, ct);
        return Created($"/api/inspections/{result.InspectionId}", result);
    }

    [HttpPut("{inspectionId:guid}/scores")]
    public async Task<IActionResult> SaveScores(
        Guid inspectionId,
        [FromServices] SaveInspectionHandler handler,
        [FromBody] SaveInspectionRequest request,
        CancellationToken ct)
    {
        if (inspectionId != request.InspectionId)
            return BadRequest("Route inspectionId does not match payload inspectionId.");

        var result = await handler.HandleAsync(request, ct);
        return Ok(result);
    }
}