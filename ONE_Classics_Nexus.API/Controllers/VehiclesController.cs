using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using CreateVehicleCommand = ONE_Classics_Nexus.Application.Contracts.Vehicles.CreateVehicleCommand;
using UpdateVehicleCommand = ONE_Classics_Nexus.Application.Contracts.Vehicles.UpdateVehicleCommand;
using VehicleDto = ONE_Classics_Nexus.Application.Contracts.Vehicles.VehicleDto;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
    }

    [HttpPost]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VehicleResponse>> Create(
        [FromBody] CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Request body is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (string.IsNullOrWhiteSpace(request.Vin))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "VIN is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (string.IsNullOrWhiteSpace(request.Make))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Make is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Model is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (request.Year < 1886 || request.Year > 3000)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Year must be between 1886 and 3000.",
                Instance = HttpContext.Request.Path
            });
        }

        if (request.Mileage < 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Mileage must be zero or greater.",
                Instance = HttpContext.Request.Path
            });
        }

        var command = new CreateVehicleCommand
        {
            Vin = request.Vin,
            Year = request.Year,
            Make = request.Make,
            Model = request.Model,
            Trim = request.Trim,
            BodyStyle = request.BodyStyle,
            Engine = request.Engine,
            Transmission = request.Transmission,
            Drivetrain = request.Drivetrain,
            Mileage = request.Mileage
        };

        var created = await _vehicleService.CreateAsync(command, cancellationToken);
        var response = MapToResponse(created);

        return CreatedAtAction(nameof(GetById), new { id = response.VehicleId }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id, cancellationToken);

        if (vehicle is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Vehicle '{id}' was not found.",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(MapToResponse(vehicle));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleResponse>> Update(
        Guid id,
        [FromBody] UpdateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Request body is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (string.IsNullOrWhiteSpace(request.Vin))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "VIN is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (string.IsNullOrWhiteSpace(request.Make))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Make is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (string.IsNullOrWhiteSpace(request.Model))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Model is required.",
                Instance = HttpContext.Request.Path
            });
        }

        if (request.Year < 1886 || request.Year > 3000)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Year must be between 1886 and 3000.",
                Instance = HttpContext.Request.Path
            });
        }

        if (request.Mileage < 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Mileage must be zero or greater.",
                Instance = HttpContext.Request.Path
            });
        }

        var command = new UpdateVehicleCommand
        {
            VehicleId = id,
            Vin = request.Vin,
            Year = request.Year,
            Make = request.Make,
            Model = request.Model,
            Trim = request.Trim,
            BodyStyle = request.BodyStyle,
            Engine = request.Engine,
            Transmission = request.Transmission,
            Drivetrain = request.Drivetrain,
            Mileage = request.Mileage
        };

        var updated = await _vehicleService.UpdateAsync(command, cancellationToken);

        if (updated is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Vehicle '{id}' was not found.",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(MapToResponse(updated));
    }

    private static VehicleResponse MapToResponse(VehicleDto vehicle)
    {
        return new VehicleResponse
        {
            VehicleId = vehicle.VehicleId,
            Vin = vehicle.Vin ?? string.Empty,
            Year = vehicle.Year,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Trim = vehicle.Trim,
            BodyStyle = vehicle.BodyStyle,
            Engine = vehicle.Engine,
            Transmission = vehicle.Transmission,
            Drivetrain = vehicle.Drivetrain,
            Mileage = vehicle.Mileage ?? 0
        };
    }
}