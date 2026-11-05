/*
  ONE Classics Nexus
  inspection_templates_test_data_v1.3.sql

  Purpose:
    Seed test data for the `inspection_templates` table aligned to
    `Nexus_Inspection_Data_Model_v1.3.md` (inspection data model v1.3).

  Notes:
    - Target test schema: `one_classics_nexus_test` (DDL excerpts in v1.3 use this schema).
    - Uses CHAR(36) GUID identifiers consistent with application layer.
    - Depends on the `users` test data being present for `created_by_user_id` references.
    - Idempotent via ON DUPLICATE KEY UPDATE.
*/

USE one_classics_nexus_test;

INSERT INTO inspection_templates (
    inspection_template_id,
    template_code,
    template_family,
    template_name,
    version_label,
    description,
    total_system_count,
    status,
    effective_from_utc,
    effective_to_utc,
    created_by_user_id,
    created_at_utc,
    updated_at_utc
)
VALUES
-- Standard 12-system template (Active)
(
    '9cb93bbd-fd5e-4f7e-b28c-1125703d21fc',
    'STD-12SYS-V1',
    'Standard',
    'Standard 12-System Inspection',
    'v1.0',
    'Canonical 12-system template for general vehicle inspections.',
    12,
    'Active',
    '2026-01-01 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Vintage/Classic template (Active)
(
    'b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594',
    'VINTAGE_CLASSIC_12SYS_V1',
    'VintageClassic',
    'Vintage/Classic 12-System Inspection',
    'v1.0',
    'Template tailored to vintage and classic vehicles; emphasizes provenance and originality.',
    12,
    'Active',
    '2026-01-15 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Performance vehicle template (Active)
(
    '1f2b3c4d-5e6f-7081-92a3-b4c5d6e7f801',
    'PERF-12SYS-V1',
    'Performance',
    'Performance Vehicle 12-System Inspection',
    'v1.0',
    'Template optimized for performance and modified vehicles.',
    12,
    'Active',
    '2026-02-01 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Fleet template (Draft)
(
    '2a3b4c5d-6e7f-8091-12a3-b4c5d6e7f802',
    'FLEET-12SYS-V1-DRAFT',
    'Fleet',
    'Fleet Inspection Template (Draft)',
    'v1.0-draft',
    'Streamlined template for fleet assessments; currently in draft.',
    12,
    'Draft',
    NULL,
    NULL,
    '00000000-0000-0000-0000-000000000102',
    NOW(6),
    NOW(6)
),

-- Archived previous standard template
(
    '3c4d5e6f-7081-92a3-b4c5-d6e7f8010001',
    'STD-12SYS-V0.9',
    'Standard',
    'Standard 12-System Inspection',
    'v0.9',
    'Legacy version of the standard template retained for historical reference.',
    12,
    'Archived',
    '2023-06-01 00:00:00.000000',
    '2026-01-01 00:00:00.000000',
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
)
ON DUPLICATE KEY UPDATE
  template_code = VALUES(template_code),
  template_family = VALUES(template_family),
  template_name = VALUES(template_name),
  version_label = VALUES(version_label),
  description = VALUES(description),
  total_system_count = VALUES(total_system_count),
  status = VALUES(status),
  effective_from_utc = VALUES(effective_from_utc),
  effective_to_utc = VALUES(effective_to_utc),
  created_by_user_id = VALUES(created_by_user_id),
  updated_at_utc = NOW(6);
