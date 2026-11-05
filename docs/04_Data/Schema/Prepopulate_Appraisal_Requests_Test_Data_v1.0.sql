/*
    ONE Classics Nexus
    Script: Nexus_Prepopulate_Appraisal_Requests_Test_Data_v1.0.sql
    Purpose: Pre-populate the appraisal_requests table with deterministic test data.
    Database: MariaDB / MySQL

    Prerequisites:
    - Run Nexus_Prepopulate_Clients_Test_Data_v1.0.sql first.
    - Run Nexus_Prepopulate_Client_Contacts_Test_Data_v1.0.sql first.

    Notes:
    - This script assumes the appraisal_requests table already exists.
    - appraisal_request_id values are deterministic so the script is repeatable.
    - created_appraisal_id is intentionally NULL for this intake seed data so requests can be
      loaded before appraisal records are created.
    - The script inserts only rows where the referenced client exists and the referenced
      contact is either NULL or exists.
    - Uses ON DUPLICATE KEY UPDATE so the script can be rerun safely.
    - No DELETE statements are included.
*/

START TRANSACTION;

DROP TEMPORARY TABLE IF EXISTS tmp_nexus_appraisal_request_seed;

CREATE TEMPORARY TABLE tmp_nexus_appraisal_request_seed
(
    appraisal_request_id CHAR(36) NOT NULL,
    client_id CHAR(36) NOT NULL,
    requested_by_contact_id CHAR(36) NULL,
    created_appraisal_id CHAR(36) NULL,
    expected_client_name VARCHAR(200) NOT NULL,
    requested_service VARCHAR(50) NOT NULL,
    request_channel VARCHAR(30) NULL,
    request_status VARCHAR(30) NOT NULL,
    intake_notes TEXT NULL,
    requested_on_utc DATETIME(6) NOT NULL,
    target_due_date_utc DATETIME(6) NULL
);

INSERT INTO tmp_nexus_appraisal_request_seed
(
    appraisal_request_id,
    client_id,
    requested_by_contact_id,
    created_appraisal_id,
    expected_client_name,
    requested_service,
    request_channel,
    request_status,
    intake_notes,
    requested_on_utc,
    target_due_date_utc
)
VALUES
-- Robert Turner / standard vehicle workflow
(
    '12000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    '11000000-0000-0000-0000-000000000001',
    NULL,
    'Robert Turner',
    'StandardAppraisal',
    'Phone',
    'New',
    'Client requested a standard market appraisal for a personal vehicle. Confirm VIN, mileage, and title status during intake.',
    UTC_TIMESTAMP(6) - INTERVAL 10 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 4 DAY
),

-- Evelyn Harper / vintage classic workflow
(
    '12000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000002',
    '11000000-0000-0000-0000-000000000003',
    NULL,
    'Evelyn Harper',
    'ClassicAppraisal',
    'Email',
    'IntakeReview',
    'Vintage/classic appraisal request. Client states documentation package is available and vehicle may require provenance review.',
    UTC_TIMESTAMP(6) - INTERVAL 8 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 7 DAY
),

-- Marcus Bennett / prospect request
(
    '12000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000003',
    '11000000-0000-0000-0000-000000000004',
    NULL,
    'Marcus Bennett',
    'PrePurchaseReview',
    'Web',
    'PendingClientInfo',
    'Prospect requested pre-purchase review. Waiting for seller listing URL, VIN, and available inspection photos.',
    UTC_TIMESTAMP(6) - INTERVAL 6 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 3 DAY
),

-- Silverline Classics / dealer or business inventory workflow
(
    '12000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000004',
    '11000000-0000-0000-0000-000000000005',
    NULL,
    'Silverline Classics LLC',
    'DealerInventoryAppraisal',
    'Email',
    'Scheduled',
    'Business client requested appraisal support for multiple classic inventory units. Initial request seeded as one intake record for workflow testing.',
    UTC_TIMESTAMP(6) - INTERVAL 5 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 10 DAY
),

-- Heritage Auto Exchange / wholesale appraisal request
(
    '12000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000005',
    '11000000-0000-0000-0000-000000000007',
    NULL,
    'Heritage Auto Exchange Inc.',
    'WholesaleAppraisal',
    'Referral',
    'New',
    'Dealer client requested wholesale valuation guidance for trade-in decision support.',
    UTC_TIMESTAMP(6) - INTERVAL 3 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 5 DAY
),

-- Black Canyon Collection / collector insurance workflow
(
    '12000000-0000-0000-0000-000000000006',
    '10000000-0000-0000-0000-000000000006',
    '11000000-0000-0000-0000-000000000009',
    NULL,
    'Black Canyon Collection',
    'InsuranceValueAppraisal',
    'Phone',
    'InProgress',
    'Collector client requested insurance value appraisal. High-documentation workflow expected; request documentation and photo library.',
    UTC_TIMESTAMP(6) - INTERVAL 12 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 2 DAY
),

-- Estate of Harold Whitman / estate valuation workflow
(
    '12000000-0000-0000-0000-000000000007',
    '10000000-0000-0000-0000-000000000007',
    '11000000-0000-0000-0000-000000000011',
    NULL,
    'Estate of Harold Whitman',
    'EstateValuation',
    'Email',
    'OnHold',
    'Estate representative requested valuation. On hold pending attorney authorization and vehicle ownership documentation.',
    UTC_TIMESTAMP(6) - INTERVAL 15 DAY,
    UTC_TIMESTAMP(6) + INTERVAL 14 DAY
),

-- Summit Restoration Works / inactive business historical request
(
    '12000000-0000-0000-0000-000000000008',
    '10000000-0000-0000-0000-000000000008',
    '11000000-0000-0000-0000-000000000013',
    NULL,
    'Summit Restoration Works LLC',
    'CustomVehicleAppraisal',
    'Web',
    'Cancelled',
    'Inactive client test record. Request cancelled after intake; useful for status filtering and history validation.',
    UTC_TIMESTAMP(6) - INTERVAL 30 DAY,
    UTC_TIMESTAMP(6) - INTERVAL 20 DAY
);

/*
    Dependency visibility: missing clients.
    Expected result: zero rows.
*/
SELECT
    s.client_id,
    s.expected_client_name,
    COUNT(c.client_id) AS matching_client_count
FROM tmp_nexus_appraisal_request_seed s
LEFT JOIN clients c
    ON c.client_id = s.client_id
GROUP BY
    s.client_id,
    s.expected_client_name
HAVING COUNT(c.client_id) = 0
ORDER BY s.expected_client_name;

/*
    Dependency visibility: missing requested_by contacts.
    Expected result: zero rows for rows where requested_by_contact_id is not NULL.
*/
SELECT
    s.requested_by_contact_id,
    s.expected_client_name,
    s.appraisal_request_id,
    COUNT(cc.client_contact_id) AS matching_contact_count
FROM tmp_nexus_appraisal_request_seed s
LEFT JOIN client_contacts cc
    ON cc.client_contact_id = s.requested_by_contact_id
WHERE s.requested_by_contact_id IS NOT NULL
GROUP BY
    s.requested_by_contact_id,
    s.expected_client_name,
    s.appraisal_request_id
HAVING COUNT(cc.client_contact_id) = 0
ORDER BY s.expected_client_name;

INSERT INTO appraisal_requests
(
    appraisal_request_id,
    client_id,
    requested_by_contact_id,
    created_appraisal_id,
    requested_service,
    request_channel,
    request_status,
    intake_notes,
    requested_on_utc,
    target_due_date_utc,
    created_at_utc,
    updated_at_utc
)
SELECT
    s.appraisal_request_id,
    s.client_id,
    s.requested_by_contact_id,
    s.created_appraisal_id,
    s.requested_service,
    s.request_channel,
    s.request_status,
    s.intake_notes,
    s.requested_on_utc,
    s.target_due_date_utc,
    UTC_TIMESTAMP(6),
    UTC_TIMESTAMP(6)
FROM tmp_nexus_appraisal_request_seed s
INNER JOIN clients c
    ON c.client_id = s.client_id
LEFT JOIN client_contacts cc
    ON cc.client_contact_id = s.requested_by_contact_id
WHERE
    s.requested_by_contact_id IS NULL
    OR cc.client_contact_id IS NOT NULL
ON DUPLICATE KEY UPDATE
    client_id = VALUES(client_id),
    requested_by_contact_id = VALUES(requested_by_contact_id),
    created_appraisal_id = VALUES(created_appraisal_id),
    requested_service = VALUES(requested_service),
    request_channel = VALUES(request_channel),
    request_status = VALUES(request_status),
    intake_notes = VALUES(intake_notes),
    requested_on_utc = VALUES(requested_on_utc),
    target_due_date_utc = VALUES(target_due_date_utc),
    updated_at_utc = UTC_TIMESTAMP(6);

DROP TEMPORARY TABLE IF EXISTS tmp_nexus_appraisal_request_seed;

COMMIT;

/*
    Verification: Confirm seeded appraisal requests.
*/
SELECT
    ar.appraisal_request_id,
    ar.client_id,
    c.legal_name,
    ar.requested_by_contact_id,
    cc.contact_name AS requested_by_contact_name,
    ar.created_appraisal_id,
    ar.requested_service,
    ar.request_channel,
    ar.request_status,
    ar.requested_on_utc,
    ar.target_due_date_utc,
    ar.created_at_utc,
    ar.updated_at_utc
FROM appraisal_requests ar
INNER JOIN clients c
    ON c.client_id = ar.client_id
LEFT JOIN client_contacts cc
    ON cc.client_contact_id = ar.requested_by_contact_id
WHERE ar.appraisal_request_id IN
(
    '12000000-0000-0000-0000-000000000001',
    '12000000-0000-0000-0000-000000000002',
    '12000000-0000-0000-0000-000000000003',
    '12000000-0000-0000-0000-000000000004',
    '12000000-0000-0000-0000-000000000005',
    '12000000-0000-0000-0000-000000000006',
    '12000000-0000-0000-0000-000000000007',
    '12000000-0000-0000-0000-000000000008'
)
ORDER BY
    ar.requested_on_utc DESC,
    c.legal_name;

/*
    Verification: Request counts by status and service.
*/
SELECT
    request_status,
    requested_service,
    COUNT(*) AS request_count
FROM appraisal_requests
WHERE appraisal_request_id IN
(
    '12000000-0000-0000-0000-000000000001',
    '12000000-0000-0000-0000-000000000002',
    '12000000-0000-0000-0000-000000000003',
    '12000000-0000-0000-0000-000000000004',
    '12000000-0000-0000-0000-000000000005',
    '12000000-0000-0000-0000-000000000006',
    '12000000-0000-0000-0000-000000000007',
    '12000000-0000-0000-0000-000000000008'
)
GROUP BY
    request_status,
    requested_service
ORDER BY
    request_status,
    requested_service;

/*
    Verification: Confirm no seeded appraisal request points to a contact belonging to a different client.
    Expected result: zero rows.
*/
SELECT
    ar.appraisal_request_id,
    ar.client_id AS request_client_id,
    cc.client_id AS contact_client_id,
    ar.requested_by_contact_id
FROM appraisal_requests ar
INNER JOIN client_contacts cc
    ON cc.client_contact_id = ar.requested_by_contact_id
WHERE ar.appraisal_request_id IN
(
    '12000000-0000-0000-0000-000000000001',
    '12000000-0000-0000-0000-000000000002',
    '12000000-0000-0000-0000-000000000003',
    '12000000-0000-0000-0000-000000000004',
    '12000000-0000-0000-0000-000000000005',
    '12000000-0000-0000-0000-000000000006',
    '12000000-0000-0000-0000-000000000007',
    '12000000-0000-0000-0000-000000000008'
)
AND ar.client_id <> cc.client_id;
