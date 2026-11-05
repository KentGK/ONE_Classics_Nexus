USE one_classics_nexus;

INSERT INTO roles (role_id, role_name, role_code, description, is_system_role)
VALUES
('00000000-0000-0000-0000-000000000001', 'Administrator', 'ADMIN', 'Full platform administration', 1),
('00000000-0000-0000-0000-000000000002', 'Appraiser', 'APPRAISER', 'Inspection and appraisal execution', 1),
('00000000-0000-0000-0000-000000000003', 'Reviewer', 'REVIEWER', 'Quality and review authority', 1),
('00000000-0000-0000-0000-000000000004', 'Client', 'CLIENT', 'Client portal access', 1)
ON DUPLICATE KEY UPDATE
  role_name = VALUES(role_name),
  description = VALUES(description),
  is_system_role = VALUES(is_system_role);
