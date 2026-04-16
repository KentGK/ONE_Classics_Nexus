using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace ONE_Classics_Nexus.Infrastructure.Database
{
    public sealed class MariaDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MariaDbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
         
            Console.WriteLine("TEST DB CONNECTION: " + _connectionString);
        }

        public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}