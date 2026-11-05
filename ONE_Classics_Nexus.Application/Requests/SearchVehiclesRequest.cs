namespace ONE_Classics_Nexus.Application.Requests;

public sealed class SearchVehiclesRequest
{
    public string? Search { get; set; }
    public string? Vin { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Series { get; set; }
    public Guid? VehicleTypeId { get; set; }
    public string? BodyStyle { get; set; }
    public string? TitleStatus { get; set; }
    public bool? HasRedFlags { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
