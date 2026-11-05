-- ============================================================================
-- SUPERSEDED - DO NOT BUILD FROM THIS FILE
-- Replaced by: docs/04_Data/SQL/Clients/Clients_v2.0.sql (LOCKED v2.0)
-- Migration:   docs/04_Data/SQL/Clients/Update_Clients_To_v2.0.sql
-- Note: this revision's uppercase status_code vocabulary
--       ('ACTIVE'/'INACTIVE'/'ARCHIVED') was replaced in v2.0 by mixed-case
--       Status_Code values ('Active'/'Inactive'/'Archived').
-- ============================================================================

-- ============================================================================
-- ONE Classics Nexus
-- 4.1 CLIENTS - MariaDB CREATE Script
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1, section 4.1
-- Target: MariaDB 10.5+
-- ============================================================================

CREATE TABLE IF NOT EXISTS clients
(
    client_id              CHAR(36)        NOT NULL,
    client_number          VARCHAR(30)     NOT NULL,
    client_type_code       VARCHAR(30)     NOT NULL,
    display_name           VARCHAR(200)    NOT NULL,
    organization_name      VARCHAR(200)    NULL,
    address_line1          VARCHAR(200)    NULL,
    address_line2          VARCHAR(200)    NULL,
    city                   VARCHAR(100)    NULL,
    state_region           VARCHAR(100)    NULL,
    postal_code            VARCHAR(30)     NULL,
    country_code           CHAR(2)         NULL,
    status_code            VARCHAR(20)     NOT NULL DEFAULT 'ACTIVE',
    row_version            BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc         DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    created_by_user_id     CHAR(36)        NOT NULL,
    updated_at_utc         DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_by_user_id     CHAR(36)        NOT NULL,

    CONSTRAINT pk_clients
        PRIMARY KEY (client_id),

    CONSTRAINT uq_clients_client_number
        UNIQUE (client_number),

    CONSTRAINT ck_clients_client_id_uuid
        CHECK (
            BINARY client_id = BINARY LOWER(client_id)
            AND BINARY client_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_clients_client_number_required
        CHECK (CHAR_LENGTH(TRIM(client_number)) > 0),

    CONSTRAINT ck_clients_client_type_code
        CHECK (
            client_type_code IN
            ('PERSON', 'ORGANIZATION', 'ESTATE_TRUST', 'GOVERNMENT', 'OTHER')
        ),

    CONSTRAINT ck_clients_display_name_required
        CHECK (CHAR_LENGTH(TRIM(display_name)) > 0),

    CONSTRAINT ck_clients_country_code
        CHECK (
            country_code IS NULL
            OR BINARY country_code REGEXP '^[A-Z]{2}$'
        ),

    CONSTRAINT ck_clients_status_code
        CHECK (status_code IN ('ACTIVE', 'INACTIVE', 'ARCHIVED')),

    CONSTRAINT ck_clients_row_version
        CHECK (row_version >= 1),

    CONSTRAINT ck_clients_audit_dates
        CHECK (updated_at_utc >= created_at_utc),

    INDEX idx_clients_display_name (display_name),
    INDEX idx_clients_organization_name (organization_name),
    INDEX idx_clients_status_display (status_code, display_name)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Client master records for appraisal request intake and appraisal workflows';

-- Design notes:
-- 1. Generate client_id in the application as a canonical lower-case UUID.
-- 2. Generate client_number through the Nexus numbering service; do not use
--    MAX(client_number) + 1 because it is unsafe under concurrency.
-- 3. The application must increment row_version atomically on every update.
-- 4. Actor columns intentionally have no foreign keys in this baseline. Add
--    them only after confirming that users.user_id uses the same CHAR(36) type
--    and that the desired delete behavior preserves historical audit records.
-- 5. CLIENT_CONTACT is implemented separately and references clients.client_id.

-- Recommended optimistic-concurrency update pattern:
--
-- UPDATE clients
-- SET display_name       = @DisplayName,
--     organization_name  = @OrganizationName,
--     updated_at_utc     = UTC_TIMESTAMP(6),
--     updated_by_user_id = @UpdatedByUserId,
--     row_version        = row_version + 1
-- WHERE client_id        = @ClientId
--   AND row_version      = @ExpectedRowVersion;
--
-- Treat ROW_COUNT() = 0 as either not found or a concurrency conflict.
