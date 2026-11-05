namespace ONE_Classics_Nexus.Domain.Constants;

public static class TitleStatuses
{
    public const string Clean = "Clean";
    public const string Salvage = "Salvage";
    public const string Rebuilt = "Rebuilt";
    public const string Flood = "Flood";
    public const string Bonded = "Bonded";
    public const string PartsOnly = "Parts Only";
    public const string NoTitle = "NoTitle";
    public const string BillOfSale = "Bill Of Sale";
    public const string ForeignRegistration = "Foreign Registration";
    public const string Unknown = "Unknown";

    public static IReadOnlySet<string> AllowedStatuses { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Clean,
        Salvage,
        Rebuilt,
        Flood,
        Bonded,
        PartsOnly,
        NoTitle,
        BillOfSale,
        ForeignRegistration,
        Unknown
    };

    /// <summary>
    /// Returns canonical title status value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        if (trimmed.Equals(Clean, StringComparison.OrdinalIgnoreCase))
        {
            return Clean;
        }

        if (trimmed.Equals(Salvage, StringComparison.OrdinalIgnoreCase))
        {
            return Salvage;
        }

        if (trimmed.Equals(Rebuilt, StringComparison.OrdinalIgnoreCase))
        {
            return Rebuilt;
        }

        if (trimmed.Equals(Flood, StringComparison.OrdinalIgnoreCase))
        {
            return Flood;
        }

        if (trimmed.Equals(Bonded, StringComparison.OrdinalIgnoreCase))
        {
            return Bonded;
        }

        if (trimmed.Equals(PartsOnly, StringComparison.OrdinalIgnoreCase))
        {
            return PartsOnly;
        }

        if (trimmed.Equals(NoTitle, StringComparison.OrdinalIgnoreCase))
        {
            return NoTitle;
        }

        if (trimmed.Equals(BillOfSale, StringComparison.OrdinalIgnoreCase))
        {
            return BillOfSale;
        }

        if (trimmed.Equals(ForeignRegistration, StringComparison.OrdinalIgnoreCase))
        {
            return ForeignRegistration;
        }

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        return null;
    }
}
