using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IClientRepository
{
    Task<Clients> CreateAsync(
        Clients client,
        CancellationToken cancellationToken);

    Task<Clients?> GetByIdAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns a filtered, sorted, paged page of clients for the Client List.
    /// Filtering, sorting and paging are all applied in the database so the
    /// caller never materialises the full table.
    /// </summary>
    Task<PagedResult<Clients>> ListAsync(
        ClientBrowseFilter filter,
        CancellationToken cancellationToken);

    Task<Clients?> UpdateAsync(
        Clients client,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    Task<ClientContact> CreateContactAsync(
        ClientContact contact,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ClientContact>> GetContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken);

    Task<ClientContact?> GetContactByIdAsync(
        Guid clientContactId,
        CancellationToken cancellationToken);

    Task<ClientContact?> UpdateContactAsync(
        ClientContact contact,
        CancellationToken cancellationToken);

    Task<bool> DeleteContactAsync(
        Guid clientContactId,
        CancellationToken cancellationToken);
}