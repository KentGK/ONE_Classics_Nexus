namespace ONE_Classics_Nexus.Tests.Integration.Infrastructure;

public sealed class AppraisalSubjectSnapshotDbRow
{
    public string Appraisal_Subject_Snapshot_Id { get; init; } = string.Empty;
    public string Appraisal_Id { get; init; } = string.Empty;
    public string Vehicle_Id { get; init; } = string.Empty;
    public string? Vin { get; init; }
    public int Year { get; init; }
    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string Snapshot_Status { get; init; } = string.Empty;
    public long Row_Version { get; init; }
}
