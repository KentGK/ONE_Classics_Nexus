using Dapper;
using ONE_Classics_Nexus.Application.Valuation.Interfaces;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using ONE_Classics_Nexus.Infrastructure.Database;
using System.Data;

namespace ONE_Classics_Nexus.Infrastructure.Valuation.Queries;

/// <summary>
/// Database-backed query service for loading valuation input data.
/// </summary>
public sealed class AppraisalQueryService : IAppraisalQueryService
{
    private readonly MySqlConnectionFactory _connectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppraisalQueryService"/> class.
    /// </summary>
    public AppraisalQueryService(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <inheritdoc />
    public async Task<SubjectVehicle> GetSubjectVehicleAsync(Guid appraisalId, CancellationToken ct)
    {
        using IDbConnection conn = _connectionFactory.Create();

        const string sql = """
            SELECT
                v.vehicle_id      AS VehicleId,
                v.year            AS Year,
                v.make            AS Make,
                v.model           AS Model,
                v.trim            AS Trim,
                v.body_style      AS BodyStyle,
                v.engine          AS Engine,
                v.transmission    AS Transmission,
                v.drivetrain      AS Drivetrain,
                v.series          AS Series,
                v.mileage         AS Mileage,
                v.vin             AS Vin
            FROM appraisals a
            INNER JOIN vehicles v
                ON v.vehicle_id = a.vehicle_id
            WHERE a.appraisal_id = @AppraisalId
            LIMIT 1;
            """;

        SubjectVehicle? subject = await conn.QuerySingleOrDefaultAsync<SubjectVehicle>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: ct));

        return subject ?? throw new InvalidOperationException(
            $"No subject vehicle found for appraisal '{appraisalId}'.");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ConditionScore>> GetConditionScoresAsync(Guid appraisalId, CancellationToken ct)
    {
        using IDbConnection conn = _connectionFactory.Create();

        // condition_scores has no appraisal_id; it is linked via inspections.
        // Scores are persisted on a 0-5 scale and scaled to the 0-10 scale the
        // valuation engine expects. Weights are already normalized to total 1.0.
        const string sql = """
        SELECT
            cs.system_key        AS SystemKey,
            cs.score_0_to_5 * 2  AS Score0To10,
            cs.weight            AS Weight0To1,
            cs.is_red_flag       AS IsRedFlag,
            cs.notes             AS Notes
        FROM condition_scores cs
        INNER JOIN inspections i
            ON i.inspection_id = cs.inspection_id
        WHERE i.appraisal_id = @AppraisalId
          AND i.inspection_id = (
                SELECT i2.inspection_id
                FROM inspections i2
                WHERE i2.appraisal_id = @AppraisalId
                ORDER BY i2.inspected_on_utc DESC, i2.created_at_utc DESC
                LIMIT 1
          )
        ORDER BY cs.system_number;
        """;

        IEnumerable<ConditionScore> rows = await conn.QueryAsync<ConditionScore>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: ct));

        return rows.ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ComparableSale>> GetComparableSalesAsync(Guid appraisalId, CancellationToken ct)
    {
        using IDbConnection conn = _connectionFactory.Create();

        const string sql = """
        SELECT
            comparable_id AS ComparableId,
            year AS Year,
            make AS Make,
            model AS Model,
            trim AS Trim,
            body_style AS BodyStyle,
            engine AS Engine,
            transmission AS Transmission,
            drivetrain AS Drivetrain,
            series AS Series,
            sale_price AS SalePrice,
            sale_date AS SaleDateUtc,
            source AS Source,
            source_listing_url AS Url,
            mileage AS Mileage,
            venue_type AS Venue,
            notes AS Notes
        FROM comparable_sales
        WHERE appraisal_id = @AppraisalId
        ORDER BY sale_date DESC, sale_price DESC;
        """;

        IEnumerable<ComparableSale> rows = await conn.QueryAsync<ComparableSale>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: ct));

        return rows.ToList();
    }
}