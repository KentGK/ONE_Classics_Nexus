using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Comparable;

public sealed class ComparableRepository : IComparableRepository
{
    private readonly MySqlConnectionFactory _factory;

    public ComparableRepository(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<ComparableDto> CreateAsync(Guid appraisalId, CreateComparableRequest request, CancellationToken ct)
    {
        var comparableId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        const string sql = @"
INSERT INTO comparable_sales
(
    comparable_id,
    appraisal_id,
    year,
    make,
    model,
    trim,
    source,
    sale_date,
    sale_price,
    body_style,
    drivetrain,
    engine,
    transmission,
    mileage,
    originality_level,
    restoration_level,
    title_status,
    overall_condition_score,
    has_red_flags,
    notes,
    created_at,
    updated_at
)
VALUES
(
    @ComparableId,
    @AppraisalId,
    @Year,
    @Make,
    @Model,
    @Trim,
    @Source,
    @SaleDate,
    @SalePrice,
    @BodyStyle,
    @Drivetrain,
    @Engine,
    @Transmission,
    @Mileage,
    @OriginalityLevel,
    @RestorationLevel,
    @TitleStatus,
    @OverallConditionScore,
    @HasRedFlags,
    @Notes,
    @CreatedAt,
    @UpdatedAt
);";

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        await conn.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    ComparableId = comparableId.ToString(),
                    AppraisalId = appraisalId.ToString(),
                    request.Year,
                    request.Make,
                    request.Model,
                    request.Trim,
                    request.Source,
                    SaleDate = request.SaleDate.ToDateTime(TimeOnly.MinValue),
                    request.SalePrice,
                    request.BodyStyle,
                    request.Drivetrain,
                    request.Engine,
                    request.Transmission,
                    request.Mileage,
                    request.OriginalityLevel,
                    request.RestorationLevel,
                    request.TitleStatus,
                    request.OverallConditionScore,
                    request.HasRedFlags,
                    request.Notes,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                cancellationToken: ct));

        return new ComparableDto
        {
            ComparableId = comparableId,
            AppraisalId = appraisalId,
            Year = request.Year,
            Make = request.Make,
            Model = request.Model,
            Trim = request.Trim,
            Source = request.Source,
            SaleDate = request.SaleDate.ToDateTime(TimeOnly.MinValue),
            SalePrice = request.SalePrice,
            BodyStyle = request.BodyStyle,
            Drivetrain = request.Drivetrain,
            Engine = request.Engine,
            Transmission = request.Transmission,
            Mileage = request.Mileage,
            OriginalityLevel = request.OriginalityLevel,
            RestorationLevel = request.RestorationLevel,
            TitleStatus = request.TitleStatus,
            OverallConditionScore = request.OverallConditionScore,
            HasRedFlags = request.HasRedFlags,
            Notes = request.Notes,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public async Task<IReadOnlyList<ComparableDto>> GetByAppraisalIdAsync(Guid appraisalId, CancellationToken ct)
    {
        const string sql = @"
            SELECT
                comparable_id            AS ComparableId,
                appraisal_id             AS AppraisalId,
                year                     AS Year,
                make                     AS Make,
                model                    AS Model,
                trim                     AS Trim,
                source                   AS Source,
                sale_date                AS SaleDate,
                sale_price               AS SalePrice,
                body_style               AS BodyStyle,
                drivetrain               AS Drivetrain,
                engine                   AS Engine,
                transmission             AS Transmission,
                mileage                  AS Mileage,
                originality_level        AS OriginalityLevel,
                restoration_level        AS RestorationLevel,
                title_status             AS TitleStatus,
                overall_condition_score  AS OverallConditionScore,
                has_red_flags            AS HasRedFlags,
                notes                    AS Notes,
                created_at               AS CreatedAt,
                updated_at               AS UpdatedAt
            FROM comparable_sales
            WHERE appraisal_id = @AppraisalId
            ORDER BY sale_date DESC, sale_price DESC;";

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<ComparableDto>(
            new CommandDefinition(
                sql,
                new { AppraisalId = appraisalId.ToString() },
                cancellationToken: ct));

        return rows.ToList();
    }
}