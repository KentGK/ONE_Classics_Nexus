namespace ONE_Classics_Nexus.Application.DTOs;

/// <summary>
/// A single Client List row. Carries the three displayed columns plus the
/// identifier needed to open the client for editing on row selection.
/// </summary>
public sealed record ClientListItemDto(
    Guid ClientId,
    string DisplayName,
    string? Email,
    string ClientTypeCode,
    string StatusCode);
