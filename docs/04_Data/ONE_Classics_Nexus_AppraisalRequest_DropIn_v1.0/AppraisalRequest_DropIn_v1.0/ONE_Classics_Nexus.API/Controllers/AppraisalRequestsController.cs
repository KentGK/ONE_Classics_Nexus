using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Domain.AppraisalRequests;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController, Route("api/appraisal-requests"), Authorize]
public sealed class AppraisalRequestsController(IAppraisalRequestService service) : ControllerBase
{
    [HttpPost] public Task<IActionResult> Create(CreateAppraisalRequestRequest r,CancellationToken ct)=>Run(async()=>{var x=await service.CreateAsync(r,ct);return CreatedAtAction(nameof(Get),new{id=x.AppraisalRequestId},x);});
    [HttpGet("{id:guid}")] public Task<IActionResult> Get(Guid id,CancellationToken ct)=>Run(async()=>Ok(await service.GetAsync(id,ct)));
    [HttpGet] public Task<IActionResult> Search([FromQuery]AppraisalRequestSearchCriteria q,CancellationToken ct)=>Run(async()=>Ok(await service.SearchAsync(q,ct)));
    [HttpPut("{id:guid}")] public Task<IActionResult> Update(Guid id,UpdateAppraisalRequestRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.UpdateAsync(id,r,ct)));
    [HttpPost("{id:guid}/submit")] public Task<IActionResult> Submit(Guid id,RowVersionRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.SubmitAsync(id,r.RowVersion,ct)));
    [HttpPost("{id:guid}/begin-review")] public Task<IActionResult> BeginReview(Guid id,RowVersionRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.BeginReviewAsync(id,r.RowVersion,ct)));
    [HttpPost("{id:guid}/needs-information")] public Task<IActionResult> NeedsInfo(Guid id,NeedsInformationRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.NeedsInformationAsync(id,r.Notes,r.RowVersion,ct)));
    [HttpPost("{id:guid}/qualify")] public Task<IActionResult> Qualify(Guid id,QualifyRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.QualifyAsync(id,r.Notes,r.RowVersion,ct)));
    [HttpPost("{id:guid}/accept")] public Task<IActionResult> Accept(Guid id,RowVersionRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.AcceptAsync(id,r.RowVersion,ct)));
    [HttpPost("{id:guid}/decline")] public Task<IActionResult> Decline(Guid id,ReasonRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.DeclineAsync(id,r.Reason,r.RowVersion,ct)));
    [HttpPost("{id:guid}/cancel")] public Task<IActionResult> Cancel(Guid id,ReasonRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.CancelAsync(id,r.Reason,r.RowVersion,ct)));
    [HttpPost("{id:guid}/withdraw")] public Task<IActionResult> Withdraw(Guid id,ReasonRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.WithdrawAsync(id,r.Reason,r.RowVersion,ct)));
    [HttpPost("{id:guid}/assign")] public Task<IActionResult> Assign(Guid id,AssignRequest r,CancellationToken ct)=>Run(async()=>Ok(await service.AssignAsync(id,r.AssignedToUserId,r.RowVersion,ct)));
    [HttpPost("{id:guid}/convert")] public Task<IActionResult> Convert(Guid id,ConvertAppraisalRequestCommand r,CancellationToken ct)=>Run(async()=>{var x=await service.ConvertAsync(id,r,ct);return Created($"/api/appraisals/{x.AppraisalId}",x);});

    private async Task<IActionResult> Run(Func<Task<IActionResult>> action)
    {
        try{return await action();}
        catch(AppraisalRequestApplicationException ex){return ProblemWithCode(ex.StatusCode,ex.Code,ex.Message);}
        catch(AppraisalRequestDomainException ex){return ProblemWithCode(422,ex.Code,ex.Message);}
    }
    private ObjectResult ProblemWithCode(int status,string code,string detail)
    { var pd=new ProblemDetails{Status=status,Title="Appraisal request operation failed.",Detail=detail};pd.Extensions["code"]=code;return StatusCode(status,pd); }
}
