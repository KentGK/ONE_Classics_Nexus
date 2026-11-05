using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.Application.Valuation.Interfaces;
using ONE_Classics_Nexus.Application.Valuation.Services;
using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Engines;
using ONE_Classics_Nexus.Infrastructure.Valuation.Queries;
using ONE_Classics_Nexus.Infrastructure.Valuation.Repositories;

namespace ONE_Classics_Nexus.Infrastructure.DependencyInjection;

public static class ValuationServiceRegistration
{
    public static IServiceCollection AddValuationServices(this IServiceCollection services)
    {
        services.AddScoped<IConditionEngine, DefaultConditionEngine>();
        services.AddScoped<ISimilarityEngine, DefaultSimilarityEngine>();
        services.AddScoped<IComparableEngine, DefaultComparableEngine>();
        services.AddScoped<IMarketEngine, DefaultMarketEngine>();
        services.AddScoped<IConfidenceEngine, DefaultConfidenceEngine>();
        services.AddScoped<IValuationEngine, DefaultValuationEngine>();

        services.AddScoped<IAppraisalQueryService, AppraisalQueryService>();
        services.AddScoped<IValuationApplicationService, ValuationApplicationService>();
        services.AddScoped<IValuationRunRepository, ValuationRunRepository>();

        return services;
    }
}