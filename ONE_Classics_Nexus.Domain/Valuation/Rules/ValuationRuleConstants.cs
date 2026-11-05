namespace ONE_Classics_Nexus.Domain.Valuation.Rules;

public static class ValuationRuleConstants
{
    public const int ExpectedSystemCount = 12;
    public const decimal RedFlagPenaltyPerItem = 5m;
    public const decimal MaxRedFlagPenalty = 25m;
    public const decimal DefaultMonthlyTimeAdjustmentRate = 0.0025m;
    public const decimal DefaultMileageAdjustmentRatePerMile = 0.10m;
    public const decimal BaseRangeWidthPct = 0.05m;
    public const decimal VariableRangeWidthPct = 0.15m;
}