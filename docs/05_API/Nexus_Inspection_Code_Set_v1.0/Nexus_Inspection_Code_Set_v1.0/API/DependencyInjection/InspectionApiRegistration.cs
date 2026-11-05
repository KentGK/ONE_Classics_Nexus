using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Services;

namespace ONE_Classics_Nexus.API.DependencyInjection;

public static class InspectionApiRegistration
{
    public static IServiceCollection AddInspectionApiServices(this IServiceCollection services)
    {
        services.AddScoped<IInspectionApiService, InspectionApiService>();

        return services;
    }
}
