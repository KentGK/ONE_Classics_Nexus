using System;
namespace ONE_Classics_Nexus.Domain.Entities;

public sealed class ClientContact
{
    public Guid ClientContactId { get; set; }

    public Guid ClientId { get; set; }

    public string ContactType { get; set; } = "Primary";

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? JobTitle { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; } = true;

    public string? Notes { get; set; }

    public long RowVersion { get; set; } = 1;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public static ClientContact Create(
        Guid clientId,
        string contactType = "Primary",
        string? firstName = null,
        string? lastName = null,
        string? jobTitle = null,
        string? email = null,
        string? phone = null,
        bool isPrimary = false)
    {
        if (clientId == Guid.Empty)
        {
            throw new ArgumentException(
                "ClientId is required.",
                nameof(clientId));
        }

        if (string.IsNullOrWhiteSpace(contactType))
        {
            throw new ArgumentException(
                "ContactType is required.",
                nameof(contactType));
        }

        var now = DateTime.UtcNow;

        return new ClientContact
        {
            ClientContactId = Guid.NewGuid(),
            ClientId = clientId,
            ContactType = contactType.Trim(),
            FirstName = Normalize(firstName),
            LastName = Normalize(lastName),
            JobTitle = Normalize(jobTitle),
            Email = Normalize(email),
            Phone = Normalize(phone),
            IsPrimary = isPrimary,
            IsActive = true,
            RowVersion = 1,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
    }

    public void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
        RowVersion++;
    }

    public void SetPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}