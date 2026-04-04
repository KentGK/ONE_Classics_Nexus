using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Application.Requests;

public sealed class SaveInspectionRequest
{
    public Guid InspectionId { get; set; }
    public List<SaveConditionScoreRequest> Scores { get; set; } = new();
}

public sealed class SaveConditionScoreRequest
{
    public int SystemNumber { get; set; }
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
}