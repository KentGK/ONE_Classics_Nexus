using Dapper;
using MySqlConnector;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.Database;
using VehicleEntity = ONE_Classics_Nexus.Domain.Entities.Vehicle;

namespace ONE_Classics_Nexus.Infrastructure.Repositories;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly MariaDbConnectionFactory _factory;

    public VehicleRepository(MariaDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<VehicleEntity> CreateAsync(VehicleEntity vehicle, CancellationToken ct)
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

    public async Task<VehicleEntity?> GetByIdAsync(Guid vehicleId, CancellationToken ct)
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

        return await conn.QuerySingleOrDefaultAsync<VehicleEntity>(new CommandDefinition(
            sql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: ct));
    }

    public async Task<VehicleEntity?> UpdateAsync(VehicleEntity vehicle, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        const string sql = """
            UPDATE vehicles SET vin = @Vin, year = @Year, make = @Make,
                model = @Model, trim = @Trim, body_style = @BodyStyle,
                engine = @Engine, transmission = @Transmission,
                drivetrain = @Drivetrain, title_status = @TitleStatus
            WHERE vehicle_id = @VehicleId;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        var affected = await conn.ExecuteAsync(new CommandDefinition(sql, new
        {
            VehicleId = vehicle.VehicleId.ToString(), vehicle.Vin, vehicle.Year,
            vehicle.Make, vehicle.Model, vehicle.Trim, vehicle.BodyStyle,
            vehicle.Engine, vehicle.Transmission, vehicle.Drivetrain,
            vehicle.TitleStatus
        }, cancellationToken: ct));

        return affected == 0 ? null : await GetByIdAsync(vehicle.VehicleId, ct);
    }

    public async Task<bool> ArchiveAsync(Guid vehicleId, CancellationToken ct)
    {
        const string sql = """
            UPDATE vehicles SET vehicle_status = @Status
            WHERE vehicle_id = @VehicleId;
            """;
        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        return await conn.ExecuteAsync(new CommandDefinition(sql, new
        {
            VehicleId = vehicleId.ToString(), Status = ONE_Classics_Nexus.Domain.Constants.VehicleStatuses.Archived
        }, cancellationToken: ct)) > 0;
    }

    public async Task<(IReadOnlyList<VehicleEntity> Items, int TotalCount)> ListAsync(
        int page, int pageSize, string make, string model, int? year,
        string titleStatus, CancellationToken ct)
    {
        const string filter = """
            FROM vehicles WHERE (@Make = '' OR make LIKE CONCAT('%', @Make, '%'))
            AND (@Model = '' OR model LIKE CONCAT('%', @Model, '%'))
            AND (@Year IS NULL OR year = @Year)
            AND (@TitleStatus = '' OR title_status = @TitleStatus)
            """;
        var args = new
        {
            Make = make, Model = model, Year = year, TitleStatus = titleStatus,
            Limit = pageSize, Offset = (page - 1) * pageSize
        };
        await using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        var total = await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            "SELECT COUNT(*) " + filter, args, cancellationToken: ct));
        var items = (await conn.QueryAsync<VehicleEntity>(new CommandDefinition(
            "SELECT * " + filter + " ORDER BY created_at_utc DESC LIMIT @Limit OFFSET @Offset",
            args, cancellationToken: ct))).ToList();
        return (items, total);
    }
}