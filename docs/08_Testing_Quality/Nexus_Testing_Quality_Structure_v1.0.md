# ONE Classics Nexus
## 08_Testing_Quality Structure Recommendation

**Document Version:** 1.0  
**Prepared For:** ONE Classics Nexus Project Team  
**System:** ONE_Classics_Nexus  
**Repository Path:** `/docs/08_Testing_Quality/`  
**Document Type:** Testing and Quality Folder Structure Recommendation  
**Status:** Draft for Project Adoption  

---

## 1. Purpose

This document defines the recommended structure for the Nexus `/docs/08_Testing_Quality/` documentation area.

The purpose of this folder is to provide a controlled location for:

- Test strategy
- Test planning
- Test cases
- Automated testing guidance
- Test data
- Quality gates
- Defect and risk tracking
- Test execution reports

The folder should serve as the official QA control point for SDLC signoff, phase readiness, release readiness, and regression evidence.

---

## 2. Recommended Folder Structure

```text
/docs/08_Testing_Quality/
│
├── 01_Test_Strategy/
│   ├── Nexus_Test_Strategy_v1.0.md
│   ├── Nexus_Quality_Assurance_Approach_v1.0.md
│   └── Nexus_Test_Environment_Strategy_v1.0.md
│
├── 02_Test_Plans/
│   ├── Nexus_Phase_1_Test_Plan_v1.0.md
│   ├── Nexus_Phase_2_Test_Plan_v1.0.md
│   ├── Nexus_API_Test_Plan_v1.0.md
│   ├── Nexus_WinForms_Test_Plan_v1.0.md
│   └── Nexus_Database_Test_Plan_v1.0.md
│
├── 03_Test_Cases/
│   ├── Vehicle_Test_Cases_v1.0.md
│   ├── Appraisal_Test_Cases_v1.0.md
│   ├── Inspection_Test_Cases_v1.0.md
│   ├── Comparable_Sales_Test_Cases_v1.0.md
│   ├── Valuation_Run_Test_Cases_v1.0.md
│   └── WinForms_UI_Test_Cases_v1.0.md
│
├── 04_Automated_Testing/
│   ├── Unit_Test_Strategy_v1.0.md
│   ├── Integration_Test_Strategy_v1.0.md
│   ├── API_Test_Automation_Guide_v1.0.md
│   ├── Database_Test_Automation_Guide_v1.0.md
│   └── Golden_Dataset_Testing_Guide_v1.0.md
│
├── 05_Test_Data/
│   ├── Swagger_Test_Data_v1.0.md
│   ├── Vehicle_Test_Data_v1.0.md
│   ├── Appraisal_Test_Data_v1.0.md
│   ├── Inspection_Test_Data_v1.0.md
│   ├── Comparable_Sales_Test_Data_v1.0.md
│   └── Golden_Valuation_Dataset_v1.0.md
│
├── 06_Quality_Gates/
│   ├── Nexus_Definition_of_Done_v1.0.md
│   ├── Nexus_Quality_Gates_v1.0.md
│   ├── Phase_Readiness_Checklist_v1.0.md
│   ├── Release_Readiness_Checklist_v1.0.md
│   └── Regression_Test_Checklist_v1.0.md
│
├── 07_Defects_Risks/
│   ├── Defect_Management_Process_v1.0.md
│   ├── Known_Issues_Log_v1.0.md
│   ├── Testing_Risk_Register_v1.0.md
│   └── Production_Defect_Triage_Guide_v1.0.md
│
└── 08_Test_Reports/
    ├── Phase_1_Test_Results_v1.0.md
    ├── Phase_2_Test_Results_v1.0.md
    ├── API_Test_Execution_Report_v1.0.md
    ├── Integration_Test_Execution_Report_v1.0.md
    └── Regression_Test_Execution_Report_v1.0.md
```

---

## 3. Recommended Priority Order

The following documents should be created first because they support immediate engineering execution, API validation, database verification, and phase-level QA control.

```text
/docs/08_Testing_Quality/01_Test_Strategy/Nexus_Test_Strategy_v1.0.md
/docs/08_Testing_Quality/06_Quality_Gates/Nexus_Definition_of_Done_v1.0.md
/docs/08_Testing_Quality/02_Test_Plans/Nexus_API_Test_Plan_v1.0.md
/docs/08_Testing_Quality/03_Test_Cases/Vehicle_Test_Cases_v1.0.md
/docs/08_Testing_Quality/03_Test_Cases/Appraisal_Test_Cases_v1.0.md
/docs/08_Testing_Quality/03_Test_Cases/Inspection_Test_Cases_v1.0.md
/docs/08_Testing_Quality/04_Automated_Testing/Integration_Test_Strategy_v1.0.md
/docs/08_Testing_Quality/05_Test_Data/Swagger_Test_Data_v1.0.md
```

### 3.1 Priority Rationale

| Priority | Document | Rationale |
|---:|---|---|
| 1 | `Nexus_Test_Strategy_v1.0.md` | Establishes the overall QA model, test levels, environments, roles, and acceptance expectations. |
| 2 | `Nexus_Definition_of_Done_v1.0.md` | Defines the minimum quality standard before a feature, slice, or phase is considered complete. |
| 3 | `Nexus_API_Test_Plan_v1.0.md` | Supports immediate Swagger/API validation for Vehicles, Appraisals, Inspections, Comparables, and Valuation Runs. |
| 4 | `Vehicle_Test_Cases_v1.0.md` | Validates the first foundational entity used by downstream appraisal workflows. |
| 5 | `Appraisal_Test_Cases_v1.0.md` | Validates the appraisal container that links Vehicles, Inspections, Comparables, Valuation Runs, and Reports. |
| 6 | `Inspection_Test_Cases_v1.0.md` | Validates the inspection-first data model and 12-system condition scoring workflow. |
| 7 | `Integration_Test_Strategy_v1.0.md` | Defines API, repository, MariaDB, and service-layer integration testing expectations. |
| 8 | `Swagger_Test_Data_v1.0.md` | Provides repeatable test payloads for manual and automated API contract validation. |

---

## 4. Nexus-Specific Alignment

The `/docs/08_Testing_Quality/` folder should validate the locked Nexus delivery areas below.

```text
Vehicles CRUD
Appraisals CRUD
Inspections and 12-system condition scoring
Comparable sales
Valuation runs
Valuation trace lines
MariaDB schema integrity
API contract behavior
WinForms API integration
Golden valuation dataset
Regression testing before phase promotion
```

---

## 5. Alignment to Nexus Architecture

The testing and quality documentation should align to the Nexus architecture layers:

| Nexus Layer | Testing Focus |
|---|---|
| Domain | Entity rules, value object validation, scoring rules, valuation math invariants |
| Application | Use cases, service behavior, validation rules, orchestration logic |
| Infrastructure | Dapper repositories, MariaDB schema alignment, query behavior, transaction handling |
| API | REST endpoints, DTO contracts, status codes, ProblemDetails responses, Swagger validation |
| WinForms | API integration, usability flow, form validation, appraiser field workflow |
| Engine | Condition Engine, Comparable Engine, Similarity Engine, Valuation Engine, Confidence Engine, Trace Engine |
| Tests | Unit, integration, golden run, regression, smoke, and release-readiness testing |

---

## 6. Alignment to Nexus Phase Delivery

| Phase | Testing and Quality Emphasis |
|---|---|
| Phase 1 | Vehicles, Appraisals, Inspections, WinForms shell, basic scoring, API CRUD, repository validation |
| Phase 2 | Comparable Sales, Valuation Runs, Trace Lines, Confidence Scoring, valuation engine determinism |
| Phase 3 | Full CRUD coverage, reporting lifecycle, admin workflows, client portal readiness, release regression |

---

## 7. Quality Gate Coverage

The folder should support the following quality gates:

| Quality Gate | Required Evidence |
|---|---|
| Feature Done | Unit tests, API tests, repository tests, acceptance checklist |
| API Ready | Swagger clean, endpoint tests, DTO validation, error response validation |
| Database Ready | Schema/repository alignment, FK validation, migration verification, seed data checks |
| Engine Ready | Math tests, deterministic golden runs, trace-line validation |
| UI Ready | Field workflow validation, input validation, API integration testing |
| Phase Ready | Regression checklist, test execution report, unresolved defect review |
| Release Ready | Smoke test, integration test results, known issues log, release readiness checklist |

---

## 8. Minimum Testing Coverage Expectations

| Area | Minimum Coverage |
|---|---|
| Vehicles | POST, GET, PUT, validation, duplicate VIN, not-found behavior |
| Appraisals | Create by vehicle, retrieve, invalid vehicle, appraisal workflow linkage |
| Inspections | Create, save 12-system scores, retrieve scores, validate score range 0–5 |
| Condition Scores | Validate `system_number`, `system_key`, `score_0_to_5`, weight totals, red flag persistence |
| Comparable Sales | POST, GET, schema alignment, sale price, source, sale date, condition fields |
| Valuation Runs | Run valuation, retrieve valuation history, persist final value, value range, confidence, trace lines |
| Trace Lines | Validate sort order, trace type, trace key, amount/factor/message capture |
| Database | Table columns, indexes, foreign keys, repository SQL alignment |
| WinForms | Form navigation, API service calls, validation messages, save/retrieve flows |
| Regression | Re-run core CRUD, inspection scoring, valuation run, and report smoke tests before phase promotion |

---

## 9. Recommended Naming Standard

All test and quality documents should follow this naming pattern:

```text
Nexus_[Subject]_[DocumentType]_v[major].[minor].md
```

Examples:

```text
Nexus_API_Test_Plan_v1.0.md
Nexus_Quality_Gates_v1.0.md
Nexus_Definition_of_Done_v1.0.md
```

Entity-specific documents may use this pattern:

```text
[Entity_or_Module]_Test_Cases_v[major].[minor].md
```

Examples:

```text
Vehicle_Test_Cases_v1.0.md
Inspection_Test_Cases_v1.0.md
Valuation_Run_Test_Cases_v1.0.md
```

---

## 10. Recommended Next Documents

After this structure is adopted, the next recommended documents to generate are:

1. `Nexus_Test_Strategy_v1.0.md`
2. `Nexus_Definition_of_Done_v1.0.md`
3. `Nexus_API_Test_Plan_v1.0.md`
4. `Vehicle_Test_Cases_v1.0.md`
5. `Inspection_Test_Cases_v1.0.md`
6. `Swagger_Test_Data_v1.0.md`

---

## 11. Summary

The `/docs/08_Testing_Quality/` folder should become the official Nexus QA control area.

It should ensure that Nexus remains:

- Inspection-driven
- API-centered
- Database-aligned
- Engine-tested
- Deterministic
- Traceable
- Regression-controlled
- Release-ready
