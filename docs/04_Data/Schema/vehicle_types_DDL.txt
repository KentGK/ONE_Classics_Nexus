-- one_classics_nexus_test.vehicle_types definition

CREATE TABLE `vehicle_types` (
  `vehicle_type_id` char(36) NOT NULL,
  `type_code` varchar(30) NOT NULL,
  `type_name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`vehicle_type_id`),
  UNIQUE KEY `uk_vehicle_types_code` (`type_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;