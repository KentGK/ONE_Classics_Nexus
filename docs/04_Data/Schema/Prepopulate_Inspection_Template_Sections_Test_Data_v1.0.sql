/*
  ONE Classics Nexus
  Prepopulate_Inspection_Template_Sections_Test_Data_v1.0.sql

  Purpose:
    Populate inspection_template_sections table with test data for the 12-system inspection model.
    This includes all 12 standard inspection systems with their weights and display information.

  Notes:
    - Designed for MariaDB/MySQL.
    - Uses CHAR(36) GUID identifiers.
    - Depends on inspection_templates table being populated first.
    - Uses standard 12-system inspection model with fixed weights.
    - System numbers are 1-12 and correspond to InspectionSystems constants.
*/

USE one_classics_nexus;

-- Insert inspection template sections for Standard template
INSERT INTO inspection_template_sections (
    inspection_template_section_id,
    inspection_template_id,
    system_number,
    system_key,
    display_name,
    description,
    weight,
    sort_order,
    created_at_utc
)
VALUES
-- System 1: Exterior Body Paint
('11111111-1111-1111-1111-111111111101',
 '00000000-0000-0000-0000-000000000001',
 1,
 'exterior_body_paint',
 'Exterior Body & Paint',
 'Inspection of vehicle exterior, body condition, paint quality, and cosmetic appearance',
 1.000000,
 1,
 NOW(6)),

-- System 2: Glass, Trim & Lighting
('11111111-1111-1111-1111-111111111102',
 '00000000-0000-0000-0000-000000000001',
 2,
 'glass_trim_lighting',
 'Glass, Trim & Lighting',
 'Inspection of windows, glass condition, trim pieces, and all exterior lighting systems',
 0.700000,
 2,
 NOW(6)),

-- System 3: Interior Cabin
('11111111-1111-1111-1111-111111111103',
 '00000000-0000-0000-0000-000000000001',
 3,
 'interior_cabin',
 'Interior Cabin',
 'Inspection of seats, carpeting, headliner, and interior condition',
 0.900000,
 3,
 NOW(6)),

-- System 4: Dashboard & Controls
('11111111-1111-1111-1111-111111111104',
 '00000000-0000-0000-0000-000000000001',
 4,
 'dashboard_controls',
 'Dashboard & Controls',
 'Inspection of dashboard, steering wheel, and all control functions',
 0.600000,
 4,
 NOW(6)),

-- System 5: Engine Compartment
('11111111-1111-1111-1111-111111111105',
 '00000000-0000-0000-0000-000000000001',
 5,
 'engine_compartment',
 'Engine Compartment',
 'Inspection of engine condition, hoses, belts, and engine bay cleanliness',
 1.000000,
 5,
 NOW(6)),

-- System 6: Fuel, Exhaust & Cooling
('11111111-1111-1111-1111-111111111106',
 '00000000-0000-0000-0000-000000000001',
 6,
 'fuel_exhaust_cooling',
 'Fuel, Exhaust & Cooling',
 'Inspection of fuel system, exhaust components, and cooling system',
 0.750000,
 6,
 NOW(6)),

-- System 7: Transmission & Driveline
('11111111-1111-1111-1111-111111111107',
 '00000000-0000-0000-0000-000000000001',
 7,
 'transmission_driveline',
 'Transmission & Driveline',
 'Inspection of transmission, transfer case, and driveline components',
 0.950000,
 7,
 NOW(6)),

-- System 8: Suspension, Steering & Brakes
('11111111-1111-1111-1111-111111111108',
 '00000000-0000-0000-0000-000000000001',
 8,
 'suspension_steering_brakes',
 'Suspension, Steering & Brakes',
 'Inspection of suspension components, steering system, and brake system',
 0.950000,
 8,
 NOW(6)),

-- System 9: Chassis & Undercarriage
('11111111-1111-1111-1111-111111111109',
 '00000000-0000-0000-0000-000000000001',
 9,
 'chassis_undercarriage',
 'Chassis & Undercarriage',
 'Inspection of frame, undercarriage, and structural integrity',
 0.900000,
 9,
 NOW(6)),

-- System 10: Wheels & Tires
('11111111-1111-1111-1111-111111111110',
 '00000000-0000-0000-0000-000000000001',
 10,
 'wheels_tires',
 'Wheels & Tires',
 'Inspection of wheels, tires, and tire condition',
 0.550000,
 10,
 NOW(6)),

-- System 11: Electrical & Accessories
('11111111-1111-1111-1111-111111111111',
 '00000000-0000-0000-0000-000000000001',
 11,
 'electrical_accessories',
 'Electrical & Accessories',
 'Inspection of electrical system, battery, and vehicle accessories',
 0.650000,
 11,
 NOW(6)),

-- System 12: Documentation & Authenticity
('11111111-1111-1111-1111-111111111112',
 '00000000-0000-0000-0000-000000000001',
 12,
 'documentation_authenticity',
 'Documentation & Authenticity',
 'Inspection of vehicle documentation, title, and authenticity verification',
 0.850000,
 12,
 NOW(6))
ON DUPLICATE KEY UPDATE
  system_number = VALUES(system_number),
  system_key = VALUES(system_key),
  display_name = VALUES(display_name),
  description = VALUES(description),
  weight = VALUES(weight),
  sort_order = VALUES(sort_order);
