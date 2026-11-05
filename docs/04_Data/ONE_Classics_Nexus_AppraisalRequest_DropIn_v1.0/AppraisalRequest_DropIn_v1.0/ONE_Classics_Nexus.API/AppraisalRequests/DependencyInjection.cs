using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.API.AppraisalRequests;
public static class DependencyInjection
{
    public static IServiceCollection AddAppraisalRequestsApi(this IServiceCollection services)
    { services.AddHttpContextAccessor(); services.AddScoped<ICurrentNexusUser,HttpCurrentNexusUser>(); return services; }
}
