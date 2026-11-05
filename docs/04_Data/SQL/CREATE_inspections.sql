-- one_classics_nexus_test.inspections definition

CREATE TABLE `inspections` (
  `inspection_id` char(36) NOT NULL,
  `appraisal_id` char(36) NOT NULL,
  `inspection_template_id` char(36) NOT NULL,
  `inspected_by_user_id` char(36) DEFAULT NULL,
  `inspection_type` varchar(30) DEFAULT NULL,
  `status` varchar(30) NOT NULL,
  `inspection_location` varchar(255) DEFAULT NULL,
  `inspected_on_utc` datetime(6) DEFAULT NULL,
  `summary_notes` text DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `completion_percentage` decimal(5,2) DEFAULT NULL,
  `overall_condition_score` decimal(5,2) DEFAULT NULL,
  `has_red_flags` tinyint(1) NOT NULL,
  `row_version` bigint(20) NOT NULL,
  PRIMARY KEY (`inspection_id`),
  KEY `idx_inspections_appraisal` (`appraisal_id`),
  KEY `idx_inspections_template` (`inspection_template_id`),
  KEY `idx_inspections_user` (`inspected_by_user_id`),
  KEY `idx_inspections_status` (`status`),
  CONSTRAINT `fk_inspections_appraisal` FOREIGN KEY (`appraisal_id`) REFERENCES `appraisals` (`appraisal_id`),
  CONSTRAINT `fk_inspections_template` FOREIGN KEY (`inspection_template_id`) REFERENCES `inspection_templates` (`inspection_template_id`),
  CONSTRAINT `fk_inspections_user` FOREIGN KEY (`inspected_by_user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;