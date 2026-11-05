/*
    ONE Classics Nexus
    Script: Nexus_Prepopulate_Client_Contacts_Test_Data_v1.0.sql
    Purpose: Pre-populate the client_contacts table with deterministic test data.
    Database: MariaDB / MySQL

    Prerequisite:
    - Run Nexus_Prepopulate_Clients_Test_Data_v1.0.sql first.

    Notes:
    - This script assumes the client_contacts table already exists.
    - client_contact_id values are deterministic so the script is repeatable.
    - The script resolves seeded contacts against existing clients to protect FK integrity.
    - Uses ON DUPLICATE KEY UPDATE so the script can be rerun safely.
    - No DELETE statements are included.
*/

START TRANSACTION;

DROP TEMPORARY TABLE IF EXISTS tmp_nexus_client_contact_seed;

CREATE TEMPORARY TABLE tmp_nexus_client_contact_seed
(
    client_contact_id CHAR(36) NOT NULL,
    client_id CHAR(36) NOT NULL,
    expected_client_name VARCHAR(200) NOT NULL,
    contact_name VARCHAR(200) NOT NULL,
    contact_role VARCHAR(50) NULL,
    email VARCHAR(255) NULL,
    phone VARCHAR(50) NULL,
    is_primary TINYINT(1) NOT NULL
);

INSERT INTO tmp_nexus_client_contact_seed
(
    client_contact_id,
    client_id,
    expected_client_name,
    contact_name,
    contact_role,
    email,
    phone,
    is_primary
)
VALUES
-- Robert Turner / individual client
(
    '11000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    'Robert Turner',
    'Robert Turner',
    'Owner',
    'robert.turner@example.test',
    '208-555-0101',
    1
),
(
    '11000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000001',
    'Robert Turner',
    'Linda Turner',
    'Alternate Contact',
    'linda.turner@example.test',
    '208-555-0102',
    0
),

-- Evelyn Harper / individual classic workflow client
(
    '11000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000002',
    'Evelyn Harper',
    'Evelyn Harper',
    'Owner',
    'evelyn.harper@example.test',
    '208-555-0110',
    1
),

-- Marcus Bennett / prospect intake client
(
    '11000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000003',
    'Marcus Bennett',
    'Marcus Bennett',
    'Prospect',
    'marcus.bennett@example.test',
    '208-555-0120',
    1
),

-- Silverline Classics LLC / business client
(
    '11000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000004',
    'Silverline Classics LLC',
    'Dana Morales',
    'General Manager',
    'dana.morales@silverline.example.test',
    '208-555-0130',
    1
),
(
    '11000000-0000-0000-0000-000000000006',
    '10000000-0000-0000-0000-000000000004',
    'Silverline Classics LLC',
    'Owen Price',
    'Inventory Coordinator',
    'owen.price@silverline.example.test',
    '208-555-0131',
    0
),

-- Heritage Auto Exchange Inc. / dealer client
(
    '11000000-0000-0000-0000-000000000007',
    '10000000-0000-0000-0000-000000000005',
    'Heritage Auto Exchange Inc.',
    'Carla Reeves',
    'Dealer Principal',
    'carla.reeves@heritageauto.example.test',
    '208-555-0140',
    1
),
(
    '11000000-0000-0000-0000-000000000008',
    '10000000-0000-0000-0000-000000000005',
    'Heritage Auto Exchange Inc.',
    'Nolan Brooks',
    'Sales Manager',
    'nolan.brooks@heritageauto.example.test',
    '208-555-0141',
    0
),

-- Black Canyon Collection / collector client
(
    '11000000-0000-0000-0000-000000000009',
    '10000000-0000-0000-0000-000000000006',
    'Black Canyon Collection',
    'Graham Whitaker',
    'Collection Manager',
    'graham.whitaker@blackcanyon.example.test',
    '208-555-0150',
    1
),
(
    '11000000-0000-0000-0000-000000000010',
    '10000000-0000-0000-0000-000000000006',
    'Black Canyon Collection',
    'Iris Coleman',
    'Documentation Specialist',
    'iris.coleman@blackcanyon.example.test',
    '208-555-0151',
    0
),

-- Estate of Harold Whitman / estate client
(
    '11000000-0000-0000-0000-000000000011',
    '10000000-0000-0000-0000-000000000007',
    'Estate of Harold Whitman',
    'Patricia Whitman',
    'Estate Representative',
    'patricia.whitman@example.test',
    '208-555-0160',
    1
),
(
    '11000000-0000-0000-0000-000000000012',
    '10000000-0000-0000-0000-000000000007',
    'Estate of Harold Whitman',
    'Samuel Porter',
    'Estate Attorney',
    'samuel.porter@law.example.test',
    '208-555-0161',
    0
),

-- Summit Restoration Works LLC / inactive business client
(
    '11000000-0000-0000-0000-000000000013',
    '10000000-0000-0000-0000-000000000008',
    'Summit Restoration Works LLC',
    'Elena Vargas',
    'Operations Manager',
    'elena.vargas@summitrestoration.example.test',
    '208-555-0170',
    1
);

/*
    Optional dependency visibility:
    If rows are returned here, the matching clients are missing.
    Run Nexus_Prepopulate_Clients_Test_Data_v1.0.sql before this script.
*/
SELECT
    s.client_id,
    s.expected_client_name,
    COUNT(c.client_id) AS matching_client_count
FROM tmp_nexus_client_contact_seed s
LEFT JOIN clients c
    ON c.client_id = s.client_id
GROUP BY
    s.client_id,
    s.expected_client_name
HAVING COUNT(c.client_id) = 0
ORDER BY s.expected_client_name;

INSERT INTO client_contacts
(
    client_contact_id,
    client_id,
    contact_name,
    contact_role,
    email,
    phone,
    is_primary,
    created_at_utc,
    updated_at_utc
)
SELECT
    s.client_contact_id,
    s.client_id,
    s.contact_name,
    s.contact_role,
    s.email,
    s.phone,
    s.is_primary,
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
FROM tmp_nexus_client_contact_seed s
INNER JOIN clients c
    ON c.client_id = s.client_id
ON DUPLICATE KEY UPDATE
    client_id = VALUES(client_id),
    contact_name = VALUES(contact_name),
    contact_role = VALUES(contact_role),
    email = VALUES(email),
    phone = VALUES(phone),
    is_primary = VALUES(is_primary),
    updated_at_utc = UTC_TIMESTAMP(6);

DROP TEMPORARY TABLE IF EXISTS tmp_nexus_client_contact_seed;

COMMIT;

/*
    Verification: Confirm seeded client contacts.
*/
SELECT
    cc.client_contact_id,
    cc.client_id,
    c.legal_name,
    cc.contact_name,
    cc.contact_role,
    cc.email,
    cc.phone,
    cc.is_primary,
    cc.created_at_utc,
    cc.updated_at_utc
FROM client_contacts cc
INNER JOIN clients c
    ON c.client_id = cc.client_id
WHERE cc.client_contact_id IN
(
    '11000000-0000-0000-0000-000000000001',
    '11000000-0000-0000-0000-000000000002',
    '11000000-0000-0000-0000-000000000003',
    '11000000-0000-0000-0000-000000000004',
    '11000000-0000-0000-0000-000000000005',
    '11000000-0000-0000-0000-000000000006',
    '11000000-0000-0000-0000-000000000007',
    '11000000-0000-0000-0000-000000000008',
    '11000000-0000-0000-0000-000000000009',
    '11000000-0000-0000-0000-000000000010',
    '11000000-0000-0000-0000-000000000011',
    '11000000-0000-0000-0000-000000000012',
    '11000000-0000-0000-0000-000000000013'
)
ORDER BY
    c.legal_name,
    cc.is_primary DESC,
    cc.contact_name;

/*
    Verification: Confirm exactly one primary contact for each seeded client.
*/
SELECT
    c.client_id,
    c.legal_name,
    SUM(CASE WHEN cc.is_primary = 1 THEN 1 ELSE 0 END) AS primary_contact_count,
    COUNT(cc.client_contact_id) AS total_contact_count
FROM clients c
LEFT JOIN client_contacts cc
    ON cc.client_id = c.client_id
WHERE c.client_id IN
(
    '10000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000006',
    '10000000-0000-0000-0000-000000000007',
    '10000000-0000-0000-0000-000000000008'
)
GROUP BY
    c.client_id,
    c.legal_name
ORDER BY c.legal_name;

/*
    Verification: Detect duplicate primary contacts per client.
    Expected result: zero rows.
*/
SELECT
    client_id,
    COUNT(*) AS primary_contact_count
FROM client_contacts
WHERE is_primary = 1
GROUP BY client_id
HAVING COUNT(*) > 1;
