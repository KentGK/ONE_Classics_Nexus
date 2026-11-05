// ============================================================
// AppraisalWorkspaceForm.cs
// ============================================================

using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Controls;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals
{
    using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;
    using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Services;
    using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;
    using System.Windows.Forms;

    public partial class AppraisalWorkspaceForm : Form
    {
        private readonly AppraisalWorkspaceViewModel _viewModel;
        private readonly Dictionary<AppraisalWorkspaceSection, IAppraisalWorkspacePage> _pages;
        private readonly AppraisalIntakePage _intakePage;
        private readonly AppraisalSummaryControl summaryControl;
        private CancellationTokenSource? _loadCancellation;

        public AppraisalWorkspaceForm(AppraisalWorkspaceViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();

            if (contentHost == null)
                throw new InvalidOperationException("contentHost is not initialized.");
            if (statusStrip == null)
                throw new InvalidOperationException("statusStrip is not initialized.");
            if (bodySplit == null)
                throw new InvalidOperationException("bodySplit is not initialized.");
            if (bodySplit.Panel1 == null)
                throw new InvalidOperationException("bodySplit.Panel1 is not initialized.");

            BackColor = AppraisalWorkspaceTheme.Canvas;
            contentHost.BackColor = AppraisalWorkspaceTheme.Canvas;
            statusStrip.BackColor = AppraisalWorkspaceTheme.Surface;
            statusStrip.ForeColor = AppraisalWorkspaceTheme.TextSecondary;

            _intakePage = new AppraisalIntakePage();
            _pages = CreatePages(_intakePage);
            AddPagesToHost();

            summaryControl = new AppraisalSummaryControl();
            summaryControl.Dock = DockStyle.Top;
            contentHost.Controls.Add(summaryControl);

            headerControl = new AppraisalHeaderControl();
            workflowControl = new AppraisalWorkflowControl();

            if (workflowControl == null)
                throw new InvalidOperationException("workflowControl is not initialized.");

            workflowControl.Dock = DockStyle.Fill;

            bodySplit.Panel1.Controls.Add(workflowControl);
            bodySplit.Panel1.Controls.Add(headerControl);

            _viewModel.StateChanged += ViewModel_StateChanged;
            workflowControl.SectionSelected += WorkflowControl_SectionSelected;
            headerControl.RefreshRequested += HeaderControl_RefreshRequested;
            headerControl.SaveRequested += HeaderControl_SaveRequested;
            _intakePage.DirtyStateChanged += (_, _) => _viewModel.MarkDirty();

            ShowSection(AppraisalWorkspaceSection.Overview);
            RaiseShellStatusChanged(
                statusText: "Appraisals - Ready",
                contextText: "Appraisal workspace");

            bodySplit.SplitterDistance = 220;
        }

        public event EventHandler<AppraisalWorkspaceNavigationEventArgs>? NavigationRequested;

        public event EventHandler<AppraisalWorkspaceShellStatusChangedEventArgs>? ShellStatusChanged;

        public AppraisalWorkspaceState State => _viewModel.State;

        public async Task LoadAppraisalAsync(Guid appraisalId, CancellationToken cancellationToken = default)
        {
            _loadCancellation?.Cancel();
            _loadCancellation?.Dispose();
            _loadCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                await _viewModel.LoadAsync(appraisalId, _loadCancellation.Token);
                BindAll();
            }
            catch (OperationCanceledException) when (_loadCancellation != null && _loadCancellation.IsCancellationRequested)
            {
                if (statusLabel != null)
                    statusLabel.Text = "Load cancelled";
                RaiseShellStatusChanged(statusText: "Appraisal load cancelled");
            }
            catch (AppraisalApiException ex)
            {
                ShowApiError(ex);
            }
            catch (Exception ex)
            {
                ShowUnexpectedError(ex);
            }
        }

        private Dictionary<AppraisalWorkspaceSection, IAppraisalWorkspacePage> CreatePages(AppraisalIntakePage intakePage)
        {
            var pages = new IAppraisalWorkspacePage[]
            {
                new AppraisalOverviewPage(),
                intakePage,
                new AppraisalVehiclePage(),
                new AppraisalInspectionPage(),
                new AppraisalComparablesPage(),
                new AppraisalValuationPage(),
                new AppraisalReviewPage(),
                new AppraisalReportPage()
            };

            foreach (var page in pages.OfType<WorkspacePageBase>())
                page.NavigationRequested += (_, args) => NavigationRequested?.Invoke(this, args);

            return pages.ToDictionary(page => page.Section);
        }

        private void AddPagesToHost()
        {
            foreach (var page in _pages.Values)
            {
                page.View.Dock = DockStyle.Fill;
                page.View.Visible = false;
                contentHost.Controls.Add(page.View);
            }
        }

        private void BindAll()
        {
            var appraisal = _viewModel.State.Appraisal;
            if (appraisal is null)
                return;

            headerControl?.Bind(appraisal, _viewModel.State.HasUnsavedChanges, _viewModel.State.IsBusy);
            summaryControl?.Bind(appraisal);
            workflowControl?.Bind(appraisal);

            foreach (var page in _pages.Values)
                page.Bind(appraisal);

            if (rowVersionLabel != null)
                rowVersionLabel.Text = $"Row v{appraisal.RowVersion}";
            if (apiLabel != null)
                apiLabel.Text = "API connected";
            if (statusLabel != null)
                statusLabel.Text = $"Loaded {appraisal.DisplayNumber}";
            Text = $"ONE Classics Nexus — Appraisal {appraisal.DisplayNumber}";

            RaiseShellStatusChanged();
        }

        private void ShowSection(AppraisalWorkspaceSection section)
        {
            if (!_pages.TryGetValue(section, out var selected))
                return;

            foreach (var page in _pages.Values)
                page.View.Visible = ReferenceEquals(page, selected);

            selected.View.BringToFront();
            workflowControl?.SetSelected(section);
            _viewModel?.SetActiveSection(section);
            if (statusLabel != null)
                statusLabel.Text = section.ToString();

            var contextText = _viewModel?.State.Appraisal is null
                ? string.Empty
                : $"Appraisal workspace — {section}";
            RaiseShellStatusChanged(contextText: contextText);
        }

        private async void HeaderControl_SaveRequested(object? sender, EventArgs e)
        {
            await SaveAsync();
        }

        private async Task SaveAsync()
        {
            var current = _viewModel.State.Appraisal;
            if (current is null || _viewModel.State.IsBusy)
                return;

            try
            {
                var request = _intakePage.CaptureUpdate(current);
                await _viewModel.SaveAsync(request);
                BindAll();
                //For possible null dereference of statusLabel
                if (statusLabel != null)
                    statusLabel.Text = "Appraisal saved";
                RaiseShellStatusChanged(statusText: "Appraisal saved");
            }
            catch (AppraisalConcurrencyException ex)
            {
                var result = MessageBox.Show(
                    this,
                    $"{ex.Message}{Environment.NewLine}{Environment.NewLine}Reload the latest appraisal now? Unsaved edits will be discarded.",
                    "Appraisal changed by another user",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                RaiseShellStatusChanged(
                    statusText: "Appraisals - Ready",
                    contextText: "Appraisal workspace");

                if (result == DialogResult.Yes)
                    await ReloadAsync();
            }
            catch (InvalidOperationException ex)
            {
                ShowUnexpectedError(ex);
            }
            catch (AppraisalApiException ex)
            {
                ShowApiError(ex);
            }
            catch (Exception ex)
            {
                ShowUnexpectedError(ex);
            }
        }

        private async void HeaderControl_RefreshRequested(object? sender, EventArgs e)
        {
            if (_viewModel.State.HasUnsavedChanges)
            {
                var result = MessageBox.Show(
                    this,
                    "Refresh will discard unsaved workspace edits. Continue?",
                    "Refresh appraisal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                    return;
            }

            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            try
            {
                await _viewModel.ReloadAsync();
                BindAll();
                statusLabel.Text = "Appraisal refreshed";
                RaiseShellStatusChanged(statusText: "Appraisal refreshed");
            }
            catch (AppraisalApiException ex)
            {
                ShowApiError(ex);
            }
            catch (Exception ex)
            {
                ShowUnexpectedError(ex);
            }
        }

        private void WorkflowControl_SectionSelected(object? sender, AppraisalWorkspaceSection section)
        {
            ShowSection(section);
        }

        private void ViewModel_StateChanged(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateStateChrome));
                return;
            }

            UpdateStateChrome();
        }

        private void UpdateStateChrome()
        {
            var state = _viewModel.State;
            UseWaitCursor = state.IsBusy;
            headerControl.SetBusy(state.IsBusy);

            if (state.Appraisal is not null)
                headerControl.Bind(state.Appraisal, state.HasUnsavedChanges, state.IsBusy);

            if (state.IsBusy)
                statusLabel.Text = "Working…";
            else if (state.ErrorMessage != null)
                statusLabel.Text = state.ErrorMessage;
            else if (state.HasUnsavedChanges)
                statusLabel.Text = "Unsaved changes";

            RaiseShellStatusChanged();
        }

        private void ShowApiError(AppraisalApiException ex)
        {
            apiLabel.Text = $"API {(int)ex.StatusCode}";
            statusLabel.Text = ex.Message;


            RaiseShellStatusChanged(
                statusText: "Appraisals - Ready",
                contextText: "Appraisal workspace");
            MessageBox.Show(
                this,
                ex.Message,
                "Nexus API Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void ShowUnexpectedError(Exception ex)
        {
            apiLabel.Text = "Error";
            statusLabel.Text = ex.Message;
            RaiseShellStatusChanged(
                statusText: "Appraisal workspace error");
            MessageBox.Show(
                this,
                $"An unexpected error occurred.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                "Appraisal Workspace",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void RaiseShellStatusChanged(string? statusText = null, string? contextText = null)
        {
            var state = _viewModel.State;
            var displayNumber = state.Appraisal?.DisplayNumber;
            ShellStatusChanged?.Invoke(this, new AppraisalWorkspaceShellStatusChangedEventArgs(
                statusText ?? ResolveDefaultStatusText(state, displayNumber),
                contextText ?? "Appraisal workspace",
                state.IsBusy,
                state.HasUnsavedChanges,
                state.ErrorMessage != null,
                displayNumber,
                state.ActiveSection
            ));
        }

        private static string ResolveDefaultStatusText(
            AppraisalWorkspaceState state,
            string? displayNumber)
        {
            if (state.IsBusy)
                return "Appraisal workspace working...";

            if (!string.IsNullOrWhiteSpace(state.ErrorMessage))
                return state.ErrorMessage;

            if (state.HasUnsavedChanges)
                return "Appraisal workspace has unsaved changes";

            if (!string.IsNullOrWhiteSpace(displayNumber))
                return $"Appraisal {displayNumber} loaded";

            return "Appraisals - Ready";
        }

        private void statusStrip_ItemClicked(
            object? sender,
            ToolStripItemClickedEventArgs e)
        {
        }

        private void bodySplit_Panel1_Paint(
            object? sender,
            PaintEventArgs e)
        {
        }
    }
}
