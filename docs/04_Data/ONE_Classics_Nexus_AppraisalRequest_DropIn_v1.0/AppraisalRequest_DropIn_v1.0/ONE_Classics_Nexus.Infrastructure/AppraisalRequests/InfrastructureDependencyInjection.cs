using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddAppraisalRequestsInfrastructure(this IServiceCollection services,IConfiguration config)
    {
        var cs=config.GetConnectionString("Nexus") ?? throw new InvalidOperationException("ConnectionStrings:Nexus is required.");
        services.AddSingleton<IAppraisalRequestConnectionFactory>(_=>new AppraisalRequestConnectionFactory(cs));
        services.AddScoped<IAppraisalRequestRepository,AppraisalRequestRepository>();
        services.AddScoped<IAppraisalRequestNumberGenerator,AppraisalRequestNumberGenerator>();
        services.AddScoped<IAppraisalRequestAuditWriter,AppraisalRequestAuditWriter>();
        services.AddScoped<IAppraisalRequestConversionGateway,ExistingAppraisalConversionGateway>();
        return services;
    }
}
