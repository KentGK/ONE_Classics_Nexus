using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Vehicle;

public sealed class VehicleTypeRepository : IVehicleTypeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public VehicleTypeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<bool> ExistsByIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default)
    {
        if (vehicleTypeId == Guid.Empty)
        {
            return false;
        }

        const string sql = """
            SELECT COUNT(1)
            FROM vehicle_types
            WHERE vehicle_type_id = @VehicleTypeId
            LIMIT 1;
            """;

        using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var count = await connection.ExecuteScalarAsync<long>(
            new CommandDefinition(
                sql,
                new { VehicleTypeId = vehicleTypeId },
                cancellationToken: cancellationToken));

        return count > 0;
    }

    public async Task<VehicleTypeRecord?> GetByIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default)
    {
        if (vehicleTypeId == Guid.Empty)
        {
            return null;
        }

        const string sql = """
            SELECT
                vehicle_type_id AS VehicleTypeId,
                type_code AS TypeCode,
                type_name AS TypeName,
                description AS Description,
                is_active AS IsActive,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM vehicle_types
            WHERE vehicle_type_id = @VehicleTypeId
            LIMIT 1;
            """;

        using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QuerySingleOrDefaultAsync<VehicleTypeRow>(
            new CommandDefinition(
                sql,
                new { VehicleTypeId = vehicleTypeId },
                cancellationToken: cancellationToken));

        return row is null ? null : Map(row);
    }

    public async Task<IReadOnlyList<VehicleTypeRecord>> ListActiveAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                vehicle_type_id AS VehicleTypeId,
                type_code AS TypeCode,
                type_name AS TypeName,
                description AS Description,
                is_active AS IsActive,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM vehicle_types
            WHERE is_active = 1
            ORDER BY type_name ASC;
            """;

        using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<VehicleTypeRow>(
            new CommandDefinition(
                sql,
                cancellationToken: cancellationToken));

        return rows.Select(Map).ToList();
    }

    private static VehicleTypeRecord Map(VehicleTypeRow row)
    {
        return new VehicleTypeRecord(
            row.VehicleTypeId,
            row.TypeCode,
            row.TypeName,
            row.Description,
            row.IsActive,
            row.CreatedAtUtc,
            row.UpdatedAtUtc);
    }

    private sealed class VehicleTypeRow
    {
        public Guid VehicleTypeId { get; init; }
        public string TypeCode { get; init; } = string.Empty;
        public string TypeName { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public DateTime UpdatedAtUtc { get; init; }
    }
}
