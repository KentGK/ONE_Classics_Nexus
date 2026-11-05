using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.WinForms.Services;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections
{
    public partial class InspectionWorkspaceForm : Form
    {
        private readonly ApiClient _apiClient;
        private readonly Guid _inspectionId;
        private InspectionDto? _inspection;
        private AppraisalDto? _appraisal;
        private VehicleDto? _vehicle;

        public InspectionWorkspaceForm(ApiClient apiClient, Guid inspectionId)
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
                _inspection = await _apiClient.GetInspectionAsync(_inspectionId);
                _appraisal = await _apiClient.GetAppraisalAsync(_inspection.AppraisalId);
                _vehicle = await _apiClient.GetVehicleAsync(_appraisal.VehicleId);

                txtInspectionId.Text = _inspection.InspectionId.ToString();
                SetComboBoxSelection(cmbInspectionType, _inspection.InspectionType);

                // NOTE: InspectionDto does not currently expose a Status property.
                // Once the API/DTO is extended with an inspection Status, replace
                // this with SetComboBoxSelection(cmbStatus, _inspection.Status).
                SetComboBoxSelection(cmbStatus, _appraisal.Status);

                lblAppraisalId.Text = _appraisal.AppraisalId.ToString();
                lblAppraisalStatus.Text = _appraisal.Status;

                // NOTE: AppraisalDto does not currently expose AppraisalNumber or
                // AppraisalType. Extend the DTO/API response to populate these labels.
                lblAppraisalNumber.Text = string.Empty;
                lblAppraisalType.Text = string.Empty;

                lblVin.Text = _vehicle.Vin;
                lblYearMakeModel.Text = $"{_vehicle.Year} {_vehicle.Make} {_vehicle.Model}".Trim();
                lblBodyStyle.Text = _vehicle.BodyStyle ?? string.Empty;
                lblDrivetrain.Text = _vehicle.Drivetrain ?? string.Empty;
                lblEngineTransmission.Text = string.Join(" / ",
                    new[] { _vehicle.Engine, _vehicle.Transmission }.Where(s => !string.IsNullOrWhiteSpace(s)));

                // NOTE: InspectionDto does not currently expose SummaryNotes.
                // Extend the DTO/API response to populate quick notes.
                txtQuickNotes.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading inspection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void SetComboBoxSelection(ComboBox comboBox, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var index = comboBox.FindStringExact(value);
            if (index >= 0)
            {
                comboBox.SelectedIndex = index;
            }
        }

        private void InitializeComponent()
        {

        }
    }
}
