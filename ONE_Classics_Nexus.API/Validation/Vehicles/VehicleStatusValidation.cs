namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class VehicleStatusValidation
{
    internal static readonly string[] AllowedStatuses =
    [
        "Active",
        "Archived",
        "Merged",
        "Duplicate"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedStatuses.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase)
            || string.Equals(value?.Trim(), "Duplicate Vehicle", StringComparison.OrdinalIgnoreCase);
    }
}
