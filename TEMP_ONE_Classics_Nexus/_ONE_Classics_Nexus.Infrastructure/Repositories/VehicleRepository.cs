using System.Data;
using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using VehicleEntity = ONE_Classics_Nexus.Domain.Entities.Vehicle;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Vehicle
{
    public sealed class VehicleRepository : IVehicleRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public VehicleRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<VehicleEntity?> InsertAsync(
            VehicleEntity vehicle,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            const string sql = """
                INSERT INTO vehicles
                (
                    vehicle_id,
                    vehicle_type_id,
                    vin,
                    year,
                    make,
                    model,
                    trim,
                    series_name,
                    body_style,
                    drivetrain,
                    engine,
                    transmission,
                    mileage,
                    odometer_unit,
                    exterior_color,
                    interior_color,
                    title_status,
                    authenticity_profile,
                    restoration_level,
                    documentation_level,
                    overall_condition_score,
                    has_red_flags,
                    notes,
                    ownership_notes,
                    created_at_utc,
                    updated_at_utc
                )
                VALUES
                (
                    @VehicleId,
                    @VehicleTypeId,
                    @Vin,
                    @Year,
                    @Make,
                    @Model,
                    @Trim,
                    @SeriesName,
                    @BodyStyle,
                    @Drivetrain,
                    @Engine,
                    @Transmission,
                    @Mileage,
                    @OdometerUnit,
                    @ExteriorColor,
                    @InteriorColor,
                    @TitleStatus,
                    @AuthenticityProfile,
                    @RestorationLevel,
                    @DocumentationLevel,
                    @OverallConditionScore,
                    @HasRedFlags,
                    @Notes,
                    @OwnershipSummary,
                    @CreatedAtUtc,
                    @UpdatedAtUtc
                );
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var parameters = new
            {
                VehicleId = vehicle.VehicleId,
                vehicle.VehicleTypeId,
                vehicle.Vin,
                vehicle.Year,
                vehicle.Make,
                vehicle.Model,
                vehicle.Trim,
                vehicle.SeriesName,
                vehicle.BodyStyle,
                vehicle.Drivetrain,
                vehicle.Engine,
                vehicle.Transmission,
                vehicle.Mileage,
                vehicle.OdometerUnit,
                vehicle.ExteriorColor,
                vehicle.InteriorColor,
                vehicle.TitleStatus,
                vehicle.AuthenticityProfile,
                vehicle.RestorationLevel,
                vehicle.DocumentationLevel,
                vehicle.OverallConditionScore,
                vehicle.HasRedFlags,
                vehicle.Notes,
                vehicle.OwnershipSummary,
                vehicle.CreatedAtUtc,
                vehicle.UpdatedAtUtc
            };

            await connection.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

            return vehicle;
        }

        // CreateAsync uses the column subset you provided in the SQL snippet.
        public async Task<VehicleEntity?> CreateAsync(
            VehicleEntity vehicle,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            const string sql = @"
                INSERT INTO vehicles (
                    vehicle_id,
                    vehicle_type_id,
                    vin,
                    year,
                    make,
                    model,
                    trim,
                    body_style,
                    engine,
                    transmission,
                    drivetrain,
                    mileage,
                    created_at_utc,
                    updated_at_utc
                )
                VALUES (
                    @VehicleId,
                    @VehicleTypeId,
                    @Vin,
                    @Year,
                    @Make,
                    @Model,
                    @Trim,
                    @BodyStyle,
                    @Engine,
                    @Transmission,
                    @Drivetrain,
                    @Mileage,
                    @CreatedAtUtc,
                    @UpdatedAtUtc
                );";

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var parameters = new
            {
                VehicleId = vehicle.VehicleId,
                vehicle.VehicleTypeId,
                vehicle.Vin,
                vehicle.Year,
                vehicle.Make,
                vehicle.Model,
                vehicle.Trim,
                vehicle.BodyStyle,
                vehicle.Engine,
                vehicle.Transmission,
                vehicle.Drivetrain,
                vehicle.Mileage,
                vehicle.CreatedAtUtc,
                vehicle.UpdatedAtUtc
            };

            await connection.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

            return vehicle;
        }

        public async Task<VehicleEntity?> GetByIdAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            const string sql = """
                SELECT
                    vehicle_id                AS VehicleId,
                    vehicle_type_id           AS VehicleTypeId,
                    vin                       AS Vin,
                    year                      AS Year,
                    make                      AS Make,
                    model                     AS Model,
                    trim                      AS Trim,
                    series_name               AS SeriesName,
                    body_style                AS BodyStyle,
                    drivetrain                AS Drivetrain,
                    engine                    AS Engine,
                    transmission              AS Transmission,
                    mileage                   AS Mileage,
                    odometer_unit             AS OdometerUnit,
                    exterior_color            AS ExteriorColor,
                    interior_color            AS InteriorColor,
                    title_status              AS TitleStatus,
                    authenticity_profile      AS AuthenticityProfile,
                    restoration_level         AS RestorationLevel,
                    documentation_level       AS DocumentationLevel,
                    overall_condition_score   AS OverallConditionScore,
                    has_red_flags             AS HasRedFlags,
                    notes                     AS Notes,
                    ownership_notes           AS OwnershipSummary,
                    created_at_utc            AS CreatedAtUtc,
                    updated_at_utc            AS UpdatedAtUtc
                FROM vehicles
                WHERE vehicle_id = @VehicleId
                LIMIT 1;
                """
;
            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var row = await connection.QuerySingleOrDefaultAsync<VehicleRow>(
                new CommandDefinition(sql, new { VehicleId = vehicleId.ToString() }, cancellationToken: cancellationToken));

            return row is null ? null : Map(row);
        }

        public async Task<VehicleEntity?> UpdateAsync(
            VehicleEntity vehicle,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            const string sql = """
                UPDATE vehicles
                SET
                    vehicle_type_id = @VehicleTypeId,
                    vin = @Vin,
                    year = @Year,
                    make = @Make,
                    model = @Model,
                    trim = @Trim,
                    series_name = @SeriesName,
                    body_style = @BodyStyle,
                    drivetrain = @Drivetrain,
                    engine = @Engine,
                    transmission = @Transmission,
                    mileage = @Mileage,
                    odometer_unit = @OdometerUnit,
                    exterior_color = @ExteriorColor,
                    interior_color = @InteriorColor,
                    title_status = @TitleStatus,
                    authenticity_profile = @AuthenticityProfile,
                    restoration_level = @RestorationLevel,
                    documentation_level = @DocumentationLevel,
                    overall_condition_score = @OverallConditionScore,
                    has_red_flags = @HasRedFlags,
                    notes = @Notes,
                    ownership_notes = @OwnershipSummary,
                    updated_at_utc = @UpdatedAtUtc
                WHERE vehicle_id = @VehicleId;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var parameters = new
            {
                VehicleId = vehicle.VehicleId,
                vehicle.VehicleTypeId,
                vehicle.Vin,
                vehicle.Year,
                vehicle.Make,
                vehicle.Model,
                vehicle.Trim,
                vehicle.SeriesName,
                vehicle.BodyStyle,
                vehicle.Drivetrain,
                vehicle.Engine,
                vehicle.Transmission,
                vehicle.Mileage,
                vehicle.OdometerUnit,
                vehicle.ExteriorColor,
                vehicle.InteriorColor,
                vehicle.TitleStatus,
                vehicle.AuthenticityProfile,
                vehicle.RestorationLevel,
                vehicle.DocumentationLevel,
                vehicle.OverallConditionScore,
                vehicle.HasRedFlags,
                vehicle.Notes,
                vehicle.OwnershipSummary,
                vehicle.UpdatedAtUtc
            };

            var affected = await connection.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

            if (affected == 0)
                return null;

            return await GetByIdAsync(vehicle.VehicleId, cancellationToken);
        }

        private static VehicleEntity Map(VehicleRow row)
        {
            return new VehicleEntity
            {
                VehicleId = row.VehicleId,
                VehicleTypeId = row.VehicleTypeId,
                Vin = row.Vin,
                Year = row.Year,
                Make = row.Make,
                Model = row.Model,
                Trim = row.Trim,
                SeriesName = row.SeriesName,
                BodyStyle = row.BodyStyle,
                Drivetrain = row.Drivetrain,
                Engine = row.Engine,
                Transmission = row.Transmission,
                Mileage = row.Mileage,
                OdometerUnit = row.OdometerUnit,
                ExteriorColor = row.ExteriorColor,
                InteriorColor = row.InteriorColor,
                TitleStatus = row.TitleStatus,
                AuthenticityProfile = row.AuthenticityProfile,
                RestorationLevel = row.RestorationLevel,
                DocumentationLevel = row.DocumentationLevel,
                OverallConditionScore = row.OverallConditionScore,
                HasRedFlags = row.HasRedFlags,
                Notes = row.Notes,
                OwnershipSummary = row.OwnershipSummary,
                CreatedAtUtc = row.CreatedAtUtc,
                UpdatedAtUtc = row.UpdatedAtUtc
            };
        }

        private sealed class VehicleRow
        {
            public Guid VehicleId { get; init; }
            public Guid VehicleTypeId { get; init; }
            public string? Vin { get; init; }
            public int Year { get; init; }
            public string Make { get; init; } = string.Empty;
            public string Model { get; init; } = string.Empty;
            public string? Trim { get; init; }
            public string? SeriesName { get; init; }
            public string? BodyStyle { get; init; }
            public string? Drivetrain { get; init; }
            public string? Engine { get; init; }
            public string? Transmission { get; init; }
            public int? Mileage { get; init; }
            public string OdometerUnit { get; init; } = "miles";
            public string? ExteriorColor { get; init; }
            public string? InteriorColor { get; init; }
            public string? TitleStatus { get; init; }
            public string? AuthenticityProfile { get; init; }
            public string? RestorationLevel { get; init; }
            public string? DocumentationLevel { get; init; }
            public decimal? OverallConditionScore { get; init; }
            public bool HasRedFlags { get; init; }
            public string? Notes { get; init; }
            public string? OwnershipSummary { get; init; }
            public DateTime CreatedAtUtc { get; init; }
            public DateTime UpdatedAtUtc { get; init; }
        }
    }
}