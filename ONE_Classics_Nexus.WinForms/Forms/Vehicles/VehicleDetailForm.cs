// ============================================================
// VehicleDetailForm.cs
// ============================================================

using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.WinForms.Services;

namespace ONE_Classics_Nexus.WinForms.Forms.Vehicles
{
    public partial class VehicleDetailForm : Form
    {
        private readonly IVehicleApiService _vehicleApiService;
        private CancellationTokenSource? _cts;
        private Guid? _currentVehicleId;
        private Guid _currentVehicleTypeId;
        private long _currentRowVersion;
        private bool _isDirty;
        private bool _isLoadingVehicleList;
        private bool _isChangingVehicleSelection;

        public VehicleDetailForm(IVehicleApiService vehicleApiService)
        {
            _vehicleApiService = vehicleApiService ?? throw new ArgumentNullException(nameof(vehicleApiService));

            InitializeComponent();
            ConfigureForm();
            BindLookupValues();
            SetEditMode(false);
            ClearForm();
            _ = LoadVehicleListAsync();
        }

        public VehicleDetailForm(IVehicleApiService vehicleApiService, Guid vehicleId) : this(vehicleApiService)
        {
            _ = LoadVehicleAsync(vehicleId);
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

            numOverallConditionScore.Minimum = 0;
            numOverallConditionScore.Maximum = 5;
            numOverallConditionScore.DecimalPlaces = 2;
            numOverallConditionScore.Increment = 0.25M;

            HookDirtyTracking(this);
            lblStatus.Text = "Ready.";
        }

        private void BindLookupValues()
        {
            cboOdometerUnit.Items.Clear();
            cboOdometerUnit.Items.AddRange(new[]
            {
                "miles",
                "kilometers",
                "hours",
                "unknown",
                "not_applicable"
            });

            cboTitleStatus.Items.Clear();
            cboTitleStatus.Items.AddRange(new[]
            {
                "",
                "Clean",
                "Salvage",
                "Rebuilt",
                "Flood",
                "Bonded",
                "Parts Only",
                "NoTitle",
                "Bill Of Sale",
                "Foreign Registration",
                "Unknown"
            });

            cboAuthenticityProfile.Items.Clear();
            cboAuthenticityProfile.Items.AddRange(new[]
            {
                "",
                "Unknown",
                "Original",
                "Restored",
                "Survivor",
                "Restomod",
                "Custom",
                "Tribute",
                "Industrial",
                "Industrial Standardized"
            });

            cboRestorationLevel.Items.Clear();
            cboRestorationLevel.Items.AddRange(new[]
            {
                "",
                "Unknown",
                "Unrestored",
                "Preserved",
                "Driver Restoration",
                "Older Restoration",
                "Full Restoration",
                "Concours Restoration",
                "Restomod",
                "Reconstruction"
            });

            cboDocumentationLevel.Items.Clear();
            cboDocumentationLevel.Items.AddRange(new[]
            {
                "",
                "Unknown",
                "Minimal",
                "Partial",
                "Good",
                "Extensive",
                "Exceptional"
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
                    case CheckBox checkBox:
                        checkBox.CheckedChanged += (_, _) => MarkDirty();
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
            btnDelete.Enabled = false; // DELETE endpoint not implemented in API yet
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

                System.Diagnostics.Debug.WriteLine($"Attempting to load vehicle: {vehicleId}");

                var dto = await _vehicleApiService.GetByIdAsync(vehicleId, _cts.Token);

                if (dto == null)
                {
                    MessageBox.Show(
                        $"Vehicle with ID '{vehicleId}' was not found in the database.\n\n" +
                        "Please verify the Vehicle ID is correct.",
                        "Vehicle Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    SetBusy(false, "Vehicle not found.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Vehicle loaded: {dto.Year} {dto.Make} {dto.Model}");

                BindToForm(dto);
                _currentVehicleId = dto.VehicleId;
                SelectVehicleInList(dto.VehicleId);
                SetEditMode(true);
                SetDirty(false);
                SetBusy(false, $"Vehicle loaded: {dto.Year} {dto.Make} {dto.Model}");
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Load canceled.");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Failed to get vehicle"))
            {
                SetBusy(false, "Load failed - API connection error.");
                MessageBox.Show(
                    $"Cannot connect to the API server.\n\n" +
                    $"Please ensure the API is running at: {_vehicleApiService.GetType().Name}\n\n" +
                    $"Error: {ex.InnerException?.Message ?? ex.Message}",
                    "API Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                SetBusy(false, "Load failed.");
                MessageBox.Show(
                    $"An unexpected error occurred:\n\n{ex.Message}\n\n" +
                    $"Stack Trace:\n{ex.StackTrace}",
                    "Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BindToForm(VehicleResponse dto)
        {
            _currentVehicleTypeId = dto.VehicleTypeId ?? Guid.Empty;
            _currentRowVersion = dto.RowVersion;

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
            cboDocumentationLevel.Text = dto.DocumentationLevel ?? string.Empty;
            numOverallConditionScore.Value = dto.OverallConditionScore.HasValue ? ClampConditionScore(dto.OverallConditionScore.Value) : 0;
            chkHasRedFlags.Checked = dto.HasRedFlags;
            txtOwnershipNotes.Text = dto.OwnershipNotes ?? string.Empty;
            txtNotes.Text = dto.Notes ?? string.Empty;
            txtCreatedAtUtc.Text = dto.CreatedAtUtc.ToString("u");
            txtUpdatedAtUtc.Text = dto.UpdatedAtUtc == default ? string.Empty : dto.UpdatedAtUtc.ToString("u");
        }

        private CreateVehicleRequest BuildCreateRequest()
        {
            return new CreateVehicleRequest
            {
                VehicleTypeId = Guid.Empty, // TODO: Add VehicleType selection UI
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
                DocumentationLevel = string.IsNullOrWhiteSpace(cboDocumentationLevel.Text) ? null : cboDocumentationLevel.Text.Trim(),
                OverallConditionScore = numOverallConditionScore.Value == 0 ? null : (decimal?)numOverallConditionScore.Value,
                HasRedFlags = chkHasRedFlags.Checked,
                OwnershipNotes = txtOwnershipNotes.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private UpdateVehicleRequest BuildUpdateRequest()
        {
            return new UpdateVehicleRequest
            {
                RowVersion = _currentRowVersion,
                VehicleTypeId = _currentVehicleTypeId,
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
                DocumentationLevel = string.IsNullOrWhiteSpace(cboDocumentationLevel.Text) ? null : cboDocumentationLevel.Text.Trim(),
                OverallConditionScore = numOverallConditionScore.Value == 0 ? null : (decimal?)numOverallConditionScore.Value,
                HasRedFlags = chkHasRedFlags.Checked,
                OwnershipNotes = txtOwnershipNotes.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private bool ValidateForm()
        {
            errorProvider.Clear();
            var isValid = true;

            if (string.IsNullOrWhiteSpace(txtVin.Text))
            {
                errorProvider.SetError(txtVin, "VIN is required.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtVin.Text) && txtVin.Text.Trim().Length > 50)
            {
                errorProvider.SetError(txtVin, "VIN exceeds max supported length.");
                isValid = false;
            }

            if ((int)numYear.Value < 1886 || (int)numYear.Value > DateTime.UtcNow.Year + 1)
            {
                errorProvider.SetError(numYear, "Year is outside the supported range.");
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

            if (string.IsNullOrWhiteSpace(cboOdometerUnit.Text))
            {
                errorProvider.SetError(cboOdometerUnit, "Odometer unit is required.");
                isValid = false;
            }

            if (numOverallConditionScore.Value != 0 && (numOverallConditionScore.Value < 0 || numOverallConditionScore.Value > 5))
            {
                errorProvider.SetError(numOverallConditionScore, "Condition score must be between 0 and 5.");
                isValid = false;
            }

            return isValid;
        }

        private async Task SaveAsync()
        {
            if (!ValidateForm())
            {
                lblStatus.Text = "Validation failed.";  // FIX: Don't call SetBusy here
                return;
            }

            CancelPending();
            _cts = new CancellationTokenSource();

            try
            {
                SetBusy(true, _currentVehicleId.HasValue ? "Saving vehicle..." : "Creating vehicle...");

                if (_currentVehicleId.HasValue)
                {
                    var updated = await _vehicleApiService.UpdateAsync(
                        _currentVehicleId.Value,
                        BuildUpdateRequest(),
                        _cts.Token);

                    if (updated)
                    {
                        var refreshed = await _vehicleApiService.GetByIdAsync(_currentVehicleId.Value, _cts.Token);
                        if (refreshed != null)
                        {
                            BindToForm(refreshed);
                        }

                        SetEditMode(true);
                        SetDirty(false);
                        SetBusy(false, "Vehicle saved.");
                    }
                    else
                    {
                        MessageBox.Show("Vehicle not found.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SetBusy(false, "Update failed.");
                    }
                }
                else
                {
                    var saved = await _vehicleApiService.CreateAsync(
                        BuildCreateRequest(),
                        _cts.Token);

                    _currentVehicleId = saved.VehicleId;
                    BindToForm(saved);
                    SetEditMode(true);
                    SetDirty(false);
                    await LoadVehicleListAsync();
                    SelectVehicleInList(saved.VehicleId);
                    SetBusy(false, "Vehicle created.");
                }
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Save canceled.");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("409") || ex.Message.Contains("already exists"))
            {
                SetBusy(false, "Save failed.");
                MessageBox.Show(
                    "A vehicle with this VIN already exists.",
                    "Duplicate VIN",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
            _currentVehicleTypeId = Guid.Empty;
            _currentRowVersion = 0;

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
            cboDocumentationLevel.SelectedIndex = 0;
            numOverallConditionScore.Value = 0;
            chkHasRedFlags.Checked = false;
            txtOwnershipNotes.Clear();
            txtNotes.Clear();
            txtCreatedAtUtc.Clear();
            txtUpdatedAtUtc.Clear();
            errorProvider.Clear();

            SetEditMode(false);
            SetDirty(false);
            lblStatus.Text = "New vehicle entry.";
        }

        private void SetBusy(
            bool isBusy,
            string statusText)
        {
            lblStatus.Text = statusText;
            progressBar.Visible = isBusy;
            progressBar.Style = isBusy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;

            cboSelectVehicle.Enabled = !isBusy;
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

        private static decimal ClampConditionScore(decimal value)
        {
            if (value < 0) return 0;
            if (value > 5) return 5;
            return value;
        }

        private async Task LoadVehicleListAsync()
        {
            _isLoadingVehicleList = true;

            try
            {
                var vehicles = await _vehicleApiService.ListAsync();
                var items = vehicles
                    .Select(vehicle => new VehicleSelectionItem(vehicle))
                    .OrderBy(item => item.DisplayText)
                    .ToList();

                cboSelectVehicle.DataSource = items;
                cboSelectVehicle.DisplayMember = nameof(VehicleSelectionItem.DisplayText);
                cboSelectVehicle.ValueMember = nameof(VehicleSelectionItem.VehicleId);

                if (_currentVehicleId.HasValue)
                    SelectVehicleInList(_currentVehicleId.Value);

                lblStatus.Text = items.Count == 0 ? "No vehicles found." : "Ready.";
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = "Vehicle list load canceled.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Vehicle list load failed.";
                ShowError(ex);
            }
            finally
            {
                _isLoadingVehicleList = false;
            }
        }

        private async void cboSelectVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingVehicleList || _isChangingVehicleSelection || cboSelectVehicle.SelectedValue is not Guid vehicleId)
                return;

            if (_isDirty)
            {
                var result = MessageBox.Show(
                    "Discard unsaved changes?",
                    "Load Vehicle",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    if (_currentVehicleId.HasValue)
                        SelectVehicleInList(_currentVehicleId.Value);
                    return;
                }
            }

            await LoadVehicleAsync(vehicleId);
        }

        private void SelectVehicleInList(Guid vehicleId)
        {
            if (cboSelectVehicle.DataSource == null)
                return;

            _isChangingVehicleSelection = true;
            try
            {
                cboSelectVehicle.SelectedValue = vehicleId;
            }
            finally
            {
                _isChangingVehicleSelection = false;
            }
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
                "Delete functionality is not yet available. API endpoint not implemented.",
                "Feature Not Available",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CancelPending();
            base.OnFormClosing(e);
        }

        private sealed class VehicleSelectionItem
        {
            public VehicleSelectionItem(VehicleResponse vehicle)
            {
                VehicleId = vehicle.VehicleId;
                DisplayText = $"{vehicle.Year} {vehicle.Make} {vehicle.Model} {vehicle.VehicleId.ToString()[..7]}";
            }

            public Guid VehicleId { get; }
            public string DisplayText { get; }
        }

        private void txtVehicleId_TextChanged(object sender, EventArgs e)
        {

        }

        private void tblAttributes_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}