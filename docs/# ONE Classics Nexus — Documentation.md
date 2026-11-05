# ONE Classics Nexus — Documentation

This directory contains all project documentation aligned to the Nexus SDLC.

## Structure
- 00_Project_Governance → PM control
- 01_Requirements → Source of truth
- 02_Architecture → System design
- 03_Design → Implementation design
- 04_Data → Database (LOCKED SOURCE)
- 05_API → API contracts
- 06_Engine → AVM core logic (CRITICAL)
- 07_UI_UX → Interface design
- 08_Testing_Quality → Validation
- 09_Deployment → Ops
- 10_Product_Investor → External materials

## Rules
1. No duplicate definitions across folders
2. AVM logic only lives in /06_Engine
3. DB schema only lives in /04_Data/Schema
4. All features must trace back to SRS