using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using UserEntity = ONE_Classics_Nexus.Domain.Entities.User;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.User;

public sealed class UserRepository : IUserRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public UserRepository(MySqlConnectionFactory connectionFactory) =>
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

    public async Task<UserEntity> CreateAsync(
        UserEntity user, 
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO users (user_id, email, username, display_name, first_name, last_name,
                auth_provider, external_subject, is_active, created_at_utc, updated_at_utc)
            VALUES (@UserId, @Email, @Username, @DisplayName, @FirstName, @LastName,
                @AuthProvider, @ExternalSubject, @IsActive, @CreatedAtUtc, @UpdatedAtUtc);
            """;
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));
        return user;
    }

    public async Task<UserEntity?> GetByIdAsync(
        Guid userId, 
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT user_id AS UserId, email AS Email, username AS Username, display_name AS DisplayName,
                first_name AS FirstName, last_name AS LastName, auth_provider AS AuthProvider,
                external_subject AS ExternalSubject, is_active AS IsActive,
                created_at_utc AS CreatedAtUtc, updated_at_utc AS UpdatedAtUtc
            FROM users WHERE user_id = @UserId LIMIT 1;
            """;
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<UserEntity>(new CommandDefinition(
            sql, new { UserId = userId.ToString() }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<UserEntity>> ListAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT user_id AS UserId, email AS Email, username AS Username, display_name AS DisplayName,
                first_name AS FirstName, last_name AS LastName, auth_provider AS AuthProvider,
                external_subject AS ExternalSubject, is_active AS IsActive,
                created_at_utc AS CreatedAtUtc, updated_at_utc AS UpdatedAtUtc
            FROM users ORDER BY created_at_utc DESC, user_id;
            """;
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var users = await connection.QueryAsync<UserEntity>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return users.ToArray();
    }

    public async Task<UserEntity?> UpdateAsync(
        UserEntity user, 
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE users SET email = @Email, username = @Username, display_name = @DisplayName,
                first_name = @FirstName, last_name = @LastName, auth_provider = @AuthProvider,
                external_subject = @ExternalSubject, is_active = @IsActive, updated_at_utc = @UpdatedAtUtc
            WHERE user_id = @UserId;
            """;
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var affected = await connection.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));
        return affected == 0 ? null : await GetByIdAsync(user.UserId, cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM users WHERE user_id = @UserId;";
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var affected = await connection.ExecuteAsync(new CommandDefinition(
            sql, new { UserId = userId.ToString() }, cancellationToken: cancellationToken));
        return affected > 0;
    }
}
