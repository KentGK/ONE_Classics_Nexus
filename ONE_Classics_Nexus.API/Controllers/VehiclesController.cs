using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Vehicles;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVehicleHandler handler,
        [FromBody] CreateVehicleRequest request,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(request, ct);
        return Created($"/api/vehicles/{result.VehicleId}", result);
    }
}