USE one_classics_nexus;

CREATE OR REPLACE VIEW vw_appraisal_latest_valuation AS
SELECT vr.*
FROM valuation_runs vr
JOIN (
    SELECT appraisal_id, MAX(created_at_utc) AS max_created_at_utc
    FROM valuation_runs
    GROUP BY appraisal_id
) latest
  ON latest.appraisal_id = vr.appraisal_id
 AND latest.max_created_at_utc = vr.created_at_utc;

CREATE OR REPLACE VIEW vw_inspection_section_rollup AS
SELECT
    i.appraisal_id,
    iss.inspection_id,
    iss.system_number,
    iss.system_key,
    iss.score_0_to_5,
    iss.normalized_score_0_to_100,
    iss.weight,
    iss.weighted_score,
    iss.red_flag_count,
    iss.updated_at_utc
FROM inspection_section_scores iss
JOIN inspections i
  ON i.inspection_id = iss.inspection_id;

CREATE OR REPLACE VIEW vw_valuation_trace_report AS
SELECT
    vr.appraisal_id,
    vtl.valuation_run_id,
    vtl.sort_order,
    vtl.trace_type,
    vtl.trace_key,
    vtl.amount,
    vtl.factor,
    vtl.message,
    vtl.evidence_ref,
    vtl.created_at_utc
FROM valuation_trace_lines vtl
JOIN valuation_runs vr
  ON vr.valuation_run_id = vtl.valuation_run_id;

-- Optional reporting helper query
-- Returns appraisals that are ready for valuation but have no valuation run yet
-- SELECT a.appraisal_id, a.appraisal_number, a.status, a.valuation_status
-- FROM appraisals a
-- LEFT JOIN valuation_runs vr ON vr.appraisal_id = a.appraisal_id
-- WHERE a.status IN ('ReadyForValuation', 'Valued')
--   AND vr.valuation_run_id IS NULL;
