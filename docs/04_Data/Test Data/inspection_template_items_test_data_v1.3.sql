/*
  ONE Classics Nexus
  inspection_template_items_test_data_v1.3.sql

  Purpose:
    Seed test data for the `inspection_template_items` table aligned to
    `Nexus_Inspection_Data_Model_v1.3.md` (inspection data model v1.3) and the
    inspection_templates / inspection_template_sections seed guidance.

  Notes:
    - Target test schema: `one_classics_nexus_test`.
    - Uses CHAR(36) GUID identifiers consistent with application layer.
    - Depends on the `inspection_templates` and `inspection_template_sections` seed data being present.
    - Idempotent via ON DUPLICATE KEY UPDATE.
*/

USE one_classics_nexus_test;

INSERT INTO inspection_template_items (
    inspection_template_item_id,
    inspection_template_section_id,
    item_key,
    display_name,
    help_text,
    response_type,
    default_weight,
    allows_red_flag,
    is_required,
    sort_order,
    created_at_utc
)
VALUES
-- Template: Vintage/Classic (template id: b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594)
-- 10 items for each of the 12 systems (system 1..12)

-- System 1: body_exterior (section id b95932c7...591)
('00000000-0000-0000-0000-000000000001','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item01','Paint Finish','Assess paint consistency, finish, overspray, and visible repairs.','Score0To5',0.10,1,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000002','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item02','Panel Alignment','Check door, hood and trunk alignment and gaps.','Score0To5',0.10,1,1,2,NOW(6)),
('00000000-0000-0000-0000-000000000003','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item03','Corrosion','Inspect for visible corrosion, rust-through or treatment.','Score0To5',0.10,1,1,3,NOW(6)),
('00000000-0000-0000-0000-000000000004','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item04','Repaint Evidence','Identify overspray, mismatched panels or refinish evidence.','Boolean',NULL,1,0,4,NOW(6)),
('00000000-0000-0000-0000-000000000005','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item05','Dents & Dings','Note dents, dings and panel damage.','Score0To5',0.08,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000006','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item06','Trim Condition','Inspect external trim pieces for damage or replacements.','Score0To5',0.06,0,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000007','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item07','Glass Condition','Inspect windshield and windows for chips/cracks.','Score0To5',0.05,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000008','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item08','Seals & Weatherstrips','Check seals for aging and leaks.','Boolean',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000009','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item09','Chrome & Brightwork','Assess chrome, brightwork and plating condition.','Score0To5',0.05,0,0,9,NOW(6)),
('00000000-0000-0000-0000-00000000000A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a591','body_exterior_item10','Underbody Paint/Coating','Inspect underbody coating and protection.','Score0To5',0.06,0,0,10,NOW(6)),

-- System 2: frame_structure (section id b95932c7...592)
('00000000-0000-0000-0000-00000000000B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item01','Frame Straightness','Assess frame alignment and straightness.','Score0To5',0.12,1,1,1,NOW(6)),
('00000000-0000-0000-0000-00000000000C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item02','Rust Penetration','Inspect for structural rust and repairs.','Score0To5',0.10,1,1,2,NOW(6)),
('00000000-0000-0000-0000-00000000000D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item03','Weld Repairs','Check for evidence of frame welding or major repairs.','Boolean',NULL,1,0,3,NOW(6)),
('00000000-0000-0000-0000-00000000000E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item04','Subframe Condition','Inspect subframes and mounting points.','Score0To5',0.08,0,0,4,NOW(6)),
('00000000-0000-0000-0000-00000000000F','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item05','Floor Pan Condition','Inspect floor pans for rust or repairs.','Score0To5',0.06,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000010','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item06','Mounting Points','Check integrity of suspension and body mounts.','Score0To5',0.05,0,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000011','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item07','Rust Treatment History','Document evidence of rust treatment.','Text',NULL,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000012','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item08','Torsion & Twist','Assess for torsional damage.','Score0To5',0.07,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000013','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item09','Jack Points','Check condition of jack points and reinforcement.','Boolean',NULL,0,0,9,NOW(6)),
('00000000-0000-0000-0000-000000000014','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a592','frame_structure_item10','Alignment Correction History','Note any alignment corrections recorded.','Text',NULL,0,0,10,NOW(6)),

-- System 3: powertrain (section id b95932c7...593)
('00000000-0000-0000-0000-000000000015','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item01','Engine Start','Cold start behavior and smoke.','Score0To5',0.15,1,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000016','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item02','Oil Pressure','Check oil pressure history and leaks.','Numeric',NULL,0,0,2,NOW(6)),
('00000000-0000-0000-0000-000000000017','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item03','Compression Test','Record compression test results when available.','Numeric',NULL,0,0,3,NOW(6)),
('00000000-0000-0000-0000-000000000018','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item04','Oil Leaks','Inspect for oil leaks at seals and gaskets.','Score0To5',0.10,1,1,4,NOW(6)),
('00000000-0000-0000-0000-000000000019','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item05','Belts & Hoses','Condition of belts and hoses.','Score0To5',0.05,0,0,5,NOW(6)),
('00000000-0000-0000-0000-00000000001A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item06','Service History','Engine service history notes.','Text',NULL,0,0,6,NOW(6)),
('00000000-0000-0000-0000-00000000001B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item07','Fuel System','Fuel delivery and injection system condition.','Score0To5',0.08,0,0,7,NOW(6)),
('00000000-0000-0000-0000-00000000001C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item08','Exhaust Condition','Inspect exhaust for leaks and modifications.','Score0To5',0.06,0,0,8,NOW(6)),
('00000000-0000-0000-0000-00000000001D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item09','Cooling System','Radiator, hoses and coolant condition.','Score0To5',0.06,0,0,9,NOW(6)),
('00000000-0000-0000-0000-00000000001E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a593','powertrain_item10','Modification Notes','Document engine modifications and documented tuning.','Text',NULL,1,0,10,NOW(6)),

-- System 4: transmission_driveline (section id b95932c7...594)
('00000000-0000-0000-0000-00000000001F','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item01','Shift Quality','Assess gearshift smoothness and clunks.','Score0To5',0.12,1,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000020','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item02','Fluid Condition','Check fluid level and contamination.','Boolean',NULL,0,0,2,NOW(6)),
('00000000-0000-0000-0000-000000000021','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item03','Leaks','Inspect for transmission leaks.','Score0To5',0.08,0,0,3,NOW(6)),
('00000000-0000-0000-0000-000000000022','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item04','Clutch Condition','Clutch wear (manual) or torque converter issues.','Score0To5',0.10,0,1,4,NOW(6)),
('00000000-0000-0000-0000-000000000023','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item05','Driveline Vibrations','Note vibrations or driveline noise.','Score0To5',0.06,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000024','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item06','Differential','Inspect differential leaks and condition.','Score0To5',0.05,0,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000025','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item07','Axles & CV Joints','Check axles and CV joints for play.','Score0To5',0.05,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000026','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item08','Service Records','Transmission service and rebuild records.','Text',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000027','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item09','Gearbox Noise','Unusual noises from gearbox.','Score0To5',0.04,0,0,9,NOW(6)),
('00000000-0000-0000-0000-000000000028','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594','transmission_item10','Modification Impact','Document any driveline modifications.','Text',NULL,0,0,10,NOW(6)),

-- System 5: suspension_steering (section id b95932c7...595)
('00000000-0000-0000-0000-000000000029','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item01','Shock Absorbers','Inspect shocks/struts for leakage and wear.','Score0To5',0.10,0,1,1,NOW(6)),
('00000000-0000-0000-0000-00000000002A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item02','Bushings','Check suspension bushings for play.','Score0To5',0.06,0,0,2,NOW(6)),
('00000000-0000-0000-0000-00000000002B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item03','Steering Play','Assess steering play and alignment.','Score0To5',0.08,0,0,3,NOW(6)),
('00000000-0000-0000-0000-00000000002C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item04','Alignment','Document alignment and toe/camber issues.','Numeric',NULL,0,0,4,NOW(6)),
('00000000-0000-0000-0000-00000000002D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item05','Spring Condition','Inspect springs for sag or damage.','Score0To5',0.05,0,0,5,NOW(6)),
('00000000-0000-0000-0000-00000000002E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item06','Ride Height','Check ride height consistency.','Numeric',NULL,0,0,6,NOW(6)),
('00000000-0000-0000-0000-00000000002F','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item07','Control Arms','Check control arms for damage.','Score0To5',0.05,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000030','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item08','Steering Rack','Inspect steering rack and tie rods.','Score0To5',0.05,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000031','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item09','Adjustment History','Document suspension adjustments performed.','Text',NULL,0,0,9,NOW(6)),
('00000000-0000-0000-0000-000000000032','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a595','suspension_item10','Noise & Rattle','Note any suspension noises.','Score0To5',0.04,0,0,10,NOW(6)),

-- System 6: braking_system (section id b95932c7...596)
('00000000-0000-0000-0000-000000000033','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item01','Pad Thickness','Measure and record pad thickness.','Numeric',NULL,0,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000034','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item02','Rotor Condition','Inspect rotors for scoring and runout.','Score0To5',0.08,0,0,2,NOW(6)),
('00000000-0000-0000-0000-000000000035','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item03','Brake Fluid','Check fluid level and contamination.','Boolean',NULL,0,0,3,NOW(6)),
('00000000-0000-0000-0000-000000000036','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item04','ABS Function','Verify ABS system functionality indicator.','Boolean',NULL,0,0,4,NOW(6)),
('00000000-0000-0000-0000-000000000037','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item05','Handbrake Operation','Check handbrake engagement and travel.','Score0To5',0.03,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000038','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item06','Brake Lines','Inspect brake lines for corrosion/leaks.','Score0To5',0.04,0,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000039','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item07','Caliper Condition','Inspect calipers for sticking or leaks.','Score0To5',0.04,0,0,7,NOW(6)),
('00000000-0000-0000-0000-00000000003A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item08','Parking Brake','Parking brake hold test.','Boolean',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-00000000003B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item09','Brake Noise','Note squeal or grinding noises.','Score0To5',0.02,0,0,9,NOW(6)),
('00000000-0000-0000-0000-00000000003C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a596','braking_item10','Service Records','Brake service history.','Text',NULL,0,0,10,NOW(6)),

-- System 7: electrical_system (section id b95932c7...597)
('00000000-0000-0000-0000-00000000003D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item01','Battery Condition','Test battery and terminals.','Score0To5',0.06,0,1,1,NOW(6)),
('00000000-0000-0000-0000-00000000003E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item02','Charging System','Inspect alternator charging output.','Numeric',NULL,0,0,2,NOW(6)),
('00000000-0000-0000-0000-00000000003F','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item03','Lighting Function','Check all exterior and interior lights.','Boolean',NULL,0,0,3,NOW(6)),
('00000000-0000-0000-0000-000000000040','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item04','Wiring Condition','Inspect for frayed or modified wiring.','Score0To5',0.04,0,0,4,NOW(6)),
('00000000-0000-0000-0000-000000000041','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item05','Accessory Function','Test accessories (heater, A/C, stereo).','Score0To5',0.05,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000042','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item06','Instrument Cluster','Check gauges and indicator operation.','Boolean',NULL,0,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000043','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item07','Starter Operation','Check starter engagement and noise.','Score0To5',0.03,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000044','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item08','Fault Codes','Scan for stored fault codes.','Text',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000045','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item09','Ground Points','Check ground strap condition.','Score0To5',0.02,0,0,9,NOW(6)),
('00000000-0000-0000-0000-000000000046','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a597','electrical_item10','Modification Wiring','Document aftermarket wiring modifications.','Text',NULL,1,0,10,NOW(6)),

-- System 8: interior (section id b95932c7...598)
('00000000-0000-0000-0000-000000000047','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item01','Seat Upholstery','Inspect seats for tears and wear.','Score0To5',0.08,0,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000048','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item02','Carpet Condition','Assess carpet wear and stains.','Score0To5',0.05,0,0,2,NOW(6)),
('00000000-0000-0000-0000-000000000049','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item03','Headliner','Check headliner for sagging or damage.','Score0To5',0.04,0,0,3,NOW(6)),
('00000000-0000-0000-0000-00000000004A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item04','Dashboard','Inspect dashboard for cracks and condition.','Score0To5',0.04,0,0,4,NOW(6)),
('00000000-0000-0000-0000-00000000004B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item05','Controls & Switches','Test all switches and controls.','Boolean',NULL,0,0,5,NOW(6)),
('00000000-0000-0000-0000-00000000004C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item06','Odor & Mold','Note any odors or mold evidence.','Text',NULL,0,0,6,NOW(6)),
('00000000-0000-0000-0000-00000000004D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item07','Instrument Condition','Check condition of instruments and bezels.','Score0To5',0.03,0,0,7,NOW(6)),
('00000000-0000-0000-0000-00000000004E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item08','Trim Restoration','Note aftermarket restorations to interior trim.','Text',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-00000000004F','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item09','Seat Mechanisms','Test mechanical seat adjustments.','Boolean',NULL,0,0,9,NOW(6)),
('00000000-0000-0000-0000-000000000050','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a598','interior_item10','Safety Equipment','Check seat belts and airbags (if present).','Boolean',NULL,1,0,10,NOW(6)),

-- System 9: wheels_tires (section id b95932c7...599)
('00000000-0000-0000-0000-000000000051','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item01','Tire Tread','Measure tread depth across tires.','Numeric',NULL,0,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000052','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item02','Wheel Condition','Inspect wheels for damage or repairs.','Score0To5',0.05,0,0,2,NOW(6)),
('00000000-0000-0000-0000-000000000053','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item03','Alignment','Record alignment observations.','Numeric',NULL,0,0,3,NOW(6)),
('00000000-0000-0000-0000-000000000054','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item04','Spare Condition','Inspect spare tire condition.','Boolean',NULL,0,0,4,NOW(6)),
('00000000-0000-0000-0000-000000000055','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item05','Tire Age','Record tire manufacture date code.','Text',NULL,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000056','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item06','Runout','Check wheel runout.','Numeric',NULL,0,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000057','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item07','Wheel Fitment','Confirm fitment and offsets.','Score0To5',0.03,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000058','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item08','Tire Matching','Check matching tire sizes and types.','Boolean',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000059','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item09','Bolt Torque','Verify wheel bolt torque where applicable.','Boolean',NULL,0,0,9,NOW(6)),
('00000000-0000-0000-0000-00000000005A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a599','wheels_item10','Tire Wear Pattern','Note evens/uneven wear patterns.','Text',NULL,0,0,10,NOW(6)),

-- System 10: trim_glass_details (section id b95932c7...59a)
('00000000-0000-0000-0000-00000000005B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item01','Trim Fitment','Check fitment and completeness of trim.','Score0To5',0.03,0,1,1,NOW(6)),
('00000000-0000-0000-0000-00000000005C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item02','Glass Chips','Inspect for chips and cracks.','Score0To5',0.04,0,0,2,NOW(6)),
('00000000-0000-0000-0000-00000000005D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item03','Molding Condition','Assess rubber/plastic moldings.','Score0To5',0.02,0,0,3,NOW(6)),
('00000000-0000-0000-0000-00000000005E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item04','Glass Replacement History','Document any glass replacements.','Text',NULL,0,0,4,NOW(6)),
('00000000-0000-0000-0000-00000000005F','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item05','Seal Condition','Check window and door seals.','Score0To5',0.02,0,0,5,NOW(6)),
('00000000-0000-0000-0000-000000000060','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item06','Badge Authenticity','Verify badges and emblems authenticity.','Boolean',NULL,1,0,6,NOW(6)),
('00000000-0000-0000-0000-000000000061','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item07','Chrome Pitting','Inspect chrome components for pitting.','Score0To5',0.02,0,0,7,NOW(6)),
('00000000-0000-0000-0000-000000000062','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item08','Window Operation','Test window function and regulators.','Boolean',NULL,0,0,8,NOW(6)),
('00000000-0000-0000-0000-000000000063','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item09','Trim Fasteners','Check fastener condition and replacements.','Score0To5',0.01,0,0,9,NOW(6)),
('00000000-0000-0000-0000-000000000064','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59a','trim_item10','Glass Sealant','Note any sealant application or repairs.','Text',NULL,0,0,10,NOW(6)),

-- System 11: documentation_provenance (section id b95932c7...59b)
('00000000-0000-0000-0000-000000000065','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item01','Title Present','Title and registration present.','Boolean',NULL,1,1,1,NOW(6)),
('00000000-0000-0000-0000-000000000066','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item02','Service Records','Assess completeness of service history.','Score0To5',0.40,0,1,2,NOW(6)),
('00000000-0000-0000-0000-000000000067','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item03','Provenance Documents','Check bills of sale and ownership chain.','Score0To5',0.30,0,1,3,NOW(6)),
('00000000-0000-0000-0000-000000000068','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item04','Restoration Records','Document restoration invoices and photos.','Text',NULL,0,0,4,NOW(6)),
('00000000-0000-0000-0000-000000000069','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item05','Originality Notes','Assess originality of major components.','Score0To5',0.10,1,0,5,NOW(6)),
('00000000-0000-0000-0000-00000000006A','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item06','VIN Consistency','Verify VINs on chassis and plates.','Boolean',NULL,1,1,6,NOW(6)),
('00000000-0000-0000-0000-00000000006B','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item07','Title Branding','Check title branding history.','Text',NULL,0,0,7,NOW(6)),
('00000000-0000-0000-0000-00000000006C','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item08','Matching Numbers','Confirm engine and major component matching numbers.','Boolean',NULL,1,0,8,NOW(6)),
('00000000-0000-0000-0000-00000000006D','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item09','Certificate of Authenticity','Record any COA or expert reports.','Text',NULL,0,0,9,NOW(6)),
('00000000-0000-0000-0000-00000000006E','b95932c7-42c0-4c4b-a8c6-4a4c2fe5a59b','docs_item10','Provenance Score','Aggregate provenance confidence.','Score0To5',0.10,0,0,10,NOW(6))

ON DUPLICATE KEY UPDATE
  display_name = VALUES(display_name),
  help_text = VALUES(help_text),
  response_type = VALUES(response_type),
  default_weight = VALUES(default_weight),
  allows_red_flag = VALUES(allows_red_flag),
  is_required = VALUES(is_required),
  sort_order = VALUES(sort_order),
  created_at_utc = VALUES(created_at_utc);
