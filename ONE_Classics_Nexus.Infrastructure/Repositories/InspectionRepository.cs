using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class InspectionRepository : IInspectionRepository
{
    private readonly MySqlConnectionFactory _factory;

    public InspectionRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    // ============================================
    // CREATE INSPECTION
    // ============================================
    public async Task<Inspection> CreateAsync(Inspection inspection, CancellationToken ct)
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
                appraisal_id,
                inspection_id,
                subsystem_key,
                score,
                weight,
                is_red_flag,
                notes,
                created_at,
                updated_at
            )
            VALUES
            (
                @ConditionScoreId,
                @AppraisalId,
                @InspectionId,
                @SubsystemKey,
                @Score,
                @Weight,
                @IsRedFlag,
                @Notes,
                UTC_TIMESTAMP(),
                NULL
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);

        // Insert inspection
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

        // Insert scores (Phase 2 schema)
        foreach (var score in inspection.ConditionScores)
        {
            await conn.ExecuteAsync(new CommandDefinition(
                insertScoreSql,
                new
                {
                    ConditionScoreId = score.ConditionScoreId.ToString(),
                    AppraisalId = inspection.AppraisalId.ToString(),
                    InspectionId = inspection.InspectionId.ToString(),
                    SubsystemKey = score.SystemKey,   // map Phase1 → Phase2
                    Score = score.Score0To5 * 2m,     // convert 0–5 → 0–10
                    Weight = score.Weight,
                    IsRedFlag = score.IsRedFlag,
                    Notes = score.Notes
                },
                transaction: tx,
                cancellationToken: ct));
        }

        await tx.CommitAsync(ct);
        return inspection;
    }

    // ============================================
    // GET INSPECTION
    // ============================================
    public async Task<Inspection?> GetByIdAsync(Guid inspectionId, CancellationToken ct)
    {
        const string inspectionSql = """
        SELECT
            inspection_id     AS InspectionId,
            appraisal_id      AS AppraisalId,
            inspection_type   AS InspectionType,
            created_at_utc    AS CreatedAtUtc,
            updated_at_utc    AS UpdatedAtUtc
        FROM inspections
        WHERE inspection_id = @InspectionId;
        """;

        const string scoresSql = """
        SELECT
            condition_score_id AS ConditionScoreId,
            inspection_id      AS InspectionId,
            subsystem_key      AS SystemKey,
            score / 2.0        AS Score0To5,
            weight             AS Weight,
            is_red_flag        AS IsRedFlag,
            notes              AS Notes
        FROM condition_scores
        WHERE inspection_id = @InspectionId
        ORDER BY subsystem_key;
        """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var inspection = await conn.QuerySingleOrDefaultAsync<Inspection>(
            new CommandDefinition(
                inspectionSql,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: ct));

        if (inspection is null)
            return null;

        var scores = await conn.QueryAsync<ConditionScore>(
            new CommandDefinition(
                scoresSql,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: ct));

        inspection.ConditionScores = scores.ToList();
        return inspection;
    }

    // ============================================
    // SAVE SCORES (UPDATE)
    // ============================================
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
                appraisal_id,
                inspection_id,
                subsystem_key,
                score,
                weight,
                is_red_flag,
                notes,
                created_at,
                updated_at
            )
            VALUES
            (
                @ConditionScoreId,
                @AppraisalId,
                @InspectionId,
                @SubsystemKey,
                @Score,
                @Weight,
                @IsRedFlag,
                @Notes,
                UTC_TIMESTAMP(),
                NULL
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

        // Delete existing
        await conn.ExecuteAsync(new CommandDefinition(
            deleteScoresSql,
            new { InspectionId = inspectionId.ToString() },
            transaction: tx,
            cancellationToken: ct));

        // IMPORTANT: get appraisal_id once
        const string getAppraisalIdSql = """
            SELECT CAST(appraisal_id AS CHAR(36)) AS appraisal_id
            FROM inspections
            WHERE inspection_id = @InspectionId;
            """;

        var appraisalId = await conn.QuerySingleAsync<string>(
            new CommandDefinition(
                getAppraisalIdSql,
                new { InspectionId = inspectionId.ToString() },
                transaction: tx,
                cancellationToken: ct));

        // Insert new scores
        foreach (var score in scores)
        {
            await conn.ExecuteAsync(new CommandDefinition(
                insertScoreSql,
                new
                {
                    ConditionScoreId = score.ConditionScoreId.ToString(),
                    AppraisalId = appraisalId,
                    InspectionId = inspectionId.ToString(),
                    SubsystemKey = score.SystemKey,
                    Score = score.Score0To5 * 2m,
                    Weight = score.Weight,
                    IsRedFlag = score.IsRedFlag,
                    Notes = score.Notes
                },
                transaction: tx,
                cancellationToken: ct));
        }

        // Update inspection timestamp
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