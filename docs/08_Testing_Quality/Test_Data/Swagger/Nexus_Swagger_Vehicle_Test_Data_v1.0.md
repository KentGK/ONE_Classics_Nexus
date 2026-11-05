# ONE Classics Nexus  
## Swagger Test Data  
**Version:** 1.0  
**Document Owner:** QA / BA / TA  
**Target Folder:** `/docs/08_Testing_Quality/05_Test_Data/`  
**Recommended File Name:** `Swagger_Test_Data_v1.0.md`  
**System:** `ONE_Classics_Nexus`  
**Created:** 2026-06-09  

---

## 1. Purpose

This document provides comprehensive Swagger-ready test data for validating the Nexus API through the Swagger UI.

The data is organized around the locked Nexus workflow:

```text
Vehicle → Appraisal → Inspection → Condition Scores
→ Comparable Sales → Valuation Run → Report
```

Use this document to test:

- Create, read, and update API behavior
- Required field validation
- 12-system condition scoring
- Vehicle-type-specific score weights
- Comparable sale creation
- Valuation run preconditions
- Negative and boundary validation cases
- Regression readiness before phase promotion

---

## 2. Swagger Test Assumptions

| Item | Value |
|---|---|
| Primary Swagger URL | `https://localhost:5001/swagger` |
| Alternate API URL | `http://localhost:5000` |
| JSON casing | `camelCase` |
| Primary subject vehicle | `1970 Chevrolet K10 Short Bed 4x4` |
| Primary inspection type | `VintageClassic` |
| Primary valuation style | Inspection-driven deterministic valuation |
| ID format | GUID / UUID returned by API |

> If Swagger shows PascalCase properties, use the same values but match the casing displayed by Swagger.

---

## 3. Test Execution Order

Run these in order so each response supplies IDs for the next request.

| Step | Method | Endpoint | Save From Response |
|---:|---|---|---|
| 1 | POST | `/api/vehicles` | `vehicleId` |
| 2 | GET | `/api/vehicles/{vehicleId}` | Confirm vehicle |
| 3 | PUT | `/api/vehicles/{vehicleId}` | Confirm update |
| 4 | POST | `/api/appraisals` | `appraisalId` |
| 5 | GET | `/api/appraisals/{appraisalId}` | Confirm appraisal |
| 6 | POST | `/api/inspections` | `inspectionId` |
| 7 | PUT | `/api/inspections/{inspectionId}/scores` | Confirm scores saved |
| 8 | GET | `/api/inspections/{inspectionId}` | Confirm 12 scores |
| 9 | POST | `/api/appraisals/{appraisalId}/comparables` | `comparableId` values |
| 10 | GET | `/api/appraisals/{appraisalId}/comparables` | Confirm comps |
| 11 | POST | `/api/appraisals/{appraisalId}/valuation/run` | `valuationRunId` |
| 12 | GET | `/api/appraisals/{appraisalId}/valuation/runs` | Confirm valuation |
| 13 | POST | `/api/appraisals/{appraisalId}/report` | Confirm report request |

---

## 4. Runtime Variables

Replace these placeholders with IDs returned from earlier Swagger responses.

```json
{
  "vehicleId": "{{vehicleId}}",
  "appraisalId": "{{appraisalId}}",
  "inspectionId": "{{inspectionId}}",
  "comparableId": "{{comparableId}}",
  "valuationRunId": "{{valuationRunId}}"
}
```

---

# 5. Vehicle Test Data

## 5.1 POST `/api/vehicles` — Primary Subject Vehicle

Use this as the safest payload for the current Vehicle API DTO.

```json
{
  "vin": "CE140Z100001",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "6.0L LS V8",
  "transmission": "4L80E Automatic",
  "drivetrain": "4x4",
  "mileage": 1250
}
```

### Expected Result

| Field | Expected |
|---|---|
| HTTP status | `200 OK` or `201 Created` |
| Response contains | `vehicleId` |
| VIN | `CE140Z100001` |
| Make / Model | `Chevrolet K10` |

---

## 5.2 GET `/api/vehicles/{vehicleId}`

No request body.

### Expected Result

The API should return the vehicle created in section 5.1.

---

## 5.3 PUT `/api/vehicles/{vehicleId}`

```json
{
  "vin": "CE140Z100001",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom Deluxe",
  "bodyStyle": "Short Bed Pickup",
  "engine": "6.0L LS V8",
  "transmission": "4L80E Automatic",
  "drivetrain": "4x4",
  "mileage": 1325
}
```

### Expected Result

| Field | Expected |
|---|---|
| HTTP status | `200 OK`, `204 No Content`, or API-defined success response |
| Trim | `Custom Deluxe` |
| Mileage | `1325` |

---

## 5.4 Optional Extended Vehicle Payload

Use this only if the API contract exposes the full repository/schema-level vehicle fields.

```json
{
  "vin": "CE140Z100001",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "6.0L LS V8",
  "transmission": "4L80E Automatic",
  "drivetrain": "4x4",
  "mileage": 1250,
  "seriesName": "C/K 10 Series",
  "odometerUnit": "Miles",
  "exteriorColor": "Olive Green / White",
  "interiorColor": "Black",
  "titleStatus": "Clean",
  "authenticityProfile": "Restomod",
  "restorationLevel": "Frame-Off Restoration",
  "documentationLevel": "High",
  "overallConditionScore": 4.35,
  "hasRedFlags": false,
  "OwnershipNotes": "Owner-built restoration with modern drivetrain and documented upgrades.",
  "notes": "Factory 4x4 short-wheelbase K-code truck with air conditioning."
}
```

---

## 5.5 Additional Vehicle Type Seeds

### Standard Vehicle

```json
{
  "vin": "1G1YY22G0W5123456",
  "year": 1998,
  "make": "Chevrolet",
  "model": "Corvette",
  "trim": "Base",
  "bodyStyle": "Coupe",
  "engine": "5.7L LS1 V8",
  "transmission": "6-Speed Manual",
  "drivetrain": "RWD",
  "mileage": 48250
}
```

### Custom Vehicle

```json
{
  "vin": "124379N500001",
  "year": 1969,
  "make": "Chevrolet",
  "model": "Camaro",
  "trim": "Pro Touring",
  "bodyStyle": "Coupe",
  "engine": "LS3 V8",
  "transmission": "Tremec 6-Speed Manual",
  "drivetrain": "RWD",
  "mileage": 2800
}
```

### Vintage Vehicle

```json
{
  "vin": "A1929ID00001",
  "year": 1929,
  "make": "Ford",
  "model": "Model A",
  "trim": "Tudor Sedan",
  "bodyStyle": "Tudor Sedan",
  "engine": "201 CID Inline-4",
  "transmission": "3-Speed Manual",
  "drivetrain": "RWD",
  "mileage": 12000
}
```

---

# 6. Appraisal Test Data

## 6.1 POST `/api/appraisals`

```json
{
  "vehicleId": "{{vehicleId}}"
}
```

### Expected Result

| Field | Expected |
|---|---|
| HTTP status | `200 OK` or `201 Created` |
| Response contains | `appraisalId` |
| Vehicle relationship | Appraisal references `vehicleId` |

---

## 6.2 GET `/api/appraisals/{appraisalId}`

No request body.

---

## 6.3 Optional PUT `/api/appraisals/{appraisalId}`

Use only if the current API exposes appraisal update.

```json
{
  "vehicleId": "{{vehicleId}}",
  "status": "InProgress",
  "appraisalNumber": "NEX-2026-0001"
}
```

---

# 7. Inspection Test Data

## 7.1 POST `/api/inspections` — Vintage/Classic Inspection

```json
{
  "appraisalId": "{{appraisalId}}",
  "inspectionType": "VintageClassic"
}
```

### Expected Result

| Field | Expected |
|---|---|
| HTTP status | `200 OK` or `201 Created` |
| Response contains | `inspectionId` |
| Inspection type | `VintageClassic` |

---

## 7.2 POST `/api/inspections` — Standard Inspection

```json
{
  "appraisalId": "{{appraisalId}}",
  "inspectionType": "Standard"
}
```

---

## 7.3 POST `/api/inspections` — Custom Inspection

```json
{
  "appraisalId": "{{appraisalId}}",
  "inspectionType": "Custom"
}
```

---

# 8. Condition Score Test Data

## 8.1 PUT `/api/inspections/{inspectionId}/scores` — Vintage/Classic 12-System Scores

This is the primary score set for the 1970 Chevrolet K10 subject vehicle.

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 4.4,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Frame-off paint and body work; minor use marks only."
    },
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "score0To5": 4.6,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Frame and mounts refinished; no rust-through."
    },
    {
      "systemNumber": 3,
      "systemKey": "powertrain",
      "score0To5": 4.5,
      "weight": 0.13,
      "isRedFlag": false,
      "notes": "6.0L LS conversion is clean, reliable, and well installed."
    },
    {
      "systemNumber": 4,
      "systemKey": "transmission_driveline",
      "score0To5": 4.4,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "4L80E and NP241 operate correctly."
    },
    {
      "systemNumber": 5,
      "systemKey": "suspension_steering",
      "score0To5": 4.1,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Suspension refreshed and steering tracks correctly."
    },
    {
      "systemNumber": 6,
      "systemKey": "braking_system",
      "score0To5": 4.2,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Brakes perform consistently with no pull."
    },
    {
      "systemNumber": 7,
      "systemKey": "electrical_system",
      "score0To5": 4.0,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Modern wiring and accessories operate correctly."
    },
    {
      "systemNumber": 8,
      "systemKey": "interior",
      "score0To5": 4.2,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Interior restored with comfort upgrades."
    },
    {
      "systemNumber": 9,
      "systemKey": "wheels_tires",
      "score0To5": 4.0,
      "weight": 0.04,
      "isRedFlag": false,
      "notes": "Wheels and tires suit vehicle usage."
    },
    {
      "systemNumber": 10,
      "systemKey": "trim_glass_details",
      "score0To5": 3.9,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Trim and glass present well; minor detail imperfections."
    },
    {
      "systemNumber": 11,
      "systemKey": "documentation_provenance",
      "score0To5": 4.1,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Ownership/build documentation available."
    },
    {
      "systemNumber": 12,
      "systemKey": "road_test_functional",
      "score0To5": 4.3,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Road test confirms stable operation."
    }
  ]
}
```

### Expected Result

| Validation | Expected |
|---|---|
| Score range | Every `score0To5` is between `0.00` and `5.00` |
| System count | 12 rows |
| System numbers | 1 through 12 |
| Weight total | `1.00` |
| Red flags | `false` for all baseline rows |
| Condition index expectation | High driver/restomod quality |

---

## 8.2 PUT `/api/inspections/{inspectionId}/scores` — Standard Vehicle Weights

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 3.8,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Minor paint chips; panels aligned."
    },
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "score0To5": 4.1,
      "weight": 0.1,
      "isRedFlag": false,
      "notes": "No structural corrosion found."
    },
    {
      "systemNumber": 3,
      "systemKey": "powertrain",
      "score0To5": 4.0,
      "weight": 0.18,
      "isRedFlag": false,
      "notes": "Starts cleanly; no major leaks."
    },
    {
      "systemNumber": 4,
      "systemKey": "transmission_driveline",
      "score0To5": 3.9,
      "weight": 0.14,
      "isRedFlag": false,
      "notes": "Shift quality acceptable."
    },
    {
      "systemNumber": 5,
      "systemKey": "suspension_steering",
      "score0To5": 3.7,
      "weight": 0.1,
      "isRedFlag": false,
      "notes": "Normal wear items present."
    },
    {
      "systemNumber": 6,
      "systemKey": "braking_system",
      "score0To5": 4.2,
      "weight": 0.1,
      "isRedFlag": false,
      "notes": "Brakes test good."
    },
    {
      "systemNumber": 7,
      "systemKey": "electrical_system",
      "score0To5": 3.6,
      "weight": 0.1,
      "isRedFlag": false,
      "notes": "All major circuits functional."
    },
    {
      "systemNumber": 8,
      "systemKey": "interior",
      "score0To5": 3.5,
      "weight": 0.04,
      "isRedFlag": false,
      "notes": "Driver seat wear visible."
    },
    {
      "systemNumber": 9,
      "systemKey": "wheels_tires",
      "score0To5": 4.0,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Tires within date and tread range."
    },
    {
      "systemNumber": 10,
      "systemKey": "trim_glass_details",
      "score0To5": 3.4,
      "weight": 0.02,
      "isRedFlag": false,
      "notes": "Small trim blemishes."
    },
    {
      "systemNumber": 11,
      "systemKey": "documentation_provenance",
      "score0To5": 3.0,
      "weight": 0.04,
      "isRedFlag": false,
      "notes": "Partial service documentation."
    },
    {
      "systemNumber": 12,
      "systemKey": "road_test_functional",
      "score0To5": 4.0,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Road test passed."
    }
  ]
}
```

---

## 8.3 PUT `/api/inspections/{inspectionId}/scores` — Custom Vehicle Weights

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 4.5,
      "weight": 0.15,
      "isRedFlag": false,
      "notes": "High-quality paint and panel alignment."
    },
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "score0To5": 4.6,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Frame refinished; no repair evidence."
    },
    {
      "systemNumber": 3,
      "systemKey": "powertrain",
      "score0To5": 4.4,
      "weight": 0.16,
      "isRedFlag": false,
      "notes": "LS swap is clean and well documented."
    },
    {
      "systemNumber": 4,
      "systemKey": "transmission_driveline",
      "score0To5": 4.3,
      "weight": 0.1,
      "isRedFlag": false,
      "notes": "4L80E conversion performs well."
    },
    {
      "systemNumber": 5,
      "systemKey": "suspension_steering",
      "score0To5": 4.0,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Modernized steering components."
    },
    {
      "systemNumber": 6,
      "systemKey": "braking_system",
      "score0To5": 4.2,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Disc brake conversion performs well."
    },
    {
      "systemNumber": 7,
      "systemKey": "electrical_system",
      "score0To5": 4.0,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Harness upgrade documented."
    },
    {
      "systemNumber": 8,
      "systemKey": "interior",
      "score0To5": 4.1,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Restored interior with comfort upgrades."
    },
    {
      "systemNumber": 9,
      "systemKey": "wheels_tires",
      "score0To5": 4.0,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Aftermarket wheel/tire package fits build."
    },
    {
      "systemNumber": 10,
      "systemKey": "trim_glass_details",
      "score0To5": 3.9,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Glass and trim are above driver quality."
    },
    {
      "systemNumber": 11,
      "systemKey": "documentation_provenance",
      "score0To5": 3.7,
      "weight": 0.03,
      "isRedFlag": false,
      "notes": "Build receipts and photos provided."
    },
    {
      "systemNumber": 12,
      "systemKey": "road_test_functional",
      "score0To5": 4.3,
      "weight": 0.04,
      "isRedFlag": false,
      "notes": "No drivability issues during test."
    }
  ]
}
```

---

## 8.4 Red Flag Scenario

Use this payload to verify red flag behavior. It should either:

- Save red flags and force manual review downstream, or
- Return validation errors if red flag rules are stricter in the current API.

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 2.0,
      "weight": 0.12,
      "isRedFlag": true,
      "notes": "Severe corrosion in cab corners and bed supports."
    },
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "score0To5": 1.5,
      "weight": 0.09,
      "isRedFlag": true,
      "notes": "Frame scaling and suspected repair near rear spring mount."
    },
    {
      "systemNumber": 3,
      "systemKey": "powertrain",
      "score0To5": 3.0,
      "weight": 0.13,
      "isRedFlag": false,
      "notes": "Engine starts but leaks observed."
    },
    {
      "systemNumber": 4,
      "systemKey": "transmission_driveline",
      "score0To5": 3.0,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Functional but driveline vibration present."
    },
    {
      "systemNumber": 5,
      "systemKey": "suspension_steering",
      "score0To5": 2.5,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Steering play above normal."
    },
    {
      "systemNumber": 6,
      "systemKey": "braking_system",
      "score0To5": 2.0,
      "weight": 0.05,
      "isRedFlag": true,
      "notes": "Pulls hard under braking; unsafe until repaired."
    },
    {
      "systemNumber": 7,
      "systemKey": "electrical_system",
      "score0To5": 2.5,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Intermittent accessory operation."
    },
    {
      "systemNumber": 8,
      "systemKey": "interior",
      "score0To5": 2.8,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Driver-quality worn interior."
    },
    {
      "systemNumber": 9,
      "systemKey": "wheels_tires",
      "score0To5": 2.0,
      "weight": 0.04,
      "isRedFlag": true,
      "notes": "Aged tires with sidewall cracking."
    },
    {
      "systemNumber": 10,
      "systemKey": "trim_glass_details",
      "score0To5": 2.7,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Missing trim and scratched glass."
    },
    {
      "systemNumber": 11,
      "systemKey": "documentation_provenance",
      "score0To5": 1.5,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Minimal documentation."
    },
    {
      "systemNumber": 12,
      "systemKey": "road_test_functional",
      "score0To5": 2.0,
      "weight": 0.09,
      "isRedFlag": true,
      "notes": "Road test aborted due to braking concern."
    }
  ]
}
```

---

# 9. Comparable Sales Test Data

Post each comparable one at a time to:

```text
POST /api/appraisals/{appraisalId}/comparables
```

## 9.1 Comparable 1 — Local Restored K10

```json
{
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "Automatic",
  "mileage": 18500,
  "exteriorColor": "Green",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Frame-Off Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Medium",
  "titleStatus": "Clean",
  "source": "Bring a Trailer",
  "sourceListingUrl": "https://example.com/comps/1970-k10-green",
  "venueType": "Online Auction",
  "saleDate": "2025-09-18",
  "salePrice": 68500.0,
  "askingPrice": null,
  "locationCity": "Boise",
  "locationState": "ID",
  "overallConditionScore": 4.1,
  "conditionSummary": "Restored driver-quality K10 with strong presentation.",
  "authenticityScore": 0.72,
  "hasRedFlags": false,
  "notes": "Strong body and similar drivetrain profile; not LS swapped."
}
```

## 9.2 Comparable 2 — LS Restomod K10

```json
{
  "year": 1971,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Cheyenne",
  "bodyStyle": "Short Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "4x4",
  "engine": "LS V8",
  "transmission": "Automatic",
  "mileage": 4200,
  "exteriorColor": "Blue / White",
  "interiorColor": "Black",
  "originalityLevel": "Restomod",
  "restorationLevel": "Frame-Off Restoration",
  "matchingNumbers": false,
  "documentationLevel": "High",
  "titleStatus": "Clean",
  "source": "Mecum",
  "sourceListingUrl": "https://example.com/comps/1971-k10-ls",
  "venueType": "Live Auction",
  "saleDate": "2025-06-07",
  "salePrice": 82500.0,
  "askingPrice": null,
  "locationCity": "Denver",
  "locationState": "CO",
  "overallConditionScore": 4.5,
  "conditionSummary": "High-quality LS restomod with documentation.",
  "authenticityScore": 0.68,
  "hasRedFlags": false,
  "notes": "Closest drivetrain/build comparable."
}
```

## 9.3 Comparable 3 — Lower Anchor Long Bed K10

```json
{
  "year": 1969,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Long Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "Manual",
  "mileage": 60200,
  "exteriorColor": "Red",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Older Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Low",
  "titleStatus": "Clean",
  "source": "Hemmings",
  "sourceListingUrl": "https://example.com/comps/1969-k10-longbed",
  "venueType": "Dealer Listing",
  "saleDate": "2024-11-12",
  "salePrice": 46250.0,
  "askingPrice": 48900.0,
  "locationCity": "Spokane",
  "locationState": "WA",
  "overallConditionScore": 3.6,
  "conditionSummary": "Older restoration, long bed, good driver.",
  "authenticityScore": 0.8,
  "hasRedFlags": false,
  "notes": "Useful lower anchor; body configuration adjustment expected."
}
```

## 9.4 Comparable 4 — Upper Anchor Show-Quality K10

```json
{
  "year": 1972,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Cheyenne Super",
  "bodyStyle": "Short Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "4x4",
  "engine": "LS3 V8",
  "transmission": "4L80E Automatic",
  "mileage": 980,
  "exteriorColor": "Orange / White",
  "interiorColor": "Houndstooth",
  "originalityLevel": "Restomod",
  "restorationLevel": "Show Quality",
  "matchingNumbers": false,
  "documentationLevel": "High",
  "titleStatus": "Clean",
  "source": "Barrett-Jackson",
  "sourceListingUrl": "https://example.com/comps/1972-k10-show",
  "venueType": "Live Auction",
  "saleDate": "2025-01-25",
  "salePrice": 112000.0,
  "askingPrice": null,
  "locationCity": "Scottsdale",
  "locationState": "AZ",
  "overallConditionScore": 4.85,
  "conditionSummary": "Show-quality restomod; premium presentation.",
  "authenticityScore": 0.6,
  "hasRedFlags": false,
  "notes": "Upper market anchor; requires show-quality adjustment."
}
```

## 9.5 Comparable 5 — C10 Restomod Adjustment Candidate

```json
{
  "year": 1970,
  "make": "Chevrolet",
  "model": "C10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "2WD",
  "engine": "LS V8",
  "transmission": "Automatic",
  "mileage": 5100,
  "exteriorColor": "White",
  "interiorColor": "Black",
  "originalityLevel": "Restomod",
  "restorationLevel": "Frame-Off Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Medium",
  "titleStatus": "Clean",
  "source": "Cars & Bids",
  "sourceListingUrl": "https://example.com/comps/1970-c10-ls",
  "venueType": "Online Auction",
  "saleDate": "2024-08-09",
  "salePrice": 61500.0,
  "askingPrice": null,
  "locationCity": "Salt Lake City",
  "locationState": "UT",
  "overallConditionScore": 4.2,
  "conditionSummary": "High-quality C10 restomod; 2WD adjustment required.",
  "authenticityScore": 0.62,
  "hasRedFlags": false,
  "notes": "Useful restomod quality comp, but drivetrain differs."
}
```

---

## 9.6 GET `/api/appraisals/{appraisalId}/comparables`

No request body.

### Expected Result

The API should return all comparable sales attached to the appraisal.

---

# 10. Valuation Run Test Data

## 10.1 POST `/api/appraisals/{appraisalId}/valuation/run` — No Body Option

Some Nexus valuation endpoints run from persisted appraisal data only. If Swagger does not require a body, use:

```json
{}
```

## 10.2 POST `/api/appraisals/{appraisalId}/valuation/run` — Versioned Body Option

Use this only if Swagger exposes a request body.

```json
{
  "engineVersion": "nexus-avm-v1.0",
  "rubricVersion": "condition-rubric-v1.0",
  "rulesetVersion": "classic-restomod-ruleset-v1.0",
  "runNotes": "Swagger test valuation run using complete K10 dataset."
}
```

### Expected Result

| Field | Expected |
|---|---|
| HTTP status | `200 OK` or `201 Created` |
| Response contains | `valuationRunId` |
| Final value | Populated |
| Value low/high | Populated if implemented |
| Confidence | Populated if implemented |
| Trace lines | Persisted or retrievable |

---

## 10.3 GET `/api/appraisals/{appraisalId}/valuation/runs`

No request body.

---

## 10.4 Optional GET `/api/valuation/runs/{valuationRunId}`

No request body.

---

# 11. Report Test Data

## 11.1 POST `/api/appraisals/{appraisalId}/report`

Use only if report endpoint is currently implemented.

```json
{
  "templateCode": "classic-appraisal-standard-v1",
  "outputFormat": "Pdf",
  "includeTraceLines": true,
  "includeComparableSales": true,
  "includeInspectionSummary": true
}
```

### Expected Result

| Field | Expected |
|---|---|
| HTTP status | `200 OK`, `201 Created`, or `202 Accepted` |
| Report format | `Pdf` |
| Includes | Inspection summary, comparable sales, trace lines |

---

# 12. Negative Test Data

## 12.1 Vehicle Missing Required Values

```json
{
  "vin": "",
  "year": 1970,
  "make": "",
  "model": "K10",
  "mileage": 1250
}
```

Expected: `400 Bad Request` or validation ProblemDetails.

---

## 12.2 Vehicle Invalid Year and Mileage

```json
{
  "vin": "BADYEAR000001",
  "year": 1799,
  "make": "Chevrolet",
  "model": "K10",
  "mileage": -10
}
```

Expected: `400 Bad Request`.

---

## 12.3 Duplicate VIN Test

Re-submit the valid vehicle payload from section 5.1.

Expected: duplicate-key handling, usually `409 Conflict` or `400 Bad Request`.

---

## 12.4 Inspection Score Out of Range

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 5.5,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Invalid score above maximum."
    }
  ]
}
```

Expected: `400 Bad Request`.

---

## 12.5 Invalid System Number

```json
{
  "scores": [
    {
      "systemNumber": 13,
      "systemKey": "unknown_system",
      "score0To5": 3.0,
      "weight": 0.01,
      "isRedFlag": false,
      "notes": "Invalid system number."
    }
  ]
}
```

Expected: `400 Bad Request`.

---

## 12.6 Weights Do Not Equal 1.00

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 4.0,
      "weight": 0.5,
      "isRedFlag": false,
      "notes": "Weight total intentionally invalid."
    },
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "score0To5": 4.0,
      "weight": 0.2,
      "isRedFlag": false,
      "notes": "Weight total intentionally invalid."
    }
  ]
}
```

Expected: `400 Bad Request` if weight-total validation is implemented.

---

## 12.7 Comparable Negative Sale Price

```json
{
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "Automatic",
  "mileage": 18500,
  "exteriorColor": "Green",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Frame-Off Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Medium",
  "titleStatus": "Clean",
  "source": "Bring a Trailer",
  "sourceListingUrl": "https://example.com/comps/1970-k10-green",
  "venueType": "Online Auction",
  "saleDate": "2025-09-18",
  "salePrice": -100.0,
  "askingPrice": null,
  "locationCity": "Boise",
  "locationState": "ID",
  "overallConditionScore": 4.1,
  "conditionSummary": "Restored driver-quality K10 with strong presentation.",
  "authenticityScore": 0.72,
  "hasRedFlags": false,
  "notes": "Invalid negative sale price."
}
```

Expected: `400 Bad Request`.

---

## 12.8 Comparable Future Sale Date

```json
{
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "C/K 10 Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "Automatic",
  "mileage": 18500,
  "exteriorColor": "Green",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Frame-Off Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Medium",
  "titleStatus": "Clean",
  "source": "Bring a Trailer",
  "sourceListingUrl": "https://example.com/comps/1970-k10-green",
  "venueType": "Online Auction",
  "saleDate": "2035-01-01",
  "salePrice": 68500.0,
  "askingPrice": null,
  "locationCity": "Boise",
  "locationState": "ID",
  "overallConditionScore": 4.1,
  "conditionSummary": "Restored driver-quality K10 with strong presentation.",
  "authenticityScore": 0.72,
  "hasRedFlags": false,
  "notes": "Invalid future sale date for valuation dataset."
}
```

Expected: `400 Bad Request` if future sale dates are blocked.

---

# 13. Boundary Test Data

## 13.1 Minimum Valid Score

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 0.0,
      "weight": 1.0,
      "isRedFlag": true,
      "notes": "Boundary test: minimum score."
    }
  ]
}
```

## 13.2 Maximum Valid Score

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 5.0,
      "weight": 1.0,
      "isRedFlag": false,
      "notes": "Boundary test: maximum score."
    }
  ]
}
```

## 13.3 Zero Mileage

```json
{
  "vin": "CE140Z100002",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "6.0L LS V8",
  "transmission": "4L80E Automatic",
  "drivetrain": "4x4",
  "mileage": 0
}
```

## 13.4 Null Optional Mileage

Use only if the API allows nullable mileage.

```json
{
  "vin": "CE140Z100003",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "6.0L LS V8",
  "transmission": "4L80E Automatic",
  "drivetrain": "4x4",
  "mileage": null
}
```

---

# 14. Regression Checklist

Use this checklist after completing the Swagger test run.

| Area | Pass Criteria |
|---|---|
| Vehicle CRUD | POST, GET, and PUT work without 500 errors |
| Appraisal creation | Appraisal links to vehicle |
| Inspection creation | Inspection links to appraisal |
| Score save | 12 condition scores persist |
| Score schema | Uses `systemNumber`, `systemKey`, `score0To5`, `weight`, `isRedFlag` |
| Weight validation | Correct type weights total `1.00` |
| Comparables | At least 5 comparables persist |
| Valuation run | Valuation executes from persisted inspection and comps |
| Traceability | Trace lines are saved or exposed |
| ProblemDetails | Invalid requests return controlled validation errors |
| Swagger cleanliness | No duplicate route or schema conflicts |

---

# 15. Recommended Swagger Test Matrix

| Test ID | Area | Request | Expected |
|---|---|---|---|
| SWG-VEH-001 | Vehicle | POST valid K10 | Vehicle created |
| SWG-VEH-002 | Vehicle | GET created vehicle | Vehicle returned |
| SWG-VEH-003 | Vehicle | PUT update mileage/trim | Vehicle updated |
| SWG-VEH-004 | Vehicle | POST duplicate VIN | Validation/conflict |
| SWG-APP-001 | Appraisal | POST for vehicle | Appraisal created |
| SWG-APP-002 | Appraisal | GET appraisal | Appraisal returned |
| SWG-INS-001 | Inspection | POST VintageClassic | Inspection created |
| SWG-SCR-001 | Scores | PUT 12 valid scores | Scores saved |
| SWG-SCR-002 | Scores | Score greater than 5 | Validation error |
| SWG-SCR-003 | Scores | Invalid system number | Validation error |
| SWG-CMP-001 | Comparables | POST comp 1 | Comparable created |
| SWG-CMP-002 | Comparables | POST all 5 comps | All comps created |
| SWG-CMP-003 | Comparables | GET comps | Comps returned |
| SWG-VAL-001 | Valuation | POST valuation run | Run created |
| SWG-VAL-002 | Valuation | GET valuation runs | Run returned |
| SWG-RPT-001 | Report | POST report if supported | Report generated/requested |

---

# 16. Machine-Readable Companion File

A JSON payload library is also provided as:

```text
Nexus_Swagger_Test_Data_v1.0.json
```

Use it as a copy/paste source for Swagger request bodies or future API automation.

---

# 17. Notes for Current Nexus Implementation

1. Use the minimal vehicle payload first because the current API DTO may expose fewer fields than the locked database schema.
2. Use the extended vehicle payload only after the Vehicle API contract has been expanded to include all repository columns.
3. If valuation run fails, confirm these prerequisites:
   - Appraisal exists.
   - Inspection exists.
   - 12 condition scores exist.
   - At least 3 comparable sales exist.
   - `valuation_runs` and `valuation_trace_lines` tables match the lock schema.
4. If Swagger shows a request body schema that differs from this document, preserve the values but adapt the property names to the live Swagger schema.

---
