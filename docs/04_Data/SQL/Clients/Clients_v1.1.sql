-- ============================================================================
-- SUPERSEDED - DO NOT BUILD FROM THIS FILE
-- Replaced by: docs/04_Data/SQL/Clients/Clients_v2.0.sql (LOCKED v2.0)
-- Migration:   docs/04_Data/SQL/Clients/Update_Clients_To_v2.0.sql
-- Retained for historical reference only. This revision lacks Status_Code.
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
  `organization_type` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Client master records for appraisal request intake and appraisal workflows';