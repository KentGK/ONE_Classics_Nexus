-- =====================================================================
-- ONE Classics Nexus Database Schema v1.0
-- Target: MariaDB 10.6+
-- Database: one_classics_nexus
-- Purpose: Canonical ERD + SQL implementation for Nexus Phase 1/2/3 baseline
-- =====================================================================

CREATE DATABASE IF NOT EXISTS one_classics_nexus
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE one_classics_nexus;

-- =====================================================================
-- 1. Core reference / governance tables
-- =====================================================================

CREATE TABLE IF NOT EXISTS system_config_versions (
    config_version_id      CHAR(36)     NOT NULL,
    version_label          VARCHAR(50)  NOT NULL,
    engine_version         VARCHAR(50)  NOT NULL,
    rubric_version         VARCHAR(50)  NOT NULL,
    ruleset_version        VARCHAR(50)  NOT NULL,
    status                 VARCHAR(20)  NOT NULL DEFAULT 'Draft',
    effective_from_utc     DATETIME     NULL,
    effective_to_utc       DATETIME     NULL,
    notes                  TEXT         NULL,
    created_at_utc         DATETIME     NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME     NULL,
    PRIMARY KEY (config_version_id),
    UNIQUE KEY ux_system_config_versions_label (version_label)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS system_weights (
    weight_id              CHAR(36)      NOT NULL,
    config_version_id      CHAR(36)      NULL,
    vehicle_type           VARCHAR(20)   NOT NULL,
    system_number          INT           NOT NULL,
    system_code            VARCHAR(30)   NOT NULL,
    system_key             VARCHAR(80)   NOT NULL,
    weight                 DECIMAL(5,4)  NOT NULL,
    version                INT           NOT NULL DEFAULT 1,
    is_active              BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (weight_id),
    UNIQUE KEY ux_system_weights_active (vehicle_type, system_number, version),
    KEY ix_system_weights_vehicle_type (vehicle_type, is_active),
    CONSTRAINT fk_system_weights_config
      FOREIGN KEY (config_version_id) REFERENCES system_config_versions(config_version_id),
    CONSTRAINT ck_system_weights_number CHECK (system_number BETWEEN 1 AND 12),
    CONSTRAINT ck_system_weights_weight CHECK (weight >= 0 AND weight <= 1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS inspection_templates (
    template_id            CHAR(36)      NOT NULL,
    config_version_id      CHAR(36)      NULL,
    vehicle_type           VARCHAR(20)   NOT NULL,
    version                INT           NOT NULL DEFAULT 1,
    template_name          VARCHAR(120)  NOT NULL,
    status                 VARCHAR(20)   NOT NULL DEFAULT 'Draft',
    is_active              BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME      NULL,
    PRIMARY KEY (template_id),
    UNIQUE KEY ux_inspection_templates (vehicle_type, version),
    KEY ix_inspection_templates_active (vehicle_type, is_active),
    CONSTRAINT fk_inspection_templates_config
      FOREIGN KEY (config_version_id) REFERENCES system_config_versions(config_version_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS inspection_template_items (
    item_id                CHAR(36)      NOT NULL,
    template_id            CHAR(36)      NOT NULL,
    system_number          INT           NOT NULL,
    system_code            VARCHAR(30)   NOT NULL,
    system_key             VARCHAR(80)   NOT NULL,
    item_key               VARCHAR(100)  NOT NULL,
    item_name              VARCHAR(150)  NOT NULL,
    description            TEXT          NULL,
    display_order          INT           NOT NULL DEFAULT 0,
    is_required            BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (item_id),
    UNIQUE KEY ux_template_item_key (template_id, item_key),
    KEY ix_template_items_template_system (template_id, system_number),
    CONSTRAINT fk_template_items_template
      FOREIGN KEY (template_id) REFERENCES inspection_templates(template_id),
    CONSTRAINT ck_template_items_system_number CHECK (system_number BETWEEN 1 AND 12)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS red_flag_rules (
    rule_id                CHAR(36)      NOT NULL,
    config_version_id      CHAR(36)      NULL,
    vehicle_type           VARCHAR(20)   NULL,
    system_number          INT           NOT NULL,
    system_code            VARCHAR(30)   NOT NULL,
    system_key             VARCHAR(80)   NOT NULL,
    rule_key               VARCHAR(100)  NOT NULL,
    severity               VARCHAR(20)   NOT NULL,
    description            TEXT          NOT NULL,
    modifier               DECIMAL(4,2)  NOT NULL,
    version                INT           NOT NULL DEFAULT 1,
    is_active              BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (rule_id),
    UNIQUE KEY ux_red_flag_rule_version (rule_key, version),
    KEY ix_red_flag_rules_active (vehicle_type, system_code, is_active),
    CONSTRAINT fk_red_flag_rules_config
      FOREIGN KEY (config_version_id) REFERENCES system_config_versions(config_version_id),
    CONSTRAINT ck_red_flag_rules_system_number CHECK (system_number BETWEEN 1 AND 12),
    CONSTRAINT ck_red_flag_rules_modifier CHECK (modifier > 0 AND modifier <= 1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- 2. Core vehicle and appraisal lifecycle
-- =====================================================================

CREATE TABLE IF NOT EXISTS vehicles (
    vehicle_id             CHAR(36)      NOT NULL,
    vin                    VARCHAR(50)   NULL,
    year                   INT           NOT NULL,
    make                   VARCHAR(100)  NOT NULL,
    model                  VARCHAR(100)  NOT NULL,
    trim                   VARCHAR(100)  NULL,
    body_style             VARCHAR(100)  NULL,
    engine                 VARCHAR(100)  NULL,
    transmission           VARCHAR(100)  NULL,
    drivetrain             VARCHAR(100)  NULL,
    mileage                INT           NULL,
    exterior_color         VARCHAR(100)  NULL,
    interior_color         VARCHAR(100)  NULL,
    title_status           VARCHAR(100)  NULL,
    is_deleted             BOOLEAN       NOT NULL DEFAULT FALSE,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME      NULL,
    PRIMARY KEY (vehicle_id),
    UNIQUE KEY ux_vehicles_vin (vin),
    KEY ix_vehicles_make_model_year (make, model, year),
    CONSTRAINT ck_vehicles_year CHECK (year BETWEEN 1886 AND 2100),
    CONSTRAINT ck_vehicles_mileage CHECK (mileage IS NULL OR mileage >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS appraisals (
    appraisal_id           CHAR(36)      NOT NULL,
    vehicle_id             CHAR(36)      NOT NULL,
    status                 VARCHAR(40)   NOT NULL DEFAULT 'Draft',
    requested_by           VARCHAR(150)  NULL,
    assigned_to            VARCHAR(150)  NULL,
    notes                  TEXT          NULL,
    row_version            INT           NOT NULL DEFAULT 1,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME      NULL,
    PRIMARY KEY (appraisal_id),
    KEY ix_appraisals_vehicle_id (vehicle_id),
    KEY ix_appraisals_status (status),
    CONSTRAINT fk_appraisals_vehicle
      FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS appraisal_status_history (
    history_id             CHAR(36)      NOT NULL,
    appraisal_id           CHAR(36)      NOT NULL,
    from_status            VARCHAR(40)   NULL,
    to_status              VARCHAR(40)   NOT NULL,
    changed_by             VARCHAR(150)  NULL,
    changed_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    notes                  TEXT          NULL,
    PRIMARY KEY (history_id),
    KEY ix_status_history_appraisal (appraisal_id, changed_at_utc),
    CONSTRAINT fk_status_history_appraisal
      FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- 3. Inspection and condition scoring
-- =====================================================================

CREATE TABLE IF NOT EXISTS inspections (
    inspection_id          CHAR(36)      NOT NULL,
    appraisal_id           CHAR(36)      NOT NULL,
    template_id            CHAR(36)      NULL,
    vehicle_type           VARCHAR(20)   NOT NULL,
    status                 VARCHAR(30)   NOT NULL DEFAULT 'Draft',
    notes                  TEXT          NULL,
    is_deleted             BOOLEAN       NOT NULL DEFAULT FALSE,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME      NULL,
    PRIMARY KEY (inspection_id),
    KEY ix_inspections_appraisal_id (appraisal_id),
    KEY ix_inspections_template_id (template_id),
    KEY ix_inspections_status (status),
    CONSTRAINT fk_inspections_appraisal
      FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_inspections_template
      FOREIGN KEY (template_id) REFERENCES inspection_templates(template_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS condition_scores (
    condition_score_id     CHAR(36)      NOT NULL,
    inspection_id          CHAR(36)      NOT NULL,
    appraisal_id           CHAR(36)      NOT NULL,
    system_number          INT           NOT NULL,
    system_code            VARCHAR(30)   NOT NULL,
    system_key             VARCHAR(80)   NOT NULL,
    score_0_to_5           DECIMAL(3,2)  NOT NULL,
    weight                 DECIMAL(5,4)  NOT NULL,
    weighted_value         DECIMAL(6,4)  GENERATED ALWAYS AS (score_0_to_5 * weight) STORED,
    is_red_flag            BOOLEAN       NOT NULL DEFAULT FALSE,
    notes                  TEXT          NULL,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME      NULL,
    PRIMARY KEY (condition_score_id),
    UNIQUE KEY ux_condition_scores_inspection_system (inspection_id, system_number),
    KEY ix_condition_scores_appraisal_id (appraisal_id),
    KEY ix_condition_scores_system (system_number, system_key),
    CONSTRAINT fk_condition_scores_inspection
      FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_condition_scores_appraisal
      FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT ck_condition_scores_system_number CHECK (system_number BETWEEN 1 AND 12),
    CONSTRAINT ck_condition_scores_score CHECK (score_0_to_5 >= 0 AND score_0_to_5 <= 5),
    CONSTRAINT ck_condition_scores_weight CHECK (weight >= 0 AND weight <= 1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS inspection_red_flags (
    inspection_red_flag_id CHAR(36)      NOT NULL,
    inspection_id          CHAR(36)      NOT NULL,
    condition_score_id     CHAR(36)      NULL,
    rule_id                CHAR(36)      NULL,
    system_number          INT           NOT NULL,
    system_code            VARCHAR(30)   NOT NULL,
    system_key             VARCHAR(80)   NOT NULL,
    severity               VARCHAR(20)   NOT NULL,
    description            TEXT          NOT NULL,
    modifier               DECIMAL(4,2)  NOT NULL DEFAULT 1.00,
    notes                  TEXT          NULL,
    created_at_utc         DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (inspection_red_flag_id),
    KEY ix_red_flags_inspection (inspection_id),
    KEY ix_red_flags_rule (rule_id),
    CONSTRAINT fk_inspection_red_flags_inspection
      FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_inspection_red_flags_score
      FOREIGN KEY (condition_score_id) REFERENCES condition_scores(condition_score_id),
    CONSTRAINT fk_inspection_red_flags_rule
      FOREIGN KEY (rule_id) REFERENCES red_flag_rules(rule_id),
    CONSTRAINT ck_inspection_red_flags_system_number CHECK (system_number BETWEEN 1 AND 12),
    CONSTRAINT ck_inspection_red_flags_modifier CHECK (modifier > 0 AND modifier <= 1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- 4. Comparable sales and valuation
-- =====================================================================

CREATE TABLE IF NOT EXISTS comparable_sales (
    comparable_id          CHAR(36)       NOT NULL,
    appraisal_id           CHAR(36)       NOT NULL,
    year                   INT            NOT NULL,
    make                   VARCHAR(100)   NOT NULL,
    model                  VARCHAR(100)   NOT NULL,
    trim                   VARCHAR(100)   NULL,
    body_style             VARCHAR(100)   NULL,
    series                 VARCHAR(100)   NULL,
    drivetrain             VARCHAR(100)   NULL,
    engine                 VARCHAR(100)   NULL,
    transmission           VARCHAR(100)   NULL,
    mileage                INT            NULL,
    sale_price             DECIMAL(12,2)  NOT NULL,
    asking_price           DECIMAL(12,2)  NULL,
    sale_date              DATE           NOT NULL,
    source                 VARCHAR(120)   NOT NULL,
    source_listing_url     TEXT           NULL,
    venue_type             VARCHAR(80)    NULL,
    overall_condition_score DECIMAL(5,2)  NULL,
    notes                  TEXT           NULL,
    is_deleted             BOOLEAN        NOT NULL DEFAULT FALSE,
    created_at_utc         DATETIME       NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc         DATETIME       NULL,
    PRIMARY KEY (comparable_id),
    KEY ix_comparable_sales_appraisal (appraisal_id),
    KEY ix_comparable_sales_subject (make, model, year),
    KEY ix_comparable_sales_sale_date (sale_date),
    CONSTRAINT fk_comparable_sales_appraisal
      FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT ck_comparable_sales_sale_price CHECK (sale_price >= 0),
    CONSTRAINT ck_comparable_sales_mileage CHECK (mileage IS NULL OR mileage >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS valuation_runs (
    valuation_run_id       CHAR(36)       NOT NULL,
    appraisal_id           CHAR(36)       NOT NULL,
    inspection_id          CHAR(36)       NULL,
    engine_version         VARCHAR(50)    NOT NULL,
    rubric_version         VARCHAR(50)    NOT NULL,
    ruleset_version        VARCHAR(50)    NOT NULL,
    condition_score        DECIMAL(6,2)   NULL,
    condition_tier         VARCHAR(10)    NULL,
    tier_multiplier        DECIMAL(5,2)   NULL,
    median_comparable_value DECIMAL(12,2) NULL,
    red_flag_modifier      DECIMAL(5,2)   NULL,
    confidence_score       DECIMAL(5,4)   NULL,
    confidence_modifier    DECIMAL(5,2)   NULL,
    final_value            DECIMAL(12,2)  NULL,
    value_low              DECIMAL(12,2)  NULL,
    value_high             DECIMAL(12,2)  NULL,
    status                 VARCHAR(40)    NOT NULL,
    created_at_utc         DATETIME       NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (valuation_run_id),
    KEY ix_valuation_runs_appraisal (appraisal_id, created_at_utc),
    KEY ix_valuation_runs_inspection (inspection_id),
    KEY ix_valuation_runs_status (status),
    CONSTRAINT fk_valuation_runs_appraisal
      FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_valuation_runs_inspection
      FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS valuation_comparable_links (
    valuation_comparable_link_id CHAR(36)     NOT NULL,
    valuation_run_id       CHAR(36)           NOT NULL,
    comparable_id          CHAR(36)           NOT NULL,
    similarity_score       DECIMAL(5,4)       NULL,
    adjusted_value         DECIMAL(12,2)      NULL,
    adjustment_notes       TEXT               NULL,
    created_at_utc         DATETIME           NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (valuation_comparable_link_id),
    UNIQUE KEY ux_valuation_comparable_link (valuation_run_id, comparable_id),
    KEY ix_vcl_comparable (comparable_id),
    CONSTRAINT fk_vcl_valuation_run
      FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id),
    CONSTRAINT fk_vcl_comparable
      FOREIGN KEY (comparable_id) REFERENCES comparable_sales(comparable_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS valuation_trace_lines (
    trace_id               CHAR(36)       NOT NULL,
    valuation_run_id       CHAR(36)       NOT NULL,
    sort_order             INT            NOT NULL,
    type                   VARCHAR(50)    NOT NULL,
    trace_key              VARCHAR(100)   NOT NULL,
    amount                 DECIMAL(12,2)  NULL,
    factor                 DECIMAL(10,5)  NULL,
    message                TEXT           NOT NULL,
    evidence_ref           VARCHAR(255)   NULL,
    created_at_utc         DATETIME       NOT NULL DEFAULT UTC_TIMESTAMP(),
    PRIMARY KEY (trace_id),
    KEY ix_trace_lines_run_order (valuation_run_id, sort_order),
    CONSTRAINT fk_trace_lines_valuation_run
      FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- 5. Reports and audit
-- =====================================================================

CREATE TABLE IF NOT EXISTS appraisal_reports (
    report_id              CHAR(36)       NOT NULL,
    appraisal_id           CHAR(36)       NOT NULL,
    valuation_run_id       CHAR(36)       NULL,
    template_key           VARCHAR(100)   NOT NULL,
    report_format          VARCHAR(20)    NOT NULL,
    report_status          VARCHAR(30)    NOT NULL DEFAULT 'Draft',
    storage_uri            TEXT           NULL,
    download_url           TEXT           NULL,
    created_at_utc         DATETIME       NOT NULL DEFAULT UTC_TIMESTAMP(),
    finalized_at_utc       DATETIME       NULL,
    locked_at_utc          DATETIME       NULL,
    PRIMARY KEY (report_id),
    KEY ix_reports_appraisal (appraisal_id),
    KEY ix_reports_valuation_run (valuation_run_id),
    KEY ix_reports_status (report_status),
    CONSTRAINT fk_reports_appraisal
      FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_reports_valuation_run
      FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS audit_log (
    audit_id               CHAR(36)       NOT NULL,
    entity_name            VARCHAR(100)   NOT NULL,
    entity_id              CHAR(36)       NOT NULL,
    action                 VARCHAR(50)    NOT NULL,
    changed_by             VARCHAR(150)   NULL,
    changed_at_utc         DATETIME       NOT NULL DEFAULT UTC_TIMESTAMP(),
    before_json            JSON           NULL,
    after_json             JSON           NULL,
    notes                  TEXT           NULL,
    PRIMARY KEY (audit_id),
    KEY ix_audit_entity (entity_name, entity_id),
    KEY ix_audit_changed_at (changed_at_utc)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- 6. Views
-- =====================================================================

CREATE OR REPLACE VIEW vw_appraisal_latest_valuation AS
SELECT
    a.appraisal_id,
    a.vehicle_id,
    v.year,
    v.make,
    v.model,
    v.trim,
    a.status AS appraisal_status,
    vr.valuation_run_id,
    vr.created_at_utc AS valuation_created_at_utc,
    vr.status AS valuation_status,
    vr.condition_score,
    vr.condition_tier,
    vr.final_value,
    vr.value_low,
    vr.value_high,
    vr.confidence_score
FROM appraisals a
JOIN vehicles v ON v.vehicle_id = a.vehicle_id
LEFT JOIN valuation_runs vr
    ON vr.valuation_run_id = (
        SELECT vr2.valuation_run_id
        FROM valuation_runs vr2
        WHERE vr2.appraisal_id = a.appraisal_id
        ORDER BY vr2.created_at_utc DESC
        LIMIT 1
    );

CREATE OR REPLACE VIEW vw_inspection_condition_summary AS
SELECT
    i.inspection_id,
    i.appraisal_id,
    i.vehicle_type,
    COUNT(cs.condition_score_id) AS scored_system_count,
    SUM(cs.weighted_value) AS total_weighted_value,
    SUM(cs.weighted_value) * 20 AS condition_index,
    MAX(CASE WHEN cs.is_red_flag THEN 1 ELSE 0 END) AS has_red_flags
FROM inspections i
LEFT JOIN condition_scores cs ON cs.inspection_id = i.inspection_id
WHERE i.is_deleted = FALSE
GROUP BY i.inspection_id, i.appraisal_id, i.vehicle_type;

-- =====================================================================
-- End of Schema
-- =====================================================================
