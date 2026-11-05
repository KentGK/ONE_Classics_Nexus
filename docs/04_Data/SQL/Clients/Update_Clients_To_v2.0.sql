-- ============================================================================
-- ONE Classics Nexus
-- CLIENTS - Migration to Schema v2.0 (LOCKED)
-- Version: 2.0
-- Target: MariaDB 10.5+
-- ----------------------------------------------------------------------------
-- Purpose:
--   Brings any earlier `clients` revision up to the locked v2.0 shape defined
--   in docs/04_Data/SQL/Clients/Clients_v2.0.sql.
--
-- Context:
--   Three divergent `clients` shapes existed before the v2.0 lock:
--     a) Clients_v1.1.sql       - display_name / client_type_code, no status
--     b) Clients_Create_v1.0.sql- display_name / client_type_code / status_code
--     c) Nexus_Full_Lock_v1.0   - legal_name / client_type / status
--   plus a fourth shape that only ever existed in ClientRepository.cs
--   (client_type / company_name / address_line_1 / state_or_province /
--    status / is_active).
--
--   This script is written defensively against all of them. Every statement
--   is guarded so the script is idempotent and safe to re-run.
--
-- Safety:
--   Review before running against data you care about. The DROP section at the
--   end is deliberately commented out; see the notes there.
-- ============================================================================

START TRANSACTION;

-- ----------------------------------------------------------------------------
-- Helper: conditional DDL procedure
-- MariaDB lacks ADD COLUMN IF NOT EXISTS in all supported versions, and lacks
-- CHANGE COLUMN IF EXISTS entirely, so route the DDL through a guard.
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS nexus_clients_v2_migrate;

DELIMITER //

CREATE PROCEDURE nexus_clients_v2_migrate()
BEGIN
    DECLARE schema_name_v VARCHAR(64);
    SET schema_name_v = DATABASE();

    -- ------------------------------------------------------------------
    -- 1. Column renames (shape c and the repository-only shape)
    -- ------------------------------------------------------------------

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'client_type') THEN
        ALTER TABLE clients
            CHANGE COLUMN `client_type` `client_type_code` VARCHAR(30) NOT NULL;
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'company_name') THEN
        ALTER TABLE clients
            CHANGE COLUMN `company_name` `organization_name` VARCHAR(200) NULL;
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'address_line_1') THEN
        ALTER TABLE clients
            CHANGE COLUMN `address_line_1` `address_line1` VARCHAR(200) NULL;
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'address_line_2') THEN
        ALTER TABLE clients
            CHANGE COLUMN `address_line_2` `address_line2` VARCHAR(200) NULL;
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'state_or_province') THEN
        ALTER TABLE clients
            CHANGE COLUMN `state_or_province` `state_region` VARCHAR(100) NULL;
    END IF;

    -- ------------------------------------------------------------------
    -- 2. Added columns
    -- ------------------------------------------------------------------

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'middle_name') THEN
        ALTER TABLE clients ADD COLUMN `middle_name` VARCHAR(100) NULL AFTER `last_name`;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'preferred_contact_method') THEN
        ALTER TABLE clients ADD COLUMN `preferred_contact_method` VARCHAR(20) NULL AFTER `phone`;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'organization_name') THEN
        ALTER TABLE clients ADD COLUMN `organization_name` VARCHAR(200) NULL;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'organization_type') THEN
        ALTER TABLE clients ADD COLUMN `organization_type` VARCHAR(100) NULL;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'created_by_user_id') THEN
        ALTER TABLE clients ADD COLUMN `created_by_user_id` CHAR(36) NOT NULL DEFAULT '';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'updated_by_user_id') THEN
        ALTER TABLE clients ADD COLUMN `updated_by_user_id` CHAR(36) NOT NULL DEFAULT '';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'row_version') THEN
        ALTER TABLE clients ADD COLUMN `row_version` BIGINT UNSIGNED NOT NULL DEFAULT 1;
    END IF;

    -- display_name: add nullable first so existing rows can be backfilled
    -- before the NOT NULL constraint is applied (see section 3).
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'display_name') THEN
        ALTER TABLE clients ADD COLUMN `display_name` VARCHAR(200) NULL AFTER `middle_name`;
    END IF;

    -- ------------------------------------------------------------------
    -- 3. Backfill display_name before enforcing NOT NULL
    -- ------------------------------------------------------------------
    -- Source preference: legacy legal_name (shape c), then organization_name
    -- for organizations, then first/last name, then client_number as a last
    -- resort so the NOT NULL constraint can never fail.

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'legal_name') THEN
        UPDATE clients
        SET display_name = legal_name
        WHERE (display_name IS NULL OR TRIM(display_name) = '')
          AND legal_name IS NOT NULL
          AND TRIM(legal_name) <> '';
    END IF;

    UPDATE clients
    SET display_name = organization_name
    WHERE (display_name IS NULL OR TRIM(display_name) = '')
      AND organization_name IS NOT NULL
      AND TRIM(organization_name) <> '';

    UPDATE clients
    SET display_name = TRIM(CONCAT_WS(' ', first_name, last_name))
    WHERE (display_name IS NULL OR TRIM(display_name) = '')
      AND TRIM(CONCAT_WS(' ', first_name, last_name)) <> '';

    UPDATE clients
    SET display_name = client_number
    WHERE display_name IS NULL OR TRIM(display_name) = '';

    ALTER TABLE clients
        MODIFY COLUMN `display_name` VARCHAR(200) NOT NULL;

    -- ------------------------------------------------------------------
    -- 4. Status_Code
    -- ------------------------------------------------------------------
    -- Added mixed case to match the UI vocabulary directly. Legacy sources are
    -- normalised: the uppercase status_code of shape b, the free-text status of
    -- shape c, and the boolean is_active of the repository-only shape.

    -- Note: MariaDB identifiers are case-insensitive, so a pre-existing
    -- `status_code` column from shape (b) IS this column. It is therefore
    -- normalised in place rather than copied, and only ever created when no
    -- spelling of the column exists yet.
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND column_name = 'Status_Code') THEN
        ALTER TABLE clients
            ADD COLUMN `Status_Code` VARCHAR(20) NOT NULL DEFAULT 'Active';
    ELSE
        -- Align an inherited column to the locked type, name casing and default.
        ALTER TABLE clients
            CHANGE COLUMN `Status_Code` `Status_Code`
                VARCHAR(20) NOT NULL DEFAULT 'Active';

        -- Shape (b) stored uppercase codes; fold them to the locked mixed case.
        UPDATE clients
        SET Status_Code = CONCAT(
                UPPER(SUBSTRING(TRIM(Status_Code), 1, 1)),
                LOWER(SUBSTRING(TRIM(Status_Code), 2)))
        WHERE Status_Code IS NOT NULL
          AND TRIM(Status_Code) <> '';
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'status') THEN
        UPDATE clients
        SET Status_Code = CONCAT(
                UPPER(SUBSTRING(TRIM(status), 1, 1)),
                LOWER(SUBSTRING(TRIM(status), 2)))
        WHERE status IS NOT NULL
          AND TRIM(status) <> '';
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND column_name = 'is_active') THEN
        UPDATE clients
        SET Status_Code = CASE WHEN is_active = 0 THEN 'Inactive' ELSE 'Active' END
        WHERE is_active IS NOT NULL;
    END IF;

    -- Anything unrecognised falls back to the schema default.
    UPDATE clients
    SET Status_Code = 'Active'
    WHERE Status_Code IS NULL
       OR TRIM(Status_Code) = ''
       OR Status_Code NOT IN ('Active', 'Inactive', 'Archived');

    -- ------------------------------------------------------------------
    -- 5. NOT NULL tightening on email / phone
    -- ------------------------------------------------------------------
    -- v2.0 declares both NOT NULL. Blank-fill first so the MODIFY cannot fail
    -- on pre-existing NULLs.

    UPDATE clients SET email = '' WHERE email IS NULL;
    UPDATE clients SET phone = '' WHERE phone IS NULL;

    ALTER TABLE clients
        MODIFY COLUMN `email` VARCHAR(254) NOT NULL,
        MODIFY COLUMN `phone` VARCHAR(40) NOT NULL;

    -- ------------------------------------------------------------------
    -- 6. Indexes
    -- ------------------------------------------------------------------

    IF NOT EXISTS (SELECT 1 FROM information_schema.statistics
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND index_name = 'idx_clients_status_display') THEN
        ALTER TABLE clients
            ADD INDEX `idx_clients_status_display` (`Status_Code`, `display_name`);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.statistics
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND index_name = 'idx_clients_display_name') THEN
        ALTER TABLE clients
            ADD INDEX `idx_clients_display_name` (`display_name`);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.statistics
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND index_name = 'idx_clients_email') THEN
        ALTER TABLE clients
            ADD INDEX `idx_clients_email` (`email`);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM information_schema.statistics
                   WHERE table_schema = schema_name_v
                     AND table_name = 'clients'
                     AND index_name = 'idx_clients_type') THEN
        ALTER TABLE clients
            ADD INDEX `idx_clients_type` (`client_type_code`);
    END IF;

    -- Legacy indexes that referenced now-renamed columns.
    IF EXISTS (SELECT 1 FROM information_schema.statistics
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND index_name = 'idx_clients_name') THEN
        ALTER TABLE clients DROP INDEX `idx_clients_name`;
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.statistics
               WHERE table_schema = schema_name_v
                 AND table_name = 'clients'
                 AND index_name = 'idx_clients_status') THEN
        ALTER TABLE clients DROP INDEX `idx_clients_status`;
    END IF;
END //

DELIMITER ;

CALL nexus_clients_v2_migrate();

DROP PROCEDURE IF EXISTS nexus_clients_v2_migrate;

COMMIT;

-- ============================================================================
-- Deferred column drops
-- ----------------------------------------------------------------------------
-- The superseded columns below are intentionally LEFT IN PLACE by this script.
-- Dropping them is destructive and irreversible, and the data in legal_name /
-- status / status_code / is_active / country is the only record of what the
-- backfill above derived its values from.
--
-- Verify the migration result first:
--
--   SELECT client_id, display_name, Status_Code, client_type_code
--   FROM clients ORDER BY display_name;
--
-- Once satisfied, drop them in a separate, deliberate change:
--
--   ALTER TABLE clients DROP COLUMN legal_name;
--   ALTER TABLE clients DROP COLUMN preferred_name;
--   ALTER TABLE clients DROP COLUMN status;
--   ALTER TABLE clients DROP COLUMN is_active;
--   ALTER TABLE clients DROP COLUMN country;
--   ALTER TABLE clients DROP COLUMN country_code;
--
-- `status_code` is deliberately absent from this list: MariaDB identifiers are
-- case-insensitive, so it is the same column as the live `Status_Code`.
-- ============================================================================
