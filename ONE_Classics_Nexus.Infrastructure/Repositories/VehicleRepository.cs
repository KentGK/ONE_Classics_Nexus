using Dapper;
using MySqlConnector;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly MariaDbConnectionFactory _factory;

    public VehicleRepository(MariaDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct)
    {
        const string sql = """
            INSERT INTO vehicles
            (
                vehicle_id,
                vin,
                year,
                make,
                model,
                trim,
                body_style,
                engine,
                transmission,
                drivetrain,
                title_status,
                created_at_utc
            )
            VALUES
            (
                @VehicleId,
                @Vin,
                @Year,
                @Make,
                @Model,
                @Trim,
                @BodyStyle,
                @Engine,
                @Transmission,
                @Drivetrain,
                @TitleStatus,
                @CreatedAtUtc
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        try
        {
            await conn.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    VehicleId = vehicle.VehicleId.ToString(),
                    vehicle.Vin,
                    vehicle.Year,
                    vehicle.Make,
                    vehicle.Model,
                    vehicle.Trim,
                    vehicle.BodyStyle,
                    vehicle.Engine,
                    vehicle.Transmission,
                    vehicle.Drivetrain,
                    vehicle.TitleStatus,
                    vehicle.CreatedAtUtc
                },
                cancellationToken: ct));

            return vehicle;
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new InvalidOperationException($"A vehicle with VIN '{vehicle.Vin}' already exists.");
        }
    }

    public async Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken ct)
    {
        const string sql = """
            SELECT
                vehicle_id      AS VehicleId,
                vin             AS Vin,
                year            AS Year,
                make            AS Make,
                model           AS Model,
                trim            AS Trim,
                body_style      AS BodyStyle,
                engine          AS Engine,
                transmission    AS Transmission,
                drivetrain      AS Drivetrain,
                title_status    AS TitleStatus,
                created_at_utc  AS CreatedAtUtc
            FROM vehicles
            WHERE vehicle_id = @VehicleId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        return await conn.QuerySingleOrDefaultAsync<Vehicle>(new CommandDefinition(
            sql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: ct));
    }
}