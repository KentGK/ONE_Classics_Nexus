using System.Data;
using Dapper;
using ONE_Classics_Nexus.Application.Valuation.Interfaces;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Valuation.Repositories;

/// <summary>
/// Persists valuation runs and ordered trace lines.
/// </summary>
public sealed class ValuationRunRepository : IValuationRunRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public ValuationRunRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveAsync(Guid appraisalId, ValuationResult result, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(result);

        using IDbConnection conn = _connectionFactory.Create();
        conn.Open();

        using IDbTransaction tx = conn.BeginTransaction();

        try
        {
            Guid valuationRunId = Guid.NewGuid();

            const string insertRunSql = """
                INSERT INTO valuation_runs
                (
                    valuation_run_id,
                    appraisal_id,
                    engine_version,
                    rubric_version,
                    ruleset_version,
                    final_value,
                    low_value,
                    high_value,
                    confidence,
                    status,
                    created_at
                )
                VALUES
                (
                    @ValuationRunId,
                    @AppraisalId,
                    @EngineVersion,
                    @RubricVersion,
                    @RulesetVersion,
                    @FinalValue,
                    @LowValue,
                    @HighValue,
                    @Confidence,
                    @Status,
                    UTC_TIMESTAMP()
                );
                """;

            await conn.ExecuteAsync(
                new CommandDefinition(
                    insertRunSql,
                    new
                    {
                        ValuationRunId = valuationRunId.ToString(),
                        AppraisalId = appraisalId.ToString(),
                        result.EngineVersion,
                        result.RubricVersion,
                        result.RulesetVersion,
                        result.FinalValue,
                        result.LowValue,
                        result.HighValue,
                        Confidence = result.Confidence0To1,
                        Status = result.Status.ToString()
                    },
                    transaction: tx,
                    cancellationToken: ct));

            const string insertTraceSql = """
                INSERT INTO valuation_trace_lines
                (
                    valuation_trace_line_id,
                    valuation_run_id,
                    sort_order,
                    type,
                    trace_key,
                    amount,
                    factor,
                    message,
                    evidence_ref
                )
                VALUES
                (
                    @ValuationTraceLineId,
                    @ValuationRunId,
                    @SortOrder,
                    @Type,
                    @TraceKey,
                    @Amount,
                    @Factor,
                    @Message,
                    @EvidenceRef
                );
                """;

            foreach (ValuationTraceLine line in result.Trace.OrderBy(x => x.SortOrder))
            {
                await conn.ExecuteAsync(
                    new CommandDefinition(
                        insertTraceSql,
                        new
                        {
                            ValuationTraceLineId = Guid.NewGuid().ToString(),
                            ValuationRunId = valuationRunId.ToString(),
                            line.SortOrder,
                            line.Type,
                            TraceKey = line.Key,
                            line.Amount,
                            line.Factor,
                            line.Message,
                            line.EvidenceRef
                        },
                        transaction: tx,
                        cancellationToken: ct));
            }

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}