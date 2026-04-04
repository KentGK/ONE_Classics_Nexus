using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record InspectionDto(
    Guid InspectionId,
    Guid AppraisalId,
    string InspectionType,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    IReadOnlyList<ConditionScoreDto> ConditionScores);