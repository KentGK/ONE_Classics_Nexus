USE one_classics_nexus;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS report_exports;
DROP TABLE IF EXISTS appraisal_reports;
DROP TABLE IF EXISTS valuation_trace_lines;
DROP TABLE IF EXISTS valuation_comparable_links;
DROP TABLE IF EXISTS valuation_runs;
DROP TABLE IF EXISTS comparable_adjustments;
DROP TABLE IF EXISTS comparable_sales;
DROP TABLE IF EXISTS inspection_media;
DROP TABLE IF EXISTS inspection_red_flags;
DROP TABLE IF EXISTS inspection_item_scores;
DROP TABLE IF EXISTS inspection_section_scores;
DROP TABLE IF EXISTS inspections;
DROP TABLE IF EXISTS inspection_template_items;
DROP TABLE IF EXISTS inspection_template_sections;
DROP TABLE IF EXISTS inspection_templates;
DROP TABLE IF EXISTS appraisal_assignments;
DROP TABLE IF EXISTS appraisals;
DROP TABLE IF EXISTS appraisal_requests;
DROP TABLE IF EXISTS client_contacts;
DROP TABLE IF EXISTS clients;
DROP TABLE IF EXISTS user_roles;
DROP TABLE IF EXISTS roles;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS vehicles;

SET FOREIGN_KEY_CHECKS = 1;
