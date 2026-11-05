namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class OdometerUnitValidation
{
    internal static readonly string[] AllowedUnits =
    [
        "miles",
        "kilometers",
        "hours",
        "unknown",
        "not_applicable"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedUnits.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
