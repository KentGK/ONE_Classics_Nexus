using Dapper;
using MySqlConnector;
using ONE_Classics_Nexus.Tests.Integration.Infrastructure;

namespace ONE_Classics_Nexus.Tests.Integration.Infrastructure;

public sealed class TestDatabaseHelper
{
    private readonly string _connectionString;

    public TestDatabaseHelper(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        const string sql = """
        DELETE FROM vehicles
        WHERE vehicle_id = @VehicleId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteVehicleByVinAsync(string vin, CancellationToken cancellationToken = default)
    {
        const string sql = """
        DELETE FROM vehicles
        WHERE vin = @Vin;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { Vin = vin },
            cancellationToken: cancellationToken));
    }

    public async Task<VehicleDbRow?> GetVehicleRowAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        const string sql = """
        SELECT
            vehicle_id   AS Vehicle_Id,
            vin          AS Vin,
            year         AS Year,
            make         AS Make,
            model        AS Model,
            trim         AS Trim,
            body_style   AS Body_Style,
            engine       AS Engine,
            transmission AS Transmission,
            drivetrain   AS Drivetrain,
            mileage      AS Mileage
        FROM vehicles
        WHERE vehicle_id = @VehicleId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        return await conn.QuerySingleOrDefaultAsync<VehicleDbRow>(
            new CommandDefinition(
                sql,
                new { VehicleId = vehicleId.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task<int> CountByVinAsync(string vin, CancellationToken cancellationToken = default)
    {
        const string sql = """
        SELECT COUNT(*)
        FROM vehicles
        WHERE vin = @Vin;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { Vin = vin },
            cancellationToken: cancellationToken));
    }
}