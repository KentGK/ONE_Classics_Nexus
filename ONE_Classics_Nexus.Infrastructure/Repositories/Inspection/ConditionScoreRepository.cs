using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Infrastructure.Database;
using ConditionScoreEntity = ONE_Classics_Nexus.Domain.Entities.ConditionScore;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Inspection;

public sealed class ConditionScoreRepository : IConditionScoreRepository
{
    private readonly MySqlConnectionFactory _factory;

    public ConditionScoreRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<ConditionScoreEntity>> GetByInspectionIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                condition_score_id AS ConditionScoreId,
                inspection_id AS InspectionId,
                system_number AS SystemNumber,
                system_key AS SystemKey,
                score_0_to_5 AS Score0To5,
                weight AS Weight,
                is_red_flag AS IsRedFlag,
                notes AS Notes,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM condition_scores
            WHERE inspection_id = @InspectionId
            ORDER BY system_number;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var rows = await conn.QueryAsync<ConditionScoreEntity>(
            new CommandDefinition(
                sql,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: cancellationToken));

        return rows.ToArray();
    }

    public async Task ReplaceScoresAsync(
        Guid inspectionId,
        IReadOnlyList<ConditionScoreEntity> scores,
        CancellationToken cancellationToken = default)
    {
        const string deleteSql = """
            DELETE FROM condition_scores
            WHERE inspection_id = @InspectionId;
            """;

        const string insertSql = """
            INSERT INTO condition_scores
            (
                condition_score_id,
                inspection_id,
                system_number,
                system_key,
                score_0_to_5,
                weight,
                is_red_flag,
                notes,
                created_at_utc,
                updated_at_utc
            )
            VALUES
            (
                @ConditionScoreId,
                @InspectionId,
                @SystemNumber,
                @SystemKey,
                @Score0To5,
                @Weight,
                @IsRedFlag,
                @Notes,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);
        await using var transaction = await conn.BeginTransactionAsync(cancellationToken);

        try
        {
            await conn.ExecuteAsync(new CommandDefinition(
                deleteSql,
                new { InspectionId = inspectionId.ToString() },
                transaction,
                cancellationToken: cancellationToken));

            foreach (var score in scores)
            {
                await conn.ExecuteAsync(new CommandDefinition(
                    insertSql,
                    ToParameters(score),
                    transaction,
                    cancellationToken: cancellationToken));
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static object ToParameters(ConditionScoreEntity score)
    {
        return new
        {
            ConditionScoreId = score.ConditionScoreId.ToString(),
            InspectionId = score.InspectionId.ToString(),
            score.SystemNumber,
            score.SystemKey,
            score.Score0To5,
            score.Weight,
            score.IsRedFlag,
            score.Notes,
            score.CreatedAtUtc,
            score.UpdatedAtUtc
        };
    }
}
