-- ============================================================================
-- ONE Classics Nexus
-- SQL Patch: Add username column to users table
-- Version: 1.0
-- Target DB: MariaDB / one_classics_nexus
-- Purpose:
--   Adds a username column to the users table and backfills existing rows.
--   The script is designed to be idempotent and safe to rerun.
-- ============================================================================

-- Optional: uncomment if you want the script to explicitly select the database.
-- USE one_classics_nexus;

SET @database_name = DATABASE();
SET @table_name = 'users';
SET @column_name = 'username';

-- ---------------------------------------------------------------------------
-- 1. Validate target database/table context
-- ---------------------------------------------------------------------------
SELECT
    CASE
        WHEN @database_name IS NULL THEN 'WARNING: No database selected. Run USE one_classics_nexus; before executing this script.'
        ELSE CONCAT('Running against database: ', @database_name)
    END AS execution_context;

SELECT COUNT(*)
INTO @users_table_exists
FROM information_schema.tables
WHERE table_schema = @database_name
  AND table_name = @table_name;

-- ---------------------------------------------------------------------------
-- 2. Add users.username if missing
-- ---------------------------------------------------------------------------
SELECT COUNT(*)
INTO @username_column_exists
FROM information_schema.columns
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = @column_name;

SELECT COUNT(*)
INTO @email_column_exists
FROM information_schema.columns
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = 'email';

SET @after_email_clause = IF(@email_column_exists = 1, ' AFTER `email`', '');

SET @add_username_sql = IF(
    @users_table_exists = 1 AND @username_column_exists = 0,
    CONCAT(
        'ALTER TABLE `', @table_name, '` ',
        'ADD COLUMN `username` VARCHAR(100) NULL',
        @after_email_clause,
        ';'
    ),
    'SELECT ''Skipped: users.username already exists or users table was not found.'' AS message;'
);

PREPARE stmt FROM @add_username_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ---------------------------------------------------------------------------
-- 3. Backfill username from email local-part when possible
--    Example: admin.one@nexus.test -> admin.one
-- ---------------------------------------------------------------------------
SELECT COUNT(*)
INTO @username_column_exists_after_add
FROM information_schema.columns
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = @column_name;

SET @backfill_from_email_sql = IF(
    @users_table_exists = 1
    AND @username_column_exists_after_add = 1
    AND @email_column_exists = 1,
    CONCAT(
        'UPDATE `', @table_name, '` ',
        'SET `username` = LOWER(TRIM(SUBSTRING_INDEX(`email`, ''@'', 1))) ',
        'WHERE (`username` IS NULL OR TRIM(`username`) = '''') ',
        'AND `email` IS NOT NULL ',
        'AND TRIM(`email`) <> '''';'
    ),
    'SELECT ''Skipped: email-based username backfill could not run.'' AS message;'
);

PREPARE stmt FROM @backfill_from_email_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ---------------------------------------------------------------------------
-- 4. Backfill any remaining blank usernames using the primary identifier
--    Preferred identifier: user_id
--    Fallback identifier: id
-- ---------------------------------------------------------------------------
SELECT COUNT(*)
INTO @user_id_column_exists
FROM information_schema.columns
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = 'user_id';

SELECT COUNT(*)
INTO @id_column_exists
FROM information_schema.columns
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = 'id';

SET @identifier_column = CASE
    WHEN @user_id_column_exists = 1 THEN 'user_id'
    WHEN @id_column_exists = 1 THEN 'id'
    ELSE NULL
END;

SET @fallback_username_sql = IF(
    @users_table_exists = 1
    AND @username_column_exists_after_add = 1
    AND @identifier_column IS NOT NULL,
    CONCAT(
        'UPDATE `', @table_name, '` ',
        'SET `username` = CONCAT(''user_'', LEFT(REPLACE(CAST(`', @identifier_column, '` AS CHAR), ''-'', ''''), 8)) ',
        'WHERE `username` IS NULL OR TRIM(`username`) = '''';'
    ),
    'SELECT ''Skipped: identifier-based username backfill could not run.'' AS message;'
);

PREPARE stmt FROM @fallback_username_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ---------------------------------------------------------------------------
-- 5. Resolve duplicate usernames before adding a unique index
--    Keeps the lowest identifier value unchanged and appends a deterministic
--    suffix to duplicate rows.
-- ---------------------------------------------------------------------------
SET @dedupe_username_sql = IF(
    @users_table_exists = 1
    AND @username_column_exists_after_add = 1
    AND @identifier_column IS NOT NULL,
    CONCAT(
        'UPDATE `', @table_name, '` u ',
        'JOIN ( ',
        '    SELECT `username`, MIN(CAST(`', @identifier_column, '` AS CHAR)) AS keep_identifier ',
        '    FROM `', @table_name, '` ',
        '    WHERE `username` IS NOT NULL AND TRIM(`username`) <> '''' ',
        '    GROUP BY `username` ',
        '    HAVING COUNT(*) > 1 ',
        ') d ',
        'ON u.`username` = d.`username` ',
        'AND CAST(u.`', @identifier_column, '` AS CHAR) <> d.keep_identifier ',
        'SET u.`username` = CONCAT( ',
        '    u.`username`, ''_'', ',
        '    LEFT(REPLACE(CAST(u.`', @identifier_column, '` AS CHAR), ''-'', ''''), 6) ',
        ');'
    ),
    'SELECT ''Skipped: username duplicate cleanup could not run.'' AS message;'
);

PREPARE stmt FROM @dedupe_username_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ---------------------------------------------------------------------------
-- 6. Make username NOT NULL when all rows have a value
-- ---------------------------------------------------------------------------
SET @count_null_usernames_sql = IF(
    @users_table_exists = 1 AND @username_column_exists_after_add = 1,
    CONCAT(
        'SELECT COUNT(*) INTO @null_username_count ',
        'FROM `', @table_name, '` ',
        'WHERE `username` IS NULL OR TRIM(`username`) = '''';'
    ),
    'SET @null_username_count = 1;'
);

PREPARE stmt FROM @count_null_usernames_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @make_username_not_null_sql = IF(
    @users_table_exists = 1
    AND @username_column_exists_after_add = 1
    AND @null_username_count = 0,
    CONCAT(
        'ALTER TABLE `', @table_name, '` ',
        'MODIFY COLUMN `username` VARCHAR(100) NOT NULL;'
    ),
    'SELECT ''Skipped: users.username remains nullable because at least one row has no username.'' AS message;'
);

PREPARE stmt FROM @make_username_not_null_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ---------------------------------------------------------------------------
-- 7. Add a unique index for username if missing and if duplicates do not exist
-- ---------------------------------------------------------------------------
SELECT COUNT(*)
INTO @username_index_exists
FROM information_schema.statistics
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND index_name = 'ux_users_username';

SET @count_duplicate_usernames_sql = IF(
    @users_table_exists = 1 AND @username_column_exists_after_add = 1,
    CONCAT(
        'SELECT COUNT(*) INTO @duplicate_username_count ',
        'FROM ( ',
        '    SELECT `username` ',
        '    FROM `', @table_name, '` ',
        '    WHERE `username` IS NOT NULL AND TRIM(`username`) <> '''' ',
        '    GROUP BY `username` ',
        '    HAVING COUNT(*) > 1 ',
        ') duplicate_usernames;'
    ),
    'SET @duplicate_username_count = 1;'
);

PREPARE stmt FROM @count_duplicate_usernames_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @add_username_index_sql = IF(
    @users_table_exists = 1
    AND @username_column_exists_after_add = 1
    AND @username_index_exists = 0
    AND @duplicate_username_count = 0,
    CONCAT(
        'CREATE UNIQUE INDEX `ux_users_username` ',
        'ON `', @table_name, '` (`username`);'
    ),
    'SELECT ''Skipped: unique index ux_users_username already exists, duplicates remain, or users table was not found.'' AS message;'
);

PREPARE stmt FROM @add_username_index_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ---------------------------------------------------------------------------
-- 8. Verification
-- ---------------------------------------------------------------------------
SELECT
    table_name,
    column_name,
    column_type,
    is_nullable,
    column_default
FROM information_schema.columns
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = @column_name;

SELECT
    table_name,
    index_name,
    non_unique,
    column_name
FROM information_schema.statistics
WHERE table_schema = @database_name
  AND table_name = @table_name
  AND column_name = @column_name
ORDER BY index_name, seq_in_index;

SET @preview_users_sql = IF(
    @users_table_exists = 1 AND @username_column_exists_after_add = 1,
    CONCAT(
        'SELECT * FROM `', @table_name, '` ',
        'ORDER BY `username` ',
        'LIMIT 25;'
    ),
    'SELECT ''Preview skipped: users.username was not available.'' AS message;'
);

PREPARE stmt FROM @preview_users_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
