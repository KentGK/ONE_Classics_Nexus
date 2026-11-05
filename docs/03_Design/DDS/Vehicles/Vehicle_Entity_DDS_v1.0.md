# ONE CLASSICS NEXUS
## Detailed Design Specification
### Vehicle Entity
### Version 1.0 — Appraisal-Centric / USPAB Aligned

**Document Type:** Entity Detailed Design Specification  
**Parent DDS:** Nexus DDS v3.0 — Appraisal-Centric Platform Architecture  
**Requirements Baseline:** Nexus SRS v2.0; USPAP Standards 7 & 8 Requirements v3.0  
**System:** `ONE_Classics_Nexus`  
**Target Platform:** .NET 8 / ASP.NET Core / WinForms / MariaDB  
**Architecture:** Layered / Clean Architecture  
**Aggregate Root:** `Vehicle`  
**Primary Physical Table:** `vehicles`  
**Status:** Proposed Implementation Baseline  

---

# 1. PURPOSE

The `Vehicle` entity represents the **current reusable master record of a physical collector, classic, custom, or standard vehicle known to ONE Classics Nexus**.

The Vehicle exists independently of any individual appraisal assignment.

A Vehicle may:

- exist before an appraisal is created;
- originate from an appraisal request or other intake process;
- participate in multiple appraisals over its lifetime;
- accumulate improved identification and descriptive information;
- be inspected multiple times;
- be associated with historical appraisal subject snapshots; and
- continue to evolve after a prior appraisal has been completed or issued.

The parent Nexus DDS expressly identifies `Vehicle` as the aggregate responsible for the **current reusable physical vehicle record**, while `Appraisal` remains the governing aggregate for appraisal workflow.

Accordingly:

> **The Vehicle is the master asset record. It is not the historical subject record of a completed appraisal.**

For each appraisal, the value-relevant state of the Vehicle shall be captured in an `AppraisalSubjectSnapshot`.

The governing DDS establishes that completed appraisals must not rely solely on the mutable `vehicles` row and requires preservation of identifying, mechanical, condition-related, authenticity, restoration, documentation, provenance, and ownership characteristics in the appraisal subject snapshot.

---

# 2. DESIGN AUTHORITY

The Vehicle design shall follow the Nexus requirements hierarchy:

```text
USPAP Standards 7 & 8 Requirements
        ↓
Nexus SRS
        ↓
Nexus DDS
        ↓
Vehicle Entity DDS
        ↓
Domain Model
        ↓
Application Services
        ↓
Persistence
        ↓
REST API
        ↓
WinForms UI
        ↓
Validation
        ↓
Automated Tests
```

The parent DDS requires preservation of the existing layered .NET 8 architecture and establishes direct traceability through domain, database, services, APIs, UI, validation, and tests.

Where the implementation-derived SRS differs from newer approved DDS decisions:

1. existing working behavior shall be preserved where compatible;
2. later approved domain decisions shall govern future implementation;
3. new behavior shall not silently invalidate historical appraisal data; and
4. configuration shall be versioned where it affects appraisal results.

---

# 3. ARCHITECTURAL CONTEXT

The existing solution contains the following principal projects:

```text
ONE_Classics_Nexus.Domain
ONE_Classics_Nexus.Application
ONE_Classics_Nexus.Infrastructure
ONE_Classics_Nexus.API
ONE_Classics_Nexus.WinForms
ONE_Classics_Nexus.Tests
```

The solution file confirms these as separate projects, while the solution documentation structure separately contains DDS, ERD, sequence-diagram, requirements, and traceability assets.

The implementation-derived SRS assigns responsibilities as follows:

| Layer | Vehicle Responsibility |
|---|---|
| Domain | Vehicle entity, invariants, value objects, domain rules |
| Application | Vehicle commands, queries, DTOs, service/repository abstractions |
| Infrastructure | MariaDB/Dapper persistence |
| API | HTTP contracts and vehicle endpoints |
| WinForms | Vehicle search, create/edit/detail workflows |
| Tests | Domain, application, API, repository, integration tests |

The SRS identifies the same clean-architecture division and specifies .NET 8 for the primary Nexus projects.

---

# 4. APPRAISAL-CENTRIC POSITION

The logical relationship shall be:

```text
                      ┌──────────────────┐
                      │     Vehicle      │
                      │ Current Master   │
                      └────────┬─────────┘
                               │
                    1          │          0..*
                               ↓
                      ┌──────────────────┐
                      │    Appraisal     │
                      │ Workflow Context │
                      └────────┬─────────┘
                               │
                               ↓
                   ┌────────────────────────┐
                   │ AppraisalSubjectSnapshot│
                   │ Historical Subject State│
                   └────────────────────────┘
```

More generally:

```text
Vehicle
   │
   ├── Appraisal #1
   │      └── Subject Snapshot v1
   │
   ├── Appraisal #2
   │      └── Subject Snapshot v2
   │
   └── Appraisal #3
          └── Subject Snapshot v3
```

Changes to the current Vehicle after Appraisal #1 shall not alter the historical subject facts preserved for Appraisal #1.

This implements the parent DDS design decision:

```text
Vehicle
    = reusable current asset record

AppraisalSubjectSnapshot
    = property state relied upon
      for a particular appraisal
```

---

# 5. USPAP DOMAIN BOUNDARY

## 5.1 Vehicle Is Not the Appraisal Conclusion

The Vehicle may contain current descriptive information concerning:

- identity;
- configuration;
- mechanical specification;
- mileage;
- colors;
- title status;
- authenticity profile;
- restoration profile;
- documentation profile;
- ownership summary; and
- descriptive notes.

The Vehicle shall not own:

- intended use;
- intended users;
- property interest appraised;
- value definition;
- appraisal effective date;
- assignment conditions;
- scope of work;
- final appraisal condition conclusion;
- final authenticity conclusion;
- final originality conclusion;
- final provenance conclusion;
- valuation reconciliation;
- final opinion of value; or
- appraisal report conclusions.

Those concepts belong to the Appraisal context.

---

# 6. STANDARD 7 SUBJECT IDENTIFICATION RESPONSIBILITY

USPAP-aligned requirements require Nexus to uniquely identify the subject property.

For collector vehicles, supported identification includes, where applicable:

- Vehicle ID;
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

The Vehicle master shall therefore contain or support these identifiers where they represent current known vehicle facts.

The appraisal snapshot shall copy the appraisal-relevant values actually relied upon.

---

# 7. VALUE-RELEVANT VEHICLE CHARACTERISTICS

The Standard 7 requirements additionally require support for value-relevant characteristics such as:

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

The Vehicle master may provide **current or preliminary information** concerning these characteristics.

However:

> Current Vehicle master attributes shall never silently replace an appraisal-specific professional analysis.

Detailed analytical conclusions belong to the Appraisal's Subject Analysis, Inspection, Authenticity, Provenance, Market, and Reconciliation records.

---

# 8. AGGREGATE DESIGN

## 8.1 Aggregate Root

`Vehicle` shall be an independent aggregate root.

Recommended conceptual model:

```csharp
Vehicle
{
    Guid VehicleId;

    Guid? VehicleTypeId;

    VehicleIdentity Identity;
    VehicleConfiguration Configuration;
    VehicleOdometer Odometer;

    string? ExteriorColor;
    string? InteriorColor;
    string? TitleStatus;
    string? OdometerStatus;

    string? AuthenticityProfile;
    string? OriginalityProfile;
    string? RestorationLevel;
    string? DocumentationLevel;

    string? OwnershipNotes;
    string? Notes;
    string VehicleStatus;

    long RowVersion;

    DateTime CreatedAtUtc;
    DateTime UpdatedAtUtc;
}
```

For compatibility with the existing Nexus implementation, these may initially remain scalar properties on the entity and physical `vehicles` table.

The value-object decomposition is a logical design boundary and need not require immediate table decomposition.

---

# 9. PROPOSED VEHICLE ENTITY

Recommended production entity:

```csharp
public sealed class Vehicle
{
    public Guid VehicleId { get; private set; }

    public Guid? VehicleTypeId { get; private set; }

    // Primary identity
    public string? Vin { get; private set; }
    public string? ChassisNumber { get; private set; }
    public string? BodyNumber { get; private set; }
    public string? EngineNumber { get; private set; }

    // Classification
    public int Year { get; private set; }
    public string Make { get; private set; } = string.Empty;
    public string? Marque { get; private set; }
    public string Model { get; private set; } = string.Empty;
    public string? SeriesName { get; private set; }
    public string? Trim { get; private set; }
    public string? BodyStyle { get; private set; }

    // Mechanical configuration
    public string? Engine { get; private set; }
    public string? Transmission { get; private set; }
    public string? Drivetrain { get; private set; }

    // Odometer
    public int? Mileage { get; private set; }
    public string? OdometerUnit { get; private set; }
    public string? OdometerStatus { get; private set; }

    // Appearance
    public string? ExteriorColor { get; private set; }
    public string? InteriorColor { get; private set; }

    // Current known status/profile
    public string? TitleStatus { get; private set; }
    public string? AuthenticityProfile { get; private set; }
    public string? OriginalityProfile { get; private set; }
    public string? RestorationLevel { get; private set; }
    public string? DocumentationLevel { get; private set; }

    // Current summary data
    public decimal? OverallConditionScore { get; private set; }
    public bool HasRedFlags { get; private set; }

    public string? OwnershipNotes { get; private set; }
    public string? Notes { get; private set; }

    // Current master-record lifecycle status.
    public string VehicleStatus { get; private set; } = "Active";

    // Concurrency / audit
    public long RowVersion { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
}
```

---

# 10. EXISTING VEHICLE FUNCTIONAL REQUIREMENTS

The current implementation-derived SRS establishes the following working baseline:

| Requirement | Behavior |
|---|---|
| `FR-VEH-001` | Create a Vehicle |
| `FR-VEH-002` | Retrieve Vehicle by ID |
| `FR-VEH-003` | List Vehicles with paging/filtering |
| `FR-VEH-004` | Update Vehicle |

The existing creation requirement already supports make, model, year, VIN, trim, series, body style, drivetrain, engine, transmission, mileage, odometer unit, colors, title status, authenticity/restoration/documentation profile, and notes.

This DDS expands that implementation baseline without invalidating those existing functions.

---

# 11. VEHICLE TYPE CLASSIFICATION

Each Vehicle should optionally reference:

```text
vehicle_types
```

Relationship:

```text
VehicleType 1 ───────< Vehicle 0..*
```

Recommended classifications include:

```text
Standard
Custom
Vintage/Classic
```

Vehicle type shall influence appraisal and inspection configuration, but shall not change the identity of the canonical inspection systems.

The parent DDS expressly requires vehicle-type-specific behavior to be configured through template/ruleset data instead of changing canonical system identities.

Therefore:

```text
Vehicle
    ↓
VehicleType
    ↓
Applicable Inspection Template
Applicable Weight Configuration
Applicable Ruleset
Applicable Valuation Profile
```

Vehicle type is classification data.

It is **not** itself a valuation conclusion.

---

# 12. CANONICAL INSPECTION RELATIONSHIP

The target Nexus design uses twelve invariant inspection-system identities:

```text
1   BODY_EXT       body_exterior
2   FRAME_STRUCT   frame_structure
3   POWERTRAIN     powertrain
4   TRANS_DRIVE    transmission_driveline
5   SUS_STEER      suspension_steering
6   BRAKE_SYS      braking_system
7   ELEC_SYS       electrical_system
8   INTERIOR       interior
9   WHEEL_TIRE     wheels_tires
10  TRIM_GLASS     trim_glass_details
11  DOC_PROV       documentation_provenance
12  ROAD_FUNC      road_test_functional
```

Vehicle type shall not create, remove, renumber, or rename these canonical identities.

Instead:

```text
VehicleType
      ↓
VehicleTypeSystemWeight
      ↓
Inspection / Ruleset Resolution
```

The appraisal or valuation record must preserve the effective configuration actually used.

---

# 13. VEHICLE VERSUS INSPECTION

A Vehicle does not own authoritative condition scoring.

Condition observations originate through:

```text
Vehicle
   ↓
Appraisal
   ↓
Inspection
   ↓
Condition Scores
```

The SRS identifies the implemented process as:

```text
Vehicle
   → Appraisal
      → Inspection
         → Condition Scores
      → Comparable Sales
      → Valuation Run
      → Report
```



Therefore:

- detailed inspection scores belong to an Inspection;
- red flags belong to the inspection/evidence context;
- appraisal condition analysis belongs to the Appraisal;
- Vehicle may contain a current denormalized condition summary only.

---

# 14. DENORMALIZED CONDITION SUMMARY

The following Vehicle attributes may be retained for efficient UI/search use:

```text
overall_condition_score
has_red_flags
```

These fields shall be treated as:

> **Current convenience summaries, not authoritative historical appraisal conclusions.**

They may be updated from an approved/current inspection through an explicit application service.

They shall not be used to reconstruct an issued appraisal.

An appraisal shall use its associated inspections, preserved subject snapshot, analysis records, and valuation trace.

---

# 15. IDENTITY DESIGN

## 15.1 Primary Key

```text
vehicle_id UUID/GUID
```

Requirements:

- generated once;
- globally unique;
- never reused;
- independent of VIN;
- independent of appraisal;
- stable for the lifetime of the Vehicle record.

---

# 16. VIN DESIGN

`vin` is an important identifier but shall not serve as the primary key.

Reasons include:

- pre-VIN collector vehicles;
- chassis-number-based identification;
- incomplete or unreadable VINs;
- non-US vehicles;
- reconstructed vehicles;
- specialized coachbuilt vehicles;
- historical numbering schemes;
- duplicate-data cleanup requirements.

Recommended rule:

```text
VehicleId = technical identity
VIN       = subject identifier
```

VIN uniqueness shall therefore be handled deliberately rather than assuming every historical vehicle has a modern 17-character VIN.

---

# 17. ALTERNATE IDENTIFIERS

The target model shall support, at minimum:

```text
vin
chassis_number
body_number
engine_number
```

Future normalization may introduce:

```text
vehicle_identifiers
```

Example:

```text
vehicle_identifier_id
vehicle_id
identifier_type
identifier_value
issuing_authority
is_primary
verification_status
notes
created_at_utc
updated_at_utc
```

This normalization is recommended for later implementation if Nexus must support complex collector-vehicle identity analysis.

For v1.0, scalar master fields are acceptable.

---

# 18. CLASSIFICATION FIELDS

Recommended fields:

```text
year
make
marque
model
series_name
trim
body_style
vehicle_type_id
```

### Definitions

**Make**  
Manufacturer or commonly recognized manufacturer designation.

**Marque**  
Optional historically or commercially relevant marque when distinct from corporate manufacturer naming.

**Model**  
Primary model designation.

**Series**  
Series, generation, platform, model family, or subseries.

**Trim**  
Factory trim or configuration designation.

**Body Style**  
Coupe, convertible, roadster, sedan, wagon, pickup, etc.

---

# 19. MECHANICAL CONFIGURATION

Recommended master fields:

```text
engine
transmission
drivetrain
```

These fields describe the **current known configuration**.

The master record shall not automatically assert:

- original engine;
- matching-numbers engine;
- factory-installed transmission;
- authenticity;
- originality; or
- correctness.

Those determinations require appraisal evidence and analysis.

Example:

```text
Vehicle.Engine = "427 CID V8"

does NOT imply

AppraisalSubjectAnalysis.EngineOriginality = ConfirmedOriginal
```

---

# 20. ODOMETER DESIGN

Recommended fields:

```text
mileage INT
odometer_unit
```

Controlled `odometer_unit` values should include:

```text
miles
kilometers
hours
unknown
not_applicable
```

Future expansion should support:

```text
odometer_status
```

Examples:

```text
Actual
ExceedsMechanicalLimits
NotActual
TMU
Unknown
Replaced
```

Mileage shall always be interpreted relative to:

- date observed;
- inspection;
- supporting evidence; and
- odometer status.

The Vehicle field represents the current known summary only.

---

# 21. TITLE STATUS

Recommended:

```text
title_status
```

The master field may represent the latest known title status.

Possible controlled values may include:

```text
Clean
Salvage
Rebuilt
Flood
Bonded
PartsOnly
NoTitle
BillOfSale
ForeignRegistration
Unknown
```

Jurisdiction-specific definitions shall not be hard-coded into the Vehicle entity.

Material title conditions used in an appraisal must be preserved in the appraisal evidence and subject snapshot.

---

# 22. AUTHENTICITY PROFILE

Recommended current master attribute:

```text
authenticity_profile
```

This represents a **summary classification**, not the evidentiary record supporting an appraisal conclusion.

Detailed authenticity analysis shall remain appraisal-specific and may include:

- matching numbers;
- manufacturer records;
- component serial numbers;
- date codes;
- body authenticity;
- drivetrain authenticity;
- original components;
- replacement components;
- reproduction components;
- restoration changes; and
- independent certifications.

These are specifically contemplated by the Standard 7 requirements.

---

# 23. ORIGINALITY PROFILE

Recommended new/current master attribute:

```text
originality_profile
```

Suggested controlled categories may include:

```text
Unknown
Original
PredominantlyOriginal
Restored
PartiallyRestored
Modified
HighlyModified
Reconstructed
```

These values are descriptive master classifications only.

Appraisal-specific originality conclusions shall include supporting evidence and analytical narrative.

---

# 24. RESTORATION LEVEL

Recommended:

```text
restoration_level
```

Potential classifications:

```text
Unknown
Unrestored
Preserved
DriverRestoration
OlderRestoration
FullRestoration
ConcoursRestoration
Restomod
Reconstruction
```

Restoration quality and restoration level should remain conceptually distinct.

Future normalized analysis may separately capture:

```text
restoration_date
restoration_quality
restoration_scope
restorer
restoration_documentation
```

---

# 25. DOCUMENTATION LEVEL

Recommended:

```text
documentation_level
```

This shall provide a current searchable summary of the Vehicle's documentation.

Potential categories:

```text
Unknown
Minimal
Partial
Good
Extensive
Exceptional
```

Actual appraisal reliance must occur through `AppraisalEvidence`.

---

# 26. OWNERSHIP SUMMARY

Recommended:

```text
ownership_notes TEXT
```

This is a descriptive current summary.

It shall not replace detailed provenance records where provenance materially affects value.

USPAP-oriented requirements support analysis of:

- ownership history;
- historical significance;
- competition history;
- exhibition history;
- awards;
- manufacturer records;
- title history; and
- chain-of-ownership evidence.

---

# 27. NOTES

```text
notes TEXT
```

Vehicle notes shall be understood as:

- operational;
- descriptive;
- intake-oriented; or
- master-record annotations.

They shall not be treated as automatically verified appraisal facts.

Where a Vehicle note becomes material to an appraisal:

```text
Vehicle Note
     ↓
Appraisal Evidence / Analysis
     ↓
Verification
     ↓
Appraisal Conclusion
```

---

# 28. DATABASE DESIGN

Recommended target table:

```sql
CREATE TABLE vehicles
(
    vehicle_id               CHAR(36)      NOT NULL,

    vehicle_type_id          CHAR(36)      NULL,

    vin                      VARCHAR(32)   NULL,
    chassis_number           VARCHAR(64)   NULL,
    body_number              VARCHAR(64)   NULL,
    engine_number            VARCHAR(64)   NULL,

    year                     SMALLINT      NOT NULL,
    make                     VARCHAR(100)  NOT NULL,
    marque                   VARCHAR(100)  NULL,
    model                    VARCHAR(100)  NOT NULL,
    series_name              VARCHAR(100)  NULL,
    trim                     VARCHAR(100)  NULL,
    body_style               VARCHAR(100)  NULL,

    engine                   VARCHAR(255)  NULL,
    transmission             VARCHAR(255)  NULL,
    drivetrain               VARCHAR(100)  NULL,

    mileage                  DECIMAL(12,1) NULL,
    odometer_unit            VARCHAR(20)   NULL,

    exterior_color           VARCHAR(100)  NULL,
    interior_color           VARCHAR(100)  NULL,

    title_status             VARCHAR(50)   NULL,
    authenticity_profile     VARCHAR(50)   NULL,
    originality_profile      VARCHAR(50)   NULL,
    restoration_level        VARCHAR(50)   NULL,
    documentation_level      VARCHAR(50)   NULL,

    overall_condition_score  DECIMAL(5,2)  NULL,
    has_red_flags            BOOLEAN       NOT NULL DEFAULT FALSE,

    ownership_notes       TEXT          NULL,
    notes                    TEXT          NULL,

    row_version              BIGINT        NOT NULL DEFAULT 1,

    created_at_utc           DATETIME(6)   NOT NULL,
    updated_at_utc           DATETIME(6)   NOT NULL,

    CONSTRAINT pk_vehicles
        PRIMARY KEY (vehicle_id),

    CONSTRAINT fk_vehicles_vehicle_type
        FOREIGN KEY (vehicle_type_id)
        REFERENCES vehicle_types(vehicle_type_id)
);
```

This represents the target design.

CREATE TABLE `vehicles` (
  `vehicle_id` char(36) NOT NULL,
  `vehicle_type_id` char(36) DEFAULT NULL,
  `vin` varchar(50) DEFAULT NULL,
  `chassis_number` varchar(64) DEFAULT NULL,
  `body_number` varchar(64) DEFAULT NULL,
  `engine_number` varchar(64) DEFAULT NULL,
  `year` int(11) NOT NULL,
  `make` varchar(100) NOT NULL COMMENT 'Manufacturer or commonly recognized manufacturer designation.',
  `marque` varchar(100) DEFAULT NULL COMMENT 'Optional historically or commercially relevant marque when distinct from corporate manufacturer naming.',
  `model` varchar(100) NOT NULL COMMENT 'Primary model designation.',
  `trim` varchar(100) DEFAULT NULL COMMENT 'Factory trim or configuration designation.',
  `series_name` varchar(100) DEFAULT NULL COMMENT 'Series, generation, platform, model family, or subseries.',
  `body_style` varchar(100) DEFAULT NULL COMMENT 'Coupe, convertible, roadster, sedan, wagon, pickup, etc.',
  `drivetrain` varchar(100) DEFAULT NULL,
  `engine` varchar(150) DEFAULT NULL,
  `transmission` varchar(150) DEFAULT NULL,
  `mileage` int(11) DEFAULT NULL,
  `odometer_unit` varchar(20) DEFAULT NULL,
  `odometer_status` varchar(30) DEFAULT NULL,
  `exterior_color` varchar(100) DEFAULT NULL,
  `interior_color` varchar(100) DEFAULT NULL,
  `title_status` varchar(50) DEFAULT NULL,
  `authenticity_profile` varchar(100) DEFAULT NULL,
  `originality_profile` varchar(100) DEFAULT NULL,
  `restoration_level` varchar(100) DEFAULT NULL,
  `documentation_level` varchar(100) DEFAULT NULL,
  `overall_condition_score` decimal(4,2) DEFAULT NULL,
  `has_red_flags` tinyint(1) NOT NULL DEFAULT 0,
  `notes` text DEFAULT NULL,
  `ownership_notes` text DEFAULT NULL,
  `created_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `updated_at_utc` datetime(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `vehicle_status` varchar(30) NOT NULL DEFAULT 'Active',
  `row_version` bigint(20) NOT NULL DEFAULT 1,
  PRIMARY KEY (`vehicle_id`),
  KEY `idx_vehicles_make` (`make`),
  KEY `idx_vehicles_year` (`year`),
  KEY `idx_vehicles_vehicle_type` (`vehicle_type_id`),
  KEY `idx_vehicles_vin` (`vin`),
  KEY `idx_vehicles_chassis_number` (`chassis_number`),
  CONSTRAINT `fk_vehicles_vehicle_type`
    FOREIGN KEY (`vehicle_type_id`)
    REFERENCES `vehicle_types` (`vehicle_type_id`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_unicode_ci;

---

## 28.1 Physical Schema Authority

The DDL in this section is the implementation baseline for the current `vehicles` table.

The following mappings are mandatory:

| Database column | Domain property | Database type |
|---|---|---|
| `vehicle_id` | `VehicleId` | `char(36)` |
| `vehicle_type_id` | `VehicleTypeId` | nullable `char(36)` |
| `mileage` | `Mileage` | nullable `int` |
| `odometer_status` | `OdometerStatus` | nullable `varchar(30)` |
| `ownership_notes` | `OwnershipNotes` | nullable `text` |
| `vehicle_status` | `VehicleStatus` | `varchar(30)` |
| `row_version` | `RowVersion` | `bigint` |

`ownership_summary` is not a physical column in the current schema and shall not be used by the domain, application, or persistence layers.

`Mileage` shall remain `int?` unless the database DDL is deliberately changed through a versioned migration.

`VehicleStatus` defaults to `Active`, and `RowVersion` defaults to `1` at the database level.

# 29. INDEX DESIGN

Recommended indexes:

```sql
CREATE INDEX idx_vehicles_vin
    ON vehicles(vin);

CREATE INDEX idx_vehicles_year_make_model
    ON vehicles(year, make, model);

CREATE INDEX idx_vehicles_make_model
    ON vehicles(make, model);

CREATE INDEX idx_vehicles_vehicle_type
    ON vehicles(vehicle_type_id);

CREATE INDEX idx_vehicles_chassis_number
    ON vehicles(chassis_number);

CREATE INDEX idx_vehicles_updated
    ON vehicles(updated_at_utc);
```

Optional search indexes may later include:

```text
series_name
trim
body_style
title_status
```

---

# 30. VIN UNIQUENESS POLICY

A blanket database-level unique VIN constraint is not recommended until business rules address:

- blank VINs;
- partial VINs;
- pre-1981 identifiers;
- duplicate legacy imports;
- VIN corrections;
- cloned/contested identity cases.

Instead, the application should perform duplicate detection.

Example:

```text
Entered VIN
    ↓
Normalize
    ↓
Search Existing Vehicles
    ↓
No Match ───────────→ Continue Create
    ↓
Potential Match
    ↓
Warn User
    ↓
Open Existing / Override With Permission
```

---

# 31. CONCURRENCY

Vehicle shall support optimistic concurrency.

Recommended:

```text
row_version BIGINT
```

Update logic:

```sql
UPDATE vehicles
SET
    ...,
    row_version = row_version + 1,
    updated_at_utc = @UpdatedAtUtc
WHERE vehicle_id = @VehicleId
  AND row_version = @ExpectedRowVersion;
```

If affected rows = `0`:

```text
VehicleConcurrencyException
```

API response:

```http
409 Conflict
```

Example error code:

```text
VEHICLE_CONCURRENCY_CONFLICT
```

---

# 32. AUDITABILITY

At minimum, Vehicle shall preserve:

```text
created_at_utc
updated_at_utc
row_version
```

Target architecture should additionally record material master-data changes in the Nexus audit subsystem.

Examples:

```text
VehicleCreated
VehicleIdentityChanged
VehicleClassificationChanged
VehicleConfigurationChanged
VehicleTitleStatusChanged
VehicleTypeChanged
VehicleMerged
VehicleArchived
```

Changes to a Vehicle shall never rewrite issued appraisal history.

---

# 33. VEHICLE CREATION

Recommended domain factory:

```csharp
public static Vehicle Create(
    int year,
    string make,
    string model,
    string? vin,
    Guid? vehicleTypeId,
    DateTime utcNow)
```

Required invariants:

```text
VehicleId != Guid.Empty
Year valid
Make not blank
Model not blank
CreatedAtUtc populated
UpdatedAtUtc populated
RowVersion initialized
```

Creation shall not require:

- an Appraisal;
- an Inspection;
- a VIN;
- a complete provenance record;
- a valuation;
- an owner;
- a client.

This allows vehicles to be established before full appraisal research is available.

---

# 34. VEHICLE UPDATE

Vehicle changes shall occur through explicit methods rather than unrestricted public setters.

Examples:

```csharp
UpdateIdentity(...)
UpdateClassification(...)
UpdateMechanicalConfiguration(...)
UpdateOdometer(...)
UpdateColors(...)
UpdateTitleStatus(...)
UpdateProfiles(...)
UpdateOwnershipNotes(...)
UpdateNotes(...)
ChangeVehicleType(...)
ApplyConditionSummary(...)
```

Each successful material mutation shall:

```text
validate
    ↓
change state
    ↓
increment RowVersion
    ↓
update UpdatedAtUtc
```

---

# 35. VEHICLE DELETE POLICY

Hard deletion should not be allowed once a Vehicle participates in appraisal history.

Recommended states:

```text
Active
Archived
Merged
Duplicate
```

If a duplicate Vehicle is identified:

```text
Duplicate Vehicle
      ↓
Review Relationships
      ↓
Move/associate permissible current records
      ↓
Retain audit reference
      ↓
Mark duplicate as Merged
```

Historical appraisal snapshots shall remain unchanged.

---

# 36. DOMAIN VALIDATION RULES

## VEH-DOM-001

`VehicleId` shall never be empty after creation.

## VEH-DOM-002

`Year` shall be a plausible positive model year.

## VEH-DOM-003

`Make` shall not be blank.

## VEH-DOM-004

`Model` shall not be blank.

## VEH-DOM-005

Mileage, when present, shall not be negative.

## VEH-DOM-006

`VehicleTypeId`, shall reference an active/valid Vehicle Type according to application-layer configuration rules.

## VEH-DOM-007

Changing current Vehicle data shall not mutate existing appraisal snapshots.

## VEH-DOM-008

A Vehicle shall not directly own a final opinion of value.

## VEH-DOM-009

A Vehicle shall not directly own appraisal intended use, intended users, property interest, or value definition.

## VEH-DOM-010

A master-data condition summary shall not override detailed inspection or appraisal analysis records.

---

# 37. APPLICATION LAYER DESIGN

Recommended feature layout:

```text
ONE_Classics_Nexus.Application/
└── Vehicles/
    ├── Commands/
    │   ├── CreateVehicle/
    │   ├── UpdateVehicle/
    │   ├── ArchiveVehicle/
    │   └── MergeVehicle/
    │
    ├── Queries/
    │   ├── GetVehicle/
    │   ├── ListVehicles/
    │   ├── SearchVehicles/
    │   └── FindPotentialDuplicates/
    │
    ├── Contracts/
    ├── DTOs/
    ├── Validation/
    └── Services/
```

The parent DDS already recommends `Application/Vehicles/` as a first-class feature area and requires application services—not controllers—to orchestrate domain behavior.

---

# 38. APPLICATION SERVICE

Recommended abstraction:

```csharp
public interface IVehicleService
{
    Task<VehicleDto> CreateAsync(
        CreateVehicleCommand command,
        CancellationToken cancellationToken);

    Task<VehicleDto?> GetAsync(
        Guid vehicleId,
        CancellationToken cancellationToken);

    Task<PagedResult<VehicleListItemDto>> ListAsync(
        ListVehiclesQuery query,
        CancellationToken cancellationToken);

    Task<VehicleDto> UpdateAsync(
        Guid vehicleId,
        UpdateVehicleCommand command,
        CancellationToken cancellationToken);

    Task ArchiveAsync(
        Guid vehicleId,
        long expectedRowVersion,
        CancellationToken cancellationToken);
}
```

Existing handlers may remain where already implemented.

The service facade may orchestrate them rather than requiring an immediate wholesale rewrite.

---

# 39. REPOSITORY INTERFACE

Recommended Domain/Application abstraction:

```csharp
public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken);

    Task<Vehicle?> GetByVinAsync(
        string normalizedVin,
        CancellationToken cancellationToken);

    Task<PagedResult<Vehicle>> ListAsync(
        VehicleFilter filter,
        CancellationToken cancellationToken);

    Task InsertAsync(
        Vehicle vehicle,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        Vehicle vehicle,
        long expectedRowVersion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Vehicle>> FindPotentialDuplicatesAsync(
        VehicleDuplicateCriteria criteria,
        CancellationToken cancellationToken);
}
```

---

# 40. INFRASTRUCTURE DESIGN

Implementation:

```text
ONE_Classics_Nexus.Infrastructure/
└── Vehicles/
    ├── VehicleRepository.cs
    ├── VehicleSql.cs
    └── VehicleRecordMapper.cs
```

Persistence shall continue using MariaDB-compatible infrastructure.

The current SRS identifies Dapper-backed MySQL/MariaDB persistence as the implementation baseline.

Repository responsibilities:

- SQL only;
- parameter binding;
- record/entity materialization;
- persistence concurrency enforcement;
- database exception translation where appropriate.

Repository shall not determine:

- appraisal readiness;
- appraisal subject conclusions;
- valuation results;
- Standard 7 completion;
- report readiness.

---

# 41. CREATE VEHICLE CONTRACT

Recommended API contract:

```csharp
public sealed record CreateVehicleRequest(
    Guid? VehicleTypeId,
    string? Vin,
    string? ChassisNumber,
    string? BodyNumber,
    string? EngineNumber,
    int Year,
    string Make,
    string? Marque,
    string Model,
    string? SeriesName,
    string? Trim,
    string? BodyStyle,
    string? Engine,
    string? Transmission,
    string? Drivetrain,
    int? Mileage,
    string? OdometerUnit,
    string? ExteriorColor,
    string? InteriorColor,
    string? TitleStatus,
    string? AuthenticityProfile,
    string? OriginalityProfile,
    string? RestorationLevel,
    string? DocumentationLevel,
    string? OwnershipNotes,
    string? Notes);
```

---

# 42. UPDATE VEHICLE CONTRACT

Recommended:

```csharp
public sealed record UpdateVehicleRequest(
    long RowVersion,
    Guid? VehicleTypeId,
    string? Vin,
    string? ChassisNumber,
    string? BodyNumber,
    string? EngineNumber,
    int Year,
    string Make,
    string? Marque,
    string Model,
    string? SeriesName,
    string? Trim,
    string? BodyStyle,
    string? Engine,
    string? Transmission,
    string? Drivetrain,
    int? Mileage,
    string? OdometerUnit,
    string? ExteriorColor,
    string? InteriorColor,
    string? TitleStatus,
    string? AuthenticityProfile,
    string? OriginalityProfile,
    string? RestorationLevel,
    string? DocumentationLevel,
    string? OwnershipNotes,
    string? Notes);
```

`overall_condition_score` and `has_red_flags` should not normally be directly editable through this general update contract.

They should be controlled through inspection-derived application behavior.

---

# 43. VEHICLE RESPONSE CONTRACT

```csharp
public sealed record VehicleResponse(
    Guid VehicleId,
    Guid? VehicleTypeId,

    string? Vin,
    string? ChassisNumber,
    string? BodyNumber,
    string? EngineNumber,

    int Year,
    string Make,
    string? Marque,
    string Model,
    string? SeriesName,
    string? Trim,
    string? BodyStyle,

    string? Engine,
    string? Transmission,
    string? Drivetrain,

    int? Mileage,
    string? OdometerUnit,

    string? ExteriorColor,
    string? InteriorColor,

    string? TitleStatus,
    string? AuthenticityProfile,
    string? OriginalityProfile,
    string? RestorationLevel,
    string? DocumentationLevel,

    decimal? OverallConditionScore,
    bool HasRedFlags,

    string? OwnershipNotes,
    string? Notes,

    long RowVersion,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
```

---

# 44. REST API

Recommended endpoints:

```http
POST   /api/vehicles
GET    /api/vehicles/{vehicleId}
GET    /api/vehicles
PUT    /api/vehicles/{vehicleId}
```

These preserve the currently implemented Vehicle requirements.

Target extensions:

```http
GET    /api/vehicles/search
GET    /api/vehicles/{vehicleId}/appraisals
GET    /api/vehicles/{vehicleId}/inspections
GET    /api/vehicles/{vehicleId}/history
POST   /api/vehicles/{vehicleId}/archive
POST   /api/vehicles/{vehicleId}/merge
```

---

# 45. HTTP RESPONSE SEMANTICS

Recommended:

| Operation | Response |
|---|---|
| Create | `201 Created` |
| Get existing | `200 OK` |
| List | `200 OK` |
| Update | `200 OK` |
| Missing Vehicle | `404 Not Found` |
| Validation failure | `400 Bad Request` |
| Business rule failure | `422 Unprocessable Entity` |
| Concurrency conflict | `409 Conflict` |
| Duplicate warning requiring intervention | `409 Conflict` or explicit domain result |

---

# 46. VEHICLE LIST QUERY

Recommended filtering:

```text
search
vin
yearFrom
yearTo
make
model
series
vehicleTypeId
bodyStyle
titleStatus
hasRedFlags
page
pageSize
sortBy
sortDirection
```

Example:

```http
GET /api/vehicles?make=Chevrolet&model=Camaro&yearFrom=1967&yearTo=2002&page=1&pageSize=50
```

---

# 47. SEARCH DESIGN

Search shall support partial matching against:

```text
VIN
chassis number
year
make
marque
model
series
trim
body style
```

Default search result:

```text
VehicleId
Year
Make
Model
Series
Trim
VIN/Primary Identifier
Vehicle Type
Current Condition Summary
Title Status
Updated Date
```

---

# 48. DUPLICATE DETECTION

Potential duplicate indicators:

```text
Exact normalized VIN
Exact chassis number
Exact body number
Year + Make + Model + VIN suffix
Year + Make + Model + chassis number
```

Duplicate detection shall produce candidates rather than automatically merge records.

Example result:

```csharp
VehicleDuplicateCandidate
{
    Guid VehicleId;
    string MatchReason;
    decimal MatchConfidence;
}
```

Human review is required before merge.

---

# 49. APPRAISAL CREATION INTEGRATION

Existing Nexus requirements establish that an Appraisal is created against a Vehicle.

Flow:

```text
Select/Create Vehicle
        ↓
Create Appraisal
        ↓
Appraisal.VehicleId = Vehicle.VehicleId
        ↓
Populate Draft Subject Snapshot
        ↓
Appraiser Reviews Subject Information
        ↓
Approve Subject Snapshot
```

Creation of an Appraisal shall not automatically imply that every Vehicle master field has been verified.

---

# 50. SUBJECT SNAPSHOT GENERATION

Recommended service:

```csharp
public interface IAppraisalSubjectSnapshotService
{
    Task<AppraisalSubjectSnapshot>
        CreateFromVehicleAsync(
            Guid appraisalId,
            Guid vehicleId,
            CancellationToken cancellationToken);

    Task<AppraisalSubjectSnapshot>
        RefreshDraftFromVehicleAsync(
            Guid appraisalId,
            CancellationToken cancellationToken);

    Task ApproveAsync(
        Guid appraisalId,
        Guid userId,
        CancellationToken cancellationToken);
}
```

Snapshot generation should copy relevant fields such as:

```text
VIN
chassis number
body number
engine number
year
make
marque
model
series
trim
body style
engine
transmission
drivetrain
mileage
odometer unit
colors
title status
authenticity profile
originality profile
restoration level
documentation level
ownership/provenance summary
```

---

# 51. SNAPSHOT MUTABILITY RULE

Recommended lifecycle:

```text
Draft Subject Snapshot
        ↓
Review / Research / Correction
        ↓
Approved Subject Snapshot
        ↓
Development Complete
        ↓
IMMUTABLE
```

Once the appraisal reaches Development Complete:

```text
Vehicle changes
        ≠
Historical Subject Snapshot changes
```

A later correction to an issued appraisal shall follow formal revision/reopening/supersession procedures.

The parent DDS requires historical accuracy to take precedence over current master data.

---

# 52. VEHICLE → APPRAISAL DATA PROPAGATION

Propagation shall always be explicit.

Permitted:

```text
Vehicle Master
     ↓ explicit copy
Draft Appraisal Snapshot
```

Prohibited:

```text
Vehicle Master
     ↓ automatic live binding
Issued Appraisal Snapshot
```

Also prohibited:

```text
Vehicle Update
     ↓
Silent update of appraisal analysis
     ↓
Silent recalculation
     ↓
Silent report modification
```

---

# 53. APPRAISAL → VEHICLE PROPAGATION

Appraisal research may discover improved master information.

Example:

```text
Appraisal Research
      ↓
VIN corrected
Engine number established
Factory color established
      ↓
User chooses "Update Vehicle Master"
      ↓
Vehicle update transaction
```

This shall be explicit.

An appraisal shall not silently rewrite Vehicle master data merely because an appraiser entered different information into a snapshot.

---

# 54. INSPECTION INTEGRATION

Relationship:

```text
Vehicle
  1
  │
  └──── 0..* Appraisal
              │
              └──── 0..* Inspection
```

An Inspection shall remain appraisal-associated for professional appraisal workflow.

Vehicle-centric UI may display inspection history by traversing the appraisal relationships.

---

# 55. COMPARABLE RELATIONSHIP

`ComparableSale` shall remain appraisal evidence rather than becoming a child of Vehicle.

Correct:

```text
Vehicle → Appraisal → ComparableSale
```

Not:

```text
Vehicle → ComparableSale
```

This prevents comparable selection from becoming a permanent characteristic of the physical asset.

---

# 56. VALUATION RELATIONSHIP

Likewise:

```text
Vehicle
   ↓
Appraisal
   ↓
ValuationRun
```

A `ValuationRun` represents an appraisal analytical result.

The parent DDS explicitly distinguishes an AVM analytical indication from the approved professional final value conclusion.

Therefore Vehicle shall have no authoritative:

```text
CurrentAppraisedValue
FinalValue
USPAPValue
```

field.

Any dashboard convenience value must reference a specific appraisal or valuation run.

---

# 57. WINFORMS VEHICLE MODULE

The Nexus DDS identifies `Vehicles` as a primary WinForms module within the existing shell.

Recommended views:

```text
VehicleListForm
VehicleDetailForm
VehicleEditForm / edit panel
VehicleHistoryPanel
VehicleAppraisalsPanel
```

Primary brand color:

```text
#0E2841
```

---

# 58. VEHICLE LIST UI

Recommended columns:

```text
Year
Make
Model
Series / Trim
VIN / Chassis
Vehicle Type
Title Status
Condition
Red Flag
Last Updated
```

Toolbar:

```text
New Vehicle
Search
Filter
Refresh
Open
```

---

# 59. VEHICLE DETAIL UI

Recommended sections:

```text
┌─────────────────────────────────────────────────────┐
│ VEHICLE HEADER                                      │
│ 1969 Chevrolet Camaro Z/28                          │
│ VIN / Chassis                         Vehicle Type   │
├─────────────────────────────────────────────────────┤
│ Identification                                      │
│ Classification                                      │
│ Mechanical Configuration                            │
│ Odometer                                            │
│ Colors                                              │
│ Title / Status                                      │
│ Authenticity / Originality                          │
│ Restoration / Documentation                         │
│ Ownership Summary                                   │
│ Notes                                               │
├─────────────────────────────────────────────────────┤
│ Appraisals | Inspections | History                  │
└─────────────────────────────────────────────────────┘
```

---

# 60. VEHICLE DETAIL COMMANDS

Recommended commands:

```text
Edit
Save
Cancel
Create Appraisal
View Appraisals
View Inspection History
Archive
History
```

`Create Appraisal` shall:

1. identify the selected Vehicle;
2. open appraisal creation;
3. establish `VehicleId`;
4. initialize a draft subject snapshot;
5. transition control to Appraisal Workspace.

---

# 61. UI EDITABILITY

Vehicle master information remains editable subject to permissions.

Appraisal snapshots shall show a separate state:

```text
CURRENT VEHICLE MASTER
vs.
APPRAISAL SUBJECT SNAPSHOT
```

The UI shall never imply that changing Vehicle master data retroactively updates prior appraisal reports.

Recommended warning:

```text
Changes to the Vehicle master record do not modify
previously approved appraisal subject snapshots or
issued appraisal reports.
```

---

# 62. PERMISSIONS

Suggested role matrix:

| Operation | Administrator | Appraiser | Reviewer | Inspector | ReadOnly |
|---|---:|---:|---:|---:|---:|
| View Vehicle | ✓ | ✓ | ✓ | ✓ | ✓ |
| Create Vehicle | ✓ | ✓ | Conditional | Conditional | — |
| Edit Vehicle | ✓ | ✓ | Conditional | Conditional | — |
| Archive Vehicle | ✓ | Conditional | — | — | — |
| Merge Vehicle | ✓ | Conditional | — | — | — |
| Create Appraisal | ✓ | ✓ | Conditional | — | — |
| View History | ✓ | ✓ | ✓ | ✓ | ✓ |

Exact authorization policy belongs to the Security design and may be tightened independently.

---

# 63. VALIDATION STRATEGY

Validation occurs in layers:

```text
API Contract Validation
        ↓
Application Validation
        ↓
Domain Invariants
        ↓
Repository / Database Constraints
```

UI validation improves usability but shall not be the authoritative validation layer.

---

# 64. API VALIDATION EXAMPLES

### Invalid year

```json
{
  "code": "VEHICLE_YEAR_INVALID",
  "message": "Vehicle year is outside the permitted range."
}
```

### Missing make

```json
{
  "code": "VEHICLE_MAKE_REQUIRED",
  "message": "Vehicle make is required."
}
```

### Missing model

```json
{
  "code": "VEHICLE_MODEL_REQUIRED",
  "message": "Vehicle model is required."
}
```

### Negative mileage

```json
{
  "code": "VEHICLE_MILEAGE_INVALID",
  "message": "Mileage cannot be negative."
}
```

---

# 65. ERROR CODES

Recommended controlled codes:

```text
VEHICLE_NOT_FOUND
VEHICLE_VALIDATION_FAILED
VEHICLE_YEAR_INVALID
VEHICLE_MAKE_REQUIRED
VEHICLE_MODEL_REQUIRED
VEHICLE_MILEAGE_INVALID
VEHICLE_TYPE_INVALID
VEHICLE_DUPLICATE_SUSPECTED
VEHICLE_CONCURRENCY_CONFLICT
VEHICLE_ARCHIVE_BLOCKED
VEHICLE_MERGE_BLOCKED
```

---

# 66. STANDARD 7 TRACEABILITY

| Standard 7 Area | Vehicle Responsibility | Appraisal Responsibility |
|---|---|---|
| Subject identification | Current identity master | Approved snapshot |
| Relevant characteristics | Current/preliminary attributes | Reviewed appraisal facts |
| Condition | Current summary | Inspection + analysis |
| Authenticity | Current profile | Evidence-backed conclusion |
| Originality | Current profile | Evidence-backed conclusion |
| Restoration | Current profile/history | Value-relevant analysis |
| Documentation | Current summary | Evidence and reliance |
| Provenance | Current summary | Detailed provenance analysis |
| Property interest | None | Required |
| Value definition | None | Required |
| Effective date | None | Required |
| Intended use | None | Required |
| Final value | None | Approved opinion |

This separation supports the USPAP requirement to maintain traceability from what was identified through what was relied upon, analyzed, calculated, reconciled, concluded, reported, and certified.

---

# 67. STANDARD 8 RELATIONSHIP

The Vehicle entity has no direct Standard 8 reporting lifecycle responsibility.

Standard 8 reporting consumes:

```text
Approved Appraisal
       ↓
Approved Subject Snapshot
       ↓
Approved Analysis
       ↓
Approved Final Opinion
       ↓
Report Composition
```

The current mutable Vehicle master shall not be queried as the sole source when reproducing an issued appraisal report.

This prevents:

```text
Vehicle changed today
       ↓
Historical report regenerated tomorrow
       ↓
Different subject facts
```

---

# 68. TEST DESIGN

## 68.1 Domain Tests

Required:

```text
Vehicle_Create_ValidData_Succeeds
Vehicle_Create_EmptyMake_Fails
Vehicle_Create_EmptyModel_Fails
Vehicle_Create_NegativeMileage_Fails

Vehicle_UpdateIdentity_UpdatesRowVersion
Vehicle_UpdateConfiguration_UpdatesTimestamp
Vehicle_UpdateMileage_RejectsNegativeValue

Vehicle_ChangeVehicleType_SucceedsForValidType
Vehicle_ApplyConditionSummary_UpdatesSummary

Vehicle_MasterChange_DoesNotMutateSnapshot
```

---

# 69. APPLICATION TESTS

```text
CreateVehicleHandler_ValidRequest_ReturnsVehicle
CreateVehicleHandler_DuplicateVin_ReturnsCandidateWarning

GetVehicleHandler_ExistingVehicle_ReturnsVehicle
GetVehicleHandler_MissingVehicle_ReturnsNotFound

ListVehiclesHandler_FilterByMake_ReturnsMatches
ListVehiclesHandler_FilterByYear_ReturnsMatches
ListVehiclesHandler_PaginatesCorrectly

UpdateVehicleHandler_ValidVersion_Succeeds
UpdateVehicleHandler_StaleVersion_ReturnsConflict
```

---

# 70. REPOSITORY TESTS

```text
VehicleRepository_Insert_PersistsAllFields
VehicleRepository_GetById_MapsAllFields
VehicleRepository_GetByVin_ReturnsMatch
VehicleRepository_Update_IncrementsRowVersion
VehicleRepository_Update_StaleVersion_ReturnsFalse

VehicleRepository_List_AppliesPaging
VehicleRepository_List_AppliesFilters
VehicleRepository_NullOptionalFields_RoundTrip
```

---

# 71. API INTEGRATION TESTS

```text
POST_Vehicle_Valid_Returns201
POST_Vehicle_Invalid_Returns400

GET_Vehicle_Existing_Returns200
GET_Vehicle_Missing_Returns404

GET_Vehicles_ReturnsPagedResults
GET_Vehicles_FilterByMake_Works
GET_Vehicles_FilterByModel_Works

PUT_Vehicle_Valid_Returns200
PUT_Vehicle_StaleRowVersion_Returns409
PUT_Vehicle_Missing_Returns404
```

The parent DDS explicitly identifies Vehicle API testing as a minimum integration suite.

---

# 72. APPRAISAL INTEGRATION TESTS

Critical historical-integrity tests:

```text
CreateAppraisal_FromVehicle_CreatesSubjectSnapshot

VehicleUpdate_AfterSnapshotApproval_DoesNotChangeSnapshot

VehicleUpdate_AfterDevelopmentComplete_DoesNotChangeSnapshot

VehicleUpdate_AfterReportIssued_DoesNotChangeIssuedReport

SnapshotRefresh_DraftAppraisal_RequiresExplicitAction

SnapshotRefresh_ApprovedSnapshot_IsBlockedOrVersioned
```

These tests are mandatory for appraisal-centric integrity.

---

# 73. DATABASE MIGRATION STRATEGY

Implementation shall be incremental.

### Phase A — Reconcile Existing Vehicle Schema

Confirm:

```text
vehicle_id
vin
year
make
model
trim
series_name
body_style
drivetrain
engine
transmission
mileage
odometer_unit
exterior_color
interior_color
title_status
authenticity_profile
restoration_level
documentation_level
overall_condition_score
has_red_flags
ownership_notes
notes
created_at_utc
updated_at_utc
```

### Phase B — Add Target Fields Where Missing

Potential additions:

```text
vehicle_type_id
chassis_number
body_number
engine_number
marque
originality_profile
row_version
```

### Phase C — Add Indexes and Constraints

### Phase D — Update Repository Mapping

### Phase E — Expand API Contracts

### Phase F — Update WinForms Vehicle Detail

### Phase G — Add Appraisal Subject Snapshot Integration

No destructive migration should be performed until existing records have been profiled.

---

# 74. BACKWARD COMPATIBILITY

Existing Vehicle functions shall remain operational:

```text
POST /api/vehicles
GET /api/vehicles/{id}
GET /api/vehicles
PUT /api/vehicles/{id}
```

New optional attributes should initially be additive.

Existing records with null values shall remain readable.

---

# 75. PERFORMANCE REQUIREMENTS

Vehicle retrieval should require a single indexed master-record query.

Paged list queries shall:

- never load the entire Vehicle table;
- project list-only columns;
- use deterministic ordering;
- support indexed common filters.

Appraisal detail should retrieve the historical subject snapshot rather than repeatedly resolving mutable Vehicle state.

---

# 76. DATA INTEGRITY RULES

The following rules are non-negotiable:

```text
1. VehicleId is permanent.

2. Vehicle and AppraisalSubjectSnapshot are distinct records.

3. A Vehicle master change cannot alter an issued appraisal.

4. VIN is an identifier, not the database primary key.

5. Appraisal value does not belong on Vehicle as an authoritative field.

6. Inspection scores do not belong directly on Vehicle.

7. Vehicle condition summaries are denormalized convenience values only.

8. Authenticity/originality profiles do not substitute for appraisal analysis.

9. Vehicle type configuration shall not alter canonical system identities.

10. Historical appraisal accuracy overrides current Vehicle master data.
```

---

# 77. ENTITY RELATIONSHIP MODEL

```text
┌──────────────────────┐
│    vehicle_types     │
├──────────────────────┤
│ PK vehicle_type_id   │
│    code              │
│    name              │
│    is_active         │
└──────────┬───────────┘
           │ 1
           │
           │ 0..*
┌──────────▼───────────┐
│       vehicles       │
├──────────────────────┤
│ PK vehicle_id        │
│ FK vehicle_type_id   │
│    vin               │
│    chassis_number    │
│    year              │
│    make              │
│    model             │
│    ...               │
│    row_version       │
└──────────┬───────────┘
           │ 1
           │
           │ 0..*
┌──────────▼───────────┐
│      appraisals      │
├──────────────────────┤
│ PK appraisal_id      │
│ FK vehicle_id        │
│    status            │
│    effective_date    │
└──────────┬───────────┘
           │ 1
           │
           │ 1..*
┌──────────▼────────────────────┐
│ appraisal_subject_snapshots  │
├───────────────────────────────┤
│ PK subject_snapshot_id        │
│ FK appraisal_id               │
│    vehicle identity copy      │
│    configuration copy         │
│    relevant characteristics  │
│    approved/locked state      │
└───────────────────────────────┘
```

Supporting relationships:

```text
VehicleType
    │
    └── VehicleTypeSystemWeight

Vehicle
    │
    └── Appraisal
          ├── AppraisalSubjectSnapshot
          ├── Inspection
          ├── Evidence
          ├── SubjectAnalysis
          ├── ComparableSale
          ├── ValuationRun
          ├── Reconciliation
          ├── FinalValueOpinion
          └── AppraisalReport
```

---

# 78. COMPONENT INTERACTION

```text
WinForms Vehicle UI
        │
        ↓
Vehicle API Client
        │
        ↓ HTTP
VehiclesController
        │
        ↓
Application Vehicle Service / Handler
        │
        ├────→ Domain Vehicle
        │
        ↓
IVehicleRepository
        │
        ↓
VehicleRepository / Dapper
        │
        ↓
MariaDB vehicles
```

Appraisal creation extension:

```text
VehicleDetailForm
      │
      └── Create Appraisal
              ↓
        Appraisal Application Service
              ↓
        Create Appraisal
              ↓
        Create Draft Subject Snapshot
              ↓
        AppraisalWorkspace
```

---

# 79. SEQUENCE — CREATE VEHICLE

```text
User
 │
 ↓
VehicleForm
 │ Create
 ↓
VehicleApiClient
 │ POST /api/vehicles
 ↓
VehiclesController
 │
 ↓
CreateVehicleHandler
 │
 ├── Validate
 │
 ├── Check duplicate candidates
 │
 ├── Vehicle.Create()
 │
 ↓
VehicleRepository
 │ INSERT
 ↓
MariaDB
 │
 ↑
VehicleResponse
```

---

# 80. SEQUENCE — UPDATE VEHICLE

```text
User
 │
 ↓
VehicleDetailForm
 │ Save
 ↓
VehicleApiClient
 │ PUT /api/vehicles/{id}
 ↓
VehiclesController
 ↓
UpdateVehicleHandler
 │
 ├── Load Vehicle
 ├── Validate RowVersion
 ├── Apply domain changes
 └── Persist
 ↓
VehicleRepository
 │
 ├── UPDATE ... WHERE row_version = expected
 └── row_version + 1
 ↓
MariaDB
```

---

# 81. SEQUENCE — CREATE APPRAISAL FROM VEHICLE

```text
User
 │
 ↓
VehicleDetailForm
 │ Create Appraisal
 ↓
AppraisalApiClient
 ↓
AppraisalController
 ↓
CreateAppraisalHandler
 │
 ├── Verify Vehicle
 ├── Create Appraisal
 └── Persist
 ↓
SubjectSnapshotService
 │
 ├── Read current Vehicle
 ├── Copy relevant attributes
 └── Create Draft Snapshot
 ↓
AppraisalWorkspace
```

---

# 82. CORE DESIGN DECISIONS

## VEH-DEC-001 — Vehicle Is a Reusable Master Asset

The Vehicle represents the current known physical asset and may participate in multiple appraisals.

## VEH-DEC-002 — Vehicle Is Not the Historical Appraisal Subject

Historical subject state shall be preserved by `AppraisalSubjectSnapshot`.

## VEH-DEC-003 — Vehicle Is Mutable

The master record may evolve as additional information becomes known.

## VEH-DEC-004 — Approved Appraisal Snapshots Are Historically Controlled

Master changes shall not mutate completed appraisal subject records.

## VEH-DEC-005 — Vehicle Has No Authoritative Appraised Value

Appraised value belongs to the Appraisal and approved `FinalValueOpinion`.

## VEH-DEC-006 — Inspection Detail Does Not Belong to Vehicle

Inspection data belongs to appraisal-associated Inspection records.

## VEH-DEC-007 — Denormalized Condition Data Is Non-Authoritative

`overall_condition_score` and `has_red_flags` are current convenience summaries only.

## VEH-DEC-008 — VIN Is Not the Primary Key

`VehicleId` provides stable system identity across historical and unusual vehicles.

## VEH-DEC-009 — Vehicle Type Is Configuration Context

Vehicle type influences applicable inspection/ruleset configuration without changing the canonical twelve system identities.

## VEH-DEC-010 — Appraisal Propagation Is Explicit

Data movement between Vehicle and appraisal snapshot shall never occur silently.

## VEH-DEC-011 — Historical Accuracy Overrides Convenience

Issued appraisals must reproduce the data relied upon at issuance even where current Vehicle data subsequently changes.

## VEH-DEC-012 — Appraisal Analysis Remains Appraisal-Specific

Authenticity, originality, provenance, condition, rarity, and collector desirability conclusions remain evidence-backed appraisal analyses rather than mutable Vehicle master conclusions.

---

# 83. REQUIREMENTS TRACEABILITY

| DDS Requirement | Source |
|---|---|
| Vehicle CRUD | `FR-VEH-001`–`FR-VEH-004` |
| Vehicle linked to Appraisal | `FR-APR-001` |
| Collector vehicle identification | `S7-REQ-004` / `FR-USPAP-008` |
| Relevant property characteristics | `S7-REQ-005` / `FR-USPAP-009`, `010` |
| Historical subject snapshot | `S7-REQ-005` |
| Separate property interest | `S7-REQ-006` |
| Inspection evidence | `S7-REQ-014` |
| Condition analysis | `S7-REQ-017` |
| Authenticity/originality | `S7-REQ-018` |
| Provenance | `S7-REQ-019` |
| Rarity/desirability | `S7-REQ-020` |
| Historical integrity | Nexus DDS `DDS-DEC-010` |
| Vehicle/snapshot separation | Nexus DDS `DDS-DEC-002` |

---

# 84. IMPLEMENTATION TARGET

Recommended target file organization:

```text
ONE_Classics_Nexus.Domain/
└── Vehicles/
    ├── Vehicle.cs
    ├── VehicleErrors.cs
    ├── VehicleStatus.cs
    └── ValueObjects/
        ├── VehicleIdentity.cs
        └── VehicleOdometer.cs

ONE_Classics_Nexus.Application/
└── Vehicles/
    ├── Commands/
    ├── Queries/
    ├── DTOs/
    ├── Contracts/
    ├── Validation/
    └── IVehicleRepository.cs

ONE_Classics_Nexus.Infrastructure/
└── Vehicles/
    ├── VehicleRepository.cs
    ├── VehicleSql.cs
    └── VehicleRecordMapper.cs

ONE_Classics_Nexus.API/
├── Controllers/
│   └── VehiclesController.cs
└── Contracts/
    └── Vehicles/
        ├── CreateVehicleRequest.cs
        ├── UpdateVehicleRequest.cs
        └── VehicleResponse.cs

ONE_Classics_Nexus.WinForms/
├── Forms/
│   └── Vehicles/
│       ├── VehicleListForm.cs
│       └── VehicleDetailForm.cs
├── Services/
│   └── VehicleApiClient.cs
└── Models/
    └── VehicleDto.cs

ONE_Classics_Nexus.Tests/
└── Vehicles/
    ├── Domain/
    ├── Application/
    ├── Infrastructure/
    └── API/
```

This extends rather than replaces the existing solution structure.

---

# 85. DEFINITION OF DONE

The Vehicle entity implementation is complete when:

- [ ] Vehicle can be created.
- [ ] Vehicle can be retrieved by ID.
- [ ] Vehicle list supports paging and filtering.
- [ ] Vehicle can be updated.
- [ ] Vehicle uses optimistic concurrency.
- [ ] Vehicle type can be associated without altering canonical inspection-system identities.
- [ ] Collector-vehicle identifiers required by the target subject model are supported.
- [ ] Current authenticity/originality/restoration/documentation summaries are supported.
- [ ] Vehicle master condition summaries are explicitly non-authoritative.
- [ ] Duplicate identity detection exists.
- [ ] Vehicle cannot directly own a final appraisal value.
- [ ] Vehicle can participate in multiple Appraisals.
- [ ] Creating an Appraisal can generate a draft `AppraisalSubjectSnapshot`.
- [ ] Approved snapshots are independent of subsequent Vehicle changes.
- [ ] Issued appraisal history cannot be altered by Vehicle master updates.
- [ ] Domain tests pass.
- [ ] Repository tests pass.
- [ ] API integration tests pass.
- [ ] appraisal/snapshot historical-integrity tests pass.
- [ ] WinForms Vehicle module supports search, create, view, edit, and Create Appraisal.
- [ ] requirements-to-code traceability is updated.

---

# 86. FINAL ARCHITECTURAL RULE

The governing Vehicle rule for ONE Classics Nexus shall be:

```text
VEHICLE
Current reusable physical-asset record
            │
            │ referenced by
            ↓
APPRAISAL
Professional assignment and workflow
            │
            │ freezes
            ↓
APPRAISAL SUBJECT SNAPSHOT
Property state relied upon in that assignment
            │
            │ supports
            ↓
EVIDENCE + ANALYSIS + VALUATION
            │
            ↓
FINAL VALUE OPINION
            │
            ↓
REPORT / CERTIFICATION / ISSUANCE
```

Or stated more simply:

> **The Vehicle tells Nexus what the asset is currently understood to be. The Appraisal Subject Snapshot records what the appraiser relied upon for a specific assignment. The Appraisal records what the appraiser analyzed and concluded. The issued report records what was communicated.**

That separation is the foundation for preserving both reusable vehicle master data and defensible appraisal history within the ONE Classics Nexus appraisal-centric architecture.