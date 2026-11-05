# ONE Classics Nexus — System Glossary

**Version:** 1.1  
**Document Type:** Cross-Referenced System Glossary  
**Owner:** Business Analyst  
**Status:** Draft / Active  
**Solution:** ONE Classics Nexus  

---

## Table of Contents

1. [Core Platform Terms](#1-core-platform-terms)
2. [Vehicle Classification Terms](#2-vehicle-classification-terms)
3. [Canonical 12-System Architecture Terms](#3-canonical-12-system-architecture-terms)
4. [Inspection and Scoring Terms](#4-inspection-and-scoring-terms)
5. [AVM and Valuation Terms](#5-avm-and-valuation-terms)
6. [Collector Value Layer Terms](#6-collector-value-layer-terms)
7. [Comparable Sales and Similarity Terms](#7-comparable-sales-and-similarity-terms)
8. [Database and Data Model Terms](#8-database-and-data-model-terms)
9. [API and Integration Terms](#9-api-and-integration-terms)
10. [Engine Architecture Terms](#10-engine-architecture-terms)
11. [Testing and Quality Terms](#11-testing-and-quality-terms)
12. [UI/UX Terms](#12-uiux-terms)
13. [Deployment and Operations Terms](#13-deployment-and-operations-terms)
14. [Product and Investor Terms](#14-product-and-investor-terms)
15. [Reference and Standards Terms](#15-reference-and-standards-terms)
16. [Archive and Document Control Terms](#16-archive-and-document-control-terms)
17. [Governance Principles](#17-governance-principles)
18. [Canonical 12-System Reference Table](#18-canonical-12-system-reference-table)
19. [Cross-Reference Map](#19-cross-reference-map)

---

# 1. Core Platform Terms

## ONE Classics
The entity owner of the Nexus solution. ONE Classics owns the product vision, valuation standards, business rules, branding, and commercialization strategy for Nexus.

**Cross-References:**
- `/docs/10_Product_Investor/Product_Definition.md`
- `/docs/00_Project_Governance/Project_Roadmap.md`

## Nexus
The marketable software solution being developed by ONE Classics. Nexus supports vehicle inspection, condition scoring, comparable analysis, valuation, reporting, and future investor-ready productization.

**Cross-References:**
- `/docs/02_Architecture/System_Architecture.md`
- `/docs/01_Requirements/SRS/Nexus_SRS_v3.0.md`

## Nexus Project
The internal software development project responsible for designing, building, testing, documenting, and preparing the Nexus solution for field-ready and investor-ready use.

**Cross-References:**
- `/docs/00_Project_Governance/SDLC_Playbook.md`
- `/docs/00_Project_Governance/Phase_Plans/`

## Nexus Documentation Set
The complete controlled documentation library for Nexus, organized under `/docs`, including governance, requirements, architecture, design, data, API, engine, UI/UX, testing, deployment, product, reference, and archive materials.

**Cross-References:**
- `/docs/README.md`

---

# 2. Vehicle Classification Terms

## Vehicle Type
A first-class configurable classification that determines scoring weights, inspection wording, red flag behavior, collector value treatment, and valuation profile logic. Current supported vehicle types are Standard Vehicle, Custom Vehicle, and Classic Vehicle.

**Cross-References:**
- `/docs/01_Requirements/SRS/Nexus_SRS_v3.0.md`
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`

## Standard Vehicle
A production vehicle evaluated primarily for reliability, safety, operational condition, and practical market value. Standard Vehicle scoring emphasizes critical systems such as powertrain, transmission/driveline, braking, electrical, and road test functionality.

**Cross-References:**
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`
- `/docs/04_Data/Tables/condition_scores.md`

## Custom Vehicle
A modified or custom-built vehicle evaluated on condition, craftsmanship, build quality, design intent, mechanical execution, documentation, and market desirability. Originality is interpreted as build authenticity rather than factory correctness.

**Cross-References:**
- `/docs/07_UI_UX/UX_Specs/Inspection_UI.md`
- `/docs/06_Engine/Valuation_Engine/README.md`

## Classic Vehicle
A collector-oriented vehicle evaluated heavily on rarity, originality, provenance, completeness, historical significance, authenticity, and collector market demand.

**Cross-References:**
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`
- `/docs/10_Product_Investor/Market_Analysis.md`

---

# 3. Canonical 12-System Architecture Terms

## Canonical 12-System Model
The fixed inspection and scoring framework used across Standard, Custom, and Classic vehicles. The 12 systems must remain unchanged across vehicle types; vehicle-type differences are handled through weights, descriptions, inspection items, red flag rules, and valuation profiles.

**Cross-References:**
- `/docs/01_Requirements/SRS/Nexus_SRS_v3.0.md`
- `/docs/03_Design/DDS/Nexus_DDS_v2.0.md`
- `/docs/04_Data/Tables/condition_scores.md`

## System Number
The fixed numeric identifier from 1 through 12 assigned to each canonical inspection system. This number must remain stable across requirements, database records, APIs, valuation logic, reports, and tests.

**Cross-References:**
- `/docs/04_Data/Schema/001_Full_Schema.sql`
- `/docs/04_Data/Data_Dictionary.md`

## System Name
The business-facing name of a canonical vehicle system, such as Body & Exterior, Frame & Structure, or Engine / Powertrain.

**Cross-References:**
- `/docs/01_Requirements/Glossary/Nexus_Glossary_v1.1.md#18-canonical-12-system-reference-table`

## System Code
The uppercase short code used for documentation, reporting, rules, and readable technical references, such as `POWERTRAIN`, `BRAKE_SYS`, or `ROAD_FUNC`.

**Cross-References:**
- `/docs/06_Engine/Traceability_Model/ValuationTrace.md`

## System Key
The canonical snake_case key used by the database and engine logic, such as `powertrain`, `braking_system`, or `road_test_functional`.

**Cross-References:**
- `/docs/04_Data/Tables/condition_scores.md`
- `/docs/04_Data/Data_Dictionary.md`

## System Weight
A decimal weighting factor used by the Condition Engine to determine the contribution of each system to the Condition Index. System weights must sum to 1.00 for each vehicle-type profile.

**Cross-References:**
- `/docs/06_Engine/Condition_Engine/README.md`
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`

---

# 4. Inspection and Scoring Terms

## Inspection
The structured process of evaluating a vehicle across all 12 canonical systems and recording scores, notes, red flags, and supporting observations.

**Cross-References:**
- `/docs/07_UI_UX/UX_Specs/Inspection_UI.md`
- `/docs/04_Data/Tables/inspections.md`

## Inspection Type
The inspection configuration associated with a Vehicle Type. Inspection Type may influence scoring profiles, inspection item wording, required fields, and red flag definitions while preserving the canonical 12-system structure.

**Cross-References:**
- `/docs/01_Requirements/SRS/Nexus_SRS_v3.0.md`

## Inspection Item
A specific checklist item or field-level observation within one of the 12 systems. Inspection items support detailed scoring, field usability, notes, and future report generation.

**Cross-References:**
- `/docs/07_UI_UX/UX_Specs/Inspection_UI.md`

## Condition Score
A numeric score from 0.00 to 5.00 assigned to a canonical system. The score represents the observed condition of that system.

**Cross-References:**
- `/docs/04_Data/Tables/condition_scores.md`
- `/docs/06_Engine/Condition_Engine/README.md`

## Score 0–5 Scale
The system-level condition scoring scale used by Nexus:

| Score | Meaning |
|---:|---|
| 0 | Failed / Unsafe |
| 1 | Poor |
| 2 | Fair |
| 3 | Good |
| 4 | Very Good |
| 5 | Excellent |

**Cross-References:**
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`

## Red Flag
A critical defect, risk, or failure that materially affects vehicle safety, reliability, usability, or value. Red flags must not be hidden by averaging.

**Cross-References:**
- `/docs/06_Engine/RedFlag_Engine/README.md`
- `/docs/04_Data/Tables/condition_scores.md`

## Red Flag Adjustment
A deterministic valuation or condition penalty applied after base condition scoring when one or more red flags are present.

**Cross-References:**
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`
- `/docs/06_Engine/RedFlag_Engine/README.md`

## Inspection Notes
Free-text observations entered by an appraiser or inspector to explain condition, defects, exceptions, or supporting details for a system score.

**Cross-References:**
- `/docs/04_Data/Tables/condition_scores.md`
- `/docs/07_UI_UX/UX_Specs/Inspection_UI.md`

---

# 5. AVM and Valuation Terms

## AVM
Automated Valuation Model. The deterministic engine used by Nexus to compute vehicle value from inspection data, comparable sales, market factors, confidence inputs, red flags, and collector value modifiers.

**Cross-References:**
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`

## Condition Index
A normalized 0–100 condition score calculated from weighted 0–5 system scores. It is a core AVM input and must be deterministic.

**Cross-References:**
- `/docs/06_Engine/Condition_Engine/README.md`
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`

## Base Value
The starting valuation amount derived from comparable sales before applying condition, collector, market, red flag, or confidence adjustments.

**Cross-References:**
- `/docs/06_Engine/Comparable_Engine/README.md`
- `/docs/06_Engine/Valuation_Engine/README.md`

## Valuation Run
A single execution of the Nexus AVM pipeline for an appraisal. A Valuation Run produces valuation outputs and traceability records.

**Cross-References:**
- `/docs/05_API/Endpoints/Valuation.md`
- `/docs/06_Engine/Traceability_Model/ValuationTrace.md`

## Valuation Range
The calculated low, mid, and high value outputs produced by the AVM.

**Cross-References:**
- `/docs/05_API/Endpoints/Valuation.md`
- `/docs/08_Testing_Quality/Validation/AVM_Validation.md`

## Final Value
The final recommended valuation amount, typically represented by the mid value unless business rules or report formatting require otherwise.

**Cross-References:**
- `/docs/06_Engine/Valuation_Engine/README.md`

## Confidence Score
A score representing the reliability of the valuation result based on comparable count, comparable similarity, inspection completeness, data quality, and market signal strength.

**Cross-References:**
- `/docs/06_Engine/Confidence_Engine/README.md`
- `/docs/08_Testing_Quality/Validation/AVM_Validation.md`

## Valuation Status
The execution status of a Valuation Run. Recommended statuses include `Success`, `InsufficientData`, `NeedsReview`, `Error`, `Failed`, and `Pending`.

**Cross-References:**
- `/docs/05_API/Endpoints/Valuation.md`
- `/docs/06_Engine/Valuation_Engine/README.md`

---

# 6. Collector Value Layer Terms

## Collector Value Layer
A post-condition valuation layer that adjusts value based on collector-specific drivers. It does not alter the canonical 12-system mapping, system scores, or condition score schema.

**Cross-References:**
- `/docs/06_Engine/Valuation_Engine/README.md`
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`

## Rarity
A measure of scarcity, production volume, limited editions, unusual trims, unique builds, or historically uncommon configurations.

**Cross-References:**
- `/docs/10_Product_Investor/Market_Analysis.md`

## Originality
The degree to which a vehicle retains factory-correct components, configuration, and authenticity. For Custom Vehicles, originality is interpreted as build authenticity and consistency with design intent.

**Cross-References:**
- `/docs/06_Engine/Similarity_Engine/README.md`

## Provenance
The documented history of a vehicle, including ownership history, restoration records, builder reputation, show history, documentation, and historical significance.

**Cross-References:**
- `/docs/04_Data/Tables/appraisals.md`
- `/docs/06_Engine/Valuation_Engine/README.md`

## Market Desirability
The demand level for a vehicle, segment, configuration, style, or collector category.

**Cross-References:**
- `/docs/06_Engine/Market_Engine/README.md`

---

# 7. Comparable Sales and Similarity Terms

## Comparable Sale
A sale, auction result, listing, or market data point used to estimate the value of a subject vehicle.

**Cross-References:**
- `/docs/04_Data/Tables/comparable_sales.md`
- `/docs/06_Engine/Comparable_Engine/README.md`

## Subject Vehicle
The vehicle currently being inspected, appraised, or valued.

**Cross-References:**
- `/docs/04_Data/Tables/vehicles.md`
- `/docs/05_API/Endpoints/Vehicles.md`

## Similarity Score
A score indicating how closely a comparable sale matches the subject vehicle across attributes such as year, make, model, trim, drivetrain, engine, condition, originality, documentation, and market segment.

**Cross-References:**
- `/docs/06_Engine/Similarity_Engine/README.md`

## Comparable Weight
The relative influence assigned to a comparable sale after similarity, recency, quality, and relevance are evaluated.

**Cross-References:**
- `/docs/06_Engine/Comparable_Engine/README.md`

## Recency Factor
A factor that adjusts comparable influence based on sale date or market freshness.

**Cross-References:**
- `/docs/06_Engine/Market_Engine/README.md`

## Median Comparable Price
The median price of selected comparable sales, often used as a robust base-value input.

**Cross-References:**
- `/docs/06_Engine/Comparable_Engine/README.md`

---

# 8. Database and Data Model Terms

## Schema
The formal structure of database tables, columns, keys, constraints, indexes, and views.

**Cross-References:**
- `/docs/04_Data/Schema/001_Full_Schema.sql`

## Canonical Schema
The single source of truth for the Nexus database structure. The canonical schema must prevent parallel table variants and column drift.

**Cross-References:**
- `/docs/04_Data/Schema/001_Full_Schema.sql`
- `/docs/04_Data/Data_Dictionary.md`

## ERD
Entity Relationship Diagram. A diagram representing database tables and relationships.

**Cross-References:**
- `/docs/04_Data/ERD/Nexus_ERD.mmd`

## Data Dictionary
The controlled reference document describing database tables, columns, types, business meaning, and usage notes.

**Cross-References:**
- `/docs/04_Data/Data_Dictionary.md`

## `vehicles`
The database table storing subject vehicle identity and descriptive attributes.

**Cross-References:**
- `/docs/04_Data/Tables/vehicles.md`
- `/docs/05_API/Endpoints/Vehicles.md`

## `appraisals`
The database table storing appraisal records linked to vehicles and related inspection/valuation activity.

**Cross-References:**
- `/docs/04_Data/Tables/appraisals.md`
- `/docs/05_API/Endpoints/Appraisals.md`

## `inspections`
The database table storing inspection-level records for a vehicle appraisal.

**Cross-References:**
- `/docs/04_Data/Tables/inspections.md`
- `/docs/05_API/Endpoints/Inspections.md`

## `condition_scores`
The authoritative database table storing system-level condition scores. Required canonical fields include `system_number`, `system_key`, `score_0_to_5`, `is_red_flag`, and `notes`.

**Cross-References:**
- `/docs/04_Data/Tables/condition_scores.md`
- `/docs/06_Engine/Condition_Engine/README.md`

## `comparable_sales`
The database table storing comparable sales and listing data used by the Comparable and Similarity engines.

**Cross-References:**
- `/docs/04_Data/Tables/comparable_sales.md`
- `/docs/06_Engine/Comparable_Engine/README.md`

## View
A database object that exposes a query result as a reusable virtual table.

**Cross-References:**
- `/docs/04_Data/Views/vw_appraisal_latest_valuation.sql`

---

# 9. API and Integration Terms

## API
Application Programming Interface. The contract through which clients and UI layers interact with Nexus backend functionality.

**Cross-References:**
- `/docs/05_API/OpenAPI/nexus_openapi.yaml`

## Endpoint
A specific API route and method, such as `POST /api/appraisals/{appraisalId}/valuation/run`.

**Cross-References:**
- `/docs/05_API/Endpoints/`

## OpenAPI
A machine-readable API specification format used to define routes, requests, responses, schemas, and error handling.

**Cross-References:**
- `/docs/05_API/OpenAPI/nexus_openapi.yaml`

## Swagger
The tooling used to display, test, and validate OpenAPI specifications.

**Cross-References:**
- `/docs/05_API/OpenAPI/nexus_openapi.yaml`

## DTO
Data Transfer Object. A contract object used to pass data between API, application, UI, and integration boundaries.

**Cross-References:**
- `/docs/05_API/DTOs/README.md`

## Problem Details
A standardized API error response structure used to provide consistent 400/404/500 error responses.

**Cross-References:**
- `/docs/05_API/Error_Handling.md`

## API Versioning
The strategy for evolving the Nexus API without breaking existing clients.

**Cross-References:**
- `/docs/05_API/Versioning_Strategy.md`

---

# 10. Engine Architecture Terms

## Engine Layer
The system layer containing deterministic valuation, scoring, comparison, market, confidence, red flag, and traceability logic.

**Cross-References:**
- `/docs/06_Engine/`
- `/docs/03_Design/DDS/Nexus_DDS_v2.0.md`

## Condition Engine
Computes the Condition Index from canonical 12-system scores and vehicle-type-specific weights.

**Cross-References:**
- `/docs/06_Engine/Condition_Engine/README.md`

## Comparable Engine
Loads, filters, normalizes, and prepares comparable sales data for valuation.

**Cross-References:**
- `/docs/06_Engine/Comparable_Engine/README.md`

## Similarity Engine
Scores how closely each comparable sale matches the subject vehicle.

**Cross-References:**
- `/docs/06_Engine/Similarity_Engine/README.md`

## Market Engine
Applies market-related logic such as demand, momentum, liquidity, recency, and market sanity checks.

**Cross-References:**
- `/docs/06_Engine/Market_Engine/README.md`

## Valuation Engine
Combines condition, comparable, similarity, market, red flag, collector, and confidence inputs to produce valuation outputs.

**Cross-References:**
- `/docs/06_Engine/Valuation_Engine/README.md`

## Confidence Engine
Computes valuation confidence based on data quality, comparable support, inspection completeness, similarity strength, and market reliability.

**Cross-References:**
- `/docs/06_Engine/Confidence_Engine/README.md`

## RedFlag Engine
Evaluates red flag inputs and applies deterministic penalty or review logic.

**Cross-References:**
- `/docs/06_Engine/RedFlag_Engine/README.md`

## Trace Engine
Generates valuation trace outputs explaining how a valuation result was produced.

**Cross-References:**
- `/docs/06_Engine/Traceability_Model/ValuationTrace.md`

## Valuation Trace
A record-level explanation of valuation factors, adjustments, confidence inputs, red flags, and market logic.

**Cross-References:**
- `/docs/06_Engine/Traceability_Model/ValuationTrace.md`

---

# 11. Testing and Quality Terms

## Test Strategy
The formal testing approach for Nexus, including unit tests, integration tests, end-to-end tests, golden tests, validation, and defect management.

**Cross-References:**
- `/docs/08_Testing_Quality/Test_Strategy.md`

## Unit Test
A test that validates a small piece of logic in isolation, especially deterministic AVM math and engine components.

**Cross-References:**
- `/docs/08_Testing_Quality/Unit_Tests/README.md`

## Integration Test
A test that validates multiple layers together, such as API + repository + database behavior.

**Cross-References:**
- `/docs/08_Testing_Quality/Integration_Tests/README.md`

## Golden Test
A deterministic test scenario using fixed inputs and expected outputs to confirm that the AVM engine remains stable and auditable.

**Cross-References:**
- `/docs/06_Engine/Test_Scenarios/Golden_Test.md`
- `/docs/08_Testing_Quality/Test_Data/Golden_Dataset.sql`

## Edge Case
A boundary, unusual, or high-risk scenario used to test system behavior under non-standard inputs.

**Cross-References:**
- `/docs/06_Engine/Test_Scenarios/Edge_Cases.md`

## AVM Validation
The process of verifying that valuation outputs match defined mathematical rules, expected results, traceability standards, and confidence behavior.

**Cross-References:**
- `/docs/08_Testing_Quality/Validation/AVM_Validation.md`

## Defect
A confirmed gap, error, regression, or deviation from requirements, design, schema, or expected behavior.

**Cross-References:**
- `/docs/08_Testing_Quality/Defects/README.md`

---

# 12. UI/UX Terms

## UI
User Interface. The visual and interactive portion of Nexus used by appraisers, administrators, or clients.

**Cross-References:**
- `/docs/07_UI_UX/`

## UX
User Experience. The usability, flow, accessibility, field efficiency, and overall effectiveness of the Nexus interface.

**Cross-References:**
- `/docs/07_UI_UX/UX_Specs/Inspection_UI.md`

## WinForms UI
The Windows desktop interface for internal Nexus operations, including dashboard, vehicle, appraisal, inspection, comparables, valuation, reporting, and admin workflows.

**Cross-References:**
- `/docs/07_UI_UX/Wireframes/WinForms/README.md`

## Web UI
The future web-based interface for client inspection requests, appraisal requests, appraisal results retrieval, and administrative workflows.

**Cross-References:**
- `/docs/07_UI_UX/Wireframes/Web/README.md`

## Inspection UI
The field-ready interface used to capture 12-system inspection scores, notes, and red flags.

**Cross-References:**
- `/docs/07_UI_UX/UX_Specs/Inspection_UI.md`

## Dashboard
The main navigation and summary screen for Nexus users.

**Cross-References:**
- `/docs/07_UI_UX/Wireframes/WinForms/README.md`

## Design System
The controlled set of colors, layout standards, components, typography, and visual rules used by Nexus UI.

**Cross-References:**
- `/docs/07_UI_UX/Design_System/Colors.md`
- `/docs/07_UI_UX/Design_System/Components.md`

## User Flow
A mapped sequence of user actions required to complete a business task, such as creating a vehicle, capturing an inspection, or running a valuation.

**Cross-References:**
- `/docs/07_UI_UX/User_Flows/README.md`

---

# 13. Deployment and Operations Terms

## Deployment
The process of releasing Nexus software, database updates, configuration, and documentation to a target environment.

**Cross-References:**
- `/docs/09_Deployment_Operations/Deployment_Guide.md`

## Environment
A runtime context for Nexus, such as Development, Test, Staging, or Production.

**Cross-References:**
- `/docs/09_Deployment_Operations/Environments.md`

## Configuration
Environment-specific settings such as connection strings, ports, feature flags, and logging options.

**Cross-References:**
- `/docs/09_Deployment_Operations/Config/README.md`

## Logging
Recording application and system events for troubleshooting, monitoring, validation, and operations.

**Cross-References:**
- `/docs/09_Deployment_Operations/Logging_Monitoring.md`

## Monitoring
Observing system health, performance, failures, and operational events.

**Cross-References:**
- `/docs/09_Deployment_Operations/Logging_Monitoring.md`

## Backup and Recovery
The process and procedures for protecting and restoring Nexus data, schema, and critical configuration.

**Cross-References:**
- `/docs/09_Deployment_Operations/Backup_Recovery.md`

---

# 14. Product and Investor Terms

## Product Definition
The document describing what Nexus is, who it serves, its key capabilities, and its business purpose.

**Cross-References:**
- `/docs/10_Product_Investor/Product_Definition.md`

## Market Analysis
The analysis of target users, market demand, valuation segments, competitive context, and commercial opportunity.

**Cross-References:**
- `/docs/10_Product_Investor/Market_Analysis.md`

## Competitive Analysis
The comparison of Nexus against alternative tools, manual appraisal processes, valuation platforms, and market incumbents.

**Cross-References:**
- `/docs/10_Product_Investor/Competitive_Analysis.md`

## Pricing Model
The business model used to define product pricing, usage-based fees, subscription tiers, appraisal fees, or enterprise licensing.

**Cross-References:**
- `/docs/10_Product_Investor/Pricing_Model.md`

## Pitch Deck
Investor-facing or stakeholder-facing slide content summarizing the Nexus solution, market opportunity, product architecture, traction, and roadmap.

**Cross-References:**
- `/docs/10_Product_Investor/Pitch_Deck/README.md`

## Demo Scenario
A curated walkthrough used to demonstrate Nexus capabilities, such as vehicle creation, inspection capture, valuation run, and report generation.

**Cross-References:**
- `/docs/10_Product_Investor/Demo_Scenarios/README.md`

---

# 15. Reference and Standards Terms

## Standard
A documented convention, rule, industry reference, coding convention, schema convention, or process standard used by Nexus.

**Cross-References:**
- `/docs/11_Reference/Standards/README.md`

## External Research
Supporting research from industry, valuation, inspection, appraisal, collector vehicle, market, or technical sources.

**Cross-References:**
- `/docs/11_Reference/External_Research/README.md`

## Industry Source
A recognized data source, auction source, appraisal source, or market reference used to support Nexus valuation and product decisions.

**Cross-References:**
- `/docs/11_Reference/Industry_Sources/README.md`

## Template
A reusable document, form, checklist, specification, or report pattern used across the Nexus project.

**Cross-References:**
- `/docs/11_Reference/Templates/README.md`

---

# 16. Archive and Document Control Terms

## Archive
The controlled storage area for retired, superseded, or historical project documents that should be preserved but not used as current source of truth.

**Cross-References:**
- `/docs/99_Archive/`

## Deprecated Document
A document that has been replaced by a newer authoritative version.

**Cross-References:**
- `/docs/99_Archive/Deprecated_SRS/`

## Legacy Schema
An older database structure that has been retired and must not be used for current development.

**Cross-References:**
- `/docs/99_Archive/Old_Schema/`

## Retired Feature
A feature, design, table, endpoint, or workflow that has been removed from current scope or replaced by a newer design.

**Cross-References:**
- `/docs/99_Archive/Retired_Features/`

## Version
The revision number of a document, schema, API, or design artifact.

**Cross-References:**
- `/docs/00_Project_Governance/Decision_Log.md`

## Source of Truth
The authoritative location for a specific project artifact, such as requirements, schema, API contracts, AVM math, or testing rules.

**Cross-References:**
- `/docs/README.md`

---

# 17. Governance Principles

## Deterministic Valuation
The rule that the same inputs must always produce the same valuation outputs.

**Cross-References:**
- `/docs/06_Engine/AVM_Math/AVM_Spec_v3.0.md`
- `/docs/08_Testing_Quality/Test_Strategy.md`

## Explainable Output
The requirement that valuation results must include traceable explanations for scores, adjustments, confidence, and final outputs.

**Cross-References:**
- `/docs/06_Engine/Traceability_Model/ValuationTrace.md`

## Schema Authority Rule
The rule that database structure must be defined only in the canonical schema source and must not drift across scripts, repositories, or tests.

**Cross-References:**
- `/docs/04_Data/Schema/001_Full_Schema.sql`

## Engine Isolation Rule
The rule that all AVM, valuation, scoring, red flag, confidence, and trace logic belongs in the Engine documentation and corresponding engine implementation layer.

**Cross-References:**
- `/docs/06_Engine/`

## Traceability Chain
The required mapping from requirements through implementation and validation:

```text
SRS → DDS → DB → API → Engine → Tests
```

**Cross-References:**
- `/docs/01_Requirements/Traceability/Requirements_Matrix.md`
- `/docs/01_Requirements/Traceability/API_Traceability.md`
- `/docs/01_Requirements/Traceability/DB_Traceability.md`

---

# 18. Canonical 12-System Reference Table

| # | System Name | System Code | System Key | Description |
|---:|---|---|---|---|
| 1 | Body & Exterior | BODY_EXT | body_exterior | Exterior panels, paint, body condition, fit, corrosion, and cosmetic structural observations. |
| 2 | Frame & Structure | FRAME_STRUCT | frame_structure | Frame rails, unibody structure, chassis integrity, collision evidence, rust, and structural safety. |
| 3 | Engine / Powertrain | POWERTRAIN | powertrain | Engine condition, startup, leaks, compression indicators, drivability, and primary power delivery. |
| 4 | Transmission & Driveline | TRANS_DRIVE | transmission_driveline | Transmission, transfer case, driveshafts, differential, clutch, shifting, and driveline operation. |
| 5 | Suspension & Steering | SUS_STEER | suspension_steering | Suspension components, steering system, alignment behavior, handling, wear, and safety. |
| 6 | Braking System | BRAKE_SYS | braking_system | Brake operation, hydraulic system, pads/shoes, rotors/drums, parking brake, and stopping safety. |
| 7 | Electrical System | ELEC_SYS | electrical_system | Charging, starting, wiring, lighting, gauges, controls, electrical accessories, and reliability. |
| 8 | Interior | INTERIOR | interior | Seats, dash, trim, carpet, controls, comfort systems, interior condition, and usability. |
| 9 | Wheels & Tires | WHEEL_TIRE | wheels_tires | Tire condition, wheel condition, fitment, tread, date codes, safety, and roadworthiness. |
| 10 | Trim, Glass & Details | TRIM_GLASS | trim_glass_details | Exterior trim, glass, seals, mirrors, detail parts, chrome, badges, and finish quality. |
| 11 | Documentation & Provenance | DOC_PROV | documentation_provenance | Title, service records, ownership history, restoration records, authenticity support, and provenance. |
| 12 | Road Test & Functional | ROAD_FUNC | road_test_functional | Real-world operating behavior, road test results, functional checks, noises, handling, and usability. |

---

# 19. Cross-Reference Map

| Glossary Area | Primary Document Location | Related Locations |
|---|---|---|
| Governance | `/docs/00_Project_Governance/` | Roadmap, risks, decisions, phase plans |
| Requirements | `/docs/01_Requirements/SRS/` | Traceability, glossary, use cases |
| Architecture | `/docs/02_Architecture/` | Diagrams, solution structure, NFRs |
| Design | `/docs/03_Design/DDS/` | Sequence diagrams, components, patterns |
| Data | `/docs/04_Data/` | Schema, tables, ERD, data dictionary, views |
| API | `/docs/05_API/` | OpenAPI, endpoints, DTOs, error handling |
| Engine | `/docs/06_Engine/` | AVM math, red flags, confidence, traceability |
| UI/UX | `/docs/07_UI_UX/` | Wireframes, mockups, design system, user flows |
| Testing | `/docs/08_Testing_Quality/` | Unit, integration, validation, test data, defects |
| Deployment | `/docs/09_Deployment_Operations/` | Environments, config, logging, backup |
| Product / Investor | `/docs/10_Product_Investor/` | Product definition, market, competition, pricing, demos |
| Reference | `/docs/11_Reference/` | Standards, research, sources, templates |
| Archive | `/docs/99_Archive/` | Deprecated SRS, old schema, legacy designs, retired features |

---

## Document Control

| Field | Value |
|---|---|
| Document Name | Nexus_Glossary_v1.1.md |
| Version | 1.1 |
| Owner | Business Analyst |
| Status | Draft / Active |
| Primary Folder | `/docs/01_Requirements/Glossary/` |
| Related Docs | SRS, DDS, AVM Spec, Data Dictionary, OpenAPI, Test Strategy |

