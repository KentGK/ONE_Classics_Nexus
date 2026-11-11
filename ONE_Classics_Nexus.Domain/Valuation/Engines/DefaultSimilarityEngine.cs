using ONE_Classics_Nexus.Domain.Valuation.Abstractions;
using ONE_Classics_Nexus.Domain.Valuation.Models;
using System;

namespace ONE_Classics_Nexus.Domain.Valuation.Engines;

public sealed class DefaultSimilarityEngine : ISimilarityEngine
{
    public decimal CalculateSimilarity0To1(SubjectVehicle subject, ComparableSale comparable)
    {
        decimal score = 0m;

        score += Match(subject.Series, comparable.Series) * 0.20m;
        score += Match(subject.BodyStyle, comparable.BodyStyle) * 0.20m;
        score += Match(subject.Drivetrain, comparable.Drivetrain) * 0.15m;
        score += Match(subject.Engine, comparable.Engine) * 0.15m;
        score += Match(subject.Transmission, comparable.Transmission) * 0.10m;
        score += YearProximity(subject.Year, comparable.Year) * 0.10m;
        score += MileageProximity(subject.Mileage, comparable.Mileage) * 0.10m;

        return Clamp(score, 0m, 1m);
    }

    private static decimal Match(string? a, string? b)
    {
        if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
            return 0.50m;

        return string.Equals(a.Trim(), b.Trim(), StringComparison.OrdinalIgnoreCase)
            ? 1.00m
            : 0.00m;
    }

    private static decimal YearProximity(int a, int b)
    {
        int diff = Math.Abs(a - b);

        if (diff == 0) return 1.00m;
        if (diff == 1) return 0.85m;
        if (diff == 2) return 0.70m;
        if (diff <= 4) return 0.50m;
        return 0.20m;
    }

    private static decimal MileageProximity(decimal? a, decimal? b)
    {
        if (!a.HasValue || !b.HasValue)
            return 0.50m;

        decimal diff = Math.Abs(a.Value - b.Value);

        if (diff <= 1_000m) return 1.00m;
        if (diff <= 5_000m) return 0.85m;
        if (diff <= 15_000m) return 0.65m;
        if (diff <= 30_000m) return 0.45m;
        return 0.20m;
    }

    private static decimal Clamp(decimal value, decimal min, decimal max)
        => Math.Min(max, Math.Max(min, value));
}