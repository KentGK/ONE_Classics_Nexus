using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace ONE_Classics_Nexus.Infrastructure.Database;

public sealed class MySqlConnectionFactory
{
    private readonly string _connectionString;

    public MySqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    }

    public MySqlConnection Create()
    {
        return new MySqlConnection(_connectionString);
    }
}