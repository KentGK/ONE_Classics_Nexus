namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class RestorationLevelValidation
{
    internal static readonly string[] AllowedLevels =
    [
        "Unknown",
        "Unrestored",
        "Preserved",
        "Driver Restoration",
        "Older Restoration",
        "Full Restoration",
        "Concours Restoration",
        "Restomod",
        "Reconstruction"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedLevels.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
