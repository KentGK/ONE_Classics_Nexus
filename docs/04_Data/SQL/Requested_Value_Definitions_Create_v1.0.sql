-- one_classics_nexus_test.requested_value_definitions definition

CREATE TABLE `requested_value_definitions` (
  `requested_value_definition_id` char(36) NOT NULL,
  `value_definition_code` varchar(50) NOT NULL,
  `value_definition_name` varchar(150) NOT NULL,
  `definition_text` text NOT NULL,
  `source_reference` varchar(255) DEFAULT NULL,
  `jurisdiction_scope` varchar(100) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `sort_order` int(11) NOT NULL DEFAULT 100,
  `row_version` bigint(20) unsigned NOT NULL DEFAULT 1,
  `created_at_utc` datetime(6) NOT NULL DEFAULT utc_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT utc_timestamp(6) ON UPDATE utc_timestamp(6),
  PRIMARY KEY (`requested_value_definition_id`),
  UNIQUE KEY `uq_requested_value_definitions_code` (`value_definition_code`),
  KEY `idx_requested_value_definitions_active_sort` (`is_active`,`sort_order`,`value_definition_name`),
  CONSTRAINT `ck_requested_value_definitions_id_uuid` CHECK (cast(`requested_value_definition_id` as char charset binary) = cast(lcase(`requested_value_definition_id`) as char charset binary) and cast(`requested_value_definition_id` as char charset binary) regexp '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'),
  CONSTRAINT `ck_requested_value_definitions_code_required` CHECK (char_length(trim(`value_definition_code`)) > 0),
  CONSTRAINT `ck_requested_value_definitions_name_required` CHECK (char_length(trim(`value_definition_name`)) > 0),
  CONSTRAINT `ck_requested_value_definitions_definition_required` CHECK (char_length(trim(`definition_text`)) > 0),
  CONSTRAINT `ck_requested_value_definitions_row_version` CHECK (`row_version` >= 1),
  CONSTRAINT `ck_requested_value_definitions_audit_dates` CHECK (`updated_at_utc` >= `created_at_utc`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Client-requested value-definition reference data for appraisal intake.';
