namespace ONE_Classics_Nexus.Domain.Constants;

public static class VehicleStatuses
{
    public const string Active = "Active";
    public const string Archived = "Archived";
    public const string Merged = "Merged";
    public const string Duplicate = "Duplicate";

    public static IReadOnlySet<string> AllowedStatuses { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Active,
        Archived,
        Merged,
        Duplicate
    };

    /// <summary>
    /// Returns canonical vehicle status value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        if (trimmed.Equals(Active, StringComparison.OrdinalIgnoreCase))
        {
            return Active;
        }

        if (trimmed.Equals(Archived, StringComparison.OrdinalIgnoreCase))
        {
            return Archived;
        }

        if (trimmed.Equals(Merged, StringComparison.OrdinalIgnoreCase))
        {
            return Merged;
        }

        if (trimmed.Equals(Duplicate, StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("Duplicate Vehicle", StringComparison.OrdinalIgnoreCase))
        {
            return Duplicate;
        }

        return null;
    }
}
