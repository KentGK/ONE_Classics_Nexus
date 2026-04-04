using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.UseCases.Appraisals;
using ONE_Classics_Nexus.Application.UseCases.Inspections;
using ONE_Classics_Nexus.Application.UseCases.Vehicles;
using ONE_Classics_Nexus.Infrastructure.Database;
using ONE_Classics_Nexus.Infrastructure.Repositories;

namespace ONE_Classics_Nexus.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexusInfrastructure(this IServiceCollection services)
    {
        // Register both factory types so existing repositories continue to resolve.
        services.AddSingleton<MySqlConnectionFactory>();
        services.AddSingleton<MariaDbConnectionFactory>();

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IAppraisalRepository, AppraisalRepository>();
        services.AddScoped<IInspectionRepository, InspectionRepository>();

        services.AddScoped<CreateVehicleHandler>();
        services.AddScoped<CreateAppraisalHandler>();
        services.AddScoped<CreateInspectionHandler>();
        services.AddScoped<SaveInspectionHandler>();

        return services;
    }
}