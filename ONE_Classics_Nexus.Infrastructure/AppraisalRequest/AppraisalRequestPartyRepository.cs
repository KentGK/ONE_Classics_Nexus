using Dapper;
using ONE_Classics_Nexus.Application.AppraisalRequests;

namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;

public sealed class AppraisalRequestPartyRepository(IAppraisalRequestConnectionFactory factory)
    : IAppraisalRequestPartyRepository, IPartyReferenceDataRepository
{
    public async Task<PartyIdentificationResponse?> GetAsync(Guid appraisalRequestId, CancellationToken cancellationToken)
    {
        const string requestSql = """
            SELECT
                ar.appraisal_request_id AS AppraisalRequestId,
                ar.client_id AS ClientId,
                ar.primary_client_contact_id AS PrimaryClientContactId,
                ar.intended_use_id AS IntendedUseId,
                ar.converted_appraisal_id AS ConvertedAppraisalId,
                ar.request_status AS RequestStatus,
                ar.party_identification_reviewed_at_utc AS ReviewedAtUtc,
                ar.party_identification_reviewed_by_user_id AS ReviewedByUserId,
                ar.row_version AS AppraisalRequestRowVersion,
                c.client_number AS ClientNumber,
                c.client_type_code AS ClientTypeCode,
                c.display_name AS ClientDisplayName,
                c.organization_name AS ClientOrganizationName,
                c.email AS ClientEmail,
                c.phone AS ClientPhone,
                c.address_line1 AS ClientAddressLine1,
                c.address_line2 AS ClientAddressLine2,
                c.city AS ClientCity,
                c.state_region AS ClientStateRegion,
                c.postal_code AS ClientPostalCode,
                -- clients schema v2.0 has no country column; the DTO field is
                -- left null rather than reintroducing a column to the lock.
                NULL AS ClientCountryCode,
                c.row_version AS ClientRowVersion,
                cc.client_contact_id AS ContactId,
                cc.contact_name AS ContactName,
                cc.title AS ContactTitle,
                cc.contact_role AS ContactRole,
                cc.email AS ContactEmail,
                cc.phone AS ContactPhone,
                cc.address_line1 AS ContactAddressLine1,
                cc.address_line2 AS ContactAddressLine2,
                cc.city AS ContactCity,
                cc.state_region AS ContactStateRegion,
                cc.postal_code AS ContactPostalCode,
                cc.country_code AS ContactCountryCode,
                cc.row_version AS ContactRowVersion,
                iu.intended_use_id AS UseId,
                iu.use_category_id AS UseCategoryId,
                iu.intended_use_code AS IntendedUseCode,
                iu.intended_use_name AS IntendedUseName,
                iu.description AS IntendedUseDescription,
                uc.use_category_name AS UseCategoryName,
                iu.requires_custom_narrative AS RequiresCustomNarrative,
                ar.intended_use_description_snapshot AS CustomNarrative,
                iu.row_version AS IntendedUseRowVersion
            FROM appraisal_requests ar
            LEFT JOIN clients c ON c.client_id = ar.client_id
            LEFT JOIN client_contacts cc ON cc.client_contact_id = ar.primary_client_contact_id
            LEFT JOIN intended_uses iu ON iu.intended_use_id = ar.intended_use_id
            LEFT JOIN use_categories uc ON uc.use_category_id = iu.use_category_id
            WHERE ar.appraisal_request_id = @AppraisalRequestId;
            """;

        const string intendedUsersSql = """
            SELECT
                intended_user_id AS IntendedUserId,
                user_type_id AS UserTypeId,
                is_client AS IsClient,
                display_name AS DisplayName,
                organization_name AS OrganizationName,
                relationship_to_client AS RelationshipToClient,
                address_line1 AS AddressLine1,
                address_line2 AS AddressLine2,
                city AS City,
                state_region AS StateRegion,
                postal_code AS PostalCode,
                country_code AS CountryCode,
                email AS Email,
                phone AS Phone,
                use_restrictions AS UseRestrictions,
                sort_order AS SortOrder,
                is_active AS IsActive,
                row_version AS RowVersion
            FROM appraisal_request_intended_users
            WHERE appraisal_request_id = @AppraisalRequestId
            ORDER BY sort_order, display_name;
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);

        var request = await connection.QuerySingleOrDefaultAsync<RequestRow>(
            new CommandDefinition(
                requestSql,
                new { AppraisalRequestId = appraisalRequestId },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        if (request is null)
        {
            return null;
        }

        var users = (await connection.QueryAsync<IntendedUserRow>(
            new CommandDefinition(
                intendedUsersSql,
                new { AppraisalRequestId = appraisalRequestId },
                cancellationToken: cancellationToken)).ConfigureAwait(false))
            .Select(MapUser)
            .ToList();

        return new PartyIdentificationResponse(
            request.AppraisalRequestId,
            request.ClientId,
            MapClient(request),
            request.PrimaryClientContactId,
            MapContact(request),
            request.IntendedUseId,
            MapIntendedUse(request),
            users,
            request.ReviewedAtUtc is null ? null : new DateTimeOffset(DateTime.SpecifyKind(request.ReviewedAtUtc.Value, DateTimeKind.Utc)),
            request.ReviewedByUserId,
            request.ConvertedAppraisalId,
            request.RequestStatus,
            Convert.ToUInt64(request.AppraisalRequestRowVersion),
            new ValidationSummary(true, 0, 0, Array.Empty<ValidationIssue>()));
    }

    public async Task<bool> SetClientAsync(Guid appraisalRequestId, Guid clientId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE appraisal_requests
            SET client_id = @ClientId,
                primary_client_contact_id = NULL,
                client_snapshot_json = NULL,
                client_contact_snapshot_json = NULL,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6)
            WHERE appraisal_request_id = @AppraisalRequestId
              AND row_version = @ExpectedRowVersion;
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    AppraisalRequestId = appraisalRequestId,
                    ClientId = clientId,
                    ExpectedRowVersion = expectedRowVersion
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return changed == 1;
    }

    public async Task<bool> SetPrimaryClientContactAsync(Guid appraisalRequestId, Guid? clientContactId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE appraisal_requests
            SET primary_client_contact_id = @ClientContactId,
                client_contact_snapshot_json = NULL,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6)
            WHERE appraisal_request_id = @AppraisalRequestId
              AND row_version = @ExpectedRowVersion;
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    AppraisalRequestId = appraisalRequestId,
                    ClientContactId = clientContactId,
                    ExpectedRowVersion = expectedRowVersion
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return changed == 1;
    }

    public async Task<bool> SetProposedIntendedUseAsync(Guid appraisalRequestId, Guid intendedUseId, string? customNarrative, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE appraisal_requests
            SET intended_use_id = @IntendedUseId,
                intended_use_description_snapshot = @CustomNarrative,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6)
            WHERE appraisal_request_id = @AppraisalRequestId
              AND row_version = @ExpectedRowVersion;
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    AppraisalRequestId = appraisalRequestId,
                    IntendedUseId = intendedUseId,
                    CustomNarrative = customNarrative,
                    ExpectedRowVersion = expectedRowVersion
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return changed == 1;
    }

    public async Task<Guid> AddProposedIntendedUserAsync(Guid appraisalRequestId, CreateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
    {
        var intendedUserId = Guid.NewGuid();

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var tx = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var touched = await TouchRequestAsync(connection, tx, appraisalRequestId, request.ExpectedRowVersion, cancellationToken).ConfigureAwait(false);
        if (!touched)
        {
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw new AppraisalRequestApplicationException("CONCURRENCY-001", "The record was changed by another user.", 409);
        }

        const string sql = """
            INSERT INTO appraisal_request_intended_users
            (
                intended_user_id,
                appraisal_request_id,
                user_type_id,
                source_client_id,
                source_client_contact_id,
                is_client,
                display_name,
                organization_name,
                relationship_to_client,
                address_line1,
                address_line2,
                city,
                state_region,
                postal_code,
                country_code,
                email,
                phone,
                use_restrictions,
                sort_order,
                is_active,
                row_version,
                created_at_utc,
                created_by_user_id,
                updated_at_utc,
                updated_by_user_id
            )
            VALUES
            (
                @IntendedUserId,
                @AppraisalRequestId,
                @UserTypeId,
                @SourceClientId,
                @SourceClientContactId,
                @IsClient,
                @DisplayName,
                @OrganizationName,
                @RelationshipToClient,
                @AddressLine1,
                @AddressLine2,
                @City,
                @StateRegion,
                @PostalCode,
                @CountryCode,
                @Email,
                @Phone,
                @UseRestrictions,
                @SortOrder,
                TRUE,
                1,
                UTC_TIMESTAMP(6),
                @ActorId,
                UTC_TIMESTAMP(6),
                @ActorId
            );
            """;

        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    IntendedUserId = intendedUserId,
                    AppraisalRequestId = appraisalRequestId,
                    request.UserTypeId,
                    request.SourceClientId,
                    request.SourceClientContactId,
                    request.IsClient,
                    request.DisplayName,
                    request.OrganizationName,
                    request.RelationshipToClient,
                    request.Address?.AddressLine1,
                    request.Address?.AddressLine2,
                    request.Address?.City,
                    request.Address?.StateRegion,
                    request.Address?.PostalCode,
                    request.Address?.CountryCode,
                    request.Email,
                    request.Phone,
                    request.UseRestrictions,
                    request.SortOrder,
                    ActorId = actorId
                },
                transaction: tx,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
        return intendedUserId;
    }

    public async Task<bool> UpdateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, UpdateIntendedUserRequest request, Guid actorId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var tx = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var touched = await TouchRequestAsync(connection, tx, appraisalRequestId, request.ExpectedRowVersion, cancellationToken).ConfigureAwait(false);
        if (!touched)
        {
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
            return false;
        }

        const string sql = """
            UPDATE appraisal_request_intended_users
            SET user_type_id = @UserTypeId,
                source_client_id = @SourceClientId,
                source_client_contact_id = @SourceClientContactId,
                is_client = @IsClient,
                display_name = @DisplayName,
                organization_name = @OrganizationName,
                relationship_to_client = @RelationshipToClient,
                address_line1 = @AddressLine1,
                address_line2 = @AddressLine2,
                city = @City,
                state_region = @StateRegion,
                postal_code = @PostalCode,
                country_code = @CountryCode,
                email = @Email,
                phone = @Phone,
                use_restrictions = @UseRestrictions,
                sort_order = @SortOrder,
                is_active = @IsActive,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6),
                updated_by_user_id = @ActorId
            WHERE intended_user_id = @IntendedUserId
              AND appraisal_request_id = @AppraisalRequestId;
            """;

        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    IntendedUserId = intendedUserId,
                    AppraisalRequestId = appraisalRequestId,
                    request.UserTypeId,
                    request.SourceClientId,
                    request.SourceClientContactId,
                    request.IsClient,
                    request.DisplayName,
                    request.OrganizationName,
                    request.RelationshipToClient,
                    request.Address?.AddressLine1,
                    request.Address?.AddressLine2,
                    request.Address?.City,
                    request.Address?.StateRegion,
                    request.Address?.PostalCode,
                    request.Address?.CountryCode,
                    request.Email,
                    request.Phone,
                    request.UseRestrictions,
                    request.SortOrder,
                    request.IsActive,
                    ActorId = actorId
                },
                transaction: tx,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        if (changed != 1)
        {
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
            return false;
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> DeactivateProposedIntendedUserAsync(Guid appraisalRequestId, Guid intendedUserId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var tx = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var touched = await TouchRequestAsync(connection, tx, appraisalRequestId, expectedRowVersion, cancellationToken).ConfigureAwait(false);
        if (!touched)
        {
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
            return false;
        }

        const string sql = """
            UPDATE appraisal_request_intended_users
            SET is_active = FALSE,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6),
                updated_by_user_id = @ActorId
            WHERE intended_user_id = @IntendedUserId
              AND appraisal_request_id = @AppraisalRequestId;
            """;

        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    IntendedUserId = intendedUserId,
                    AppraisalRequestId = appraisalRequestId,
                    ActorId = actorId
                },
                transaction: tx,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        if (changed != 1)
        {
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
            return false;
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> SaveClientExclusionExplanationAsync(Guid appraisalRequestId, string explanation, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE appraisal_requests
            SET internal_notes = @Explanation,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6)
            WHERE appraisal_request_id = @AppraisalRequestId
              AND row_version = @ExpectedRowVersion;
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);

        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    AppraisalRequestId = appraisalRequestId,
                    Explanation = explanation,
                    ExpectedRowVersion = expectedRowVersion
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return changed == 1;
    }

    public async Task<bool> MarkConvertedAsync(Guid appraisalRequestId, ulong expectedRowVersion, Guid actorId, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE appraisal_requests
            SET request_status = 'Converted',
                converted_at_utc = UTC_TIMESTAMP(6),
                party_identification_reviewed_at_utc = UTC_TIMESTAMP(6),
                party_identification_reviewed_by_user_id = @ActorId,
                row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6)
            WHERE appraisal_request_id = @AppraisalRequestId
              AND row_version = @ExpectedRowVersion
              AND converted_appraisal_id IS NULL;
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);

        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    AppraisalRequestId = appraisalRequestId,
                    ActorId = actorId,
                    ExpectedRowVersion = expectedRowVersion
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return changed == 1;
    }

    public async Task<bool> ClientIsActiveAsync(Guid clientId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM clients WHERE client_id = @ClientId AND IFNULL(is_active, 1) = 1;";

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new { ClientId = clientId },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return count == 1;
    }

    public async Task<bool> ContactBelongsToClientAsync(Guid clientId, Guid clientContactId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM client_contacts WHERE client_contact_id = @ClientContactId AND client_id = @ClientId;";

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new
                {
                    ClientContactId = clientContactId,
                    ClientId = clientId
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return count == 1;
    }

    public async Task<bool> ContactIsActiveAsync(Guid clientContactId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM client_contacts WHERE client_contact_id = @ClientContactId AND is_active = 1;";

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new { ClientContactId = clientContactId },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return count == 1;
    }

    public async Task<bool> IntendedUseIsSelectableAsync(Guid intendedUseId, DateTime utcNow, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT COUNT(1)
            FROM intended_uses
            WHERE intended_use_id = @IntendedUseId
              AND is_active = 1
              AND effective_from_utc <= @AsOfUtc
              AND (effective_to_utc IS NULL OR effective_to_utc > @AsOfUtc);
            """;

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new
                {
                    IntendedUseId = intendedUseId,
                    AsOfUtc = utcNow
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return count == 1;
    }

    public async Task<bool> IntendedUserTypeIsActiveAsync(Guid userTypeId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM intended_user_types WHERE user_type_id = @UserTypeId AND is_active = 1;";

        await using var connection = await factory.OpenAsync(cancellationToken).ConfigureAwait(false);
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new { UserTypeId = userTypeId },
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return count == 1;
    }

    private static ProposedIntendedUserSnapshot MapUser(IntendedUserRow x)
    {
        return new ProposedIntendedUserSnapshot(
            x.IntendedUserId,
            x.UserTypeId,
            x.IsClient,
            x.DisplayName,
            x.OrganizationName,
            x.RelationshipToClient,
            new PostalAddress(x.AddressLine1, x.AddressLine2, x.City, x.StateRegion, x.PostalCode, x.CountryCode),
            x.Email,
            x.Phone,
            x.UseRestrictions,
            x.SortOrder,
            x.IsActive,
            Convert.ToUInt64(x.RowVersion));
    }

    private static ClientSnapshot? MapClient(RequestRow x)
    {
        if (x.ClientId == Guid.Empty || string.IsNullOrWhiteSpace(x.ClientNumber))
        {
            return null;
        }

        return new ClientSnapshot(
            x.ClientId,
            x.ClientNumber,
            x.ClientTypeCode ?? string.Empty,
            x.ClientDisplayName ?? string.Empty,
            x.ClientOrganizationName,
            x.ClientEmail,
            x.ClientPhone,
            new PostalAddress(x.ClientAddressLine1, x.ClientAddressLine2, x.ClientCity, x.ClientStateRegion, x.ClientPostalCode, x.ClientCountryCode),
            Convert.ToUInt64(x.ClientRowVersion));
    }

    private static ClientContactSnapshot? MapContact(RequestRow x)
    {
        if (!x.PrimaryClientContactId.HasValue || x.ContactId is null || x.ContactId == Guid.Empty)
        {
            return null;
        }

        return new ClientContactSnapshot(
            x.ContactId.Value,
            x.ContactName ?? string.Empty,
            x.ContactTitle,
            x.ContactRole,
            x.ContactEmail,
            x.ContactPhone,
            new PostalAddress(x.ContactAddressLine1, x.ContactAddressLine2, x.ContactCity, x.ContactStateRegion, x.ContactPostalCode, x.ContactCountryCode),
            Convert.ToUInt64(x.ContactRowVersion));
    }

    private static IntendedUseSnapshot? MapIntendedUse(RequestRow x)
    {
        if (!x.IntendedUseId.HasValue || x.UseId is null || x.UseId == Guid.Empty)
        {
            return null;
        }

        return new IntendedUseSnapshot(
            x.UseId.Value,
            x.UseCategoryId ?? Guid.Empty,
            x.IntendedUseCode ?? string.Empty,
            x.IntendedUseName ?? string.Empty,
            x.IntendedUseDescription ?? string.Empty,
            x.UseCategoryName ?? string.Empty,
            x.RequiresCustomNarrative,
            x.CustomNarrative,
            Convert.ToUInt64(x.IntendedUseRowVersion));
    }

    private static async Task<bool> TouchRequestAsync(System.Data.Common.DbConnection connection, System.Data.Common.DbTransaction transaction, Guid appraisalRequestId, ulong expectedRowVersion, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE appraisal_requests
            SET row_version = row_version + 1,
                updated_at_utc = UTC_TIMESTAMP(6)
            WHERE appraisal_request_id = @AppraisalRequestId
              AND row_version = @ExpectedRowVersion;
            """;

        var changed = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    AppraisalRequestId = appraisalRequestId,
                    ExpectedRowVersion = expectedRowVersion
                },
                transaction: transaction,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        return changed == 1;
    }

    private sealed class RequestRow
    {
        public Guid AppraisalRequestId { get; set; }
        public Guid ClientId { get; set; }
        public Guid? PrimaryClientContactId { get; set; }
        public Guid? IntendedUseId { get; set; }
        public Guid? ConvertedAppraisalId { get; set; }
        public string RequestStatus { get; set; } = string.Empty;
        public DateTime? ReviewedAtUtc { get; set; }
        public Guid? ReviewedByUserId { get; set; }
        public long AppraisalRequestRowVersion { get; set; }

        public string? ClientNumber { get; set; }
        public string? ClientTypeCode { get; set; }
        public string? ClientDisplayName { get; set; }
        public string? ClientOrganizationName { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientPhone { get; set; }
        public string? ClientAddressLine1 { get; set; }
        public string? ClientAddressLine2 { get; set; }
        public string? ClientCity { get; set; }
        public string? ClientStateRegion { get; set; }
        public string? ClientPostalCode { get; set; }
        public string? ClientCountryCode { get; set; }
        public long ClientRowVersion { get; set; }

        public Guid? ContactId { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? ContactRole { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactAddressLine1 { get; set; }
        public string? ContactAddressLine2 { get; set; }
        public string? ContactCity { get; set; }
        public string? ContactStateRegion { get; set; }
        public string? ContactPostalCode { get; set; }
        public string? ContactCountryCode { get; set; }
        public long ContactRowVersion { get; set; }

        public Guid? UseId { get; set; }
        public Guid? UseCategoryId { get; set; }
        public string? IntendedUseCode { get; set; }
        public string? IntendedUseName { get; set; }
        public string? IntendedUseDescription { get; set; }
        public string? UseCategoryName { get; set; }
        public bool RequiresCustomNarrative { get; set; }
        public string? CustomNarrative { get; set; }
        public long IntendedUseRowVersion { get; set; }
    }

    private sealed class IntendedUserRow
    {
        public Guid IntendedUserId { get; set; }
        public Guid UserTypeId { get; set; }
        public bool IsClient { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? OrganizationName { get; set; }
        public string RelationshipToClient { get; set; } = string.Empty;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateRegion { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryCode { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? UseRestrictions { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public long RowVersion { get; set; }
    }
}
