/*
================================================================================
ONE Classics Nexus
Clients Test Data Seed Script
Version: 1.1
Target DB: one_classics_nexus or one_classics_nexus_test
DB Engine: MariaDB

Purpose:
  Pre-populates the clients table with deterministic baseline test clients for
  appraisal request intake and appraisal workflow testing.

Clients Included:
  Individual (client_type_code = 'INDIVIDUAL'):
    - CLT-1001 Robert Turner
    - CLT-1002 Evelyn Harper
    - CLT-1003 Marcus Bennett
    - CLT-1004 Sofia Ramirez
    - CLT-1005 Daniel Whitfield

  Organization (client_type_code = 'ORGANIZATION'):
    - CLT-2001 Silverline Classics LLC
    - CLT-2002 Heritage Auto Exchange Inc.
    - CLT-2003 Lakeshore Collector Group

Execution Notes:
  1. Select the target database before running this script.
     Example:
       USE one_classics_nexus_test;
       SOURCE Nexus_Prepopulate_Clients_Test_Data_v1.1.sql;

  2. Script is idempotent. Re-running it will not create duplicate clients.
     Matching is performed on client_number.

  3. No DELETE statements are included.

  4. created_by_user_id / updated_by_user_id reference the primary admin test
     user seeded by Nexus_Prepopulate_Users_Test_Data_v1.0.sql
     ('00000000-0000-0000-0000-000000000101').

  5. For organization clients, first_name / last_name hold the primary contact
     person because those columns are NOT NULL in the locked schema.
================================================================================
*/

START TRANSACTION;

USE one_classics_nexus_test;

/* -----------------------------------------------------------------------------
   Temporary source set for deterministic, repeatable seed data.
----------------------------------------------------------------------------- */
DROP TEMPORARY TABLE IF EXISTS tmp_nexus_clients_seed;

CREATE TEMPORARY TABLE tmp_nexus_clients_seed
(
    client_id                CHAR(36)     NOT NULL,
    client_number            VARCHAR(30)  NOT NULL,
    client_type_code         VARCHAR(30)  NOT NULL,
    first_name               VARCHAR(100) NOT NULL,
    last_name                VARCHAR(100) NOT NULL,
    middle_name              VARCHAR(100) NULL,
    display_name             VARCHAR(200) NOT NULL,
    address_line1            VARCHAR(200) NULL,
    address_line2            VARCHAR(200) NULL,
    city                     VARCHAR(100) NULL,
    state_region             VARCHAR(100) NULL,
    postal_code              VARCHAR(30)  NULL,
    email                    VARCHAR(254) NULL,
    phone                    VARCHAR(40)  NULL,
    preferred_contact_method VARCHAR(20)  NULL,
    notes                    TEXT         NULL
);

INSERT INTO tmp_nexus_clients_seed
(
    client_id,
    client_number,
    client_type_code,
    first_name,
    last_name,
    middle_name,
    display_name,
    address_line1,
    address_line2,
    city,
    state_region,
    postal_code,
    email,
    phone,
    preferred_contact_method,
    notes
)
VALUES
    -- Individual clients
    (
        '10000000-0000-0000-0000-000000000001',
        'CLT-1001',
        'INDIVIDUAL',
        'Robert',
        'Turner',
        'James',
        'Robert Turner',
        '1427 Sycamore Lane',
        NULL,
        'Springfield',
        'IL',
        '62704',
        'robert.turner@example.com',
        '+1-217-555-0142',
        'Email',
        'Test individual client for standard appraisal workflow.'
    ),
    (
        '10000000-0000-0000-0000-000000000002',
        'CLT-1002',
        'INDIVIDUAL',
        'Evelyn',
        'Harper',
        NULL,
        'Evelyn Harper',
        '88 Marigold Court',
        'Unit 3',
        'Ann Arbor',
        'MI',
        '48104',
        'evelyn.harper@example.com',
        '+1-734-555-0188',
        'Phone',
        'Test individual client for vintage/classic vehicle appraisal workflow.'
    ),
    (
        '10000000-0000-0000-0000-000000000003',
        'CLT-1003',
        'INDIVIDUAL',
        'Marcus',
        'Bennett',
        'Lee',
        'Marcus Bennett',
        '905 Harbor View Road',
        NULL,
        'Charleston',
        'SC',
        '29403',
        'marcus.bennett@example.com',
        '+1-843-555-0133',
        'Email',
        'Prospective test client for appraisal request intake testing.'
    ),
    (
        '10000000-0000-0000-0000-000000000004',
        'CLT-1004',
        'INDIVIDUAL',
        'Sofia',
        'Ramirez',
        'Maria',
        'Sofia Ramirez',
        '2210 Desert Bloom Drive',
        NULL,
        'Scottsdale',
        'AZ',
        '85251',
        'sofia.ramirez@example.com',
        '+1-480-555-0171',
        'Text',
        'Test individual client for restoration valuation scenarios.'
    ),
    (
        '10000000-0000-0000-0000-000000000005',
        'CLT-1005',
        'INDIVIDUAL',
        'Daniel',
        'Whitfield',
        NULL,
        'Daniel Whitfield',
        '47 Beacon Hill Street',
        'Apt 12B',
        'Boston',
        'MA',
        '02108',
        'daniel.whitfield@example.com',
        '+1-617-555-0126',
        'Email',
        'Test individual client for insurance appraisal scenarios.'
    ),

    -- Organizational clients
    (
        '10000000-0000-0000-0000-000000000101',
        'CLT-2001',
        'ORGANIZATION',
        'Gregory',
        'Salazar',
        NULL,
        'Silverline Classics LLC',
        '3400 Industrial Parkway',
        'Suite 220',
        'Dallas',
        'TX',
        '75207',
        'contact@silverlineclassics.example.com',
        '+1-214-555-0190',
        'Email',
        'Test organizational client representing a classic vehicle dealer.'
    ),
    (
        '10000000-0000-0000-0000-000000000102',
        'CLT-2002',
        'ORGANIZATION',
        'Priya',
        'Natarajan',
        NULL,
        'Heritage Auto Exchange Inc.',
        '6125 Commerce Boulevard',
        NULL,
        'Columbus',
        'OH',
        '43215',
        'appraisals@heritageautoexchange.example.com',
        '+1-614-555-0117',
        'Phone',
        'Test organizational client for multi-vehicle appraisal scenarios.'
    ),
    (
        '10000000-0000-0000-0000-000000000103',
        'CLT-2003',
        'ORGANIZATION',
        'Andre',
        'Kowalski',
        'P',
        'Lakeshore Collector Group',
        '77 Lakeshore Terrace',
        'Building C',
        'Milwaukee',
        'WI',
        '53202',
        'office@lakeshorecollectorgroup.example.com',
        '+1-414-555-0164',
        'Email',
        'Test organizational client for collection portfolio appraisal scenarios.'
    );

/* -----------------------------------------------------------------------------
   Insert clients that do not already exist (matched on client_number).
----------------------------------------------------------------------------- */
INSERT INTO clients
(
    client_id,
    client_number,
    client_type_code,
    first_name,
    last_name,
    middle_name,
    display_name,
    address_line1,
    address_line2,
    city,
    state_region,
    postal_code,
    email,
    phone,
    preferred_contact_method,
    notes,
    row_version,
    created_at_utc,
    created_by_user_id,
    updated_at_utc,
    updated_by_user_id
)
SELECT
    s.client_id,
    s.client_number,
    s.client_type_code,
    s.first_name,
    s.last_name,
    s.middle_name,
    s.display_name,
    s.address_line1,
    s.address_line2,
    s.city,
    s.state_region,
    s.postal_code,
    s.email,
    s.phone,
    s.preferred_contact_method,
    s.notes,
    1,
    UTC_TIMESTAMP(6),
    '00000000-0000-0000-0000-000000000101',
    UTC_TIMESTAMP(6),
    '00000000-0000-0000-0000-000000000101'
FROM tmp_nexus_clients_seed AS s
WHERE NOT EXISTS
(
    SELECT 1
    FROM clients AS c
    WHERE c.client_number = s.client_number
);

/* -----------------------------------------------------------------------------
   Refresh existing seeded clients so the script stays idempotent.
----------------------------------------------------------------------------- */
UPDATE clients AS c
JOIN tmp_nexus_clients_seed AS s
    ON s.client_number = c.client_number
SET
    c.client_type_code         = s.client_type_code,
    c.first_name               = s.first_name,
    c.last_name                = s.last_name,
    c.middle_name              = s.middle_name,
    c.display_name             = s.display_name,
    c.address_line1            = s.address_line1,
    c.address_line2            = s.address_line2,
    c.city                     = s.city,
    c.state_region             = s.state_region,
    c.postal_code              = s.postal_code,
    c.email                    = s.email,
    c.phone                    = s.phone,
    c.preferred_contact_method = s.preferred_contact_method,
    c.notes                    = s.notes,
    c.row_version              = c.row_version + 1,
    c.updated_at_utc           = UTC_TIMESTAMP(6),
    c.updated_by_user_id       = '00000000-0000-0000-0000-000000000101';

DROP TEMPORARY TABLE IF EXISTS tmp_nexus_clients_seed;

COMMIT;

/* -----------------------------------------------------------------------------
   Verification
----------------------------------------------------------------------------- */
SELECT
    client_number,
    client_type_code,
    display_name,
    city,
    state_region,
    email,
    phone
FROM clients
WHERE client_number IN
(
    'CLT-1001', 'CLT-1002', 'CLT-1003', 'CLT-1004', 'CLT-1005',
    'CLT-2001', 'CLT-2002', 'CLT-2003'
)
ORDER BY client_number;
