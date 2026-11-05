/*
  ONE Classics Nexus
  Update_AutoNumbering_v1.0.sql

  Purpose:
    Introduce reusable entity auto-number sequencing and enforce
    unique formatted numbers for appraisals, clients, and requests.
*/

USE one_classics_nexus_temp;

CREATE TABLE IF NOT EXISTS entity_number_sequences (
    sequence_key VARCHAR(64) NOT NULL,
    last_value BIGINT NOT NULL,
    PRIMARY KEY (sequence_key)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE clients
    ADD COLUMN client_number VARCHAR(50) NULL;

ALTER TABLE clients
    ADD UNIQUE KEY uq_clients_number (client_number);

ALTER TABLE appraisal_requests
    ADD UNIQUE KEY uq_appraisal_requests_request_number (request_number);

INSERT INTO entity_number_sequences(sequence_key, last_value)
SELECT 'appraisal', COALESCE(MAX(CAST(SUBSTRING_INDEX(appraisal_number, '_', -1) AS UNSIGNED)), 0)
FROM appraisals
WHERE appraisal_number LIKE 'AN\_%'
ON DUPLICATE KEY UPDATE last_value = VALUES(last_value);

INSERT INTO entity_number_sequences(sequence_key, last_value)
SELECT 'client', COALESCE(MAX(CAST(SUBSTRING_INDEX(client_number, '_', -1) AS UNSIGNED)), 0)
FROM clients
WHERE client_number LIKE 'CN\_%'
ON DUPLICATE KEY UPDATE last_value = VALUES(last_value);

INSERT INTO entity_number_sequences(sequence_key, last_value)
SELECT 'appraisal_request', COALESCE(MAX(CAST(SUBSTRING_INDEX(request_number, '_', -1) AS UNSIGNED)), 0)
FROM appraisal_requests
WHERE request_number LIKE 'RN\_%'
ON DUPLICATE KEY UPDATE last_value = VALUES(last_value);
