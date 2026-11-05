namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class DocumentationLevelValidation
{
    internal static readonly string[] AllowedLevels =
    [
        "Unknown",
        "Minimal",
        "Partial",
        "Good",
        "Extensive",
        "Exceptional"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedLevels.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
