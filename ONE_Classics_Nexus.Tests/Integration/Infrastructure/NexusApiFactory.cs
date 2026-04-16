using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ONE_Classics_Nexus.Tests.Integration.Infrastructure;

public sealed class NexusApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: true);
            config.AddJsonFile("appsettings.Test.json", optional: true);
            config.AddEnvironmentVariables();
        });
    }
}