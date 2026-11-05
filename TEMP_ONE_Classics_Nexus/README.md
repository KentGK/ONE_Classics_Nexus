# ONE Classics Nexus — Documentation Index
Version: 1.0  
Owner: PM / BA / TA  
Status: Active  

---

## 📌 Overview

This directory contains all documentation for the **ONE Classics Nexus** platform, structured according to the Software Development Life Cycle (SDLC).

---

## 🧭 Quick Navigation

- [00 Project Governance](#00-project-governance)
- [01 Requirements](#01-requirements)
- [02 Architecture](#02-architecture)
- [03 Design](#03-design)
- [04 Data (Database)](#04-data-database)
- [05 API](#05-api)
- [06 Engine (AVM Core)](#06-engine-avm-core)
- [07 UI/UX](#07-uiux)
- [08 Testing & Quality](#08-testing--quality)
- [09 Deployment & Operations](#09-deployment--operations)
- [10 Product & Investor](#10-product--investor)
- [11 Reference](#11-reference)
- [99 Archive](#99-archive)

---

# 00 Project Governance

- [SDLC Playbook](00_Project_Governance/SDLC_Playbook.md)
- [Project Roadmap](00_Project_Governance/Project_Roadmap.md)
- [Risk Register](00_Project_Governance/Risk_Register.md)
- [Decision Log](00_Project_Governance/Decision_Log.md)

### Phase Plans
- [Phase 1](00_Project_Governance/Phase_Plans/Phase_1.md)
- [Phase 2](00_Project_Governance/Phase_Plans/Phase_2.md)
- [Phase 3](00_Project_Governance/Phase_Plans/Phase_3.md)

### Action Items
- [Active](00_Project_Governance/Action_Items/Active.md)
- [Completed](00_Project_Governance/Action_Items/Completed.md)

---

# 01 Requirements

### SRS
- [SRS v2.0](01_Requirements/SRS/Nexus_SRS_v2.0.md)
- [SRS v3.0](01_Requirements/SRS/Nexus_SRS_v3.0.md)

### Traceability
- [Requirements Matrix](01_Requirements/Traceability/Requirements_Matrix.md)
- [API Traceability](01_Requirements/Traceability/API_Traceability.md)
- [DB Traceability](01_Requirements/Traceability/DB_Traceability.md)

### Glossary
- [Nexus Glossary v1.1](01_Requirements/Glossary/Nexus_Glossary_v1.1.md)

---

# 02 Architecture

- [System Architecture](02_Architecture/System_Architecture.md)
- [Logical Architecture](02_Architecture/Logical_Architecture.md)
- [Physical Architecture](02_Architecture/Physical_Architecture.md)

### Diagrams
- [High-Level Diagram](02_Architecture/Architecture_Diagrams/High_Level.mmd)
- [Data Flow](02_Architecture/Architecture_Diagrams/Data_Flow.mmd)
- [Deployment](02_Architecture/Architecture_Diagrams/Deployment.mmd)

### Solution Structure
- [Visual Studio Project Map](02_Architecture/Solution_Structure/VisualStudio_Project_Map.md)

### Non-Functional Requirements
- [Performance](02_Architecture/NFRs/Performance.md)
- [Scalability](02_Architecture/NFRs/Scalability.md)
- [Reliability](02_Architecture/NFRs/Reliability.md)

---

# 03 Design

### DDS
- [DDS v1.0](03_Design/DDS/Nexus_DDS_v1.0.md)
- [DDS v2.0](03_Design/DDS/Nexus_DDS_v2.0.md)

### Sequence Diagrams
- [Vehicle](03_Design/Sequence_Diagrams/Vehicle.mmd)
- [Appraisal](03_Design/Sequence_Diagrams/Appraisal.mmd)
- [Inspection](03_Design/Sequence_Diagrams/Inspection.mmd)
- [Valuation](03_Design/Sequence_Diagrams/Valuation.mmd)

### Component Design
- [Services](03_Design/Component_Design/Services.md)
- [Repositories](03_Design/Component_Design/Repositories.md)
- [Handlers](03_Design/Component_Design/Handlers.md)

---

# 04 Data (Database)

### Schema (🔒 Authoritative)
- [Full Schema](04_Data/Schema/001_Full_Schema.sql)
- [Seed Data](04_Data/Schema/002_Seed_Data.sql)

### Tables
- [Vehicles](04_Data/Tables/vehicles.md)
- [Appraisals](04_Data/Tables/appraisals.md)
- [Inspections](04_Data/Tables/inspections.md)
- [Condition Scores](04_Data/Tables/condition_scores.md)
- [Comparable Sales](04_Data/Tables/comparable_sales.md)

### Views
- [Latest Valuation View](04_Data/Views/vw_appraisal_latest_valuation.sql)

### ERD
- [ERD (Mermaid)](04_Data/ERD/Nexus_ERD.mmd)
- [ERD (Image)](04_Data/ERD/Nexus_ERD.png)

### Data Dictionary
- [Data Dictionary](04_Data/Data_Dictionary.md)

---

# 05 API

### OpenAPI
- [API Specification](05_API/OpenAPI/nexus_openapi.yaml)

### Endpoints
- [Vehicles](05_API/Endpoints/Vehicles.md)
- [Appraisals](05_API/Endpoints/Appraisals.md)
- [Inspections](05_API/Endpoints/Inspections.md)
- [Valuation](05_API/Endpoints/Valuation.md)

### API Governance
- [Error Handling](05_API/Error_Handling.md)
- [Versioning Strategy](05_API/Versioning_Strategy.md)

---

# 06 Engine (AVM Core)

### AVM Math (🔥 Core Contract)
- [AVM Spec v2.0](06_Engine/AVM_Math/AVM_Spec_v2.0.md)
- [AVM Spec v3.0](06_Engine/AVM_Math/AVM_Spec_v3.0.md)

### Engine Modules
- [Condition Engine](06_Engine/Condition_Engine/README.md)
- [Comparable Engine](06_Engine/Comparable_Engine/README.md)
- [Similarity Engine](06_Engine/Similarity_Engine/README.md)
- [Market Engine](06_Engine/Market_Engine/README.md)
- [Valuation Engine](06_Engine/Valuation_Engine/README.md)
- [Confidence Engine](06_Engine/Confidence_Engine/README.md)
- [Red Flag Engine](06_Engine/RedFlag_Engine/README.md)

### Traceability
- [Valuation Trace Model](06_Engine/Traceability_Model/ValuationTrace.md)

### Test Scenarios
- [Golden Test](06_Engine/Test_Scenarios/Golden_Test.md)
- [Edge Cases](06_Engine/Test_Scenarios/Edge_Cases.md)

---

# 07 UI/UX

### UX Specifications
- [Inspection UI](07_UI_UX/UX_Specs/Inspection_UI.md)

### Design System
- [Colors](07_UI_UX/Design_System/Colors.md)
- [Components](07_UI_UX/Design_System/Components.md)

---

# 08 Testing & Quality

- [Test Strategy](08_Testing_Quality/Test_Strategy.md)

### Test Data
- [Golden Dataset](08_Testing_Quality/Test_Data/Golden_Dataset.sql)

### Validation
- [AVM Validation](08_Testing_Quality/Validation/AVM_Validation.md)

---

# 09 Deployment & Operations

- [Environments](09_Deployment_Operations/Environments.md)
- [Deployment Guide](09_Deployment_Operations/Deployment_Guide.md)
- [Logging & Monitoring](09_Deployment_Operations/Logging_Monitoring.md)
- [Backup & Recovery](09_Deployment_Operations/Backup_Recovery.md)

---

# 10 Product & Investor

- [Product Definition](10_Product_Investor/Product_Definition.md)
- [Market Analysis](10_Product_Investor/Market_Analysis.md)
- [Competitive Analysis](10_Product_Investor/Competitive_Analysis.md)
- [Pricing Model](10_Product_Investor/Pricing_Model.md)

---

# 11 Reference

- [Standards](11_Reference/Standards/README.md)
- [Templates](11_Reference/Templates/README.md)

---

# 99 Archive

- Deprecated and legacy materials

---

## 🔑 Governance Rules

1. **Single Source of Truth**
   - Schema → `/04_Data/Schema`
   - AVM Logic → `/06_Engine`

2. **Traceability Required**