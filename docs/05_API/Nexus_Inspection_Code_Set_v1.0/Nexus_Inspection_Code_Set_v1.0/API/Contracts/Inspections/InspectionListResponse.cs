namespace ONE_Classics_Nexus.API.Contracts.Inspections;

public sealed class InspectionListResponse
{
    public IReadOnlyList<InspectionResponse> Items { get; set; } = Array.Empty<InspectionResponse>();
}
