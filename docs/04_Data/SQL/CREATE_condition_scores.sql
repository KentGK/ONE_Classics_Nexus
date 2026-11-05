-- one_classics_nexus_test.condition_scores definition

CREATE TABLE `condition_scores` (
  `condition_score_id` char(36) NOT NULL,
  `inspection_id` char(36) NOT NULL,
  `system_number` int(11) NOT NULL,
  `system_key` varchar(50) NOT NULL,
  `score_0_to_5` decimal(3,2) NOT NULL,
  `weight` decimal(6,4) NOT NULL,
  `is_red_flag` tinyint(1) NOT NULL DEFAULT 0,
  `notes` text DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`condition_score_id`),
  UNIQUE KEY `uk_inspection_system` (`inspection_id`,`system_number`),
  KEY `idx_inspection` (`inspection_id`),
  CONSTRAINT `fk_condition_scores_inspection` FOREIGN KEY (`inspection_id`) REFERENCES `inspections` (`inspection_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;