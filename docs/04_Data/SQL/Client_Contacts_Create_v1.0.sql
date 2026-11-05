-- ============================================================================
-- ONE Classics Nexus
-- 4.2 CLIENT_CONTACTS - MariaDB CREATE TABLE Script
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1, section 4.2
-- Prerequisite: clients table
-- Target: MariaDB 10.5+
-- ============================================================================

CREATE TABLE IF NOT EXISTS client_contacts
(
    client_contact_id        CHAR(36)        NOT NULL,
    client_id                CHAR(36)        NOT NULL,
    contact_name             VARCHAR(200)    NOT NULL,
    title                    VARCHAR(120)    NULL,
    contact_role             VARCHAR(100)    NULL,
    email                    VARCHAR(254)    NULL,
    phone                    VARCHAR(40)     NULL,
    alternate_phone          VARCHAR(40)     NULL,
    preferred_contact_method VARCHAR(20)     NULL,
    address_line1            VARCHAR(200)    NULL,
    address_line2            VARCHAR(200)    NULL,
    city                     VARCHAR(100)    NULL,
    state_region             VARCHAR(100)    NULL,
    postal_code              VARCHAR(30)     NULL,
    country_code             CHAR(2)         NULL,
    is_primary               BOOLEAN         NOT NULL DEFAULT FALSE,
    is_active                BOOLEAN         NOT NULL DEFAULT TRUE,

    -- A NULL generated value permits multiple non-primary/inactive contacts.
    -- A non-NULL client_id is unique, enforcing at most one active primary
    -- contact for each client.
    primary_marker CHAR(36)
        GENERATED ALWAYS AS
        (
            CASE
                WHEN is_active = TRUE AND is_primary = TRUE THEN client_id
                ELSE NULL
            END
        ) STORED,

    notes                     TEXT            NULL,
    row_version               BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc            DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    created_by_user_id        CHAR(36)        NOT NULL,
    updated_at_utc            DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_by_user_id        CHAR(36)        NOT NULL,

    CONSTRAINT pk_client_contacts
        PRIMARY KEY (client_contact_id),

    CONSTRAINT uq_client_contacts_active_primary
        UNIQUE (primary_marker),

    CONSTRAINT fk_client_contacts_client
        FOREIGN KEY (client_id)
        REFERENCES clients (client_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT ck_client_contacts_contact_id_uuid
        CHECK (
            BINARY client_contact_id = BINARY LOWER(client_contact_id)
            AND BINARY client_contact_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_client_contacts_client_id_uuid
        CHECK (
            BINARY client_id = BINARY LOWER(client_id)
            AND BINARY client_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_client_contacts_name_required
        CHECK (CHAR_LENGTH(TRIM(contact_name)) > 0),

    CONSTRAINT ck_client_contacts_preferred_method
        CHECK (
            preferred_contact_method IS NULL
            OR preferred_contact_method IN
                ('EMAIL', 'PHONE', 'SMS', 'MAIL', 'OTHER')
        ),

    CONSTRAINT ck_client_contacts_preferred_method_data
        CHECK (
            preferred_contact_method IS NULL
            OR preferred_contact_method = 'OTHER'
            OR (preferred_contact_method = 'EMAIL' AND NULLIF(TRIM(email), '') IS NOT NULL)
            OR (preferred_contact_method IN ('PHONE', 'SMS')
                AND COALESCE(NULLIF(TRIM(phone), ''), NULLIF(TRIM(alternate_phone), '')) IS NOT NULL)
            OR (preferred_contact_method = 'MAIL' AND NULLIF(TRIM(address_line1), '') IS NOT NULL)
        ),

    CONSTRAINT ck_client_contacts_country_code
        CHECK (
            country_code IS NULL
            OR BINARY country_code REGEXP '^[A-Z]{2}$'
        ),

    CONSTRAINT ck_client_contacts_primary_active
        CHECK (is_primary = FALSE OR is_active = TRUE),

    CONSTRAINT ck_client_contacts_row_version
        CHECK (row_version >= 1),

    CONSTRAINT ck_client_contacts_audit_dates
        CHECK (updated_at_utc >= created_at_utc),

    INDEX idx_client_contacts_client_status
        (client_id, is_active, is_primary, contact_name),

    INDEX idx_client_contacts_email
        (email),

    INDEX idx_client_contacts_phone
        (phone)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Reusable client-owned operational contacts for appraisal requests';

-- Design notes:
-- 1. Generate client_contact_id in the application as a canonical lower-case
--    UUID. client_id must identify an existing clients row.
-- 2. Ownership is immutable after a contact has been referenced. To move a
--    contact to another client, create a new contact and deactivate the old row.
-- 3. Referenced contacts are deactivated, not hard-deleted.
-- 4. Store email in normalized form for comparison. If original presentation
--    casing must be retained, keep it in an application snapshot rather than a
--    second authoritative email field.
-- 5. The application must increment row_version atomically on every update.
-- 6. Changing which contact is primary should occur in one transaction:
--      a. clear the current active primary;
--      b. mark the selected active contact primary;
--      c. commit only after both updates succeed.
-- 7. Actor columns intentionally have no foreign keys in this baseline. Add
--    them only after confirming users.user_id type and audit-retention behavior.

-- Recommended optimistic-concurrency update pattern:
--
-- UPDATE client_contacts
-- SET contact_name             = @ContactName,
--     title                    = @Title,
--     contact_role             = @ContactRole,
--     email                    = @Email,
--     phone                    = @Phone,
--     alternate_phone          = @AlternatePhone,
--     preferred_contact_method = @PreferredContactMethod,
--     is_primary               = @IsPrimary,
--     is_active                = @IsActive,
--     updated_at_utc           = UTC_TIMESTAMP(6),
--     updated_by_user_id       = @UpdatedByUserId,
--     row_version              = row_version + 1
-- WHERE client_contact_id      = @ClientContactId
--   AND row_version            = @ExpectedRowVersion;
--
-- Treat ROW_COUNT() = 0 as either not found or a concurrency conflict.
