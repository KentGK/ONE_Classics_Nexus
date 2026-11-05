using System.Data.Common;
using MySqlConnector;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;
public interface IAppraisalRequestConnectionFactory { Task<DbConnection> OpenAsync(CancellationToken ct); }
public sealed class AppraisalRequestConnectionFactory(string connectionString) : IAppraisalRequestConnectionFactory
{
    public async Task<DbConnection> OpenAsync(CancellationToken ct) { var c = new MySqlConnection(connectionString); await c.OpenAsync(ct); return c; }
}
