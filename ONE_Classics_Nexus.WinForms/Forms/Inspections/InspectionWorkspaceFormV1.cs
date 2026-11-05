using System.Threading.Tasks;
using System;
using System.Windows.Forms;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.WinForms.Services; 

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections
{
    /// <summary>
    /// Deprecated: superseded by <see cref="InspectionWorkspaceForm"/>, which is wired to
    /// <c>IInspectionApiService</c>, <c>IAppraisalApiService</c>, and <c>IVehicleApiService</c>
    /// for live data binding, refreshing the Appraisal Summary and Vehicle Summary panels
    /// whenever an Inspection ID is selected, plus full save/complete/delete workflows.
    /// This form is retained temporarily for reference only, is not registered for DI
    /// creation, and should not be used for new development. It will be removed in a
    /// future cleanup pass.
    /// </summary>
    [Obsolete("Use InspectionWorkspaceForm instead. This form is deprecated and will be removed.")]
    public partial class InspectionWorkspaceFormV1 : Form
    {
        private readonly Guid _inspectionId;
        private InspectionDto? _inspection;
        private AppraisalDto? _appraisal;
        private VehicleDto? _vehicle;
        private readonly ApiClient _apiClient;

        public InspectionWorkspaceFormV1(ApiClient apiClient, Guid inspectionId)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _inspectionId = inspectionId;
            InitializeComponent();
            SetupCustomControls();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Load inspection data from API
                _inspection = await _apiClient.GetInspectionAsync(_inspectionId);
                _appraisal = await _apiClient.GetAppraisalAsync(_inspection.AppraisalId);
                _vehicle = await _apiClient.GetVehicleAsync(_appraisal.VehicleId);

                // Sample data for demonstration
                txtInspectionId.Text = "b2a3f68b-5b7a-4d7f-9a1d-3d4a66f10e21";
                cmbStatus.SelectedIndex = 1; // In Progress
                cmbInspectionType.SelectedIndex = 0; // Physical

                lblAppraisalId.Text = "e4f8c9a1-2b3c-4d5e-9f6a-1c2b3d4e56fa";
                lblAppraisalNumber.Text = "APP-2025-000123";
                lblAppraisalStatus.Text = "In Progress";
                lblAppraisalType.Text = "Market Value";

                lblVin.Text = "1GCDT14H1HZ123456";
                lblYearMakeModel.Text = "1970 Chevrolet Chevelle SS";
                lblBodyStyle.Text = "2 Door Coupe";
                lblDrivetrain.Text = "RWD (Rear)";
                lblEngineTransmission.Text = "454 V8 / 4-Speed Muncie";

                txtQuickNotes.Text = "Vehicle presents well overall. Minor surface rust on rear frame rails.\n" +
                    "Brake system needs service (front calipers sticking).\n" +
                    "Electrical system shows aged wiring in engine bay.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading inspection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}