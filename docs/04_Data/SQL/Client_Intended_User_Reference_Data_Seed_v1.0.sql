-- ============================================================================
-- ONE Classics Nexus
-- 14.1.2 Seed Client / Intended User Reference Data and Supporting Indexes
-- Version: 1.0
-- Source: ONE_Classics_Nexus_Client_Intended_User_DDS_v1.1
--         Sections 4.4, 4.5, 4.6 and 14.1.2
-- Target: MariaDB 10.5+
--
-- Prerequisites:
--   use_categories
--   intended_uses
--   intended_user_types
--
-- Behavior:
--   * Uses fixed lower-case UUIDs and immutable upper-case machine codes.
--   * Validates that the staged seed sets contain no duplicate UUIDs or codes.
--   * Fails before inserts when an existing UUID maps to another code, or an
--     existing code maps to another UUID.
--   * Inserts missing rows only. Existing rows are not semantically overwritten.
--   * Adds supporting indexes idempotently.
-- ============================================================================

SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;

-- A fixed date makes environments reproducible and avoids deployment-time
-- differences in effective dating and audit timestamps.
SET @seed_effective_from_utc = TIMESTAMP('2026-01-01 00:00:00.000000');

DROP TEMPORARY TABLE IF EXISTS seed_use_categories;
CREATE TEMPORARY TABLE seed_use_categories
(
    use_category_id    CHAR(36)     NOT NULL,
    category_code      VARCHAR(50)  NOT NULL,
    use_category_name  VARCHAR(100) NOT NULL,
    description        TEXT         NULL,
    sort_order         INT          NOT NULL,
    PRIMARY KEY (use_category_id),
    UNIQUE KEY uq_seed_use_categories_code (category_code)
) ENGINE = InnoDB;

INSERT INTO seed_use_categories
    (use_category_id, category_code, use_category_name, description, sort_order)
VALUES
    ('71000000-0000-4000-8000-000000000001', 'INSURANCE_RISK',
     'Insurance and Risk Management',
     'Uses involving insurance placement, scheduled coverage, risk management, or the analysis and settlement of a covered loss.', 10),
    ('71000000-0000-4000-8000-000000000002', 'ESTATE_FIDUCIARY',
     'Estate and Fiduciary',
     'Uses involving estate planning, estate administration, settlement, trusts, or other fiduciary responsibilities.', 20),
    ('71000000-0000-4000-8000-000000000003', 'LEGAL_DISPUTE',
     'Legal and Dispute Resolution',
     'Uses supporting litigation, equitable distribution, mediation, arbitration, or another identified legal proceeding.', 30),
    ('71000000-0000-4000-8000-000000000004', 'CREDIT_FINANCING',
     'Credit and Financing',
     'Uses supporting a lender or other identified financing decision involving the subject personal property.', 40),
    ('71000000-0000-4000-8000-000000000005', 'TRANSACTION',
     'Acquisition and Disposition',
     'Uses supporting a specifically identified purchase, sale, exchange, consignment, or other disposition decision.', 50),
    ('71000000-0000-4000-8000-000000000006', 'TAX_REGULATORY',
     'Tax and Regulatory',
     'Uses involving a specifically identified tax, charitable-contribution, governmental, or regulatory matter.', 60),
    ('71000000-0000-4000-8000-000000000007', 'COLLECTION_ADVISORY',
     'Collection Management and Advisory',
     'Uses supporting collection stewardship, planning, prioritization, internal decision-making, or other identified advisory needs.', 70),
    ('71000000-0000-4000-8000-000000000008', 'OTHER_SPECIFIED',
     'Other Specified Use',
     'Administrative category for an intended use not represented by another controlled definition; request-specific narrative is required.', 900);

DROP TEMPORARY TABLE IF EXISTS seed_intended_uses;
CREATE TEMPORARY TABLE seed_intended_uses
(
    intended_use_id           CHAR(36)     NOT NULL,
    use_category_id           CHAR(36)     NOT NULL,
    intended_use_code         VARCHAR(50)  NOT NULL,
    intended_use_name         VARCHAR(200) NOT NULL,
    description               TEXT         NOT NULL,
    default_restrictions      TEXT         NULL,
    requires_custom_narrative BOOLEAN      NOT NULL,
    PRIMARY KEY (intended_use_id),
    UNIQUE KEY uq_seed_intended_uses_code (intended_use_code)
) ENGINE = InnoDB;

INSERT INTO seed_intended_uses
    (intended_use_id, use_category_id, intended_use_code, intended_use_name,
     description, default_restrictions, requires_custom_narrative)
VALUES
    ('72000000-0000-4000-8000-000000000001', '71000000-0000-4000-8000-000000000001',
     'INSURANCE_COVERAGE', 'Insurance Coverage',
     'To assist identified intended users in establishing, reviewing, or updating insurance coverage for the subject property.',
     'Not intended to establish a claim settlement amount or determine coverage under an insurance contract.', FALSE),
    ('72000000-0000-4000-8000-000000000002', '71000000-0000-4000-8000-000000000001',
     'INSURANCE_CLAIM', 'Insurance Claim',
     'To assist identified intended users in analyzing value for a specifically identified insurance claim or loss matter.',
     'The appraisal does not determine policy coverage, liability, causation, or the insurer''s contractual obligation.', TRUE),
    ('72000000-0000-4000-8000-000000000003', '71000000-0000-4000-8000-000000000002',
     'ESTATE_PLANNING', 'Estate Planning',
     'To assist identified intended users with planning decisions concerning the ownership, transfer, or administration of estate property.',
     'Not intended for estate-tax filing or settlement unless that use is expressly identified in the assignment.', FALSE),
    ('72000000-0000-4000-8000-000000000004', '71000000-0000-4000-8000-000000000002',
     'ESTATE_SETTLEMENT', 'Estate Settlement',
     'To assist identified intended users with the administration, allocation, or settlement of property held by an estate or trust.',
     'Any tax-reporting or court use must be expressly identified and supported by the assignment scope.', TRUE),
    ('72000000-0000-4000-8000-000000000005', '71000000-0000-4000-8000-000000000003',
     'EQUITABLE_DISTRIBUTION', 'Equitable Distribution',
     'To assist identified intended users with the allocation or division of property in a specifically identified proceeding or negotiated settlement.',
     'The appraisal provides a value opinion and does not determine legal ownership, entitlement, or the final allocation of property.', TRUE),
    ('72000000-0000-4000-8000-000000000006', '71000000-0000-4000-8000-000000000003',
     'LITIGATION', 'Litigation Support',
     'To assist identified intended users in a specifically identified dispute, claim, mediation, arbitration, or court proceeding.',
     'Use is limited to the identified matter and intended users; the appraisal does not provide legal conclusions.', TRUE),
    ('72000000-0000-4000-8000-000000000007', '71000000-0000-4000-8000-000000000004',
     'FINANCING', 'Financing',
     'To assist an identified lender or other intended user in a specifically identified credit or collateral decision.',
     'Not a guarantee of collateral performance, marketability, loan approval, or future value.', TRUE),
    ('72000000-0000-4000-8000-000000000008', '71000000-0000-4000-8000-000000000005',
     'ACQUISITION', 'Acquisition Decision',
     'To assist identified intended users in evaluating a specifically identified purchase, exchange, or acquisition decision.',
     'Not a warranty, condition guarantee, title opinion, or recommendation to complete a transaction.', FALSE),
    ('72000000-0000-4000-8000-000000000009', '71000000-0000-4000-8000-000000000005',
     'DISPOSITION', 'Disposition Decision',
     'To assist identified intended users in evaluating a specifically identified sale, exchange, consignment, or other disposition decision.',
     'Not a guarantee of sale price, time to sell, market acceptance, or transaction outcome.', FALSE),
    ('72000000-0000-4000-8000-000000000010', '71000000-0000-4000-8000-000000000006',
     'CHARITABLE_CONTRIBUTION', 'Charitable Contribution',
     'To assist identified intended users with valuation requirements associated with a specifically identified charitable contribution.',
     'The client and intended users remain responsible for legal, tax, filing, and substantiation requirements outside the appraisal assignment.', TRUE),
    ('72000000-0000-4000-8000-000000000011', '71000000-0000-4000-8000-000000000006',
     'TAX_RELATED', 'Tax-Related Matter',
     'To assist identified intended users with a specifically identified federal, state, local, or other tax matter.',
     'The appraisal does not provide tax or legal advice and is limited to the identified tax purpose, date, value definition, and jurisdiction.', TRUE),
    ('72000000-0000-4000-8000-000000000012', '71000000-0000-4000-8000-000000000007',
     'COLLECTION_MANAGEMENT', 'Collection Management',
     'To assist identified intended users with stewardship, inventory prioritization, retention, transfer, or planning decisions for a collection.',
     'Not intended for insurance, lending, litigation, tax reporting, or a transaction unless separately identified in the assignment.', FALSE),
    ('72000000-0000-4000-8000-000000000013', '71000000-0000-4000-8000-000000000007',
     'ADVISORY', 'Advisory Decision Support',
     'To assist identified intended users with a specifically described advisory or planning decision involving the subject property.',
     'The request must identify the decision to be supported and any restrictions on use or reliance.', TRUE),
    ('72000000-0000-4000-8000-000000000014', '71000000-0000-4000-8000-000000000008',
     'OTHER_SPECIFIED', 'Other Specified Intended Use',
     'To support an intended use that is not adequately represented by another controlled definition and is fully described in request-specific narrative.',
     'Use and reliance are limited to the expressly identified intended use and intended users.', TRUE);

DROP TEMPORARY TABLE IF EXISTS seed_intended_user_types;
CREATE TEMPORARY TABLE seed_intended_user_types
(
    user_type_id    CHAR(36)     NOT NULL,
    user_type_code  VARCHAR(50)  NOT NULL,
    user_type_name  VARCHAR(100) NOT NULL,
    description     TEXT         NULL,
    sort_order      INT          NOT NULL,
    PRIMARY KEY (user_type_id),
    UNIQUE KEY uq_seed_intended_user_types_code (user_type_code)
) ENGINE = InnoDB;

INSERT INTO seed_intended_user_types
    (user_type_id, user_type_code, user_type_name, description, sort_order)
VALUES
    ('73000000-0000-4000-8000-000000000001', 'CLIENT', 'Client',
     'The party that engages the appraiser. Client status does not automatically make the party an intended user; that status must be explicitly recorded.', 10),
    ('73000000-0000-4000-8000-000000000002', 'PROPERTY_OWNER', 'Property Owner',
     'An owner or ownership representative expressly identified as an intended user.', 20),
    ('73000000-0000-4000-8000-000000000003', 'TRUSTEE_EXECUTOR', 'Trustee, Executor, or Fiduciary',
     'A trustee, executor, personal representative, administrator, or other fiduciary expressly identified as an intended user.', 30),
    ('73000000-0000-4000-8000-000000000004', 'ATTORNEY_COUNSEL', 'Attorney or Legal Counsel',
     'Legal counsel expressly identified as an intended user for the specified matter; this classification grants no application-security role.', 40),
    ('73000000-0000-4000-8000-000000000005', 'COURT_TRIBUNAL', 'Court or Tribunal',
     'A court, arbitrator, mediator, tribunal, or similar adjudicative body expressly identified as an intended user.', 50),
    ('73000000-0000-4000-8000-000000000006', 'INSURER', 'Insurer or Claims Representative',
     'An insurer, underwriter, broker, adjuster, or claims representative expressly identified as an intended user.', 60),
    ('73000000-0000-4000-8000-000000000007', 'LENDER', 'Lender or Financial Institution',
     'A lender, credit committee, financial institution, or financing representative expressly identified as an intended user.', 70),
    ('73000000-0000-4000-8000-000000000008', 'TAX_AUTHORITY', 'Tax Authority',
     'A federal, state, local, or other taxing authority expressly identified as an intended user.', 80),
    ('73000000-0000-4000-8000-000000000009', 'GOVERNMENT_AGENCY', 'Government or Regulatory Agency',
     'A governmental or regulatory body, other than a tax authority, expressly identified as an intended user.', 90),
    ('73000000-0000-4000-8000-000000000010', 'CHARITABLE_ORGANIZATION', 'Charitable Organization',
     'A qualified charitable organization or its authorized representative expressly identified as an intended user.', 100),
    ('73000000-0000-4000-8000-000000000011', 'PROSPECTIVE_BUYER', 'Prospective Buyer',
     'A specifically identified prospective purchaser expressly identified as an intended user.', 110),
    ('73000000-0000-4000-8000-000000000012', 'PROSPECTIVE_SELLER', 'Prospective Seller or Consignor',
     'A specifically identified prospective seller, consignor, or disposition representative expressly identified as an intended user.', 120),
    ('73000000-0000-4000-8000-000000000013', 'ADVISOR', 'Professional Advisor',
     'An accountant, wealth advisor, collection manager, consultant, or other professional advisor expressly identified as an intended user.', 130),
    ('73000000-0000-4000-8000-000000000014', 'OTHER_NAMED_PARTY', 'Other Named Party',
     'A specifically named intended user not adequately represented by another classification. The record must describe the party and relationship.', 900);

DELIMITER $$

DROP PROCEDURE IF EXISTS validate_client_intended_user_seed_v1_0$$
CREATE PROCEDURE validate_client_intended_user_seed_v1_0()
BEGIN
    DECLARE issue_count INT DEFAULT 0;

    -- Explicit duplicate checks remain as migration assertions even though the
    -- temporary tables also carry unique keys. They document Section 14.1.2 and
    -- protect the validation if staging-table constraints are later changed.
    SELECT COUNT(*) INTO issue_count
    FROM (
        SELECT category_code FROM seed_use_categories
        GROUP BY category_code HAVING COUNT(*) > 1
    ) AS duplicate_category_codes;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed validation failed: duplicate use_categories.category_code values.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM (
        SELECT intended_use_code FROM seed_intended_uses
        GROUP BY intended_use_code HAVING COUNT(*) > 1
    ) AS duplicate_intended_use_codes;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed validation failed: duplicate intended_uses.intended_use_code values.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM (
        SELECT user_type_code FROM seed_intended_user_types
        GROUP BY user_type_code HAVING COUNT(*) > 1
    ) AS duplicate_user_type_codes;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed validation failed: duplicate intended_user_types.user_type_code values.';
    END IF;

    -- Validate staged parent references independently of the target database.
    SELECT COUNT(*) INTO issue_count
    FROM seed_intended_uses AS iu
    LEFT JOIN seed_use_categories AS uc
        ON uc.use_category_id = iu.use_category_id
    WHERE uc.use_category_id IS NULL;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed validation failed: an intended use references an unknown seed category.';
    END IF;

    -- A stable ID and code are a permanent pair. Abort rather than silently
    -- remapping either side when a database already contains conflicting data.
    SELECT COUNT(*) INTO issue_count
    FROM seed_use_categories AS s
    INNER JOIN use_categories AS t
        ON t.use_category_id = s.use_category_id
       AND BINARY t.category_code <> BINARY s.category_code;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed conflict: a use-category UUID is already assigned to another code.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM seed_use_categories AS s
    INNER JOIN use_categories AS t
        ON BINARY t.category_code = BINARY s.category_code
       AND t.use_category_id <> s.use_category_id;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed conflict: a use-category code is already assigned to another UUID.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM seed_intended_uses AS s
    INNER JOIN intended_uses AS t
        ON t.intended_use_id = s.intended_use_id
       AND BINARY t.intended_use_code <> BINARY s.intended_use_code;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed conflict: an intended-use UUID is already assigned to another code.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM seed_intended_uses AS s
    INNER JOIN intended_uses AS t
        ON BINARY t.intended_use_code = BINARY s.intended_use_code
       AND t.intended_use_id <> s.intended_use_id;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed conflict: an intended-use code is already assigned to another UUID.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM seed_intended_user_types AS s
    INNER JOIN intended_user_types AS t
        ON t.user_type_id = s.user_type_id
       AND BINARY t.user_type_code <> BINARY s.user_type_code;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed conflict: an intended-user-type UUID is already assigned to another code.';
    END IF;

    SELECT COUNT(*) INTO issue_count
    FROM seed_intended_user_types AS s
    INNER JOIN intended_user_types AS t
        ON BINARY t.user_type_code = BINARY s.user_type_code
       AND t.user_type_id <> s.user_type_id;
    IF issue_count > 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Seed conflict: an intended-user-type code is already assigned to another UUID.';
    END IF;
END$$

DELIMITER ;

CALL validate_client_intended_user_seed_v1_0();
DROP PROCEDURE validate_client_intended_user_seed_v1_0;

START TRANSACTION;

INSERT INTO use_categories
    (use_category_id, category_code, use_category_name, description, sort_order,
     is_active, effective_from_utc, effective_to_utc, row_version,
     created_at_utc, updated_at_utc)
SELECT
    s.use_category_id, s.category_code, s.use_category_name, s.description,
    s.sort_order, TRUE, @seed_effective_from_utc, NULL, 1,
    @seed_effective_from_utc, @seed_effective_from_utc
FROM seed_use_categories AS s
LEFT JOIN use_categories AS t
    ON t.use_category_id = s.use_category_id
WHERE t.use_category_id IS NULL;

INSERT INTO intended_uses
    (intended_use_id, use_category_id, intended_use_code, intended_use_name,
     description, default_restrictions, requires_custom_narrative, is_active,
     effective_from_utc, effective_to_utc, row_version,
     created_at_utc, updated_at_utc)
SELECT
    s.intended_use_id, s.use_category_id, s.intended_use_code,
    s.intended_use_name, s.description, s.default_restrictions,
    s.requires_custom_narrative, TRUE, @seed_effective_from_utc, NULL, 1,
    @seed_effective_from_utc, @seed_effective_from_utc
FROM seed_intended_uses AS s
LEFT JOIN intended_uses AS t
    ON t.intended_use_id = s.intended_use_id
WHERE t.intended_use_id IS NULL;

INSERT INTO intended_user_types
    (user_type_id, user_type_code, user_type_name, description, sort_order,
     is_active, row_version, created_at_utc, updated_at_utc)
SELECT
    s.user_type_id, s.user_type_code, s.user_type_name, s.description,
    s.sort_order, TRUE, 1, @seed_effective_from_utc, @seed_effective_from_utc
FROM seed_intended_user_types AS s
LEFT JOIN intended_user_types AS t
    ON t.user_type_id = s.user_type_id
WHERE t.user_type_id IS NULL;

COMMIT;

-- Supporting indexes. The table creation scripts already define the primary
-- UI indexes; these code-first indexes make code audit and FK traversal direct.
CREATE INDEX IF NOT EXISTS idx_use_categories_code_active
    ON use_categories (category_code, is_active);

CREATE INDEX IF NOT EXISTS idx_intended_uses_code_active
    ON intended_uses (intended_use_code, is_active);

CREATE INDEX IF NOT EXISTS idx_intended_uses_category_sort
    ON intended_uses (use_category_id, is_active, intended_use_name);

CREATE INDEX IF NOT EXISTS idx_intended_user_types_code_active
    ON intended_user_types (user_type_code, is_active);

-- Post-seed validation. Every result set should return zero rows.

-- 1. No duplicate persisted category codes.
SELECT category_code, COUNT(*) AS duplicate_count
FROM use_categories
GROUP BY category_code
HAVING COUNT(*) > 1;

-- 2. No duplicate persisted intended-use codes.
SELECT intended_use_code, COUNT(*) AS duplicate_count
FROM intended_uses
GROUP BY intended_use_code
HAVING COUNT(*) > 1;

-- 3. No duplicate persisted intended-user-type codes.
SELECT user_type_code, COUNT(*) AS duplicate_count
FROM intended_user_types
GROUP BY user_type_code
HAVING COUNT(*) > 1;

-- 4. All canonical seed rows exist with their stable UUID/code pairing.
SELECT 'use_categories' AS seed_table, s.use_category_id AS seed_id,
       s.category_code AS seed_code
FROM seed_use_categories AS s
LEFT JOIN use_categories AS t
    ON t.use_category_id = s.use_category_id
   AND BINARY t.category_code = BINARY s.category_code
WHERE t.use_category_id IS NULL
UNION ALL
SELECT 'intended_uses', s.intended_use_id, s.intended_use_code
FROM seed_intended_uses AS s
LEFT JOIN intended_uses AS t
    ON t.intended_use_id = s.intended_use_id
   AND BINARY t.intended_use_code = BINARY s.intended_use_code
WHERE t.intended_use_id IS NULL
UNION ALL
SELECT 'intended_user_types', s.user_type_id, s.user_type_code
FROM seed_intended_user_types AS s
LEFT JOIN intended_user_types AS t
    ON t.user_type_id = s.user_type_id
   AND BINARY t.user_type_code = BINARY s.user_type_code
WHERE t.user_type_id IS NULL;

DROP TEMPORARY TABLE IF EXISTS seed_intended_user_types;
DROP TEMPORARY TABLE IF EXISTS seed_intended_uses;
DROP TEMPORARY TABLE IF EXISTS seed_use_categories;

-- Important use rule:
-- These definitions organize and guide appraisal-request intake. They do not
-- establish an appraisal's authoritative intended use or intended users, and
-- intended use must never be inferred from appraisal type, report name, client
-- type, or recipient. Acceptance/conversion must preserve the reviewed snapshot.
