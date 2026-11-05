using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Appraisal;

public sealed class AppraisalSubjectSnapshotRepository : IAppraisalSubjectSnapshotRepository
{
    private readonly MySqlConnectionFactory _factory;

    public AppraisalSubjectSnapshotRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<AppraisalSubjectSnapshot?> GetByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                appraisal_subject_snapshot_id AS AppraisalSubjectSnapshotId,
                appraisal_id AS AppraisalId,
                vehicle_id AS VehicleId,
                vin AS Vin,
                chassis_number AS ChassisNumber,
                body_number AS BodyNumber,
                engine_number AS EngineNumber,
                year AS Year,
                make AS Make,
                marque AS Marque,
                model AS Model,
                series_name AS SeriesName,
                trim AS Trim,
                body_style AS BodyStyle,
                engine AS Engine,
                transmission AS Transmission,
                drivetrain AS Drivetrain,
                mileage AS Mileage,
                odometer_unit AS OdometerUnit,
                exterior_color AS ExteriorColor,
                interior_color AS InteriorColor,
                title_status AS TitleStatus,
                authenticity_profile AS AuthenticityProfile,
                originality_profile AS OriginalityProfile,
                restoration_level AS RestorationLevel,
                documentation_level AS DocumentationLevel,
                ownership_notes AS OwnershipNotes,
                snapshot_status AS SnapshotStatus,
                approved_by_user_id AS ApprovedByUserId,
                approved_at_utc AS ApprovedAtUtc,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM appraisal_subject_snapshots
            WHERE appraisal_id = @AppraisalId
            LIMIT 1;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var row = await conn.QuerySingleOrDefaultAsync<SnapshotRow>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: cancellationToken));

        return row is null ? null : Map(row);
    }

    public async Task<AppraisalSubjectSnapshot> InsertAsync(
        AppraisalSubjectSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        const string sql = """
            INSERT INTO appraisal_subject_snapshots
            (
                appraisal_subject_snapshot_id,
                appraisal_id,
                vehicle_id,
                vin,
                chassis_number,
                body_number,
                engine_number,
                year,
                make,
                marque,
                model,
                series_name,
                trim,
                body_style,
                engine,
                transmission,
                drivetrain,
                mileage,
                odometer_unit,
                exterior_color,
                interior_color,
                title_status,
                authenticity_profile,
                originality_profile,
                restoration_level,
                documentation_level,
                ownership_notes,
                snapshot_status,
                approved_by_user_id,
                approved_at_utc,
                row_version,
                created_at_utc,
                updated_at_utc
            )
            VALUES
            (
                @AppraisalSubjectSnapshotId,
                @AppraisalId,
                @VehicleId,
                @Vin,
                @ChassisNumber,
                @BodyNumber,
                @EngineNumber,
                @Year,
                @Make,
                @Marque,
                @Model,
                @SeriesName,
                @Trim,
                @BodyStyle,
                @Engine,
                @Transmission,
                @Drivetrain,
                @Mileage,
                @OdometerUnit,
                @ExteriorColor,
                @InteriorColor,
                @TitleStatus,
                @AuthenticityProfile,
                @OriginalityProfile,
                @RestorationLevel,
                @DocumentationLevel,
                @OwnershipNotes,
                @SnapshotStatus,
                @ApprovedByUserId,
                @ApprovedAtUtc,
                @RowVersion,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        await conn.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(snapshot),
            cancellationToken: cancellationToken));

        return snapshot;
    }

    public async Task<AppraisalSubjectSnapshot?> UpdateAsync(
        AppraisalSubjectSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        const string sql = """
            UPDATE appraisal_subject_snapshots
            SET
                vin = @Vin,
                chassis_number = @ChassisNumber,
                body_number = @BodyNumber,
                engine_number = @EngineNumber,
                year = @Year,
                make = @Make,
                marque = @Marque,
                model = @Model,
                series_name = @SeriesName,
                trim = @Trim,
                body_style = @BodyStyle,
                engine = @Engine,
                transmission = @Transmission,
                drivetrain = @Drivetrain,
                mileage = @Mileage,
                odometer_unit = @OdometerUnit,
                exterior_color = @ExteriorColor,
                interior_color = @InteriorColor,
                title_status = @TitleStatus,
                authenticity_profile = @AuthenticityProfile,
                originality_profile = @OriginalityProfile,
                restoration_level = @RestorationLevel,
                documentation_level = @DocumentationLevel,
                ownership_notes = @OwnershipNotes,
                snapshot_status = @SnapshotStatus,
                approved_by_user_id = @ApprovedByUserId,
                approved_at_utc = @ApprovedAtUtc,
                row_version = @RowVersion,
                updated_at_utc = @UpdatedAtUtc
            WHERE appraisal_subject_snapshot_id = @AppraisalSubjectSnapshotId
              AND row_version = @ExpectedRowVersion;
            """;

        await using var conn = _factory.Create();
        await conn.OpenAsync(cancellationToken);

        var expectedRowVersion = snapshot.RowVersion - 1;

        var affected = await conn.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(snapshot, expectedRowVersion),
            cancellationToken: cancellationToken));

        if (affected == 0)
        {
            return null;
        }

        return snapshot;
    }

    public async Task<bool> DeleteByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM appraisal_subject_snapshots
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

    private static AppraisalSubjectSnapshot Map(SnapshotRow row)
    {
        return AppraisalSubjectSnapshot.Rehydrate(
            ParseGuid(row.AppraisalSubjectSnapshotId, nameof(row.AppraisalSubjectSnapshotId)),
            ParseGuid(row.AppraisalId, nameof(row.AppraisalId)),
            ParseGuid(row.VehicleId, nameof(row.VehicleId)),
            row.Vin,
            row.ChassisNumber,
            row.BodyNumber,
            row.EngineNumber,
            row.Year,
            row.Make,
            row.Marque,
            row.Model,
            row.SeriesName,
            row.Trim,
            row.BodyStyle,
            row.Engine,
            row.Transmission,
            row.Drivetrain,
            row.Mileage,
            row.OdometerUnit,
            row.ExteriorColor,
            row.InteriorColor,
            row.TitleStatus,
            row.AuthenticityProfile,
            row.OriginalityProfile,
            row.RestorationLevel,
            row.DocumentationLevel,
            row.OwnershipNotes,
            row.SnapshotStatus,
            string.IsNullOrWhiteSpace(row.ApprovedByUserId) ? null : ParseGuid(row.ApprovedByUserId, nameof(row.ApprovedByUserId)),
            row.ApprovedAtUtc,
            row.RowVersion,
            row.CreatedAtUtc,
            row.UpdatedAtUtc);
    }

    private static Guid ParseGuid(string value, string fieldName)
    {
        if (!Guid.TryParse(value, out var parsed))
        {
            throw new InvalidOperationException($"Invalid GUID value in {fieldName}.");
        }

        return parsed;
    }

    private static object ToParameters(AppraisalSubjectSnapshot snapshot, long? expectedRowVersion = null)
    {
        return new
        {
            AppraisalSubjectSnapshotId = snapshot.AppraisalSubjectSnapshotId.ToString(),
            AppraisalId = snapshot.AppraisalId.ToString(),
            VehicleId = snapshot.VehicleId.ToString(),
            snapshot.Vin,
            snapshot.ChassisNumber,
            snapshot.BodyNumber,
            snapshot.EngineNumber,
            snapshot.Year,
            snapshot.Make,
            snapshot.Marque,
            snapshot.Model,
            snapshot.SeriesName,
            snapshot.Trim,
            snapshot.BodyStyle,
            snapshot.Engine,
            snapshot.Transmission,
            snapshot.Drivetrain,
            snapshot.Mileage,
            snapshot.OdometerUnit,
            snapshot.ExteriorColor,
            snapshot.InteriorColor,
            snapshot.TitleStatus,
            snapshot.AuthenticityProfile,
            snapshot.OriginalityProfile,
            snapshot.RestorationLevel,
            snapshot.DocumentationLevel,
            snapshot.OwnershipNotes,
            snapshot.SnapshotStatus,
            ApprovedByUserId = snapshot.ApprovedByUserId?.ToString(),
            snapshot.ApprovedAtUtc,
            snapshot.RowVersion,
            snapshot.CreatedAtUtc,
            snapshot.UpdatedAtUtc,
            ExpectedRowVersion = expectedRowVersion
        };
    }

    private sealed class SnapshotRow
    {
        public string AppraisalSubjectSnapshotId { get; init; } = string.Empty;
        public string AppraisalId { get; init; } = string.Empty;
        public string VehicleId { get; init; } = string.Empty;
        public string? Vin { get; init; }
        public string? ChassisNumber { get; init; }
        public string? BodyNumber { get; init; }
        public string? EngineNumber { get; init; }
        public int Year { get; init; }
        public string Make { get; init; } = string.Empty;
        public string? Marque { get; init; }
        public string Model { get; init; } = string.Empty;
        public string? SeriesName { get; init; }
        public string? Trim { get; init; }
        public string? BodyStyle { get; init; }
        public string? Engine { get; init; }
        public string? Transmission { get; init; }
        public string? Drivetrain { get; init; }
        public int? Mileage { get; init; }
        public string? OdometerUnit { get; init; }
        public string? ExteriorColor { get; init; }
        public string? InteriorColor { get; init; }
        public string? TitleStatus { get; init; }
        public string? AuthenticityProfile { get; init; }
        public string? OriginalityProfile { get; init; }
        public string? RestorationLevel { get; init; }
        public string? DocumentationLevel { get; init; }
        public string? OwnershipNotes { get; init; }
        public string SnapshotStatus { get; init; } = string.Empty;
        public string? ApprovedByUserId { get; init; }
        public DateTime? ApprovedAtUtc { get; init; }
        public long RowVersion { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public DateTime UpdatedAtUtc { get; init; }
    }

    private sealed class FakeSnapshotRepository : IAppraisalSubjectSnapshotRepository
    {
        private readonly AppraisalSubjectSnapshot? _snapshot;

        public FakeSnapshotRepository(AppraisalSubjectSnapshot? snapshot)
        {
            _snapshot = snapshot;
        }

        public Task<AppraisalSubjectSnapshot?> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken)
            => Task.FromResult(_snapshot);

        public Task<AppraisalSubjectSnapshot> InsertAsync(AppraisalSubjectSnapshot snapshot, CancellationToken cancellationToken)
            => Task.FromResult(snapshot);

        public Task<AppraisalSubjectSnapshot?> UpdateAsync(AppraisalSubjectSnapshot snapshot, CancellationToken cancellationToken)
            => Task.FromResult<AppraisalSubjectSnapshot?>(snapshot);

        public Task<bool> DeleteByAppraisalIdAsync(Guid appraisalId, CancellationToken cancellationToken)
            => Task.FromResult(_snapshot is not null);
    }
}
