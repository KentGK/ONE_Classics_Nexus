-- ============================================================================
-- ONE Classics Nexus
-- 4.3 APPRAISAL_REQUESTS - MariaDB CREATE TABLE and Indexes
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1, section 4.3
-- Target: MariaDB 10.5+
--
-- Required parent tables:
--   clients, client_contacts, intended_uses, requested_value_definitions, appraisals
--
-- Audit actor IDs intentionally remain
-- unconstrained until their authoritative parent table names, identifier
-- datatypes, and retention behavior are reconciled with the Nexus schema.
-- ============================================================================

CREATE TABLE IF NOT EXISTS appraisal_requests
(
    -- Core request identity and lifecycle
    appraisal_request_id                    CHAR(36)        NOT NULL,
    request_number                          VARCHAR(30)     NOT NULL,
    status_code                             VARCHAR(30)     NOT NULL DEFAULT 'DRAFT',
    disposition_reason                      VARCHAR(1000)   NULL,

    -- Client, operational contact, and proposed use
    client_id                               CHAR(36)        NULL,
    primary_client_contact_id               CHAR(36)        NULL,
    intended_use_id                         CHAR(36)        NULL,
    intended_use_custom_narrative           TEXT            NULL,

    -- Client-requested appraisal parameters; not authoritative Appraisal facts
    requested_effective_date_utc            DATETIME(6)     NULL,
    requested_effective_date_precision      VARCHAR(20)     NULL,
    requested_effective_date_time_zone      VARCHAR(64)     NULL,
    requested_value_definition_id           CHAR(36)        NULL,
    requested_scope_summary                 TEXT            NULL,

    -- One request may convert to no more than one formal Appraisal
    converted_appraisal_id                  CHAR(36)        NULL,

    -- Point-in-time request snapshots
    client_snapshot_json                    LONGTEXT        NULL,
    client_contact_snapshot_json            LONGTEXT        NULL,
    intended_use_name_snapshot              VARCHAR(200)    NULL,
    intended_use_description_snapshot       TEXT            NULL,
    intended_use_category_snapshot          VARCHAR(100)    NULL,

    -- Party-identification review
    party_identification_reviewed_at_utc     DATETIME(6)     NULL,
    party_identification_reviewed_by_user_id CHAR(36)        NULL,

    -- Lifecycle audit
    submitted_at_utc                        DATETIME(6)     NULL,
    submitted_by_user_id                    CHAR(36)        NULL,
    accepted_at_utc                         DATETIME(6)     NULL,
    accepted_by_user_id                     CHAR(36)        NULL,
    converted_at_utc                        DATETIME(6)     NULL,
    converted_by_user_id                    CHAR(36)        NULL,
    closed_at_utc                           DATETIME(6)     NULL,
    closed_by_user_id                       CHAR(36)        NULL,

    -- Optimistic concurrency and record audit
    row_version                             BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc                          DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    created_by_user_id                      CHAR(36)        NOT NULL,
    updated_at_utc                          DATETIME(6)     NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_by_user_id                      CHAR(36)        NOT NULL,

    CONSTRAINT pk_appraisal_requests
        PRIMARY KEY (appraisal_request_id),

    CONSTRAINT uq_appraisal_requests_number
        UNIQUE (request_number),

    CONSTRAINT uq_appraisal_requests_converted_appraisal
        UNIQUE (converted_appraisal_id),

    CONSTRAINT fk_appraisal_requests_client
        FOREIGN KEY (client_id)
        REFERENCES clients (client_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_requests_primary_contact
        FOREIGN KEY (primary_client_contact_id)
        REFERENCES client_contacts (client_contact_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_requests_intended_use
        FOREIGN KEY (intended_use_id)
        REFERENCES intended_uses (intended_use_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_requests_value_definition
        FOREIGN KEY (requested_value_definition_id)
        REFERENCES requested_value_definitions (requested_value_definition_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT fk_appraisal_requests_converted_appraisal
        FOREIGN KEY (converted_appraisal_id)
        REFERENCES appraisals (appraisal_id)
        ON UPDATE RESTRICT
        ON DELETE RESTRICT,

    CONSTRAINT ck_appraisal_requests_id_uuid
        CHECK (
            BINARY appraisal_request_id = BINARY LOWER(appraisal_request_id)
            AND BINARY appraisal_request_id REGEXP
                '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
        ),

    CONSTRAINT ck_appraisal_requests_number_required
        CHECK (CHAR_LENGTH(TRIM(request_number)) > 0),

    CONSTRAINT ck_appraisal_requests_status
        CHECK (
            status_code IN
            (
                'DRAFT',
                'SUBMITTED',
                'UNDER_REVIEW',
                'ACCEPTED',
                'CONVERTED',
                'DECLINED',
                'CANCELLED',
                'WITHDRAWN'
            )
        ),

    CONSTRAINT ck_appraisal_requests_client_uuid
        CHECK (
            client_id IS NULL
            OR (
                BINARY client_id = BINARY LOWER(client_id)
                AND BINARY client_id REGEXP
                    '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
            )
        ),

    CONSTRAINT ck_appraisal_requests_contact_uuid
        CHECK (
            primary_client_contact_id IS NULL
            OR (
                BINARY primary_client_contact_id = BINARY LOWER(primary_client_contact_id)
                AND BINARY primary_client_contact_id REGEXP
                    '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
            )
        ),

    CONSTRAINT ck_appraisal_requests_intended_use_uuid
        CHECK (
            intended_use_id IS NULL
            OR (
                BINARY intended_use_id = BINARY LOWER(intended_use_id)
                AND BINARY intended_use_id REGEXP
                    '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
            )
        ),

    CONSTRAINT ck_appraisal_requests_date_precision
        CHECK (
            requested_effective_date_precision IS NULL
            OR requested_effective_date_precision IN ('DATE_ONLY', 'DATE_TIME')
        ),

    CONSTRAINT ck_appraisal_requests_requested_date
        CHECK (
            (requested_effective_date_utc IS NULL
                AND requested_effective_date_precision IS NULL
                AND requested_effective_date_time_zone IS NULL)
            OR
            (requested_effective_date_utc IS NOT NULL
                AND requested_effective_date_precision IS NOT NULL)
        ),

    CONSTRAINT ck_appraisal_requests_client_snapshot_json
        CHECK (
            client_snapshot_json IS NULL
            OR JSON_VALID(client_snapshot_json)
        ),

    CONSTRAINT ck_appraisal_requests_contact_snapshot_json
        CHECK (
            client_contact_snapshot_json IS NULL
            OR JSON_VALID(client_contact_snapshot_json)
        ),

    CONSTRAINT ck_appraisal_requests_party_review_pair
        CHECK (
            (party_identification_reviewed_at_utc IS NULL
                AND party_identification_reviewed_by_user_id IS NULL)
            OR
            (party_identification_reviewed_at_utc IS NOT NULL
                AND party_identification_reviewed_by_user_id IS NOT NULL)
        ),

    CONSTRAINT ck_appraisal_requests_conversion_pair
        CHECK (
            (converted_appraisal_id IS NULL
                AND converted_at_utc IS NULL
                AND converted_by_user_id IS NULL)
            OR
            (converted_appraisal_id IS NOT NULL
                AND converted_at_utc IS NOT NULL
                AND converted_by_user_id IS NOT NULL)
        ),

    CONSTRAINT ck_appraisal_requests_converted_status
        CHECK (
            (status_code = 'CONVERTED' AND converted_appraisal_id IS NOT NULL)
            OR
            (status_code <> 'CONVERTED' AND converted_appraisal_id IS NULL)
        ),

    CONSTRAINT ck_appraisal_requests_client_gate
        CHECK (
            status_code = 'DRAFT'
            OR client_id IS NOT NULL
        ),

    CONSTRAINT ck_appraisal_requests_intended_use_gate
        CHECK (
            status_code NOT IN ('ACCEPTED', 'CONVERTED')
            OR intended_use_id IS NOT NULL
        ),

    CONSTRAINT ck_appraisal_requests_acceptance_audit
        CHECK (
            status_code NOT IN ('ACCEPTED', 'CONVERTED')
            OR (accepted_at_utc IS NOT NULL AND accepted_by_user_id IS NOT NULL)
        ),

    CONSTRAINT ck_appraisal_requests_closed_status
        CHECK (
            status_code NOT IN ('DECLINED', 'CANCELLED', 'WITHDRAWN')
            OR (
                NULLIF(TRIM(disposition_reason), '') IS NOT NULL
                AND closed_at_utc IS NOT NULL
                AND closed_by_user_id IS NOT NULL
            )
        ),

    CONSTRAINT ck_appraisal_requests_row_version
        CHECK (row_version >= 1),

    CONSTRAINT ck_appraisal_requests_audit_dates
        CHECK (updated_at_utc >= created_at_utc),

    -- Client history and client-specific request searches
    INDEX idx_appraisal_requests_client
        (client_id, created_at_utc),

    INDEX idx_appraisal_requests_client_status
        (client_id, status_code, updated_at_utc),

    -- Contact history and request routing
    INDEX idx_appraisal_requests_primary_contact
        (primary_client_contact_id, status_code),

    -- Proposed-use analysis and qualification queues
    INDEX idx_appraisal_requests_intended_use
        (intended_use_id, status_code),

    INDEX idx_appraisal_requests_value_definition
        (requested_value_definition_id),

    -- Operational queues and recent activity
    INDEX idx_appraisal_requests_status_updated
        (status_code, updated_at_utc),

    INDEX idx_appraisal_requests_review_queue
        (status_code, party_identification_reviewed_at_utc, created_at_utc),

    -- Requested effective-date search/reporting
    INDEX idx_appraisal_requests_requested_effective_date
        (requested_effective_date_utc)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = 'Pre-appraisal intake requests and proposed client/use identification';

-- ============================================================================
-- Cross-table rules that require application/domain validation
-- ============================================================================
-- 1. primary_client_contact_id must identify an active contact owned by
--    appraisal_requests.client_id. The two independent foreign keys prevent
--    missing rows but do not prove ownership or active status.
-- 2. client_id is nullable only while the request is DRAFT. Submission must
--    require a selected client through application validation.
-- 3. intended_use_id is nullable during DRAFT intake. Acceptance and conversion
--    must require a valid proposed use and required custom narrative.
-- 4. Snapshot values become immutable when the request is accepted. Corrections
--    require the authorized amendment workflow and audit history.
-- 5. The Appraisal created during conversion must formally review proposed
--    client, intended-use, and intended-user facts; request data is not by
--    itself authoritative USPAP problem identification.
-- 6. Status transitions and lifecycle timestamp/actor pairs are aggregate
--    rules and must be enforced by the application service in one transaction.

-- Recommended optimistic-concurrency update pattern:
--
-- UPDATE appraisal_requests
-- SET status_code       = @StatusCode,
--     updated_at_utc    = UTC_TIMESTAMP(6),
--     updated_by_user_id = @UpdatedByUserId,
--     row_version       = row_version + 1
-- WHERE appraisal_request_id = @AppraisalRequestId
--   AND row_version           = @ExpectedRowVersion;
--
-- Treat ROW_COUNT() = 0 as either not found or a concurrency conflict.
