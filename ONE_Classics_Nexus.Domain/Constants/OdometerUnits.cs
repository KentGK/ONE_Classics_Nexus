namespace ONE_Classics_Nexus.Domain.Constants;

public static class OdometerUnits
{
    public const string Miles = "miles";
    public const string Kilometers = "kilometers";
    public const string Hours = "hours";
    public const string Unknown = "unknown";
    public const string NotApplicable = "not_applicable";

    public static IReadOnlySet<string> AllowedUnits { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Miles,
        Kilometers,
        Hours,
        Unknown,
        NotApplicable
    };

    /// <summary>
    /// Returns canonical lower-case odometer unit value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        // Legacy values persisted before controlled list enforcement.
        if (trimmed.Equals("km", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("kms", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("kilometres", StringComparison.OrdinalIgnoreCase))
        {
            return Kilometers;
        }

        if (trimmed.Equals("mi", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("mile", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("miles", StringComparison.OrdinalIgnoreCase))
        {
            return Miles;
        }

        if (trimmed.Equals(Miles, StringComparison.OrdinalIgnoreCase))
        {
            return Miles;
        }

        if (trimmed.Equals(Kilometers, StringComparison.OrdinalIgnoreCase))
        {
            return Kilometers;
        }

        if (trimmed.Equals(Hours, StringComparison.OrdinalIgnoreCase))
        {
            return Hours;
        }

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals(NotApplicable, StringComparison.OrdinalIgnoreCase))
        {
            return NotApplicable;
        }

        return null;
    }
}
