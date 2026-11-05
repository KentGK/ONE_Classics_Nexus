-- one_classics_nexus_test.inspection_template_sections definition

CREATE TABLE `inspection_template_sections` (
  `inspection_template_section_id` char(36) NOT NULL,
  `inspection_template_id` char(36) NOT NULL,
  `system_number` int(11) NOT NULL,
  `system_key` varchar(50) NOT NULL,
  `section_name` varchar(150) NOT NULL,
  `description` text DEFAULT NULL,
  `weight` decimal(9,6) NOT NULL,
  `sort_order` int(11) NOT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime NOT NULL,
  `is_required` tinyint(1) NOT NULL,
  PRIMARY KEY (`inspection_template_section_id`),
  UNIQUE KEY `uq_template_sections_template_system` (`inspection_template_id`,`system_number`),
  KEY `idx_template_sections_template` (`inspection_template_id`),
  KEY `idx_template_sections_system_key` (`system_key`),
  CONSTRAINT `fk_template_sections_template` FOREIGN KEY (`inspection_template_id`) REFERENCES `inspection_templates` (`inspection_template_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;