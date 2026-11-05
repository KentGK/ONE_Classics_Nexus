using System.ComponentModel.DataAnnotations;

namespace ONE_Classics_Nexus.API.Contracts.Clients;

public sealed record CreateClientRequest(
    [Required, StringLength(30)] string ClientTypeCode,
    [StringLength(100)] string? FirstName,
    [StringLength(100)] string? LastName,
    [StringLength(100)] string? MiddleName,
    [StringLength(200)] string? DisplayName,
    [Required, StringLength(254), EmailAddress] string Email,
    [Required, StringLength(40)] string Phone,
    [StringLength(200)] string? OrganizationName,
    [StringLength(100)] string? OrganizationType,
    [Required, StringLength(20)] string StatusCode,
    [StringLength(200)] string? AddressLine1,
    [StringLength(200)] string? AddressLine2,
    [StringLength(100)] string? City,
    [StringLength(100)] string? StateRegion,
    [StringLength(30)] string? PostalCode,
    [StringLength(20)] string? PreferredContactMethod,
    string? Notes);

public sealed record UpdateClientRequest(
    [Required, StringLength(30)] string ClientTypeCode,
    [StringLength(100)] string? FirstName,
    [StringLength(100)] string? LastName,
    [StringLength(100)] string? MiddleName,
    [Required, StringLength(200)] string DisplayName,
    [Required, StringLength(254), EmailAddress] string Email,
    [Required, StringLength(40)] string Phone,
    [StringLength(200)] string? OrganizationName,
    [StringLength(100)] string? OrganizationType,
    [Required, StringLength(20)] string StatusCode,
    [StringLength(200)] string? AddressLine1,
    [StringLength(200)] string? AddressLine2,
    [StringLength(100)] string? City,
    [StringLength(100)] string? StateRegion,
    [StringLength(30)] string? PostalCode,
    [StringLength(20)] string? PreferredContactMethod,
    string? Notes);

public sealed record ClientResponse(
    Guid ClientId,
    string ClientNumber,
    string ClientTypeCode,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string DisplayName,
    string Email,
    string Phone,
    string? OrganizationName,
    string? OrganizationType,
    string StatusCode,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? StateRegion,
    string? PostalCode,
    string? PreferredContactMethod,
    string? Notes,
    long RowVersion,
    DateTime CreatedAtUtc,
    Guid CreatedByUserId,
    DateTime UpdatedAtUtc,
    Guid UpdatedByUserId);

public sealed record CreateClientContactRequest(
    [StringLength(100)] string? ContactName,
    [StringLength(100)] string? ContactRole,
    [StringLength(200)] string? Email,
    [StringLength(100)] string? Phone,
    bool IsPrimary);

public sealed record UpdateClientContactRequest(
    [StringLength(100)] string? ContactName,
    [StringLength(100)] string? ContactRole,
    [StringLength(200)] string? Email,
    [StringLength(100)] string? Phone,
    bool IsPrimary);

public sealed record ClientContactResponse(
    Guid ClientContactId,
    Guid ClientId,
    string ContactName,
    string? ContactRole,
    string? Email,
    string? Phone,
    bool IsPrimary,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public static class ClientResponseMapper
{
    public static ClientResponse MapClientResponse(
        ONE_Classics_Nexus.Domain.Entities.Clients client)
    {
        return new ClientResponse(
            client.ClientId,
            client.ClientNumber,
            client.ClientTypeCode,
            client.FirstName,
            client.LastName,
            client.MiddleName,
            client.DisplayName,
            client.Email,
            client.Phone,
            client.OrganizationName,
            client.OrganizationType,
            client.StatusCode,
            client.AddressLine1,
            client.AddressLine2,
            client.City,
            client.StateRegion,
            client.PostalCode,
            client.PreferredContactMethod,
            client.Notes,
            client.RowVersion,
            client.CreatedAtUtc,
            client.CreatedByUserId,
            client.UpdatedAtUtc,
            client.UpdatedByUserId);
    }
}