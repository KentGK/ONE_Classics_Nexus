namespace ONE_Classics_Nexus.Domain.Constants;

public static class DocumentationLevels
{
    public const string Unknown = "Unknown";
    public const string Minimal = "Minimal";
    public const string Partial = "Partial";
    public const string Good = "Good";
    public const string Extensive = "Extensive";
    public const string Exceptional = "Exceptional";

    public static IReadOnlySet<string> AllowedLevels { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Unknown,
        Minimal,
        Partial,
        Good,
        Extensive,
        Exceptional
    };

    /// <summary>
    /// Returns canonical documentation level value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        // Legacy values persisted before controlled list enforcement.
        if (trimmed.Equals("None", StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals("Moderate", StringComparison.OrdinalIgnoreCase))
        {
            return Good;
        }

        if (trimmed.Equals("Complete", StringComparison.OrdinalIgnoreCase))
        {
            return Exceptional;
        }

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals(Minimal, StringComparison.OrdinalIgnoreCase))
        {
            return Minimal;
        }

        if (trimmed.Equals(Partial, StringComparison.OrdinalIgnoreCase))
        {
            return Partial;
        }

        if (trimmed.Equals(Good, StringComparison.OrdinalIgnoreCase))
        {
            return Good;
        }

        if (trimmed.Equals(Extensive, StringComparison.OrdinalIgnoreCase))
        {
            return Extensive;
        }

        if (trimmed.Equals(Exceptional, StringComparison.OrdinalIgnoreCase))
        {
            return Exceptional;
        }

        return null;
    }
}
