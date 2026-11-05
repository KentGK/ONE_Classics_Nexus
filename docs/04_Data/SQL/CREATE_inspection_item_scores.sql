-- one_classics_nexus_test.inspection_item_scores definition

CREATE TABLE `inspection_item_scores` (
  `inspection_item_score_id` char(36) NOT NULL,
  `inspection_id` char(36) NOT NULL,
  `appraisal_id` char(36) NOT NULL,
  `inspection_template_section_id` char(36) NOT NULL,
  `inspection_template_item_id` char(36) NOT NULL,
  `system_number` int(11) NOT NULL,
  `item_key` varchar(80) NOT NULL,
  `score_0_to_5` decimal(4,2) DEFAULT NULL,
  `boolean_response` tinyint(1) DEFAULT NULL,
  `numeric_response` decimal(18,4) DEFAULT NULL,
  `text_response` text DEFAULT NULL,
  `is_red_flag` tinyint(1) NOT NULL DEFAULT 0,
  `evidence_note` text DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`inspection_item_score_id`),
  KEY `idx_item_scores_appraisal` (`appraisal_id`),
  KEY `idx_item_scores_inspection` (`inspection_id`),
  KEY `idx_item_scores_section` (`inspection_template_section_id`),
  KEY `idx_item_scores_item` (`inspection_template_item_id`),
  CONSTRAINT `fk_item_scores_appraisal` FOREIGN KEY (`appraisal_id`) REFERENCES `appraisals` (`appraisal_id`),
  CONSTRAINT `fk_item_scores_inspection` FOREIGN KEY (`inspection_id`) REFERENCES `inspections` (`inspection_id`),
  CONSTRAINT `fk_item_scores_item` FOREIGN KEY (`inspection_template_item_id`) REFERENCES `inspection_template_items` (`inspection_template_item_id`),
  CONSTRAINT `fk_item_scores_section` FOREIGN KEY (`inspection_template_section_id`) REFERENCES `inspection_template_sections` (`inspection_template_section_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;