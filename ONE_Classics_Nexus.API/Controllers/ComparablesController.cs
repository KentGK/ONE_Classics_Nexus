using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/appraisals/{appraisalId:guid}/comparables")]
public sealed class ComparablesController : ControllerBase
{
    private readonly IComparableRepository _repository;

    public ComparablesController(IComparableRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<ComparableDto>> Create(
        Guid appraisalId,
        [FromBody] CreateComparableRequest request,
        CancellationToken ct)
    {
        var created = await _repository.CreateAsync(appraisalId, request, ct);

        return CreatedAtAction(
            nameof(GetByAppraisalId),
            new { appraisalId },
            created);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ComparableDto>>> GetByAppraisalId(
        Guid appraisalId,
        CancellationToken ct)
    {
        var rows = await _repository.GetByAppraisalIdAsync(appraisalId, ct);
        return Ok(rows);
    }
}