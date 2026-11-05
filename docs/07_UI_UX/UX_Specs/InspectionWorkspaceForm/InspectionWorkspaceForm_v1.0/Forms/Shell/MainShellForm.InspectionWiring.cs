using ONE_Classics_Nexus.WinForms.Forms.Inspections;

namespace ONE_Classics_Nexus.WinForms.Forms.Shell;

public partial class MainShellForm
{
    private void OpenInspectionWorkspace()
    {
        var workspace = new InspectionWorkspaceForm();

        workspace.NewInspectionRequested += (_, _) =>
        {
            SetStatus("New inspection requested");
            // Open your NewInspection dialog or create workflow here.
        };

        workspace.ViewVehicleRequested += (_, e) =>
        {
            SetStatus($"Open vehicle {e.Id:D}");
            // LoadChildForm(new VehicleDetailForm(e.Id));
        };

        workspace.ViewSectionRequested += (_, e) =>
        {
            SetStatus($"Open inspection sections for {e.Id:D}");
            // Load your Sections & Items page here.
        };

        workspace.ViewAllMediaRequested += (_, e) =>
        {
            SetStatus($"Open media for inspection {e.Id:D}");
            // Load your Inspection Media page here.
        };

        LoadChildForm(workspace);
    }
}

/*
Replace the Inspections case in MainShellForm.NavButton_Click with:

case "Inspections":
    lblContext.Text = "Create, review, score, and complete vehicle inspections.";
    OpenInspectionWorkspace();
    SetStatus("Inspection workspace loaded");
    break;

For visual testing without the API:

var workspace = new InspectionWorkspaceForm(
    new MockInspectionWorkspaceApiClient(),
    disposeApiClient: true);
LoadChildForm(workspace);
*/
