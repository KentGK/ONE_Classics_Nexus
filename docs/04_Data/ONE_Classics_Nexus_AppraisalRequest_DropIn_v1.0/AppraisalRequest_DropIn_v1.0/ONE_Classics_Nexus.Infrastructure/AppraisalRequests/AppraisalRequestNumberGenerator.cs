using Dapper;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;
public sealed class AppraisalRequestNumberGenerator(IAppraisalRequestConnectionFactory factory) : IAppraisalRequestNumberGenerator
{
    public async Task<string> NextAsync(DateTime utcNow,CancellationToken ct)
    {
        var year=utcNow.Year; await using var c=await factory.OpenAsync(ct); await using var tx=await c.BeginTransactionAsync(ct);
        const string sql=@"INSERT INTO appraisal_request_sequences(sequence_year,last_value) VALUES(@Year,1)
ON DUPLICATE KEY UPDATE last_value=LAST_INSERT_ID(last_value+1); SELECT IF(ROW_COUNT()=1,1,LAST_INSERT_ID());";
        var n=await c.ExecuteScalarAsync<long>(new CommandDefinition(sql,new{Year=year},tx,cancellationToken:ct)); await tx.CommitAsync(ct);
        return $"AR-{year}-{n:000000}";
    }
}
