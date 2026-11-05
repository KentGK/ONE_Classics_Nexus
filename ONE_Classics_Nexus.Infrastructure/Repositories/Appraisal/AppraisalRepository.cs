using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Infrastructure.Database;
using System.Text;
using AppraisalEntity = ONE_Classics_Nexus.Domain.Entities.Appraisal;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Appraisal;

public sealed class AppraisalRepository : IAppraisalRepository
{
    private readonly MySqlConnectionFactory _factory;

    public AppraisalRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<AppraisalEntity> InsertAsync(
        AppraisalEntity appraisal,
        CancellationToken cancellationToken)
    {
        if (appraisal is null) throw new ArgumentNullException(nameof(appraisal));

        // Ensure required defaults align with DB schema defaults
        if (appraisal.AppraisalId == Guid.Empty)
            appraisal.AppraisalId = Guid.NewGuid();

        var now = DateTime.UtcNow;
        if (appraisal.CreatedAtUtc == default)
            appraisal.CreatedAtUtc = now;

        if (appraisal.UpdatedAtUtc == default)
            appraisal.UpdatedAtUtc = now;

        if (appraisal.RowVersion <= 0)
            appraisal.RowVersion = 1;

        const string sql = """
            INSERT INTO appraisals
            (
                appraisal_id,
                vehicle_id,
                appraisal_number,
                appraisal_request_id,
                client_id,
                assigned_appraiser_id,
                assigned_reviewer_id,
                appraisal_type,
                status,
                intake_status,
                inspection_status,
                valuation_status,
                report_status,
                effective_date_utc,
                row_version,
                created_at_utc,
                updated_at_utc
            )
            VALUES
            (
                @AppraisalId,
                @VehicleId,
                @AppraisalNumber,
                @AppraisalRequestId,
                @ClientId,
                @AssignedAppraiserId,
                @AssignedReviewerId,
                @AppraisalType,
                @Status,
                @IntakeStatus,
                @InspectionStatus,
                @ValuationStatus,
                @ReportStatus,
                @EffectiveDateUtc,
                @RowVersion,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(appraisal),
            cancellationToken: cancellationToken));

        return appraisal;
    }

    public Task<AppraisalEntity> CreateAsync(
        AppraisalEntity appraisal,
        CancellationToken ct)
    {
        // Alias to InsertAsync for interface compatibility
        return InsertAsync(appraisal, ct);
    }

    private object? ToParameters(AppraisalEntity appraisal)
    {
        return ToParametersStatic(appraisal);
    }

    private static object ToParametersStatic(AppraisalEntity appraisal)
    {
        return new
        {
            AppraisalId = appraisal.AppraisalId.ToString(),
            VehicleId = appraisal.VehicleId.ToString(),
            appraisal.AppraisalNumber,
            AppraisalRequestId = appraisal.AppraisalRequestId.HasValue ? appraisal.AppraisalRequestId.Value.ToString() : null,
            ClientId = appraisal.ClientId.HasValue ? appraisal.ClientId.Value.ToString() : null,
            AssignedAppraiserId = appraisal.AssignedAppraiserId.HasValue ? appraisal.AssignedAppraiserId.Value.ToString() : null,
            AssignedReviewerId = appraisal.AssignedReviewerId.HasValue ? appraisal.AssignedReviewerId.Value.ToString() : null,
            appraisal.AppraisalType,
            appraisal.Status,
            appraisal.IntakeStatus,
            appraisal.InspectionStatus,
            appraisal.ValuationStatus,
            appraisal.ReportStatus,
            appraisal.EffectiveDateUtc,
            appraisal.RowVersion,
            appraisal.CreatedAtUtc,
            appraisal.UpdatedAtUtc
        };
    }

    public async Task<AppraisalEntity?> GetByIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                appraisal_id            AS AppraisalId,
                vehicle_id              AS VehicleId,
                appraisal_number        AS AppraisalNumber,
                appraisal_request_id    AS AppraisalRequestId,
                client_id               AS ClientId,
                assigned_appraiser_id   AS AssignedAppraiserId,
                assigned_reviewer_id    AS AssignedReviewerId,
                appraisal_type          AS AppraisalType,
                status                  AS Status,
                intake_status           AS IntakeStatus,
                inspection_status       AS InspectionStatus,
                valuation_status        AS ValuationStatus,
                report_status           AS ReportStatus,
                effective_date_utc      AS EffectiveDateUtc,
                row_version             AS RowVersion,
                created_at_utc          AS CreatedAtUtc,
                updated_at_utc          AS UpdatedAtUtc
            FROM appraisals
            WHERE appraisal_id = @AppraisalId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        return await conn.QuerySingleOrDefaultAsync<AppraisalEntity>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task<AppraisalEntity?> UpdateAsync(
        AppraisalEntity appraisal,
        CancellationToken cancellationToken = default)
    {
        if (appraisal is null) throw new ArgumentNullException(nameof(appraisal));

        const string currentVehicleAndStatusSql = """
            SELECT
                vehicle_id AS VehicleId,
                status AS Status
            FROM appraisals
            WHERE appraisal_id = @AppraisalId
            LIMIT 1;
            """;

        // Ensure UpdatedAtUtc is current per DB ON UPDATE semantics
        appraisal.UpdatedAtUtc = DateTime.UtcNow;

        const string sql = """
            UPDATE appraisals
            SET
                vehicle_id = @VehicleId,
                appraisal_number = @AppraisalNumber,
                appraisal_request_id = @AppraisalRequestId,
                client_id = @ClientId,
                assigned_appraiser_id = @AssignedAppraiserId,
                assigned_reviewer_id = @AssignedReviewerId,
                appraisal_type = @AppraisalType,
                status = @Status,
                intake_status = @IntakeStatus,
                inspection_status = @InspectionStatus,
                valuation_status = @ValuationStatus,
                report_status = @ReportStatus,
                effective_date_utc = @EffectiveDateUtc,
                row_version = row_version + 1,
                updated_at_utc = @UpdatedAtUtc
            WHERE appraisal_id = @AppraisalId
              AND row_version = @RowVersion;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var current = await conn.QuerySingleOrDefaultAsync<AppraisalVehicleLinkRow>(
            new CommandDefinition(
                currentVehicleAndStatusSql,
                new { AppraisalId = appraisal.AppraisalId.ToString() },
                cancellationToken: cancellationToken));

        if (current is null)
        {
            return null;
        }

        var isIssued = string.Equals(current.Status, "Issued", StringComparison.OrdinalIgnoreCase);
        var requestedVehicleId = appraisal.VehicleId.ToString();
        var isVehicleRelink = !string.Equals(current.VehicleId, requestedVehicleId, StringComparison.OrdinalIgnoreCase);

        if (isIssued && isVehicleRelink)
        {
            throw new ConflictException("Cannot change vehicle for an issued appraisal.");
        }

        var affected = await conn.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(appraisal),
            cancellationToken: cancellationToken));

        if (affected == 0)
            return null;

        // DB incremented row_version; mirror the change in the entity.
        appraisal.RowVersion++;

        return appraisal;
    }

    public async Task<bool> DeleteAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM appraisals
            WHERE appraisal_id = @AppraisalId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var affected = await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: cancellationToken));

        return affected > 0;
    }

    public async Task<PagedResult<AppraisalEntity>> BrowseAsync(
        AppraisalBrowseFilter filter,
        CancellationToken cancellationToken)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize switch
        {
            < 1 => 25,
            > 200 => 200,
            _ => filter.PageSize
        };

        var where = new StringBuilder(" WHERE 1 = 1 ");
        var parameters = new DynamicParameters();

        if (filter.VehicleId.HasValue)
        {
            where.Append(" AND vehicle_id = @VehicleId ");
            parameters.Add("VehicleId", filter.VehicleId.Value.ToString());
        }

        if (filter.ClientId.HasValue)
        {
            where.Append(" AND client_id = @ClientId ");
            parameters.Add("ClientId", filter.ClientId.Value.ToString());
        }

        if (filter.AssignedAppraiserId.HasValue)
        {
            where.Append(" AND assigned_appraiser_id = @AssignedAppraiserId ");
            parameters.Add("AssignedAppraiserId", filter.AssignedAppraiserId.Value.ToString());
        }

        if (filter.AssignedReviewerId.HasValue)
        {
            where.Append(" AND assigned_reviewer_id = @AssignedReviewerId ");
            parameters.Add("AssignedReviewerId", filter.AssignedReviewerId.Value.ToString());
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            where.Append(" AND status = @Status ");
            parameters.Add("Status", filter.Status);
        }

        if (!string.IsNullOrWhiteSpace(filter.AppraisalType))
        {
            where.Append(" AND appraisal_type = @AppraisalType ");
            parameters.Add("AppraisalType", filter.AppraisalType);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            where.Append(" AND appraisal_number LIKE @Search ");
            parameters.Add("Search", $"%{filter.Search}%");
        }

        if (filter.EffectiveDateFromUtc.HasValue)
        {
            where.Append(" AND effective_date_utc >= @EffectiveDateFromUtc ");
            parameters.Add("EffectiveDateFromUtc", filter.EffectiveDateFromUtc.Value);
        }

        if (filter.EffectiveDateToUtc.HasValue)
        {
            where.Append(" AND effective_date_utc <= @EffectiveDateToUtc ");
            parameters.Add("EffectiveDateToUtc", filter.EffectiveDateToUtc.Value);
        }

        var sortColumn = filter.SortBy?.Trim().ToLowerInvariant() switch
        {
            "effectivedateutc" => "effective_date_utc",
            "status" => "status",
            "appraisalnumber" => "appraisal_number",
            "updatedatutc" => "updated_at_utc",
            _ => "created_at_utc"
        };

        var sortDirection = filter.SortDescending ? "DESC" : "ASC";

        var countSql = $"SELECT COUNT(*) FROM appraisals {where};";

        var pagedSql = $"""
            SELECT
                appraisal_id            AS AppraisalId,
                vehicle_id              AS VehicleId,
                appraisal_number        AS AppraisalNumber,
                appraisal_request_id    AS AppraisalRequestId,
                client_id               AS ClientId,
                assigned_appraiser_id   AS AssignedAppraiserId,
                assigned_reviewer_id    AS AssignedReviewerId,
                appraisal_type          AS AppraisalType,
                status                  AS Status,
                intake_status           AS IntakeStatus,
                inspection_status       AS InspectionStatus,
                valuation_status        AS ValuationStatus,
                report_status           AS ReportStatus,
                effective_date_utc      AS EffectiveDateUtc,
                row_version             AS RowVersion,
                created_at_utc          AS CreatedAtUtc,
                updated_at_utc          AS UpdatedAtUtc
            FROM appraisals
            {where}
            ORDER BY {sortColumn} {sortDirection}
            LIMIT @PageSize OFFSET @Offset;
            """;

        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", (page - 1) * pageSize);

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var totalCount = await conn.ExecuteScalarAsync<long>(
            new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));

        var items = await conn.QueryAsync<AppraisalEntity>(
            new CommandDefinition(pagedSql, parameters, cancellationToken: cancellationToken));

        return new PagedResult<AppraisalEntity>
        {
            Items = items.ToArray(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private sealed class AppraisalVehicleLinkRow
    {
        public string VehicleId { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }
}