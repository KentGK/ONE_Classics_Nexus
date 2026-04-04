using System;

namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record ConditionScoreDto(
    Guid ConditionScoreId,
    Guid InspectionId,
    int SystemNumber,
    string SystemKey,
    decimal Score0To5,
    decimal Weight,
    bool IsRedFlag,
    string? Notes);