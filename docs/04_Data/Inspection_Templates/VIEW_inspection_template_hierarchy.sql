-- ONE Classics Nexus
-- View: vw_inspection_template_hierarchy
-- Purpose: Flatten inspection templates -> sections -> items into a single read-only view
-- Compatible with MySQL / MariaDB

DROP VIEW IF EXISTS `vw_inspection_template_hierarchy`;
DELIMITER $$
CREATE VIEW `vw_inspection_template_hierarchy` AS
SELECT
  t.inspection_template_id,
  t.template_code,
  t.template_family,
  t.template_name,
  t.version_label,
  t.description        AS template_description,
  t.total_system_count,
  t.status             AS template_status,
  t.effective_from_utc,
  t.effective_to_utc,
  t.created_by_user_id,
  t.created_at_utc     AS template_created_at_utc,
  t.updated_at_utc     AS template_updated_at_utc,

  s.inspection_template_section_id,
  s.system_number,
  s.system_key,
  s.display_name       AS section_display_name,
  s.description        AS section_description,
  s.weight             AS section_weight,
  s.sort_order         AS section_sort_order,
  s.created_at_utc     AS section_created_at_utc,

  i.inspection_template_item_id,
  i.item_key,
  i.display_name       AS item_display_name,
  i.help_text          AS item_help_text,
  i.response_type      AS item_response_type,
  i.default_weight     AS item_default_weight,
  i.allows_red_flag    AS item_allows_red_flag,
  i.is_required        AS item_is_required,
  i.sort_order         AS item_sort_order,
  i.created_at_utc     AS item_created_at_utc
FROM inspection_templates AS t
  JOIN inspection_template_sections AS s
    ON s.inspection_template_id = t.inspection_template_id
  LEFT JOIN inspection_template_items AS i
    ON i.inspection_template_section_id = s.inspection_template_section_id
;$$
DELIMITER ;

-- Notes:
-- - This view returns one row per item. If a section has no items it will still appear with NULL item columns because of LEFT JOIN.
-- - Use the view for read-only reporting and UI population. Do not rely on it for write operations.
-- - Consider adding indexes on underlying tables for performant queries against this view when used in production.
