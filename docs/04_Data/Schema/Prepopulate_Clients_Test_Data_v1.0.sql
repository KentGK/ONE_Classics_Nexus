/*
    ONE Classics Nexus
    Script: Nexus_Prepopulate_Clients_Test_Data_v1.0.sql
    Purpose: Pre-populate the clients table with deterministic test data.
    Database: MariaDB / MySQL

    Notes:
    - This script assumes the clients table already exists.
    - client_id values are deterministic so the script is repeatable.
    - Uses ON DUPLICATE KEY UPDATE so the script can be rerun safely.
    - No DELETE statements are included.
*/

START TRANSACTION;

INSERT INTO clients
(
    client_id,
    client_type,
    legal_name,
    preferred_name,
    status,
    notes,
    created_at_utc,
    updated_at_utc
)
VALUES
(
    '10000000-0000-0000-0000-000000000001',
    'Individual',
    'Robert Turner',
    'Bob Turner',
    'Active',
    'Test individual client for standard appraisal workflow.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000002',
    'Individual',
    'Evelyn Harper',
    'Evelyn Harper',
    'Active',
    'Test individual client for vintage/classic vehicle appraisal workflow.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000003',
    'Individual',
    'Marcus Bennett',
    'Marcus Bennett',
    'Prospect',
    'Prospective test client for appraisal request intake testing.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000004',
    'Business',
    'Silverline Classics LLC',
    'Silverline Classics',
    'Active',
    'Test business client representing a classic vehicle dealer.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000005',
    'Dealer',
    'Heritage Auto Exchange Inc.',
    'Heritage Auto Exchange',
    'Active',
    'Test dealer client for multi-vehicle appraisal scenarios.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000006',
    'Collector',
    'Black Canyon Collection',
    'Black Canyon Collection',
    'Active',
    'Test collector client for high-documentation appraisal scenarios.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000007',
    'Estate',
    'Estate of Harold Whitman',
    'Whitman Estate',
    'Active',
    'Test estate client for probate or estate valuation workflows.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
),
(
    '10000000-0000-0000-0000-000000000008',
    'Business',
    'Summit Restoration Works LLC',
    'Summit Restoration Works',
    'Inactive',
    'Inactive test business client for status filtering validation.',
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
)
ON DUPLICATE KEY UPDATE
    client_type = VALUES(client_type),
    legal_name = VALUES(legal_name),
    preferred_name = VALUES(preferred_name),
    status = VALUES(status),
    notes = VALUES(notes),
    updated_at_utc = UTC_TIMESTAMP(6);

COMMIT;

/*
    Verification: Confirm seeded clients.
*/
SELECT
    client_id,
    client_type,
    legal_name,
    preferred_name,
    status,
    created_at_utc,
    updated_at_utc
FROM clients
WHERE client_id IN
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
ORDER BY legal_name;

/*
    Verification: Client counts by type and status.
*/
SELECT
    client_type,
    status,
    COUNT(*) AS client_count
FROM clients
GROUP BY client_type, status
ORDER BY client_type, status;
