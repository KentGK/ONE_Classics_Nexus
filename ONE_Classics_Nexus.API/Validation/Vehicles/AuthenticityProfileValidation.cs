namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class AuthenticityProfileValidation
{
    internal static readonly string[] AllowedProfiles =
    [
        "Unknown",
        "Original",
        "Restored",
        "Survivor",
        "Restomod",
        "Custom",
        "Tribute",
        "Industrial",
        "Industrial Standardized"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedProfiles.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase)
            || string.Equals(value?.Trim(), "Industry Standardized", StringComparison.OrdinalIgnoreCase);
    }
}
