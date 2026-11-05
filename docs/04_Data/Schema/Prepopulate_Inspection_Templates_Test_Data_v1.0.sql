/*
  ONE Classics Nexus
  Prepopulate_Inspection_Templates_Test_Data_v1.0.sql

  Purpose:
    Populate inspection_templates table with standard test templates for vehicle inspection workflows.
    This includes the primary Standard template and alternative templates for different inspection scenarios.

  Notes:
    - Designed for MariaDB/MySQL.
    - Uses CHAR(36) GUID identifiers.
    - Depends on users table being populated first for created_by_user_id reference.
    - All templates follow the 12-system inspection model.
    - Templates can be in Draft, Active, or Archived status.

  Templates Included:
    1. Standard Inspection Template v1.0 (Primary) - Used for general vehicle appraisals
    2. Classic Car Inspection Template v1.0 - Specialized for classic and vintage vehicles
    3. Performance Vehicle Inspection Template v1.0 - Specialized for high-performance vehicles
    4. Fleet Inspection Template v1.0 - Streamlined for bulk vehicle assessments
*/

USE one_classics_nexus;

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
-- Primary Standard Inspection Template
(
    '00000000-0000-0000-0000-000000000001',
    'STD-V1.0',
    'STANDARD',
    'Standard Inspection Template',
    'v1.0',
    'Comprehensive 12-system inspection template for general vehicle appraisals. Covers all major vehicle systems with balanced weighting for condition assessment.',
    12,
    'Active',
    '2024-01-01 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Classic Car Inspection Template
(
    '00000000-0000-0000-0000-000000000002',
    'CLASSIC-V1.0',
    'CLASSIC',
    'Classic Car Inspection Template',
    'v1.0',
    'Specialized 12-system inspection template tailored for classic and vintage vehicles. Emphasizes original components, patina, and historical accuracy.',
    12,
    'Active',
    '2024-01-15 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Performance Vehicle Inspection Template
(
    '00000000-0000-0000-0000-000000000003',
    'PERF-V1.0',
    'PERFORMANCE',
    'Performance Vehicle Inspection Template',
    'v1.0',
    'Specialized 12-system inspection template for high-performance and sports vehicles. Focuses on performance systems, engine modifications, and track readiness.',
    12,
    'Active',
    '2024-01-20 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Fleet Inspection Template
(
    '00000000-0000-0000-0000-000000000004',
    'FLEET-V1.0',
    'FLEET',
    'Fleet Inspection Template',
    'v1.0',
    'Streamlined 12-system inspection template for bulk vehicle assessments and fleet operations. Optimized for quick and efficient condition evaluation.',
    12,
    'Active',
    '2024-01-25 00:00:00.000000',
    NULL,
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Archive: Previous Standard Template Version
(
    '00000000-0000-0000-0000-000000000005',
    'STD-V0.9',
    'STANDARD',
    'Standard Inspection Template',
    'v0.9',
    'Previous version of the standard inspection template. Maintained for historical reference and legacy appraisals.',
    12,
    'Archived',
    '2023-06-01 00:00:00.000000',
    '2024-01-01 00:00:00.000000',
    '00000000-0000-0000-0000-000000000101',
    NOW(6),
    NOW(6)
),

-- Draft: Work-in-Progress Commercial Inspection Template
(
    '00000000-0000-0000-0000-000000000006',
    'COMM-V1.0-DRAFT',
    'COMMERCIAL',
    'Commercial Vehicle Inspection Template',
    'v1.0 (Draft)',
    'Work-in-progress template for commercial and commercial-grade vehicles. Currently in review and refinement phase.',
    12,
    'Draft',
    NULL,
    NULL,
    '00000000-0000-0000-0000-000000000102',
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
