using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace ONE_Classics_Nexus.Infrastructure.Database;

public sealed class MariaDbConnectionFactory
{
    private readonly string _connectionString;

    public MariaDbConnectionFactory(IConfiguration configuration)
    {
        // Option 1: Read from appsettings.json
        var configured = configuration.GetConnectionString("MariaDb");

        // Option 2: Fallback (hardcoded for Phase 1 dev)
        _connectionString = !string.IsNullOrWhiteSpace(configured)
            ? configured
            : BuildDefaultConnectionString();
    }

    public MySqlConnection Create()
    {
        return new MySqlConnection(_connectionString);
    }

    private static string BuildDefaultConnectionString()
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Port = 3309,
            Database = "one_classics_nexus",
            UserID = "root",
            Password = "KentK1015",

            // Recommended settings
            AllowUserVariables = true,
            SslMode = MySqlSslMode.None,

            // Optional performance tuning
            ConnectionTimeout = 15,
            DefaultCommandTimeout = 30
        };

        return builder.ConnectionString;
    }
}