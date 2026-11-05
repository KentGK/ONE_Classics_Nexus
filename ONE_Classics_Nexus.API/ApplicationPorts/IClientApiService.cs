using ONE_Classics_Nexus.API.Contracts.Clients;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Requests;

namespace ONE_Classics_Nexus.API.ApplicationPorts;

public interface IClientApiService
{
    /// <summary>
    /// Returns the filtered, sorted, paged Client List.
    /// </summary>
    Task<PagedResult<ClientListItemDto>> ListAsync(
        ListClientsRequest request,
        CancellationToken cancellationToken);

    Task<ClientResponse> CreateAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken);

    Task<ClientResponse?> GetByIdAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        Guid clientId,
        UpdateClientRequest request,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    Task<ClientContactResponse> CreateContactAsync(
        Guid clientId,
        CreateClientContactRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ClientContactResponse>> GetContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    Task<ClientContactResponse?> GetContactByIdAsync(
        Guid clientContactId,
        CancellationToken cancellationToken);

    Task<bool> UpdateContactAsync(
        Guid clientContactId,
        UpdateClientContactRequest request,
        CancellationToken cancellationToken);

    Task<bool> DeleteContactAsync(
        Guid clientContactId,
        CancellationToken cancellationToken);
}