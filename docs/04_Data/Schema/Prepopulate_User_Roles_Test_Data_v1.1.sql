/*
================================================================================
ONE Classics Nexus
User Roles Test Data Seed Script
Version: 1.1
Target DB: one_classics_nexus or one_classics_nexus_test
DB Engine: MariaDB

Purpose:
  Pre-populates the user_roles junction table with role assignments that align
  with the Nexus roles and users seed scripts.

Depends On:
  1. Nexus_Prepopulate_Roles_Test_Data_v1.0.sql
     - Administrator
     - Appraiser

  2. Nexus_Prepopulate_Users_Test_Data_v1.0.sql
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
       SOURCE Nexus_Prepopulate_User_Roles_Test_Data_v1.1.sql;

  2. Run order:
       a. roles seed script
       b. users seed script
       c. this user_roles seed script

  3. Script is idempotent. Re-running it will not duplicate assignments.

Expected user_roles table shape:
  user_roles.user_id        CHAR(36) or VARCHAR(36)
  user_roles.role_id        CHAR(36) or VARCHAR(36)
  user_roles.created_at_utc DATETIME

Recommended uniqueness rule:
  UNIQUE(user_id, role_id)

If your current user_roles table includes user_role_id CHAR(36) as a required
primary key, use the alternate INSERT pattern at the bottom of this script.
================================================================================
*/

START TRANSACTION;

/* -----------------------------------------------------------------------------
   Temporary source set for deterministic, repeatable role assignments.
----------------------------------------------------------------------------- */
DROP TEMPORARY TABLE IF EXISTS tmp_nexus_user_role_seed;

CREATE TEMPORARY TABLE tmp_nexus_user_role_seed
(
    email     VARCHAR(255) NOT NULL,
    role_name VARCHAR(100) NOT NULL
);

INSERT INTO tmp_nexus_user_role_seed
(
    email,
    role_name
)
VALUES
    -- Administrator users
    ('admin@oneclassics.local', 'Administrator'),
    ('admin.operations@oneclassics.local', 'Administrator'),
    ('admin.qa@oneclassics.local', 'Administrator'),

    -- Appraiser users
    ('appraiser@oneclassics.local', 'Appraiser'),
    ('appraiser.classic@oneclassics.local', 'Appraiser'),
    ('appraiser.custom@oneclassics.local', 'Appraiser'),
    ('appraiser.field@oneclassics.local', 'Appraiser');

/* -----------------------------------------------------------------------------
   Optional elevated workflow test assignment.

   Uncomment this row if the primary admin account should also exercise the
   Appraiser workflow during Swagger/API/WinForms testing.
----------------------------------------------------------------------------- */

INSERT INTO tmp_nexus_user_role_seed (email, role_name)
VALUES ('admin@oneclassics.local', 'Appraiser');


/* -----------------------------------------------------------------------------
   Pre-check: identify missing users or roles before insert.
   These SELECT statements do not stop execution; they make missing dependencies
   obvious in the SQL output.
----------------------------------------------------------------------------- */
SELECT
    'Missing users required by user_roles seed' AS check_name,
    s.email
FROM tmp_nexus_user_role_seed s
LEFT JOIN users u
    ON u.email = s.email
WHERE u.user_id IS NULL
ORDER BY s.email;

SELECT
    'Missing roles required by user_roles seed' AS check_name,
    s.role_name
FROM tmp_nexus_user_role_seed s
LEFT JOIN roles r
    ON r.role_name = s.role_name
WHERE r.role_id IS NULL
GROUP BY s.role_name
ORDER BY s.role_name;

/* -----------------------------------------------------------------------------
   Insert missing user-role mappings.
----------------------------------------------------------------------------- */
INSERT INTO user_roles
(
    user_id,
    role_id,
    created_at_utc
)
SELECT
    u.user_id,
    r.role_id,
    UTC_TIMESTAMP()
FROM tmp_nexus_user_role_seed s
JOIN users u
    ON u.email = s.email
JOIN roles r
    ON r.role_name = s.role_name
WHERE NOT EXISTS
(
    SELECT 1
    FROM user_roles ur
    WHERE ur.user_id = u.user_id
      AND ur.role_id = r.role_id
);

COMMIT;

/* -----------------------------------------------------------------------------
   Verification Query
----------------------------------------------------------------------------- */
SELECT
    u.user_id,
    u.email,
    u.username,
    u.display_name,
    r.role_id,
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
  AND r.role_name IN ('Administrator', 'Appraiser')
ORDER BY
    r.role_name,
    u.email;

/* -----------------------------------------------------------------------------
   Recommended Index / Constraint

   Run this once only if your schema does not already enforce uniqueness.
   MariaDB does not support ADD INDEX IF NOT EXISTS consistently across all
   supported versions, so keep this as a controlled schema patch, not test seed.
----------------------------------------------------------------------------- */
/*
ALTER TABLE user_roles
ADD UNIQUE KEY uk_user_roles_user_role (user_id, role_id);
*/

/*
================================================================================
Alternate INSERT Pattern if user_roles has required user_role_id CHAR(36)
================================================================================
Use this version instead of the INSERT block above if your table shape is:

  user_roles.user_role_id   CHAR(36) PK
  user_roles.user_id        CHAR(36) FK
  user_roles.role_id        CHAR(36) FK
  user_roles.created_at_utc DATETIME

INSERT INTO user_roles
(
    user_role_id,
    user_id,
    role_id,
    created_at_utc
)
SELECT
    UUID(),
    u.user_id,
    r.role_id,
    UTC_TIMESTAMP()
FROM tmp_nexus_user_role_seed s
JOIN users u
    ON u.email = s.email
JOIN roles r
    ON r.role_name = s.role_name
WHERE NOT EXISTS
(
    SELECT 1
    FROM user_roles ur
    WHERE ur.user_id = u.user_id
      AND ur.role_id = r.role_id
);
================================================================================
*/
