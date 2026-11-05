/*
  ONE Classics Nexus
  Nexus_Full_Lock_Schema_v1.0.sql

  Purpose:
    Canonical lock-schema build script for one_classics_nexus.

  Notes:
    - Designed for MariaDB/MySQL.
    - Uses CHAR(36) GUID identifiers to align with the C# / Dapper implementation.
    - Preserves the locked 12-system inspection / AVM model.
    - condition_scores is the canonical AVM condition-input table.
    - valuation_runs + valuation_trace_lines are the canonical valuation audit trail.
    - The clients table follows locked schema v2.0; see
      docs/04_Data/SQL/Clients/Clients_v2.0.sql for the authoritative definition
      and Update_Clients_To_v2.0.sql for the migration path.
*/

CREATE DATABASE IF NOT EXISTS one_classics_nexus_temp
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE one_classics_nexus_temp;

SET FOREIGN_KEY_CHECKS = 0;

DROP VIEW IF EXISTS vw_valuation_trace_report;
DROP VIEW IF EXISTS vw_inspection_section_rollup;
DROP VIEW IF EXISTS vw_appraisal_latest_valuation;

DROP TABLE IF EXISTS report_exports;
DROP TABLE IF EXISTS appraisal_reports;
DROP TABLE IF EXISTS valuation_trace_lines;
DROP TABLE IF EXISTS valuation_comparable_links;
DROP TABLE IF EXISTS valuation_runs;
DROP TABLE IF EXISTS comparable_adjustments;
DROP TABLE IF EXISTS comparable_sales;
DROP TABLE IF EXISTS condition_scores;
DROP TABLE IF EXISTS inspection_red_flags;
DROP TABLE IF EXISTS inspection_media;
DROP TABLE IF EXISTS inspection_item_scores;
DROP TABLE IF EXISTS inspection_section_scores;
DROP TABLE IF EXISTS inspections;
DROP TABLE IF EXISTS appraisal_assignments;
DROP TABLE IF EXISTS appraisals;
DROP TABLE IF EXISTS appraisal_requests;
DROP TABLE IF EXISTS inspection_template_items;
DROP TABLE IF EXISTS inspection_template_sections;
DROP TABLE IF EXISTS inspection_templates;
DROP TABLE IF EXISTS vehicles;
DROP TABLE IF EXISTS vehicle_type_system_weights;
DROP TABLE IF EXISTS vehicle_types;
DROP TABLE IF EXISTS user_roles;
DROP TABLE IF EXISTS roles;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS entity_number_sequences;
DROP TABLE IF EXISTS client_contacts;
DROP TABLE IF EXISTS clients;

SET FOREIGN_KEY_CHECKS = 1;

-- =========================================================
-- Security / identity
-- =========================================================

CREATE TABLE users (
    user_id CHAR(36) NOT NULL,
    email VARCHAR(255) NOT NULL,
    username varchar(100) NOT NULL,
    display_name VARCHAR(150) NOT NULL,
    first_name VARCHAR(100) NULL,
    last_name VARCHAR(100) NULL,
    auth_provider VARCHAR(50) NULL,
    external_subject VARCHAR(255) NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (user_id),
    UNIQUE KEY uq_users_email (email),
    KEY idx_users_active (is_active)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE roles (
    role_id CHAR(36) NOT NULL,
    role_code VARCHAR(50) NOT NULL,
    role_name VARCHAR(100) NOT NULL,
    description VARCHAR(255) NULL,
    is_system_role TINYINT(1) NOT NULL DEFAULT 0,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (role_id),
    UNIQUE KEY uq_roles_code (role_code),
    UNIQUE KEY uq_roles_name (role_name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE user_roles (
    user_role_id CHAR(36) NOT NULL,
    user_id CHAR(36) NOT NULL,
    role_id CHAR(36) NOT NULL,
    assigned_by_user_id CHAR(36) NULL,
    assigned_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (user_role_id),
    UNIQUE KEY uq_user_roles_user_role (user_id, role_id),
    KEY idx_user_roles_user (user_id),
    KEY idx_user_roles_role (role_id),
    KEY idx_user_roles_assigned_by (assigned_by_user_id),
    CONSTRAINT fk_user_roles_user FOREIGN KEY (user_id) REFERENCES users(user_id),
    CONSTRAINT fk_user_roles_role FOREIGN KEY (role_id) REFERENCES roles(role_id),
    CONSTRAINT fk_user_roles_assigned_by FOREIGN KEY (assigned_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Clients
-- =========================================================

CREATE TABLE entity_number_sequences (
    sequence_key VARCHAR(64) NOT NULL,
    last_value BIGINT NOT NULL,
    PRIMARY KEY (sequence_key)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE clients (
    client_id CHAR(36) NOT NULL,
    client_number VARCHAR(30) NOT NULL,
    client_type_code VARCHAR(30) NOT NULL,
    first_name VARCHAR(100) NULL,
    last_name VARCHAR(100) NULL,
    middle_name VARCHAR(100) NULL,
    display_name VARCHAR(200) NOT NULL,
    address_line1 VARCHAR(200) NULL,
    address_line2 VARCHAR(200) NULL,
    city VARCHAR(100) NULL,
    state_region VARCHAR(100) NULL,
    postal_code VARCHAR(30) NULL,
    email VARCHAR(254) NOT NULL,
    phone VARCHAR(40) NOT NULL,
    preferred_contact_method VARCHAR(20) NULL,
    notes TEXT NULL,
    row_version BIGINT UNSIGNED NOT NULL DEFAULT 1,
    created_at_utc DATETIME(6) NOT NULL DEFAULT UTC_TIMESTAMP(6),
    created_by_user_id CHAR(36) NOT NULL,
    updated_at_utc DATETIME(6) NOT NULL DEFAULT UTC_TIMESTAMP(6),
    updated_by_user_id CHAR(36) NOT NULL,
    organization_name VARCHAR(200) NULL,
    organization_type VARCHAR(100) NULL,
    Status_Code VARCHAR(20) NOT NULL DEFAULT 'Active',
    PRIMARY KEY (client_id),
    UNIQUE KEY uq_clients_client_number (client_number),
    KEY idx_clients_display_name (display_name),
    KEY idx_clients_email (email),
    KEY idx_clients_type (client_type_code),
    KEY idx_clients_status_display (Status_Code, display_name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE client_contacts (
    client_contact_id CHAR(36) NOT NULL,
    client_id CHAR(36) NOT NULL,
    contact_name VARCHAR(200) NOT NULL,
    contact_role VARCHAR(50) NULL,
    email VARCHAR(255) NULL,
    phone VARCHAR(50) NULL,
    is_primary TINYINT(1) NOT NULL DEFAULT 0,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (client_contact_id),
    KEY idx_client_contacts_client (client_id),
    KEY idx_client_contacts_primary (client_id, is_primary),
    CONSTRAINT fk_client_contacts_client FOREIGN KEY (client_id) REFERENCES clients(client_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Vehicle taxonomy and vehicles
-- =========================================================

CREATE TABLE vehicle_types (
    vehicle_type_id CHAR(36) NOT NULL,
    type_code VARCHAR(30) NOT NULL,
    type_name VARCHAR(100) NOT NULL,
    description TEXT NULL,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (vehicle_type_id),
    UNIQUE KEY uk_vehicle_types_code (type_code)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE vehicle_type_system_weights (
    vehicle_type_system_weight_id CHAR(36) NOT NULL,
    vehicle_type_id CHAR(36) NOT NULL,
    system_number INT NOT NULL,
    system_code VARCHAR(30) NOT NULL,
    system_key VARCHAR(50) NOT NULL,
    system_name VARCHAR(100) NOT NULL,
    weight DECIMAL(9,6) NOT NULL,
    is_critical_system TINYINT(1) NOT NULL DEFAULT 0,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    effective_from_utc DATETIME(6) NULL,
    effective_to_utc DATETIME(6) NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (vehicle_type_system_weight_id),
    UNIQUE KEY uk_vehicle_type_system_weights_type_system (vehicle_type_id, system_number),
    KEY idx_vehicle_type_system_weights_vehicle_type (vehicle_type_id),
    KEY idx_vehicle_type_system_weights_system_key (system_key),
    CONSTRAINT fk_vehicle_type_system_weights_vehicle_type FOREIGN KEY (vehicle_type_id) REFERENCES vehicle_types(vehicle_type_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE vehicles (
    vehicle_id CHAR(36) NOT NULL,
    vehicle_type_id CHAR(36) NOT NULL,
    vin VARCHAR(50) NOT NULL,
    year INT NOT NULL,
    make VARCHAR(100) NOT NULL,
    model VARCHAR(100) NOT NULL,
    trim VARCHAR(100) NULL,
    series_name VARCHAR(100) NULL,
    body_style VARCHAR(100) NULL,
    drivetrain VARCHAR(100) NULL,
    engine VARCHAR(150) NULL,
    transmission VARCHAR(150) NULL,
    mileage INT NULL,
    odometer_unit varchar(20) NULL,
    exterior_color VARCHAR(100) NULL,
    interior_color VARCHAR(100) NULL,
    title_status VARCHAR(50) NULL,
    authenticity_profile VARCHAR(100) NULL,
    restoration_level VARCHAR(100) NULL,
    documentation_level VARCHAR(100) NULL,
    overall_condition_score DECIMAL(4,2) NULL,
    has_red_flags BOOLEAN NOT NULL DEFAULT FALSE,
    notes TEXT NULL,
    ownership_notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (vehicle_id),
    UNIQUE KEY uq_vehicles_vin (vin),
    KEY idx_vehicles_make (make),
    KEY idx_vehicles_year (year),
    KEY idx_vehicles_vehicle_type (vehicle_type_id),
    CONSTRAINT fk_vehicles_vehicle_type FOREIGN KEY (vehicle_type_id) REFERENCES vehicle_types(vehicle_type_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Inspection templates
-- =========================================================

CREATE TABLE inspection_templates (
    inspection_template_id CHAR(36) NOT NULL,
    template_code VARCHAR(50) NOT NULL,
    template_family VARCHAR(30) NOT NULL,
    template_name VARCHAR(150) NOT NULL,
    version_label VARCHAR(30) NOT NULL,
    description TEXT NULL,
    total_system_count INT NOT NULL DEFAULT 12,
    status VARCHAR(30) NOT NULL,
    effective_from_utc DATETIME(6) NULL,
    effective_to_utc DATETIME(6) NULL,
    created_by_user_id CHAR(36) NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_template_id),
    KEY idx_inspection_templates_code (template_code),
    KEY idx_inspection_templates_family (template_family),
    KEY idx_inspection_templates_created_by (created_by_user_id),
    CONSTRAINT fk_inspection_templates_created_by FOREIGN KEY (created_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE inspection_template_sections (
    inspection_template_section_id CHAR(36) NOT NULL,
    inspection_template_id CHAR(36) NOT NULL,
    system_number INT NOT NULL,
    system_key VARCHAR(50) NOT NULL,
    display_name VARCHAR(150) NOT NULL,
    description TEXT NULL,
    weight DECIMAL(9,6) NOT NULL,
    sort_order INT NOT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_template_section_id),
    UNIQUE KEY uq_template_sections_template_system (inspection_template_id, system_number),
    KEY idx_template_sections_template (inspection_template_id),
    KEY idx_template_sections_system_key (system_key),
    CONSTRAINT fk_template_sections_template FOREIGN KEY (inspection_template_id) REFERENCES inspection_templates(inspection_template_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE inspection_template_items (
    inspection_template_item_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NOT NULL,
    item_key VARCHAR(80) NOT NULL,
    display_name VARCHAR(200) NOT NULL,
    help_text TEXT NULL,
    response_type VARCHAR(30) NOT NULL,
    default_weight DECIMAL(9,6) NULL,
    allows_red_flag TINYINT(1) NOT NULL DEFAULT 1,
    is_required TINYINT(1) NOT NULL DEFAULT 0,
    sort_order INT NOT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_template_item_id),
    UNIQUE KEY uq_template_items_section_key (inspection_template_section_id, item_key),
    KEY idx_template_items_section (inspection_template_section_id),
    CONSTRAINT fk_template_items_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Appraisal intake and appraisals
-- =========================================================

CREATE TABLE appraisal_requests (
    appraisal_request_id CHAR(36) NOT NULL,
    request_number VARCHAR(50) NOT NULL,
    client_id CHAR(36) NOT NULL,
    requested_by_contact_id CHAR(36) NULL,
    created_appraisal_id CHAR(36) NULL,
    requested_service VARCHAR(50) NOT NULL,
    request_channel VARCHAR(30) NULL,
    request_status VARCHAR(30) NOT NULL,
    intake_notes TEXT NULL,
    requested_on_utc DATETIME(6) NOT NULL,
    target_due_date_utc DATETIME(6) NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (appraisal_request_id),
    UNIQUE KEY uq_appraisal_requests_number (request_number),
    KEY idx_appraisal_requests_client (client_id),
    KEY idx_appraisal_requests_status (request_status),
    KEY fk_appraisal_requests_contact (requested_by_contact_id),
    KEY fk_appraisal_requests_created_appraisal (created_appraisal_id),
    CONSTRAINT fk_appraisal_requests_client FOREIGN KEY (client_id) REFERENCES clients(client_id),
    CONSTRAINT fk_appraisal_requests_contact FOREIGN KEY (requested_by_contact_id) REFERENCES client_contacts(client_contact_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE appraisals (
    appraisal_id CHAR(36) NOT NULL,
    appraisal_number VARCHAR(50) NULL,
    appraisal_request_id CHAR(36) NULL,
    vehicle_id CHAR(36) NOT NULL,
    client_id CHAR(36) NULL,
    assigned_appraiser_id CHAR(36) NULL,
    assigned_reviewer_id CHAR(36) NULL,
    appraisal_type VARCHAR(50) NOT NULL,
    status VARCHAR(30) NOT NULL,
    intake_status VARCHAR(30) NULL,
    inspection_status VARCHAR(30) NULL,
    valuation_status VARCHAR(30) NULL,
    report_status VARCHAR(30) NULL,
    effective_date_utc DATETIME(6) NULL,
    row_version BIGINT NOT NULL DEFAULT 1,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (appraisal_id),
    UNIQUE KEY uq_appraisals_number (appraisal_number),
    KEY idx_appraisals_vehicle (vehicle_id),
    KEY idx_appraisals_client (client_id),
    KEY idx_appraisals_status (status),
    KEY fk_appraisals_request (appraisal_request_id),
    KEY fk_appraisals_assigned_appraiser (assigned_appraiser_id),
    KEY fk_appraisals_assigned_reviewer (assigned_reviewer_id),
    CONSTRAINT fk_appraisals_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id),
    CONSTRAINT fk_appraisals_client FOREIGN KEY (client_id) REFERENCES clients(client_id),
    CONSTRAINT fk_appraisals_request FOREIGN KEY (appraisal_request_id) REFERENCES appraisal_requests(appraisal_request_id),
    CONSTRAINT fk_appraisals_assigned_appraiser FOREIGN KEY (assigned_appraiser_id) REFERENCES users(user_id),
    CONSTRAINT fk_appraisals_assigned_reviewer FOREIGN KEY (assigned_reviewer_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE appraisal_requests
    ADD CONSTRAINT fk_appraisal_requests_created_appraisal
    FOREIGN KEY (created_appraisal_id) REFERENCES appraisals(appraisal_id);

CREATE TABLE appraisal_assignments (
    appraisal_assignment_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    user_id CHAR(36) NOT NULL,
    assignment_role VARCHAR(30) NOT NULL,
    assignment_status VARCHAR(30) NOT NULL,
    assigned_at_utc DATETIME(6) NOT NULL,
    released_at_utc DATETIME(6) NULL,
    notes TEXT NULL,
    PRIMARY KEY (appraisal_assignment_id),
    KEY idx_appraisal_assignments_appraisal (appraisal_id),
    KEY idx_appraisal_assignments_user (user_id),
    KEY idx_appraisal_assignments_active (appraisal_id, assignment_status),
    CONSTRAINT fk_appraisal_assignments_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_appraisal_assignments_user FOREIGN KEY (user_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Inspections and scoring
-- =========================================================

CREATE TABLE inspections (
    inspection_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    inspection_template_id CHAR(36) NOT NULL,
    inspected_by_user_id CHAR(36) NULL,
    inspection_type VARCHAR(30) NOT NULL,
    status VARCHAR(30) NOT NULL,
    inspection_location VARCHAR(255) NULL,
    inspected_on_utc DATETIME(6) NULL,
    summary_notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_id),
    KEY idx_inspections_appraisal (appraisal_id),
    KEY idx_inspections_template (inspection_template_id),
    KEY idx_inspections_user (inspected_by_user_id),
    KEY idx_inspections_status (status),
    CONSTRAINT fk_inspections_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_inspections_template FOREIGN KEY (inspection_template_id) REFERENCES inspection_templates(inspection_template_id),
    CONSTRAINT fk_inspections_user FOREIGN KEY (inspected_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE inspection_section_scores (
    inspection_section_score_id CHAR(36) NOT NULL,
    inspection_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NOT NULL,
    system_number INT NOT NULL,
    system_key VARCHAR(50) NOT NULL,
    score_0_to_5 DECIMAL(4,2) NOT NULL,
    normalized_score_0_to_100 DECIMAL(6,2) NULL,
    weight DECIMAL(9,6) NOT NULL,
    weighted_score DECIMAL(12,6) NULL,
    red_flag_count INT NOT NULL DEFAULT 0,
    notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_section_score_id),
    UNIQUE KEY uq_section_scores_inspection_system (inspection_id, system_number),
    KEY idx_section_scores_appraisal (appraisal_id),
    KEY idx_section_scores_inspection (inspection_id),
    KEY idx_section_scores_section (inspection_template_section_id),
    CONSTRAINT fk_section_scores_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_section_scores_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_section_scores_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE inspection_item_scores (
    inspection_item_score_id CHAR(36) NOT NULL,
    inspection_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NOT NULL,
    inspection_template_item_id CHAR(36) NOT NULL,
    system_number INT NOT NULL,
    item_key VARCHAR(80) NOT NULL,
    score_0_to_5 DECIMAL(4,2) NULL,
    boolean_response TINYINT(1) NULL,
    numeric_response DECIMAL(18,4) NULL,
    text_response TEXT NULL,
    is_red_flag TINYINT(1) NOT NULL DEFAULT 0,
    evidence_note TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_item_score_id),
    KEY idx_item_scores_appraisal (appraisal_id),
    KEY idx_item_scores_inspection (inspection_id),
    KEY idx_item_scores_section (inspection_template_section_id),
    KEY idx_item_scores_item (inspection_template_item_id),
    CONSTRAINT fk_item_scores_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_item_scores_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_item_scores_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id),
    CONSTRAINT fk_item_scores_item FOREIGN KEY (inspection_template_item_id) REFERENCES inspection_template_items(inspection_template_item_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE inspection_media (
    inspection_media_id CHAR(36) NOT NULL,
    inspection_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NULL,
    inspection_template_item_id CHAR(36) NULL,
    media_type VARCHAR(20) NOT NULL,
    storage_provider VARCHAR(50) NOT NULL,
    storage_path VARCHAR(500) NULL,
    file_name VARCHAR(255) NOT NULL,
    file_extension VARCHAR(20) NULL,
    mime_type VARCHAR(100) NULL,
    caption VARCHAR(255) NULL,
    content_hash VARCHAR(128) NULL,
    sort_order INT NOT NULL DEFAULT 0,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_media_id),
    KEY idx_inspection_media_appraisal (appraisal_id),
    KEY idx_inspection_media_inspection (inspection_id),
    KEY idx_inspection_media_section (inspection_template_section_id),
    KEY idx_inspection_media_item (inspection_template_item_id),
    CONSTRAINT fk_inspection_media_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_inspection_media_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_inspection_media_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id),
    CONSTRAINT fk_inspection_media_item FOREIGN KEY (inspection_template_item_id) REFERENCES inspection_template_items(inspection_template_item_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE inspection_red_flags (
    inspection_red_flag_id CHAR(36) NOT NULL,
    inspection_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    inspection_template_section_id CHAR(36) NULL,
    inspection_template_item_id CHAR(36) NULL,
    system_number INT NULL,
    red_flag_code VARCHAR(50) NULL,
    title VARCHAR(150) NOT NULL,
    description TEXT NULL,
    severity VARCHAR(20) NOT NULL,
    penalty_factor DECIMAL(8,4) NULL,
    requires_manual_review TINYINT(1) NOT NULL DEFAULT 0,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_red_flag_id),
    KEY idx_red_flags_appraisal (appraisal_id),
    KEY idx_red_flags_inspection (inspection_id),
    KEY idx_red_flags_section (inspection_template_section_id),
    KEY idx_red_flags_item (inspection_template_item_id),
    KEY idx_red_flags_severity (severity),
    CONSTRAINT fk_red_flags_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_red_flags_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id),
    CONSTRAINT fk_red_flags_section FOREIGN KEY (inspection_template_section_id) REFERENCES inspection_template_sections(inspection_template_section_id),
    CONSTRAINT fk_red_flags_item FOREIGN KEY (inspection_template_item_id) REFERENCES inspection_template_items(inspection_template_item_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE condition_scores (
    condition_score_id CHAR(36) NOT NULL,
    inspection_id CHAR(36) NOT NULL,
    system_number INT NOT NULL,
    system_key VARCHAR(50) NOT NULL,
    score_0_to_5 DECIMAL(3,2) NOT NULL,
    weight DECIMAL(6,4) NOT NULL,
    is_red_flag TINYINT(1) NOT NULL DEFAULT 0,
    notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (condition_score_id),
    UNIQUE KEY uk_inspection_system (inspection_id, system_number),
    KEY idx_inspection (inspection_id),
    CONSTRAINT fk_condition_scores_inspection FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Comparables
-- =========================================================

CREATE TABLE comparable_sales (
    comparable_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    year INT NOT NULL,
    make VARCHAR(100) NOT NULL,
    model VARCHAR(100) NOT NULL,
    trim VARCHAR(100) NULL,
    series VARCHAR(100) NULL,
    body_style VARCHAR(100) NULL,
    engine VARCHAR(150) NULL,
    transmission VARCHAR(150) NULL,
    drivetrain VARCHAR(100) NULL,
    mileage INT NULL,
    exterior_color VARCHAR(100) NULL,
    interior_color VARCHAR(100) NULL,
    originality_level VARCHAR(50) NULL,
    restoration_level VARCHAR(50) NULL,
    matching_numbers VARCHAR(30) NULL,
    documentation_level VARCHAR(50) NULL,
    title_status VARCHAR(50) NULL,
    source VARCHAR(100) NOT NULL,
    source_listing_url VARCHAR(500) NULL,
    venue_type VARCHAR(50) NULL,
    sale_type VARCHAR(30) NULL,
    sale_date_utc DATETIME(6) NULL,
    sale_price DECIMAL(18,2) NULL,
    asking_price DECIMAL(18,2) NULL,
    currency_code VARCHAR(10) NOT NULL DEFAULT 'USD',
    location_city VARCHAR(100) NULL,
    location_state VARCHAR(100) NULL,
    overall_condition_score DECIMAL(6,2) NULL,
    condition_summary VARCHAR(255) NULL,
    authenticity_score DECIMAL(6,2) NULL,
    has_red_flags TINYINT(1) NOT NULL DEFAULT 0,
    notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (comparable_id),
    KEY idx_comparable_sales_appraisal (appraisal_id),
    KEY idx_comparable_sales_lookup (year, make, model),
    KEY idx_comparable_sales_sale_date (sale_date_utc),
    CONSTRAINT fk_comparable_sales_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE comparable_adjustments (
    comparable_adjustment_id CHAR(36) NOT NULL,
    comparable_id CHAR(36) NOT NULL,
    adjustment_category VARCHAR(50) NOT NULL,
    adjustment_key VARCHAR(80) NOT NULL,
    adjustment_delta DECIMAL(18,2) NOT NULL,
    adjustment_factor DECIMAL(12,6) NULL,
    rationale TEXT NULL,
    sort_order INT NOT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (comparable_adjustment_id),
    KEY idx_comparable_adjustments_comp (comparable_id),
    CONSTRAINT fk_comparable_adjustments_comp FOREIGN KEY (comparable_id) REFERENCES comparable_sales(comparable_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Valuation engine output and audit trail
-- =========================================================

CREATE TABLE valuation_runs (
    valuation_run_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    valuation_status VARCHAR(30) NOT NULL,
    engine_version VARCHAR(50) NOT NULL,
    rubric_version VARCHAR(50) NOT NULL,
    ruleset_version VARCHAR(50) NOT NULL,
    condition_index DECIMAL(12,6) NULL,
    market_factor DECIMAL(12,6) NULL,
    red_flag_factor DECIMAL(12,6) NULL,
    confidence_score DECIMAL(12,6) NULL,
    confidence_band VARCHAR(20) NULL,
    value_low DECIMAL(18,2) NULL,
    final_value DECIMAL(18,2) NULL,
    value_high DECIMAL(18,2) NULL,
    manual_review_required TINYINT(1) NOT NULL DEFAULT 0,
    run_notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (valuation_run_id),
    KEY idx_valuation_runs_appraisal (appraisal_id),
    KEY idx_valuation_runs_status (valuation_status),
    KEY idx_valuation_runs_created_at_utc (created_at_utc),
    CONSTRAINT fk_valuation_runs_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE valuation_comparable_links (
    valuation_comparable_link_id CHAR(36) NOT NULL,
    valuation_run_id CHAR(36) NOT NULL,
    comparable_id CHAR(36) NOT NULL,
    selection_rank INT NOT NULL,
    similarity_score DECIMAL(12,6) NULL,
    weighting_factor DECIMAL(12,6) NULL,
    adjusted_price DECIMAL(18,2) NULL,
    was_included_in_median TINYINT(1) NOT NULL DEFAULT 0,
    notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (valuation_comparable_link_id),
    UNIQUE KEY uq_valuation_comparable_links_run_comp (valuation_run_id, comparable_id),
    KEY idx_valuation_comparable_links_run (valuation_run_id),
    KEY idx_valuation_comparable_links_comp (comparable_id),
    CONSTRAINT fk_valuation_comparable_links_run FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id),
    CONSTRAINT fk_valuation_comparable_links_comp FOREIGN KEY (comparable_id) REFERENCES comparable_sales(comparable_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE valuation_trace_lines (
    valuation_trace_line_id CHAR(36) NOT NULL,
    valuation_run_id CHAR(36) NOT NULL,
    sort_order INT NOT NULL,
    trace_type VARCHAR(30) NOT NULL,
    trace_key VARCHAR(80) NOT NULL,
    amount DECIMAL(18,4) NULL,
    factor DECIMAL(18,6) NULL,
    message VARCHAR(1000) NULL,
    evidence_ref VARCHAR(255) NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (valuation_trace_line_id),
    KEY idx_valuation_trace_lines_run (valuation_run_id),
    UNIQUE KEY uq_valuation_trace_lines_order (valuation_run_id, sort_order),
    CONSTRAINT fk_valuation_trace_lines_run FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Reports and exports
-- =========================================================

CREATE TABLE appraisal_reports (
    appraisal_report_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    valuation_run_id CHAR(36) NULL,
    report_type VARCHAR(50) NOT NULL,
    report_version VARCHAR(50) NOT NULL,
    template_name VARCHAR(150) NULL,
    report_status VARCHAR(30) NOT NULL,
    narrative_snapshot MEDIUMTEXT NULL,
    generated_by_user_id CHAR(36) NULL,
    generated_on_utc DATETIME(6) NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (appraisal_report_id),
    KEY idx_appraisal_reports_appraisal (appraisal_id),
    KEY idx_appraisal_reports_run (valuation_run_id),
    KEY fk_appraisal_reports_user (generated_by_user_id),
    CONSTRAINT fk_appraisal_reports_appraisal FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_appraisal_reports_run FOREIGN KEY (valuation_run_id) REFERENCES valuation_runs(valuation_run_id),
    CONSTRAINT fk_appraisal_reports_user FOREIGN KEY (generated_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE report_exports (
    report_export_id CHAR(36) NOT NULL,
    appraisal_report_id CHAR(36) NOT NULL,
    export_format VARCHAR(20) NOT NULL,
    export_status VARCHAR(30) NOT NULL,
    storage_provider VARCHAR(50) NOT NULL,
    storage_path VARCHAR(500) NULL,
    file_name VARCHAR(255) NOT NULL,
    file_size_bytes BIGINT NULL,
    checksum_sha256 VARCHAR(64) NULL,
    exported_by_user_id CHAR(36) NULL,
    exported_on_utc DATETIME(6) NOT NULL,
    PRIMARY KEY (report_export_id),
    KEY idx_report_exports_report (appraisal_report_id),
    KEY idx_report_exports_user (exported_by_user_id),
    CONSTRAINT fk_report_exports_report FOREIGN KEY (appraisal_report_id) REFERENCES appraisal_reports(appraisal_report_id),
    CONSTRAINT fk_report_exports_user FOREIGN KEY (exported_by_user_id) REFERENCES users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================================================
-- Core views
-- =========================================================

CREATE VIEW vw_appraisal_latest_valuation AS
SELECT
    vr.appraisal_id,
    vr.valuation_run_id,
    vr.valuation_status,
    vr.engine_version,
    vr.rubric_version,
    vr.ruleset_version,
    vr.condition_index,
    vr.market_factor,
    vr.red_flag_factor,
    vr.confidence_score,
    vr.confidence_band,
    vr.value_low,
    vr.final_value,
    vr.value_high,
    vr.manual_review_required,
    vr.run_notes,
    vr.created_at_utc
FROM valuation_runs vr
JOIN (
    SELECT appraisal_id, MAX(created_at_utc) AS max_created_at_utc
    FROM valuation_runs
    GROUP BY appraisal_id
) latest
    ON latest.appraisal_id = vr.appraisal_id
   AND latest.max_created_at_utc = vr.created_at_utc;

CREATE VIEW vw_inspection_section_rollup AS
SELECT
    inspection_id,
    appraisal_id,
    system_number,
    system_key,
    score_0_to_5,
    normalized_score_0_to_100,
    weight,
    weighted_score,
    red_flag_count,
    updated_at_utc
FROM inspection_section_scores;

CREATE VIEW vw_valuation_trace_report AS
SELECT
    vr.appraisal_id,
    vtl.valuation_run_id,
    vtl.sort_order,
    vtl.trace_type,
    vtl.trace_key,
    vtl.amount,
    vtl.factor,
    vtl.message,
    vtl.evidence_ref,
    vtl.created_at_utc
FROM valuation_trace_lines vtl
JOIN valuation_runs vr
    ON vr.valuation_run_id = vtl.valuation_run_id;

-- =========================================================
-- Seed data: locked vehicle types and Standard vehicle weights
-- =========================================================

INSERT INTO vehicle_types (vehicle_type_id, type_code, type_name, description, is_active)
VALUES
('00000000-0000-0000-0000-000000000101', 'STANDARD', 'Standard Vehicle', 'Production vehicle with OEM-oriented appraisal behavior.', 1),
('00000000-0000-0000-0000-000000000102', 'CUSTOM',   'Custom Vehicle',   'Modified, custom-built, or build-driven appraisal behavior.', 1),
('00000000-0000-0000-0000-000000000103', 'CLASSIC',  'Classic Vehicle',  'Collector or historically significant appraisal behavior.', 1);

INSERT INTO vehicle_type_system_weights (
    vehicle_type_system_weight_id, vehicle_type_id, system_number, system_code, system_key,
    system_name, weight, is_critical_system, is_active, effective_from_utc
)
VALUES
(UUID(), '00000000-0000-0000-0000-000000000101',  1, 'BODY_EXT',     'body_exterior',             'Body & Exterior',             0.060000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  2, 'FRAME_STRUCT', 'frame_structure',           'Frame & Structure',           0.100000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  3, 'POWERTRAIN',   'powertrain',                'Engine / Powertrain',         0.180000, 1, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  4, 'TRANS_DRIVE',  'transmission_driveline',    'Transmission & Driveline',    0.140000, 1, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  5, 'SUS_STEER',    'suspension_steering',       'Suspension & Steering',       0.100000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  6, 'BRAKE_SYS',    'braking_system',            'Braking System',              0.100000, 1, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  7, 'ELEC_SYS',     'electrical_system',         'Electrical System',           0.100000, 1, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  8, 'INTERIOR',     'interior',                  'Interior',                    0.040000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101',  9, 'WHEEL_TIRE',   'wheels_tires',              'Wheels & Tires',              0.060000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101', 10, 'TRIM_GLASS',   'trim_glass_details',        'Trim, Glass & Details',       0.020000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101', 11, 'DOC_PROV',     'documentation_provenance',  'Documentation & Provenance',  0.020000, 0, 1, CURRENT_TIMESTAMP(6)),
(UUID(), '00000000-0000-0000-0000-000000000101', 12, 'ROAD_FUNC',    'road_test_functional',      'Road Test & Functional',      0.080000, 1, 1, CURRENT_TIMESTAMP(6));

-- Validation helpers:
-- SELECT type_code FROM vehicle_types ORDER BY type_code;
-- SELECT vt.type_code, SUM(w.weight) AS total_weight
-- FROM vehicle_type_system_weights w
-- JOIN vehicle_types vt ON vt.vehicle_type_id = w.vehicle_type_id
-- GROUP BY vt.type_code;
