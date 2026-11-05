-- ============================================================================
-- ONE Classics Nexus
-- 4.7 APPRAISAL_REQUEST_INTENDED_USERS - MariaDB CREATE TABLE and Indexes
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1, section 4.7
--
-- Required parent tables:
--   appraisal_requests, intended_user_types, clients, client_contacts
--
-- Target: MariaDB 10.5+
-- ============================================================================

CREATE TABLE IF NOT EXISTS appraisal_request_intended_users
(
    intended_user_id         CHAR(36)        NOT NULL,
    appraisal_request_id     CHAR(36)        NOT NULL,
    user_type_id             CHAR(36)        NOT NULL,
    source_client_id         CHAR(36)        NULL,
    source_client_contact_id CHAR(36)        NULL,
    is_client                BOOLEAN         NOT NULL DEFAULT FALSE,

    -- Nullable generated value permits multiple non-client intended users.
    -- A non-NULL request ID is unique, enforcing no more than one intended-user
    -- row representing the engaging client for each Appraisal Request.
    client_marker CHAR(36)
        GENERATED ALWAYS AS
        (
            CASE
                WHEN is_client = TRUE THEN appraisal_request_id
                ELSE NULL
            END
        ) STORED,

    display_name             VARCHAR(200)    NOT NULL,
    organization_name        VARCHAR(200)    NULL,
    relationship_to_client   VARCHAR(200)    NOT NULL,
    address_line1            VARCHAR(200)    NULL,
    address_line2            VARCHAR(200)    NULL,
    city                     VARCHAR(100)    NULL,
    state_region             VARCHAR(100)    NULL,
    postal_code              VARCHAR(30)     NULL,
    country_code             CHAR(2)         NULL,
    email                    VARCHAR(254)    NULL,
    phone                    VARCHAR(40)     NULL,
    use_restrictions         TEXT            NULL,
    acknowledged_at_utc      DATETIME(6)     NULL,
    sort_order               INT             NOT NULL DEFAULT 100,
    is_active                BOOLEAN         NOT NULL DEFAULT TRUE,
    row_version              BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc           DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    created_by_user_id       CHAR(36)        NOT NULL,
    updated_at_utc           DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_by_user_id       CHAR(36)        NOT NULL,

    CONSTRAINT pk_appraisal_request_intended_users
        PRIMARY KEY (intended_user_id),

    CONSTRAINT uq_appraisal_request_one_client_user
        UNIQUE (client_marker),

    CONSTRAINT fk_appraisal_request_users_request
        FOREIGN KEY (appraisal_request_id)
        REFERENCES appraisal_requests (appraisal_request_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_request_users_type
        FOREIGN KEY (user_type_id)
        REFERENCES intended_user_types (user_type_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_request_users_source_client
        FOREIGN KEY (source_client_id)
        REFERENCES clients (client_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_request_users_source_contact
        FOREIGN KEY (source_client_contact_id)
        REFERENCES client_contacts (client_contact_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT ck_appraisal_request_users_id_uuid
        CHECK (
            BINARY intended_user_id = BINARY LOWER(intended_user_id)
            AND BINARY intended_user_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_appraisal_request_users_request_id_uuid
        CHECK (
            BINARY appraisal_request_id = BINARY LOWER(appraisal_request_id)
            AND BINARY appraisal_request_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_appraisal_request_users_type_id_uuid
        CHECK (
            BINARY user_type_id = BINARY LOWER(user_type_id)
            AND BINARY user_type_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_appraisal_request_users_source_client_uuid
        CHECK (
            source_client_id IS NULL
            OR (
                BINARY source_client_id = BINARY LOWER(source_client_id)
                AND BINARY source_client_id REGEXP
                    '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
            )
        ),

    CONSTRAINT ck_appraisal_request_users_source_contact_uuid
        CHECK (
            source_client_contact_id IS NULL
            OR (
                BINARY source_client_contact_id = BINARY LOWER(source_client_contact_id)
                AND BINARY source_client_contact_id REGEXP
                    '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
            )
        ),

    CONSTRAINT ck_appraisal_request_users_source_pair
        CHECK (
            source_client_contact_id IS NULL
            OR source_client_id IS NOT NULL
        ),

    CONSTRAINT ck_appraisal_request_users_boolean_values
        CHECK (is_client IN (FALSE, TRUE) AND is_active IN (FALSE, TRUE)),

    CONSTRAINT ck_appraisal_request_users_display_name
        CHECK (CHAR_LENGTH(TRIM(display_name)) > 0),

    CONSTRAINT ck_appraisal_request_users_relationship
        CHECK (CHAR_LENGTH(TRIM(relationship_to_client)) > 0),

    CONSTRAINT ck_appraisal_request_users_client_relationship
        CHECK (
            is_client = FALSE
            OR BINARY TRIM(relationship_to_client) = BINARY 'Client'
        ),

    CONSTRAINT ck_appraisal_request_users_country_code
        CHECK (
            country_code IS NULL
            OR BINARY country_code REGEXP '^[A-Z]{2}$'
        ),

    CONSTRAINT ck_appraisal_request_users_sort_order
        CHECK (sort_order > 0),

    CONSTRAINT ck_appraisal_request_users_row_version
        CHECK (row_version >= 1),

    CONSTRAINT ck_appraisal_request_users_audit_dates
        CHECK (updated_at_utc >= created_at_utc),

    -- Primary workspace/report load path.
    INDEX idx_appraisal_request_users_request_order
        (appraisal_request_id, is_active, sort_order),

    -- Supports classification filters and type-deactivation dependency checks.
    INDEX idx_appraisal_request_users_type
        (user_type_id, is_active),

    -- Supports master-to-snapshot traceability and dependency checks.
    INDEX idx_appraisal_request_users_source_client
        (source_client_id),

    INDEX idx_appraisal_request_users_source_contact
        (source_client_contact_id),

    -- Supports request-local duplicate warnings without prohibiting legitimate
    -- same-name users serving different roles.
    INDEX idx_appraisal_request_users_duplicate_review
        (appraisal_request_id, display_name, organization_name, user_type_id),

    -- Supports contact lookup and duplicate warning.
    INDEX idx_appraisal_request_users_email
        (email)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Request-specific proposed intended-user snapshots and restrictions';

-- ============================================================================
-- Cross-table rules requiring application/domain validation
-- ============================================================================
-- 1. source_client_contact_id must belong to source_client_id. Independent
--    foreign keys prevent missing rows but do not prove ownership.
-- 2. If is_client = TRUE, source_client_id should normally match the request's
--    client_id. An accepted snapshot-only exception requires explicit review.
-- 3. At least one active intended user is required by the Ready-for-Review gate.
-- 4. If the client is excluded from intended-user status, the application must
--    require at least one other intended user and the required explanation.
-- 5. An intended user is not automatically an authorized report recipient.
-- 6. Accepted or converted rows are preserved as historical snapshots. Draft
--    removal sets is_active = FALSE instead of deleting the row.
-- 7. The request aggregate service duplicate-checks normalized display_name,
--    organization_name, and user_type_id. This is intentionally not a UNIQUE
--    constraint because legitimate role distinctions may exist.
-- 8. Actor columns have no foreign keys until users.user_id datatype and audit-
--    retention behavior are reconciled.
-- 9. The request foreign key uses RESTRICT. For an allowed never-submitted Draft
--    hard delete, delete its child rows explicitly in the same transaction.

-- Recommended request workspace query:
--
-- SELECT
--     iu.*,
--     iut.user_type_code,
--     iut.user_type_name
-- FROM appraisal_request_intended_users AS iu
-- INNER JOIN intended_user_types AS iut
--     ON iut.user_type_id = iu.user_type_id
-- WHERE iu.appraisal_request_id = @AppraisalRequestId
--   AND iu.is_active = TRUE
-- ORDER BY iu.sort_order, iu.display_name;

-- Recommended optimistic-concurrency update pattern:
--
-- UPDATE appraisal_request_intended_users
-- SET user_type_id          = @UserTypeId,
--     display_name          = @DisplayName,
--     organization_name     = @OrganizationName,
--     relationship_to_client = @RelationshipToClient,
--     use_restrictions      = @UseRestrictions,
--     is_active             = @IsActive,
--     sort_order            = @SortOrder,
--     updated_at_utc        = UTC_TIMESTAMP(6),
--     updated_by_user_id    = @UpdatedByUserId,
--     row_version           = row_version + 1
-- WHERE intended_user_id = @IntendedUserId
--   AND row_version      = @ExpectedRowVersion;
--
-- Treat ROW_COUNT() = 0 as either not found or a concurrency conflict.
