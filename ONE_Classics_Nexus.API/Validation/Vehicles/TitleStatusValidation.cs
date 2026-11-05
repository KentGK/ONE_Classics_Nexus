namespace ONE_Classics_Nexus.API.Validation.Vehicles;

internal static class TitleStatusValidation
{
    internal static readonly string[] AllowedStatuses =
    [
        "Clean",
        "Salvage",
        "Rebuilt",
        "Flood",
        "Bonded",
        "Parts Only",
        "NoTitle",
        "Bill Of Sale",
        "Foreign Registration",
        "Unknown"
    ];

    internal static bool IsAllowed(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            || AllowedStatuses.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
