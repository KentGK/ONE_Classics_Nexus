# ONE Classics Nexus — Appraisal Status Schema Update Script

**Version:** 1.0  
**Target DB:** MariaDB  
**Purpose:** Add a standardized `appraisal_statuses` lookup table and add `appraisal_status_id` to the `appraisals` table.

---

## 1. Design Summary

This script introduces a controlled appraisal workflow status model.

### Standardized Appraisal Statuses

| Code | Name | Description |
|---|---|---|
| `DRAFT` | Draft | Appraisal record has been created but work has not started. |
| `INTAKE` | Intake | Vehicle/appraisal intake data is being collected and validated. |
| `PENDING_INSPECTION` | Pending Inspection | Waiting for inspection scheduling or appraiser action. |
| `IN_INSPECTION` | In Inspection | Inspection and condition scoring are actively in progress. |
| `INSPECTION_COMPLETE` | Inspection Complete | Inspection scoring completed and ready for valuation. |
| `PENDING_COMPARABLES` | Pending Comparables | Comparable vehicle data collection is in progress. |
| `VALUATION_IN_PROGRESS` | Valuation In Progress | AVM valuation engine execution underway. |
| `PENDING_REVIEW` | Pending Review | Requires reviewer, QA, or red-flag review. |
| `COMPLETED` | Completed | Appraisal finalized and valuation complete. |
| `REPORT_GENERATED` | Report Generated | Report package has been generated/exported. |
| `DELIVERED` | Delivered | Appraisal delivered to client/requestor. |
| `CANCELLED` | Cancelled | Appraisal cancelled before completion. |
| `ARCHIVED` | Archived | Closed and retained for historical/audit purposes. |

---

## 2. SQL Script

```sql
-- =====================================================================
-- ONE Classics Nexus
-- Appraisal Status Lookup + Appraisals Column Update
-- Version: 1.0
-- Target: MariaDB
-- =====================================================================

USE one_classics_nexus;

-- =====================================================================
-- 1. Create appraisal_statuses lookup table
-- =====================================================================

CREATE TABLE IF NOT EXISTS appraisal_statuses (
    appraisal_status_id        CHAR(36)      NOT NULL,
    appraisal_status_code      VARCHAR(40)   NOT NULL,
    appraisal_status_name      VARCHAR(100)  NOT NULL,
    description                TEXT          NULL,
    is_terminal                BOOLEAN       NOT NULL DEFAULT FALSE,
    is_active                  BOOLEAN       NOT NULL DEFAULT TRUE,
    display_order              INT           NOT NULL DEFAULT 0,
    created_at_utc             DATETIME      NOT NULL DEFAULT UTC_TIMESTAMP(),
    updated_at_utc             DATETIME      NULL,

    PRIMARY KEY (appraisal_status_id),
    UNIQUE KEY ux_appraisal_status_code (appraisal_status_code),
    UNIQUE KEY ux_appraisal_status_name (appraisal_status_name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- 2. Seed standardized appraisal statuses
-- =====================================================================

INSERT INTO appraisal_statuses
(
    appraisal_status_id,
    appraisal_status_code,
    appraisal_status_name,
    description,
    is_terminal,
    is_active,
    display_order
)
VALUES
(UUID(), 'DRAFT', 'Draft',
 'Appraisal record has been created but work has not started.',
 FALSE, TRUE, 10),

(UUID(), 'INTAKE', 'Intake',
 'Vehicle/appraisal intake data is being collected and validated.',
 FALSE, TRUE, 20),

(UUID(), 'PENDING_INSPECTION', 'Pending Inspection',
 'Waiting for inspection scheduling or appraiser action.',
 FALSE, TRUE, 30),

(UUID(), 'IN_INSPECTION', 'In Inspection',
 'Inspection and condition scoring are actively in progress.',
 FALSE, TRUE, 40),

(UUID(), 'INSPECTION_COMPLETE', 'Inspection Complete',
 'Inspection scoring completed and ready for valuation.',
 FALSE, TRUE, 50),

(UUID(), 'PENDING_COMPARABLES', 'Pending Comparables',
 'Comparable vehicle data collection is in progress.',
 FALSE, TRUE, 60),

(UUID(), 'VALUATION_IN_PROGRESS', 'Valuation In Progress',
 'AVM valuation engine execution underway.',
 FALSE, TRUE, 70),

(UUID(), 'PENDING_REVIEW', 'Pending Review',
 'Requires reviewer, QA, or red-flag review.',
 FALSE, TRUE, 80),

(UUID(), 'COMPLETED', 'Completed',
 'Appraisal finalized and valuation complete.',
 TRUE, TRUE, 90),

(UUID(), 'REPORT_GENERATED', 'Report Generated',
 'Report package has been generated/exported.',
 TRUE, TRUE, 100),

(UUID(), 'DELIVERED', 'Delivered',
 'Appraisal delivered to client/requestor.',
 TRUE, TRUE, 110),

(UUID(), 'CANCELLED', 'Cancelled',
 'Appraisal cancelled before completion.',
 TRUE, TRUE, 120),

(UUID(), 'ARCHIVED', 'Archived',
 'Closed and retained for historical/audit purposes.',
 TRUE, TRUE, 130)

ON DUPLICATE KEY UPDATE
    appraisal_status_name = VALUES(appraisal_status_name),
    description = VALUES(description),
    is_terminal = VALUES(is_terminal),
    is_active = VALUES(is_active),
    display_order = VALUES(display_order),
    updated_at_utc = UTC_TIMESTAMP();

-- =====================================================================
-- 3. Add appraisal_status_id column to appraisals if missing
-- =====================================================================

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'appraisals'
      AND COLUMN_NAME = 'appraisal_status_id'
);

SET @sql := IF(
    @column_exists = 0,
    'ALTER TABLE appraisals ADD COLUMN appraisal_status_id CHAR(36) NULL AFTER appraisal_type_id',
    'SELECT ''Column appraisals.appraisal_status_id already exists'' AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- =====================================================================
-- 4. Backfill existing appraisals
-- =====================================================================

UPDATE appraisals a
JOIN appraisal_statuses s
    ON s.appraisal_status_code = 'DRAFT'
SET a.appraisal_status_id = s.appraisal_status_id
WHERE a.appraisal_status_id IS NULL;

-- =====================================================================
-- 5. Add foreign key if missing
-- =====================================================================

SET @fk_exists := (
    SELECT COUNT(*)
    FROM information_schema.TABLE_CONSTRAINTS
    WHERE CONSTRAINT_SCHEMA = DATABASE()
      AND TABLE_NAME = 'appraisals'
      AND CONSTRAINT_NAME = 'fk_appraisals_appraisal_status'
      AND CONSTRAINT_TYPE = 'FOREIGN KEY'
);

SET @sql := IF(
    @fk_exists = 0,
    'ALTER TABLE appraisals ADD CONSTRAINT fk_appraisals_appraisal_status FOREIGN KEY (appraisal_status_id) REFERENCES appraisal_statuses(appraisal_status_id)',
    'SELECT ''Foreign key fk_appraisals_appraisal_status already exists'' AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- =====================================================================
-- 6. Add supporting index if missing
-- =====================================================================

SET @index_exists := (
    SELECT COUNT(*)
    FROM information_schema.STATISTICS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'appraisals'
      AND INDEX_NAME = 'ix_appraisals_appraisal_status_id'
);

SET @sql := IF(
    @index_exists = 0,
    'CREATE INDEX ix_appraisals_appraisal_status_id ON appraisals(appraisal_status_id)',
    'SELECT ''Index ix_appraisals_appraisal_status_id already exists'' AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- =====================================================================
-- 7. Verification
-- =====================================================================

SELECT
    appraisal_status_code,
    appraisal_status_name,
    is_terminal,
    is_active,
    display_order
FROM appraisal_statuses
ORDER BY display_order;

SELECT
    COLUMN_NAME,
    COLUMN_TYPE,
    IS_NULLABLE
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME = 'appraisals'
  AND COLUMN_NAME = 'appraisal_status_id';

-- =====================================================================
-- End of Script
-- =====================================================================
```

---

## 3. DBA Notes

- `appraisal_status_id` is the canonical FK column.
- `appraisal_status_code` is the stable business/API identifier.
- Existing appraisals are backfilled to `DRAFT`.
- Script is idempotent and safe to rerun.
- `status` on `appraisals` should eventually be deprecated in favor of the lookup-driven model.

## 4. Recommended API Mapping

| API / DTO Property | DB Column |
|---|---|
| `statusCode` | `appraisal_statuses.appraisal_status_code` |
| `statusName` | `appraisal_statuses.appraisal_status_name` |
| `appraisalStatusId` | `appraisals.appraisal_status_id` |

## 5. Recommended Repository Query

```sql
SELECT
    a.appraisal_id,
    a.vehicle_id,
    a.appraisal_type_id,
    a.appraisal_status_id,
    s.appraisal_status_code,
    s.appraisal_status_name,
    a.notes,
    a.row_version,
    a.created_at_utc,
    a.updated_at_utc
FROM appraisals a
JOIN appraisal_statuses s
    ON s.appraisal_status_id = a.appraisal_status_id
WHERE a.appraisal_id = ?;
```
