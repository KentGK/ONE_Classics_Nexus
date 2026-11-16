using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Infrastructure.Repositories.Inspection;

namespace ONE_Classics_Nexus.Infrastructure.DependencyInjection;

public static class InspectionRepositoryRegistration
{
    public static IServiceCollection AddInspectionRepositories(this IServiceCollection services)
    {
        services.AddScoped<IInspectionRepository, InspectionRepository>();
        services.AddScoped<IConditionScoreRepository, ConditionScoreRepository>();

        return services;
    }
}
