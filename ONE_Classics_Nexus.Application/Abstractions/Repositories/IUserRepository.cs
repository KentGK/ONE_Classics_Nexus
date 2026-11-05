using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(
        User user, 
        CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(
        Guid userId, 
        CancellationToken cancellationToken);

    Task<IReadOnlyList<User>> ListAsync(
        CancellationToken cancellationToken);

    Task<User?> UpdateAsync(
        User user, 
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid userId, 
        CancellationToken cancellationToken);
}
