openapi: 3.0.3

info:
  title: ONE Classics Nexus API
  version: 1.0.0
  description: |
    Inspection-driven automotive appraisal and valuation system.

    This API supports:
    - Vehicle management
    - Appraisal lifecycle
    - 12-system inspection scoring
    - Comparable sales analysis
    - Deterministic valuation engine execution
    - Valuation traceability

servers:
  - url: https://localhost:5001

tags:
  - name: Vehicles
  - name: Appraisals
  - name: Inspections
  - name: Comparables
  - name: Valuation
  - name: Reports

paths:

  #############################################
  # VEHICLES
  #############################################

  /api/vehicles:
    post:
      tags: [Vehicles]
      summary: Create vehicle
      operationId: createVehicle
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateVehicleRequest'
      responses:
        '201':
          description: Vehicle created
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/VehicleResponse'
        '400':
          $ref: '#/components/responses/BadRequest'

  /api/vehicles/{vehicleId}:
    get:
      tags: [Vehicles]
      summary: Get vehicle by ID
      operationId: getVehicle
      parameters:
        - $ref: '#/components/parameters/VehicleId'
      responses:
        '200':
          description: Vehicle found
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/VehicleResponse'
        '404':
          $ref: '#/components/responses/NotFound'

    put:
      tags: [Vehicles]
      summary: Update vehicle
      operationId: updateVehicle
      parameters:
        - $ref: '#/components/parameters/VehicleId'
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/UpdateVehicleRequest'
      responses:
        '204':
          description: Updated
        '400':
          $ref: '#/components/responses/BadRequest'

  #############################################
  # APPRAISALS
  #############################################

  /api/appraisals:
    post:
      tags: [Appraisals]
      summary: Create appraisal
      operationId: createAppraisal
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateAppraisalRequest'
      responses:
        '201':
          description: Appraisal created
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AppraisalResponse'

  /api/appraisals/{appraisalId}:
    get:
      tags: [Appraisals]
      summary: Get appraisal
      operationId: getAppraisal
      parameters:
        - $ref: '#/components/parameters/AppraisalId'
      responses:
        '200':
          description: Appraisal found
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AppraisalResponse'
        '404':
          $ref: '#/components/responses/NotFound'

  #############################################
  # INSPECTIONS
  #############################################

  /api/inspections:
    post:
      tags: [Inspections]
      summary: Create inspection
      operationId: createInspection
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateInspectionRequest'
      responses:
        '201':
          description: Inspection created
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/InspectionResponse'

  /api/inspections/{inspectionId}:
    get:
      tags: [Inspections]
      summary: Get inspection
      operationId: getInspection
      parameters:
        - $ref: '#/components/parameters/InspectionId'
      responses:
        '200':
          description: Inspection found
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/InspectionResponse'

  /api/inspections/{inspectionId}/scores:
    put:
      tags: [Inspections]
      summary: Save 12-system inspection scores
      operationId: saveInspectionScores
      parameters:
        - $ref: '#/components/parameters/InspectionId'
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/SaveInspectionScoresRequest'
      responses:
        '204':
          description: Scores saved
        '400':
          $ref: '#/components/responses/BadRequest'

  #############################################
  # COMPARABLES
  #############################################

  /api/appraisals/{appraisalId}/comparables:
    post:
      tags: [Comparables]
      summary: Add comparable sale
      operationId: addComparable
      parameters:
        - $ref: '#/components/parameters/AppraisalId'
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateComparableRequest'
      responses:
        '201':
          description: Comparable added

    get:
      tags: [Comparables]
      summary: Get comparable sales
      operationId: getComparables
      parameters:
        - $ref: '#/components/parameters/AppraisalId'
      responses:
        '200':
          description: Comparable list
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ComparableListResponse'

  #############################################
  # VALUATION
  #############################################

  /api/appraisals/{appraisalId}/valuation/run:
    post:
      tags: [Valuation]
      summary: Execute valuation engine
      operationId: runValuation
      parameters:
        - $ref: '#/components/parameters/AppraisalId'
      responses:
        '200':
          description: Valuation completed
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ValuationRunResponse'

  /api/appraisals/{appraisalId}/valuation/runs:
    get:
      tags: [Valuation]
      summary: Get valuation runs
      operationId: getValuationRuns
      parameters:
        - $ref: '#/components/parameters/AppraisalId'
      responses:
        '200':
          description: List of valuation runs
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ValuationRunListResponse'

  /api/valuation/runs/{valuationRunId}:
    get:
      tags: [Valuation]
      summary: Get valuation run detail with trace
      operationId: getValuationRun
      parameters:
        - $ref: '#/components/parameters/ValuationRunId'
      responses:
        '200':
          description: Valuation run detail
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ValuationRunDetailResponse'

#############################################
# COMPONENTS
#############################################

components:

  #############################################
  # PARAMETERS
  #############################################

  parameters:
    VehicleId:
      name: vehicleId
      in: path
      required: true
      schema:
        type: string
        format: uuid

    AppraisalId:
      name: appraisalId
      in: path
      required: true
      schema:
        type: string
        format: uuid

    InspectionId:
      name: inspectionId
      in: path
      required: true
      schema:
        type: string
        format: uuid

    ValuationRunId:
      name: valuationRunId
      in: path
      required: true
      schema:
        type: string
        format: uuid

  #############################################
  # SCHEMAS
  #############################################

  schemas:

    #########################################
    # VEHICLE
    #########################################

    CreateVehicleRequest:
      type: object
      required: [vin, year, make, model]
      properties:
        vin:
          type: string
        year:
          type: integer
        make:
          type: string
        model:
          type: string
        trim:
          type: string
        body_style:
          type: string
        engine:
          type: string
        transmission:
          type: string
        drivetrain:
          type: string
        mileage:
          type: integer

    UpdateVehicleRequest:
      allOf:
        - $ref: '#/components/schemas/CreateVehicleRequest'

    VehicleResponse:
      type: object
      properties:
        vehicle_id:
          type: string
        vin:
          type: string
        year:
          type: integer
        make:
          type: string
        model:
          type: string

    #########################################
    # INSPECTION SCORES (12 SYSTEM CORE)
    #########################################

    InspectionScoreDto:
      type: object
      required:
        - system_number
        - system_key
        - score_0_to_5
      properties:
        system_number:
          type: integer
        system_key:
          type: string
        score_0_to_5:
          type: number
          format: decimal
        is_red_flag:
          type: boolean
        notes:
          type: string

    SaveInspectionScoresRequest:
      type: object
      required: [scores]
      properties:
        scores:
          type: array
          minItems: 12
          maxItems: 12
          items:
            $ref: '#/components/schemas/InspectionScoreDto'

    #########################################
    # VALUATION
    #########################################

    ValuationRunResponse:
      type: object
      properties:
        valuation_run_id:
          type: string
        final_value:
          type: number
        confidence:
          type: number

    ValuationRunDetailResponse:
      type: object
      properties:
        valuation_run_id:
          type: string
        final_value:
          type: number
        confidence:
          type: number
        trace_lines:
          type: array
          items:
            $ref: '#/components/schemas/TraceLineDto'

    TraceLineDto:
      type: object
      properties:
        sort_order:
          type: integer
        type:
          type: string
        trace_key:
          type: string
        amount:
          type: number
        factor:
          type: number
        message:
          type: string

  #############################################
  # RESPONSES
  #############################################

  responses:

    BadRequest:
      description: Invalid request
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/ProblemDetails'

    NotFound:
      description: Resource not found
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/ProblemDetails'

  #############################################
  # ERROR MODEL
  #############################################

    ProblemDetails:
      type: object
      properties:
        title:
          type: string
        status:
          type: integer
        detail:
          type: string