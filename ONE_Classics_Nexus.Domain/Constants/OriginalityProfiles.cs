namespace ONE_Classics_Nexus.Domain.Constants;

public static class OriginalityProfiles
{
    public const string Unknown = "Unknown";
    public const string Original = "Original";
    public const string PredominantlyOriginal = "Predominantly Original";
    public const string Restored = "Restored";
    public const string PartiallyRestored = "Partially Restored";
    public const string Modified = "Modified";
    public const string HighlyModified = "Highly Modified";
    public const string Reconstructed = "Reconstructed";

    public static IReadOnlySet<string> AllowedProfiles { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Unknown,
        Original,
        PredominantlyOriginal,
        Restored,
        PartiallyRestored,
        Modified,
        HighlyModified,
        Reconstructed
    };

    /// <summary>
    /// Returns canonical originality profile value when input is allowed; otherwise null.
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

        if (trimmed.Equals(PredominantlyOriginal, StringComparison.OrdinalIgnoreCase))
        {
            return PredominantlyOriginal;
        }

        if (trimmed.Equals(Restored, StringComparison.OrdinalIgnoreCase))
        {
            return Restored;
        }

        if (trimmed.Equals(PartiallyRestored, StringComparison.OrdinalIgnoreCase))
        {
            return PartiallyRestored;
        }

        if (trimmed.Equals(Modified, StringComparison.OrdinalIgnoreCase))
        {
            return Modified;
        }

        if (trimmed.Equals(HighlyModified, StringComparison.OrdinalIgnoreCase))
        {
            return HighlyModified;
        }

        if (trimmed.Equals(Reconstructed, StringComparison.OrdinalIgnoreCase))
        {
            return Reconstructed;
        }

        return null;
    }
}
