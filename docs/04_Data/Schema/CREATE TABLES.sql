USE one_classics_nexus;

-- ============================================================
-- ACCESS / RBAC
-- ============================================================

CREATE TABLE users (
    user_id               CHAR(36)     NOT NULL,
    email                 VARCHAR(255) NOT NULL,
    display_name          VARCHAR(150) NOT NULL,
    first_name            VARCHAR(100) NULL,
    last_name             VARCHAR(100) NULL,
    is_active             TINYINT(1)   NOT NULL DEFAULT 1,
    auth_provider         VARCHAR(50)  NULL,
    external_subject      VARCHAR(255) NULL,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (user_id),
    UNIQUE KEY uq_users_email (email),
    KEY idx_users_active (is_active)
) ENGINE=InnoDB;

CREATE TABLE roles (
    role_id               CHAR(36)     NOT NULL,
    role_name             VARCHAR(100) NOT NULL,
    role_code             VARCHAR(50)  NOT NULL,
    description           VARCHAR(255) NULL,
    is_system_role        TINYINT(1)   NOT NULL DEFAULT 0,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (role_id),
    UNIQUE KEY uq_roles_code (role_code),
    UNIQUE KEY uq_roles_name (role_name)
) ENGINE=InnoDB;

CREATE TABLE user_roles (
    user_role_id          CHAR(36)     NOT NULL,
    user_id               CHAR(36)     NOT NULL,
    role_id               CHAR(36)     NOT NULL,
    assigned_at_utc       DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    assigned_by_user_id   CHAR(36)     NULL,
    PRIMARY KEY (user_role_id),
    UNIQUE KEY uq_user_roles (user_id, role_id),
    KEY idx_user_roles_role (role_id),
    CONSTRAINT fk_user_roles_user FOREIGN KEY (user_id) REFERENCES users(user_id),
    CONSTRAINT fk_user_roles_role FOREIGN KEY (role_id) REFERENCES roles(role_id),
    CONSTRAINT fk_user_roles_assigned_by FOREIGN KEY (assigned_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB;

-- ============================================================
-- CLIENT INTAKE / REQUESTS
-- ============================================================

CREATE TABLE clients (
    client_id             CHAR(36)     NOT NULL,
    client_type           VARCHAR(30)  NOT NULL DEFAULT 'Individual',
    legal_name            VARCHAR(200) NOT NULL,
    preferred_name        VARCHAR(200) NULL,
    status                VARCHAR(30)  NOT NULL DEFAULT 'Active',
    notes                 TEXT         NULL,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (client_id),
    KEY idx_clients_status (status),
    KEY idx_clients_name (legal_name)
) ENGINE=InnoDB;

CREATE TABLE client_contacts (
    client_contact_id     CHAR(36)     NOT NULL,
    client_id             CHAR(36)     NOT NULL,
    contact_name          VARCHAR(200) NOT NULL,
    email                 VARCHAR(255) NULL,
    phone                 VARCHAR(50)  NULL,
    contact_role          VARCHAR(50)  NULL,
    is_primary            TINYINT(1)   NOT NULL DEFAULT 0,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (client_contact_id),
    KEY idx_client_contacts_client (client_id),
    KEY idx_client_contacts_primary (client_id, is_primary),
    CONSTRAINT fk_client_contacts_client FOREIGN KEY (client_id) REFERENCES clients(client_id)
) ENGINE=InnoDB;

CREATE TABLE appraisal_requests (
    appraisal_request_id  CHAR(36)     NOT NULL,
    client_id             CHAR(36)     NOT NULL,
    requested_by_contact_id CHAR(36)   NULL,
    request_status        VARCHAR(30)  NOT NULL DEFAULT 'Submitted',
    request_channel       VARCHAR(30)  NULL,
    requested_service     VARCHAR(50)  NOT NULL DEFAULT 'Appraisal',
    requested_on_utc      DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    target_due_date_utc   DATETIME(6)  NULL,
    intake_notes          TEXT         NULL,
    created_appraisal_id  CHAR(36)     NULL,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (appraisal_request_id),
    KEY idx_appraisal_requests_client (client_id),
    KEY idx_appraisal_requests_status (request_status),
    CONSTRAINT fk_appraisal_requests_client FOREIGN KEY (client_id) REFERENCES clients(client_id),
    CONSTRAINT fk_appraisal_requests_contact FOREIGN KEY (requested_by_contact_id) REFERENCES client_contacts(client_contact_id)
) ENGINE=InnoDB;

-- ============================================================
-- SUBJECT VEHICLES / APPRAISAL WORKFLOW
-- ============================================================

CREATE TABLE vehicles (
    vehicle_id            CHAR(36)     NOT NULL,
    vin                   VARCHAR(50)  NOT NULL,
    year                  INT          NOT NULL,
    make                  VARCHAR(100) NOT NULL,
    model                 VARCHAR(100) NOT NULL,
    trim                  VARCHAR(100) NULL,
    series                VARCHAR(100) NULL,
    body_style            VARCHAR(100) NULL,
    engine                VARCHAR(150) NULL,
    transmission          VARCHAR(150) NULL,
    drivetrain            VARCHAR(100) NULL,
    mileage               INT          NULL,
    exterior_color        VARCHAR(100) NULL,
    interior_color        VARCHAR(100) NULL,
    title_status          VARCHAR(50)  NULL,
    ownership_notes       TEXT         NULL,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (vehicle_id),
    UNIQUE KEY uq_vehicles_vin (vin),
    KEY idx_vehicles_lookup (year, make, model),
    KEY idx_vehicles_make_model (make, model)
) ENGINE=InnoDB;

CREATE TABLE appraisals (
    appraisal_id          CHAR(36)     NOT NULL,
    vehicle_id            CHAR(36)     NOT NULL,
    client_id             CHAR(36)     NULL,
    appraisal_request_id  CHAR(36)     NULL,
    appraisal_number      VARCHAR(50)  NULL,
    appraisal_type        VARCHAR(50)  NOT NULL DEFAULT 'MarketValue',
    status                VARCHAR(30)  NOT NULL DEFAULT 'Draft',
    intake_status         VARCHAR(30)  NULL,
    inspection_status     VARCHAR(30)  NULL,
    valuation_status      VARCHAR(30)  NULL,
    report_status         VARCHAR(30)  NULL,
    effective_date_utc    DATETIME(6)  NULL,
    assigned_appraiser_id CHAR(36)     NULL,
    assigned_reviewer_id  CHAR(36)     NULL,
    row_version           BIGINT       NOT NULL DEFAULT 0,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (appraisal_id),
    UNIQUE KEY uq_appraisals_number (appraisal_number),
    KEY idx_appraisals_vehicle (vehicle_id),
    KEY idx_appraisals_client (client_id),
    KEY idx_appraisals_status (status),
    CONSTRAINT fk_appraisals_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id),
    CONSTRAINT fk_appraisals_client FOREIGN KEY (client_id) REFERENCES clients(client_id),
    CONSTRAINT fk_appraisals_request FOREIGN KEY (appraisal_request_id) REFERENCES appraisal_requests(appraisal_request_id),
    CONSTRAINT fk_appraisals_assigned_appraiser FOREIGN KEY (assigned_appraiser_id) REFERENCES users(user_id),
    CONSTRAINT fk_appraisals_assigned_reviewer FOREIGN KEY (assigned_reviewer_id) REFERENCES users(user_id)
) ENGINE=InnoDB;

ALTER TABLE appraisal_requests
ADD CONSTRAINT fk_appraisal_requests_created_appraisal FOREIGN KEY (created_appraisal_id) REFERENCES appraisals(appraisal_id);

CREATE TABLE appraisal_assignments (
    appraisal_assignment_id CHAR(36)   NOT NULL,
    appraisal_id          CHAR(36)     NOT NULL,
    user_id               CHAR(36)     NOT NULL,
    assignment_role       VARCHAR(30)  NOT NULL,
    assignment_status     VARCHAR(30)  NOT NULL DEFAULT 'Active',
    assigned_at_utc       DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    released_at_utc       DATETIME(6)  NULL,
    notes                 TEXT         NULL,
    PRIMARY KEY (appraisal_assignment_id),
    KEY idx_appraisal_assignments_appraisal (appraisal_id),
    KEY idx_appraisal_assignments_user (user_id),
    KEY idx_appraisal_assignments_active (appraisal_id, assignment_status),
    CONSTRAINT fk_appraisal_assignments_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_appraisal_assignments_user FOREIGN KEY (user_id) REFERENCES users(user_id)
) ENGINE=InnoDB;

-- ============================================================
-- VERSIONED INSPECTION TEMPLATE MODEL
-- ============================================================

CREATE TABLE inspection_templates (
    inspection_template_id CHAR(36)    NOT NULL,
    template_code          VARCHAR(50) NOT NULL,
    template_name          VARCHAR(150) NOT NULL,
    template_family        VARCHAR(30) NOT NULL,
    version_label          VARCHAR(30) NOT NULL,
    status                 VARCHAR(30) NOT NULL DEFAULT 'Draft',
    description            TEXT        NULL,
    effective_from_utc     DATETIME(6) NULL,
    effective_to_utc       DATETIME(6) NULL,
    total_system_count     INT         NOT NULL DEFAULT 12,
    created_by_user_id     CHAR(36)    NULL,
    created_at_utc         DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc         DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_template_id),
    UNIQUE KEY uq_inspection_templates_version (template_code, version_label),
    KEY idx_inspection_templates_family (template_family, status),
    CONSTRAINT fk_inspection_templates_created_by FOREIGN KEY (created_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB;

CREATE TABLE inspection_template_sections (
    inspection_template_section_id CHAR(36) NOT NULL,
    inspection_template_id CHAR(36) NOT NULL,
    system_number         INT          NOT NULL,
    system_key            VARCHAR(50)  NOT NULL,
    display_name          VARCHAR(150) NOT NULL,
    description           TEXT         NULL,
    weight                DECIMAL(9,6) NOT NULL,
    sort_order            INT          NOT NULL,
    created_at_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_template_section_id),
    UNIQUE KEY uq_template_section_system (inspection_template_id, system_number),
    UNIQUE KEY uq_template_section_key (inspection_template_id, system_key),
    KEY idx_template_sections_template (inspection_template_id, sort_order),
    CONSTRAINT fk_template_sections_template FOREIGN KEY (inspection_template_id) REFERENCES inspection_templates(inspection_template_id)
) ENGINE=InnoDB;

CREATE TABLE inspection_template_items (
    inspection_template_item_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NOT NULL,
    item_key               VARCHAR(80)  NOT NULL,
    display_name           VARCHAR(200) NOT NULL,
    help_text              TEXT         NULL,
    response_type          VARCHAR(30)  NOT NULL DEFAULT 'Score0To5',
    is_required            TINYINT(1)   NOT NULL DEFAULT 0,
    default_weight         DECIMAL(9,6) NULL,
    allows_red_flag        TINYINT(1)   NOT NULL DEFAULT 0,
    sort_order             INT          NOT NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_template_item_id),
    UNIQUE KEY uq_template_items_key (inspection_template_section_id, item_key),
    KEY idx_template_items_section (inspection_template_section_id, sort_order),
    CONSTRAINT fk_template_items_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id)
) ENGINE=InnoDB;

-- ============================================================
-- INSPECTION EXECUTION
-- ============================================================

CREATE TABLE inspections (
    inspection_id          CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    inspection_template_id CHAR(36)     NOT NULL,
    inspection_type        VARCHAR(30)  NOT NULL DEFAULT 'Standard',
    status                 VARCHAR(30)  NOT NULL DEFAULT 'Draft',
    inspected_by_user_id   CHAR(36)     NULL,
    inspected_on_utc       DATETIME(6)  NULL,
    inspection_location    VARCHAR(255) NULL,
    summary_notes          TEXT         NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_id),
    KEY idx_inspections_appraisal (appraisal_id),
    KEY idx_inspections_template (inspection_template_id),
    KEY idx_inspections_status (status),
    CONSTRAINT fk_inspections_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_inspections_template FOREIGN KEY (inspection_template_id) REFERENCES inspection_templates(inspection_template_id),
    CONSTRAINT fk_inspections_user FOREIGN KEY (inspected_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB;

CREATE TABLE inspection_section_scores (
    inspection_section_score_id CHAR(36) NOT NULL,
    inspection_id          CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    inspection_template_section_id CHAR(36) NOT NULL,
    system_number          INT          NOT NULL,
    system_key             VARCHAR(50)  NOT NULL,
    score_0_to_5           DECIMAL(4,2) NOT NULL,
    normalized_score_0_to_100 DECIMAL(6,2) NULL,
    weight                 DECIMAL(9,6) NOT NULL,
    weighted_score         DECIMAL(12,6) NULL,
    red_flag_count         INT          NOT NULL DEFAULT 0,
    notes                  TEXT         NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_section_score_id),
    UNIQUE KEY uq_section_scores (inspection_id, inspection_template_section_id),
    KEY idx_section_scores_appraisal (appraisal_id),
    KEY idx_section_scores_system (inspection_id, system_number),
    CONSTRAINT fk_section_scores_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_section_scores_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_section_scores_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id)
) ENGINE=InnoDB;

CREATE TABLE inspection_item_scores (
    inspection_item_score_id CHAR(36)  NOT NULL,
    inspection_id          CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    inspection_template_item_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NOT NULL,
    system_number          INT          NOT NULL,
    item_key               VARCHAR(80)  NOT NULL,
    score_0_to_5           DECIMAL(4,2) NULL,
    boolean_response       TINYINT(1)   NULL,
    text_response          TEXT         NULL,
    numeric_response       DECIMAL(18,4) NULL,
    evidence_note          TEXT         NULL,
    is_red_flag            TINYINT(1)   NOT NULL DEFAULT 0,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_item_score_id),
    UNIQUE KEY uq_item_scores (inspection_id, inspection_template_item_id),
    KEY idx_item_scores_appraisal (appraisal_id),
    KEY idx_item_scores_section (inspection_id, system_number),
    CONSTRAINT fk_item_scores_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_item_scores_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_item_scores_item FOREIGN KEY (inspection_template_item_id) REFERENCES inspection_template_items(inspection_template_item_id),
    CONSTRAINT fk_item_scores_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id)
) ENGINE=InnoDB;

CREATE TABLE inspection_red_flags (
    inspection_red_flag_id CHAR(36)    NOT NULL,
    inspection_id          CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    inspection_template_section_id CHAR(36) NULL,
    inspection_template_item_id CHAR(36) NULL,
    system_number          INT          NULL,
    severity               VARCHAR(20)  NOT NULL,
    red_flag_code          VARCHAR(50)  NULL,
    title                  VARCHAR(150) NOT NULL,
    description            TEXT         NULL,
    penalty_factor         DECIMAL(8,4) NULL,
    requires_manual_review TINYINT(1)   NOT NULL DEFAULT 0,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_red_flag_id),
    KEY idx_red_flags_inspection (inspection_id),
    KEY idx_red_flags_appraisal (appraisal_id),
    KEY idx_red_flags_severity (severity),
    CONSTRAINT fk_red_flags_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_red_flags_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_red_flags_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id),
    CONSTRAINT fk_red_flags_item FOREIGN KEY (inspection_template_item_id) REFERENCES inspection_template_items(inspection_template_item_id)
) ENGINE=InnoDB;

CREATE TABLE inspection_media (
    inspection_media_id    CHAR(36)     NOT NULL,
    inspection_id          CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    inspection_template_section_id CHAR(36) NULL,
    inspection_template_item_id CHAR(36) NULL,
    media_type             VARCHAR(20)  NOT NULL DEFAULT 'Photo',
    file_name              VARCHAR(255) NOT NULL,
    file_extension         VARCHAR(20)  NULL,
    mime_type              VARCHAR(100) NULL,
    storage_provider       VARCHAR(50)  NOT NULL DEFAULT 'PendingDecision',
    storage_path           VARCHAR(500) NULL,
    content_hash           VARCHAR(128) NULL,
    sort_order             INT          NOT NULL DEFAULT 0,
    caption                VARCHAR(255) NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_media_id),
    KEY idx_inspection_media_inspection (inspection_id),
    KEY idx_inspection_media_appraisal (appraisal_id),
    CONSTRAINT fk_inspection_media_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_inspection_media_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_inspection_media_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id),
    CONSTRAINT fk_inspection_media_item FOREIGN KEY (inspection_template_item_id) REFERENCES inspection_template_items(inspection_template_item_id)
) ENGINE=InnoDB;

-- ============================================================
-- COMPARABLE SALES / ADJUSTMENTS
-- ============================================================

CREATE TABLE comparable_sales (
    comparable_id          CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    source                 VARCHAR(100) NOT NULL,
    source_listing_url     VARCHAR(500) NULL,
    venue_type             VARCHAR(50)  NULL,
    sale_type              VARCHAR(30)  NULL,
    sale_date_utc          DATETIME(6)  NULL,
    sale_price             DECIMAL(18,2) NULL,
    asking_price           DECIMAL(18,2) NULL,
    currency_code          VARCHAR(10)  NOT NULL DEFAULT 'USD',
    year                   INT          NOT NULL,
    make                   VARCHAR(100) NOT NULL,
    model                  VARCHAR(100) NOT NULL,
    trim                   VARCHAR(100) NULL,
    series                 VARCHAR(100) NULL,
    body_style             VARCHAR(100) NULL,
    drivetrain             VARCHAR(100) NULL,
    engine                 VARCHAR(150) NULL,
    transmission           VARCHAR(150) NULL,
    mileage                INT          NULL,
    exterior_color         VARCHAR(100) NULL,
    interior_color         VARCHAR(100) NULL,
    originality_level      VARCHAR(50)  NULL,
    restoration_level      VARCHAR(50)  NULL,
    matching_numbers       VARCHAR(30)  NULL,
    documentation_level    VARCHAR(50)  NULL,
    title_status           VARCHAR(50)  NULL,
    location_city          VARCHAR(100) NULL,
    location_state         VARCHAR(100) NULL,
    overall_condition_score DECIMAL(6,2) NULL,
    condition_summary      VARCHAR(255) NULL,
    authenticity_score     DECIMAL(6,2) NULL,
    has_red_flags          TINYINT(1)   NOT NULL DEFAULT 0,
    notes                  TEXT         NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (comparable_id),
    KEY idx_comparable_sales_appraisal (appraisal_id),
    KEY idx_comparable_sales_sale_date (sale_date_utc),
    KEY idx_comparable_sales_lookup (year, make, model),
    CONSTRAINT fk_comparable_sales_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id)
) ENGINE=InnoDB;

CREATE TABLE comparable_adjustments (
    comparable_adjustment_id CHAR(36)  NOT NULL,
    comparable_id          CHAR(36)     NOT NULL,
    adjustment_category    VARCHAR(50)  NOT NULL,
    adjustment_key         VARCHAR(80)  NOT NULL,
    adjustment_delta       DECIMAL(18,2) NOT NULL,
    adjustment_factor      DECIMAL(12,6) NULL,
    rationale              TEXT         NULL,
    sort_order             INT          NOT NULL DEFAULT 0,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (comparable_adjustment_id),
    KEY idx_comparable_adjustments_comp (comparable_id),
    CONSTRAINT fk_comparable_adjustments_comp FOREIGN KEY (comparable_id) REFERENCES comparable_sales(comparable_id)
) ENGINE=InnoDB;

-- ============================================================
-- DETERMINISTIC VALUATION
-- ============================================================

CREATE TABLE valuation_runs (
    valuation_run_id       CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    valuation_status       VARCHAR(30)  NOT NULL DEFAULT 'Pending',
    engine_version         VARCHAR(50)  NOT NULL,
    rubric_version         VARCHAR(50)  NOT NULL,
    ruleset_version        VARCHAR(50)  NOT NULL,
    condition_index        DECIMAL(12,6) NULL,
    market_factor          DECIMAL(12,6) NULL,
    red_flag_factor        DECIMAL(12,6) NULL,
    confidence_score       DECIMAL(12,6) NULL,
    confidence_band        VARCHAR(20)  NULL,
    value_low              DECIMAL(18,2) NULL,
    final_value            DECIMAL(18,2) NULL,
    value_high             DECIMAL(18,2) NULL,
    manual_review_required TINYINT(1)   NOT NULL DEFAULT 0,
    run_notes              TEXT         NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (valuation_run_id),
    KEY idx_valuation_runs_appraisal (appraisal_id, created_at_utc),
    KEY idx_valuation_runs_status (valuation_status),
    CONSTRAINT fk_valuation_runs_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id)
) ENGINE=InnoDB;

CREATE TABLE valuation_comparable_links (
    valuation_comparable_link_id CHAR(36) NOT NULL,
    valuation_run_id       CHAR(36)     NOT NULL,
    comparable_id          CHAR(36)     NOT NULL,
    selection_rank         INT          NOT NULL,
    similarity_score       DECIMAL(12,6) NULL,
    adjusted_price         DECIMAL(18,2) NULL,
    weighting_factor       DECIMAL(12,6) NULL,
    was_included_in_median TINYINT(1)   NOT NULL DEFAULT 1,
    notes                  TEXT         NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (valuation_comparable_link_id),
    UNIQUE KEY uq_val_comp_link (valuation_run_id, comparable_id),
    KEY idx_valuation_comparable_links_run (valuation_run_id, selection_rank),
    CONSTRAINT fk_valuation_comparable_links_run FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id),
    CONSTRAINT fk_valuation_comparable_links_comp FOREIGN KEY (comparable_id) REFERENCES comparable_sales(comparable_id)
) ENGINE=InnoDB;

CREATE TABLE valuation_trace_lines (
    valuation_trace_line_id CHAR(36)    NOT NULL,
    valuation_run_id       CHAR(36)     NOT NULL,
    sort_order            INT           NOT NULL,
    trace_type            VARCHAR(30)   NOT NULL,
    trace_key             VARCHAR(80)   NOT NULL,
    amount                DECIMAL(18,4) NULL,
    factor                DECIMAL(18,6) NULL,
    message               VARCHAR(1000) NULL,
    evidence_ref          VARCHAR(255)  NULL,
    created_at_utc        DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (valuation_trace_line_id),
    UNIQUE KEY uq_valuation_trace_lines_order (valuation_run_id, sort_order),
    KEY idx_valuation_trace_lines_run (valuation_run_id),
    CONSTRAINT fk_valuation_trace_lines_run FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id)
) ENGINE=InnoDB;

-- ============================================================
-- REPORTING / EXPORTS
-- ============================================================

CREATE TABLE appraisal_reports (
    appraisal_report_id    CHAR(36)     NOT NULL,
    appraisal_id           CHAR(36)     NOT NULL,
    valuation_run_id       CHAR(36)     NULL,
    report_version         VARCHAR(50)  NOT NULL,
    report_status          VARCHAR(30)  NOT NULL DEFAULT 'Draft',
    report_type            VARCHAR(50)  NOT NULL DEFAULT 'Appraisal',
    template_name          VARCHAR(150) NULL,
    generated_on_utc       DATETIME(6)  NULL,
    generated_by_user_id   CHAR(36)     NULL,
    narrative_snapshot     MEDIUMTEXT   NULL,
    created_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc         DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (appraisal_report_id),
    KEY idx_appraisal_reports_appraisal (appraisal_id),
    KEY idx_appraisal_reports_run (valuation_run_id),
    CONSTRAINT fk_appraisal_reports_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_appraisal_reports_run FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id),
    CONSTRAINT fk_appraisal_reports_user FOREIGN KEY (generated_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB;

CREATE TABLE report_exports (
    report_export_id       CHAR(36)     NOT NULL,
    appraisal_report_id    CHAR(36)     NOT NULL,
    export_format          VARCHAR(20)  NOT NULL,
    export_status          VARCHAR(30)  NOT NULL DEFAULT 'Generated',
    file_name              VARCHAR(255) NOT NULL,
    storage_provider       VARCHAR(50)  NOT NULL DEFAULT 'PendingDecision',
    storage_path           VARCHAR(500) NULL,
    file_size_bytes        BIGINT       NULL,
    exported_on_utc        DATETIME(6)  NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    exported_by_user_id    CHAR(36)     NULL,
    checksum_sha256        VARCHAR(64)  NULL,
    PRIMARY KEY (report_export_id),
    KEY idx_report_exports_report (appraisal_report_id),
    CONSTRAINT fk_report_exports_report FOREIGN KEY (appraisal_report_id) REFERENCES appraisal_reports(appraisal_report_id),
    CONSTRAINT fk_report_exports_user FOREIGN KEY (exported_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB;
