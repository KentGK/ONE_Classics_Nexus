-- one_classics_nexus_test.inspection_templates definition

CREATE TABLE `inspection_templates` (
  `inspection_template_id` char(36) NOT NULL,
  `template_code` varchar(50) NOT NULL,
  `template_family` varchar(30) NOT NULL,
  `template_name` varchar(150) NOT NULL,
  `version_label` varchar(30) NOT NULL,
  `description` text DEFAULT NULL,
  `total_system_count` int(11) NOT NULL DEFAULT 12,
  `status` varchar(30) NOT NULL,
  `effective_from_utc` datetime(6) DEFAULT NULL,
  `effective_to_utc` datetime(6) DEFAULT NULL,
  `created_by_user_id` char(36) DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`inspection_template_id`),
  KEY `idx_inspection_templates_code` (`template_code`),
  KEY `idx_inspection_templates_family` (`template_family`),
  KEY `idx_inspection_templates_created_by` (`created_by_user_id`),
  CONSTRAINT `fk_inspection_templates_created_by` FOREIGN KEY (`created_by_user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;