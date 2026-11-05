using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.WinForms.Features.Appraisals;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

namespace ONE_Classics_Nexus.WinForms.DependencyInjection;

public static class AppraisalWorkspaceServiceCollectionExtensions
{
    public static IServiceCollection AddAppraisalWorkspace(
        this IServiceCollection services,
        Uri apiBaseAddress)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(apiBaseAddress);

        if (!apiBaseAddress.IsAbsoluteUri)
            throw new ArgumentException("Nexus API base address must be absolute.", nameof(apiBaseAddress));

        services.AddHttpClient<IAppraisalWorkspaceService, AppraisalWorkspaceService>(client =>
        {
            client.BaseAddress = apiBaseAddress;
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });

        services.AddTransient<AppraisalWorkspaceViewModel>();
        services.AddTransient<AppraisalWorkspaceForm>();
        return services;
    }
}
