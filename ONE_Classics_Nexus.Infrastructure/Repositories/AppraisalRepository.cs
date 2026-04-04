using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class AppraisalRepository : IAppraisalRepository
{
    private readonly MySqlConnectionFactory _factory;

    public AppraisalRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Appraisal> CreateAsync(Appraisal appraisal, CancellationToken ct)
    {
        const string sql = """
            INSERT INTO appraisals
            (
                appraisal_id,
                vehicle_id,
                status,
                created_at_utc,
                updated_at_utc,
                row_version
            )
            VALUES
            (
                @AppraisalId,
                @VehicleId,
                @Status,
                @CreatedAtUtc,
                @UpdatedAtUtc,
                @RowVersion
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            appraisal,
            cancellationToken: ct));

        return appraisal;
    }

    public async Task<Appraisal?> GetByIdAsync(Guid appraisalId, CancellationToken ct)
    {
        const string sql = """
            SELECT
                appraisal_id     AS AppraisalId,
                vehicle_id       AS VehicleId,
                status           AS Status,
                created_at_utc   AS CreatedAtUtc,
                updated_at_utc   AS UpdatedAtUtc,
                row_version      AS RowVersion
            FROM appraisals
            WHERE appraisal_id = @AppraisalId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        return await conn.QuerySingleOrDefaultAsync<Appraisal>(new CommandDefinition(
            sql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: ct));
    }
}