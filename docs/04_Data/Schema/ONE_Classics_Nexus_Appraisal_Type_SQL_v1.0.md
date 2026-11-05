# ONE Classics Nexus — Appraisal Type Schema Update Script

**Version:** 1.0  
**Target DB:** MariaDB  
**Purpose:** Add a standardized `appraisal_types` lookup table and add `appraisal_type_code` to the `appraisals` table.

---

## 1. Design Summary

This script adds a controlled lookup table for appraisal types and links appraisals to that lookup by code.

### Standardized Appraisal Types

| Code | Description |
|---|---|
| `MARKET_VALUE` | General fair-market-value appraisal based on condition, comparables, and valuation engine output. |
| `INSURANCE` | Appraisal intended to support insurance coverage, stated value, or agreed-value documentation. |
| `PRE_PURCHASE` | Inspection-led appraisal used before purchase to assess condition, risk, and fair market value. |
| `CLASSIC` | Appraisal for vintage/classic collector vehicles where originality, provenance, documentation, and collector value are emphasized. |
| `CUSTOM` | Appraisal for modified, custom-built, restomod, or non-factory-configured vehicles. |
| `ESTATE_PROBATE` | Appraisal for estate, probate, inheritance, or legal valuation support. |
| `DIMINISHED_VALUE` | Appraisal used to estimate value loss after damage, repair, accident history, or title impact. |
| `DONATION_TAX` | Appraisal for charitable donation or tax-support documentation. |
| `LOAN_COLLATERAL` | Appraisal for lender, financing, collateral, or underwriting support. |

---

## 2. SQL Script

```sql
-- =====================================================================
-- ONE Classics Nexus
-- Appraisal Type Lookup + Appraisals Column Update
-- Version: 1.0
-- Target: MariaDB
-- =====================================================================

USE one_classics_nexus;

CREATE TABLE IF NOT EXISTS appraisal_types (
    appraisal_type_code        VARCHAR(30)   NOT NULL,
    appraisal_type_name        VARCHAR(100)  NOT NULL,
    description                TEXT          NULL,
    is_active                  BOOLEAN       NOT NULL DEFAULT TRUE,
    display_order              INT           NOT NULL DEFAULT 0,
    created_at_utc             DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc             DATETIME      NULL,
    PRIMARY KEY (appraisal_type_code),
    UNIQUE KEY ux_appraisal_types_name (appraisal_type_name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO appraisal_types
(
    appraisal_type_code,
    appraisal_type_name,
    description,
    is_active,
    display_order
)
VALUES
('MARKET_VALUE', 'Market Value', 'General fair-market-value appraisal based on condition, comparables, and valuation engine output.', TRUE, 10),
('INSURANCE', 'Insurance', 'Appraisal intended to support insurance coverage, stated value, or agreed-value documentation.', TRUE, 20),
('PRE_PURCHASE', 'Pre-Purchase', 'Inspection-led appraisal used before purchase to assess condition, risk, and fair market value.', TRUE, 30),
('CLASSIC', 'Classic / Vintage', 'Appraisal for vintage/classic collector vehicles where originality, provenance, documentation, and collector value are emphasized.', TRUE, 40),
('CUSTOM', 'Custom / Modified', 'Appraisal for modified, custom-built, restomod, or non-factory-configured vehicles.', TRUE, 50),
('ESTATE_PROBATE', 'Estate / Probate', 'Appraisal for estate, probate, inheritance, or legal valuation support.', TRUE, 60),
('DIMINISHED_VALUE', 'Diminished Value', 'Appraisal used to estimate value loss after damage, repair, accident history, or title impact.', TRUE, 70),
('DONATION_TAX', 'Donation / Tax', 'Appraisal for charitable donation or tax-support documentation.', TRUE, 80),
('LOAN_COLLATERAL', 'Loan / Collateral', 'Appraisal for lender, financing, collateral, or underwriting support.', TRUE, 90)
ON DUPLICATE KEY UPDATE
    appraisal_type_name = VALUES(appraisal_type_name),
    description = VALUES(description),
    is_active = VALUES(is_active),
    display_order = VALUES(display_order),
    updated_at_utc = UTC_TIMESTAMP();

-- Add appraisals.appraisal_type_code if missing.
SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'appraisals'
      AND COLUMN_NAME = 'appraisal_type_code'
);

SET @sql := IF(
    @column_exists = 0,
    'ALTER TABLE appraisals ADD COLUMN appraisal_type_code VARCHAR(30) NOT NULL DEFAULT ''MARKET_VALUE'' AFTER vehicle_id',
    'SELECT ''Column appraisals.appraisal_type_code already exists'' AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

UPDATE appraisals
SET appraisal_type_code = 'MARKET_VALUE'
WHERE appraisal_type_code IS NULL
   OR appraisal_type_code = '';

-- Add FK if missing.
SET @fk_exists := (
    SELECT COUNT(*)
    FROM information_schema.TABLE_CONSTRAINTS
    WHERE CONSTRAINT_SCHEMA = DATABASE()
      AND TABLE_NAME = 'appraisals'
      AND CONSTRAINT_NAME = 'fk_appraisals_appraisal_type'
      AND CONSTRAINT_TYPE = 'FOREIGN KEY'
);

SET @sql := IF(
    @fk_exists = 0,
    'ALTER TABLE appraisals ADD CONSTRAINT fk_appraisals_appraisal_type FOREIGN KEY (appraisal_type_code) REFERENCES appraisal_types(appraisal_type_code)',
    'SELECT ''Foreign key fk_appraisals_appraisal_type already exists'' AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Add index if missing.
SET @index_exists := (
    SELECT COUNT(*)
    FROM information_schema.STATISTICS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'appraisals'
      AND INDEX_NAME = 'ix_appraisals_appraisal_type_code'
);

SET @sql := IF(
    @index_exists = 0,
    'CREATE INDEX ix_appraisals_appraisal_type_code ON appraisals(appraisal_type_code)',
    'SELECT ''Index ix_appraisals_appraisal_type_code already exists'' AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SELECT
    appraisal_type_code,
    appraisal_type_name,
    is_active,
    display_order
FROM appraisal_types
ORDER BY display_order;

SELECT
    COLUMN_NAME,
    COLUMN_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME = 'appraisals'
  AND COLUMN_NAME = 'appraisal_type_code';
```

---

## 3. DBA Notes

- `appraisal_type_code` is the canonical column.
- `MARKET_VALUE` is the default value for existing and future appraisals.
- The script is designed to be safely re-runnable.
- Application DTOs should expose this as either `appraisalTypeCode` or `appraisalType`, but repository SQL should persist to `appraisal_type_code`.

---

## 4. Recommended Application Mapping

| API / DTO Property | DB Column |
|---|---|
| `appraisalTypeCode` | `appraisals.appraisal_type_code` |
| `appraisalTypeName` | `appraisal_types.appraisal_type_name` |
| `description` | `appraisal_types.description` |

---

## 5. Recommended Repository Query

```sql
SELECT
    a.appraisal_id,
    a.vehicle_id,
    a.appraisal_type_code,
    at.appraisal_type_name,
    a.status,
    a.requested_by,
    a.assigned_to,
    a.notes,
    a.row_version,
    a.created_at_utc,
    a.updated_at_utc
FROM appraisals a
JOIN appraisal_types at
    ON at.appraisal_type_code = a.appraisal_type_code
WHERE a.appraisal_id = ?;
```
