using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.AppraisalRequests;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/appraisal-requests/{appraisalRequestId:guid}/party-identification")]
[Authorize]
public sealed class AppraisalRequestPartyIdentificationController(
    IAppraisalRequestPartyService service,
    ICurrentNexusUser currentUser) : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get(Guid appraisalRequestId, CancellationToken cancellationToken)
    {
        return Run(async () =>
        {
            var response = await service.GetAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false);
            return response is null ? NotFound() : Ok(response);
        });
    }

    [HttpPut("client")]
    public Task<IActionResult> SetClient(Guid appraisalRequestId, SetAppraisalRequestClientRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .SetClientAsync(appraisalRequestId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    [HttpPut("primary-contact")]
    public Task<IActionResult> SetPrimaryClientContact(Guid appraisalRequestId, SetPrimaryClientContactRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .SetPrimaryClientContactAsync(appraisalRequestId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    [HttpPut("proposed-intended-use")]
    public Task<IActionResult> SetProposedIntendedUse(Guid appraisalRequestId, SetIntendedUseRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .SetProposedIntendedUseAsync(appraisalRequestId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    [HttpPost("proposed-intended-users")]
    public Task<IActionResult> AddProposedIntendedUser(Guid appraisalRequestId, CreateIntendedUserRequest request, CancellationToken cancellationToken)
    {
        return Run(async () =>
        {
            var created = await service
                .AddProposedIntendedUserAsync(appraisalRequestId, request, GetActorId(), cancellationToken)
                .ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), new { appraisalRequestId }, created);
        });
    }

    [HttpPut("proposed-intended-users/{intendedUserId:guid}")]
    public Task<IActionResult> UpdateProposedIntendedUser(Guid appraisalRequestId, Guid intendedUserId, UpdateIntendedUserRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .UpdateProposedIntendedUserAsync(appraisalRequestId, intendedUserId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    [HttpDelete("proposed-intended-users/{intendedUserId:guid}")]
    public Task<IActionResult> RemoveProposedIntendedUser(Guid appraisalRequestId, Guid intendedUserId, [FromBody] RemoveIntendedUserRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .RemoveProposedIntendedUserAsync(appraisalRequestId, intendedUserId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    [HttpPost("client-exclusion")]
    public Task<IActionResult> SaveClientExclusion(Guid appraisalRequestId, SaveClientExclusionRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .SaveClientExclusionExplanationAsync(appraisalRequestId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    [HttpPost("validate")]
    public Task<IActionResult> Validate(Guid appraisalRequestId, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service.ValidateAsync(appraisalRequestId, cancellationToken).ConfigureAwait(false)));
    }

    [HttpPost("convert")]
    public Task<IActionResult> Convert(Guid appraisalRequestId, ConvertPartyIdentificationRequest request, CancellationToken cancellationToken)
    {
        return Run(async () => Ok(await service
            .ConvertAsync(appraisalRequestId, request, GetActorId(), cancellationToken)
            .ConfigureAwait(false)));
    }

    private Guid GetActorId()
    {
        var actorId = currentUser.UserId;
        if (!actorId.HasValue || actorId.Value == Guid.Empty)
        {
            throw new AppraisalRequestApplicationException("APPRAISAL_REQUEST_FORBIDDEN", "Authenticated actor id is required.", 403);
        }

        return actorId.Value;
    }

    private async Task<IActionResult> Run(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action().ConfigureAwait(false);
        }
        catch (AppraisalRequestApplicationException ex)
        {
            return ProblemWithCode(ex.StatusCode, ex.Code, ex.Message);
        }
    }

    private ObjectResult ProblemWithCode(int status, string code, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = "Party identification operation failed.",
            Detail = detail
        };

        problem.Extensions["code"] = code;
        return StatusCode(status, problem);
    }
}
