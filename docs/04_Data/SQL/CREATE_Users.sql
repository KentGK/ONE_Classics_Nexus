-- one_classics_nexus_test.users definition

create table `users` (
  `user_id` char(36) not null,
`email` varchar(255) not null,
`username` varchar(100) not null,
`display_name` varchar(150) not null,
`first_name` varchar(100) default null,
`last_name` varchar(100) default null,
`auth_provider` varchar(50) default null,
`external_subject` varchar(255) default null,
`is_active` tinyint(1) not null default 1,
`created_at_utc` datetime(6) not null default current_timestamp(6),
`updated_at_utc` datetime(6) not null default current_timestamp(6) on
update
    current_timestamp(6),
    primary key (`user_id`),
    unique key `uq_users_email` (`email`),
    unique key `ux_users_username` (`username`),
    key `idx_users_active` (`is_active`)
) engine = InnoDB default CHARSET = utf8mb4 collate = utf8mb4_unicode_ci;
