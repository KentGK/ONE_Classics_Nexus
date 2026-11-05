using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Constants;

public static class OdometerStatuses
{
    public const string Actual = "Actual";
    public const string ExceedsMechanicalLimits = "Exceeds Mechanical Limits";
    public const string NotActual = "Not Actual";
    public const string Tmu = "TMU";
    public const string Unknown = "Unknown";
    public const string Replaced = "Replaced";

    public static IReadOnlySet<string> AllowedStatuses { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Actual,
        ExceedsMechanicalLimits,
        NotActual,
        Tmu,
        Unknown,
        Replaced
    };

    /// <summary>
    /// Returns canonical odometer status value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        if (trimmed.Equals(Actual, StringComparison.OrdinalIgnoreCase))
        {
            return Actual;
        }

        if (trimmed.Equals(ExceedsMechanicalLimits, StringComparison.OrdinalIgnoreCase))
        {
            return ExceedsMechanicalLimits;
        }

        if (trimmed.Equals(NotActual, StringComparison.OrdinalIgnoreCase))
        {
            return NotActual;
        }

        if (trimmed.Equals(Tmu, StringComparison.OrdinalIgnoreCase))
        {
            return Tmu;
        }

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals(Replaced, StringComparison.OrdinalIgnoreCase))
        {
            return Replaced;
        }

        return null;
    }
}

namespace ONE_Classics_Nexus.Domain.Constants;

public static class OdometerStatuses
{
    public const string Actual = "Actual";
    public const string ExceedsMechanicalLimits = "Exceeds Mechanical Limits";
    public const string NotActual = "Not Actual";
    public const string Tmu = "TMU";
    public const string Unknown = "Unknown";
    public const string Replaced = "Replaced";

    public static IReadOnlySet<string> AllowedStatuses { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Actual,
        ExceedsMechanicalLimits,
        NotActual,
        Tmu,
        Unknown,
        Replaced
    };

    /// <summary>
    /// Returns canonical odometer status value when input is allowed; otherwise null.
    /// </summary>
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        if (trimmed.Equals(Actual, StringComparison.OrdinalIgnoreCase))
        {
            return Actual;
        }

        if (trimmed.Equals(ExceedsMechanicalLimits, StringComparison.OrdinalIgnoreCase))
        {
            return ExceedsMechanicalLimits;
        }

        if (trimmed.Equals(NotActual, StringComparison.OrdinalIgnoreCase))
        {
            return NotActual;
        }

        if (trimmed.Equals(Tmu, StringComparison.OrdinalIgnoreCase))
        {
            return Tmu;
        }

        if (trimmed.Equals(Unknown, StringComparison.OrdinalIgnoreCase))
        {
            return Unknown;
        }

        if (trimmed.Equals(Replaced, StringComparison.OrdinalIgnoreCase))
        {
            return Replaced;
        }

        return null;
    }
}
