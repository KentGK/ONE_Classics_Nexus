using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Infrastructure.Database;
using AppraisalEntity = ONE_Classics_Nexus.Domain.Entities.Appraisal;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class AppraisalRepository : IAppraisalRepository
{
    private readonly MySqlConnectionFactory _factory;

    public AppraisalRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public Task<AppraisalEntity> CreateAsync(AppraisalEntity appraisal, CancellationToken ct) =>
        new Appraisal.AppraisalRepository(_factory).CreateAsync(appraisal, ct);

    public async Task<AppraisalEntity?> GetByIdAsync(Guid appraisalId, CancellationToken ct)
    {
        const string sql = """
            SELECT
                appraisal_id     AS AppraisalId,
                vehicle_id       AS VehicleId,
                appraisal_number AS AppraisalNumber,
                appraisal_request_id AS AppraisalRequestId,
                client_id        AS ClientId,
                assigned_appraiser_id AS AssignedAppraiserId,
                assigned_reviewer_id AS AssignedReviewerId,
                appraisal_type   AS AppraisalType,
                status           AS Status,
                intake_status    AS IntakeStatus,
                inspection_status AS InspectionStatus,
                valuation_status AS ValuationStatus,
                report_status    AS ReportStatus,
                effective_date_utc AS EffectiveDateUtc,
                created_at_utc   AS CreatedAtUtc,
                updated_at_utc   AS UpdatedAtUtc,
                row_version      AS RowVersion
            FROM appraisals
            WHERE appraisal_id = @AppraisalId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        return await conn.QuerySingleOrDefaultAsync<AppraisalEntity>(new CommandDefinition(
            sql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: ct));
    }

    public Task<PagedResult<AppraisalEntity>> BrowseAsync(AppraisalBrowseFilter filter, CancellationToken ct)
    {
        // Share the existing filtered/paged query rather than duplicate its SQL.
        var repository = new Appraisal.AppraisalRepository(_factory);
        return repository.BrowseAsync(filter, ct);
    }

    public Task<bool> DeleteAsync(Guid appraisalId, CancellationToken ct) =>
        new Appraisal.AppraisalRepository(_factory).DeleteAsync(appraisalId, ct);
}