// ============================================================
// InspectionWorkspaceForm.cs
// ============================================================

using ONE_Classics_Nexus.API.Contracts.Appraisals;
using ONE_Classics_Nexus.API.Contracts.Inspections;
using ONE_Classics_Nexus.API.Contracts.Vehicles;
using ONE_Classics_Nexus.WinForms.Services;
using ONE_Classics_Nexus.WinForms.Theming;
using System.ComponentModel;
using System.Globalization;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections;

public partial class InspectionWorkspaceForm : Form
{
    private readonly IInspectionApiService _apiClient;
    private readonly IAppraisalApiService _appraisalApiService;
    private readonly IVehicleApiService _vehicleApiService;
    private readonly BindingList<InspectionResponse> _inspectionList = new();
    private readonly BindingList<ConditionScoreRowViewModel> _scoreRows = new();
    private readonly BindingList<SectionSummaryRowViewModel> _sectionRows = new();
    private readonly BindingList<RedFlagRowViewModel> _redFlagRows = new();

    private CancellationTokenSource? _operationCts;
    private InspectionResponse? _current;
    private AppraisalResponse? _currentAppraisal;
    private VehicleResponse? _currentVehicle;
    private bool _binding;
    private bool _dirty;
    private bool _selectionInitialized;
    private Task? _initializationTask;

    public InspectionWorkspaceForm(
        IInspectionApiService apiClient,
        IAppraisalApiService appraisalApiService,
        IVehicleApiService vehicleApiService)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _appraisalApiService = appraisalApiService ?? throw new ArgumentNullException(nameof(appraisalApiService));
        _vehicleApiService = vehicleApiService ?? throw new ArgumentNullException(nameof(vehicleApiService));

        InitializeComponent();
        BuildHeader();
        BuildSelector();
        BuildSummary();
        BuildTabs();

        BuildFooter();
        ApplyTheme();
        ConfigureControls();
        WireEvents();
    }

    public Guid? InspectionId => _current?.InspectionId;

    public event EventHandler? NewInspectionRequested;
    public event EventHandler<GuidEventArgs>? ViewVehicleRequested;
    public event EventHandler<GuidEventArgs>? ViewSectionRequested;
    public event EventHandler<GuidEventArgs>? ViewAllMediaRequested;

    public async Task LoadInspectionAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        var pendingInitialization = _initializationTask;
        if (pendingInitialization is not null &&
            pendingInitialization != Task.CompletedTask &&
            !pendingInitialization.IsCompleted)
        {
            try
            {
                await pendingInitialization;
            }
            catch
            {
                // Initialization failures are already surfaced via status messages.
            }
        }

        await LoadInspectionCoreAsync(inspectionId, cancellationToken);
    }

    private async Task LoadInspectionCoreAsync(
        Guid inspectionId,
        CancellationToken cancellationToken)
    {
        CancelOperation();
        _operationCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            SetBusy(true, "Loading inspection...");
            var inspection = await _apiClient.GetByIdAsync(
                inspectionId,
                _operationCts.Token);

            if (inspection is null)
            {
                SetStatus("Inspection not found.", UiStatus.Warning);
                return;
            }

            AppraisalResponse? appraisal = null;
            VehicleResponse? vehicle = null;

            if (inspection.AppraisalId != Guid.Empty)
            {
                appraisal = await _appraisalApiService.GetByIdAsync(
                    inspection.AppraisalId,
                    _operationCts.Token);

                if (appraisal is not null && appraisal.VehicleId != Guid.Empty)
                {
                    vehicle = await _vehicleApiService.GetByIdAsync(
                        appraisal.VehicleId,
                        _operationCts.Token);
                }
            }

            BindInspection(inspection, appraisal, vehicle);
            SelectInspectionInDropdown(inspectionId);
            SetStatus("Inspection loaded.", UiStatus.Success);
        }
        catch (OperationCanceledException)
        {
            SetStatus("Inspection load cancelled.", UiStatus.Info);
        }
        catch (Exception ex)
        {
            SetStatus($"Unable to load inspection: {ex.Message}", UiStatus.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task InitializeAsync()
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        CancelOperation();
        _operationCts = new CancellationTokenSource();

        try
        {
            SetBusy(true, "Loading inspections...");
            var inspections = await _apiClient.GetAllAsync(_operationCts.Token);

            _binding = true;
            _inspectionList.RaiseListChangedEvents = false;
            _inspectionList.Clear();

            foreach (var inspection in inspections.OrderByDescending(x => x.UpdatedAtUtc))
                _inspectionList.Add(inspection);

            _inspectionList.RaiseListChangedEvents = true;
            _inspectionList.ResetBindings();
            _binding = false;

            if (_inspectionList.Count == 0)
            {
                ClearWorkspace();
                SetStatus("No inspections are available.", UiStatus.Info);
                return;
            }

            _selectionInitialized = true;
            cmbInspectionId.SelectedIndex = 0;

            if (cmbInspectionId.SelectedItem is InspectionResponse selected)
                await LoadInspectionCoreAsync(selected.InspectionId, CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            SetStatus("Inspection list load cancelled.", UiStatus.Info);
        }
        catch (Exception ex)
        {
            SetStatus($"Unable to load inspections: {ex.Message}", UiStatus.Error);
        }
        finally
        {
            _binding = false;
            SetBusy(false);
        }
    }

    private async Task SaveDraftAsync()
    {
        if (_current is null)
        {
            SetStatus("Select an inspection before saving.", UiStatus.Warning);
            return;
        }

        if (!ValidateHeader() || !ValidateScores(requireAllTwelve: false))
            return;

        await SaveWorkspaceAsync(markCompleted: false);
    }

    private async Task CompleteInspectionAsync()
    {
        if (_current is null)
        {
            SetStatus("Select an inspection before completing it.", UiStatus.Warning);
            return;
        }

        if (!ValidateHeader() || !ValidateScores(requireAllTwelve: true))
            return;

        var totalWeight = _scoreRows.Sum(x => x.Weight);
        if (Math.Abs(totalWeight - 1.0000m) > 0.0001m)
        {
            SetStatus(
                $"Condition-score weights total {totalWeight:0.0000}; completion requires 1.0000.",
                UiStatus.Warning);
            return;
        }

        var confirmed = MessageBox.Show(
            this,
            "Complete this inspection and make it available for valuation?",
            "Complete Inspection",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmed != DialogResult.Yes)
            return;

        await SaveWorkspaceAsync(markCompleted: true);
    }

    private async Task SaveWorkspaceAsync(bool markCompleted)
    {
        if (_current is null)
            return;

        CancelOperation();
        _operationCts = new CancellationTokenSource();

        try
        {
            SetBusy(true, markCompleted ? "Completing inspection..." : "Saving inspection...");

            var updateRequest = new UpdateInspectionRequest(
                InspectionTemplateId: _current.InspectionTemplateId,
                InspectedByUserId: _current.InspectedByUserId,
                InspectionType: NullIfWhiteSpace(cmbInspectionType.Text) ?? "Standard",
                Status: markCompleted
                    ? "Completed"
                    : NullIfWhiteSpace(cmbStatus.Text) ?? "Draft",
                InspectionLocation: NullIfWhiteSpace(txtLocation.Text),
                InspectedOnUtc: dtpInspectedOn.Checked
                    ? DateTime.SpecifyKind(dtpInspectedOn.Value, DateTimeKind.Local).ToUniversalTime()
                    : null,
                CompletionPercentage: _current.CompletionPercentage,
                OverallConditionScore: _current.OverallConditionScore,
                HasRedFlags: _scoreRows.Any(x => x.IsRedFlag),
                SummaryNotes: NullIfWhiteSpace(txtQuickNotes.Text),
                RowVersion: _current.RowVersion);

            var scoreRequest = new SaveInspectionScoresRequest(
                _scoreRows
                    .OrderBy(x => x.SystemNumber)
                    .Select(x => new InspectionScoreDto(
                        x.SystemNumber,
                        x.SystemKey,
                        x.Score0To5,
                        x.Weight,
                        x.IsRedFlag,
                        x.Notes))
                    .ToList());

            var updated = await _apiClient.UpdateAsync(
                _current.InspectionId,
                updateRequest,
                _operationCts.Token);

            if (!updated)
            {
                SetStatus("Inspection was not found while saving.", UiStatus.Warning);
                return;
            }

            await _apiClient.SaveScoresAsync(
                _current.InspectionId,
                scoreRequest,
                _operationCts.Token);

            await LoadInspectionCoreAsync(_current.InspectionId, _operationCts.Token);
            _dirty = false;
            SetDirtyState(false);

            SetStatus(
                markCompleted ? "Inspection completed." : "Draft saved.",
                UiStatus.Success);
        }
        catch (OperationCanceledException)
        {
            SetStatus("Save cancelled.", UiStatus.Info);
        }
        catch (Exception ex)
        {
            SetStatus($"Unable to save inspection: {ex.Message}", UiStatus.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task DeleteInspectionAsync()
    {
        if (_current is null)
        {
            SetStatus("Select an inspection before deleting.", UiStatus.Warning);
            return;
        }

        var message = string.Equals(_current.Status, "Completed", StringComparison.OrdinalIgnoreCase)
            ? "Completed inspections are status-deleted, not physically removed. Continue?"
            : "Delete this inspection?";

        if (MessageBox.Show(
                this,
                message,
                "Delete Inspection",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        CancelOperation();
        _operationCts = new CancellationTokenSource();

        try
        {
            SetBusy(true, "Deleting inspection...");
            await _apiClient.DeleteAsync(_current.InspectionId, _operationCts.Token);
            SetStatus("Inspection deleted.", UiStatus.Success);
            _initializationTask = InitializeAsync();
            await _initializationTask;
        }
        catch (OperationCanceledException)
        {
            SetStatus("Delete cancelled.", UiStatus.Info);
        }
        catch (Exception ex)
        {
            SetStatus($"Unable to delete inspection: {ex.Message}", UiStatus.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void BindInspection(
        InspectionResponse inspection,
        AppraisalResponse? appraisal,
        VehicleResponse? vehicle)
    {
        _binding = true;
        _current = inspection;
        _currentAppraisal = appraisal;
        _currentVehicle = vehicle;

        cmbStatus.Text = inspection.Status;
        cmbInspectionType.Text = inspection.InspectionType;
        cmbTemplate.Text = inspection.InspectionTemplateId.ToString();
        txtLocation.Text = inspection.InspectionLocation ?? string.Empty;
        cmbInspectedBy.Text = inspection.InspectedByUserId?.ToString() ?? string.Empty;

        dtpInspectedOn.Checked = inspection.InspectedOnUtc.HasValue;
        if (inspection.InspectedOnUtc.HasValue)
            dtpInspectedOn.Value = inspection.InspectedOnUtc.Value.ToLocalTime();

        BindScores(inspection.Scores);
        BindSections(Array.Empty<SectionSummaryRowViewModel>());
        BindRedFlags(Array.Empty<RedFlagRowViewModel>());
        BindMedia();
        BindAppraisalSummary(appraisal);
        BindVehicleSummary(vehicle);

        txtQuickNotes.Text = inspection.SummaryNotes ?? string.Empty;
        lblFooterInspectionId.Text = $"Inspection ID: {inspection.InspectionId:D}";
        lblFooterCreated.Text =
            $"Created: {inspection.CreatedAtUtc.ToLocalTime():g}";
        lblFooterUpdated.Text =
            $"Updated: {inspection.UpdatedAtUtc.ToLocalTime():g}";

        UpdateOverviewMetrics();
        UpdateChecklist();

        _dirty = false;
        SetDirtyState(false);
        _binding = false;
    }

    private void BindAppraisalSummary(AppraisalResponse? appraisal)
    {
        if (appraisal is null)
        {
            lblAppraisalIdValue.Text = string.Empty;
            lblAppraisalNumberValue.Text = string.Empty;
            lblAppraisalStatusValue.Text = string.Empty;
            lblAppraisalTypeValue.Text = string.Empty;
            lblEffectiveDateValue.Text = string.Empty;
            lblClientValue.Text = string.Empty;
            return;
        }

        lblAppraisalIdValue.Text = appraisal.AppraisalId.ToString();
        lblAppraisalNumberValue.Text = appraisal.AppraisalNumber ?? string.Empty;
        lblAppraisalStatusValue.Text = appraisal.Status;
        lblAppraisalTypeValue.Text = appraisal.AppraisalType ?? string.Empty;
        lblEffectiveDateValue.Text = appraisal.EffectiveDateUtc.HasValue
            ? appraisal.EffectiveDateUtc.Value.ToLocalTime().ToString("d", CultureInfo.CurrentCulture)
            : string.Empty;
        lblClientValue.Text = appraisal.ClientId?.ToString() ?? string.Empty;
    }

    private void BindVehicleSummary(VehicleResponse? vehicle)
    {
        if (vehicle is null)
        {
            lblVinValue.Text = string.Empty;
            lblYearMakeModelValue.Text = string.Empty;
            lblBodyStyleValue.Text = string.Empty;
            lblDrivetrainValue.Text = string.Empty;
            lblEngineTransmissionValue.Text = string.Empty;
            lblMileageValue.Text = string.Empty;
            return;
        }

        lblVinValue.Text = vehicle.Vin ?? string.Empty;
        lblYearMakeModelValue.Text = $"{vehicle.Year} {vehicle.Make} {vehicle.Model}".Trim();
        lblBodyStyleValue.Text = vehicle.BodyStyle ?? string.Empty;
        lblDrivetrainValue.Text = vehicle.Drivetrain ?? string.Empty;
        lblEngineTransmissionValue.Text = string.Join(
            " / ",
            new[] { vehicle.Engine, vehicle.Transmission }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        lblMileageValue.Text = vehicle.Mileage.HasValue
            ? $"{vehicle.Mileage.Value:N0} {vehicle.OdometerUnit ?? "miles"}"
            : string.Empty;
    }


    private void BindScores(IReadOnlyCollection<InspectionScoreDto> scores)
    {
        var bySystem = scores.ToDictionary(x => x.SystemNumber);
        _scoreRows.RaiseListChangedEvents = false;
        _scoreRows.Clear();

        foreach (var system in CanonicalInspectionSystems.All)
        {
            bySystem.TryGetValue(system.SystemNumber, out var existing);

            _scoreRows.Add(new ConditionScoreRowViewModel
            {
                SystemNumber = system.SystemNumber,
                SystemName = system.SystemName,
                SystemKey = existing?.SystemKey ?? system.SystemKey,
                Score0To5 = existing?.Score0To5 ?? 0m,
                Weight = existing?.Weight ?? system.StandardWeight,
                IsRedFlag = existing?.IsRedFlag ?? false,
                Notes = existing?.Notes
            });
        }

        _scoreRows.RaiseListChangedEvents = true;
        _scoreRows.ResetBindings();
    }

    private void BindSections(IReadOnlyCollection<SectionSummaryRowViewModel> sections)
    {
        _sectionRows.RaiseListChangedEvents = false;
        _sectionRows.Clear();

        if (sections.Count > 0)
        {
            foreach (var section in sections.OrderBy(x => x.SystemNumber))
                _sectionRows.Add(section);
        }
        else
        {
            foreach (var score in _scoreRows.OrderBy(x => x.SystemNumber))
            {
                _sectionRows.Add(new SectionSummaryRowViewModel
                {
                    SystemNumber = score.SystemNumber,
                    SectionName = score.SystemName,
                    Score0To5 = score.Score0To5,
                    ItemsScoredDisplay = "—",
                    RedFlagCount = score.IsRedFlag ? 1 : 0
                });
            }
        }

        _sectionRows.RaiseListChangedEvents = true;
        _sectionRows.ResetBindings();
    }

    private void BindRedFlags(IReadOnlyCollection<RedFlagRowViewModel> flags)
    {
        _redFlagRows.RaiseListChangedEvents = false;
        _redFlagRows.Clear();

        if (flags.Count > 0)
        {
            foreach (var flag in flags)
                _redFlagRows.Add(flag);
        }
        else
        {
            foreach (var score in _scoreRows.Where(x => x.IsRedFlag))
            {
                _redFlagRows.Add(new RedFlagRowViewModel
                {
                    System = $"{score.SystemNumber}. {score.SystemName}",
                    Severity = "Review",
                    Flag = "System Red Flag",
                    Item = score.SystemName,
                    Notes = score.Notes
                });
            }
        }

        _redFlagRows.RaiseListChangedEvents = true;
        _redFlagRows.ResetBindings();
    }

    private void BindMedia()
    {
        lblPhotoCount.Text = "0";
        lblVideoCount.Text = "0";
        lblDocumentCount.Text = "0";
        lblOtherFileCount.Text = "0";
    }

    private void UpdateOverviewMetrics()
    {
        var weightedScore0To5 = _scoreRows.Sum(x => x.Score0To5 * x.Weight);
        var conditionIndex0To100 = Math.Round(weightedScore0To5 * 20m, 2);
        var redFlags = _scoreRows.Count(x => x.IsRedFlag);

        var progress = _current?.CompletionPercentage ?? CalculateScoreCompletion();
        var scoredCount = _scoreRows.Count(x => x.Score0To5 > 0m);
        var totalItems = _scoreRows.Count;

        lblConditionIndexValue.Text = $"{conditionIndex0To100:0.00} / 100";
        lblWeightedScoreValue.Text = $"Weighted score: {weightedScore0To5:0.00} / 5.00";
        lblTotalRedFlagsValue.Text = redFlags.ToString(CultureInfo.InvariantCulture);
        lblItemsScoredValue.Text = $"{scoredCount} / {totalItems}";
        lblProgressValue.Text = $"{progress:0}%";
        lblProgressDetail.Text = $"{scoredCount} of {totalItems} items scored";
        progressInspection.Value = Math.Clamp((int)Math.Round(progress), 0, 100);

        lblSelectorProgress.Text = $"Progress {progress:0}%";
        lblSelectorProgressDetail.Text = $"{scoredCount} / {totalItems} items scored";
    }

    private decimal CalculateScoreCompletion()
    {
        if (_scoreRows.Count == 0)
            return 0m;

        return Math.Round(
            _scoreRows.Count(x => x.Score0To5 > 0m) / (decimal)_scoreRows.Count * 100m,
            2);
    }

    private void UpdateChecklist()
    {
        var allSystemsScored = _scoreRows.Count == 12 && _scoreRows.All(x => x.Score0To5 >= 0m);
        var redFlagsReviewed = _redFlagRows.Count == 0 ||
                               _redFlagRows.All(x => !string.IsNullOrWhiteSpace(x.Severity));
        var mediaAdded = (int.TryParse(lblPhotoCount.Text, out var photos) && photos > 0) ||
                         (int.TryParse(lblVideoCount.Text, out var videos) && videos > 0) ||
                         (int.TryParse(lblDocumentCount.Text, out var documents) && documents > 0);

        checklist.SetItemChecked(0, allSystemsScored);
        checklist.SetItemChecked(1, redFlagsReviewed);
        checklist.SetItemChecked(2, mediaAdded);
        checklist.SetItemChecked(3, !string.IsNullOrWhiteSpace(txtQuickNotes.Text));
        checklist.SetItemChecked(4, string.Equals(_current?.Status, "Completed", StringComparison.OrdinalIgnoreCase));
    }

    private bool ValidateHeader()
    {
        errorProvider.Clear();
        var valid = true;

        if (string.IsNullOrWhiteSpace(cmbStatus.Text))
        {
            errorProvider.SetError(cmbStatus, "Status is required.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(cmbInspectionType.Text))
        {
            errorProvider.SetError(cmbInspectionType, "Inspection type is required.");
            valid = false;
        }

        if (_current?.InspectionTemplateId == Guid.Empty)
        {
            errorProvider.SetError(cmbTemplate, "Inspection template is required.");
            valid = false;
        }

        if (!valid)
            SetStatus("Correct the highlighted inspection fields.", UiStatus.Warning);

        return valid;
    }

    private bool ValidateScores(bool requireAllTwelve)
    {
        gridScores.EndEdit();

        if (_scoreRows.Any(x => x.Score0To5 < 0m || x.Score0To5 > 5m))
        {
            SetStatus("Every condition score must be between 0.00 and 5.00.", UiStatus.Warning);
            return false;
        }

        if (_scoreRows.Any(x => x.Weight < 0m))
        {
            SetStatus("Condition-score weights cannot be negative.", UiStatus.Warning);
            return false;
        }

        if (requireAllTwelve && _scoreRows.Select(x => x.SystemNumber).Distinct().Count() != 12)
        {
            SetStatus("Exactly 12 canonical system scores are required for completion.", UiStatus.Warning);
            return false;
        }

        return true;
    }

    private void ConfigureControls()
    {
        cmbInspectionId.DataSource = _inspectionList;
        cmbInspectionId.DisplayMember = nameof(InspectionResponse.DisplayLabel);
        cmbInspectionId.ValueMember = nameof(InspectionResponse.InspectionId);
        cmbInspectionId.DropDownStyle = ComboBoxStyle.DropDown;          // set first
        cmbInspectionId.AutoCompleteSource = AutoCompleteSource.ListItems;
        cmbInspectionId.AutoCompleteMode = AutoCompleteMode.SuggestAppend; // set last
        cmbInspectionId.MaxDropDownItems = 12;

        cmbStatus.Items.AddRange(new object[]
        {
            "Draft", "InProgress", "ReadyForReview", "Completed", "Deleted"
        });

        cmbInspectionType.Items.AddRange(new object[]
        {
            "Standard", "Custom", "VintageClassic", "Physical", "Remote"
        });

        gridScores.AutoGenerateColumns = false;
        gridScores.DataSource = _scoreRows;
        gridScores.Columns.Clear();
        gridScores.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(ConditionScoreRowViewModel.SystemNumber),
            HeaderText = "System #",
            Width = 62,
            ReadOnly = true
        });
        gridScores.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(ConditionScoreRowViewModel.SystemName),
            HeaderText = "System Name",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            FillWeight = 185,
            ReadOnly = true
        });
        gridScores.Columns.Add(new DataGridViewComboBoxColumn
        {
            DataPropertyName = nameof(ConditionScoreRowViewModel.Score0To5),
            HeaderText = "Score (0-5)",
            Width = 92,
            DataSource = BuildScoreOptions()
        });
        gridScores.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(ConditionScoreRowViewModel.Weight),
            HeaderText = "Weight",
            Width = 72,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "0.00" },
            ReadOnly = true
        });
        gridScores.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(ConditionScoreRowViewModel.WeightedScore),
            HeaderText = "Weighted",
            Width = 82,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "0.000" },
            ReadOnly = true
        });
        gridScores.Columns.Add(new DataGridViewCheckBoxColumn
        {
            DataPropertyName = nameof(ConditionScoreRowViewModel.IsRedFlag),
            HeaderText = "Red Flag",
            Width = 66
        });

        gridSections.AutoGenerateColumns = false;
        gridSections.DataSource = _sectionRows;
        gridSections.Columns.Clear();
        gridSections.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SectionSummaryRowViewModel.SectionDisplay),
            HeaderText = "Section",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            FillWeight = 170,
            ReadOnly = true
        });
        gridSections.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SectionSummaryRowViewModel.Score0To5),
            HeaderText = "Score",
            Width = 62,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "0.00" },
            ReadOnly = true
        });
        gridSections.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SectionSummaryRowViewModel.ItemsScoredDisplay),
            HeaderText = "Items",
            Width = 70,
            ReadOnly = true
        });
        gridSections.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SectionSummaryRowViewModel.RedFlagCount),
            HeaderText = "Flags",
            Width = 55,
            ReadOnly = true
        });
        gridSections.Columns.Add(new DataGridViewButtonColumn
        {
            HeaderText = "Action",
            Text = "View",
            UseColumnTextForButtonValue = true,
            Width = 62
        });

        gridRedFlags.AutoGenerateColumns = false;
        gridRedFlags.DataSource = _redFlagRows;
        gridRedFlags.Columns.Clear();
        gridRedFlags.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(RedFlagRowViewModel.System),
            HeaderText = "System",
            Width = 135,
            ReadOnly = true
        });
        gridRedFlags.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(RedFlagRowViewModel.Severity),
            HeaderText = "Severity",
            Width = 75,
            ReadOnly = true
        });
        gridRedFlags.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(RedFlagRowViewModel.Flag),
            HeaderText = "Flag",
            Width = 125,
            ReadOnly = true
        });
        gridRedFlags.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(RedFlagRowViewModel.Item),
            HeaderText = "Item",
            Width = 125,
            ReadOnly = true
        });
        gridRedFlags.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(RedFlagRowViewModel.Notes),
            HeaderText = "Notes",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = true
        });

        foreach (var grid in new[] { gridScores, gridSections, gridRedFlags })
        {
            NexusControlStyles.ApplyDataGridView(grid);
            grid.RowTemplate.Height = 28;
        }

        gridScores.EditMode = DataGridViewEditMode.EditOnEnter;
        gridScores.DataError += (_, e) => e.ThrowException = false;
    }

    private static List<decimal> BuildScoreOptions()
    {
        var values = new List<decimal>();
        for (var value = 0m; value <= 5m; value += 0.25m)
            values.Add(value);
        return values;
    }

    private void WireEvents()
    {
        Load += (_, _) => _initializationTask = InitializeAsync();
        FormClosed += (_, _) => CancelOperation();

        cmbInspectionId.SelectionChangeCommitted += async (_, _) =>
        {
            if (_binding || !_selectionInitialized)
                return;

            if (_dirty && MessageBox.Show(
                    this,
                    "Discard unsaved changes and open the selected inspection?",
                    "Change Inspection",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                SelectInspectionInDropdown(_current?.InspectionId);
                return;
            }

            if (cmbInspectionId.SelectedItem is InspectionResponse selected)
                await LoadInspectionAsync(selected.InspectionId);
        };

        cmbInspectionId.KeyDown += async (_, e) =>
        {
            if (e.KeyCode != Keys.Enter)
                return;

            var match = _inspectionList.FirstOrDefault(x =>
                x.InspectionId.ToString().Contains(cmbInspectionId.Text, StringComparison.OrdinalIgnoreCase) ||
                x.DisplayLabel.Contains(cmbInspectionId.Text, StringComparison.OrdinalIgnoreCase));

            if (match is not null)
                await LoadInspectionAsync(match.InspectionId);
        };

        btnNewInspection.Click += (_, _) => NewInspectionRequested?.Invoke(this, EventArgs.Empty);
        btnSaveDraft.Click += async (_, _) => await SaveDraftAsync();
        btnCompleteInspection.Click += async (_, _) => await CompleteInspectionAsync();
        btnDeleteInspection.Click += async (_, _) => await DeleteInspectionAsync();
        btnRefresh.Click += async (_, _) =>
        {
            if (_current is not null)
                await LoadInspectionAsync(_current.InspectionId);
            else
                await InitializeAsync();
        };

        btnViewVehicle.Click += (_, _) =>
        {
            if (_current is not null)
                ViewVehicleRequested?.Invoke(this, new GuidEventArgs(_current.AppraisalId));
        };

        btnViewAllMedia.Click += (_, _) =>
        {
            if (_current is not null)
                ViewAllMediaRequested?.Invoke(this, new GuidEventArgs(_current.InspectionId));
        };

        gridSections.CellContentClick += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex != gridSections.Columns.Count - 1)
                return;

            if (_current is not null)
                ViewSectionRequested?.Invoke(this, new GuidEventArgs(_current.InspectionId));
        };

        gridScores.CellValueChanged += (_, _) =>
        {
            if (_binding)
                return;

            _scoreRows.ResetBindings();
            MarkDirty();
            UpdateOverviewMetrics();
            BindSections(Array.Empty<SectionSummaryRowViewModel>());
            BindRedFlags(Array.Empty<RedFlagRowViewModel>());
            UpdateChecklist();
        };

        gridScores.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (gridScores.IsCurrentCellDirty)
                gridScores.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };

        foreach (var combo in new[] { cmbStatus, cmbInspectionType, cmbTemplate, cmbInspectedBy })
            combo.TextChanged += (_, _) => MarkDirty();

        txtLocation.TextChanged += (_, _) => MarkDirty();
        txtQuickNotes.TextChanged += (_, _) =>
        {
            MarkDirty();
            UpdateChecklist();
        };
        dtpInspectedOn.ValueChanged += (_, _) => MarkDirty();
    }

    private void ApplyTheme()
    {
        BackColor = NexusTheme.SurfaceAlt;
        ForeColor = NexusTheme.Text;
        Font = NexusTheme.BodyFont;

        pnlHeader.BackColor = NexusTheme.Surface;
        pnlInspectionSummary.BackColor = NexusTheme.Surface;
        pnlAppraisalSummary.BackColor = NexusTheme.Surface;
        pnlVehicleSummary.BackColor = NexusTheme.Surface;
        pnlScores.BackColor = NexusTheme.Surface;
        pnlSections.BackColor = NexusTheme.Surface;
        pnlRedFlags.BackColor = NexusTheme.Surface;
        pnlMedia.BackColor = NexusTheme.Surface;
        pnlHighlights.BackColor = NexusTheme.Surface;
        pnlQuickNotes.BackColor = NexusTheme.Surface;
        pnlChecklist.BackColor = NexusTheme.Surface;
        pnlFooter.BackColor = NexusTheme.Primary;

        lblTitle.ForeColor = NexusTheme.Primary;
        lblInspectionSummaryTitle.ForeColor = NexusTheme.Primary;
        lblAppraisalTitle.ForeColor = NexusTheme.Primary;
        lblVehicleTitle.ForeColor = NexusTheme.Primary;
        lblScoresTitle.ForeColor = NexusTheme.Primary;
        lblSectionsTitle.ForeColor = NexusTheme.Primary;
        lblRedFlagsTitle.ForeColor = NexusTheme.Primary;
        lblMediaTitle.ForeColor = NexusTheme.Primary;
        lblHighlightsTitle.ForeColor = NexusTheme.Primary;
        lblQuickNotesTitle.ForeColor = NexusTheme.Primary;
        lblChecklistTitle.ForeColor = NexusTheme.Primary;

        lblConditionIndexValue.ForeColor = NexusTheme.Primary;
        lblTotalRedFlagsValue.ForeColor = NexusTheme.Danger;
        lblProgressValue.ForeColor = NexusTheme.Primary;

        NexusControlStyles.ApplyPrimaryButton(btnNewInspection);
        NexusControlStyles.ApplySecondaryButton(btnSaveDraft);
        NexusControlStyles.ApplySecondaryButton(btnCompleteInspection);
        NexusControlStyles.ApplyDangerButton(btnDeleteInspection);
        NexusControlStyles.ApplySecondaryButton(btnRefresh);
        NexusControlStyles.ApplySecondaryButton(btnViewVehicle);
        NexusControlStyles.ApplySecondaryButton(btnRecalculate);
        NexusControlStyles.ApplySecondaryButton(btnViewAllMedia);

        btnRecalculate.Click += (_, _) => UpdateOverviewMetrics();

        foreach (var combo in new[]
                 {
                     cmbInspectionId, cmbStatus, cmbInspectionType,
                     cmbTemplate, cmbInspectedBy
                 })
        {
            NexusControlStyles.ApplyComboBox(combo);
        }

        NexusControlStyles.ApplyTextBox(txtLocation);
        NexusControlStyles.ApplyTextBox(txtQuickNotes);

        lblFooterInspectionId.ForeColor = NexusTheme.White;
        lblFooterCreated.ForeColor = NexusTheme.White;
        lblFooterUpdated.ForeColor = NexusTheme.White;
        lblSaveState.ForeColor = NexusTheme.White;
    }

    private void ClearWorkspace()
    {
        _binding = true;
        _current = null;
        _currentAppraisal = null;
        _currentVehicle = null;
        cmbStatus.SelectedIndex = -1;
        cmbInspectionType.SelectedIndex = -1;
        cmbTemplate.SelectedIndex = -1;
        cmbInspectedBy.SelectedIndex = -1;
        txtLocation.Clear();
        txtQuickNotes.Clear();
        _scoreRows.Clear();
        _sectionRows.Clear();
        _redFlagRows.Clear();
        BindMedia();
        BindAppraisalSummary(null);
        BindVehicleSummary(null);
        UpdateOverviewMetrics();
        _binding = false;
    }

    private void MarkDirty()
    {
        if (_binding)
            return;

        _dirty = true;
        SetDirtyState(true);
    }

    private void SetDirtyState(bool dirty)
    {
        lblSaveState.Text = dirty ? "Unsaved changes" : "All changes saved";
        lblSaveState.ForeColor = dirty ? NexusTheme.Warning : NexusTheme.White;
    }

    private void SetBusy(bool busy, string? message = null)
    {
        UseWaitCursor = busy;
        progressBusy.Visible = busy;
        btnSaveDraft.Enabled = !busy && _current is not null;
        btnCompleteInspection.Enabled = !busy && _current is not null;
        btnDeleteInspection.Enabled = !busy && _current is not null;
        btnRefresh.Enabled = !busy;

        if (!string.IsNullOrWhiteSpace(message))
            SetStatus(message, UiStatus.Info);
    }

    private void SetStatus(string message, UiStatus status)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = status switch
        {
            UiStatus.Success => NexusTheme.Success,
            UiStatus.Warning => NexusTheme.Warning,
            UiStatus.Error => NexusTheme.Danger,
            _ => NexusTheme.TextMuted
        };
    }

    private void SelectInspectionInDropdown(Guid? inspectionId)
    {
        if (!inspectionId.HasValue)
            return;

        var index = _inspectionList
            .Select((item, position) => new { item, position })
            .FirstOrDefault(x => x.item.InspectionId == inspectionId.Value)
            ?.position ?? -1;

        if (index >= 0)
        {
            _binding = true;
            cmbInspectionId.SelectedIndex = index;
            _binding = false;
        }
    }

    private void CancelOperation()
    {
        if (_operationCts is null)
            return;

        _operationCts.Cancel();
        _operationCts.Dispose();
        _operationCts = null;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_dirty && e.CloseReason == CloseReason.UserClosing)
        {
            var result = MessageBox.Show(
                this,
                "Close the inspection workspace and discard unsaved changes?",
                "Unsaved Changes",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
        }

        base.OnFormClosing(e);
    }

    private static string Display(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "—" : value;

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private enum UiStatus
    {
        Info,
        Success,
        Warning,
        Error
    }
}

public sealed class GuidEventArgs : EventArgs
{
    public GuidEventArgs(Guid id) => Id = id;
    public Guid Id { get; }
}

internal sealed class ConditionScoreRowViewModel
{
    public Guid? ConditionScoreId { get; set; }
    public int SystemNumber { get; set; }
    public string SystemName { get; set; } = string.Empty;
    public string SystemKey { get; set; } = string.Empty;
    public decimal Score0To5 { get; set; }
    public decimal Weight { get; set; }
    public decimal WeightedScore => Score0To5 * Weight;
    public bool IsRedFlag { get; set; }
    public string? Notes { get; set; }
}

internal sealed class SectionSummaryRowViewModel
{
    public int SystemNumber { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public string SectionDisplay => $"{SystemNumber}. {SectionName}";
    public decimal Score0To5 { get; set; }
    public string ItemsScoredDisplay { get; set; } = "—";
    public int RedFlagCount { get; set; }
}

internal sealed class RedFlagRowViewModel
{
    public string System { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public string? Item { get; set; }
    public string? Notes { get; set; }
}

internal static class CanonicalInspectionSystems
{
    public static IReadOnlyList<CanonicalInspectionSystem> All { get; } =
        new[]
        {
            new CanonicalInspectionSystem(1, "Body & Exterior", "body_exterior", 0.06m),
            new CanonicalInspectionSystem(2, "Frame & Structure", "frame_structure", 0.10m),
            new CanonicalInspectionSystem(3, "Engine / Powertrain", "powertrain", 0.18m),
            new CanonicalInspectionSystem(4, "Transmission & Driveline", "transmission_driveline", 0.14m),
            new CanonicalInspectionSystem(5, "Suspension & Steering", "suspension_steering", 0.10m),
            new CanonicalInspectionSystem(6, "Braking System", "braking_system", 0.10m),
            new CanonicalInspectionSystem(7, "Electrical System", "electrical_system", 0.10m),
            new CanonicalInspectionSystem(8, "Interior", "interior", 0.04m),
            new CanonicalInspectionSystem(9, "Wheels & Tires", "wheels_tires", 0.06m),
            new CanonicalInspectionSystem(10, "Trim, Glass & Details", "trim_glass_details", 0.02m),
            new CanonicalInspectionSystem(11, "Documentation & Provenance", "documentation_provenance", 0.04m),
            new CanonicalInspectionSystem(12, "Road Test & Functional", "road_test_functional", 0.06m)
        };
}

internal sealed record CanonicalInspectionSystem(
    int SystemNumber,
    string SystemName,
    string SystemKey,
    decimal StandardWeight);
