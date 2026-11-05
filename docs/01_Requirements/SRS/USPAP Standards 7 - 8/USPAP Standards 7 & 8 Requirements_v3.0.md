# 3.x USPAP Standards 7 & 8 Requirements

## 3.x.1 Purpose

This section defines the software requirements necessary for ONE Classics Nexus to support personal property appraisal development and reporting in accordance with:

- **USPAP Standard 7 — Personal Property Appraisal, Development**
- **USPAP Standard 8 — Personal Property Appraisal, Reporting**

This section establishes traceability between:

```text
USPAP Standard / Rule / Requirement Area
        ↓
SRS Requirement
        ↓
FR-USPAP Functional Requirement
        ↓
Domain / Data / API / UI / Validation / Test
```

Standard 7 governs the development of the appraisal.

Standard 8 governs the communication of the appraisal.

The system shall preserve a clear separation between appraisal development and appraisal reporting while maintaining traceability between the two.

---

## 3.x.2 Governing Appraisal Architecture

The Appraisal shall be the central business entity for all USPAP-related workflow and data.

The logical appraisal lifecycle shall be:

```text
Assignment
    ↓
Problem Identification
    ↓
Scope of Work
    ↓
Evidence Collection
    ↓
Subject Analysis
    ↓
Market Analysis
    ↓
Valuation Analysis
    ↓
Reconciliation
    ↓
Final Opinion of Value
    ↓
Report Preparation
    ↓
Certification
    ↓
Report Issuance
    ↓
Delivery
    ↓
Appraisal Record Retention
```

The system shall maintain traceability between:

```text
What was identified
    ↓
What was relied upon
    ↓
What was analyzed
    ↓
What was calculated
    ↓
What was reconciled
    ↓
What the appraiser concluded
    ↓
What the appraiser reported
    ↓
What the appraiser certified
```

Report generation shall not substitute for appraisal development.

**Cross-reference:** `FR-USPAP-087` through `FR-USPAP-095`

---

# 3.x.3 Standard 7 — Appraisal Problem Identification

## 3.x.3.1 Client Identification

**SRS Requirement:** `S7-REQ-001`  
**Functional Requirement:** `FR-USPAP-001`, `FR-USPAP-002`

The system shall require each appraisal assignment to identify the responsible client.

Client data shall support, as applicable:

- client identifier;
- client name;
- organization;
- client type;
- contact information;
- engagement reference;
- confidentiality restrictions; and
- relationship to the subject property.

The client shall be distinguishable from:

- intended users;
- subject-property owner;
- seller;
- buyer;
- report recipients; and
- other assignment participants.

### Validation

An appraisal shall not pass the Assignment Ready gate without an identified client.

---

## 3.x.3.2 Intended Users

**SRS Requirement:** `S7-REQ-002`  
**Functional Requirement:** `FR-USPAP-004`

The system shall permit one or more intended users to be associated with the appraisal.

Each intended user shall support:

- intended-user identifier;
- name;
- organization;
- intended-user type;
- relationship to client; and
- applicable notes.

The system shall not automatically designate a report recipient as an intended user.

---

## 3.x.3.3 Intended Use

**SRS Requirement:** `S7-REQ-003`  
**Functional Requirement:** `FR-USPAP-003`

Each appraisal shall have an established intended use.

The system shall support controlled intended-use classifications and supplemental narrative.

Potential classifications include:

- insurance;
- estate planning;
- estate settlement;
- equitable distribution;
- financing;
- litigation;
- acquisition;
- disposition;
- charitable contribution;
- tax-related purposes;
- collection management; and
- advisory purposes.

### Validation

A final opinion of value shall not be approved without an established intended use.

---

## 3.x.3.4 Subject Property Identification

**SRS Requirement:** `S7-REQ-004`  
**Functional Requirement:** `FR-USPAP-008`

The system shall uniquely identify the personal property being appraised.

For collector vehicles, supported identification shall include where applicable:

- vehicle ID;
- VIN;
- chassis number;
- body number;
- engine number;
- year;
- make;
- marque;
- model;
- series;
- trim;
- body style;
- engine;
- transmission;
- drivetrain;
- mileage;
- odometer unit;
- exterior color;
- interior color; and
- title status.

---

## 3.x.3.5 Relevant Property Characteristics

**SRS Requirement:** `S7-REQ-005`  
**Functional Requirement:** `FR-USPAP-009`, `FR-USPAP-010`

The system shall support value-relevant subject characteristics including:

- condition;
- originality;
- authenticity;
- restoration history;
- restoration quality;
- matching-numbers status;
- factory configuration;
- equipment;
- modifications;
- documentation;
- provenance;
- ownership history;
- competition history;
- rarity;
- title status;
- accident history;
- structural damage;
- mechanical condition; and
- collector desirability.

The system shall preserve a historical subject snapshot for completed and issued appraisals.

---

## 3.x.3.6 Property Interest

**SRS Requirement:** `S7-REQ-006`  
**Functional Requirement:** `FR-USPAP-005`

The property interest being appraised shall be identified separately from the physical-property record.

---

## 3.x.3.7 Type and Definition of Value

**SRS Requirement:** `S7-REQ-007`  
**Functional Requirement:** `FR-USPAP-006`

The system shall require identification of:

- value type;
- definition;
- definition source;
- jurisdiction where applicable;
- citation/reference; and
- supplemental narrative.

A final appraisal conclusion shall not be approved without a defined type of value.

---

## 3.x.3.8 Effective Date

**SRS Requirement:** `S7-REQ-008`  
**Functional Requirement:** `FR-USPAP-007`

Each appraisal shall have an effective date.

The system shall maintain separate dates for:

- engagement;
- inspection;
- effective date;
- valuation run;
- report generation;
- report date;
- certification; and
- delivery.

---

## 3.x.3.9 Assignment Conditions

**SRS Requirement:** `S7-REQ-009`  
**Functional Requirement:** `FR-USPAP-014` through `FR-USPAP-017`

The system shall support structured recording of:

- general assumptions;
- extraordinary assumptions;
- hypothetical conditions;
- limiting conditions;
- jurisdictional requirements; and
- assignment-specific constraints.

---

# 3.x.4 Standard 7 — Scope of Work

## 3.x.4.1 Establish Scope of Work

**SRS Requirement:** `S7-REQ-010`  
**Functional Requirement:** `FR-USPAP-011`

The system shall require a documented scope of work.

The scope shall support, where applicable:

- degree of subject identification;
- inspection extent;
- research performed;
- markets researched;
- information sources;
- verification procedures;
- authenticity research;
- provenance research;
- condition analysis;
- comparable research;
- valuation approaches considered;
- valuation approaches applied; and
- reconciliation methodology.

---

## 3.x.4.2 Scope Appropriateness

**SRS Requirement:** `S7-REQ-011`  
**Functional Requirement:** `FR-USPAP-012`

The appraiser shall be able to affirm that the scope of work is appropriate for:

- intended use;
- intended users;
- subject property;
- assignment complexity;
- evidence availability; and
- relevant market.

---

## 3.x.4.3 Scope Versioning

**SRS Requirement:** `S7-REQ-012`  
**Functional Requirement:** `FR-USPAP-013`

Material changes to scope shall be versioned and shall preserve:

- prior scope;
- revised scope;
- reason for revision;
- responsible user;
- timestamp; and
- effect on appraisal development.

---

# 3.x.5 Standard 7 — Information and Evidence

## 3.x.5.1 Evidence Sources

**SRS Requirement:** `S7-REQ-013`  
**Functional Requirement:** `FR-USPAP-018`

Material information relied upon shall be attributable to a source.

Source records shall support:

- source type;
- source name;
- provider/publisher;
- reference;
- URL;
- source date;
- access date;
- verification status;
- reliability assessment; and
- notes.

---

## 3.x.5.2 Inspection Evidence

**SRS Requirement:** `S7-REQ-014`  
**Functional Requirement:** `FR-USPAP-021` through `FR-USPAP-026`

Where an inspection is performed, the system shall associate inspection evidence with the appraisal and subject property.

Inspection evidence shall support:

- inspector;
- inspection date;
- location;
- type;
- observations;
- scores;
- defects;
- red flags;
- photographs;
- supporting documents;
- measurements;
- notes; and
- limitations.

---

## 3.x.5.3 Inspection Extent and Limitations

**SRS Requirement:** `S7-REQ-015`  
**Functional Requirement:** `FR-USPAP-023`

The system shall identify whether the appraisal relied upon:

- physical inspection;
- limited physical inspection;
- remote inspection;
- third-party inspection;
- records-only review; or
- no inspection.

---

## 3.x.5.4 Third-Party Information

**SRS Requirement:** `S7-REQ-016`  
**Functional Requirement:** `FR-USPAP-019`, `FR-USPAP-020`

The system shall identify material information supplied by the client or another party and shall permit recording of:

- provider;
- date supplied;
- information type;
- verification performed;
- verification status;
- reliability concerns; and
- degree of reliance.

---

# 3.x.6 Standard 7 — Subject Property Analysis

## 3.x.6.1 Condition Analysis

**SRS Requirement:** `S7-REQ-017`  
**Functional Requirement:** `FR-USPAP-022` through `FR-USPAP-026`

The system shall support documented condition analysis when condition affects value.

Nexus may incorporate:

- canonical inspection-system scores;
- system weights;
- Condition Index;
- red flags; and
- condition narrative.

---

## 3.x.6.2 Authenticity and Originality

**SRS Requirement:** `S7-REQ-018`  
**Functional Requirement:** `FR-USPAP-027`, `FR-USPAP-028`

The system shall support analysis of:

- matching numbers;
- manufacturer records;
- serial numbers;
- component date codes;
- body authenticity;
- drivetrain authenticity;
- original components;
- replacement components;
- reproduction components;
- restoration changes; and
- independent certifications.

---

## 3.x.6.3 Provenance

**SRS Requirement:** `S7-REQ-019`  
**Functional Requirement:** `FR-USPAP-029`

The system shall support documented provenance analysis including, where applicable:

- ownership history;
- historical significance;
- competition history;
- exhibition history;
- awards;
- manufacturer records;
- title history; and
- chain-of-ownership evidence.

---

## 3.x.6.4 Rarity and Desirability

**SRS Requirement:** `S7-REQ-020`  
**Functional Requirement:** `FR-USPAP-030`

The system shall support analysis of rarity and collector desirability.

Rarity shall not automatically produce a positive valuation adjustment without supporting appraisal analysis or market evidence.

---

# 3.x.7 Standard 7 — Market Analysis

## 3.x.7.1 Relevant Market

**SRS Requirement:** `S7-REQ-021`  
**Functional Requirement:** `FR-USPAP-031`

The appraisal shall identify the market or markets relevant to the subject property and intended use.

---

## 3.x.7.2 Market Evidence

**SRS Requirement:** `S7-REQ-022`  
**Functional Requirement:** `FR-USPAP-032`, `FR-USPAP-033`

The system shall support:

- completed sales;
- auction results;
- private transactions;
- dealer transactions;
- asking prices;
- historical sales;
- price guides;
- market indices; and
- other relevant market evidence.

---

## 3.x.7.3 Transaction Verification

**SRS Requirement:** `S7-REQ-023`  
**Functional Requirement:** `FR-USPAP-034`

Comparable evidence shall support verification information including:

- source;
- transaction status;
- sale date;
- price;
- buyer's-premium treatment;
- fees;
- currency;
- venue;
- location;
- verification status; and
- reliability notes.

---

# 3.x.8 Standard 7 — Valuation Analysis

## 3.x.8.1 Valuation Approaches Considered

**SRS Requirement:** `S7-REQ-024`  
**Functional Requirement:** `FR-USPAP-038`

The system shall record valuation approaches considered.

---

## 3.x.8.2 Valuation Approaches Applied

**SRS Requirement:** `S7-REQ-025`  
**Functional Requirement:** `FR-USPAP-039`, `FR-USPAP-041` through `FR-USPAP-043`

For applied approaches, the system shall preserve:

- methodology;
- inputs;
- assumptions;
- calculations;
- adjustments;
- supporting evidence;
- intermediate conclusions; and
- appraiser narrative.

---

## 3.x.8.3 Excluded Valuation Approaches

**SRS Requirement:** `S7-REQ-026`  
**Functional Requirement:** `FR-USPAP-040`

The appraiser shall be able to document why a potentially relevant valuation approach was not used.

---

# 3.x.9 Standard 7 — Comparable Analysis

## 3.x.9.1 Comparable Selection

**SRS Requirement:** `S7-REQ-027`  
**Functional Requirement:** `FR-USPAP-033`, `FR-USPAP-035`

The appraiser or valuation system shall be able to document why a comparable is included or excluded.

---

## 3.x.9.2 Comparable Adjustments

**SRS Requirement:** `S7-REQ-028`  
**Functional Requirement:** `FR-USPAP-036`, `FR-USPAP-037`

Material adjustments shall support:

- category;
- direction;
- amount/factor;
- rationale;
- evidence;
- calculation source; and
- resulting adjusted value.

---

## 3.x.9.3 Automated Valuation Traceability

**SRS Requirement:** `S7-REQ-029`  
**Functional Requirement:** `FR-USPAP-036`, `FR-USPAP-041` through `FR-USPAP-044`, `FR-USPAP-090`

Where automated valuation is used, the system shall preserve sufficient information to reconstruct the valuation result.

This shall include:

- model version;
- engine version;
- ruleset version;
- rubric version;
- comparable set;
- similarity calculations;
- adjustment factors;
- condition factors;
- market factors;
- red-flag factors;
- manual overrides;
- intermediate calculations; and
- output values.

---

# 3.x.10 Standard 7 — Reconciliation and Final Value

## 3.x.10.1 Reconciliation

**SRS Requirement:** `S7-REQ-030`  
**Functional Requirement:** `FR-USPAP-045`

The system shall require reconciliation of material value indications before final value approval.

---

## 3.x.10.2 Final Opinion of Value

**SRS Requirement:** `S7-REQ-031`  
**Functional Requirement:** `FR-USPAP-046`, `FR-USPAP-048`

The system shall maintain an approved final opinion of value independently from individual valuation-run outputs.

The conclusion shall include:

- final value;
- optional range;
- currency;
- value type;
- effective date;
- reconciliation reference; and
- appraiser approval.

---

## 3.x.10.3 Manual Review

**SRS Requirement:** `S7-REQ-032`  
**Functional Requirement:** `FR-USPAP-044`, `FR-USPAP-047`, `FR-USPAP-048`

The system shall support manual review and override workflows where professional judgment requires additional appraisal analysis.

---

# 3.x.11 Standard 7 — Extraordinary Assumptions and Hypothetical Conditions

## 3.x.11.1 Extraordinary Assumptions

**SRS Requirement:** `S7-REQ-033`  
**Functional Requirement:** `FR-USPAP-014`, `FR-USPAP-015`

Each extraordinary assumption shall be recorded as a distinct appraisal condition.

---

## 3.x.11.2 Hypothetical Conditions

**SRS Requirement:** `S7-REQ-034`  
**Functional Requirement:** `FR-USPAP-014`, `FR-USPAP-016`

Each hypothetical condition shall be recorded separately and shall identify the known factual condition and assumed condition.

---

## 3.x.11.3 Material Assignment Condition Validation

**SRS Requirement:** `S7-REQ-035`  
**Functional Requirement:** `FR-USPAP-017`, `FR-USPAP-049`

Material extraordinary assumptions and hypothetical conditions shall be incorporated into appraisal development and flagged for required report disclosure.

---

# 3.x.12 Standard 7 Completion Gate

**Functional Requirement:** `FR-USPAP-049`

The system shall implement a Standard 7 completion gate before final development approval.

The gate shall evaluate, as applicable:

- client;
- intended use;
- intended users;
- subject property;
- relevant characteristics;
- property interest;
- value definition;
- effective date;
- scope of work;
- assignment conditions;
- evidence;
- inspection;
- subject analysis;
- market analysis;
- valuation methodology;
- comparable analysis;
- valuation trace;
- reconciliation; and
- final opinion of value.

---

# 3.x.13 Standard 8 — General Reporting Requirements

## 3.x.13.1 Clear and Accurate Communication

**SRS Requirement:** `S8-REQ-001`  
**Functional Requirement:** `FR-USPAP-052`, `FR-USPAP-053`, `FR-USPAP-077`

The report shall communicate appraisal analyses, opinions, and conclusions clearly, accurately, and without misleading intended users.

---

## 3.x.13.2 Sufficient Information

**SRS Requirement:** `S8-REQ-002`  
**Functional Requirement:** `FR-USPAP-054` through `FR-USPAP-070`

The report shall contain sufficient information for intended users to understand the appraisal.

---

## 3.x.13.3 Development-to-Report Consistency

**SRS Requirement:** `S8-REQ-003`  
**Functional Requirement:** `FR-USPAP-052`, `FR-USPAP-067`, `FR-USPAP-077`, `FR-USPAP-089`

Report information shall remain traceable to approved appraisal-development data.

---

# 3.x.14 Standard 8 — Written Report Classification

## 3.x.14.1 Report Type

**SRS Requirement:** `S8-REQ-004`  
**Functional Requirement:** `FR-USPAP-050`, `FR-USPAP-051`, `FR-USPAP-052`

Each written report shall be classified as:

- Appraisal Report; or
- Restricted Appraisal Report.

---

## 3.x.14.2 Report Type Prominence

**SRS Requirement:** `S8-REQ-005`  
**Functional Requirement:** `FR-USPAP-053`

The selected report classification shall be clearly identifiable in the generated report.

---

# 3.x.15 Standard 8 — Appraisal Report Content

## 3.x.15.1 Client

**SRS Requirement:** `S8-REQ-006`  
**Functional Requirement:** `FR-USPAP-054`

The report shall identify the client consistent with applicable confidentiality restrictions.

---

## 3.x.15.2 Intended Users

**SRS Requirement:** `S8-REQ-007`  
**Functional Requirement:** `FR-USPAP-055`

The report shall identify intended users as required by the applicable reporting option.

---

## 3.x.15.3 Intended Use

**SRS Requirement:** `S8-REQ-008`  
**Functional Requirement:** `FR-USPAP-056`

The report shall state the intended use.

---

## 3.x.15.4 Subject Property

**SRS Requirement:** `S8-REQ-009`  
**Functional Requirement:** `FR-USPAP-057`

The report shall contain sufficient information to identify the subject property and material value-relevant characteristics.

---

## 3.x.15.5 Property Interest

**SRS Requirement:** `S8-REQ-010`  
**Functional Requirement:** `FR-USPAP-058`

The report shall state the property interest appraised.

---

## 3.x.15.6 Type and Definition of Value

**SRS Requirement:** `S8-REQ-011`  
**Functional Requirement:** `FR-USPAP-059`

The report shall state the type and definition of value and source where applicable.

---

## 3.x.15.7 Effective Date

**SRS Requirement:** `S8-REQ-012`  
**Functional Requirement:** `FR-USPAP-060`

The report shall clearly state the effective date.

---

## 3.x.15.8 Report Date

**SRS Requirement:** `S8-REQ-013`  
**Functional Requirement:** `FR-USPAP-061`

The report date shall be independently identified.

---

## 3.x.15.9 Scope of Work

**SRS Requirement:** `S8-REQ-014`  
**Functional Requirement:** `FR-USPAP-062`, `FR-USPAP-063`

The report shall communicate sufficient scope information for intended users to understand the extent of appraisal development.

---

## 3.x.15.10 Property Research and Analysis

**SRS Requirement:** `S8-REQ-015`  
**Functional Requirement:** `FR-USPAP-057`, `FR-USPAP-063` through `FR-USPAP-065`

The report shall communicate material property research and analysis supporting the appraisal conclusion.

---

## 3.x.15.11 Valuation Methodology

**SRS Requirement:** `S8-REQ-016`  
**Functional Requirement:** `FR-USPAP-064`

The report shall identify and explain valuation methods and techniques used.

---

## 3.x.15.12 Comparable Evidence

**SRS Requirement:** `S8-REQ-017`  
**Functional Requirement:** `FR-USPAP-065`

Where comparable evidence materially supports the conclusion, sufficient comparable information shall be communicated.

---

## 3.x.15.13 Excluded Approaches

**SRS Requirement:** `S8-REQ-018`  
**Functional Requirement:** `FR-USPAP-040`, `FR-USPAP-064`

Where necessary for understanding the appraisal, excluded approaches shall be explained.

---

## 3.x.15.14 Reconciliation

**SRS Requirement:** `S8-REQ-019`  
**Functional Requirement:** `FR-USPAP-066`

The report shall communicate the reconciliation leading to the final opinion of value.

---

## 3.x.15.15 Final Opinion of Value

**SRS Requirement:** `S8-REQ-020`  
**Functional Requirement:** `FR-USPAP-067`

The report shall clearly communicate:

- final opinion;
- range where applicable;
- currency;
- value type; and
- effective date.

The reported opinion shall match the approved Standard 7 conclusion.

---

# 3.x.16 Standard 8 — Restricted Appraisal Report

## 3.x.16.1 Eligibility

**SRS Requirement:** `S8-REQ-021`  
**Functional Requirement:** `FR-USPAP-050`, `FR-USPAP-051`

Restricted Appraisal Report eligibility shall be validated before issuance.

---

## 3.x.16.2 Restricted Nature

**SRS Requirement:** `S8-REQ-022`  
**Functional Requirement:** `FR-USPAP-051`, `FR-USPAP-053`

The report shall clearly communicate its restricted nature and applicable limitations.

---

## 3.x.16.3 Development Requirements Remain Unchanged

**SRS Requirement:** `S8-REQ-023`  
**Functional Requirement:** `FR-USPAP-049` through `FR-USPAP-052`

Selection of a Restricted Appraisal Report shall not reduce Standard 7 development requirements.

---

## 3.x.16.4 Supporting Appraisal Record

**SRS Requirement:** `S8-REQ-024`  
**Functional Requirement:** `FR-USPAP-087` through `FR-USPAP-090`

Supporting information not reproduced in the report shall remain preserved in the appraisal record.

---

# 3.x.17 Standard 8 — Assumptions and Conditions

## 3.x.17.1 Extraordinary Assumptions

**SRS Requirement:** `S8-REQ-025`  
**Functional Requirement:** `FR-USPAP-017`, `FR-USPAP-068`

All material extraordinary assumptions used in development shall be disclosed.

---

## 3.x.17.2 Hypothetical Conditions

**SRS Requirement:** `S8-REQ-026`  
**Functional Requirement:** `FR-USPAP-017`, `FR-USPAP-069`

All hypothetical conditions used in development shall be disclosed.

---

## 3.x.17.3 Limiting Conditions

**SRS Requirement:** `S8-REQ-027`  
**Functional Requirement:** `FR-USPAP-070`

Material limiting conditions shall be communicated sufficiently for intended users to understand their effect.

---

# 3.x.18 Standard 8 — Certification

## 3.x.18.1 Certification Required

**SRS Requirement:** `S8-REQ-028`  
**Functional Requirement:** `FR-USPAP-071`

A written appraisal report shall require certification prior to formal issuance.

---

## 3.x.18.2 Responsible Appraiser

**SRS Requirement:** `S8-REQ-029`  
**Functional Requirement:** `FR-USPAP-072`, `FR-USPAP-075`

The certification shall identify the responsible appraiser.

---

## 3.x.18.3 Significant Appraisal Assistance

**SRS Requirement:** `S8-REQ-030`  
**Functional Requirement:** `FR-USPAP-073`

The system shall support identification and disclosure of significant appraisal assistance.

---

## 3.x.18.4 Certification Integrity

**SRS Requirement:** `S8-REQ-031`  
**Functional Requirement:** `FR-USPAP-074`, `FR-USPAP-080` through `FR-USPAP-083`

Substantive changes to issued reports shall require versioning and, where required, new certification.

---

# 3.x.19 Standard 8 — Oral Reports

## 3.x.19.1 Oral Communication Record

**SRS Requirement:** `S8-REQ-032`  
**Functional Requirement:** `FR-USPAP-086`

Where oral appraisal reporting is supported, the system shall maintain a record of the communication.

---

## 3.x.19.2 Oral Report Support

**SRS Requirement:** `S8-REQ-033`  
**Functional Requirement:** `FR-USPAP-086` through `FR-USPAP-090`

The appraisal record shall contain sufficient supporting information for an oral appraisal report.

---

# 3.x.20 Standard 8 — Report Generation and Issuance

## 3.x.20.1 Generate From Approved Data

**SRS Requirement:** `S8-REQ-034`  
**Functional Requirement:** `FR-USPAP-052`

Critical report information shall be generated from approved appraisal-record data.

---

## 3.x.20.2 Report Versioning

**SRS Requirement:** `S8-REQ-035`  
**Functional Requirement:** `FR-USPAP-080` through `FR-USPAP-083`

Each report shall maintain a unique version and immutable issued artifact.

---

## 3.x.20.3 Draft Identification

**SRS Requirement:** `S8-REQ-036`  
**Functional Requirement:** `FR-USPAP-078`

Draft reports shall be clearly distinguishable from issued reports.

---

## 3.x.20.4 Report Readiness Validation

**SRS Requirement:** `S8-REQ-037`  
**Functional Requirement:** `FR-USPAP-076` through `FR-USPAP-079`, `FR-USPAP-095`

The system shall validate all applicable reporting requirements before issuance.

---

# 3.x.21 Standard 8 — Report Delivery

## 3.x.21.1 Delivery Tracking

**SRS Requirement:** `S8-REQ-038`  
**Functional Requirement:** `FR-USPAP-084`

The system shall permit delivery events to be associated with specific report versions.

---

## 3.x.21.2 Recipient Versus Intended User

**SRS Requirement:** `S8-REQ-039`  
**Functional Requirement:** `FR-USPAP-004`, `FR-USPAP-085`

Report-recipient status shall remain distinct from intended-user status.

---

# 3.x.22 Cross-Standard Requirements

## 3.x.22.1 Complete Appraisal Record

**SRS Requirement:** `CROSS-REQ-001`  
**Functional Requirement:** `FR-USPAP-087`

The system shall maintain a complete appraisal-centric record supporting development and reporting.

---

## 3.x.22.2 Evidence-to-Conclusion Traceability

**SRS Requirement:** `CROSS-REQ-002`  
**Functional Requirement:** `FR-USPAP-088`, `FR-USPAP-090`

Material conclusions shall be traceable to supporting evidence and analysis.

---

## 3.x.22.3 Report-to-Appraisal Traceability

**SRS Requirement:** `CROSS-REQ-003`  
**Functional Requirement:** `FR-USPAP-089`

Every issued report shall remain traceable to the appraisal and approved final opinion from which it was generated.

---

## 3.x.22.4 Evidence and Historical Integrity

**SRS Requirement:** `CROSS-REQ-004`, `CROSS-REQ-005`  
**Functional Requirement:** `FR-USPAP-080` through `FR-USPAP-083`, `FR-USPAP-087`

Material historical appraisal evidence and issued reports shall not be silently overwritten.

---

## 3.x.22.5 Audit Trail

**SRS Requirement:** `CROSS-REQ-006`, `CROSS-REQ-007`  
**Functional Requirement:** `FR-USPAP-091`, `FR-USPAP-092`

Material appraisal events shall be audited with sufficient metadata for reconstruction.

---

## 3.x.22.6 Workflow Control

**SRS Requirement:** `CROSS-REQ-008` through `CROSS-REQ-010`  
**Functional Requirement:** `FR-USPAP-093` through `FR-USPAP-095`

The system shall enforce controlled appraisal lifecycle transitions and prevent premature report issuance.

---

# 3.x.23 Formal Compliance Gates

## 3.x.23.1 Gate A — Assignment Ready

The Assignment Ready gate shall validate:

- client;
- intended use;
- intended users;
- subject property;
- property interest;
- value definition;
- effective-date treatment;
- responsible appraiser; and
- scope of work.

Primary functional requirements:

`FR-USPAP-001` through `FR-USPAP-013`

---

## 3.x.23.2 Gate B — Standard 7 Development Complete

The Standard 7 gate shall evaluate applicable requirements from:

`S7-REQ-001` through `S7-REQ-035`

Primary functional requirement:

`FR-USPAP-049`

The gate shall not pass while material appraisal-development requirements remain unresolved.

---

## 3.x.23.3 Gate C — Standard 8 Report Ready

The Standard 8 gate shall evaluate applicable requirements from:

`S8-REQ-001` through `S8-REQ-039`

Primary functional requirements:

- `FR-USPAP-076`
- `FR-USPAP-095`

The gate shall validate:

- report classification;
- report content;
- Standard 7/report consistency;
- disclosures;
- certification;
- report version;
- issuance readiness; and
- blocking compliance conditions.

---

# 3.x.24 Standard 7 → Standard 8 Handoff

| Standard 7 Development Output | Standard 8 Reporting Requirement | Functional Requirement |
|---|---|---|
| Client identified | Client communicated | FR-USPAP-002 → FR-USPAP-054 |
| Intended users identified | Intended users communicated | FR-USPAP-004 → FR-USPAP-055 |
| Intended use established | Intended use communicated | FR-USPAP-003 → FR-USPAP-056 |
| Subject identified | Subject described | FR-USPAP-008–010 → FR-USPAP-057 |
| Property interest identified | Property interest reported | FR-USPAP-005 → FR-USPAP-058 |
| Value definition established | Value definition reported | FR-USPAP-006 → FR-USPAP-059 |
| Effective date established | Effective date reported | FR-USPAP-007 → FR-USPAP-060 |
| Scope developed | Scope communicated | FR-USPAP-011–013 → FR-USPAP-062 |
| Inspection scope determined | Inspection extent disclosed | FR-USPAP-023 → FR-USPAP-063 |
| Subject characteristics analyzed | Material characteristics reported | FR-USPAP-024–030 → FR-USPAP-057, 063 |
| Market analyzed | Relevant market evidence communicated | FR-USPAP-031–034 → FR-USPAP-064–065 |
| Comparables analyzed | Comparable evidence communicated | FR-USPAP-033–037 → FR-USPAP-065 |
| Valuation methods applied | Methodology communicated | FR-USPAP-038–043 → FR-USPAP-064 |
| Reconciliation completed | Reconciliation communicated | FR-USPAP-045 → FR-USPAP-066 |
| Extraordinary assumptions used | Extraordinary assumptions disclosed | FR-USPAP-015 → FR-USPAP-068 |
| Hypothetical conditions used | Hypothetical conditions disclosed | FR-USPAP-016 → FR-USPAP-069 |
| Final opinion approved | Final opinion communicated | FR-USPAP-046–048 → FR-USPAP-067 |
| Significant assistance recorded | Assistance disclosed | FR-USPAP-073 |

---

# 3.x.25 USPAP Standard-to-SRS Cross-Reference Matrix

> **Note:** This matrix intentionally maps USPAP requirement areas to Nexus SRS requirements without reproducing proprietary USPAP text. Rule numbering should be verified against the specific USPAP edition adopted by the project before final document lock.

| USPAP Standard | Rule / Section | Subsection / Requirement Area | SRS Requirement | FR Cross-Reference |
|---|---|---|---|---|
| Standard 7 | SR 7-1 | General appraisal-development requirements | 3.x.2, 3.x.12 | FR-USPAP-049, 087–095 |
| Standard 7 | SR 7-2 | Client | 3.x.3.1 / S7-REQ-001 | FR-USPAP-001–002 |
| Standard 7 | SR 7-2 | Intended users | 3.x.3.2 / S7-REQ-002 | FR-USPAP-004 |
| Standard 7 | SR 7-2 | Intended use | 3.x.3.3 / S7-REQ-003 | FR-USPAP-003 |
| Standard 7 | SR 7-2 | Subject-property identification | 3.x.3.4 / S7-REQ-004 | FR-USPAP-008 |
| Standard 7 | SR 7-2 | Relevant characteristics | 3.x.3.5 / S7-REQ-005 | FR-USPAP-009–010 |
| Standard 7 | SR 7-2 | Property interest | 3.x.3.6 / S7-REQ-006 | FR-USPAP-005 |
| Standard 7 | SR 7-2 | Type/definition of value | 3.x.3.7 / S7-REQ-007 | FR-USPAP-006 |
| Standard 7 | SR 7-2 | Effective date | 3.x.3.8 / S7-REQ-008 | FR-USPAP-007 |
| Standard 7 | SR 7-2 | Assignment conditions | 3.x.3.9 / S7-REQ-009 | FR-USPAP-014–017 |
| Standard 7 | SR 7-2 / Scope of Work | Scope determination | 3.x.4.1–3.x.4.3 / S7-REQ-010–012 | FR-USPAP-011–013 |
| Standard 7 | SR 7-3 | Information and analysis relevant to property | 3.x.5–3.x.6 / S7-REQ-013–020 | FR-USPAP-018–030 |
| Standard 7 | SR 7-3 | Inspection / identification extent | 3.x.5.2–3.x.5.3 | FR-USPAP-022–026 |
| Standard 7 | SR 7-3 | Authenticity / originality | 3.x.6.2 / S7-REQ-018 | FR-USPAP-027–028 |
| Standard 7 | SR 7-3 | Provenance | 3.x.6.3 / S7-REQ-019 | FR-USPAP-029 |
| Standard 7 | SR 7-3 | Rarity / value-relevant attributes | 3.x.6.4 / S7-REQ-020 | FR-USPAP-030 |
| Standard 7 | SR 7-4 | Market analysis | 3.x.7 / S7-REQ-021–023 | FR-USPAP-031–034 |
| Standard 7 | SR 7-4 | Comparable-sales analysis | 3.x.9 / S7-REQ-027–029 | FR-USPAP-033–044, 090 |
| Standard 7 | SR 7-4 | Valuation methodology | 3.x.8 / S7-REQ-024–026 | FR-USPAP-038–043 |
| Standard 7 | SR 7-5 | Reconciliation / final conclusion | 3.x.10 / S7-REQ-030–032 | FR-USPAP-045–048 |
| Standard 7 | Applicable assignment conditions | Extraordinary assumptions | 3.x.11.1 / S7-REQ-033 | FR-USPAP-014–015 |
| Standard 7 | Applicable assignment conditions | Hypothetical conditions | 3.x.11.2 / S7-REQ-034 | FR-USPAP-014, 016 |
| Standard 7 | Completion control | Development completion | 3.x.12 / S7-REQ-035 | FR-USPAP-017, 049 |
| Standard 8 | SR 8-1 | Clear, accurate, non-misleading reporting | 3.x.13.1 / S8-REQ-001 | FR-USPAP-052–053, 077 |
| Standard 8 | SR 8-1 | Sufficient information | 3.x.13.2 / S8-REQ-002 | FR-USPAP-054–070 |
| Standard 8 | SR 8-1 | Consistency / disclosure | 3.x.13.3 / S8-REQ-003 | FR-USPAP-052, 067, 077, 089 |
| Standard 8 | SR 8-2 | Written report classification | 3.x.14 / S8-REQ-004–005 | FR-USPAP-050–053 |
| Standard 8 | SR 8-2 | Client | 3.x.15.1 / S8-REQ-006 | FR-USPAP-054 |
| Standard 8 | SR 8-2 | Intended users | 3.x.15.2 / S8-REQ-007 | FR-USPAP-055 |
| Standard 8 | SR 8-2 | Intended use | 3.x.15.3 / S8-REQ-008 | FR-USPAP-056 |
| Standard 8 | SR 8-2 | Subject identification | 3.x.15.4 / S8-REQ-009 | FR-USPAP-057 |
| Standard 8 | SR 8-2 | Property interest | 3.x.15.5 / S8-REQ-010 | FR-USPAP-058 |
| Standard 8 | SR 8-2 | Value type / definition | 3.x.15.6 / S8-REQ-011 | FR-USPAP-059 |
| Standard 8 | SR 8-2 | Effective date | 3.x.15.7 / S8-REQ-012 | FR-USPAP-060 |
| Standard 8 | SR 8-2 | Report date | 3.x.15.8 / S8-REQ-013 | FR-USPAP-061 |
| Standard 8 | SR 8-2 | Scope of work | 3.x.15.9 / S8-REQ-014 | FR-USPAP-062–063 |
| Standard 8 | SR 8-2 | Property analysis | 3.x.15.10 / S8-REQ-015 | FR-USPAP-057, 063–065 |
| Standard 8 | SR 8-2 | Valuation methods | 3.x.15.11 / S8-REQ-016 | FR-USPAP-064 |
| Standard 8 | SR 8-2 | Comparable evidence | 3.x.15.12 / S8-REQ-017 | FR-USPAP-065 |
| Standard 8 | SR 8-2 | Excluded approach explanation | 3.x.15.13 / S8-REQ-018 | FR-USPAP-040, 064 |
| Standard 8 | SR 8-2 | Reconciliation | 3.x.15.14 / S8-REQ-019 | FR-USPAP-066 |
| Standard 8 | SR 8-2 | Value conclusion | 3.x.15.15 / S8-REQ-020 | FR-USPAP-067 |
| Standard 8 | SR 8-2 | Restricted Appraisal Report | 3.x.16 / S8-REQ-021–024 | FR-USPAP-049–053, 087–090 |
| Standard 8 | SR 8-2 | Extraordinary assumption disclosure | 3.x.17.1 / S8-REQ-025 | FR-USPAP-017, 068 |
| Standard 8 | SR 8-2 | Hypothetical-condition disclosure | 3.x.17.2 / S8-REQ-026 | FR-USPAP-017, 069 |
| Standard 8 | SR 8-2 | Limiting conditions | 3.x.17.3 / S8-REQ-027 | FR-USPAP-070 |
| Standard 8 | SR 8-3 / Certification provisions | Certification | 3.x.18 / S8-REQ-028–031 | FR-USPAP-071–075, 080–083 |
| Standard 8 | Oral report provisions | Oral appraisal communication | 3.x.19 / S8-REQ-032–033 | FR-USPAP-086–090 |
| Standard 8 | Reporting controls | Report generation/versioning | 3.x.20 / S8-REQ-034–037 | FR-USPAP-052, 076–083, 095 |
| Standard 8 | Delivery controls | Report delivery | 3.x.21 / S8-REQ-038–039 | FR-USPAP-004, 084–085 |
| Cross-standard | Record Keeping / Workfile support | Appraisal record integrity | 3.x.22 | FR-USPAP-087–092 |
| Cross-standard | Workflow | Development/report issuance gates | 3.x.23 | FR-USPAP-049, 076, 093–095 |

---

# 3.x.26 Functional Requirement Summary

| SRS Area | Functional Requirements |
|---|---|
| Assignment / Problem Identification | FR-USPAP-001–017 |
| Evidence | FR-USPAP-018–021 |
| Inspection / Condition | FR-USPAP-022–026 |
| Authenticity / Originality / Provenance | FR-USPAP-027–030 |
| Market / Comparables | FR-USPAP-031–037 |
| Valuation | FR-USPAP-038–044 |
| Reconciliation / Final Opinion | FR-USPAP-045–049 |
| Report Type / Generation | FR-USPAP-050–053 |
| Report Content | FR-USPAP-054–070 |
| Certification | FR-USPAP-071–075 |
| Finalization / Issuance | FR-USPAP-076–079 |
| Report Versioning | FR-USPAP-080–083 |
| Delivery | FR-USPAP-084–085 |
| Oral Reporting | FR-USPAP-086 |
| Appraisal Record / Traceability | FR-USPAP-087–090 |
| Audit | FR-USPAP-091–092 |
| Workflow | FR-USPAP-093–095 |

---

# 3.x.27 Governing SRS Requirement

ONE Classics Nexus shall preserve direct, auditable, and versioned traceability between appraisal development under Standard 7 and appraisal communication under Standard 8.

The governing implementation chain shall be:

```text
USPAP Standard
    ↓
SRS Requirement
    ↓
FR-USPAP Requirement
    ↓
Domain Entity
    ↓
Database Record
    ↓
Application Service
    ↓
API Contract
    ↓
WinForms Control
    ↓
Validation Rule
    ↓
Automated Test
```

For the appraisal itself, the governing evidentiary chain shall be:

```text
Evidence
    ↓
Analysis
    ↓
Valuation
    ↓
Reconciliation
    ↓
Approved Final Opinion
    ↓
Report
    ↓
Certification
    ↓
Issued Report
```

A material change at an earlier stage after report issuance shall not silently propagate into an existing issued report.

Such a change shall require an appropriate revision workflow and, where applicable, a newly certified report version.

This section shall form the canonical **USPAP Standards 7 & 8 requirements section of the ONE Classics Nexus SRS v2.x**.