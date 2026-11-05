-- ============================================================================
-- ONE Classics Nexus
-- 4.6 INTENDED_USER_TYPES - MariaDB CREATE TABLE and Indexes
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1, section 4.6
-- Target: MariaDB 10.5+
-- ============================================================================

CREATE TABLE IF NOT EXISTS intended_user_types
(
    user_type_id    CHAR(36)        NOT NULL,
    user_type_code  VARCHAR(50)     NOT NULL,
    user_type_name  VARCHAR(100)    NOT NULL,
    description     TEXT            NULL,
    sort_order      INT             NOT NULL DEFAULT 100,
    is_active       BOOLEAN         NOT NULL DEFAULT TRUE,
    row_version     BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc  DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_at_utc  DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),

    CONSTRAINT pk_intended_user_types
        PRIMARY KEY (user_type_id),

    CONSTRAINT uq_intended_user_types_code
        UNIQUE (user_type_code),

    CONSTRAINT ck_intended_user_types_id_uuid
        CHECK (
            BINARY user_type_id = BINARY LOWER(user_type_id)
            AND BINARY user_type_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_intended_user_types_code_required
        CHECK (
            CHAR_LENGTH(TRIM(user_type_code)) > 0
            AND BINARY user_type_code REGEXP '^[A-Z][A-Z0-9_]{0,49}$'
        ),

    CONSTRAINT ck_intended_user_types_name_required
        CHECK (CHAR_LENGTH(TRIM(user_type_name)) > 0),

    CONSTRAINT ck_intended_user_types_sort_order
        CHECK (sort_order > 0),

    CONSTRAINT ck_intended_user_types_row_version
        CHECK (row_version >= 1),

    CONSTRAINT ck_intended_user_types_audit_dates
        CHECK (updated_at_utc >= created_at_utc),

    -- Primary reference-data query: active types in configured display order.
    INDEX idx_intended_user_types_active_sort
        (is_active, sort_order, user_type_name),

    -- Supports administrative lookup and duplicate-name review.
    INDEX idx_intended_user_types_name
        (user_type_name)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Controlled classifications for appraisal request intended users';

-- Design notes:
-- 1. Generate user_type_id as a canonical lower-case UUID.
-- 2. user_type_code is an immutable, upper-case machine identifier. Never
--    recycle a code after it has been referenced.
-- 3. is_active controls new selection. Existing historical references remain
--    valid after a type is deactivated.
-- 4. Display-name or description changes do not rewrite accepted Appraisal
--    Request snapshots or completed Appraisal records.
-- 5. The application must increment row_version atomically on every update.
-- 6. Referenced rows are deactivated rather than hard-deleted.

-- Recommended active type query:
--
-- SELECT
--     user_type_id,
--     user_type_code,
--     user_type_name,
--     description,
--     sort_order,
--     row_version
-- FROM intended_user_types
-- WHERE is_active = TRUE
-- ORDER BY sort_order, user_type_name;

-- Recommended optimistic-concurrency update pattern:
--
-- UPDATE intended_user_types
-- SET user_type_name = @UserTypeName,
--     description    = @Description,
--     sort_order     = @SortOrder,
--     is_active      = @IsActive,
--     updated_at_utc = UTC_TIMESTAMP(6),
--     row_version    = row_version + 1
-- WHERE user_type_id = @UserTypeId
--   AND row_version  = @ExpectedRowVersion;
--
-- Treat ROW_COUNT() = 0 as either not found or a concurrency conflict.
