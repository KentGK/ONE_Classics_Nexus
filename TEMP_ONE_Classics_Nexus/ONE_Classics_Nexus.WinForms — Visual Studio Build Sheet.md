
# ONE_Classics_Nexus.WinForms — Visual Studio Build Sheet

> Use this as a working checklist while building the WinForms UI project in Visual Studio.
>
> **Project:** `ONE_Classics_Nexus.WinForms`  
> **Solution:** `ONE_Classics_Nexus`  
> **Project Type:** `Windows Forms App (.NET)`  
> **Target Framework:** `.NET 8`

---

# 0. Visual Studio Item Type Quick Reference

Use these exact Visual Studio item types as you build:

| Artifact | Visual Studio Item Type |
|---|---|
| Form | `Windows Form` |
| Reusable UI component | `User Control` |
| Standard code file | `Class` |
| Interface | `Interface` |
| Resource file | `Resources File` or `.resx` |
| Settings file | `Settings File` |
| JSON config file | `JSON File` |
| Markdown note | `Text File` then rename to `.md` |
| App config | `Application Configuration File` |

---

# 1. Create the Project

## Step 1 — Create WinForms Project
- [ ] Open Visual Studio
- [ ] Select **Create a new project**
- [ ] Choose **Windows Forms App**
- [ ] Language: **C#**
- [ ] Framework: **.NET 8**
- [ ] Click **Next**

## Step 2 — Name the Project
- [ ] Project name: `ONE_Classics_Nexus.WinForms`
- [ ] Solution name: `ONE_Classics_Nexus`
- [ ] Set the correct solution location
- [ ] Click **Create**

## Step 3 — Confirm Root Files
- [ ] Confirm `Program.cs` exists
- [ ] Confirm `ONE_Classics_Nexus.WinForms.csproj` exists

---

# 2. Create Top-Level Folders

Create these folders in this exact order.

## Add folders
- [ ] `Assets`
- [ ] `Configuration`
- [ ] `Constants`
- [ ] `DTOs`
- [ ] `Forms`
- [ ] `Controls`
- [ ] `ViewModels`
- [ ] `Services`
- [ ] `Mappers`
- [ ] `Validation`
- [ ] `Helpers`
- [ ] `Extensions`
- [ ] `Models`
- [ ] `Designers`

---

# 3. Create Nested Folders

Create these folders in this exact order.

## Assets
- [ ] `Assets/Branding`
- [ ] `Assets/Icons`
- [ ] `Assets/Themes`

## DTOs
- [ ] `DTOs/Vehicle`
- [ ] `DTOs/Appraisal`
- [ ] `DTOs/Inspection`
- [ ] `DTOs/Comparable`
- [ ] `DTOs/Valuation`
- [ ] `DTOs/Common`

## Forms
- [ ] `Forms/Shell`
- [ ] `Forms/Dashboard`
- [ ] `Forms/Vehicles`
- [ ] `Forms/Appraisals`
- [ ] `Forms/Inspections`
- [ ] `Forms/Comparables`
- [ ] `Forms/Valuations`
- [ ] `Forms/Reports`
- [ ] `Forms/Admin`

## Controls
- [ ] `Controls/Navigation`
- [ ] `Controls/Shell`
- [ ] `Controls/Dashboard`
- [ ] `Controls/Vehicles`
- [ ] `Controls/Inspections`
- [ ] `Controls/Comparables`
- [ ] `Controls/Valuations`
- [ ] `Controls/Common`

## ViewModels
- [ ] `ViewModels/Shell`
- [ ] `ViewModels/Dashboard`
- [ ] `ViewModels/Vehicles`
- [ ] `ViewModels/Appraisals`
- [ ] `ViewModels/Inspections`
- [ ] `ViewModels/Comparables`
- [ ] `ViewModels/Valuations`
- [ ] `ViewModels/Reports`

## Services
- [ ] `Services/Abstractions`
- [ ] `Services/Api`
- [ ] `Services/Navigation`
- [ ] `Services/Messaging`
- [ ] `Services/State`

## Models
- [ ] `Models/Ui`
- [ ] `Models/Session`

## Designers
- [ ] `Designers/Notes`

---

# 4. Phase 1 — Shell Foundation

## Goal
Build the main application shell first.

## 4.1 Create Configuration Files
Use item type: `Class`

- [ ] `Configuration/ApiOptions.cs`
- [ ] `Configuration/UiOptions.cs`
- [ ] `Configuration/AppSessionOptions.cs`

## 4.2 Create Constants
Use item type: `Class`

- [ ] `Constants/NavigationNames.cs`
- [ ] `Constants/UiText.cs`

## 4.3 Create Session and UI Models
Use item type: `Class`

- [ ] `Models/Session/CurrentUserContext.cs`
- [ ] `Models/Ui/BreadcrumbItem.cs`
- [ ] `Models/Ui/StatusBannerModel.cs`

## 4.4 Create Service Interfaces
Use item type: `Interface`

- [ ] `Services/Abstractions/INavigationService.cs`
- [ ] `Services/Abstractions/IMessageDialogService.cs`
- [ ] `Services/Abstractions/IBusyIndicatorService.cs`

## 4.5 Create Service Implementations
Use item type: `Class`

- [ ] `Services/Navigation/NavigationService.cs`
- [ ] `Services/State/UserSessionService.cs`
- [ ] `Services/State/BusyIndicatorService.cs`
- [ ] `Services/State/WorkspaceStateService.cs`
- [ ] `Services/Messaging/MessageDialogService.cs`

## 4.6 Create Helper and Extension Files
Use item type: `Class`

- [ ] `Extensions/ControlExtensions.cs`
- [ ] `Extensions/FormExtensions.cs`
- [ ] `Extensions/ServiceCollectionExtensions.cs`
- [ ] `Helpers/WinFormsThreadHelper.cs`

## 4.7 Create Shell ViewModels
Use item type: `Class`

- [ ] `ViewModels/Shell/NavigationItemViewModel.cs`
- [ ] `ViewModels/Shell/MainShellViewModel.cs`

## 4.8 Create Navigation Controls
Use item type: `User Control`

- [ ] `Controls/Navigation/NavHeaderControl.cs`
- [ ] `Controls/Navigation/SideNavItemControl.cs`
- [ ] `Controls/Navigation/SideNavControl.cs`

## 4.9 Create Shell Controls
Use item type: `User Control`

- [ ] `Controls/Shell/HeaderBarControl.cs`
- [ ] `Controls/Shell/StatusStripControl.cs`

## 4.10 Create Main Shell Form
Use item type: `Windows Form`

- [ ] `Forms/Shell/MainShellForm.cs`

## 4.11 Update Program Startup
- [ ] Open `Program.cs`
- [ ] Update startup to launch `MainShellForm`

### Suggested startup target
- [ ] `Application.Run(new MainShellForm());`

## 4.12 Phase 1 Checkpoint
- [ ] Project builds
- [ ] App opens into `MainShellForm`
- [ ] Shell contains:
  - [ ] left navigation region
  - [ ] top header region
  - [ ] center content host region
  - [ ] bottom status strip region

---

# 5. Phase 2 — Dashboard Workspace

## Goal
Create the first usable screen loaded by the shell.

## 5.1 Create Dashboard ViewModels
Use item type: `Class`

- [ ] `ViewModels/Dashboard/ActivityItemViewModel.cs`
- [ ] `ViewModels/Dashboard/DashboardViewModel.cs`

## 5.2 Create Dashboard Controls
Use item type: `User Control`

- [ ] `Controls/Dashboard/SummaryCardControl.cs`
- [ ] `Controls/Dashboard/ActivityGridControl.cs`

## 5.3 Create Common Controls
Use item type: `User Control`

- [ ] `Controls/Common/LoadingOverlayControl.cs`
- [ ] `Controls/Common/EmptyStateControl.cs`
- [ ] `Controls/Common/SectionTitleControl.cs`

## 5.4 Create Dashboard Form
Use item type: `Windows Form`

- [ ] `Forms/Dashboard/DashboardForm.cs`

## 5.5 Wire Dashboard into Shell
- [ ] Update `MainShellForm` to load `DashboardForm` into the content area
- [ ] Confirm dashboard loads when app starts

## 5.6 Phase 2 Checkpoint
- [ ] Shell launches
- [ ] Dashboard displays inside shell
- [ ] Project builds cleanly

---

# 6. Phase 3 — API and DTO Foundation

## Goal
Build the transport layer before the data screens.

## 6.1 Create API Interfaces
Use item type: `Interface`

- [ ] `Services/Abstractions/IApiClient.cs`
- [ ] `Services/Abstractions/IVehicleApiService.cs`
- [ ] `Services/Abstractions/IAppraisalApiService.cs`
- [ ] `Services/Abstractions/IInspectionApiService.cs`
- [ ] `Services/Abstractions/IComparableApiService.cs`
- [ ] `Services/Abstractions/IValuationApiService.cs`
- [ ] `Services/Abstractions/IReportApiService.cs`

## 6.2 Create Common DTOs
Use item type: `Class`

- [ ] `DTOs/Common/LookupItemDto.cs`
- [ ] `DTOs/Common/ApiErrorDto.cs`
- [ ] `DTOs/Common/PagedResultDto.cs`

## 6.3 Create Vehicle DTOs
Use item type: `Class`

- [ ] `DTOs/Vehicle/VehicleDto.cs`
- [ ] `DTOs/Vehicle/CreateVehicleRequest.cs`
- [ ] `DTOs/Vehicle/UpdateVehicleRequest.cs`

## 6.4 Create Appraisal DTOs
Use item type: `Class`

- [ ] `DTOs/Appraisal/AppraisalDto.cs`
- [ ] `DTOs/Appraisal/CreateAppraisalRequest.cs`
- [ ] `DTOs/Appraisal/UpdateAppraisalRequest.cs`

## 6.5 Create Inspection DTOs
Use item type: `Class`

- [ ] `DTOs/Inspection/InspectionDto.cs`
- [ ] `DTOs/Inspection/ConditionScoreDto.cs`
- [ ] `DTOs/Inspection/SaveInspectionScoresRequest.cs`
- [ ] `DTOs/Inspection/InspectionSectionDto.cs`

## 6.6 Create Comparable DTOs
Use item type: `Class`

- [ ] `DTOs/Comparable/ComparableDto.cs`
- [ ] `DTOs/Comparable/CreateComparableRequest.cs`
- [ ] `DTOs/Comparable/UpdateComparableRequest.cs`

## 6.7 Create Valuation DTOs
Use item type: `Class`

- [ ] `DTOs/Valuation/ValuationRunDto.cs`
- [ ] `DTOs/Valuation/ValuationTraceLineDto.cs`
- [ ] `DTOs/Valuation/RunValuationResponse.cs`

## 6.8 Create API Implementations
Use item type: `Class`

- [ ] `Services/Api/ApiClient.cs`
- [ ] `Services/Api/VehicleApiService.cs`
- [ ] `Services/Api/AppraisalApiService.cs`
- [ ] `Services/Api/InspectionApiService.cs`
- [ ] `Services/Api/ComparableApiService.cs`
- [ ] `Services/Api/ValuationApiService.cs`
- [ ] `Services/Api/ReportApiService.cs`

## 6.9 Phase 3 Checkpoint
- [ ] DTO structure exists
- [ ] API service structure exists
- [ ] Project builds cleanly

---

# 7. Phase 4 — Vehicle Screens

## Goal
Build vehicle list and vehicle detail workflows.

## 7.1 Create Vehicle ViewModels
Use item type: `Class`

- [ ] `ViewModels/Vehicles/VehicleListItemViewModel.cs`
- [ ] `ViewModels/Vehicles/VehicleDetailViewModel.cs`

## 7.2 Create Vehicle Mapper and Validation
Use item type: `Class`

- [ ] `Mappers/VehicleViewModelMapper.cs`
- [ ] `Validation/VehicleValidationService.cs`

## 7.3 Create Vehicle Controls
Use item type: `User Control`

- [ ] `Controls/Vehicles/VehicleSummaryControl.cs`
- [ ] `Controls/Vehicles/VehicleIdentityControl.cs`

## 7.4 Create Vehicle Forms
Use item type: `Windows Form`

- [ ] `Forms/Vehicles/VehicleListForm.cs`
- [ ] `Forms/Vehicles/VehicleDetailForm.cs`

## 7.5 Phase 4 Checkpoint
- [ ] Vehicle screens compile
- [ ] Vehicle forms can be opened from shell or temporary test launch
- [ ] Validation and view models are in place

---

# 8. Phase 5 — Appraisal Workspace

## Goal
Build the appraisal-centered workspace.

## 8.1 Create Appraisal ViewModels
Use item type: `Class`

- [ ] `ViewModels/Appraisals/AppraisalSummaryViewModel.cs`
- [ ] `ViewModels/Appraisals/AppraisalWorkspaceViewModel.cs`

## 8.2 Create Appraisal Mapper and Validation
Use item type: `Class`

- [ ] `Mappers/AppraisalViewModelMapper.cs`
- [ ] `Validation/AppraisalValidationService.cs`

## 8.3 Create Appraisal Forms
Use item type: `Windows Form`

- [ ] `Forms/Appraisals/AppraisalListForm.cs`
- [ ] `Forms/Appraisals/AppraisalWorkspaceForm.cs`

## 8.4 Phase 5 Checkpoint
- [ ] Appraisal screens compile
- [ ] Appraisal workspace can be opened from shell
- [ ] Project still builds cleanly

---

# 9. Phase 6 — Inspection UI

## Goal
Build the 12-system inspection entry workflow.

## 9.1 Create Inspection Constants
Use item type: `Class`

- [ ] `Constants/InspectionUiConstants.cs`
- [ ] `Constants/AppraisalStatusDisplay.cs`

## 9.2 Create Inspection ViewModels
Use item type: `Class`

- [ ] `ViewModels/Inspections/InspectionItemViewModel.cs`
- [ ] `ViewModels/Inspections/InspectionSectionViewModel.cs`
- [ ] `ViewModels/Inspections/InspectionViewModel.cs`

## 9.3 Create Inspection Mapper, Validation, and UI Model
Use item type: `Class`

- [ ] `Mappers/InspectionViewModelMapper.cs`
- [ ] `Validation/InspectionValidationService.cs`
- [ ] `Models/Ui/SectionCompletionState.cs`

## 9.4 Create Inspection Controls
Use item type: `User Control`

- [ ] `Controls/Inspections/InspectionItemControl.cs`
- [ ] `Controls/Inspections/InspectionSectionHeaderControl.cs`
- [ ] `Controls/Inspections/ScoreSelectorControl.cs`
- [ ] `Controls/Inspections/RedFlagBannerControl.cs`

## 9.5 Create Inspection Forms
Use item type: `Windows Form`

- [ ] `Forms/Inspections/InspectionForm.cs`
- [ ] `Forms/Inspections/InspectionSectionForm.cs`

## 9.6 Phase 6 Checkpoint
- [ ] Inspection UI compiles
- [ ] 12-system navigation structure is represented
- [ ] Section/item model is in place
- [ ] Project builds cleanly

---

# 10. Phase 7 — Comparables UI

## Goal
Build comparable sale review screens.

## 10.1 Create Comparable ViewModels
Use item type: `Class`

- [ ] `ViewModels/Comparables/ComparableViewModel.cs`
- [ ] `ViewModels/Comparables/ComparableEditorViewModel.cs`

## 10.2 Create Comparable Mapper and Validation
Use item type: `Class`

- [ ] `Mappers/ComparableViewModelMapper.cs`
- [ ] `Validation/ComparableValidationService.cs`

## 10.3 Create Comparable Controls
Use item type: `User Control`

- [ ] `Controls/Comparables/ComparableGridControl.cs`
- [ ] `Controls/Comparables/ComparableEditorControl.cs`

## 10.4 Create Comparable Form
Use item type: `Windows Form`

- [ ] `Forms/Comparables/ComparableSalesForm.cs`

## 10.5 Phase 7 Checkpoint
- [ ] Comparable UI compiles
- [ ] Project builds cleanly

---

# 11. Phase 8 — Valuation UI

## Goal
Build valuation execution and review screens.

## 11.1 Create Valuation ViewModels
Use item type: `Class`

- [ ] `ViewModels/Valuations/ValuationRunViewModel.cs`
- [ ] `ViewModels/Valuations/ValuationTraceViewModel.cs`

## 11.2 Create Valuation Mapper
Use item type: `Class`

- [ ] `Mappers/ValuationViewModelMapper.cs`

## 11.3 Create Formatting Helpers
Use item type: `Class`

- [ ] `Helpers/CurrencyFormatHelper.cs`
- [ ] `Helpers/DateTimeDisplayHelper.cs`

## 11.4 Create Valuation Controls
Use item type: `User Control`

- [ ] `Controls/Valuations/ValuationSummaryControl.cs`
- [ ] `Controls/Valuations/TraceLineGridControl.cs`

## 11.5 Create Valuation Form
Use item type: `Windows Form`

- [ ] `Forms/Valuations/ValuationRunForm.cs`

## 11.6 Phase 8 Checkpoint
- [ ] Valuation UI compiles
- [ ] Project builds cleanly

---

# 12. Phase 9 — Reports UI

## Goal
Build the reporting workflow shell.

## 12.1 Create Report ViewModel
Use item type: `Class`

- [ ] `ViewModels/Reports/ReportViewModel.cs`

## 12.2 Create Report Form
Use item type: `Windows Form`

- [ ] `Forms/Reports/ReportForm.cs`

## 12.3 Phase 9 Checkpoint
- [ ] Report UI compiles
- [ ] Project builds cleanly

---

# 13. Phase 10 — Admin UI

## Goal
Build the first admin maintenance screen.

## 13.1 Create Admin Form
Use item type: `Windows Form`

- [ ] `Forms/Admin/AdminLookupForm.cs`

## 13.2 Phase 10 Checkpoint
- [ ] Admin screen compiles
- [ ] Project builds cleanly

---

# 14. Phase 11 — Utility and Polish Pass

## Goal
Add supporting files for consistency, assets, and conventions.

## 14.1 Create Helper Files
Use item type: `Class`

- [ ] `Helpers/ControlFactory.cs`
- [ ] `Helpers/DataGridViewBuilder.cs`
- [ ] `Helpers/EnumDisplayHelper.cs`

## 14.2 Add Branding and Theme Files
- [ ] Add `Assets/Branding/one_classics_logo_placeholder.png`
- [ ] Add `Assets/Branding/app_icon_placeholder.ico`
- [ ] Add `Assets/Themes/ColorPalette.md` using `Text File` renamed to `.md`

## 14.3 Add Designer Conventions Note
- [ ] Add `Designers/Notes/WinForms_Designer_Conventions.md` using `Text File` renamed to `.md`

## 14.4 Add Property and Config Files
- [ ] Add `Properties/Resources.resx`
- [ ] Add `Properties/Settings.settings`
- [ ] Add `App.config`

## 14.5 Phase 11 Checkpoint
- [ ] Utility helpers are in place
- [ ] Assets are in place
- [ ] Project builds cleanly

---

# 15. Namespace Alignment Checklist

Use strict namespace-to-folder mapping.

## Examples
- [ ] `ONE_Classics_Nexus.WinForms.Forms.Shell`
- [ ] `ONE_Classics_Nexus.WinForms.Controls.Navigation`
- [ ] `ONE_Classics_Nexus.WinForms.Services.Api`
- [ ] `ONE_Classics_Nexus.WinForms.ViewModels.Inspections`

## Rule
- [ ] Namespace matches folder path
- [ ] Do not mix unrelated namespaces in the wrong folder
- [ ] Keep root-only files minimal

---

# 16. First Practical Build Session

Use this short list if you want the fastest productive start.

## Session 1
- [ ] Create all top-level folders
- [ ] Create shell-related nested folders
- [ ] Add `ApiOptions.cs`
- [ ] Add `NavigationNames.cs`
- [ ] Add `INavigationService.cs`
- [ ] Add `NavigationService.cs`
- [ ] Add `SideNavControl.cs`
- [ ] Add `HeaderBarControl.cs`
- [ ] Add `StatusStripControl.cs`
- [ ] Add `MainShellForm.cs`
- [ ] Add `DashboardForm.cs`
- [ ] Update `Program.cs`
- [ ] Build and run

## Session 2
- [ ] Add dashboard controls and view models
- [ ] Wire dashboard into shell
- [ ] Add common controls
- [ ] Build and run

## Session 3
- [ ] Add DTO folders and API interfaces
- [ ] Add `ApiClient.cs`
- [ ] Add entity service classes
- [ ] Build

---

# 17. Build Verification Checklist

Run this after every phase.

## Verify
- [ ] Project compiles
- [ ] No namespace drift
- [ ] No duplicate class names
- [ ] Forms and controls are in correct folders
- [ ] Designer files remain paired with the correct form/control
- [ ] `Program.cs` still launches correctly

---

# 18. Minimal Fast-Start File Order

If you only want the first set of files to create right now, use this order exactly:

- [ ] `Configuration/ApiOptions.cs`
- [ ] `Constants/NavigationNames.cs`
- [ ] `Services/Abstractions/INavigationService.cs`
- [ ] `Services/Navigation/NavigationService.cs`
- [ ] `Controls/Navigation/SideNavControl.cs`
- [ ] `Controls/Shell/HeaderBarControl.cs`
- [ ] `Controls/Shell/StatusStripControl.cs`
- [ ] `Forms/Shell/MainShellForm.cs`
- [ ] `Forms/Dashboard/DashboardForm.cs`
- [ ] Update `Program.cs`

---

# 19. Notes While Working

## For every Windows Form added
Visual Studio should generate:
- form `.cs`
- form `.Designer.cs`
- form `.resx`

## For every User Control added
Visual Studio should generate:
- control `.cs`
- control `.Designer.cs`
- sometimes control `.resx` depending on usage

## Recommended discipline
- [ ] Build after every 3–5 files
- [ ] Do not create all files first and wire later
- [ ] Keep each phase compiling before moving forward

---