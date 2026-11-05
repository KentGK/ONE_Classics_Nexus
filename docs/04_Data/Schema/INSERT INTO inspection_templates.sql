USE one_classics_nexus;

-- Standard published 12-system template baseline
INSERT INTO inspection_templates (
    inspection_template_id, template_code, template_name, template_family, version_label, status,
    description, effective_from_utc, total_system_count, created_by_user_id
) VALUES (
    '10000000-0000-0000-0000-000000000001',
    'STANDARD',
    'ONE Classics Standard 12-System Inspection',
    'Standard',
    'v1.0',
    'Published',
    'Canonical baseline standard inspection template.',
    CURRENT_TIMESTAMP(6),
    12,
    NULL
)
ON DUPLICATE KEY UPDATE
    template_name = VALUES(template_name),
    template_family = VALUES(template_family),
    status = VALUES(status),
    description = VALUES(description),
    total_system_count = VALUES(total_system_count);

INSERT INTO inspection_template_sections (
    inspection_template_section_id, inspection_template_id, system_number, system_key, display_name, description, weight, sort_order
) VALUES
('11000000-0000-0000-0000-000000000001','10000000-0000-0000-0000-000000000001',1,'identity_profile','Identity / Vehicle Profile','Vehicle identity, VIN, trim, configuration, provenance baseline.',0.050000,1),
('11000000-0000-0000-0000-000000000002','10000000-0000-0000-0000-000000000001',2,'body_exterior','Body / Exterior','Body panels, fit, damage, corrosion and exterior condition.',0.100000,2),
('11000000-0000-0000-0000-000000000003','10000000-0000-0000-0000-000000000001',3,'paint_finish','Paint / Finish','Finish quality, consistency, prep quality and defects.',0.080000,3),
('11000000-0000-0000-0000-000000000004','10000000-0000-0000-0000-000000000001',4,'glass_trim','Glass / Trim / Brightwork','Glass integrity, trim condition and brightwork completeness.',0.070000,4),
('11000000-0000-0000-0000-000000000005','10000000-0000-0000-0000-000000000001',5,'interior','Interior','Seats, dash, upholstery, controls, fit and finish.',0.100000,5),
('11000000-0000-0000-0000-000000000006','10000000-0000-0000-0000-000000000001',6,'engine_powertrain','Engine Compartment / Powertrain','Engine presentation, leaks, performance and powertrain condition.',0.120000,6),
('11000000-0000-0000-0000-000000000007','10000000-0000-0000-0000-000000000001',7,'trans_driveline','Transmission / Driveline','Transmission operation, driveline integrity and related systems.',0.100000,7),
('11000000-0000-0000-0000-000000000008','10000000-0000-0000-0000-000000000001',8,'suspension_steering_brakes','Suspension / Steering / Brakes','Handling systems, braking confidence and serviceability.',0.100000,8),
('11000000-0000-0000-0000-000000000009','10000000-0000-0000-0000-000000000001',9,'chassis_undercarriage','Chassis / Undercarriage','Frame, underbody, structural condition, rust and repairs.',0.100000,9),
('11000000-0000-0000-0000-000000000010','10000000-0000-0000-0000-000000000001',10,'electrical','Electrical / Lighting / Instruments','Electrical system functionality, wiring condition and instrumentation.',0.070000,10),
('11000000-0000-0000-0000-000000000011','10000000-0000-0000-0000-000000000001',11,'wheels_tires','Wheels / Tires','Wheel correctness, tire age/condition and roadworthiness.',0.040000,11),
('11000000-0000-0000-0000-000000000012','10000000-0000-0000-0000-000000000001',12,'docs_authenticity','Documentation / Authenticity / Provenance','Paperwork, authenticity, matching numbers, and provenance strength.',0.060000,12)
ON DUPLICATE KEY UPDATE
    display_name = VALUES(display_name),
    description = VALUES(description),
    weight = VALUES(weight),
    sort_order = VALUES(sort_order);

-- Minimal baseline item set per section; extend via versioned clones only
INSERT INTO inspection_template_items (
    inspection_template_item_id, inspection_template_section_id, item_key, display_name, help_text,
    response_type, is_required, default_weight, allows_red_flag, sort_order
) VALUES
('12000000-0000-0000-0000-000000000001','11000000-0000-0000-0000-000000000001','vin_match','VIN / Data Plate Match','Verify VIN, trim tags, serials, and identity continuity.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000002','11000000-0000-0000-0000-000000000002','panel_fit','Panel Fit / Rust / Damage','Assess panel fit, accident signs, prior repairs and visible rust.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000003','11000000-0000-0000-0000-000000000003','paint_quality','Paint Quality','Assess quality, consistency, orange peel, scratches and defects.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000004','11000000-0000-0000-0000-000000000004','glass_trim_condition','Glass / Trim Condition','Assess cracks, delamination, chips, trim completeness and pitting.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000005','11000000-0000-0000-0000-000000000005','interior_condition','Interior Condition','Assess upholstery, dash, controls, carpets and presentation.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000006','11000000-0000-0000-0000-000000000006','engine_condition','Engine Condition','Assess operating condition, leaks, drivability and presentation.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000007','11000000-0000-0000-0000-000000000007','transmission_operation','Transmission / Driveline Operation','Assess shifting, driveline noise, leaks and operational quality.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000008','11000000-0000-0000-0000-000000000008','suspension_brakes','Suspension / Steering / Brakes','Assess suspension behavior, steering accuracy and brake performance.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000009','11000000-0000-0000-0000-000000000009','frame_underbody','Frame / Undercarriage','Assess frame integrity, underbody condition and corrosion severity.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000010','11000000-0000-0000-0000-000000000010','electrical_function','Electrical Functionality','Assess lights, gauges, switches, charging and wiring quality.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000011','11000000-0000-0000-0000-000000000011','wheel_tire_condition','Wheel / Tire Condition','Assess wheel correctness, tire condition and age code risk.','Score0To5',1,1.000000,1,1),
('12000000-0000-0000-0000-000000000012','11000000-0000-0000-0000-000000000012','docs_provenance','Documentation / Provenance','Assess records, authenticity evidence and provenance strength.','Score0To5',1,1.000000,1,1)
ON DUPLICATE KEY UPDATE
    display_name = VALUES(display_name),
    help_text = VALUES(help_text),
    response_type = VALUES(response_type),
    is_required = VALUES(is_required),
    default_weight = VALUES(default_weight),
    allows_red_flag = VALUES(allows_red_flag),
    sort_order = VALUES(sort_order);
