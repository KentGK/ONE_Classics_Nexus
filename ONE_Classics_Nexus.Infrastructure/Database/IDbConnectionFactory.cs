using System.Data;

namespace ONE_Classics_Nexus.Infrastructure.Database
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
    }
}