using Dapper;
using MySqlConnector;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Infrastructure.Database;
using ONE_Classics_Nexus.Application.Results;
using ONE_Classics_Nexus.Domain.Constants;
using System.Data;
using VehicleEntity = ONE_Classics_Nexus.Domain.Entities.Vehicle;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Vehicle
{
    public sealed class VehicleRepository : IVehicleRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public VehicleRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        // Change the InsertAsync method signature to explicitly implement the interface
        public async Task<VehicleEntity?> InsertAsync(
            VehicleEntity vehicle,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            // Normalize VIN to avoid trivial duplicates
            var normalizedVin = string.IsNullOrWhiteSpace(vehicle.Vin)
                ? vehicle.Vin
                : vehicle.Vin.Trim().ToUpperInvariant();

            const string sql = """
                INSERT INTO vehicles
                (
                    vehicle_id,
                    vehicle_type_id,
                    vin,
                    chassis_number,
                    body_number,
                    engine_number,
                    year,
                    make,
                    marque,
                    model,
                    trim,
                    series_name,
                    body_style,
                    drivetrain,
                    engine,
                    transmission,
                    mileage,
                    odometer_unit,
                    odometer_status,
                    exterior_color,
                    interior_color,
                    title_status,
                    authenticity_profile,
                    originality_profile,
                    restoration_level,
                    documentation_level,
                    overall_condition_score,
                    has_red_flags,
                    vehicle_status,
                    row_version,
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
                    @ChassisNumber,
                    @BodyNumber,
                    @EngineNumber,
                    @Year,
                    @Make,
                    @Marque,
                    @Model,
                    @Trim,
                    @SeriesName,
                    @BodyStyle,
                    @Drivetrain,
                    @Engine,
                    @Transmission,
                    @Mileage,
                    @OdometerUnit,
                    @OdometerStatus,
                    @ExteriorColor,
                    @InteriorColor,
                    @TitleStatus,
                    @AuthenticityProfile,
                    @OriginalityProfile,
                    @RestorationLevel,
                    @DocumentationLevel,
                    @OverallConditionScore,
                    @HasRedFlags,
                    @VehicleStatus,
                    @RowVersion,
                    @Notes,
                    @OwnershipNotes,
                    @CreatedAtUtc,
                    @UpdatedAtUtc
                );
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var parameters = new
            {
                vehicle.VehicleId,
                vehicle.VehicleTypeId,
                Vin = normalizedVin,
                vehicle.ChassisNumber,
                vehicle.BodyNumber,
                vehicle.EngineNumber,
                vehicle.Year,
                vehicle.Make,
                vehicle.Marque,
                vehicle.Model,
                vehicle.Trim,
                vehicle.SeriesName,
                vehicle.BodyStyle,
                vehicle.Drivetrain,
                vehicle.Engine,
                vehicle.Transmission,
                vehicle.Mileage,
                vehicle.OdometerUnit,
                vehicle.OdometerStatus,
                vehicle.ExteriorColor,
                vehicle.InteriorColor,
                vehicle.TitleStatus,
                vehicle.AuthenticityProfile,
                vehicle.OriginalityProfile,
                vehicle.RestorationLevel,
                vehicle.DocumentationLevel,
                vehicle.OverallConditionScore,
                vehicle.HasRedFlags,
                vehicle.VehicleStatus,
                vehicle.RowVersion,
                vehicle.Notes,
                vehicle.OwnershipNotes,
                vehicle.CreatedAtUtc,
                vehicle.UpdatedAtUtc
            };

            try
            {
                await connection.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
                return vehicle;
            }
            catch (MySqlException mex) when (mex.Number == 1062)
            {
                // Unique constraint violation on VIN (Duplicate entry).
                // Throw domain-level ConflictException so global middleware maps to 409.
                throw new ConflictException($"A vehicle with VIN '{vehicle.Vin}' already exists.");
            }
        }

        // CreateAsync kept for compatibility with the abstraction.
        // Delegates to InsertAsync but preserves the smaller-column insert previously used.
        public async Task<VehicleEntity> CreateAsync(
            VehicleEntity vehicle,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            await InsertAsync(vehicle, cancellationToken);
            var result = await GetByIdAsync(vehicle.VehicleId, cancellationToken);
            if (result is null)
            {
                throw new InvalidOperationException($"Vehicle with ID '{vehicle.VehicleId}' was not found after insert.");
            }
            return result;
        }

        public async Task<VehicleEntity?> GetByIdAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            const string sql = """
                SELECT
                    vehicle_id              AS VehicleId,
                    vehicle_type_id         AS VehicleTypeId,
                    vin                     AS Vin,
                    chassis_number          AS ChassisNumber,
                    body_number             AS BodyNumber,
                    engine_number           AS EngineNumber,
                    year                    AS Year,
                    make                    AS Make,
                    marque                  AS Marque,
                    model                   AS Model,
                    trim                    AS Trim,
                    series_name             AS SeriesName,
                    body_style              AS BodyStyle,
                    drivetrain              AS Drivetrain,
                    engine                  AS Engine,
                    transmission            AS Transmission,
                    mileage                 AS Mileage,
                    odometer_unit           AS OdometerUnit,
                    odometer_status         AS OdometerStatus,
                    exterior_color          AS ExteriorColor,
                    interior_color          AS InteriorColor,
                    title_status            AS TitleStatus,
                    authenticity_profile    AS AuthenticityProfile,
                    originality_profile     AS OriginalityProfile,
                    restoration_level       AS RestorationLevel,
                    documentation_level     AS DocumentationLevel,
                    overall_condition_score AS OverallConditionScore,
                    has_red_flags           AS HasRedFlags,
                    notes                   AS Notes,
                    ownership_notes         AS OwnershipNotes,
                    vehicle_status          AS VehicleStatus,
                    row_version             AS RowVersion,
                    created_at_utc          AS CreatedAtUtc,
                    updated_at_utc          AS UpdatedAtUtc
                FROM vehicles
                WHERE vehicle_id = @VehicleId
                LIMIT 1;
                """;

            using var connection =
                await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var row = await connection.QuerySingleOrDefaultAsync<VehicleRow>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        VehicleId = vehicleId.ToString()
                    },
                    cancellationToken: cancellationToken));

            return row is null ? null : Map(row);
        }

        // New existence check by VIN to support pre-checks in service layer
        public async Task<bool> ExistsByVinAsync(string vin, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return false;

            var normalizedVin = vin.Trim().ToUpperInvariant();

            const string sql = """
                SELECT COUNT(1)
                FROM vehicles
                WHERE vin = @Vin
                LIMIT 1;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
            var count = await connection.ExecuteScalarAsync<long>(new CommandDefinition(sql, new { Vin = normalizedVin }, cancellationToken: cancellationToken));
            return count > 0;
        }

        public async Task<(IReadOnlyList<VehicleEntity> Items, int TotalCount)> ListAsync(
            int page,
            int pageSize,
            string? search = null,
            string? vin = null,
            int? yearFrom = null,
            int? yearTo = null,
            string? make = null,
            string? model = null,
            string? series = null,
            Guid? vehicleTypeId = null,
            string? bodyStyle = null,
            int? year = null,
            int? mileage = null,
            string? exteriorColor = null,
            bool? hasRedFlags = null,
            string? titleStatus = null,
            string? sortBy = null,
            string? sortDirection = null,
            CancellationToken cancellationToken = default)
        {
            var safePage = page < 1 ? 1 : page;
            var safePageSize = pageSize is < 1 or > 200 ? 20 : pageSize;

            var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();
            var normalizedVin = string.IsNullOrWhiteSpace(vin) ? null : vin.Trim().ToUpperInvariant();
            var normalizedMake = string.IsNullOrWhiteSpace(make) ? null : make.Trim();
            var normalizedModel = string.IsNullOrWhiteSpace(model) ? null : model.Trim();
            var normalizedSeries = string.IsNullOrWhiteSpace(series) ? null : series.Trim();
            var normalizedBodyStyle = string.IsNullOrWhiteSpace(bodyStyle) ? null : bodyStyle.Trim();
            var normalizedExteriorColor = string.IsNullOrWhiteSpace(exteriorColor) ? null : exteriorColor.Trim();
            var normalizedTitleStatus = string.IsNullOrWhiteSpace(titleStatus) ? null : titleStatus.Trim();

            var direction = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                ? "ASC"
                : "DESC";

            var sortColumn = sortBy?.Trim().ToLowerInvariant() switch
            {
                "vin" => "vin",
                "year" => "year",
                "make" => "make",
                "model" => "model",
                "series" => "series_name",
                "mileage" => "mileage",
                "createdatutc" => "created_at_utc",
                "updatedatutc" => "updated_at_utc",
                _ => "updated_at_utc"
            };

            const string whereClause = """
                WHERE (@Search IS NULL OR (
                        vin LIKE CONCAT('%', @Search, '%')
                     OR chassis_number LIKE CONCAT('%', @Search, '%')
                     OR CAST(year AS CHAR) LIKE CONCAT('%', @Search, '%')
                     OR make LIKE CONCAT('%', @Search, '%')
                     OR marque LIKE CONCAT('%', @Search, '%')
                     OR model LIKE CONCAT('%', @Search, '%')
                     OR series_name LIKE CONCAT('%', @Search, '%')
                     OR trim LIKE CONCAT('%', @Search, '%')
                     OR body_style LIKE CONCAT('%', @Search, '%')
                ))
                  AND (@Vin IS NULL OR vin LIKE CONCAT('%', @Vin, '%'))
                  AND (@YearFrom IS NULL OR year >= @YearFrom)
                  AND (@YearTo IS NULL OR year <= @YearTo)
                  AND (@Make IS NULL OR make LIKE CONCAT('%', @Make, '%'))
                  AND (@Model IS NULL OR model LIKE CONCAT('%', @Model, '%'))
                  AND (@Series IS NULL OR series_name LIKE CONCAT('%', @Series, '%'))
                  AND (@VehicleTypeId IS NULL OR vehicle_type_id = @VehicleTypeId)
                  AND (@BodyStyle IS NULL OR body_style LIKE CONCAT('%', @BodyStyle, '%'))
                  AND (@Year IS NULL OR year = @Year)
                  AND (@Mileage IS NULL OR mileage = @Mileage)
                  AND (@ExteriorColor IS NULL OR exterior_color LIKE CONCAT('%', @ExteriorColor, '%'))
                  AND (@HasRedFlags IS NULL OR has_red_flags = @HasRedFlags)
                  AND (@TitleStatus IS NULL OR title_status LIKE CONCAT('%', @TitleStatus, '%'))
                """;

            var countSql = $"""
                SELECT COUNT(*)
                FROM vehicles
                {whereClause};
                """;

            var listSql = $"""
                SELECT
                    vehicle_id              AS VehicleId,
                    vehicle_type_id         AS VehicleTypeId,
                    vin                     AS Vin,
                    chassis_number          AS ChassisNumber,
                    body_number             AS BodyNumber,
                    engine_number           AS EngineNumber,
                    year                    AS Year,
                    make                    AS Make,
                    marque                  AS Marque,
                    model                   AS Model,
                    trim                    AS Trim,
                    series_name             AS SeriesName,
                    body_style              AS BodyStyle,
                    drivetrain              AS Drivetrain,
                    engine                  AS Engine,
                    transmission            AS Transmission,
                    mileage                 AS Mileage,
                    odometer_unit           AS OdometerUnit,
                    odometer_status         AS OdometerStatus,
                    exterior_color          AS ExteriorColor,
                    interior_color          AS InteriorColor,
                    title_status            AS TitleStatus,
                    authenticity_profile    AS AuthenticityProfile,
                    originality_profile     AS OriginalityProfile,
                    restoration_level       AS RestorationLevel,
                    documentation_level     AS DocumentationLevel,
                    overall_condition_score AS OverallConditionScore,
                    has_red_flags           AS HasRedFlags,
                    notes                   AS Notes,
                    ownership_notes         AS OwnershipNotes,
                    vehicle_status          AS VehicleStatus,
                    row_version             AS RowVersion,
                    created_at_utc          AS CreatedAtUtc,
                    updated_at_utc          AS UpdatedAtUtc
                FROM vehicles
                {whereClause}
                ORDER BY {sortColumn} {direction}
                LIMIT @PageSize OFFSET @Offset;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var parameters = new
            {
                Search = normalizedSearch,
                Vin = normalizedVin,
                YearFrom = yearFrom,
                YearTo = yearTo,
                Make = normalizedMake,
                Model = normalizedModel,
                Series = normalizedSeries,
                VehicleTypeId = vehicleTypeId,
                BodyStyle = normalizedBodyStyle,
                Year = year,
                Mileage = mileage,
                ExteriorColor = normalizedExteriorColor,
                HasRedFlags = hasRedFlags,
                TitleStatus = normalizedTitleStatus,
                PageSize = safePageSize,
                Offset = (safePage - 1) * safePageSize
            };

            var totalCount = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));

            var rows = await connection.QueryAsync<VehicleRow>(
                new CommandDefinition(listSql, parameters, cancellationToken: cancellationToken));

            var items = rows.Select(Map).ToList();

            return (items, totalCount);
        }

        public Task<(IReadOnlyList<VehicleEntity> Items, int TotalCount)> ListAsync(
            int page,
            int pageSize,
            string make,
            string model,
            int? year,
            string titleStatus,
            CancellationToken cancellationToken)
        {
            return ListAsync(
                page: page,
                pageSize: pageSize,
                search: null,
                vin: null,
                yearFrom: null,
                yearTo: null,
                make: make,
                model: model,
                series: null,
                vehicleTypeId: null,
                bodyStyle: null,
                year: year,
                mileage: null,
                exteriorColor: null,
                hasRedFlags: null,
                titleStatus: titleStatus,
                sortBy: null,
                sortDirection: null,
                cancellationToken: cancellationToken);
        }

        public Task<(IReadOnlyList<VehicleEntity> Items, int TotalCount)> SearchAsync(
            SearchVehiclesRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            return ListAsync(
                page: request.Page,
                pageSize: request.PageSize,
                search: request.Search,
                vin: request.Vin,
                yearFrom: request.YearFrom,
                yearTo: request.YearTo,
                make: request.Make,
                model: request.Model,
                series: request.Series,
                vehicleTypeId: request.VehicleTypeId,
                bodyStyle: request.BodyStyle,
                year: null,
                mileage: null,
                exteriorColor: null,
                hasRedFlags: request.HasRedFlags,
                titleStatus: request.TitleStatus,
                sortBy: request.SortBy,
                sortDirection: request.SortDirection,
                cancellationToken: cancellationToken);
        }

        public async Task<VehicleEntity?> UpdateAsync(
            VehicleEntity vehicle,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            const string sql = """
                UPDATE vehicles
                SET
                    vehicle_type_id         = @VehicleTypeId,
                    vin                     = @Vin,
                    chassis_number          = @ChassisNumber,
                    body_number             = @BodyNumber,
                    engine_number           = @EngineNumber,
                    year                    = @Year,
                    make                    = @Make,
                    marque                  = @Marque,
                    model                   = @Model,
                    trim                    = @Trim,
                    series_name             = @SeriesName,
                    body_style              = @BodyStyle,
                    drivetrain              = @Drivetrain,
                    engine                  = @Engine,
                    transmission            = @Transmission,
                    mileage                 = @Mileage,
                    odometer_unit           = @OdometerUnit,
                    odometer_status         = @OdometerStatus,
                    exterior_color          = @ExteriorColor,
                    interior_color          = @InteriorColor,
                    title_status            = @TitleStatus,
                    authenticity_profile    = @AuthenticityProfile,
                    originality_profile     = @OriginalityProfile,
                    restoration_level       = @RestorationLevel,
                    documentation_level     = @DocumentationLevel,
                    overall_condition_score = @OverallConditionScore,
                    has_red_flags           = @HasRedFlags,
                    notes                   = @Notes,
                    ownership_notes         = @OwnershipNotes,
                    vehicle_status          = @VehicleStatus,
                    updated_at_utc          = @UpdatedAtUtc,
                    row_version             = row_version + 1
                WHERE vehicle_id = @VehicleId
                  AND row_version = @ExpectedRowVersion;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var parameters = new
            {
                vehicle.VehicleId,
                vehicle.VehicleTypeId,
                vehicle.Vin,
                vehicle.ChassisNumber,
                vehicle.BodyNumber,
                vehicle.EngineNumber,
                vehicle.Year,
                vehicle.Make,
                vehicle.Marque,
                vehicle.Model,
                vehicle.Trim,
                vehicle.SeriesName,
                vehicle.BodyStyle,
                vehicle.Drivetrain,
                vehicle.Engine,
                vehicle.Transmission,
                vehicle.Mileage,
                vehicle.OdometerUnit,
                vehicle.OdometerStatus,
                vehicle.ExteriorColor,
                vehicle.InteriorColor,
                vehicle.TitleStatus,
                vehicle.AuthenticityProfile,
                vehicle.OriginalityProfile,
                vehicle.RestorationLevel,
                vehicle.DocumentationLevel,
                vehicle.OverallConditionScore,
                vehicle.HasRedFlags,
                vehicle.Notes,
                vehicle.OwnershipNotes,
                vehicle.VehicleStatus,
                vehicle.UpdatedAtUtc,
                ExpectedRowVersion = vehicle.RowVersion
            };

            var affected = await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    parameters,
                    cancellationToken: cancellationToken));

            if (affected == 0)
            {
                var current = await GetByIdAsync(vehicle.VehicleId, cancellationToken);

                if (current is null)
                    return null;

                throw new ConflictException($"Vehicle '{vehicle.VehicleId}' was modified by another request.");
            }

            return await GetByIdAsync(vehicle.VehicleId, cancellationToken);
        }

        public async Task<bool> ArchiveAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            if (vehicleId == Guid.Empty)
            {
                throw new ArgumentException("Vehicle ID is required.", nameof(vehicleId));
            }

            const string sql = """
                UPDATE vehicles
                SET
                    vehicle_status = @VehicleStatus,
                    updated_at_utc = @UpdatedAtUtc,
                    row_version = row_version + 1
                WHERE vehicle_id = @VehicleId;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            var affected = await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        VehicleId = vehicleId,
                        VehicleStatus = VehicleStatuses.Archived,
                        UpdatedAtUtc = DateTime.UtcNow
                    },
                    cancellationToken: cancellationToken));

            return affected > 0;
        }

        public async Task<bool> MergeAsync(
            Guid sourceVehicleId,
            Guid targetVehicleId,
            CancellationToken cancellationToken = default)
        {
            if (sourceVehicleId == Guid.Empty)
            {
                throw new ArgumentException("Source vehicle ID is required.", nameof(sourceVehicleId));
            }

            if (targetVehicleId == Guid.Empty)
            {
                throw new ArgumentException("Target vehicle ID is required.", nameof(targetVehicleId));
            }

            const string moveAppraisalsSql = """
                UPDATE appraisals
                SET
                    vehicle_id = @TargetVehicleId,
                    updated_at_utc = @UpdatedAtUtc,
                    row_version = row_version + 1
                WHERE vehicle_id = @SourceVehicleId;
                """;

            const string issuedAppraisalCountSql = """
                SELECT COUNT(1)
                FROM appraisals
                WHERE vehicle_id = @SourceVehicleId
                  AND status = @IssuedStatus;
                """;

            const string markMergedSql = """
                UPDATE vehicles
                SET
                    vehicle_status = @VehicleStatus,
                    updated_at_utc = @UpdatedAtUtc,
                    row_version = row_version + 1
                WHERE vehicle_id = @SourceVehicleId;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                var nowUtc = DateTime.UtcNow;

                var issuedAppraisalCount = await connection.ExecuteScalarAsync<int>(
                    new CommandDefinition(
                        issuedAppraisalCountSql,
                        new
                        {
                            SourceVehicleId = sourceVehicleId,
                            IssuedStatus = "Issued"
                        },
                        transaction: transaction,
                        cancellationToken: cancellationToken));

                if (issuedAppraisalCount > 0)
                {
                    throw new ConflictException(
                        "Cannot merge vehicle because at least one linked appraisal is in Issued status.");
                }

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        moveAppraisalsSql,
                        new
                        {
                            SourceVehicleId = sourceVehicleId,
                            TargetVehicleId = targetVehicleId,
                            UpdatedAtUtc = nowUtc
                        },
                        transaction: transaction,
                        cancellationToken: cancellationToken));

                var affected = await connection.ExecuteAsync(
                    new CommandDefinition(
                        markMergedSql,
                        new
                        {
                            SourceVehicleId = sourceVehicleId,
                            VehicleStatus = VehicleStatuses.Merged,
                            UpdatedAtUtc = nowUtc
                        },
                        transaction: transaction,
                        cancellationToken: cancellationToken));

                transaction.Commit();
                return affected > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task DeleteAsync(
            Guid vehicleId,
            CancellationToken cancellationToken = default)
        {
            const string sql = """
                DELETE FROM vehicles
                WHERE vehicle_id = @VehicleId
                LIMIT 1;
                """;

            using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            await connection.ExecuteAsync(new CommandDefinition(sql, new { VehicleId = vehicleId }, cancellationToken: cancellationToken));
        }

        private static VehicleEntity Map(VehicleRow row)
        {
            var normalizedMileage = row.Mileage is < 0 ? null : row.Mileage;

            return VehicleEntity.Rehydrate(
                vehicleId: row.VehicleId,
                vehicleTypeId: row.VehicleTypeId,
                vin: row.Vin,
                year: row.Year,
                make: row.Make,
                marque: row.Marque,
                model: row.Model,
                trim: row.Trim,
                seriesName: row.SeriesName,
                bodyStyle: row.BodyStyle,
                chassisNumber: row.ChassisNumber,
                bodyNumber: row.BodyNumber,
                engineNumber: row.EngineNumber,
                engine: row.Engine,
                transmission: row.Transmission,
                drivetrain: row.Drivetrain,
                mileage: normalizedMileage,
                odometerUnit: row.OdometerUnit,
                odometerStatus: row.OdometerStatus,
                exteriorColor: row.ExteriorColor,
                interiorColor: row.InteriorColor,
                titleStatus: row.TitleStatus,
                authenticityProfile: row.AuthenticityProfile,
                originalityProfile: row.OriginalityProfile,
                restorationLevel: row.RestorationLevel,
                documentationLevel: row.DocumentationLevel,
                overallConditionScore: row.OverallConditionScore,
                hasRedFlags: row.HasRedFlags,
                notes: row.Notes,
                ownershipNotes: row.OwnershipNotes,
                vehicleStatus: row.VehicleStatus,
                rowVersion: row.RowVersion,
                createdAtUtc: row.CreatedAtUtc,
                updatedAtUtc: row.UpdatedAtUtc);
        }

        private sealed class VehicleRow
        {
            public Guid VehicleId { get; init; }
            public Guid? VehicleTypeId { get; init; }

            public string? Vin { get; init; }
            public string? ChassisNumber { get; init; }
            public string? BodyNumber { get; init; }
            public string? EngineNumber { get; init; }

            public int Year { get; init; }
            public string Make { get; init; } = string.Empty;
            public string? Marque { get; init; }
            public string Model { get; init; } = string.Empty;
            public string? Trim { get; init; }
            public string? SeriesName { get; init; }
            public string? BodyStyle { get; init; }

            public string? Drivetrain { get; init; }
            public string? Engine { get; init; }
            public string? Transmission { get; init; }

            public int? Mileage { get; init; }
            public string? OdometerUnit { get; init; }
            public string? OdometerStatus { get; init; }

            public string? ExteriorColor { get; init; }
            public string? InteriorColor { get; init; }

            public string? TitleStatus { get; init; }
            public string? AuthenticityProfile { get; init; }
            public string? OriginalityProfile { get; init; }
            public string? RestorationLevel { get; init; }
            public string? DocumentationLevel { get; init; }

            public decimal? OverallConditionScore { get; init; }
            public bool HasRedFlags { get; init; }

            public string? Notes { get; init; }
            public string? OwnershipNotes { get; init; }

            public string? VehicleStatus { get; init; }
            public long RowVersion { get; init; }

            public DateTime CreatedAtUtc { get; init; }
            public DateTime UpdatedAtUtc { get; init; }
        }
    }
}