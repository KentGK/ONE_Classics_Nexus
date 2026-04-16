namespace ONE_Classics_Nexus.Application.DTOs;

public sealed class ComparableDto
{
    public Guid ComparableId { get; set; }
    public Guid AppraisalId { get; set; }

    public int Year { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Trim { get; set; }

    public string Source { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }

    public string? BodyStyle { get; set; }
    public string? Drivetrain { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public int? Mileage { get; set; }

    public string? OriginalityLevel { get; set; }
    public string? RestorationLevel { get; set; }
    public string? TitleStatus { get; set; }

    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}