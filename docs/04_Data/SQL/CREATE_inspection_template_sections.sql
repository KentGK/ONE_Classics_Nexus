-- one_classics_nexus_test.inspection_template_sections definition

create table `inspection_template_sections` (
  `inspection_template_section_id` char(36) not null,
`inspection_template_id` char(36) not null,
`system_number` int(11) not null,
`system_key` varchar(50) not null,
`section_name` varchar(150) not null,
`description` text default null,
`weight` decimal(9, 6) not null,
`sort_order` int(11) not null,
`created_at_utc` datetime(6) not null default current_timestamp(6),
`updated_at_utc` datetime not null,
`is_required` tinyint(1) not null,
primary key (`inspection_template_section_id`),
unique key `uq_template_sections_template_system` (`inspection_template_id`,
`system_number`),
key `idx_template_sections_template` (`inspection_template_id`),
key `idx_template_sections_system_key` (`system_key`),
constraint `fk_template_sections_template` foreign key (`inspection_template_id`) references `inspection_templates` (`inspection_template_id`)
) engine = InnoDB default CHARSET = utf8mb4 collate = utf8mb4_unicode_ci;
