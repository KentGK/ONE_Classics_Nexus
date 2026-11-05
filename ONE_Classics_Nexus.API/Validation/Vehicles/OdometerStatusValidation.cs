namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class OdometerStatusValidation
{
    internal static readonly string[] AllowedStatuses =
    [
        "Actual",
        "Exceeds Mechanical Limits",
        "Not Actual",
        "TMU",
        "Unknown",
        "Replaced"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedStatuses.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
