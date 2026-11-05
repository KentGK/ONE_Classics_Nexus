namespace ONE_Classics_Nexus.Application.DTOs;

public sealed record VehicleHistoryDto(
    Guid VehicleId,
    IReadOnlyList<VehicleHistoryEventDto> Events);

public sealed record VehicleHistoryEventDto(
    string EventType,
    DateTime OccurredAtUtc,
    string Description,
    Guid? AppraisalId,
    Guid? InspectionId);
