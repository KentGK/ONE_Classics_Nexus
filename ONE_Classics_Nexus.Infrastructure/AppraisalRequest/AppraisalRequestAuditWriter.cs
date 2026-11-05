using Dapper;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;
public sealed class AppraisalRequestAuditWriter(IAppraisalRequestConnectionFactory factory) : IAppraisalRequestAuditWriter
{
    public async Task WriteAsync(Guid requestId, Guid? actor, string action, string? prior, string next, string? reason, Guid? appraisalId, CancellationToken ct)
    {
        const string sql = @"INSERT INTO appraisal_request_audit(appraisal_request_audit_id,appraisal_request_id,actor_user_id,occurred_at_utc,action,prior_status,new_status,reason,appraisal_id)
VALUES(@Id,@RequestId,@Actor,UTC_TIMESTAMP(6),@Action,@Prior,@Next,@Reason,@AppraisalId)";
        await using var c = await factory.OpenAsync(ct); await c.ExecuteAsync(new CommandDefinition(sql, new { Id = Guid.NewGuid(), RequestId = requestId, Actor = actor, Action = action, Prior = prior, Next = next, Reason = reason, AppraisalId = appraisalId }, cancellationToken: ct));
    }
}
