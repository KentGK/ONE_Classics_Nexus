-- one_classics_nexus_test.vehicle_type_system_weights definition

CREATE TABLE `vehicle_type_system_weights` (
  `vehicle_type_system_weight_id` char(36) NOT NULL,
  `vehicle_type_id` char(36) NOT NULL,
  `system_number` int(11) NOT NULL,
  `system_code` varchar(30) NOT NULL,
  `system_key` varchar(50) NOT NULL,
  `system_name` varchar(100) NOT NULL,
  `weight` decimal(9,6) NOT NULL,
  `is_critical_system` tinyint(1) NOT NULL DEFAULT 0,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `effective_from_utc` datetime(6) DEFAULT NULL,
  `effective_to_utc` datetime(6) DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  PRIMARY KEY (`vehicle_type_system_weight_id`),
  UNIQUE KEY `uk_vehicle_type_system_weights_type_system` (`vehicle_type_id`,`system_number`),
  KEY `idx_vehicle_type_system_weights_vehicle_type` (`vehicle_type_id`),
  KEY `idx_vehicle_type_system_weights_system_key` (`system_key`),
  CONSTRAINT `fk_vehicle_type_system_weights_vehicle_type` FOREIGN KEY (`vehicle_type_id`) REFERENCES `vehicle_types` (`vehicle_type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;