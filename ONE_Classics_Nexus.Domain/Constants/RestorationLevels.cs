namespace ONE_Classics_Nexus.Domain.Constants;

public static class RestorationLevels
{
    public const string Unknown = "Unknown";
    public const string Unrestored = "Unrestored";
    public const string Preserved = "Preserved";
    public const string DriverRestoration = "Driver Restoration";
    public const string OlderRestoration = "Older Restoration";
    public const string FullRestoration = "Full Restoration";
    public const string ConcoursRestoration = "Concours Restoration";
    public const string Restomod = "Restomod";
    public const string Reconstruction = "Reconstruction";

    public static IReadOnlySet<string> AllowedLevels { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Unknown,
        Unrestored,
        Preserved,
        DriverRestoration,
        OlderRestoration,
        FullRestoration,
        ConcoursRestoration,
        Restomod,
        Reconstruction
    };

    /// <summary>
    /// Returns canonical restoration level value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        // Legacy values persisted before controlled list enforcement.
        if (trimmed.Equals("DriverRestoration", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("DriverRestorationLevel", StringComparison.OrdinalIgnoreCase))
        {
            return DriverRestoration;
        }

        if (trimmed.Equals("OlderRestoration", StringComparison.OrdinalIgnoreCase))
        {
            return OlderRestoration;
        }

        if (trimmed.Equals("FullRestoration", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("Recent Restoration", StringComparison.OrdinalIgnoreCase))
        {
            return FullRestoration;
        }

        if (trimmed.Equals("ConcoursRestoration", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("Show Quality", StringComparison.OrdinalIgnoreCase))
        {
            return ConcoursRestoration;
        }

        if (trimmed.Equals("None", StringComparison.OrdinalIgnoreCase))
        {
            return Unrestored;
        }

        if (trimmed.Equals("Partial", StringComparison.OrdinalIgnoreCase))
        {
            return Preserved;
        }

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals(Unrestored, StringComparison.OrdinalIgnoreCase))
        {
            return Unrestored;
        }

        if (trimmed.Equals(Preserved, StringComparison.OrdinalIgnoreCase))
        {
            return Preserved;
        }

        if (trimmed.Equals(DriverRestoration, StringComparison.OrdinalIgnoreCase))
        {
            return DriverRestoration;
        }

        if (trimmed.Equals(OlderRestoration, StringComparison.OrdinalIgnoreCase))
        {
            return OlderRestoration;
        }

        if (trimmed.Equals(FullRestoration, StringComparison.OrdinalIgnoreCase))
        {
            return FullRestoration;
        }

        if (trimmed.Equals(ConcoursRestoration, StringComparison.OrdinalIgnoreCase))
        {
            return ConcoursRestoration;
        }

        if (trimmed.Equals(Restomod, StringComparison.OrdinalIgnoreCase))
        {
            return Restomod;
        }

        if (trimmed.Equals(Reconstruction, StringComparison.OrdinalIgnoreCase))
        {
            return Reconstruction;
        }

        return null;
    }
}
