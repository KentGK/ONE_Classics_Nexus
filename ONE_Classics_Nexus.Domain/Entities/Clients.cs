using System;
namespace ONE_Classics_Nexus.Domain.Entities;

/// <summary>
/// Client master record, aligned to locked schema v2.0
/// (docs/04_Data/SQL/Clients/Clients_v2.0.sql).
/// </summary>
public sealed class Clients
{
    public Guid ClientId { get; set; }

    public string ClientNumber { get; set; } = string.Empty;

    public string ClientTypeCode { get; set; } = ClientTypeCodes.Individual;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? MiddleName { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? StateRegion { get; set; }

    public string? PostalCode { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? PreferredContactMethod { get; set; }

    public string? Notes { get; set; }

    public long RowVersion { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Guid UpdatedByUserId { get; set; }

    public string? OrganizationName { get; set; }

    public string? OrganizationType { get; set; }

    /// <summary>
    /// Lifecycle status. Stored in mixed case and displayed verbatim; see
    /// <see cref="ClientStatusCodes"/>.
    /// </summary>
    public string StatusCode { get; set; } = ClientStatusCodes.Active;

    public static Clients Create(
        string clientNumber,
        string clientTypeCode,
        string email,
        string phone,
        Guid createdByUserId,
        string? displayName = null,
        string? firstName = null,
        string? lastName = null,
        string? middleName = null,
        string? organizationName = null,
        string? organizationType = null,
        string statusCode = ClientStatusCodes.Active,
        string? addressLine1 = null,
        string? addressLine2 = null,
        string? city = null,
        string? stateRegion = null,
        string? postalCode = null,
        string? preferredContactMethod = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(clientNumber))
        {
            throw new ArgumentException("ClientNumber is required.", nameof(clientNumber));
        }

        if (string.IsNullOrWhiteSpace(clientTypeCode))
        {
            throw new ArgumentException("ClientTypeCode is required.", nameof(clientTypeCode));
        }

        // email and phone are NOT NULL in schema v2.0.
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Phone is required.", nameof(phone));
        }

        if (string.IsNullOrWhiteSpace(statusCode))
        {
            throw new ArgumentException("StatusCode is required.", nameof(statusCode));
        }

        var resolvedDisplayName = ResolveDisplayName(
            displayName,
            firstName,
            lastName,
            organizationName);

        if (string.IsNullOrWhiteSpace(resolvedDisplayName))
        {
            throw new ArgumentException(
                "DisplayName is required, or enough name detail to derive it.",
                nameof(displayName));
        }

        var now = DateTime.UtcNow;

        return new Clients
        {
            ClientId = Guid.NewGuid(),
            ClientNumber = clientNumber.Trim(),
            ClientTypeCode = clientTypeCode.Trim(),
            FirstName = Normalize(firstName),
            LastName = Normalize(lastName),
            MiddleName = Normalize(middleName),
            DisplayName = resolvedDisplayName,
            Email = email.Trim(),
            Phone = phone.Trim(),
            OrganizationName = Normalize(organizationName),
            OrganizationType = Normalize(organizationType),
            StatusCode = statusCode.Trim(),
            AddressLine1 = Normalize(addressLine1),
            AddressLine2 = Normalize(addressLine2),
            City = Normalize(city),
            StateRegion = Normalize(stateRegion),
            PostalCode = Normalize(postalCode),
            PreferredContactMethod = Normalize(preferredContactMethod),
            Notes = Normalize(notes),
            RowVersion = 1,
            CreatedAtUtc = now,
            CreatedByUserId = createdByUserId,
            UpdatedAtUtc = now,
            UpdatedByUserId = createdByUserId
        };
    }

    public void Touch(Guid updatedByUserId)
    {
        UpdatedAtUtc = DateTime.UtcNow;
        UpdatedByUserId = updatedByUserId;
    }

    /// <summary>
    /// Derives the display name when one is not supplied: an explicit value
    /// wins, then the organization name, then the person's name parts.
    /// </summary>
    private static string ResolveDisplayName(
        string? displayName,
        string? firstName,
        string? lastName,
        string? organizationName)
    {
        if (!string.IsNullOrWhiteSpace(displayName))
        {
            return displayName.Trim();
        }

        if (!string.IsNullOrWhiteSpace(organizationName))
        {
            return organizationName.Trim();
        }

        return string.Join(
            ' ',
            new[] { firstName, lastName }
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .Select(part => part!.Trim()));
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

/// <summary>
/// Client lifecycle status values, stored and displayed in mixed case.
/// </summary>
public static class ClientStatusCodes
{
    public const string Active = "Active";
    public const string Inactive = "Inactive";
    public const string Archived = "Archived";

    public static readonly IReadOnlyList<string> All = [Active, Inactive, Archived];

    public static bool IsValid(string? value) =>
        value is not null
        && All.Any(code => string.Equals(code, value.Trim(), StringComparison.OrdinalIgnoreCase));
}

/// <summary>
/// Client type values surfaced in the Client List Type column.
/// </summary>
public static class ClientTypeCodes
{
    public const string Individual = "Individual";
    public const string Organization = "Organization";

    public static readonly IReadOnlyList<string> All = [Individual, Organization];
}
