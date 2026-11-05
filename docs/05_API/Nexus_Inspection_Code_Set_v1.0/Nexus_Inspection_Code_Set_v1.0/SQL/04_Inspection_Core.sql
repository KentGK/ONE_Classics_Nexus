CREATE TABLE IF NOT EXISTS inspections (
    inspection_id CHAR(36) NOT NULL,
    appraisal_id CHAR(36) NOT NULL,
    inspection_template_id CHAR(36) NOT NULL,
    inspected_by_user_id CHAR(36) NULL,
    inspection_type VARCHAR(30) NOT NULL,
    status VARCHAR(30) NOT NULL,
    inspection_location VARCHAR(255) NULL,
    inspected_on_utc DATETIME(6) NULL,
    completion_percentage DECIMAL(5,2) NULL,
    overall_condition_score DECIMAL(5,2) NULL,
    has_red_flags BOOLEAN NOT NULL DEFAULT FALSE,
    summary_notes TEXT NULL,
    row_version BIGINT NOT NULL DEFAULT 1,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (inspection_id),
    KEY idx_inspections_appraisal_id (appraisal_id),
    KEY idx_inspections_template_id (inspection_template_id),
    KEY idx_inspections_status (status),
    CONSTRAINT fk_inspections_appraisals
        FOREIGN KEY (appraisal_id) REFERENCES appraisals(appraisal_id),
    CONSTRAINT fk_inspections_template
        FOREIGN KEY (inspection_template_id) REFERENCES inspection_templates(inspection_template_id),
    CONSTRAINT fk_inspections_user
        FOREIGN KEY (inspected_by_user_id) REFERENCES users(user_id)
);

CREATE TABLE IF NOT EXISTS condition_scores (
    condition_score_id CHAR(36) NOT NULL,
    inspection_id CHAR(36) NOT NULL,
    system_number INT NOT NULL,
    system_key VARCHAR(50) NOT NULL,
    score_0_to_5 DECIMAL(3,2) NOT NULL,
    weight DECIMAL(6,4) NOT NULL,
    is_red_flag BOOLEAN NOT NULL DEFAULT FALSE,
    notes TEXT NULL,
    created_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at_utc DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (condition_score_id),
    UNIQUE KEY ux_condition_scores_inspection_system (inspection_id, system_number),
    KEY idx_condition_scores_inspection_id (inspection_id),
    CONSTRAINT fk_condition_scores_inspections
        FOREIGN KEY (inspection_id) REFERENCES inspections(inspection_id)
        ON DELETE CASCADE,
    CONSTRAINT chk_condition_scores_system_number
        CHECK (system_number BETWEEN 1 AND 12),
    CONSTRAINT chk_condition_scores_score
        CHECK (score_0_to_5 >= 0.00 AND score_0_to_5 <= 5.00),
    CONSTRAINT chk_condition_scores_weight
        CHECK (weight >= 0.0000 AND weight <= 1.0000)
);
