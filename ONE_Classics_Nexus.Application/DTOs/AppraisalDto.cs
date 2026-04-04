using System;

namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record AppraisalDto(
    Guid AppraisalId,
    Guid VehicleId,
    string Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    long RowVersion);