using System.Security.Claims;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.API.AppraisalRequests;
public sealed class HttpCurrentNexusUser(IHttpContextAccessor accessor) : ICurrentNexusUser
{
    public Guid? UserId { get { var s=accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? accessor.HttpContext?.User.FindFirstValue("sub"); return Guid.TryParse(s,out var id)?id:null; } }
}
