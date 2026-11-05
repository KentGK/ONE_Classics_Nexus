# ONE Classics Nexus — Documentation Structure (v1.0)

## Overview
This document defines the standardized documentation structure for the ONE Classics Nexus platform.  
It aligns with the SDLC, enforces single sources of truth, and prevents drift across requirements, architecture, design, database, and implementation.

---

# 📁 Root
/docs

---

# 1️⃣ Governance & SDLC
/docs/00_governance

**Purpose:** Project control, standards, lifecycle

SDLC_Playbook.docx  
Documentation_Standards.md  
Naming_Conventions.md  
Versioning_Strategy.md  
Change_Control_Process.md  
Definition_of_Done.md  

**Owner:** Project Manager (PM) + Enterprise Architect (EA)

---

# 2️⃣ Requirements
/docs/01_requirements

/srs  
    ONE_Classics_Nexus_SRS_v1.0.docx  
    ONE_Classics_Nexus_SRS_v2.0.docx  
    Traceability_Matrix.xlsx  

/business_rules  
    Tier_Rules.md  
    Weight_Profiles.md  
    Red_Flag_Rules.md  

/definitions  
    Nexus_Definitions.docx  

**Owner:** Business Analyst (BA)  
**Source of Truth:** Business intent and requirements

---

# 3️⃣ Architecture
/docs/02_architecture

/sad  
    ONE_Classics_Nexus_SAD_v1.0.docx  
    ONE_Classics_Nexus_SAD_v2.0.docx  

/diagrams  
    architecture_diagram.png  
    deployment_diagram.png  
    component_diagram.png  

/decisions  
    ADR-001_Modular_Monolith.md  
    ADR-002_12_System_Model.md  
    ADR-003_Database_Strategy.md  

**Owner:** Enterprise Architect (EA)  

---

# 4️⃣ Detailed Design
/docs/03_design

/dds  
    ONE_Classics_Nexus_DDS_v1.0.docx  

/sds  
    ONE_Classics_Nexus_SDS_v2.0.docx  

/lld  
    Core_Principles_LLD.docx  

/sequence_diagrams  
    vehicle_sequence.md  
    inspection_sequence.md  
    valuation_sequence.md  

**Owner:** Technical Lead (TL)  

---

# 5️⃣ AVM & Math Specifications
/docs/04_avm

AVM_Math_Spec_v1.0.md  
AVM_Math_Spec_v2.0.md  
AVM_Math_Spec_v3.0.md  

/engines  
    Condition_Engine.md  
    Comparable_Engine.md  
    Similarity_Engine.md  
    Valuation_Engine.md  
    Confidence_Engine.md  

/test_scenarios  
    Golden_Test_K10.md  
    Edge_Cases.md  

**Owner:** TL + EA  

---

# 6️⃣ Database
/docs/05_database

/schema  
    ONE_Classics_Nexus_Full_Schema.sql  
    DROP_All.sql  

/erd  
    ERD.png  
    ERD_Source.drawio  

/migrations  
    v1_initial.sql  
    v2_weights.sql  

/mapping  
    Repository_Column_Mapping.md  

**Owner:** DBA  

---

# 7️⃣ API Specification
/docs/06_api

openapi.yaml  
openapi.json  

/endpoints  
    inspections.md  
    appraisals.md  
    valuation.md  
    reports.md  

/contracts  
    DTO_Definitions.md  

**Owner:** TL + EA  

---

# 8️⃣ UI / UX Design
/docs/07_ui

/wireframes  
    WinForms_MainShell.png  
    Inspection_Form.png  
    Dashboard.png  

/mockups  
    admin_ui.png  
    client_portal.png  

/ux_rules  
    UI_Standards.md  

---

# 9️⃣ Reporting
/docs/08_reporting

Report_Template.docx  
Report_Sections.md  
Export_Strategy.md  

---

# 🔟 Testing & QA
/docs/09_testing

Test_Strategy.docx  
Test_Cases.xlsx  

/integration  
    API_Test_Cases.md  

/golden_tests  
    Valuation_Golden_Test.md  

---

# 1️⃣1️⃣ Operations
/docs/10_operations

Deployment_Guide.md  
Environment_Config.md  
Database_Setup.md  
Backup_Strategy.md  

---

# 1️⃣2️⃣ Project Management
/docs/11_project_management

Roadmap.docx  
Sprint_Plans.xlsx  
Action_Items.xlsx  
Risk_Register.xlsx  

---

# 1️⃣3️⃣ Archive
/docs/archive

---

# 🔥 Critical Rules

## Single Source of Truth
- Requirements → /01_requirements/srs  
- Database → /05_database/schema  
- API → /06_api/openapi.yaml  

## Versioning
DocumentName_vMajor.Minor.ext  

## Ownership
- BA → Requirements  
- EA → Architecture  
- TL → Design/API  
- DBA → Database  
- QA → Testing  
- PM → Governance  

---

# 🚀 Next Step
Build the API ↔ DB ↔ Repository Mapping Matrix.
