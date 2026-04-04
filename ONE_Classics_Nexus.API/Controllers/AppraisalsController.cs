using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Appraisals;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/appraisals")]
public sealed class AppraisalsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateAppraisalHandler handler,
        [FromBody] CreateAppraisalRequest request,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(request, ct);
        return Created($"/api/appraisals/{result.AppraisalId}", result);
    }
}