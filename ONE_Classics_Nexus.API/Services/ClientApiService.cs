using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Clients;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Clients;
using ONE_Classics_Nexus.Domain.Entities;
using PagedResult = ONE_Classics_Nexus.Application.Abstractions.Paging.PagedResult<ONE_Classics_Nexus.Application.DTOs.ClientListItemDto>;

namespace ONE_Classics_Nexus.API.Services;

public sealed class ClientApiService(
    IClientRepository clientRepository,
    IAutoNumberGenerator autoNumberGenerator,
    ListClientsHandler listClientsHandler,
    ICurrentNexusUser currentUser) : IClientApiService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAutoNumberGenerator _autoNumberGenerator = autoNumberGenerator;
    private readonly ListClientsHandler _listClientsHandler = listClientsHandler;
    private readonly ICurrentNexusUser _currentUser = currentUser;

    public Task<PagedResult> ListAsync(
        ListClientsRequest request,
        CancellationToken cancellationToken) =>
        _listClientsHandler.HandleAsync(request, cancellationToken);

    public async Task<ClientResponse> CreateAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var actorId = RequireCurrentUserId();

        var clientNumber = await _autoNumberGenerator
            .NextAsync("client", "CN", 6, cancellationToken)
            .ConfigureAwait(false);

        var client = Clients.Create(
            clientNumber: clientNumber,
            clientTypeCode: request.ClientTypeCode,
            email: request.Email,
            phone: request.Phone,
            createdByUserId: actorId,
            displayName: request.DisplayName,
            firstName: request.FirstName,
            lastName: request.LastName,
            middleName: request.MiddleName,
            organizationName: request.OrganizationName,
            organizationType: request.OrganizationType,
            statusCode: request.StatusCode,
            addressLine1: request.AddressLine1,
            addressLine2: request.AddressLine2,
            city: request.City,
            stateRegion: request.StateRegion,
            postalCode: request.PostalCode,
            preferredContactMethod: request.PreferredContactMethod,
            notes: request.Notes);

        var createdClient = await _clientRepository.CreateAsync(
            client,
            cancellationToken);

        return MapClientResponse(createdClient);
    }

    public async Task<ClientResponse?> GetByIdAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(
            clientId,
            cancellationToken);

        return client is null
            ? null
            : MapClientResponse(client);
    }

    public async Task<bool> UpdateAsync(
        Guid clientId,
        UpdateClientRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var actorId = RequireCurrentUserId();

        var client = await _clientRepository.GetByIdAsync(
            clientId,
            cancellationToken);

        if (client is null)
        {
            return false;
        }

        client.ClientTypeCode = request.ClientTypeCode;
        client.FirstName = request.FirstName;
        client.LastName = request.LastName;
        client.MiddleName = request.MiddleName;
        client.DisplayName = request.DisplayName;
        client.Email = request.Email;
        client.Phone = request.Phone;
        client.OrganizationName = request.OrganizationName;
        client.OrganizationType = request.OrganizationType;
        client.StatusCode = request.StatusCode;
        client.AddressLine1 = request.AddressLine1;
        client.AddressLine2 = request.AddressLine2;
        client.City = request.City;
        client.StateRegion = request.StateRegion;
        client.PostalCode = request.PostalCode;
        client.PreferredContactMethod = request.PreferredContactMethod;
        client.Notes = request.Notes;
        client.Touch(actorId);

        var updatedClient = await _clientRepository.UpdateAsync(
            client,
            cancellationToken);

        return updatedClient is not null;
    }

    /// <summary>
    /// Resolves the acting user for the audit columns, which schema v2.0
    /// declares NOT NULL. Fails closed rather than writing a placeholder id.
    /// </summary>
    private Guid RequireCurrentUserId() =>
        _currentUser.UserId
        ?? throw new UnauthorizedAccessException(
            "An authenticated user id is required to write client records.");
    

    public Task<bool> DeleteAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        return _clientRepository.DeleteAsync(
            clientId,
            cancellationToken);
    }

    public async Task<ClientContactResponse> CreateContactAsync(
        Guid clientId,
        CreateClientContactRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var contact = ClientContact.Create(
            clientId: clientId,
            firstName: request.ContactName,
            jobTitle: request.ContactRole,
            email: request.Email,
            phone: request.Phone,
            isPrimary: request.IsPrimary);

        var createdContact = await _clientRepository.CreateContactAsync(
            contact,
            cancellationToken);

        return MapContactResponse(createdContact);
    }

    public async Task<IReadOnlyList<ClientContactResponse>> GetContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var contacts = await _clientRepository.GetContactsAsync(
            clientId,
            cancellationToken);

        return contacts
            .Select(MapContactResponse)
            .ToArray();
    }

    public async Task<ClientContactResponse?> GetContactByIdAsync(
        Guid clientContactId,
        CancellationToken cancellationToken)
    {
        var contact = await _clientRepository.GetContactByIdAsync(
            clientContactId,
            cancellationToken);

        return contact is null
            ? null
            : MapContactResponse(contact);
    }

    public async Task<bool> UpdateContactAsync(
        Guid clientContactId,
        UpdateClientContactRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var contact = await _clientRepository.GetContactByIdAsync(
            clientContactId,
            cancellationToken);

        if (contact is null)
        {
            return false;
        }

        contact.FirstName = request.ContactName;
        contact.JobTitle = request.ContactRole;
        contact.Email = request.Email;
        contact.Phone = request.Phone;
        contact.IsPrimary = request.IsPrimary;
        contact.Touch();

        var updatedContact = await _clientRepository.UpdateContactAsync(
            contact,
            cancellationToken);

        return updatedContact is not null;
    }

    public Task<bool> DeleteContactAsync(
        Guid clientContactId,
        CancellationToken cancellationToken)
    {
        return _clientRepository.DeleteContactAsync(
            clientContactId,
            cancellationToken);
    }

    private static ClientResponse MapClientResponse(Clients client) =>
        ClientResponseMapper.MapClientResponse(client);

    private static ClientContactResponse MapContactResponse(
        ClientContact contact)
    {
        var contactName = $"{contact.FirstName} {contact.LastName}".Trim();

        return new ClientContactResponse(
            contact.ClientContactId,
            contact.ClientId,
            contactName,
            contact.JobTitle,
            contact.Email,
            contact.Phone,
            contact.IsPrimary,
            contact.CreatedAtUtc,
            contact.UpdatedAtUtc);
    }
}