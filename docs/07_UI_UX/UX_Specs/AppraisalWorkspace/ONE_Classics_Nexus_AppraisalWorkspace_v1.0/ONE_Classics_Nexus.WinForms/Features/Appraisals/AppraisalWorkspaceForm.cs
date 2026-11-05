using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

public partial class AppraisalWorkspaceForm : Form
{
    private readonly AppraisalWorkspaceViewModel _viewModel;
    private readonly Dictionary<AppraisalWorkspaceSection, IAppraisalWorkspacePage> _pages;
    private readonly AppraisalIntakePage _intakePage;
    private CancellationTokenSource? _loadCancellation;

    public AppraisalWorkspaceForm(AppraisalWorkspaceViewModel viewModel)
    {
        _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        InitializeComponent();

        BackColor = AppraisalWorkspaceTheme.Canvas;
        contentHost.BackColor = AppraisalWorkspaceTheme.Canvas;
        statusStrip.BackColor = AppraisalWorkspaceTheme.Surface;
        statusStrip.ForeColor = AppraisalWorkspaceTheme.TextSecondary;

        _intakePage = new AppraisalIntakePage();
        _pages = CreatePages(_intakePage);
        AddPagesToHost();

        _viewModel.StateChanged += ViewModel_StateChanged;
        workflowControl.SectionSelected += WorkflowControl_SectionSelected;
        headerControl.RefreshRequested += HeaderControl_RefreshRequested;
        headerControl.SaveRequested += HeaderControl_SaveRequested;
        _intakePage.DirtyStateChanged += (_, _) => _viewModel.MarkDirty();

        ShowSection(AppraisalWorkspaceSection.Overview);
    }

    public event EventHandler<AppraisalWorkspaceNavigationEventArgs>? NavigationRequested;

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
        catch (OperationCanceledException) when (_loadCancellation.IsCancellationRequested)
        {
            statusLabel.Text = "Load cancelled";
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

        headerControl.Bind(appraisal, _viewModel.State.HasUnsavedChanges, _viewModel.State.IsBusy);
        summaryControl.Bind(appraisal);
        workflowControl.Bind(appraisal);

        foreach (var page in _pages.Values)
            page.Bind(appraisal);

        rowVersionLabel.Text = $"Row v{appraisal.RowVersion}";
        apiLabel.Text = "API connected";
        statusLabel.Text = $"Loaded {appraisal.DisplayNumber}";
        Text = $"ONE Classics Nexus — Appraisal {appraisal.DisplayNumber}";
    }

    private void ShowSection(AppraisalWorkspaceSection section)
    {
        if (!_pages.TryGetValue(section, out var selected))
            return;

        foreach (var page in _pages.Values)
            page.View.Visible = ReferenceEquals(page, selected);

        selected.View.BringToFront();
        workflowControl.SetSelected(section);
        _viewModel.SetActiveSection(section);
        statusLabel.Text = section.ToString();
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
            statusLabel.Text = "Appraisal saved";
        }
        catch (AppraisalConcurrencyException ex)
        {
            var result = MessageBox.Show(
                this,
                $"{ex.Message}{Environment.NewLine}{Environment.NewLine}Reload the latest appraisal now? Unsaved edits will be discarded.",
                "Appraisal changed by another user",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                await ReloadAsync();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        else if (!string.IsNullOrWhiteSpace(state.ErrorMessage))
            statusLabel.Text = state.ErrorMessage;
        else if (state.HasUnsavedChanges)
            statusLabel.Text = "Unsaved changes";
    }

    private void ShowApiError(AppraisalApiException ex)
    {
        apiLabel.Text = $"API {(int)ex.StatusCode}";
        statusLabel.Text = ex.Message;
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
        MessageBox.Show(
            this,
            $"An unexpected error occurred.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
            "Appraisal Workspace",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}
