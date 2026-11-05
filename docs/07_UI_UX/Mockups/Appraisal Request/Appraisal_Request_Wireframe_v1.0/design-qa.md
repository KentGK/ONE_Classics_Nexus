# Design QA — Appraisal Request Option 1

## Comparison Target

- Source visual truth: `/workspace/scratch/7adbddf7fc8b/generated_images/exec-bf94d079-c33d-40b0-8c6c-2af62fa66f3e.png`
- Browser-rendered implementation: `/workspace/scratch/appraisal-request-option1-final.jpg`
- Full-view comparison: `/workspace/scratch/7adbddf7fc8b/appraisal-request-wireframe-option1/design-qa-comparison.jpg`
- Focused comparison: `/workspace/scratch/7adbddf7fc8b/appraisal-request-wireframe-option1/design-qa-focused.jpg`
- Source dimensions: 1600 × 1000 pixels
- Implementation dimensions: 1363 × 936 pixels
- Browser CSS viewport: 1363 × 936
- Device pixel ratio: 1
- State: default Appraisal Request list with `AR-2026-00418` selected and Request Information active

The cloud browser did not expose a viewport-resize control, so the implementation was evaluated at its actual 1363 × 936 viewport. The source and implementation were normalized to equal display height in the full-view comparison. Focused list and form regions were separately cropped and normalized to compare hierarchy, spacing, controls, typography, and content without treating the intentional responsive changes as defects.

## Browser Verification

Primary interactions tested:

- Search filtering reduced the list to the matching Porsche request.
- Selecting `AR-2026-00417` updated the detail workspace.
- `Client and Intended Users` and `Request Information` tab navigation worked.
- `New Request` created a Draft in the prototype state.
- Submitting an incomplete new Draft produced the expected blocking message.
- The page reloaded to the canonical default state after interaction testing.

Console check:

- No application-origin errors or warnings were recorded.
- The browser reported repeated metadata errors from its own Chrome extension. These are outside the prototype origin and do not affect the application.

## Required Fidelity Surfaces

### Fonts and Typography

The implementation uses Segoe UI with Tahoma and Arial fallbacks, matching the WinForms-oriented source. Heading, label, table, and metadata weights preserve the same hierarchy. Small text remains legible at the narrower browser viewport. No actionable typography mismatch remains.

### Spacing and Layout Rhythm

The navy shell, left navigation, list/editor split, request toolbar, tab strip, two-column form, validation panel, and footer align with the source composition. At 1363 pixels, action labels collapse to icons while retaining accessible names, and the Updated list column hides to preserve the list hierarchy. These are intentional responsive adaptations. No persistent controls are clipped and the viewport has no document-level overflow.

### Colors and Visual Tokens

The locked Nexus navy `#0E2841`, primary blue actions, light-gray work surfaces, blue selection state, semantic status chips, green validation success, and amber warnings closely match the source. Contrast remains appropriate for the visible states.

### Image Quality and Asset Fidelity

The source contains no photography, illustration, texture, or custom raster asset. UI icons use Font Awesome rather than handcrafted SVG, CSS drawings, emoji, or placeholder glyphs. Icon scale and stroke density are consistent across the shell and workspace.

### Copy and Content

The implementation preserves the source labels and realistic request data while replacing noncanonical request states with the workflow states used by the Appraisal Request design: Draft, Submitted, Under Review, Accepted, Converted, Declined, Cancelled, and Withdrawn. Validation, row-version, intended-use, Appraisal Type, and scope language remains aligned with the workflow and sequence artifacts.

## Full-View Comparison Evidence

The combined full-view comparison confirms the same dominant hierarchy: navy application shell, selected Appraisal Requests navigation, left request register, right request editor, persistent validation results, and lifecycle footer. The implementation is taller and narrower because of the fixed cloud-browser viewport; responsive behavior preserves the source emphasis without losing core functionality.

## Focused Comparison Evidence

The focused comparison confirms:

- matching filter, search, result-summary, table, selection, status-chip, and pagination anatomy;
- matching five-tab editor structure;
- matching Client, Primary Contact, Intended Use, Effective Date, Value Definition, Appraisal Type, Priority, and Scope Summary control grouping;
- comparable label, input, divider, padding, and vertical-rhythm treatment.

## Comparison History

### Iteration 1

- Finding: **P1 — Request header crowding at 1363 pixels.** The action group overlapped the available request-heading space.
- Fix: Added the responsive action treatment at widths up to 1450 pixels. Save, Validate, and Submit retain icons and accessible names; More Actions retains its text label.
- Post-fix evidence: `implementation-v2` and final full-view capture show the complete request number, Draft status, row version, action controls, and metadata without overlap.

- Finding: **P2 — Request list horizontal overflow.** Dense columns introduced an internal horizontal scrollbar.
- Fix: Hid the Updated column at the narrow viewport, redistributed the remaining column widths, reduced table padding, and tightened status-chip sizing.
- Post-fix evidence: the final browser capture shows all required list columns without horizontal scrolling.

### Iteration 2

No actionable P0, P1, or P2 differences remain. The remaining differences are expected responsive adaptations caused by the narrower browser viewport:

- top product-area links collapse;
- three toolbar labels collapse to accessible icon buttons;
- the Updated list column hides;
- fewer request rows are visible above the fold.

## Follow-up Polish

- P3: Add a tooltip on hover for the responsive icon-only Save, Validate, and Submit buttons.
- P3: Add an optional draggable splitter between the request list and editor for production WinForms behavior.

## Final Result

final result: passed
