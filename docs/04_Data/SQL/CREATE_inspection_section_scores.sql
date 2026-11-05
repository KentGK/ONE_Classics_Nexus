-- one_classics_nexus_test.inspection_section_scores definition

CREATE TABLE `inspection_section_scores` (
  `inspection_section_score_id` char(36) NOT NULL,
  `inspection_id` char(36) NOT NULL,
  `appraisal_id` char(36) NOT NULL,
  `inspection_template_section_id` char(36) NOT NULL,
  `system_number` int(11) NOT NULL,
  `system_key` varchar(50) NOT NULL,
  `score_0_to_5` decimal(4,2) NOT NULL,
  `normalized_score_0_to_100` decimal(6,2) DEFAULT NULL,
  `weight` decimal(9,6) NOT NULL,
  `weighted_score` decimal(12,6) DEFAULT NULL,
  `red_flag_count` int(11) NOT NULL DEFAULT 0,
  `notes` text DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`inspection_section_score_id`),
  UNIQUE KEY `uq_section_scores_inspection_system` (`inspection_id`,`system_number`),
  KEY `idx_section_scores_appraisal` (`appraisal_id`),
  KEY `idx_section_scores_inspection` (`inspection_id`),
  KEY `idx_section_scores_section` (`inspection_template_section_id`),
  CONSTRAINT `fk_section_scores_appraisal` FOREIGN KEY (`appraisal_id`) REFERENCES `appraisals` (`appraisal_id`),
  CONSTRAINT `fk_section_scores_inspection` FOREIGN KEY (`inspection_id`) REFERENCES `inspections` (`inspection_id`),
  CONSTRAINT `fk_section_scores_section` FOREIGN KEY (`inspection_template_section_id`) REFERENCES `inspection_template_sections` (`inspection_template_section_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;