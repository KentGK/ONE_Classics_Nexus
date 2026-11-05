-- ============================================================================
-- ONE Classics Nexus
-- 4.1 CLIENTS - MariaDB CREATE Script
-- Version: 2.0 (LOCKED)
-- Supersedes: Clients_v1.1.sql, Clients.sql, Clients_Create_v1.0.sql
-- Target: MariaDB 10.5+
-- ----------------------------------------------------------------------------
-- Changes from v1.1:
--   * Added Status_Code VARCHAR(20) NOT NULL DEFAULT 'Active'.
--     Values are stored in mixed case ('Active', 'Inactive', 'Archived') and
--     are surfaced to the UI verbatim, with no case mapping layer.
--   * Declared the primary key, unique client_number, and supporting indexes
--     that earlier revisions omitted from the table body.
--   * Confirmed removal of the legacy status / is_active / country columns.
-- ============================================================================

-- one_classics_nexus_test.clients definition

CREATE TABLE `clients` (
  `client_id` char(36) NOT NULL,
  `client_number` varchar(30) NOT NULL,
  `client_type_code` varchar(30) NOT NULL,
  `first_name` varchar(100) DEFAULT NULL,
  `last_name` varchar(100) DEFAULT NULL,
  `middle_name` varchar(100) DEFAULT NULL,
  `display_name` varchar(200) NOT NULL,
  `address_line1` varchar(200) DEFAULT NULL,
  `address_line2` varchar(200) DEFAULT NULL,
  `city` varchar(100) DEFAULT NULL,
  `state_region` varchar(100) DEFAULT NULL,
  `postal_code` varchar(30) DEFAULT NULL,
  `email` varchar(254) NOT NULL,
  `phone` varchar(40) NOT NULL,
  `preferred_contact_method` varchar(20) DEFAULT NULL,
  `notes` text DEFAULT NULL,
  `row_version` bigint(20) unsigned NOT NULL DEFAULT 1,
  `created_at_utc` datetime(6) NOT NULL DEFAULT utc_timestamp(6),
  `created_by_user_id` char(36) NOT NULL,
  `updated_at_utc` datetime(6) NOT NULL DEFAULT utc_timestamp(6),
  `updated_by_user_id` char(36) NOT NULL,
  `organization_name` varchar(200) DEFAULT NULL,
  `organization_type` varchar(100) DEFAULT NULL,
  `Status_Code` varchar(20) NOT NULL DEFAULT 'Active',
  PRIMARY KEY (`client_id`),
  UNIQUE KEY `uq_clients_client_number` (`client_number`),
  KEY `idx_clients_display_name` (`display_name`),
  KEY `idx_clients_email` (`email`),
  KEY `idx_clients_type` (`client_type_code`),
  KEY `idx_clients_status_display` (`Status_Code`, `display_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Client master records for appraisal request intake and appraisal workflows';

-- ----------------------------------------------------------------------------
-- Index notes
-- ----------------------------------------------------------------------------
-- idx_clients_status_display backs the Client List default view, which filters
-- Status_Code = 'Active' and sorts by display_name. Keeping the columns in that
-- order lets a single index satisfy both the filter and the sort.
--
-- idx_clients_display_name and idx_clients_email back the Client List search,
-- which matches partially against display_name and email. Leading-wildcard
-- searches cannot use these indexes; they assist anchored prefix matches only.
