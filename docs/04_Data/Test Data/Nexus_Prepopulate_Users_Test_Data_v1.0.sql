/*
================================================================================
ONE Classics Nexus
Users Test Data Seed Script
Version: 1.0
Target DB: one_classics_nexus or one_classics_nexus_test
DB Engine: MariaDB

Purpose:
  Pre-populates the users table with baseline test users for Nexus Swagger,
  API, WinForms, role-mapping, and workflow testing.

Users Included:
  Administrators:
    - admin@oneclassics.local
    - admin.operations@oneclassics.local
    - admin.qa@oneclassics.local

  Appraisers:
    - appraiser@oneclassics.local
    - appraiser.classic@oneclassics.local
    - appraiser.custom@oneclassics.local
    - appraiser.field@oneclassics.local

Execution Notes:
  1. Select the target database before running this script.
     Example:
       USE one_classics_nexus_test;
       SOURCE Nexus_Prepopulate_Users_Test_Data_v1.0.sql;

  2. Script is idempotent. Re-running it will not create duplicate users.

  3. This script only pre-populates users. Role assignment belongs in the
     normalized user_roles table.

Expected users table shape:
  users.user_id        CHAR(36) or VARCHAR(36)
  users.email          VARCHAR(...), unique preferred
  users.username       VARCHAR(...), nullable or required
  users.first_name     VARCHAR(...), nullable or required
  users.last_name      VARCHAR(...), nullable or required
  users.display_name   VARCHAR(...), nullable or required
  users.is_active      BOOLEAN/TINYINT, nullable or required
  users.created_at_utc DATETIME
  users.updated_at_utc DATETIME

If your current users table uses different column names, update the INSERT/UPDATE
column list below to match the locked schema.

Security Note:
  Password fields are intentionally omitted. These are structural test users for
  API/database workflow testing. Add password_hash/password_salt only if your
  authentication implementation requires them.
================================================================================
*/

START TRANSACTION;

/* -----------------------------------------------------------------------------
   Temporary source set for deterministic, repeatable seed data.
----------------------------------------------------------------------------- */
DROP TEMPORARY TABLE IF EXISTS tmp_nexus_users_seed;

CREATE TEMPORARY TABLE tmp_nexus_users_seed
(
    user_id      CHAR(36) NOT NULL,
    email        VARCHAR(255) NOT NULL,
    username     VARCHAR(100) NOT NULL,
    first_name   VARCHAR(100) NOT NULL,
    last_name    VARCHAR(100) NOT NULL,
    display_name VARCHAR(200) NOT NULL,
    test_role    VARCHAR(50) NOT NULL
);

INSERT INTO tmp_nexus_users_seed
(
    user_id,
    email,
    username,
    first_name,
    last_name,
    display_name,
    test_role
)
VALUES
    -- Administrators
    (
        '00000000-0000-0000-0000-000000000101',
        'admin@oneclassics.local',
        'admin.primary',
        'Alex',
        'Administrator',
        'Alex Administrator',
        'Administrator'
    ),
    (
        '00000000-0000-0000-0000-000000000102',
        'admin.operations@oneclassics.local',
        'admin.operations',
        'Morgan',
        'Operations',
        'Morgan Operations Admin',
        'Administrator'
    ),
    (
        '00000000-0000-0000-0000-000000000103',
        'admin.qa@oneclassics.local',
        'admin.qa',
        'Taylor',
        'Quality',
        'Taylor QA Admin',
        'Administrator'
    ),

    -- Appraisers
    (
        '00000000-0000-0000-0000-000000000201',
        'appraiser@oneclassics.local',
        'appraiser.primary',
        'Jordan',
        'Appraiser',
        'Jordan Appraiser',
        'Appraiser'
    ),
    (
        '00000000-0000-0000-0000-000000000202',
        'appraiser.classic@oneclassics.local',
        'appraiser.classic',
        'Casey',
        'Classic',
        'Casey Classic Appraiser',
        'Appraiser'
    ),
    (
        '00000000-0000-0000-0000-000000000203',
        'appraiser.custom@oneclassics.local',
        'appraiser.custom',
        'Riley',
        'Custom',
        'Riley Custom Appraiser',
        'Appraiser'
    ),
    (
        '00000000-0000-0000-0000-000000000204',
        'appraiser.field@oneclassics.local',
        'appraiser.field',
        'Avery',
        'Field',
        'Avery Field Appraiser',
        'Appraiser'
    );

/* -----------------------------------------------------------------------------
   Insert missing users.
----------------------------------------------------------------------------- */
INSERT INTO users
(
    user_id,
    email,
    username,
    first_name,
    last_name,
    display_name,
    is_active,
    created_at_utc,
    updated_at_utc
)
SELECT
    s.user_id,
    s.email,
    s.username,
    s.first_name,
    s.last_name,
    s.display_name,
    1,
    UTC_TIMESTAMP(),
    UTC_TIMESTAMP()
FROM tmp_nexus_users_seed s
WHERE NOT EXISTS
(
    SELECT 1
    FROM users u
    WHERE u.user_id = s.user_id
       OR u.email = s.email
);

/* -----------------------------------------------------------------------------
   Refresh existing seeded users to the current test-data baseline.
----------------------------------------------------------------------------- */
UPDATE users u
JOIN tmp_nexus_users_seed s
    ON s.email = u.email
SET
    u.username = s.username,
    u.first_name = s.first_name,
    u.last_name = s.last_name,
    u.display_name = s.display_name,
    u.is_active = 1,
    u.updated_at_utc = UTC_TIMESTAMP();

COMMIT;

/* -----------------------------------------------------------------------------
   Verification Query
----------------------------------------------------------------------------- */
SELECT
    u.user_id,
    u.email,
    u.username,
    u.first_name,
    u.last_name,
    u.display_name,
    u.is_active,
    s.test_role AS intended_test_role,
    u.created_at_utc,
    u.updated_at_utc
FROM users u
JOIN tmp_nexus_users_seed s
    ON s.email = u.email
ORDER BY
    s.test_role,
    u.email;

/* -----------------------------------------------------------------------------
   Optional Verification After Running user_roles Seed Script
   Uncomment this block after user_roles has been pre-populated.
----------------------------------------------------------------------------- */
/*
SELECT
    u.email,
    u.display_name,
    r.role_name,
    ur.created_at_utc AS role_assigned_at_utc
FROM user_roles ur
JOIN users u
    ON u.user_id = ur.user_id
JOIN roles r
    ON r.role_id = ur.role_id
WHERE u.email IN
(
    'admin@oneclassics.local',
    'admin.operations@oneclassics.local',
    'admin.qa@oneclassics.local',
    'appraiser@oneclassics.local',
    'appraiser.classic@oneclassics.local',
    'appraiser.custom@oneclassics.local',
    'appraiser.field@oneclassics.local'
)
ORDER BY
    r.role_name,
    u.email;
*/
