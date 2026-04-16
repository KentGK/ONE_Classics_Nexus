using ONE_Classics_Nexus.WinForms.Models.Ui;
using ONE_Classics_Nexus.WinForms.Services.Api;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ONE_Classics_Nexus.WinForms.Forms.Vehicles
{
    public partial class VehicleDetailForm : Form
    {
        private readonly VehicleApiClient _vehicleApiClient;
        private CancellationTokenSource? _cts;
        private Guid? _currentVehicleId;
        private bool _isDirty;

        public VehicleDetailForm()
        {
            InitializeComponent();

            var httpClient = BuildHttpClient();
            _vehicleApiClient = new VehicleApiClient(httpClient);

            ConfigureForm();
            BindLookupValues();
            SetEditMode(false);
            ClearForm();
        }

        public VehicleDetailForm(Guid vehicleId) : this()
        {
            _ = LoadVehicleAsync(vehicleId);
        }

        private static HttpClient BuildHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };
        }

        private void ConfigureForm()
        {
            txtVehicleId.ReadOnly = true;
            txtCreatedAtUtc.ReadOnly = true;
            txtUpdatedAtUtc.ReadOnly = true;

            txtVehicleId.TabStop = false;
            txtCreatedAtUtc.TabStop = false;
            txtUpdatedAtUtc.TabStop = false;

            numYear.Minimum = 1886;
            numYear.Maximum = 2100;
            numYear.Value = Math.Min(Math.Max(DateTime.Now.Year, 1886), 2100);

            numMileage.Minimum = 0;
            numMileage.Maximum = 10000000;

            HookDirtyTracking(this);
            lblStatus.Text = "Ready.";
        }

        private void BindLookupValues()
        {
            cboOdometerUnit.Items.Clear();
            cboOdometerUnit.Items.AddRange(new object[] { "miles", "km" });

            cboTitleStatus.Items.Clear();
            cboTitleStatus.Items.AddRange(new object[]
            {
                "", "clean", "rebuilt", "salvage", "bonded", "unknown"
            });

            cboAuthenticityProfile.Items.Clear();
            cboAuthenticityProfile.Items.AddRange(new object[]
            {
                "", "original", "restored", "survivor", "restomod", "custom", "tribute"
            });

            cboRestorationLevel.Items.Clear();
            cboRestorationLevel.Items.AddRange(new object[]
            {
                "", "none", "partial", "older_restoration", "recent_restoration", "show_quality"
            });

            cboOdometerUnit.SelectedIndex = 0;
        }

        private void HookDirtyTracking(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                switch (control)
                {
                    case TextBox textBox:
                        textBox.TextChanged += (_, _) => MarkDirty();
                        break;
                    case ComboBox comboBox:
                        comboBox.SelectedIndexChanged += (_, _) => MarkDirty();
                        comboBox.TextChanged += (_, _) => MarkDirty();
                        break;
                    case NumericUpDown numeric:
                        numeric.ValueChanged += (_, _) => MarkDirty();
                        break;
                }

                if (control.HasChildren)
                    HookDirtyTracking(control);
            }
        }

        private void MarkDirty()
        {
            _isDirty = true;
            UpdateButtonState();
        }

        private void SetDirty(bool value)
        {
            _isDirty = value;
            UpdateButtonState();
        }

        private void SetEditMode(bool hasExistingVehicle)
        {
            btnSave.Text = hasExistingVehicle ? "Save Changes" : "Create Vehicle";
            btnLoad.Enabled = true;
            btnDelete.Enabled = false; // enable when API DELETE exists
        }

        private void UpdateButtonState()
        {
            btnSave.Enabled = _isDirty;
        }

        private async Task LoadVehicleAsync(Guid vehicleId)
        {
            CancelPending();

            _cts = new CancellationTokenSource();

            try
            {
                SetBusy(true, "Loading vehicle...");
                var dto = await _vehicleApiClient.GetByIdAsync(vehicleId, _cts.Token);

                if (dto == null)
                {
                    MessageBox.Show(
                        "Vehicle not found.",
                        "Vehicle",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                BindToForm(dto);
                _currentVehicleId = dto.VehicleId;
                SetEditMode(true);
                SetDirty(false);
                SetBusy(false, $"Vehicle loaded: {dto.Year} {dto.Make} {dto.Model}");
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Load canceled.");
            }
            catch (Exception ex)
            {
                SetBusy(false, "Load failed.");
                ShowError(ex);
            }
        }

        private void BindToForm(VehicleResponseDto dto)
        {
            txtVehicleId.Text = dto.VehicleId == Guid.Empty ? string.Empty : dto.VehicleId.ToString();
            txtVin.Text = dto.Vin ?? string.Empty;
            numYear.Value = ClampYear(dto.Year);
            txtMake.Text = dto.Make ?? string.Empty;
            txtModel.Text = dto.Model ?? string.Empty;
            txtTrim.Text = dto.Trim ?? string.Empty;
            txtSeriesName.Text = dto.SeriesName ?? string.Empty;
            txtBodyStyle.Text = dto.BodyStyle ?? string.Empty;
            txtDrivetrain.Text = dto.Drivetrain ?? string.Empty;
            txtEngine.Text = dto.Engine ?? string.Empty;
            txtTransmission.Text = dto.Transmission ?? string.Empty;
            numMileage.Value = dto.Mileage.HasValue ? ClampMileage(dto.Mileage.Value) : 0;
            chkMileageUnknown.Checked = !dto.Mileage.HasValue;
            cboOdometerUnit.Text = string.IsNullOrWhiteSpace(dto.OdometerUnit) ? "miles" : dto.OdometerUnit;
            txtExteriorColor.Text = dto.ExteriorColor ?? string.Empty;
            txtInteriorColor.Text = dto.InteriorColor ?? string.Empty;
            cboTitleStatus.Text = dto.TitleStatus ?? string.Empty;
            cboAuthenticityProfile.Text = dto.AuthenticityProfile ?? string.Empty;
            cboRestorationLevel.Text = dto.RestorationLevel ?? string.Empty;
            txtOwnershipSummary.Text = dto.OwnershipSummary ?? string.Empty;
            txtNotes.Text = dto.Notes ?? string.Empty;
            txtCreatedAtUtc.Text = dto.CreatedAtUtc.ToString("u");
            txtUpdatedAtUtc.Text = dto.UpdatedAtUtc.ToString("u");
        }

        private CreateVehicleRequestDto BuildCreateRequest()
        {
            return new CreateVehicleRequestDto
            {
                Vin = txtVin.Text.Trim(),
                Year = (int)numYear.Value,
                Make = txtMake.Text.Trim(),
                Model = txtModel.Text.Trim(),
                Trim = txtTrim.Text.Trim(),
                SeriesName = txtSeriesName.Text.Trim(),
                BodyStyle = txtBodyStyle.Text.Trim(),
                Drivetrain = txtDrivetrain.Text.Trim(),
                Engine = txtEngine.Text.Trim(),
                Transmission = txtTransmission.Text.Trim(),
                Mileage = chkMileageUnknown.Checked ? null : (int)numMileage.Value,
                OdometerUnit = cboOdometerUnit.Text.Trim(),
                ExteriorColor = txtExteriorColor.Text.Trim(),
                InteriorColor = txtInteriorColor.Text.Trim(),
                TitleStatus = cboTitleStatus.Text.Trim(),
                AuthenticityProfile = cboAuthenticityProfile.Text.Trim(),
                RestorationLevel = cboRestorationLevel.Text.Trim(),
                OwnershipSummary = txtOwnershipSummary.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private UpdateVehicleRequestDto BuildUpdateRequest()
        {
            return new UpdateVehicleRequestDto
            {
                Vin = txtVin.Text.Trim(),
                Year = (int)numYear.Value,
                Make = txtMake.Text.Trim(),
                Model = txtModel.Text.Trim(),
                Trim = txtTrim.Text.Trim(),
                SeriesName = txtSeriesName.Text.Trim(),
                BodyStyle = txtBodyStyle.Text.Trim(),
                Drivetrain = txtDrivetrain.Text.Trim(),
                Engine = txtEngine.Text.Trim(),
                Transmission = txtTransmission.Text.Trim(),
                Mileage = chkMileageUnknown.Checked ? null : (int)numMileage.Value,
                OdometerUnit = cboOdometerUnit.Text.Trim(),
                ExteriorColor = txtExteriorColor.Text.Trim(),
                InteriorColor = txtInteriorColor.Text.Trim(),
                TitleStatus = cboTitleStatus.Text.Trim(),
                AuthenticityProfile = cboAuthenticityProfile.Text.Trim(),
                RestorationLevel = cboRestorationLevel.Text.Trim(),
                OwnershipSummary = txtOwnershipSummary.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private bool ValidateForm()
        {
            errorProvider.Clear();

            var isValid = true;

            if ((int)numYear.Value < 1886)
            {
                errorProvider.SetError(numYear, "Year is required.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtMake.Text))
            {
                errorProvider.SetError(txtMake, "Make is required.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                errorProvider.SetError(txtModel, "Model is required.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtVin.Text) && txtVin.Text.Trim().Length > 50)
            {
                errorProvider.SetError(txtVin, "VIN exceeds max supported length.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(cboOdometerUnit.Text))
            {
                errorProvider.SetError(cboOdometerUnit, "Odometer unit is required.");
                isValid = false;
            }

            return isValid;
        }

        private async Task SaveAsync()
        {
            if (!ValidateForm())
            {
                SetBusy(false, "Validation failed.");
                return;
            }

            CancelPending();
            _cts = new CancellationTokenSource();

            try
            {
                SetBusy(true, _currentVehicleId.HasValue ? "Saving vehicle..." : "Creating vehicle...");

                VehicleResponseDto saved;

                if (_currentVehicleId.HasValue)
                {
                    saved = await _vehicleApiClient.UpdateAsync(
                        _currentVehicleId.Value,
                        BuildUpdateRequest(),
                        _cts.Token);
                }
                else
                {
                    saved = await _vehicleApiClient.CreateAsync(
                        BuildCreateRequest(),
                        _cts.Token);
                }

                _currentVehicleId = saved.VehicleId;
                BindToForm(saved);
                SetEditMode(true);
                SetDirty(false);
                SetBusy(false, "Vehicle saved.");
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Save canceled.");
            }
            catch (Exception ex)
            {
                SetBusy(false, "Save failed.");
                ShowError(ex);
            }
        }

        private void ClearForm()
        {
            _currentVehicleId = null;

            txtVehicleId.Clear();
            txtVin.Clear();
            numYear.Value = ClampYear(DateTime.Now.Year);
            txtMake.Clear();
            txtModel.Clear();
            txtTrim.Clear();
            txtSeriesName.Clear();
            txtBodyStyle.Clear();
            txtDrivetrain.Clear();
            txtEngine.Clear();
            txtTransmission.Clear();
            numMileage.Value = 0;
            chkMileageUnknown.Checked = false;
            cboOdometerUnit.Text = "miles";
            txtExteriorColor.Clear();
            txtInteriorColor.Clear();
            cboTitleStatus.SelectedIndex = 0;
            cboAuthenticityProfile.SelectedIndex = 0;
            cboRestorationLevel.SelectedIndex = 0;
            txtOwnershipSummary.Clear();
            txtNotes.Clear();
            txtCreatedAtUtc.Clear();
            txtUpdatedAtUtc.Clear();
            errorProvider.Clear();

            SetEditMode(false);
            SetDirty(false);
            lblStatus.Text = "New vehicle entry.";
        }

        private void SetBusy(bool isBusy, string statusText)
        {
            lblStatus.Text = statusText;
            progressBar.Visible = isBusy;
            progressBar.Style = isBusy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;

            btnLoad.Enabled = !isBusy;
            btnSave.Enabled = !isBusy && _isDirty;
            btnNew.Enabled = !isBusy;
            btnDelete.Enabled = false;
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Vehicle Detail Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void CancelPending()
        {
            if (_cts == null)
                return;

            if (!_cts.IsCancellationRequested)
                _cts.Cancel();

            _cts.Dispose();
            _cts = null;
        }

        private static decimal ClampYear(int value)
        {
            if (value < 1886) return 1886;
            if (value > 2100) return 2100;
            return value;
        }

        private static decimal ClampMileage(int value)
        {
            if (value < 0) return 0;
            if (value > 10000000) return 10000000;
            return value;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (!Guid.TryParse(txtLookupVehicleId.Text.Trim(), out var vehicleId))
            {
                MessageBox.Show(
                    "Enter a valid Vehicle ID GUID to load.",
                    "Load Vehicle",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            await LoadVehicleAsync(vehicleId);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            await SaveAsync();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (_isDirty)
            {
                var result = MessageBox.Show(
                    "Discard unsaved changes?",
                    "New Vehicle",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Delete is intentionally disabled until the Vehicle DELETE API is approved and implemented.",
                "Delete Vehicle",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CancelPending();
            base.OnFormClosing(e);
        }
    }
}