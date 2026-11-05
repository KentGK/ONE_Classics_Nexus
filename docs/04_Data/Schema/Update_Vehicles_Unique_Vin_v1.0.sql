/*
    ONE Classics Nexus
    Script: Update_Vehicles_Unique_Vin_v1.0.sql
    Purpose: Enforce VIN uniqueness on vehicles.

    Background:
    - VehicleRepository.InsertAsync/UpdateAsync catch MySqlException 1062
      (duplicate entry) and translate it to ConflictException -> HTTP 409.
    - vehicles.vin previously carried only a non-unique index
      (KEY idx_vehicles_vin), so error 1062 could never be raised and
      duplicate VINs were silently accepted.
    - This script replaces that index with a UNIQUE key so the database
      enforces the rule the application already assumes.

    Notes:
    - vin remains NULLable. MySQL/MariaDB permit multiple NULL values under a
      UNIQUE index, so vehicles without a VIN are unaffected.
    - Run the verification query below FIRST. ALTER TABLE will fail with
      error 1062 if duplicate non-null VINs exist.
    - idx_vehicles_vin is dropped because uk_vehicles_vin serves the same
      lookup paths; keeping both would duplicate the index.

    Database: MariaDB / MySQL
*/

-- USE one_classics_nexus_test;

-- ---------------------------------------------------------------
-- STEP 1 (verification): must return zero rows before proceeding.
-- ---------------------------------------------------------------
SELECT vin, COUNT(*) AS duplicate_count
FROM vehicles
WHERE vin IS NOT NULL
  AND TRIM(vin) <> ''
GROUP BY vin
HAVING COUNT(*) > 1
ORDER BY duplicate_count DESC;

-- ---------------------------------------------------------------
-- STEP 2: replace the non-unique index with a unique constraint.
-- ---------------------------------------------------------------
ALTER TABLE `vehicles`
    DROP INDEX `idx_vehicles_vin`,
    ADD UNIQUE KEY `uk_vehicles_vin` (`vin`);

-- ---------------------------------------------------------------
-- STEP 3 (confirmation): Non_unique should report 0.
-- ---------------------------------------------------------------
SHOW INDEX FROM `vehicles` WHERE Column_name = 'vin';
