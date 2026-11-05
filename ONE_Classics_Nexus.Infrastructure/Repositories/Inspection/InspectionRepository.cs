using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Infrastructure.Database;
using InspectionEntity = ONE_Classics_Nexus.Domain.Entities.Inspection;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Inspection;

public sealed class InspectionRepository : IInspectionRepository
{
    private readonly MySqlConnectionFactory _factory;

    public InspectionRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<InspectionEntity> InsertAsync(
        InspectionEntity inspection,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO inspections
            (
                inspection_id,
                appraisal_id,
                inspection_template_id,
                inspected_by_user_id,
                inspection_type,
                status,
                inspection_location,
                inspected_on_utc,
                completion_percentage,
                overall_condition_score,
                has_red_flags,
                summary_notes,
                row_version,
                created_at_utc,
                updated_at_utc
            )
            VALUES
            (
                @InspectionId,
                @AppraisalId,
                @InspectionTemplateId,
                @InspectedByUserId,
                @InspectionType,
                @Status,
                @InspectionLocation,
                @InspectedOnUtc,
                @CompletionPercentage,
                @OverallConditionScore,
                @HasRedFlags,
                @SummaryNotes,
                @RowVersion,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(inspection),
            cancellationToken: cancellationToken));

        return inspection;
    }

    public async Task<InspectionEntity?> GetByIdAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                inspection_id AS InspectionId,
                appraisal_id AS AppraisalId,
                inspection_template_id AS InspectionTemplateId,
                inspected_by_user_id AS InspectedByUserId,
                inspection_type AS InspectionType,
                status AS Status,
                inspection_location AS InspectionLocation,
                inspected_on_utc AS InspectedOnUtc,
                completion_percentage AS CompletionPercentage,
                overall_condition_score AS OverallConditionScore,
                has_red_flags AS HasRedFlags,
                summary_notes AS SummaryNotes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM inspections
            WHERE inspection_id = @InspectionId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        return await conn.QuerySingleOrDefaultAsync<InspectionEntity>(
            new CommandDefinition(
                sql,
                new { InspectionId = inspectionId.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<InspectionEntity>> GetByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                inspection_id AS InspectionId,
                appraisal_id AS AppraisalId,
                inspection_template_id AS InspectionTemplateId,
                inspected_by_user_id AS InspectedByUserId,
                inspection_type AS InspectionType,
                status AS Status,
                inspection_location AS InspectionLocation,
                inspected_on_utc AS InspectedOnUtc,
                completion_percentage AS CompletionPercentage,
                overall_condition_score AS OverallConditionScore,
                has_red_flags AS HasRedFlags,
                summary_notes AS SummaryNotes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM inspections
            WHERE appraisal_id = @AppraisalId
            ORDER BY created_at_utc DESC;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var rows = await conn.QueryAsync<InspectionEntity>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: cancellationToken));

        return rows.ToArray();
    }

    public async Task<IReadOnlyList<InspectionEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                inspection_id AS InspectionId,
                appraisal_id AS AppraisalId,
                inspection_template_id AS InspectionTemplateId,
                inspected_by_user_id AS InspectedByUserId,
                inspection_type AS InspectionType,
                status AS Status,
                inspection_location AS InspectionLocation,
                inspected_on_utc AS InspectedOnUtc,
                completion_percentage AS CompletionPercentage,
                overall_condition_score AS OverallConditionScore,
                has_red_flags AS HasRedFlags,
                summary_notes AS SummaryNotes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM inspections
            ORDER BY created_at_utc DESC;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var result = await conn.QueryAsync<InspectionEntity>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<InspectionEntity?> UpdateAsync(
        InspectionEntity inspection,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE inspections
            SET
                inspection_template_id = @InspectionTemplateId,
                inspected_by_user_id = @InspectedByUserId,
                inspection_type = @InspectionType,
                status = @Status,
                inspection_location = @InspectionLocation,
                inspected_on_utc = @InspectedOnUtc,
                summary_notes = @SummaryNotes,
                row_version = row_version + 1,
                updated_at_utc = @UpdatedAtUtc
            WHERE inspection_id = @InspectionId
              AND row_version = @RowVersion;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var affected = await conn.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(inspection),
            cancellationToken: cancellationToken));

        if (affected == 0)
            return null;

        return await GetByIdAsync(inspection.InspectionId, cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM inspections
            WHERE inspection_id = @InspectionId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var affected = await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { InspectionId = inspectionId.ToString() },
            cancellationToken: cancellationToken));

        return affected > 0;
    }

    public async Task<InspectionEntity?> UpdateScoreSummaryAsync(
        Guid inspectionId,
        decimal completionPercentage,
        decimal overallConditionScore,
        bool hasRedFlags,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE inspections
            SET
                completion_percentage = @CompletionPercentage,
                overall_condition_score = @OverallConditionScore,
                has_red_flags = @HasRedFlags,
                updated_at_utc = @UpdatedAtUtc,
                row_version = row_version + 1
            WHERE inspection_id = @InspectionId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var affected = await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new
            {
                InspectionId = inspectionId.ToString(),
                CompletionPercentage = completionPercentage,
                OverallConditionScore = overallConditionScore,
                HasRedFlags = hasRedFlags,
                UpdatedAtUtc = DateTime.UtcNow
            },
            cancellationToken: cancellationToken));

        if (affected == 0)
            return null;

        return await GetByIdAsync(inspectionId, cancellationToken);
    }

    private static object ToParameters(InspectionEntity inspection)
    {
        return new
        {
            InspectionId = inspection.InspectionId.ToString(),
            AppraisalId = inspection.AppraisalId.ToString(),
            InspectionTemplateId = inspection.InspectionTemplateId.ToString(),
            InspectedByUserId = inspection.InspectedByUserId?.ToString(),
            inspection.InspectionType,
            inspection.Status,
            inspection.InspectionLocation,
            inspection.InspectedOnUtc,
            inspection.CompletionPercentage,
            inspection.OverallConditionScore,
            inspection.HasRedFlags,
            inspection.SummaryNotes,
            inspection.RowVersion,
            inspection.CreatedAtUtc,
            inspection.UpdatedAtUtc
        };
    }
}
