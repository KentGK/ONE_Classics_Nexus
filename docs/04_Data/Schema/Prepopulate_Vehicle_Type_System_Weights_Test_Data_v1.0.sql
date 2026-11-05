/*
    ONE Classics Nexus
    Script: Nexus_Prepopulate_Vehicle_Type_System_Weights_Test_Data_v1.0.sql
    Purpose: Pre-populate vehicle_type_system_weights with deterministic test data
             for Standard, Custom, and Classic vehicle types.
    Database: MariaDB / MySQL

    Notes:
    - This script assumes the vehicle_types table already exists and contains
      vehicle type rows for Standard, Custom, and Classic.
    - For Classic, this script also accepts existing vehicle_types names such as
      Vintage/Classic, VintageClassic, or Vintage Classic.
    - The script resolves vehicle_type_id from vehicle_types using the first
      available name/code column found in this order:
        vehicle_type_name, type_name, name, vehicle_type, vehicle_type_code, code
    - Uses ON DUPLICATE KEY UPDATE so the script can be rerun safely.
    - No DELETE statements are included.
    - The 12-system weights are expected to total 1.000000 per vehicle type.

    Optional:
    -- USE one_classics_nexus_test;
*/

DROP TEMPORARY TABLE IF EXISTS tmp_vehicle_type_lookup;
DROP TEMPORARY TABLE IF EXISTS tmp_vehicle_type_lookup_terms;
DROP TEMPORARY TABLE IF EXISTS tmp_vehicle_type_system_weights_seed;

CREATE TEMPORARY TABLE tmp_vehicle_type_lookup
(
    vehicle_type_label VARCHAR(30) NOT NULL PRIMARY KEY,
    vehicle_type_id CHAR(36) NULL
);

CREATE TEMPORARY TABLE tmp_vehicle_type_lookup_terms
(
    vehicle_type_label VARCHAR(30) NOT NULL,
    lookup_value VARCHAR(100) NOT NULL,
    KEY idx_tmp_vehicle_type_lookup_terms_label (vehicle_type_label),
    KEY idx_tmp_vehicle_type_lookup_terms_value (lookup_value)
);

INSERT INTO tmp_vehicle_type_lookup (vehicle_type_label, vehicle_type_id)
VALUES
    ('Standard', NULL),
    ('Custom', NULL),
    ('Classic', NULL);

INSERT INTO tmp_vehicle_type_lookup_terms (vehicle_type_label, lookup_value)
VALUES
    ('Standard', 'Standard'),
    ('Standard', 'STANDARD'),
    ('Standard', 'STD'),
    ('Custom', 'Custom'),
    ('Custom', 'CUSTOM'),
    ('Classic', 'Classic'),
    ('Classic', 'CLASSIC'),
    ('Classic', 'Vintage/Classic'),
    ('Classic', 'VintageClassic'),
    ('Classic', 'Vintage Classic'),
    ('Classic', 'VINTAGE_CLASSIC');

/*
    Resolve the vehicle_types lookup column dynamically so the script can support
    common schema variants without editing the seed data below.
*/
SET @vehicle_type_lookup_column :=
(
    SELECT c.column_name
    FROM information_schema.columns c
    WHERE c.table_schema = DATABASE()
      AND c.table_name = 'vehicle_types'
      AND c.column_name IN
      (
          'vehicle_type_name',
          'type_name',
          'name',
          'vehicle_type',
          'vehicle_type_code',
          'code'
      )
    ORDER BY FIELD
    (
        c.column_name,
        'vehicle_type_name',
        'type_name',
        'name',
        'vehicle_type',
        'vehicle_type_code',
        'code'
    )
    LIMIT 1
);

SET @resolve_vehicle_type_sql := IF
(
    @vehicle_type_lookup_column IS NULL,
    'UPDATE tmp_vehicle_type_lookup SET vehicle_type_id = NULL',
    CONCAT
    (
        'UPDATE tmp_vehicle_type_lookup l ',
        'JOIN ( ',
        '    SELECT t.vehicle_type_label, MIN(vt.vehicle_type_id) AS vehicle_type_id ',
        '    FROM tmp_vehicle_type_lookup_terms t ',
        '    JOIN vehicle_types vt ',
        '      ON LOWER(TRIM(CAST(vt.`', REPLACE(@vehicle_type_lookup_column, '`', '``'), '` AS CHAR))) = LOWER(TRIM(t.lookup_value)) ',
        '    GROUP BY t.vehicle_type_label ',
        ') resolved ON resolved.vehicle_type_label = l.vehicle_type_label ',
        'SET l.vehicle_type_id = resolved.vehicle_type_id'
    )
);

PREPARE resolve_vehicle_type_stmt FROM @resolve_vehicle_type_sql;
EXECUTE resolve_vehicle_type_stmt;
DEALLOCATE PREPARE resolve_vehicle_type_stmt;

CREATE TEMPORARY TABLE tmp_vehicle_type_system_weights_seed
(
    vehicle_type_system_weight_id CHAR(36) NOT NULL PRIMARY KEY,
    vehicle_type_label VARCHAR(30) NOT NULL,
    system_number INT NOT NULL,
    system_code VARCHAR(30) NOT NULL,
    system_key VARCHAR(50) NOT NULL,
    system_name VARCHAR(100) NOT NULL,
    weight DECIMAL(9,6) NOT NULL,
    is_critical_system TINYINT(1) NOT NULL DEFAULT 0,
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    effective_from_utc DATETIME(6) NULL,
    effective_to_utc DATETIME(6) NULL,
    KEY idx_tmp_vtsw_seed_vehicle_type_label (vehicle_type_label),
    KEY idx_tmp_vtsw_seed_system_key (system_key)
);

INSERT INTO tmp_vehicle_type_system_weights_seed
(
    vehicle_type_system_weight_id,
    vehicle_type_label,
    system_number,
    system_code,
    system_key,
    system_name,
    weight,
    is_critical_system,
    is_active,
    effective_from_utc,
    effective_to_utc
)
VALUES
/*
    Standard vehicle weights. Total = 1.000000.
*/
('71000000-0000-0000-0000-000000000001', 'Standard',  1, 'S01_BODY',  'body_exterior',              'Body & Exterior',              0.060000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000002', 'Standard',  2, 'S02_FRAME', 'frame_structure',            'Frame & Structure',            0.100000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000003', 'Standard',  3, 'S03_PWR',   'powertrain',                 'Engine / Powertrain',          0.180000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000004', 'Standard',  4, 'S04_DRIVE', 'transmission_driveline',     'Transmission & Driveline',     0.140000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000005', 'Standard',  5, 'S05_SUSP',  'suspension_steering',        'Suspension & Steering',        0.100000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000006', 'Standard',  6, 'S06_BRAKE', 'braking_system',             'Braking System',               0.100000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000007', 'Standard',  7, 'S07_ELEC',  'electrical_system',          'Electrical System',            0.100000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000008', 'Standard',  8, 'S08_INT',   'interior',                   'Interior',                     0.040000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000009', 'Standard',  9, 'S09_WHEEL', 'wheels_tires',               'Wheels & Tires',               0.060000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000010', 'Standard', 10, 'S10_TRIM',  'trim_glass_details',         'Trim, Glass & Details',        0.020000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000011', 'Standard', 11, 'S11_DOC',   'documentation_provenance',   'Documentation & Provenance',   0.040000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('71000000-0000-0000-0000-000000000012', 'Standard', 12, 'S12_ROAD',  'road_test_functional',       'Road Test & Functional',       0.060000, 1, 1, '2026-01-01 00:00:00.000000', NULL),

/*
    Custom vehicle weights. Total = 1.000000.
*/
('72000000-0000-0000-0000-000000000001', 'Custom',    1, 'S01_BODY',  'body_exterior',              'Body & Exterior',              0.150000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000002', 'Custom',    2, 'S02_FRAME', 'frame_structure',            'Frame & Structure',            0.120000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000003', 'Custom',    3, 'S03_PWR',   'powertrain',                 'Engine / Powertrain',          0.160000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000004', 'Custom',    4, 'S04_DRIVE', 'transmission_driveline',     'Transmission & Driveline',     0.100000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000005', 'Custom',    5, 'S05_SUSP',  'suspension_steering',        'Suspension & Steering',        0.080000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000006', 'Custom',    6, 'S06_BRAKE', 'braking_system',             'Braking System',               0.060000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000007', 'Custom',    7, 'S07_ELEC',  'electrical_system',          'Electrical System',            0.080000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000008', 'Custom',    8, 'S08_INT',   'interior',                   'Interior',                     0.080000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000009', 'Custom',    9, 'S09_WHEEL', 'wheels_tires',               'Wheels & Tires',               0.050000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000010', 'Custom',   10, 'S10_TRIM',  'trim_glass_details',         'Trim, Glass & Details',        0.050000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000011', 'Custom',   11, 'S11_DOC',   'documentation_provenance',   'Documentation & Provenance',   0.030000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('72000000-0000-0000-0000-000000000012', 'Custom',   12, 'S12_ROAD',  'road_test_functional',       'Road Test & Functional',       0.040000, 1, 1, '2026-01-01 00:00:00.000000', NULL),

/*
    Classic vehicle weights. Total = 1.000000.
    These align to the Nexus Vintage/Classic weighting model.
*/
('73000000-0000-0000-0000-000000000001', 'Classic',   1, 'S01_BODY',  'body_exterior',              'Body & Exterior',              0.120000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000002', 'Classic',   2, 'S02_FRAME', 'frame_structure',            'Frame & Structure',            0.090000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000003', 'Classic',   3, 'S03_PWR',   'powertrain',                 'Engine / Powertrain',          0.130000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000004', 'Classic',   4, 'S04_DRIVE', 'transmission_driveline',     'Transmission & Driveline',     0.080000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000005', 'Classic',   5, 'S05_SUSP',  'suspension_steering',        'Suspension & Steering',        0.060000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000006', 'Classic',   6, 'S06_BRAKE', 'braking_system',             'Braking System',               0.050000, 1, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000007', 'Classic',   7, 'S07_ELEC',  'electrical_system',          'Electrical System',            0.050000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000008', 'Classic',   8, 'S08_INT',   'interior',                   'Interior',                     0.090000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000009', 'Classic',   9, 'S09_WHEEL', 'wheels_tires',               'Wheels & Tires',               0.040000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000010', 'Classic',  10, 'S10_TRIM',  'trim_glass_details',         'Trim, Glass & Details',        0.080000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000011', 'Classic',  11, 'S11_DOC',   'documentation_provenance',   'Documentation & Provenance',   0.120000, 0, 1, '2026-01-01 00:00:00.000000', NULL),
('73000000-0000-0000-0000-000000000012', 'Classic',  12, 'S12_ROAD',  'road_test_functional',       'Road Test & Functional',       0.090000, 1, 1, '2026-01-01 00:00:00.000000', NULL);

/*
    Dependency check: unresolved vehicle types will not be inserted.
    If any vehicle_type_id is NULL, run or repair the vehicle_types seed first.
*/
SELECT
    'Vehicle type dependency check' AS check_name,
    vehicle_type_label,
    vehicle_type_id,
    CASE
        WHEN vehicle_type_id IS NULL THEN 'MISSING - no matching vehicle_types row found'
        ELSE 'FOUND'
    END AS dependency_status
FROM tmp_vehicle_type_lookup
ORDER BY vehicle_type_label;

START TRANSACTION;

INSERT INTO vehicle_type_system_weights
(
    vehicle_type_system_weight_id,
    vehicle_type_id,
    system_number,
    system_code,
    system_key,
    system_name,
    weight,
    is_critical_system,
    is_active,
    effective_from_utc,
    effective_to_utc,
    created_at_utc,
    updated_at_utc
)
SELECT
    s.vehicle_type_system_weight_id,
    l.vehicle_type_id,
    s.system_number,
    s.system_code,
    s.system_key,
    s.system_name,
    s.weight,
    s.is_critical_system,
    s.is_active,
    s.effective_from_utc,
    s.effective_to_utc,
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
FROM tmp_vehicle_type_system_weights_seed s
JOIN tmp_vehicle_type_lookup l
  ON l.vehicle_type_label = s.vehicle_type_label
WHERE l.vehicle_type_id IS NOT NULL
ON DUPLICATE KEY UPDATE
    system_code = VALUES(system_code),
    system_key = VALUES(system_key),
    system_name = VALUES(system_name),
    weight = VALUES(weight),
    is_critical_system = VALUES(is_critical_system),
    is_active = VALUES(is_active),
    effective_from_utc = VALUES(effective_from_utc),
    effective_to_utc = VALUES(effective_to_utc),
    updated_at_utc = UTC_TIMESTAMP(6);

COMMIT;

/*
    Verification: Confirm seeded rows.
*/
SELECT
    l.vehicle_type_label,
    l.vehicle_type_id,
    w.system_number,
    w.system_code,
    w.system_key,
    w.system_name,
    w.weight,
    w.is_critical_system,
    w.is_active,
    w.effective_from_utc,
    w.effective_to_utc
FROM tmp_vehicle_type_lookup l
JOIN vehicle_type_system_weights w
  ON w.vehicle_type_id = l.vehicle_type_id
WHERE l.vehicle_type_id IS NOT NULL
ORDER BY
    l.vehicle_type_label,
    w.system_number;

/*
    Verification: Each vehicle type should have 12 active systems and total weight 1.000000.
*/
SELECT
    l.vehicle_type_label,
    COUNT(*) AS active_system_count,
    ROUND(SUM(w.weight), 6) AS active_weight_total,
    CASE
        WHEN COUNT(*) = 12 AND ROUND(SUM(w.weight), 6) = 1.000000 THEN 'PASS'
        ELSE 'CHECK'
    END AS validation_status
FROM tmp_vehicle_type_lookup l
JOIN vehicle_type_system_weights w
  ON w.vehicle_type_id = l.vehicle_type_id
WHERE l.vehicle_type_id IS NOT NULL
  AND w.is_active = 1
GROUP BY
    l.vehicle_type_label
ORDER BY
    l.vehicle_type_label;

/*
    Verification: Detect duplicate active rows by vehicle type and system number.
    This should return zero rows because uk_vehicle_type_system_weights_type_system
    protects vehicle_type_id + system_number.
*/
SELECT
    vehicle_type_id,
    system_number,
    COUNT(*) AS duplicate_count
FROM vehicle_type_system_weights
WHERE is_active = 1
GROUP BY
    vehicle_type_id,
    system_number
HAVING COUNT(*) > 1;

DROP TEMPORARY TABLE IF EXISTS tmp_vehicle_type_lookup;
DROP TEMPORARY TABLE IF EXISTS tmp_vehicle_type_lookup_terms;
DROP TEMPORARY TABLE IF EXISTS tmp_vehicle_type_system_weights_seed;
