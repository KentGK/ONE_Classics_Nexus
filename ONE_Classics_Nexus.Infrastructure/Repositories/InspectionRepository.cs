using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.Database;
using InspectionEntity = ONE_Classics_Nexus.Domain.Entities.Inspection;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class InspectionRepository : IInspectionRepository
{
    private readonly MySqlConnectionFactory _factory;

    public InspectionRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<InspectionEntity> CreateAsync(InspectionEntity inspection, CancellationToken ct)
    {
        const string insertInspectionSql = """
            INSERT INTO inspections
            (
                inspection_id,
                appraisal_id,
                inspection_type,
                created_at_utc,
                updated_at_utc
            )
            VALUES
            (
                @InspectionId,
                @AppraisalId,
                @InspectionType,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        const string insertScoreSql = """
            INSERT INTO condition_scores
            (
                condition_score_id,
                inspection_id,
                system_number,
                system_key,
                score_0_to_5,
                weight,
                is_red_flag,
                notes
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
                @Notes
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);

        await conn.ExecuteAsync(new CommandDefinition(
            insertInspectionSql,
            new
            {
                InspectionId = inspection.InspectionId.ToString(),
                AppraisalId = inspection.AppraisalId.ToString(),
                inspection.InspectionType,
                inspection.CreatedAtUtc,
                inspection.UpdatedAtUtc
            },
            transaction: tx,
            cancellationToken: ct));

        foreach (var score in inspection.ConditionScores)
        {
            await conn.ExecuteAsync(new CommandDefinition(
                insertScoreSql,
                new
                {
                    ConditionScoreId = score.ConditionScoreId.ToString(),
                    InspectionId = score.InspectionId.ToString(),
                    score.SystemNumber,
                    score.SystemKey,
                    score.Score0To5,
                    score.Weight,
                    score.IsRedFlag,
                    score.Notes
                },
                transaction: tx,
                cancellationToken: ct));
        }

        await tx.CommitAsync(ct);
        return inspection;
    }

    public async Task<InspectionEntity?> GetByIdAsync(Guid inspectionId, CancellationToken ct)
    {
        const string inspectionSql = """
            SELECT
                inspection_id     AS InspectionId,
                appraisal_id      AS AppraisalId,
                inspection_type   AS InspectionType,
                status            AS Status,
                created_at_utc    AS CreatedAtUtc,
                updated_at_utc    AS UpdatedAtUtc
            FROM inspections
            WHERE inspection_id = @InspectionId;
            """;

        const string scoresSql = """
            SELECT
                condition_score_id AS ConditionScoreId,
                inspection_id      AS InspectionId,
                system_number      AS SystemNumber,
                system_key         AS SystemKey,
                score_0_to_5       AS Score0To5,
                weight             AS Weight,
                is_red_flag        AS IsRedFlag,
                notes              AS Notes
            FROM condition_scores
            WHERE inspection_id = @InspectionId
            ORDER BY system_number;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var inspection = await conn.QuerySingleOrDefaultAsync<InspectionEntity>(new CommandDefinition(
            inspectionSql,
            new { InspectionId = inspectionId.ToString() },
            cancellationToken: ct));

        if (inspection is null)
            return null;

        var scores = await conn.QueryAsync<ConditionScore>(new CommandDefinition(
            scoresSql,
            new { InspectionId = inspectionId.ToString() },
            cancellationToken: ct));

        inspection.ConditionScores = scores.ToList();
        return inspection;
    }

    public async Task<IReadOnlyList<InspectionEntity>> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken ct)
    {
        const string sql = """
            SELECT
                inspection_id   AS InspectionId,
                appraisal_id    AS AppraisalId,
                inspection_type AS InspectionType,
                status          AS Status,
                created_at_utc  AS CreatedAtUtc,
                updated_at_utc  AS UpdatedAtUtc
            FROM inspections
            WHERE appraisal_id = @AppraisalId
            ORDER BY created_at_utc DESC, inspection_id;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        var inspections = await conn.QueryAsync<InspectionEntity>(new CommandDefinition(
            sql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: ct));

        return inspections.ToArray();
    }

    public async Task SaveScoresAsync(Guid inspectionId, IReadOnlyList<ConditionScore> scores, CancellationToken ct)
    {
        const string deleteScoresSql = """
            DELETE FROM condition_scores
            WHERE inspection_id = @InspectionId;
            """;

        const string insertScoreSql = """
            INSERT INTO condition_scores
            (
                condition_score_id,
                inspection_id,
                system_number,
                system_key,
                score_0_to_5,
                weight,
                is_red_flag,
                notes
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
                @Notes
            );
            """;

        const string updateInspectionSql = """
            UPDATE inspections
            SET updated_at_utc = @UpdatedAtUtc
            WHERE inspection_id = @InspectionId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);

        await conn.ExecuteAsync(new CommandDefinition(
            deleteScoresSql,
            new { InspectionId = inspectionId.ToString() },
            transaction: tx,
            cancellationToken: ct));

        foreach (var score in scores)
        {
            await conn.ExecuteAsync(new CommandDefinition(
                insertScoreSql,
                new
                {
                    ConditionScoreId = score.ConditionScoreId.ToString(),
                    InspectionId = score.InspectionId.ToString(),
                    score.SystemNumber,
                    score.SystemKey,
                    score.Score0To5,
                    score.Weight,
                    score.IsRedFlag,
                    score.Notes
                },
                transaction: tx,
                cancellationToken: ct));
        }

        await conn.ExecuteAsync(new CommandDefinition(
            updateInspectionSql,
            new
            {
                InspectionId = inspectionId.ToString(),
                UpdatedAtUtc = DateTime.UtcNow
            },
            transaction: tx,
            cancellationToken: ct));

        await tx.CommitAsync(ct);
    }
}