using Dapper;
using MySqlConnector;

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

    public async Task<Guid?> GetAnyVehicleTypeIdAsync(CancellationToken cancellationToken = default)
    {
        // CAST keeps the value a string regardless of how the connector maps
        // char(36) columns, which it may surface as Guid.
        const string sql = """
        SELECT CAST(vehicle_type_id AS CHAR)
        FROM vehicle_types
        ORDER BY vehicle_type_id
        LIMIT 1;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        var raw = await conn.QuerySingleOrDefaultAsync<string>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return Guid.TryParse(raw, out var id)
            ? id
            : null;
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
            mileage      AS Mileage,
            vehicle_status AS Vehicle_Status
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

    public async Task DeleteAppraisalAsync(Guid appraisalId, CancellationToken cancellationToken = default)
    {
        await DeleteSnapshotByAppraisalAsync(appraisalId, cancellationToken);

        const string sql = """
        DELETE FROM appraisals
        WHERE appraisal_id = @AppraisalId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteAppraisalsByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        const string deleteSnapshotsSql = """
        DELETE ass
        FROM appraisal_subject_snapshots ass
        INNER JOIN appraisals a ON a.appraisal_id = ass.appraisal_id
        WHERE a.vehicle_id = @VehicleId;
        """;

        const string sql = """
        DELETE FROM appraisals
        WHERE vehicle_id = @VehicleId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            deleteSnapshotsSql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: cancellationToken));
        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: cancellationToken));
    }

    public async Task<AppraisalSubjectSnapshotDbRow?> GetSnapshotByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken = default)
    {
        const string sql = """
        SELECT
            appraisal_subject_snapshot_id AS Appraisal_Subject_Snapshot_Id,
            appraisal_id AS Appraisal_Id,
            vehicle_id AS Vehicle_Id,
            vin AS Vin,
            year AS Year,
            make AS Make,
            model AS Model,
            snapshot_status AS Snapshot_Status,
            row_version AS Row_Version
        FROM appraisal_subject_snapshots
        WHERE appraisal_id = @AppraisalId
        LIMIT 1;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        return await conn.QuerySingleOrDefaultAsync<AppraisalSubjectSnapshotDbRow>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task SetSnapshotStatusAsync(Guid appraisalId, string snapshotStatus, CancellationToken cancellationToken = default)
    {
        const string sql = """
        UPDATE appraisal_subject_snapshots
        SET
            snapshot_status = @SnapshotStatus,
            updated_at_utc = UTC_TIMESTAMP(6),
            row_version = row_version + 1
        WHERE appraisal_id = @AppraisalId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new
            {
                AppraisalId = appraisalId.ToString(),
                SnapshotStatus = snapshotStatus
            },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteSnapshotByAppraisalAsync(Guid appraisalId, CancellationToken cancellationToken = default)
    {
        const string sql = """
        DELETE FROM appraisal_subject_snapshots
        WHERE appraisal_id = @AppraisalId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: cancellationToken));
    }

    public async Task<int> CountAppraisalsByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        const string sql = """
        SELECT COUNT(*)
        FROM appraisals
        WHERE vehicle_id = @VehicleId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        return await conn.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new { VehicleId = vehicleId.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task SetAppraisalStatusAsync(Guid appraisalId, string status, CancellationToken cancellationToken = default)
    {
        const string sql = """
        UPDATE appraisals
        SET
            status = @Status,
            updated_at_utc = UTC_TIMESTAMP(6),
            row_version = row_version + 1
        WHERE appraisal_id = @AppraisalId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            new
            {
                AppraisalId = appraisalId.ToString(),
                Status = status
            },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteInspectionAsync(Guid inspectionId, CancellationToken cancellationToken = default)
    {
        const string deleteScoresSql = """
        DELETE FROM condition_scores
        WHERE inspection_id = @InspectionId;
        """;

        const string deleteInspectionSql = """
        DELETE FROM inspections
        WHERE inspection_id = @InspectionId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            deleteScoresSql,
            new { InspectionId = inspectionId.ToString() },
            cancellationToken: cancellationToken));
        await conn.ExecuteAsync(new CommandDefinition(
            deleteInspectionSql,
            new { InspectionId = inspectionId.ToString() },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteInspectionsByAppraisalAsync(Guid appraisalId, CancellationToken cancellationToken = default)
    {
        const string deleteScoresSql = """
        DELETE cs
        FROM condition_scores cs
        INNER JOIN inspections i ON i.inspection_id = cs.inspection_id
        WHERE i.appraisal_id = @AppraisalId;
        """;

        const string deleteInspectionsSql = """
        DELETE FROM inspections
        WHERE appraisal_id = @AppraisalId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            deleteScoresSql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: cancellationToken));
        await conn.ExecuteAsync(new CommandDefinition(
            deleteInspectionsSql,
            new { AppraisalId = appraisalId.ToString() },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteInspectionsByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        const string deleteScoresSql = """
        DELETE cs
        FROM condition_scores cs
        INNER JOIN inspections i ON i.inspection_id = cs.inspection_id
        INNER JOIN appraisals a ON a.appraisal_id = i.appraisal_id
        WHERE a.vehicle_id = @VehicleId;
        """;

        const string deleteInspectionsSql = """
        DELETE i
        FROM inspections i
        INNER JOIN appraisals a ON a.appraisal_id = i.appraisal_id
        WHERE a.vehicle_id = @VehicleId;
        """;

        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(
            deleteScoresSql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: cancellationToken));
        await conn.ExecuteAsync(new CommandDefinition(
            deleteInspectionsSql,
            new { VehicleId = vehicleId.ToString() },
            cancellationToken: cancellationToken));
    }
}