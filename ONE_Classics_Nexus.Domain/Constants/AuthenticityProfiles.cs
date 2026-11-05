namespace ONE_Classics_Nexus.Domain.Constants;

public static class AuthenticityProfiles
{
    public const string Unknown = "Unknown";
    public const string Original = "Original";
    public const string Restored = "Restored";
    public const string Survivor = "Survivor";
    public const string Restomod = "Restomod";
    public const string Custom = "Custom";
    public const string Tribute = "Tribute";
    public const string Industrial = "Industrial";
    public const string IndustrialStandardized = "Industrial Standardized";

    public static IReadOnlySet<string> AllowedProfiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Unknown,
        Original,
        Restored,
        Survivor,
        Restomod,
        Custom,
        Tribute,
        Industrial,
        IndustrialStandardized
    };

    /// <summary>
    /// Returns canonical authenticity profile value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals(Original, StringComparison.OrdinalIgnoreCase))
        {
            return Original;
        }

        if (trimmed.Equals(Restored, StringComparison.OrdinalIgnoreCase))
        {
            return Restored;
        }

        if (trimmed.Equals(Survivor, StringComparison.OrdinalIgnoreCase))
        {
            return Survivor;
        }

        if (trimmed.Equals(Restomod, StringComparison.OrdinalIgnoreCase))
        {
            return Restomod;
        }

        if (trimmed.Equals(Custom, StringComparison.OrdinalIgnoreCase))
        {
            return Custom;
        }

        if (trimmed.Equals(Tribute, StringComparison.OrdinalIgnoreCase))
        {
            return Tribute;
        }

        if (trimmed.Equals(Industrial, StringComparison.OrdinalIgnoreCase))
        {
            return Industrial;
        }

        if (trimmed.Equals(IndustrialStandardized, StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("Industry Standardized", StringComparison.OrdinalIgnoreCase))
        {
            return IndustrialStandardized;
        }

        return null;
    }
}
