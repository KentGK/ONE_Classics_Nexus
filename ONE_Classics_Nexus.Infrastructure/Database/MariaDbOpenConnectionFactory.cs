using System.Data;

namespace ONE_Classics_Nexus.Infrastructure.Database;

public sealed class MariaDbOpenConnectionFactory : IDbConnectionFactory
{
    private readonly MariaDbConnectionFactory _factory;

    public MariaDbOpenConnectionFactory(MariaDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = _factory.Create();
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
