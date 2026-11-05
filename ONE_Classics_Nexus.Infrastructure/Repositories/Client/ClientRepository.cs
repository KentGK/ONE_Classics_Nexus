using ONE_Classics_Nexus.Domain.Entities;
using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Repositories.Client;

public sealed class ClientRepository : IClientRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public ClientRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<Clients> CreateAsync(
        Clients client,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);

        const string sql = """
            INSERT INTO clients
            (
                client_id,
                client_number,
                client_type_code,
                first_name,
                last_name,
                middle_name,
                display_name,
                address_line1,
                address_line2,
                city,
                state_region,
                postal_code,
                email,
                phone,
                preferred_contact_method,
                notes,
                row_version,
                created_at_utc,
                created_by_user_id,
                updated_at_utc,
                updated_by_user_id,
                organization_name,
                organization_type,
                Status_Code
            )
            VALUES
            (
                @ClientId,
                @ClientNumber,
                @ClientTypeCode,
                @FirstName,
                @LastName,
                @MiddleName,
                @DisplayName,
                @AddressLine1,
                @AddressLine2,
                @City,
                @StateRegion,
                @PostalCode,
                @Email,
                @Phone,
                @PreferredContactMethod,
                @Notes,
                @RowVersion,
                @CreatedAtUtc,
                @CreatedByUserId,
                @UpdatedAtUtc,
                @UpdatedByUserId,
                @OrganizationName,
                @OrganizationType,
                @StatusCode
            );
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(client),
            cancellationToken: cancellationToken));

        return client;
    }

    public async Task<Clients?> GetByIdAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                client_id AS ClientId,
                client_number AS ClientNumber,
                client_type_code AS ClientTypeCode,
                first_name AS FirstName,
                last_name AS LastName,
                middle_name AS MiddleName,
                display_name AS DisplayName,
                address_line1 AS AddressLine1,
                address_line2 AS AddressLine2,
                city AS City,
                state_region AS StateRegion,
                postal_code AS PostalCode,
                email AS Email,
                phone AS Phone,
                preferred_contact_method AS PreferredContactMethod,
                notes AS Notes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                created_by_user_id AS CreatedByUserId,
                updated_at_utc AS UpdatedAtUtc,
                updated_by_user_id AS UpdatedByUserId,
                organization_name AS OrganizationName,
                organization_type AS OrganizationType,
                Status_Code AS StatusCode
            FROM clients
            WHERE client_id = @ClientId
            LIMIT 1;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<Clients>(
            new CommandDefinition(
                sql,
                new { ClientId = clientId.ToString() },
                cancellationToken: cancellationToken));
    }

    /// <summary>
    /// Returns a filtered, sorted, paged page of clients for the Client List.
    /// </summary>
    public async Task<PagedResult<Clients>> ListAsync(
        ClientBrowseFilter filter,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize is < 1 or > 200 ? 10 : filter.PageSize;

        var conditions = new List<string>();

        // Partial, case-insensitive match. The column collation is
        // utf8mb4_unicode_ci, so LIKE is already case-insensitive here.
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            conditions.Add("(display_name LIKE @Search OR email LIKE @Search)");
        }

        if (!string.IsNullOrWhiteSpace(filter.ClientTypeCode))
        {
            conditions.Add("client_type_code = @ClientTypeCode");
        }

        if (!string.IsNullOrWhiteSpace(filter.StatusCode))
        {
            conditions.Add("Status_Code = @StatusCode");
        }

        var whereClause = conditions.Count == 0
            ? string.Empty
            : "WHERE " + string.Join(" AND ", conditions);

        // Resolved through a whitelist so no caller-supplied text reaches the
        // SQL text. Secondary sort on display_name keeps paging deterministic
        // when the primary key has duplicates, which Type and Status both do.
        var sortColumn = filter.SortBy switch
        {
            ClientSortBy.ClientTypeCode => "client_type_code",
            ClientSortBy.StatusCode => "Status_Code",
            _ => "display_name"
        };

        var sortDirection = filter.SortDirection == ClientSortDirection.Descending
            ? "DESC"
            : "ASC";

        var orderClause = filter.SortBy == ClientSortBy.DisplayName
            ? $"ORDER BY display_name {sortDirection}, client_id"
            : $"ORDER BY {sortColumn} {sortDirection}, display_name ASC, client_id";

        var sql = $"""
            SELECT
                client_id AS ClientId,
                client_number AS ClientNumber,
                client_type_code AS ClientTypeCode,
                first_name AS FirstName,
                last_name AS LastName,
                middle_name AS MiddleName,
                display_name AS DisplayName,
                address_line1 AS AddressLine1,
                address_line2 AS AddressLine2,
                city AS City,
                state_region AS StateRegion,
                postal_code AS PostalCode,
                email AS Email,
                phone AS Phone,
                preferred_contact_method AS PreferredContactMethod,
                notes AS Notes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                created_by_user_id AS CreatedByUserId,
                updated_at_utc AS UpdatedAtUtc,
                updated_by_user_id AS UpdatedByUserId,
                organization_name AS OrganizationName,
                organization_type AS OrganizationType,
                Status_Code AS StatusCode
            FROM clients
            {whereClause}
            {orderClause}
            LIMIT @Take OFFSET @Skip;

            SELECT COUNT(*)
            FROM clients
            {whereClause};
            """;

        var parameters = new
        {
            // Leading wildcard supports partial matches anywhere in the value,
            // as required by the Client List search.
            Search = $"%{filter.Search?.Trim()}%",
            ClientTypeCode = filter.ClientTypeCode?.Trim(),
            StatusCode = filter.StatusCode?.Trim(),
            Take = pageSize,
            Skip = (page - 1) * pageSize
        };

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        await using var reader = await connection.QueryMultipleAsync(new CommandDefinition(
            sql,
            parameters,
            cancellationToken: cancellationToken));

        var items = (await reader.ReadAsync<Clients>()).ToArray();
        var totalCount = await reader.ReadSingleAsync<long>();

        return new PagedResult<Clients>(items, page, pageSize, totalCount);
    }

    public async Task<Clients?> UpdateAsync(
        Clients client,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(client);

        const string sql = """
            UPDATE clients
            SET
                client_number = @ClientNumber,
                client_type_code = @ClientTypeCode,
                first_name = @FirstName,
                last_name = @LastName,
                middle_name = @MiddleName,
                display_name = @DisplayName,
                address_line1 = @AddressLine1,
                address_line2 = @AddressLine2,
                city = @City,
                state_region = @StateRegion,
                postal_code = @PostalCode,
                email = @Email,
                phone = @Phone,
                preferred_contact_method = @PreferredContactMethod,
                notes = @Notes,
                organization_name = @OrganizationName,
                organization_type = @OrganizationType,
                Status_Code = @StatusCode,
                row_version = row_version + 1,
                updated_at_utc = @UpdatedAtUtc,
                updated_by_user_id = @UpdatedByUserId
            WHERE client_id = @ClientId
              AND row_version = @RowVersion;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(client),
            cancellationToken: cancellationToken));

        if (affectedRows == 0)
        {
            return null;
        }

        return await GetByIdAsync(client.ClientId, cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM clients
            WHERE client_id = @ClientId;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(
            sql,
            new { ClientId = clientId.ToString() },
            cancellationToken: cancellationToken));

        return affectedRows > 0;
    }

    public async Task<ClientContact> CreateContactAsync(
        ClientContact contact,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(contact);

        const string sql = """
            INSERT INTO client_contacts
            (
                client_contact_id,
                client_id,
                contact_type,
                first_name,
                last_name,
                contact_role,
                job_title,
                email,
                phone,
                is_primary,
                is_active,
                notes,
                row_version,
                created_at_utc,
                updated_at_utc
            )
            VALUES
            (
                @ClientContactId,
                @ClientId,
                @ContactType,
                @FirstName,
                @LastName,
                @ContactRole,
                @JobTitle,
                @Email,
                @Phone,
                @IsPrimary,
                @IsActive,
                @Notes,
                @RowVersion,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(contact),
            cancellationToken: cancellationToken));

        return contact;
    }

    public async Task<IReadOnlyList<ClientContact>> GetContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                client_contact_id AS ClientContactId,
                client_id AS ClientId,
                contact_type AS ContactType,
                first_name AS FirstName,
                last_name AS LastName,
                contact_role AS ContactRole,
                job_title AS JobTitle,
                email AS Email,
                phone AS Phone,
                is_primary AS IsPrimary,
                is_active AS IsActive,
                notes AS Notes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM client_contacts
            WHERE client_id = @ClientId
            ORDER BY is_primary DESC, created_at_utc;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        var contacts = await connection.QueryAsync<ClientContact>(
            new CommandDefinition(
                sql,
                new { ClientId = clientId.ToString() },
                cancellationToken: cancellationToken));

        return contacts.ToArray();
    }

    public async Task<ClientContact?> GetContactByIdAsync(
        Guid clientContactId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                client_contact_id AS ClientContactId,
                client_id AS ClientId,
                contact_type AS ContactType,
                first_name AS FirstName,
                last_name AS LastName,
                job_title AS JobTitle,
                email AS Email,
                phone AS Phone,
                is_primary AS IsPrimary,
                is_active AS IsActive,
                notes AS Notes,
                row_version AS RowVersion,
                created_at_utc AS CreatedAtUtc,
                updated_at_utc AS UpdatedAtUtc
            FROM client_contacts
            WHERE client_contact_id = @ClientContactId
            LIMIT 1;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<ClientContact>(
            new CommandDefinition(
                sql,
                new { ClientContactId = clientContactId.ToString() },
                cancellationToken: cancellationToken));
    }

    public async Task<ClientContact?> UpdateContactAsync(
        ClientContact contact,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(contact);

        const string sql = """
            UPDATE client_contacts
            SET
                client_id = @ClientId,
                contact_type = @ContactType,
                first_name = @FirstName,
                last_name = @LastName,
                job_title = @JobTitle,
                email = @Email,
                phone = @Phone,
                is_primary = @IsPrimary,
                is_active = @IsActive,
                notes = @Notes,
                row_version = row_version + 1,
                updated_at_utc = @UpdatedAtUtc
            WHERE client_contact_id = @ClientContactId
              AND row_version = @RowVersion;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(
            sql,
            ToParameters(contact),
            cancellationToken: cancellationToken));

        if (affectedRows == 0)
        {
            return null;
        }

        return await GetContactByIdAsync(
            contact.ClientContactId,
            cancellationToken);
    }

    public async Task<bool> DeleteContactAsync(
        Guid clientContactId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM client_contacts
            WHERE client_contact_id = @ClientContactId;
            """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(
            sql,
            new { ClientContactId = clientContactId.ToString() },
            cancellationToken: cancellationToken));

        return affectedRows > 0;
    }

    private static object ToParameters(Clients client) =>
        new
        {
            ClientId = client.ClientId.ToString(),
            client.ClientNumber,
            client.ClientTypeCode,
            client.FirstName,
            client.LastName,
            client.MiddleName,
            client.DisplayName,
            client.AddressLine1,
            client.AddressLine2,
            client.City,
            client.StateRegion,
            client.PostalCode,
            client.Email,
            client.Phone,
            client.PreferredContactMethod,
            client.Notes,
            client.RowVersion,
            client.CreatedAtUtc,
            CreatedByUserId = client.CreatedByUserId.ToString(),
            client.UpdatedAtUtc,
            UpdatedByUserId = client.UpdatedByUserId.ToString(),
            client.OrganizationName,
            client.OrganizationType,
            client.StatusCode
        };

    private static object ToParameters(ClientContact contact) =>
        new
        {
            ClientContactId = contact.ClientContactId.ToString(),
            ClientId = contact.ClientId.ToString(),
            contact.ContactType,
            contact.FirstName,
            contact.LastName,
            contact.JobTitle,
            contact.Email,
            contact.Phone,
            contact.IsPrimary,
            contact.IsActive,
            contact.Notes,
            contact.RowVersion,
            contact.CreatedAtUtc,
            contact.UpdatedAtUtc
        };
}