namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class OriginalityProfileValidation
{
    internal static readonly string[] AllowedProfiles =
    [
        "Unknown",
        "Original",
        "Predominantly Original",
        "Restored",
        "Partially Restored",
        "Modified",
        "Highly Modified",
        "Reconstructed"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedProfiles.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
