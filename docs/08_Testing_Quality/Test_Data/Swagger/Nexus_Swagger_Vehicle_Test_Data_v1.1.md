# ONE Classics Nexus
## Swagger Test Data — Locked Specification

**Version:** 1.1  
**Status:** Corrected / Locked DTO Alignment  
**Target API:** `https://localhost:5001`  
**Recommended repo path:** `/docs/08_Testing_Quality/05_Test_Data/Nexus_Swagger_Test_Data_LOCKED_v1.1.md`

---

## 1. Correction Applied

This test data package replaces the prior Swagger test data.

**Locked correction:**

- `POST /api/vehicles` does **not** include `vehicleType`.
- `PUT /api/vehicles/{id}` does **not** include `vehicleType`.
- Vehicle classification for scoring is handled through `inspectionType` on Inspection creation and by the locked system-weight rules, not through the Vehicle request body.
- API JSON examples use `camelCase`.
- Database columns remain `snake_case` and must not be copied directly into Swagger request JSON.

The Nexus MDS defines the platform as inspection-driven, API-first, deterministic, explainable, and version-controlled. It also identifies Vehicles, Appraisals, Inspections, Comparables, Valuation, and Reports as the API workflow areas, with testing covering unit, integration, and golden valuation runs. fileciteturn2file0

---

## 2. Swagger Execution Order

Run these in order.

| Step | Swagger Operation | Purpose |
|---:|---|---|
| 1 | `POST /api/vehicles` | Create subject vehicle |
| 2 | `GET /api/vehicles/{vehicleId}` | Verify vehicle persisted |
| 3 | `PUT /api/vehicles/{vehicleId}` | Verify vehicle update |
| 4 | `POST /api/appraisals` | Create appraisal for vehicle |
| 5 | `GET /api/appraisals/{appraisalId}` | Verify appraisal persisted |
| 6 | `POST /api/inspections` | Create inspection with `inspectionType` |
| 7 | `PUT /api/inspections/{inspectionId}/scores` | Save 12-system scores |
| 8 | `GET /api/inspections/{inspectionId}` | Verify scores persisted |
| 9 | `POST /api/appraisals/{appraisalId}/comparables` | Add comparable sales |
| 10 | `GET /api/appraisals/{appraisalId}/comparables` | Verify comparables |
| 11 | `POST /api/appraisals/{appraisalId}/valuation/run` | Run deterministic valuation |
| 12 | `GET /api/appraisals/{appraisalId}/valuation/runs` | Verify run and trace output |

---

## 3. Runtime Variables

Capture IDs from Swagger responses and reuse them.

```text
{vehicleId}      = vehicleId returned by POST /api/vehicles
{appraisalId}    = appraisalId returned by POST /api/appraisals
{inspectionId}   = inspectionId returned by POST /api/inspections
{comparableId}   = comparableId returned by POST comparable sales
{valuationRunId} = valuationRunId returned by POST valuation run
```

---

# 4. Vehicle Swagger Test Data

## 4.1 Vehicle POST — Primary Vintage/Classic Subject

**Endpoint**

```http
POST /api/vehicles
```

**Request body**

```json
{
  "vin": "CKE140Z100001",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "drivetrain": "4x4",
  "mileage": 84215
}
```

**Expected result**

- `201 Created` or `200 OK`, depending on controller implementation.
- Response includes `vehicleId`.
- Response echoes locked vehicle fields.
- No `vehicleType` field is present or required.

---

## 4.2 Vehicle GET

**Endpoint**

```http
GET /api/vehicles/{vehicleId}
```

**Expected result**

- `200 OK`
- Vehicle response matches the created subject vehicle.

---

## 4.3 Vehicle PUT — Primary Vehicle Update

**Endpoint**

```http
PUT /api/vehicles/{vehicleId}
```

**Request body**

```json
{
  "vin": "CKE140Z100001",
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom Deluxe",
  "bodyStyle": "Short Bed Pickup",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "drivetrain": "4x4",
  "mileage": 84287
}
```

**Expected result**

- `200 OK`, `204 No Content`, or `true`, depending on current controller implementation.
- `trim` and `mileage` are updated.
- No `vehicleType` field is present or required.

---

## 4.4 Vehicle POST — Standard Scenario

Use this to verify a normal modern standard vehicle can still be created without `vehicleType`.

```json
{
  "vin": "1FTFW1EF1JFA00001",
  "year": 2018,
  "make": "Ford",
  "model": "F-150",
  "trim": "XLT",
  "bodyStyle": "Crew Cab Pickup",
  "engine": "5.0L V8",
  "transmission": "10-Speed Automatic",
  "drivetrain": "4x4",
  "mileage": 73500
}
```

---

## 4.5 Vehicle POST — Custom Scenario

Use this to verify a custom/restomod vehicle can still be created without `vehicleType`.

```json
{
  "vin": "124379N600001",
  "year": 1969,
  "make": "Chevrolet",
  "model": "Camaro",
  "trim": "Restomod",
  "bodyStyle": "Coupe",
  "engine": "LS3 V8",
  "transmission": "6-Speed Manual",
  "drivetrain": "RWD",
  "mileage": 1250
}
```

---

## 4.6 Vehicle Negative Test — Missing Required Make

```json
{
  "vin": "NEGATIVE001",
  "year": 1970,
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "drivetrain": "4x4",
  "mileage": 84215
}
```

**Expected result**

- `400 Bad Request`
- ProblemDetails or validation error response.

---

## 4.7 Vehicle Negative Test — Invalid Year

```json
{
  "vin": "NEGATIVE002",
  "year": 1800,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "drivetrain": "4x4",
  "mileage": 84215
}
```

**Expected result**

- `400 Bad Request`, if year validation is implemented.
- If accepted, log as validation gap.

---

## 4.8 Vehicle Schema Guardrail — Do Not Use

The following field is **not part of locked Vehicle POST/PUT** and must not appear in Swagger Vehicle request schemas:

```json
{
  "vehicleType": "VintageClassic"
}
```

If Swagger shows this field under Vehicle POST or Vehicle PUT, the API contract has drifted and should be corrected.

---

# 5. Appraisal Swagger Test Data

## 5.1 Appraisal POST

**Endpoint**

```http
POST /api/appraisals
```

**Request body**

```json
{
  "vehicleId": "{{vehicleId}}"
}
```

**Expected result**

- Response includes `appraisalId`.
- Response links to the existing `vehicleId`.

---

## 5.2 Appraisal GET

```http
GET /api/appraisals/{appraisalId}
```

**Expected result**

- `200 OK`
- Returned appraisal references `{vehicleId}`.

---

# 6. Inspection Swagger Test Data

## 6.1 Inspection POST — Vintage/Classic

This is where inspection/scoring classification belongs.

**Endpoint**

```http
POST /api/inspections
```

**Request body**

```json
{
  "appraisalId": "{{appraisalId}}",
  "inspectionType": "VintageClassic"
}
```

**Expected result**

- Response includes `inspectionId`.
- `inspectionType` equals `VintageClassic`.

---

## 6.2 Inspection POST — Standard Variant

```json
{
  "appraisalId": "{{appraisalId}}",
  "inspectionType": "Standard"
}
```

---

## 6.3 Inspection POST — Custom Variant

```json
{
  "appraisalId": "{{appraisalId}}",
  "inspectionType": "Custom"
}
```

---

# 7. 12-System Condition Score Swagger Test Data

## 7.1 Vintage/Classic Full Score Save

**Endpoint**

```http
PUT /api/inspections/{inspectionId}/scores
```

**Request body**

```json
{
  "scores": [
    {
      "systemNumber": 1,
      "systemKey": "body_exterior",
      "score0To5": 4.4,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Straight panels, quality paint, minor age visible under close inspection."
    },
    {
      "systemNumber": 2,
      "systemKey": "frame_structure",
      "score0To5": 4.2,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Frame appears solid; no visible collision damage or structural rust concerns."
    },
    {
      "systemNumber": 3,
      "systemKey": "powertrain",
      "score0To5": 4.6,
      "weight": 0.13,
      "isRedFlag": false,
      "notes": "Strong-running V8 with clean idle, no observed smoke, leaks, or overheating."
    },
    {
      "systemNumber": 4,
      "systemKey": "transmission_driveline",
      "score0To5": 4.1,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Transmission shifts correctly; transfer case and driveline operate as expected."
    },
    {
      "systemNumber": 5,
      "systemKey": "suspension_steering",
      "score0To5": 4.0,
      "weight": 0.06,
      "isRedFlag": false,
      "notes": "Steering and suspension are serviceable with normal vintage-truck road feel."
    },
    {
      "systemNumber": 6,
      "systemKey": "braking_system",
      "score0To5": 4.2,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Brakes stop evenly; no pulling, fade, or hydraulic leaks observed."
    },
    {
      "systemNumber": 7,
      "systemKey": "electrical_system",
      "score0To5": 3.9,
      "weight": 0.05,
      "isRedFlag": false,
      "notes": "Lights, gauges, and charging system functional; minor wiring age noted."
    },
    {
      "systemNumber": 8,
      "systemKey": "interior",
      "score0To5": 4.3,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Interior is clean and period-correct with light wear."
    },
    {
      "systemNumber": 9,
      "systemKey": "wheels_tires",
      "score0To5": 3.8,
      "weight": 0.04,
      "isRedFlag": false,
      "notes": "Wheels present well; tires have adequate tread but are older."
    },
    {
      "systemNumber": 10,
      "systemKey": "trim_glass_details",
      "score0To5": 4.0,
      "weight": 0.08,
      "isRedFlag": false,
      "notes": "Trim and glass are complete with minor polish marks and small imperfections."
    },
    {
      "systemNumber": 11,
      "systemKey": "documentation_provenance",
      "score0To5": 4.5,
      "weight": 0.12,
      "isRedFlag": false,
      "notes": "Good ownership and restoration documentation; VIN/title alignment verified."
    },
    {
      "systemNumber": 12,
      "systemKey": "road_test_functional",
      "score0To5": 4.1,
      "weight": 0.09,
      "isRedFlag": false,
      "notes": "Road test completed; vehicle tracks, accelerates, and stops appropriately."
    }
  ]
}
```

**Expected condition index**

```text
84.9
```

**Expected result**

- 12 score rows are accepted.
- Weights sum to `1.0000`.
- Scores are within `0–5`.
- `systemNumber` values are exactly `1–12`.
- `systemKey` values match the locked Nexus 12-system model.
- No duplicate `systemNumber` exists for the same inspection.

---

# 8. Comparable Sales Swagger Test Data

Post each comparable separately.

## 8.1 Comparable 1 — Best Primary Comp

**Endpoint**

```http
POST /api/appraisals/{appraisalId}/comparables
```

```json
{
  "year": 1970,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "K-Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "mileage": 81200,
  "exteriorColor": "Blue",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Older Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Good",
  "titleStatus": "Clean",
  "source": "Bring a Trailer",
  "sourceListingUrl": "https://example.com/comps/1970-k10-bat-001",
  "venueType": "Online Auction",
  "saleDate": "2025-10-14",
  "salePrice": 68500.0,
  "askingPrice": null,
  "locationCity": "Boise",
  "locationState": "ID",
  "overallConditionScore": 4.2,
  "conditionSummary": "Restored short-bed K10 with strong presentation and similar driveline.",
  "authenticityScore": 4.0,
  "hasRedFlags": false,
  "notes": "Best primary comp: same year, make, model, body style, and drivetrain."
}
```

---

## 8.2 Comparable 2 — Similar Short Bed K10

```json
{
  "year": 1971,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom Deluxe",
  "bodyStyle": "Short Bed Pickup",
  "series": "K-Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "Automatic",
  "mileage": 90500,
  "exteriorColor": "Orange",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Driver Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Moderate",
  "titleStatus": "Clean",
  "source": "Hemmings",
  "sourceListingUrl": "https://example.com/comps/1971-k10-hemmings-002",
  "venueType": "Dealer Listing",
  "saleDate": "2025-06-22",
  "salePrice": 61250.0,
  "askingPrice": 64000.0,
  "locationCity": "Denver",
  "locationState": "CO",
  "overallConditionScore": 3.9,
  "conditionSummary": "Comparable short-bed 4x4, slightly weaker condition and documentation.",
  "authenticityScore": 3.7,
  "hasRedFlags": false,
  "notes": "Useful market comp; adjust for condition and transmission."
}
```

---

## 8.3 Comparable 3 — High-Side Comp

```json
{
  "year": 1972,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Cheyenne",
  "bodyStyle": "Short Bed Pickup",
  "series": "K-Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "mileage": 54100,
  "exteriorColor": "Green",
  "interiorColor": "Green",
  "originalityLevel": "Restored Stock",
  "restorationLevel": "Frame-Off Restoration",
  "matchingNumbers": true,
  "documentationLevel": "Excellent",
  "titleStatus": "Clean",
  "source": "Mecum",
  "sourceListingUrl": "https://example.com/comps/1972-k10-mecum-003",
  "venueType": "Live Auction",
  "saleDate": "2024-12-07",
  "salePrice": 79500.0,
  "askingPrice": null,
  "locationCity": "Las Vegas",
  "locationState": "NV",
  "overallConditionScore": 4.6,
  "conditionSummary": "Higher-grade restored example with strong documentation.",
  "authenticityScore": 4.7,
  "hasRedFlags": false,
  "notes": "High-side comp; adjust downward for year/trim and condition delta."
}
```

---

## 8.4 Comparable 4 — Low-Side Comp

```json
{
  "year": 1969,
  "make": "Chevrolet",
  "model": "K10",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "K-Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "mileage": 101200,
  "exteriorColor": "White",
  "interiorColor": "Black",
  "originalityLevel": "Modified",
  "restorationLevel": "Driver",
  "matchingNumbers": false,
  "documentationLevel": "Limited",
  "titleStatus": "Clean",
  "source": "ClassicCars.com",
  "sourceListingUrl": "https://example.com/comps/1969-k10-classiccars-004",
  "venueType": "Private Listing",
  "saleDate": "2025-03-18",
  "salePrice": 54000.0,
  "askingPrice": 57500.0,
  "locationCity": "Spokane",
  "locationState": "WA",
  "overallConditionScore": 3.5,
  "conditionSummary": "Driver-grade modified truck with less documentation.",
  "authenticityScore": 3.0,
  "hasRedFlags": false,
  "notes": "Low-side comp; useful for range floor."
}
```

---

## 8.5 Comparable 5 — Secondary Platform Comp

```json
{
  "year": 1970,
  "make": "GMC",
  "model": "K1500",
  "trim": "Custom",
  "bodyStyle": "Short Bed Pickup",
  "series": "K-Series",
  "drivetrain": "4x4",
  "engine": "350 V8",
  "transmission": "4-Speed Manual",
  "mileage": 77000,
  "exteriorColor": "Red",
  "interiorColor": "Black",
  "originalityLevel": "Mostly Stock",
  "restorationLevel": "Older Restoration",
  "matchingNumbers": false,
  "documentationLevel": "Good",
  "titleStatus": "Clean",
  "source": "Barrett-Jackson",
  "sourceListingUrl": "https://example.com/comps/1970-gmc-k1500-bj-005",
  "venueType": "Live Auction",
  "saleDate": "2025-01-25",
  "salePrice": 66000.0,
  "askingPrice": null,
  "locationCity": "Scottsdale",
  "locationState": "AZ",
  "overallConditionScore": 4.1,
  "conditionSummary": "Near-platform comparable; make/model adjustment required.",
  "authenticityScore": 3.9,
  "hasRedFlags": false,
  "notes": "Secondary comp because make/model differ from Chevrolet K10."
}
```

---

## 8.6 Comparable GET

```http
GET /api/appraisals/{appraisalId}/comparables
```

**Expected result**

- Returns all comparable records associated to `{appraisalId}`.
- Sale prices, sale dates, source fields, condition summaries, and red flag flags are preserved.

---

# 9. Valuation Swagger Test Data

## 9.1 Run Valuation

**Endpoint**

```http
POST /api/appraisals/{appraisalId}/valuation/run
```

**Preferred locked request**

No body.

If Swagger requires a body because of the current controller implementation, use:

```json
{
  "appraisalId": "{{appraisalId}}"
}
```

**Expected result**

- Response includes `valuationRunId`.
- Response includes condition index, confidence score/band, low value, final value, and high value if those fields are currently exposed.
- Valuation trace lines are persisted.

---

## 9.2 Get Valuation Runs

```http
GET /api/appraisals/{appraisalId}/valuation/runs
```

**Expected result**

- Returns at least one valuation run for `{appraisalId}`.
- Latest run should reflect the inspection scores and comparable sales from this test set.

---

# 10. Report Swagger Test Data

Only use this section if the report endpoints are currently enabled in Swagger.

## 10.1 Generate Report

```http
POST /api/appraisals/{appraisalId}/report
```

Recommended minimal request body, if required by the current Swagger schema:

```json
{
  "appraisalId": "{appraisalId}",
  "templateVersion": "Nexus_Report_Template_v1.0",
  "outputFormat": "Pdf"
}
```

## 10.2 Get Report

```http
GET /api/appraisals/{appraisalId}/report
```

---

# 11. Regression Checklist

| Area | Validation |
|---|---|
| Vehicle POST | Accepts locked vehicle DTO only; no `vehicleType` |
| Vehicle PUT | Accepts locked vehicle DTO only; no `vehicleType` |
| Appraisal POST | Requires valid `vehicleId` |
| Inspection POST | Uses `inspectionType` for Standard/Custom/VintageClassic |
| Score Save | Accepts exactly 12 systems and valid score range |
| Score Weights | Weight total equals `1.0000` |
| Comparable POST | Persists appraisal-linked comp data |
| Valuation Run | Uses inspection scores and comparables |
| Traceability | Valuation run persists explainable trace lines |
| Error Handling | Invalid payloads return ProblemDetails or validation errors |

---

# 12. Machine-Readable Payload Library

A companion JSON file is included:

```text
Nexus_Swagger_Test_Data_LOCKED_v1.1.json
```

It contains the same payloads organized by API area for reuse in Swagger, Postman, integration tests, or future automated API test harnesses.
