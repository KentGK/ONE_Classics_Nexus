-- ============================================================================
-- ONE Classics Nexus
-- 4.4 USE_CATEGORIES - MariaDB CREATE TABLE and Indexes
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1, section 4.4
-- Target: MariaDB 10.5+
-- ============================================================================

CREATE TABLE IF NOT EXISTS use_categories
(
    use_category_id    CHAR(36)        NOT NULL,
    category_code      VARCHAR(50)     NOT NULL,
    use_category_name  VARCHAR(100)    NOT NULL,
    description        TEXT            NULL,
    sort_order         INT             NOT NULL DEFAULT 100,
    is_active          BOOLEAN         NOT NULL DEFAULT TRUE,
    effective_from_utc DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    effective_to_utc   DATETIME(6)     NULL,
    row_version        BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc     DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_at_utc     DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),

    CONSTRAINT pk_use_categories
        PRIMARY KEY (use_category_id),

    CONSTRAINT uq_use_categories_code
        UNIQUE (category_code),

    CONSTRAINT uq_use_categories_name
        UNIQUE (use_category_name),

    CONSTRAINT ck_use_categories_id_uuid
        CHECK (
            BINARY use_category_id = BINARY LOWER(use_category_id)
            AND BINARY use_category_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_use_categories_code_required
        CHECK (
            CHAR_LENGTH(TRIM(category_code)) > 0
            AND BINARY category_code REGEXP '^[A-Z][A-Z0-9_]{0,49}$'
        ),

    CONSTRAINT ck_use_categories_name_required
        CHECK (CHAR_LENGTH(TRIM(use_category_name)) > 0),

    CONSTRAINT ck_use_categories_sort_order
        CHECK (sort_order > 0),

    CONSTRAINT ck_use_categories_effective_dates
        CHECK (
            effective_to_utc IS NULL
            OR effective_to_utc > effective_from_utc
        ),

    CONSTRAINT ck_use_categories_row_version
        CHECK (row_version >= 1),

    CONSTRAINT ck_use_categories_audit_dates
        CHECK (updated_at_utc >= created_at_utc),

    -- Primary UI/reference-data query: active categories in display order.
    INDEX idx_use_categories_active_sort
        (is_active, sort_order, use_category_name),

    -- Supports effective-at-time selection and administrative review.
    INDEX idx_use_categories_effective_range
        (is_active, effective_from_utc, effective_to_utc)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Controlled categories used to organize intended-use definitions';

-- Design notes:
-- 1. Generate use_category_id as a canonical lower-case UUID.
-- 2. category_code is an immutable, upper-case machine identifier. Do not
--    recycle a code after it has been referenced by an appraisal request.
-- 3. Full uniqueness is intentionally stronger than active-row-only uniqueness:
--    retired names and codes remain reserved to protect historical meaning.
-- 4. is_active controls new selection. Existing historical references remain
--    valid after deactivation or expiration.
-- 5. The application must increment row_version atomically on every update.
-- 6. Avoid overwriting a referenced definition's meaning. Retire it and add a
--    new version when a material semantic change is required.

-- Recommended effective active-category query:
--
-- SELECT
--     use_category_id,
--     category_code,
--     use_category_name,
--     description,
--     sort_order,
--     row_version
-- FROM use_categories
-- WHERE is_active = TRUE
--   AND effective_from_utc <= @AsOfUtc
--   AND (effective_to_utc IS NULL OR effective_to_utc > @AsOfUtc)
-- ORDER BY sort_order, use_category_name;

-- Recommended optimistic-concurrency update pattern:
--
-- UPDATE use_categories
-- SET use_category_name = @UseCategoryName,
--     description       = @Description,
--     sort_order        = @SortOrder,
--     is_active         = @IsActive,
--     effective_to_utc  = @EffectiveToUtc,
--     updated_at_utc    = UTC_TIMESTAMP(6),
--     row_version       = row_version + 1
-- WHERE use_category_id = @UseCategoryId
--   AND row_version     = @ExpectedRowVersion;
--
-- Treat ROW_COUNT() = 0 as either not found or a concurrency conflict.
