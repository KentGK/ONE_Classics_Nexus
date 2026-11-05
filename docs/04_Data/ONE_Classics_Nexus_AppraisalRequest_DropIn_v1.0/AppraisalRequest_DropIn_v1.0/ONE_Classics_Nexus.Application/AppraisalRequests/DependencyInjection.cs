using Microsoft.Extensions.DependencyInjection;
namespace ONE_Classics_Nexus.Application.AppraisalRequests;
public static class DependencyInjection
{
    public static IServiceCollection AddAppraisalRequestsApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAppraisalRequestClock, SystemAppraisalRequestClock>();
        services.AddScoped<IAppraisalRequestService, AppraisalRequestService>();
        return services;
    }
}
