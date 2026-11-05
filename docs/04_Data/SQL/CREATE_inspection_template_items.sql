-- one_classics_nexus_test.inspection_template_items definition

CREATE TABLE `inspection_template_items` (
  `inspection_template_item_id` char(36) NOT NULL,
  `inspection_template_section_id` char(36) NOT NULL,
  `item_key` varchar(80) NOT NULL,
  `display_name` varchar(200) NOT NULL,
  `help_text` text DEFAULT NULL,
  `response_type` varchar(30) NOT NULL,
  `default_weight` decimal(9,6) DEFAULT NULL,
  `allows_red_flag` tinyint(1) NOT NULL DEFAULT 1,
  `is_required` tinyint(1) NOT NULL DEFAULT 0,
  `sort_order` int(11) NOT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  PRIMARY KEY (`inspection_template_item_id`),
  UNIQUE KEY `uq_template_items_section_key` (`inspection_template_section_id`,`item_key`),
  KEY `idx_template_items_section` (`inspection_template_section_id`),
  CONSTRAINT `fk_template_items_section` FOREIGN KEY (`inspection_template_section_id`) REFERENCES `inspection_template_sections` (`inspection_template_section_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;