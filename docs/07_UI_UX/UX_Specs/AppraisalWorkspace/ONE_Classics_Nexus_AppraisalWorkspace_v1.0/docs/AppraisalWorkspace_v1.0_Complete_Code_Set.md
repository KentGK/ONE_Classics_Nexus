# ONE Classics Nexus — AppraisalWorkspace v1.0 Complete Code Set

**Target:** .NET 8 WinForms

This document mirrors the source and integration files in the ZIP package for review and repo implementation.

## `INTEGRATION/AppraisalWorkspace.csproj.additions.xml`

```xml
<!-- Merge only the package references not already standardized in ONE_Classics_Nexus.WinForms.csproj. -->
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.1" />
  <PackageReference Include="Microsoft.Extensions.Http" Version="8.0.1" />
</ItemGroup>
```

## `INTEGRATION/MainShellForm.integration.cs.txt`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals;

// Shell-neutral example. Replace contentPanel with the current MainShellForm content host.
private async Task OpenAppraisalWorkspaceAsync(Guid appraisalId)
{
    var workspace = await contentPanel.OpenInAsync(
        _services,
        appraisalId,
        CancellationToken.None);

    workspace.NavigationRequested += Workspace_NavigationRequested;
}

private void Workspace_NavigationRequested(
    object? sender,
    AppraisalWorkspaceNavigationEventArgs e)
{
    switch (e.Target)
    {
        case AppraisalWorkspaceSection.Vehicle:
            // Navigate to Vehicle Detail using e.RelatedEntityId.
            break;
        case AppraisalWorkspaceSection.Inspection:
            // e.Action is "open" or "create". Retain e.AppraisalId.
            break;
        case AppraisalWorkspaceSection.Comparables:
            // Navigate to Comparables filtered by e.AppraisalId.
            break;
        case AppraisalWorkspaceSection.Valuation:
            // e.Action is "open" or "run".
            break;
        case AppraisalWorkspaceSection.Report:
            // e.Action is "open" or "generate".
            break;
    }
}
```

## `INTEGRATION/Program.registration.cs.txt`

```csharp
using ONE_Classics_Nexus.WinForms.DependencyInjection;

// Use the solution's configuration source when available.
var apiBaseUrl = configuration["NexusApi:BaseUrl"]
    ?? throw new InvalidOperationException("NexusApi:BaseUrl is required.");

services.AddAppraisalWorkspace(new Uri(apiBaseUrl, UriKind.Absolute));
```

## `ONE_Classics_Nexus.Tests/WinForms/Appraisals/AppraisalWorkspaceViewModelTests.cs`

```csharp
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;
using ONE_Classics_Nexus.WinForms.Features.Appraisals;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

namespace ONE_Classics_Nexus.Tests.WinForms.Appraisals;

public sealed class AppraisalWorkspaceViewModelTests
{
    [Fact]
    public async Task LoadAsync_LoadsWorkspaceAndClearsDirtyState()
    {
        var appraisal = CreateAppraisal(rowVersion: 4);
        var service = new FakeService(appraisal);
        var sut = new AppraisalWorkspaceViewModel(service);
        sut.MarkDirty();

        await sut.LoadAsync(appraisal.AppraisalId);

        Assert.Same(appraisal, sut.State.Appraisal);
        Assert.False(sut.State.HasUnsavedChanges);
        Assert.True(sut.State.IsLoaded);
        Assert.False(sut.State.IsBusy);
    }

    [Fact]
    public async Task SaveAsync_UsesLoadedRowVersionAndVehicleId()
    {
        var appraisal = CreateAppraisal(rowVersion: 9);
        var service = new FakeService(appraisal)
        {
            UpdateResult = CreateAppraisal(rowVersion: 10, appraisal.AppraisalId, appraisal.VehicleId)
        };
        var sut = new AppraisalWorkspaceViewModel(service);
        await sut.LoadAsync(appraisal.AppraisalId);

        await sut.SaveAsync(new UpdateAppraisalWorkspaceRequest
        {
            AppraisalType = "Full",
            Status = "InProgress"
        });

        Assert.NotNull(service.LastUpdateRequest);
        Assert.Equal(9, service.LastUpdateRequest!.RowVersion);
        Assert.Equal(appraisal.VehicleId, service.LastUpdateRequest.VehicleId);
        Assert.Equal(appraisal.AppraisalId, service.LastUpdateRequest.AppraisalId);
        Assert.Equal(10, sut.State.Appraisal!.RowVersion);
    }

    [Fact]
    public async Task SetActiveSection_RaisesStateChanged()
    {
        var sut = new AppraisalWorkspaceViewModel(new FakeService(CreateAppraisal(1)));
        var raised = 0;
        sut.StateChanged += (_, _) => raised++;

        sut.SetActiveSection(AppraisalWorkspaceSection.Valuation);

        Assert.Equal(AppraisalWorkspaceSection.Valuation, sut.State.ActiveSection);
        Assert.Equal(1, raised);
    }

    private static AppraisalWorkspaceDto CreateAppraisal(
        long rowVersion,
        Guid? appraisalId = null,
        Guid? vehicleId = null)
    {
        return new AppraisalWorkspaceDto
        {
            AppraisalId = appraisalId ?? Guid.NewGuid(),
            VehicleId = vehicleId ?? Guid.NewGuid(),
            AppraisalType = "Full",
            Status = "Draft",
            RowVersion = rowVersion,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    private sealed class FakeService : IAppraisalWorkspaceService
    {
        private readonly AppraisalWorkspaceDto _getResult;

        public FakeService(AppraisalWorkspaceDto getResult) => _getResult = getResult;

        public AppraisalWorkspaceDto? UpdateResult { get; set; }
        public UpdateAppraisalWorkspaceRequest? LastUpdateRequest { get; private set; }

        public Task<AppraisalWorkspaceDto> GetAsync(Guid appraisalId, CancellationToken cancellationToken = default)
            => Task.FromResult(_getResult);

        public Task<AppraisalWorkspaceDto> UpdateAsync(
            Guid appraisalId,
            UpdateAppraisalWorkspaceRequest request,
            CancellationToken cancellationToken = default)
        {
            LastUpdateRequest = request;
            return Task.FromResult(UpdateResult ?? _getResult);
        }
    }
}
```

## `ONE_Classics_Nexus.WinForms/DependencyInjection/AppraisalWorkspaceServiceCollectionExtensions.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.WinForms.Features.Appraisals;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

namespace ONE_Classics_Nexus.WinForms.DependencyInjection;

public static class AppraisalWorkspaceServiceCollectionExtensions
{
    public static IServiceCollection AddAppraisalWorkspace(
        this IServiceCollection services,
        Uri apiBaseAddress)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(apiBaseAddress);

        if (!apiBaseAddress.IsAbsoluteUri)
            throw new ArgumentException("Nexus API base address must be absolute.", nameof(apiBaseAddress));

        services.AddHttpClient<IAppraisalWorkspaceService, AppraisalWorkspaceService>(client =>
        {
            client.BaseAddress = apiBaseAddress;
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });

        services.AddTransient<AppraisalWorkspaceViewModel>();
        services.AddTransient<AppraisalWorkspaceForm>();
        return services;
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceForm.Designer.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

partial class AppraisalWorkspaceForm
{
    private System.ComponentModel.IContainer? components = null;
    private AppraisalHeaderControl headerControl;
    private AppraisalSummaryControl summaryControl;
    private SplitContainer bodySplit;
    private AppraisalWorkflowControl workflowControl;
    private Panel contentHost;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private ToolStripStatusLabel springLabel;
    private ToolStripStatusLabel rowVersionLabel;
    private ToolStripStatusLabel apiLabel;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _loadCancellation?.Cancel();
            _loadCancellation?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        headerControl = new AppraisalHeaderControl();
        summaryControl = new AppraisalSummaryControl();
        bodySplit = new SplitContainer();
        workflowControl = new AppraisalWorkflowControl();
        contentHost = new Panel();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        springLabel = new ToolStripStatusLabel();
        rowVersionLabel = new ToolStripStatusLabel();
        apiLabel = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)bodySplit).BeginInit();
        bodySplit.Panel1.SuspendLayout();
        bodySplit.Panel2.SuspendLayout();
        bodySplit.SuspendLayout();
        SuspendLayout();

        headerControl.Dock = DockStyle.Top;
        headerControl.Height = 92;

        summaryControl.Dock = DockStyle.Top;
        summaryControl.Height = 92;

        bodySplit.Dock = DockStyle.Fill;
        bodySplit.FixedPanel = FixedPanel.Panel1;
        bodySplit.IsSplitterFixed = false;
        bodySplit.SplitterDistance = 220;
        bodySplit.SplitterWidth = 1;
        bodySplit.Panel1.Controls.Add(workflowControl);
        bodySplit.Panel2.Controls.Add(contentHost);

        workflowControl.Dock = DockStyle.Fill;
        contentHost.Dock = DockStyle.Fill;

        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, springLabel, rowVersionLabel, apiLabel });
        statusStrip.Dock = DockStyle.Bottom;
        statusLabel.Text = "Ready";
        springLabel.Spring = true;
        rowVersionLabel.Text = "Row v—";
        apiLabel.Text = "API";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1280, 820);
        Controls.Add(bodySplit);
        Controls.Add(summaryControl);
        Controls.Add(headerControl);
        Controls.Add(statusStrip);
        MinimumSize = new Size(1024, 680);
        Name = "AppraisalWorkspaceForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "ONE Classics Nexus — Appraisal Workspace";
        bodySplit.Panel1.ResumeLayout(false);
        bodySplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)bodySplit).EndInit();
        bodySplit.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceForm.cs`

```csharp
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
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceLauncher.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

public static class AppraisalWorkspaceLauncher
{
    public static async Task<AppraisalWorkspaceForm> OpenInAsync(
        this Control host,
        IServiceProvider services,
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(services);

        if (appraisalId == Guid.Empty)
            throw new ArgumentException("Appraisal id is required.", nameof(appraisalId));

        var workspace = services.GetRequiredService<AppraisalWorkspaceForm>();
        workspace.TopLevel = false;
        workspace.FormBorderStyle = FormBorderStyle.None;
        workspace.Dock = DockStyle.Fill;

        host.SuspendLayout();
        try
        {
            foreach (Control existing in host.Controls.Cast<Control>().ToArray())
                existing.Dispose();

            host.Controls.Clear();
            host.Controls.Add(workspace);
            workspace.Show();
        }
        finally
        {
            host.ResumeLayout(true);
        }

        await workspace.LoadAppraisalAsync(appraisalId, cancellationToken);
        return workspace;
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceNavigationEventArgs.cs`

```csharp
namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

public sealed class AppraisalWorkspaceNavigationEventArgs : EventArgs
{
    public AppraisalWorkspaceNavigationEventArgs(
        AppraisalWorkspaceSection target,
        Guid appraisalId,
        Guid? relatedEntityId = null,
        string? action = null)
    {
        Target = target;
        AppraisalId = appraisalId;
        RelatedEntityId = relatedEntityId;
        Action = action;
    }

    public AppraisalWorkspaceSection Target { get; }
    public Guid AppraisalId { get; }
    public Guid? RelatedEntityId { get; }
    public string? Action { get; }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceSection.cs`

```csharp
namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

public enum AppraisalWorkspaceSection
{
    Overview = 0,
    Intake = 1,
    Vehicle = 2,
    Inspection = 3,
    Comparables = 4,
    Valuation = 5,
    Review = 6,
    Report = 7
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceState.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

public sealed class AppraisalWorkspaceState
{
    public AppraisalWorkspaceDto? Appraisal { get; internal set; }
    public AppraisalWorkspaceSection ActiveSection { get; internal set; } = AppraisalWorkspaceSection.Overview;
    public bool IsBusy { get; internal set; }
    public bool HasUnsavedChanges { get; internal set; }
    public string? ErrorMessage { get; internal set; }
    public DateTimeOffset? LoadedAtUtc { get; internal set; }

    public bool IsLoaded => Appraisal is not null;
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/AppraisalWorkspaceViewModel.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals;

public sealed class AppraisalWorkspaceViewModel
{
    private readonly IAppraisalWorkspaceService _service;

    public AppraisalWorkspaceViewModel(IAppraisalWorkspaceService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public AppraisalWorkspaceState State { get; } = new();

    public event EventHandler? StateChanged;

    public async Task LoadAsync(Guid appraisalId, CancellationToken cancellationToken = default)
    {
        if (appraisalId == Guid.Empty)
            throw new ArgumentException("Appraisal id is required.", nameof(appraisalId));

        await ExecuteBusyAsync(async () =>
        {
            State.ErrorMessage = null;
            State.Appraisal = await _service.GetAsync(appraisalId, cancellationToken);
            State.LoadedAtUtc = DateTimeOffset.UtcNow;
            State.HasUnsavedChanges = false;
        });
    }

    public async Task SaveAsync(
        UpdateAppraisalWorkspaceRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (State.Appraisal is null)
            throw new InvalidOperationException("Load an appraisal before saving the workspace.");

        request.AppraisalId = State.Appraisal.AppraisalId;
        request.VehicleId = State.Appraisal.VehicleId;
        request.RowVersion = State.Appraisal.RowVersion;

        await ExecuteBusyAsync(async () =>
        {
            State.ErrorMessage = null;
            State.Appraisal = await _service.UpdateAsync(
                State.Appraisal.AppraisalId,
                request,
                cancellationToken);
            State.LoadedAtUtc = DateTimeOffset.UtcNow;
            State.HasUnsavedChanges = false;
        });
    }

    public Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        if (State.Appraisal is null)
            throw new InvalidOperationException("There is no loaded appraisal to refresh.");

        return LoadAsync(State.Appraisal.AppraisalId, cancellationToken);
    }

    public void SetActiveSection(AppraisalWorkspaceSection section)
    {
        if (State.ActiveSection == section)
            return;

        State.ActiveSection = section;
        OnStateChanged();
    }

    public void MarkDirty(bool isDirty = true)
    {
        if (State.HasUnsavedChanges == isDirty)
            return;

        State.HasUnsavedChanges = isDirty;
        OnStateChanged();
    }

    private async Task ExecuteBusyAsync(Func<Task> action)
    {
        State.IsBusy = true;
        OnStateChanged();

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            State.ErrorMessage = ex.Message;
            OnStateChanged();
            throw;
        }
        finally
        {
            State.IsBusy = false;
            OnStateChanged();
        }
    }

    private void OnStateChanged() => StateChanged?.Invoke(this, EventArgs.Empty);
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Controls/AppraisalHeaderControl.Designer.cs`

```csharp
namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

partial class AppraisalHeaderControl
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel layout;
    private Label lblTitle;
    private Label lblVehicle;
    private AppraisalStatusBadge statusBadge;
    private FlowLayoutPanel actionsPanel;
    private Button btnRefresh;
    private Button btnSave;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        layout = new TableLayoutPanel();
        lblTitle = new Label();
        lblVehicle = new Label();
        statusBadge = new AppraisalStatusBadge();
        actionsPanel = new FlowLayoutPanel();
        btnRefresh = new Button();
        btnSave = new Button();
        SuspendLayout();

        layout.ColumnCount = 3;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.RowCount = 2;
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(24, 12, 18, 10);

        lblTitle.Dock = DockStyle.Fill;
        lblTitle.TextAlign = ContentAlignment.BottomLeft;
        lblTitle.AutoEllipsis = true;
        layout.Controls.Add(lblTitle, 0, 0);

        lblVehicle.Dock = DockStyle.Fill;
        lblVehicle.TextAlign = ContentAlignment.TopLeft;
        lblVehicle.AutoEllipsis = true;
        layout.Controls.Add(lblVehicle, 0, 1);

        statusBadge.Anchor = AnchorStyles.None;
        layout.SetRowSpan(statusBadge, 2);
        layout.Controls.Add(statusBadge, 1, 0);

        actionsPanel.AutoSize = true;
        actionsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.FlowDirection = FlowDirection.LeftToRight;
        actionsPanel.WrapContents = false;
        actionsPanel.Padding = new Padding(4, 16, 0, 0);
        layout.SetRowSpan(actionsPanel, 2);
        layout.Controls.Add(actionsPanel, 2, 0);

        btnRefresh.Text = "Refresh";
        btnRefresh.AutoSize = true;
        btnRefresh.MinimumSize = new Size(86, 34);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        actionsPanel.Controls.Add(btnRefresh);

        btnSave.Text = "Save";
        btnSave.AutoSize = true;
        btnSave.MinimumSize = new Size(86, 34);
        btnSave.FlatStyle = FlatStyle.Flat;
        actionsPanel.Controls.Add(btnSave);

        Controls.Add(layout);
        Name = "AppraisalHeaderControl";
        Size = new Size(1100, 92);
        ResumeLayout(false);
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Controls/AppraisalHeaderControl.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public partial class AppraisalHeaderControl : UserControl
{
    public AppraisalHeaderControl()
    {
        InitializeComponent();
        BackColor = AppraisalWorkspaceTheme.Primary;

        lblTitle.Font = AppraisalWorkspaceTheme.HeaderFont(18f);
        lblTitle.ForeColor = Color.White;
        lblTitle.Text = "Appraisal Workspace";

        lblVehicle.Font = AppraisalWorkspaceTheme.BodyFont(10f);
        lblVehicle.ForeColor = Color.FromArgb(218, 230, 241);

        btnRefresh.BackColor = Color.White;
        btnRefresh.ForeColor = AppraisalWorkspaceTheme.Primary;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(110, 145, 175);
        btnRefresh.Cursor = Cursors.Hand;

        btnSave.BackColor = AppraisalWorkspaceTheme.Accent;
        btnSave.ForeColor = Color.White;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Cursor = Cursors.Hand;

        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        btnSave.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? RefreshRequested;
    public event EventHandler? SaveRequested;

    public void Bind(AppraisalWorkspaceDto appraisal, bool hasUnsavedChanges, bool isBusy)
    {
        ArgumentNullException.ThrowIfNull(appraisal);

        lblTitle.Text = $"Appraisal {appraisal.DisplayNumber}";
        lblVehicle.Text = appraisal.VehicleDisplayName;
        statusBadge.SetStatus(appraisal.Status);
        btnSave.Text = hasUnsavedChanges ? "Save *" : "Save";
        btnSave.Enabled = !isBusy;
        btnRefresh.Enabled = !isBusy;
    }

    public void SetBusy(bool isBusy)
    {
        btnSave.Enabled = !isBusy;
        btnRefresh.Enabled = !isBusy;
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Controls/AppraisalStatusBadge.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public sealed class AppraisalStatusBadge : Label
{
    private Color _borderColor = AppraisalWorkspaceTheme.Muted;

    public AppraisalStatusBadge()
    {
        AutoSize = false;
        Size = new Size(120, 28);
        TextAlign = ContentAlignment.MiddleCenter;
        Font = AppraisalWorkspaceTheme.BodyFont(9f);
        ForeColor = Color.White;
        BackColor = AppraisalWorkspaceTheme.Muted;
        Padding = new Padding(8, 2, 8, 2);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    public void SetStatus(string? status)
    {
        Text = string.IsNullOrWhiteSpace(status) ? "Unknown" : status;
        BackColor = AppraisalWorkspaceTheme.StatusColor(status);
        _borderColor = BackColor;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? Color.White);
        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = AppraisalWorkspaceTheme.CreateRoundedPath(bounds, 8);
        using var brush = new SolidBrush(BackColor);
        using var pen = new Pen(_borderColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.FillPath(brush, path);
        e.Graphics.DrawPath(pen, path);
        TextRenderer.DrawText(e.Graphics, Text, Font, bounds, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Controls/AppraisalSummaryControl.Designer.cs`

```csharp
namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

partial class AppraisalSummaryControl
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel grid;
    private Label lblTypeValue;
    private Label lblEffectiveValue;
    private Label lblAppraiserValue;
    private Label lblReviewerValue;
    private Label lblUpdatedValue;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        grid = new TableLayoutPanel();
        lblTypeValue = new Label();
        lblEffectiveValue = new Label();
        lblAppraiserValue = new Label();
        lblReviewerValue = new Label();
        lblUpdatedValue = new Label();
        SuspendLayout();

        grid.ColumnCount = 5;
        for (var i = 0; i < 5; i++)
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        grid.RowCount = 2;
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        grid.Dock = DockStyle.Fill;
        grid.Padding = new Padding(20, 10, 20, 10);

        AddCell(0, "Appraisal Type", lblTypeValue);
        AddCell(1, "Effective Date", lblEffectiveValue);
        AddCell(2, "Assigned Appraiser", lblAppraiserValue);
        AddCell(3, "Assigned Reviewer", lblReviewerValue);
        AddCell(4, "Last Updated", lblUpdatedValue);

        Controls.Add(grid);
        Name = "AppraisalSummaryControl";
        Size = new Size(1100, 92);
        ResumeLayout(false);
    }

    private void AddCell(int column, string caption, Label valueLabel)
    {
        var captionLabel = new Label
        {
            Text = caption.ToUpperInvariant(),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
            AutoEllipsis = true
        };
        valueLabel.Dock = DockStyle.Fill;
        valueLabel.TextAlign = ContentAlignment.TopLeft;
        valueLabel.AutoEllipsis = true;
        grid.Controls.Add(captionLabel, column, 0);
        grid.Controls.Add(valueLabel, column, 1);
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Controls/AppraisalSummaryControl.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public partial class AppraisalSummaryControl : UserControl
{
    public AppraisalSummaryControl()
    {
        InitializeComponent();
        BackColor = AppraisalWorkspaceTheme.Surface;

        foreach (Control control in grid.Controls)
        {
            if (control is not Label label)
                continue;

            var position = grid.GetPositionFromControl(label);
            var isCaption = position.Row == 0;
            label.Font = isCaption
                ? AppraisalWorkspaceTheme.BodyFont(8f)
                : AppraisalWorkspaceTheme.BodyFont(10f);
            label.ForeColor = isCaption
                ? AppraisalWorkspaceTheme.TextSecondary
                : AppraisalWorkspaceTheme.TextPrimary;
        }
    }

    public void Bind(AppraisalWorkspaceDto appraisal)
    {
        ArgumentNullException.ThrowIfNull(appraisal);

        lblTypeValue.Text = EmptyAsDash(appraisal.AppraisalType);
        lblEffectiveValue.Text = appraisal.EffectiveDateUtc?.ToLocalTime().ToString("MMM d, yyyy") ?? "—";
        lblAppraiserValue.Text = appraisal.AssignedAppraiser?.DisplayText
            ?? appraisal.AssignedAppraiserId?.ToString("D")
            ?? "Unassigned";
        lblReviewerValue.Text = appraisal.AssignedReviewer?.DisplayText
            ?? appraisal.AssignedReviewerId?.ToString("D")
            ?? "Unassigned";
        lblUpdatedValue.Text = appraisal.UpdatedAtUtc == default
            ? "—"
            : appraisal.UpdatedAtUtc.ToLocalTime().ToString("MMM d, yyyy h:mm tt");
    }

    private static string EmptyAsDash(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value;
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Controls/AppraisalWorkflowControl.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public sealed class AppraisalWorkflowControl : UserControl
{
    private readonly FlowLayoutPanel _flow;
    private readonly Dictionary<AppraisalWorkspaceSection, Button> _buttons = new();
    private AppraisalWorkspaceSection _selected = AppraisalWorkspaceSection.Overview;

    public AppraisalWorkflowControl()
    {
        BackColor = AppraisalWorkspaceTheme.Surface;
        Padding = new Padding(10, 14, 10, 10);

        _flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = AppraisalWorkspaceTheme.Surface
        };
        Controls.Add(_flow);

        foreach (var section in Enum.GetValues<AppraisalWorkspaceSection>())
        {
            var button = CreateButton(section);
            _buttons.Add(section, button);
            _flow.Controls.Add(button);
        }

        SetSelected(_selected);
    }

    public event EventHandler<AppraisalWorkspaceSection>? SectionSelected;

    public void Bind(AppraisalWorkspaceDto appraisal)
    {
        ArgumentNullException.ThrowIfNull(appraisal);

        SetButtonText(AppraisalWorkspaceSection.Overview, "Overview", appraisal.Status);
        SetButtonText(AppraisalWorkspaceSection.Intake, "Intake", appraisal.IntakeStatus);
        SetButtonText(AppraisalWorkspaceSection.Vehicle, "Vehicle", appraisal.Vehicle is null ? null : "Linked");
        SetButtonText(AppraisalWorkspaceSection.Inspection, "Inspection", appraisal.InspectionStatus);
        SetButtonText(AppraisalWorkspaceSection.Comparables, "Comparables", $"{appraisal.ComparableCount} linked");
        SetButtonText(AppraisalWorkspaceSection.Valuation, "Valuation", appraisal.ValuationStatus);
        SetButtonText(AppraisalWorkspaceSection.Review, "Review", appraisal.LatestValuation?.ManualReviewRequired == true ? "Required" : null);
        SetButtonText(AppraisalWorkspaceSection.Report, "Report", appraisal.ReportStatus);
    }

    public void SetSelected(AppraisalWorkspaceSection section)
    {
        _selected = section;
        foreach (var pair in _buttons)
        {
            var selected = pair.Key == section;
            pair.Value.BackColor = selected ? AppraisalWorkspaceTheme.Primary : AppraisalWorkspaceTheme.Surface;
            pair.Value.ForeColor = selected ? Color.White : AppraisalWorkspaceTheme.TextPrimary;
            pair.Value.FlatAppearance.BorderColor = selected ? AppraisalWorkspaceTheme.Primary : AppraisalWorkspaceTheme.Border;
        }
    }

    private Button CreateButton(AppraisalWorkspaceSection section)
    {
        var button = new Button
        {
            Tag = section,
            Text = section.ToString(),
            Size = new Size(190, 52),
            Margin = new Padding(0, 0, 0, 8),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 2, 8, 2),
            FlatStyle = FlatStyle.Flat,
            Font = AppraisalWorkspaceTheme.BodyFont(9.5f),
            Cursor = Cursors.Hand,
            UseMnemonic = false
        };
        button.FlatAppearance.BorderColor = AppraisalWorkspaceTheme.Border;
        button.Click += (_, _) =>
        {
            SetSelected(section);
            SectionSelected?.Invoke(this, section);
        };
        return button;
    }

    private void SetButtonText(AppraisalWorkspaceSection section, string caption, string? status)
    {
        if (!_buttons.TryGetValue(section, out var button))
            return;

        button.Text = string.IsNullOrWhiteSpace(status)
            ? caption
            : $"{caption}{Environment.NewLine}{status}";
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/GlobalUsings.AppraisalWorkspace.cs`

```csharp
global using System;
global using System.Collections.Generic;
global using System.Drawing;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Windows.Forms;
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Models/ApiProblemDetails.cs`

```csharp
namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

public sealed class ApiProblemDetails
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }

    public string ToDisplayMessage()
    {
        var validation = Errors is null
            ? null
            : string.Join(Environment.NewLine, Errors.SelectMany(pair =>
                pair.Value.Select(message => $"{pair.Key}: {message}")));

        if (!string.IsNullOrWhiteSpace(validation))
            return validation;

        if (!string.IsNullOrWhiteSpace(Detail))
            return Detail!;

        if (!string.IsNullOrWhiteSpace(Title))
            return Title!;

        return "The Nexus API returned an error.";
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Models/AppraisalWorkspaceDto.cs`

```csharp
using System.Text.Json.Serialization;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

public sealed class AppraisalWorkspaceDto
{
    public Guid AppraisalId { get; set; }
    public string? AppraisalNumber { get; set; }
    public Guid? AppraisalRequestId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? AppraisalTypeId { get; set; }
    public Guid? AppraisalStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string AppraisalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IntakeStatus { get; set; }
    public string? InspectionStatus { get; set; }
    public string? ValuationStatus { get; set; }
    public string? ReportStatus { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
    public long RowVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public VehicleSummaryDto? Vehicle { get; set; }
    public ClientSummaryDto? Client { get; set; }
    public UserSummaryDto? AssignedAppraiser { get; set; }
    public UserSummaryDto? AssignedReviewer { get; set; }
    public InspectionSummaryDto? LatestInspection { get; set; }
    public ValuationSummaryDto? LatestValuation { get; set; }
    public ReportSummaryDto? LatestReport { get; set; }
    public int ComparableCount { get; set; }

    [JsonIgnore]
    public string DisplayNumber => string.IsNullOrWhiteSpace(AppraisalNumber)
        ? AppraisalId.ToString("D")
        : AppraisalNumber;

    [JsonIgnore]
    public string VehicleDisplayName => Vehicle?.DisplayName
        ?? (VehicleId == Guid.Empty ? "Subject vehicle unavailable" : $"Vehicle {VehicleId:D}");
}

public sealed class VehicleSummaryDto
{
    public Guid VehicleId { get; set; }
    public Guid? VehicleTypeId { get; set; }
    public string? Vin { get; set; }
    public int? Year { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Trim { get; set; }
    public string? SeriesName { get; set; }
    public string? BodyStyle { get; set; }
    public string? Drivetrain { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public int? Mileage { get; set; }
    public string? OdometerUnit { get; set; }
    public decimal? OverallConditionScore { get; set; }
    public bool HasRedFlags { get; set; }

    [JsonIgnore]
    public string DisplayName
    {
        get
        {
            var identity = string.Join(" ", new[]
            {
                Year?.ToString(), Make, Model, Trim
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            return string.IsNullOrWhiteSpace(identity) ? "Subject vehicle" : identity;
        }
    }
}

public sealed class ClientSummaryDto
{
    public Guid ClientId { get; set; }
    public string? LegalName { get; set; }
    public string? PreferredName { get; set; }
    public string? Status { get; set; }

    [JsonIgnore]
    public string DisplayName => !string.IsNullOrWhiteSpace(PreferredName)
        ? PreferredName!
        : LegalName ?? ClientId.ToString("D");
}

public sealed class UserSummaryDto
{
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }

    [JsonIgnore]
    public string DisplayText => !string.IsNullOrWhiteSpace(DisplayName)
        ? DisplayName!
        : !string.IsNullOrWhiteSpace(Username)
            ? Username!
            : Email ?? UserId.ToString("D");
}

public sealed class InspectionSummaryDto
{
    public Guid InspectionId { get; set; }
    public string? InspectionType { get; set; }
    public string? Status { get; set; }
    public string? InspectionLocation { get; set; }
    public DateTime? InspectedOnUtc { get; set; }
    public decimal? ConditionIndex { get; set; }
    public int CompletedSystemCount { get; set; }
    public int TotalSystemCount { get; set; } = 12;
    public int RedFlagCount { get; set; }
}

public sealed class ValuationSummaryDto
{
    public Guid ValuationRunId { get; set; }
    public string? ValuationStatus { get; set; }
    public string? Status { get; set; }
    public string? EngineVersion { get; set; }
    public decimal? ConditionIndex { get; set; }
    public decimal? ConfidenceScore { get; set; }
    public string? ConfidenceBand { get; set; }
    public decimal? ValueLow { get; set; }
    public decimal? FinalValue { get; set; }
    public decimal? ValueHigh { get; set; }
    public bool ManualReviewRequired { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? ExecutedAtUtc { get; set; }
}

public sealed class ReportSummaryDto
{
    public Guid AppraisalReportId { get; set; }
    public Guid? ValuationRunId { get; set; }
    public string? ReportType { get; set; }
    public string? ReportVersion { get; set; }
    public string? ReportStatus { get; set; }
    public string? TemplateName { get; set; }
    public DateTime? GeneratedOnUtc { get; set; }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Models/UpdateAppraisalWorkspaceRequest.cs`

```csharp
namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

public sealed class UpdateAppraisalWorkspaceRequest
{
    public Guid AppraisalId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? AppraisalTypeId { get; set; }
    public Guid? AppraisalStatusId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AssignedAppraiserId { get; set; }
    public Guid? AssignedReviewerId { get; set; }
    public string AppraisalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? IntakeStatus { get; set; }
    public string? InspectionStatus { get; set; }
    public string? ValuationStatus { get; set; }
    public string? ReportStatus { get; set; }
    public DateTime? EffectiveDateUtc { get; set; }
    public long RowVersion { get; set; }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalComparablesPage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalComparablesPage : WorkspacePageBase
{
    private readonly Label _count;
    private Guid _appraisalId;

    public AppraisalComparablesPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Comparable Sales"));
        stack.Controls.Add(CreateMuted("Market evidence linked to this appraisal. Detailed selection, similarity, and adjustments remain in the Comparables workspace."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 170;
        card.Margin = new Padding(0, 12, 0, 10);
        _count = new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = AppraisalWorkspaceTheme.HeaderFont(24f),
            ForeColor = AppraisalWorkspaceTheme.Primary
        };
        card.Controls.Add(_count);
        stack.Controls.Add(card);

        var open = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Comparables");
        open.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Comparables, _appraisalId, null, "open");
        stack.Controls.Add(open);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Comparables;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _count.Text = $"{appraisal.ComparableCount}{Environment.NewLine}linked comparables";
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalInspectionPage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalInspectionPage : WorkspacePageBase
{
    private readonly Label _details;
    private readonly Button _openButton;
    private readonly Button _startButton;
    private Guid _appraisalId;
    private Guid? _inspectionId;

    public AppraisalInspectionPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Inspection"));
        stack.Controls.Add(CreateMuted("Inspection is the evidence source for the locked 12-system condition model."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 220;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label { Dock = DockStyle.Fill, Font = AppraisalWorkspaceTheme.BodyFont(10f), ForeColor = AppraisalWorkspaceTheme.TextPrimary };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        _openButton = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Inspection");
        _startButton = AppraisalWorkspaceTheme.CreateSecondaryButton("Start Inspection");
        _openButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Inspection, _appraisalId, _inspectionId, "open");
        _startButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Inspection, _appraisalId, null, "create");
        actions.Controls.Add(_openButton);
        actions.Controls.Add(_startButton);
        stack.Controls.Add(actions);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Inspection;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _inspectionId = appraisal.LatestInspection?.InspectionId;
        _openButton.Enabled = _inspectionId.HasValue && _inspectionId != Guid.Empty;

        var inspection = appraisal.LatestInspection;
        _details.Text = inspection is null
            ? $"Workflow status: {Display(appraisal.InspectionStatus)}{Environment.NewLine}{Environment.NewLine}No latest inspection summary was supplied by the API."
            : string.Join(Environment.NewLine, new[]
            {
                $"Inspection ID: {inspection.InspectionId:D}",
                $"Type: {Display(inspection.InspectionType)}",
                $"Status: {Display(inspection.Status)}",
                $"Location: {Display(inspection.InspectionLocation)}",
                $"Inspected: {inspection.InspectedOnUtc?.ToLocalTime().ToString("MMM d, yyyy h:mm tt") ?? "—"}",
                $"Systems Complete: {inspection.CompletedSystemCount}/{inspection.TotalSystemCount}",
                $"Condition Index: {inspection.ConditionIndex?.ToString("0.00") ?? "—"}",
                $"Red Flags: {inspection.RedFlagCount}"
            });
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalIntakePage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalIntakePage : WorkspacePageBase
{
    private readonly TextBox _appraisalType;
    private readonly TextBox _status;
    private readonly TextBox _intakeStatus;
    private readonly TextBox _inspectionStatus;
    private readonly TextBox _valuationStatus;
    private readonly TextBox _reportStatus;
    private readonly TextBox _clientId;
    private readonly TextBox _appraiserId;
    private readonly TextBox _reviewerId;
    private readonly TextBox _appraisalTypeId;
    private readonly TextBox _appraisalStatusId;
    private readonly DateTimePicker _effectiveDate;
    private bool _binding;

    public AppraisalIntakePage()
    {
        var stack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Intake & Workflow"));
        stack.Controls.Add(CreateMuted("Edit canonical appraisal fields. Save uses optimistic concurrency through row_version."));

        var form = new TableLayoutPanel
        {
            ColumnCount = 4,
            RowCount = 6,
            Width = 930,
            Height = 390,
            BackColor = AppraisalWorkspaceTheme.Surface,
            Padding = new Padding(18),
            Margin = new Padding(0, 12, 0, 0)
        };
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (var i = 0; i < 6; i++)
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));

        _appraisalType = AddTextField(form, 0, 0, "Appraisal Type");
        _status = AddTextField(form, 2, 0, "Overall Status");
        _intakeStatus = AddTextField(form, 0, 1, "Intake Status");
        _inspectionStatus = AddTextField(form, 2, 1, "Inspection Status");
        _valuationStatus = AddTextField(form, 0, 2, "Valuation Status");
        _reportStatus = AddTextField(form, 2, 2, "Report Status");
        _clientId = AddTextField(form, 0, 3, "Client ID");
        _appraiserId = AddTextField(form, 2, 3, "Appraiser ID");
        _reviewerId = AddTextField(form, 0, 4, "Reviewer ID");
        _appraisalTypeId = AddTextField(form, 2, 4, "Type Lookup ID");
        MakeReadOnlyLookup(_appraisalTypeId);
        _appraisalStatusId = AddTextField(form, 0, 5, "Status Lookup ID");
        MakeReadOnlyLookup(_appraisalStatusId);

        var effectiveCaption = CreateFieldCaption("Effective Date");
        form.Controls.Add(effectiveCaption, 2, 5);
        _effectiveDate = new DateTimePicker
        {
            Dock = DockStyle.Fill,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "MMM d, yyyy h:mm tt",
            ShowCheckBox = true,
            Font = AppraisalWorkspaceTheme.BodyFont(),
            Margin = new Padding(4, 9, 4, 9)
        };
        form.Controls.Add(_effectiveDate, 3, 5);

        foreach (var textBox in TextFields())
            textBox.TextChanged += (_, _) => RaiseDirty();
        _effectiveDate.ValueChanged += (_, _) => RaiseDirty();

        stack.Controls.Add(form);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Intake;

    public event EventHandler? DirtyStateChanged;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _binding = true;
        try
        {
            _appraisalType.Text = appraisal.AppraisalType;
            _status.Text = appraisal.Status;
            _intakeStatus.Text = appraisal.IntakeStatus ?? string.Empty;
            _inspectionStatus.Text = appraisal.InspectionStatus ?? string.Empty;
            _valuationStatus.Text = appraisal.ValuationStatus ?? string.Empty;
            _reportStatus.Text = appraisal.ReportStatus ?? string.Empty;
            _clientId.Text = appraisal.ClientId?.ToString("D") ?? string.Empty;
            _appraiserId.Text = appraisal.AssignedAppraiserId?.ToString("D") ?? string.Empty;
            _reviewerId.Text = appraisal.AssignedReviewerId?.ToString("D") ?? string.Empty;
            _appraisalTypeId.Text = appraisal.AppraisalTypeId?.ToString("D") ?? string.Empty;
            _appraisalStatusId.Text = appraisal.AppraisalStatusId?.ToString("D") ?? string.Empty;
            _effectiveDate.Checked = appraisal.EffectiveDateUtc.HasValue;
            if (appraisal.EffectiveDateUtc.HasValue)
                _effectiveDate.Value = appraisal.EffectiveDateUtc.Value.ToLocalTime();
        }
        finally
        {
            _binding = false;
        }
    }

    public UpdateAppraisalWorkspaceRequest CaptureUpdate(AppraisalWorkspaceDto current)
    {
        ArgumentNullException.ThrowIfNull(current);

        if (string.IsNullOrWhiteSpace(_appraisalType.Text))
            throw new InvalidOperationException("Appraisal Type is required.");
        if (string.IsNullOrWhiteSpace(_status.Text))
            throw new InvalidOperationException("Overall Status is required.");

        return new UpdateAppraisalWorkspaceRequest
        {
            AppraisalId = current.AppraisalId,
            VehicleId = current.VehicleId,
            AppraisalTypeId = ParseOptionalGuid(_appraisalTypeId.Text, "Type Lookup ID"),
            AppraisalStatusId = ParseOptionalGuid(_appraisalStatusId.Text, "Status Lookup ID"),
            ClientId = ParseOptionalGuid(_clientId.Text, "Client ID"),
            AssignedAppraiserId = ParseOptionalGuid(_appraiserId.Text, "Appraiser ID"),
            AssignedReviewerId = ParseOptionalGuid(_reviewerId.Text, "Reviewer ID"),
            AppraisalType = _appraisalType.Text.Trim(),
            Status = _status.Text.Trim(),
            IntakeStatus = NullIfWhiteSpace(_intakeStatus.Text),
            InspectionStatus = NullIfWhiteSpace(_inspectionStatus.Text),
            ValuationStatus = NullIfWhiteSpace(_valuationStatus.Text),
            ReportStatus = NullIfWhiteSpace(_reportStatus.Text),
            EffectiveDateUtc = _effectiveDate.Checked ? _effectiveDate.Value.ToUniversalTime() : null,
            RowVersion = current.RowVersion
        };
    }

    private TextBox AddTextField(TableLayoutPanel form, int captionColumn, int row, string caption)
    {
        var label = CreateFieldCaption(caption);
        var textBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Font = AppraisalWorkspaceTheme.BodyFont(),
            Margin = new Padding(4, 12, 4, 12)
        };
        form.Controls.Add(label, captionColumn, row);
        form.Controls.Add(textBox, captionColumn + 1, row);
        return textBox;
    }

    private static void MakeReadOnlyLookup(TextBox textBox)
    {
        textBox.ReadOnly = true;
        textBox.BackColor = AppraisalWorkspaceTheme.Canvas;
        textBox.TabStop = false;
    }

    private static Label CreateFieldCaption(string caption) => new()
    {
        Text = caption,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft,
        Font = AppraisalWorkspaceTheme.BodyFont(9f),
        ForeColor = AppraisalWorkspaceTheme.TextSecondary,
        Margin = new Padding(4)
    };

    private IEnumerable<TextBox> TextFields()
    {
        yield return _appraisalType;
        yield return _status;
        yield return _intakeStatus;
        yield return _inspectionStatus;
        yield return _valuationStatus;
        yield return _reportStatus;
        yield return _clientId;
        yield return _appraiserId;
        yield return _reviewerId;
        yield return _appraisalTypeId;
        yield return _appraisalStatusId;
    }

    private void RaiseDirty()
    {
        if (!_binding)
            DirtyStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private static Guid? ParseOptionalGuid(string? text, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        if (Guid.TryParse(text.Trim(), out var value) && value != Guid.Empty)
            return value;

        throw new InvalidOperationException($"{fieldName} must be a valid GUID or blank.");
    }

    private static string? NullIfWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalOverviewPage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalOverviewPage : WorkspacePageBase
{
    private readonly TableLayoutPanel _cards;
    private readonly Label _lifecycle;
    private readonly Label _vehicle;
    private readonly Label _inspection;
    private readonly Label _valuation;
    private readonly Label _market;
    private readonly Label _report;

    public AppraisalOverviewPage()
    {
        var stack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Appraisal Overview"));
        stack.Controls.Add(CreateMuted("At-a-glance lifecycle context for the subject vehicle, inspection, market evidence, valuation, and report."));

        _cards = new TableLayoutPanel
        {
            ColumnCount = 3,
            RowCount = 2,
            Width = 900,
            Height = 330,
            Margin = new Padding(0, 12, 0, 0)
        };
        for (var i = 0; i < 3; i++)
            _cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
        for (var i = 0; i < 2; i++)
            _cards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        _lifecycle = AddCard(0, 0, "Lifecycle");
        _vehicle = AddCard(1, 0, "Subject Vehicle");
        _inspection = AddCard(2, 0, "Inspection");
        _market = AddCard(0, 1, "Comparables");
        _valuation = AddCard(1, 1, "Valuation");
        _report = AddCard(2, 1, "Report");
        stack.Controls.Add(_cards);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Overview;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _lifecycle.Text = $"Status: {Display(appraisal.Status)}{Environment.NewLine}Intake: {Display(appraisal.IntakeStatus)}";
        _vehicle.Text = appraisal.Vehicle is null
            ? $"Vehicle ID: {appraisal.VehicleId:D}"
            : $"{appraisal.Vehicle.DisplayName}{Environment.NewLine}VIN: {Display(appraisal.Vehicle.Vin)}";
        _inspection.Text = appraisal.LatestInspection is null
            ? $"Status: {Display(appraisal.InspectionStatus)}{Environment.NewLine}No inspection summary loaded"
            : $"Status: {Display(appraisal.LatestInspection.Status)}{Environment.NewLine}Systems: {appraisal.LatestInspection.CompletedSystemCount}/{appraisal.LatestInspection.TotalSystemCount}";
        _market.Text = $"Linked comparables: {appraisal.ComparableCount}";
        _valuation.Text = appraisal.LatestValuation is null
            ? $"Status: {Display(appraisal.ValuationStatus)}{Environment.NewLine}No valuation summary loaded"
            : $"Value: {Money(appraisal.LatestValuation.FinalValue)}{Environment.NewLine}Confidence: {Percent(appraisal.LatestValuation.ConfidenceScore)}";
        _report.Text = appraisal.LatestReport is null
            ? $"Status: {Display(appraisal.ReportStatus)}{Environment.NewLine}No report summary loaded"
            : $"Status: {Display(appraisal.LatestReport.ReportStatus)}{Environment.NewLine}Version: {Display(appraisal.LatestReport.ReportVersion)}";
    }

    private Label AddCard(int column, int row, string title)
    {
        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Dock = DockStyle.Fill;
        card.Margin = new Padding(6);

        var titleLabel = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 30,
            Font = AppraisalWorkspaceTheme.SectionFont(10.5f),
            ForeColor = AppraisalWorkspaceTheme.Primary
        };
        var value = new Label
        {
            Dock = DockStyle.Fill,
            Font = AppraisalWorkspaceTheme.BodyFont(9.5f),
            ForeColor = AppraisalWorkspaceTheme.TextPrimary,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };
        card.Controls.Add(value);
        card.Controls.Add(titleLabel);
        _cards.Controls.Add(card, column, row);
        return value;
    }

    private static string Money(decimal? value) => value?.ToString("C0") ?? "—";
    private static string Percent(decimal? value) => value.HasValue ? $"{value.Value:P0}" : "—";
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalReportPage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalReportPage : WorkspacePageBase
{
    private readonly Label _details;
    private Guid _appraisalId;
    private Guid? _reportId;

    public AppraisalReportPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Report"));
        stack.Controls.Add(CreateMuted("Report generation, preview, version history, and export are delegated to the Reports module while retaining appraisal context."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 220;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label { Dock = DockStyle.Fill, Font = AppraisalWorkspaceTheme.BodyFont(10f), ForeColor = AppraisalWorkspaceTheme.TextPrimary };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        var open = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Reports");
        var generate = AppraisalWorkspaceTheme.CreateSecondaryButton("Generate Report");
        open.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Report, _appraisalId, _reportId, "open");
        generate.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Report, _appraisalId, null, "generate");
        actions.Controls.Add(open);
        actions.Controls.Add(generate);
        stack.Controls.Add(actions);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Report;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _reportId = appraisal.LatestReport?.AppraisalReportId;
        var report = appraisal.LatestReport;

        _details.Text = report is null
            ? $"Workflow status: {Display(appraisal.ReportStatus)}{Environment.NewLine}{Environment.NewLine}No latest report summary was supplied by the API."
            : string.Join(Environment.NewLine, new[]
            {
                $"Report ID: {report.AppraisalReportId:D}",
                $"Type: {Display(report.ReportType)}",
                $"Version: {Display(report.ReportVersion)}",
                $"Status: {Display(report.ReportStatus)}",
                $"Template: {Display(report.TemplateName)}",
                $"Generated: {report.GeneratedOnUtc?.ToLocalTime().ToString("MMM d, yyyy h:mm tt") ?? "—"}"
            });
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalReviewPage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalReviewPage : WorkspacePageBase
{
    private readonly CheckedListBox _checklist;
    private readonly Label _message;

    public AppraisalReviewPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Review Readiness"));
        stack.Controls.Add(CreateMuted("Readiness is derived from loaded workflow context. It does not bypass API-owned business rules."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 300;
        card.Margin = new Padding(0, 12, 0, 10);

        _message = new Label
        {
            Dock = DockStyle.Top,
            Height = 42,
            Font = AppraisalWorkspaceTheme.SectionFont(10.5f),
            ForeColor = AppraisalWorkspaceTheme.Primary
        };
        _checklist = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            CheckOnClick = false,
            Font = AppraisalWorkspaceTheme.BodyFont(10f),
            BackColor = AppraisalWorkspaceTheme.Surface
        };
        card.Controls.Add(_checklist);
        card.Controls.Add(_message);
        stack.Controls.Add(card);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Review;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _checklist.Items.Clear();
        Add("Subject vehicle linked", appraisal.VehicleId != Guid.Empty);
        Add("Intake workflow populated", !string.IsNullOrWhiteSpace(appraisal.IntakeStatus));
        Add("Inspection available", appraisal.LatestInspection is not null);
        Add("Inspection 12-system completion", appraisal.LatestInspection?.CompletedSystemCount >= 12);
        Add("Comparable evidence available", appraisal.ComparableCount > 0);
        Add("Valuation run available", appraisal.LatestValuation is not null);
        Add("No manual review hold", appraisal.LatestValuation?.ManualReviewRequired != true);
        Add("Report workflow initialized", !string.IsNullOrWhiteSpace(appraisal.ReportStatus) || appraisal.LatestReport is not null);

        var complete = Enumerable.Range(0, _checklist.Items.Count).Count(index => _checklist.GetItemChecked(index));
        _message.Text = $"{complete}/{_checklist.Items.Count} readiness checks satisfied";
        _message.ForeColor = complete == _checklist.Items.Count
            ? AppraisalWorkspaceTheme.Success
            : AppraisalWorkspaceTheme.Warning;
    }

    private void Add(string text, bool isComplete)
    {
        var index = _checklist.Items.Add(text);
        _checklist.SetItemChecked(index, isComplete);
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalValuationPage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalValuationPage : WorkspacePageBase
{
    private readonly Label _details;
    private readonly Button _openButton;
    private Guid _appraisalId;
    private Guid? _valuationRunId;

    public AppraisalValuationPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Valuation"));
        stack.Controls.Add(CreateMuted("Latest deterministic valuation summary and confidence context. Trace details remain owned by the Valuation workspace."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 240;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label { Dock = DockStyle.Fill, Font = AppraisalWorkspaceTheme.BodyFont(10f), ForeColor = AppraisalWorkspaceTheme.TextPrimary };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        _openButton = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Valuation");
        var run = AppraisalWorkspaceTheme.CreateSecondaryButton("Run Valuation");
        _openButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Valuation, _appraisalId, _valuationRunId, "open");
        run.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Valuation, _appraisalId, null, "run");
        actions.Controls.Add(_openButton);
        actions.Controls.Add(run);
        stack.Controls.Add(actions);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Valuation;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _valuationRunId = appraisal.LatestValuation?.ValuationRunId;
        _openButton.Enabled = _valuationRunId.HasValue && _valuationRunId != Guid.Empty;

        var valuation = appraisal.LatestValuation;
        _details.Text = valuation is null
            ? $"Workflow status: {Display(appraisal.ValuationStatus)}{Environment.NewLine}{Environment.NewLine}No latest valuation summary was supplied by the API."
            : string.Join(Environment.NewLine, new[]
            {
                $"Run ID: {valuation.ValuationRunId:D}",
                $"Status: {Display(valuation.ValuationStatus ?? valuation.Status)}",
                $"Engine: {Display(valuation.EngineVersion)}",
                $"Condition Index: {valuation.ConditionIndex?.ToString("0.00") ?? "—"}",
                $"Value Range: {Money(valuation.ValueLow)}  –  {Money(valuation.ValueHigh)}",
                $"Final Value: {Money(valuation.FinalValue)}",
                $"Confidence: {Percent(valuation.ConfidenceScore)} {Display(valuation.ConfidenceBand)}",
                $"Manual Review: {(valuation.ManualReviewRequired ? "Required" : "No")}"
            });
    }

    private static string Money(decimal? value) => value?.ToString("C0") ?? "—";
    private static string Percent(decimal? value) => value.HasValue ? value.Value.ToString("P0") : "—";
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/AppraisalVehiclePage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalVehiclePage : WorkspacePageBase
{
    private readonly Label _details;
    private readonly Button _openButton;
    private Guid _appraisalId;
    private Guid _vehicleId;

    public AppraisalVehiclePage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Subject Vehicle"));
        stack.Controls.Add(CreateMuted("The appraisal points to one canonical vehicle record. Vehicle edits remain owned by the Vehicles module."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 280;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label
        {
            Dock = DockStyle.Fill,
            Font = AppraisalWorkspaceTheme.BodyFont(10f),
            ForeColor = AppraisalWorkspaceTheme.TextPrimary,
            Padding = new Padding(4),
            AutoEllipsis = true
        };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        _openButton = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Vehicle");
        _openButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Vehicle, _appraisalId, _vehicleId, "open");
        stack.Controls.Add(_openButton);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Vehicle;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _vehicleId = appraisal.VehicleId;
        var vehicle = appraisal.Vehicle;
        _openButton.Enabled = _vehicleId != Guid.Empty;

        _details.Text = vehicle is null
            ? $"Vehicle ID: {appraisal.VehicleId:D}{Environment.NewLine}{Environment.NewLine}The current API response did not include a nested vehicle summary."
            : string.Join(Environment.NewLine, new[]
            {
                $"Vehicle: {vehicle.DisplayName}",
                $"VIN: {Display(vehicle.Vin)}",
                $"Series: {Display(vehicle.SeriesName)}",
                $"Body Style: {Display(vehicle.BodyStyle)}",
                $"Drivetrain: {Display(vehicle.Drivetrain)}",
                $"Engine: {Display(vehicle.Engine)}",
                $"Transmission: {Display(vehicle.Transmission)}",
                $"Mileage: {(vehicle.Mileage.HasValue ? $"{vehicle.Mileage:N0} {Display(vehicle.OdometerUnit)}" : "—")}",
                $"Condition Score: {vehicle.OverallConditionScore?.ToString("0.00") ?? "—"}",
                $"Red Flags: {(vehicle.HasRedFlags ? "Yes" : "No")}"
            });
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/IAppraisalWorkspacePage.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public interface IAppraisalWorkspacePage
{
    AppraisalWorkspaceSection Section { get; }
    Control View { get; }
    void Bind(AppraisalWorkspaceDto appraisal);
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Pages/WorkspacePageBase.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public abstract class WorkspacePageBase : UserControl, IAppraisalWorkspacePage
{
    protected WorkspacePageBase()
    {
        Dock = DockStyle.Fill;
        BackColor = AppraisalWorkspaceTheme.Canvas;
        AutoScroll = true;
        Padding = new Padding(18);
    }

    public abstract AppraisalWorkspaceSection Section { get; }
    public Control View => this;
    public abstract void Bind(AppraisalWorkspaceDto appraisal);

    public event EventHandler<AppraisalWorkspaceNavigationEventArgs>? NavigationRequested;

    protected void RequestNavigation(
        AppraisalWorkspaceSection target,
        Guid appraisalId,
        Guid? relatedEntityId = null,
        string? action = null)
    {
        NavigationRequested?.Invoke(
            this,
            new AppraisalWorkspaceNavigationEventArgs(target, appraisalId, relatedEntityId, action));
    }

    protected static Label CreateTitle(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = AppraisalWorkspaceTheme.HeaderFont(16f),
        ForeColor = AppraisalWorkspaceTheme.Primary,
        Margin = new Padding(0, 0, 0, 12)
    };

    protected static Label CreateMuted(string text) => new()
    {
        Text = text,
        AutoSize = true,
        MaximumSize = new Size(900, 0),
        Font = AppraisalWorkspaceTheme.BodyFont(9.5f),
        ForeColor = AppraisalWorkspaceTheme.TextSecondary,
        Margin = new Padding(0, 0, 0, 8)
    };

    protected static string Display(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value;
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Services/AppraisalApiException.cs`

```csharp
using System.Net;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public class AppraisalApiException : Exception
{
    public AppraisalApiException(
        HttpStatusCode statusCode,
        string message,
        ApiProblemDetails? problemDetails = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ProblemDetails = problemDetails;
    }

    public HttpStatusCode StatusCode { get; }
    public ApiProblemDetails? ProblemDetails { get; }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Services/AppraisalConcurrencyException.cs`

```csharp
using System.Net;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public sealed class AppraisalConcurrencyException : AppraisalApiException
{
    public AppraisalConcurrencyException(string message, ApiProblemDetails? problemDetails = null)
        : base(HttpStatusCode.Conflict, message, problemDetails)
    {
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Services/AppraisalWorkspaceService.cs`

```csharp
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public sealed class AppraisalWorkspaceService : IAppraisalWorkspaceService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public AppraisalWorkspaceService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
    }

    public async Task<AppraisalWorkspaceDto> GetAsync(
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        EnsureAppraisalId(appraisalId);

        using var response = await _httpClient.GetAsync(
            $"api/appraisals/{appraisalId:D}",
            cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        await EnsureSuccessAsync(response, payload, cancellationToken);

        return ParseWorkspace(payload, appraisalId);
    }

    public async Task<AppraisalWorkspaceDto> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalWorkspaceRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureAppraisalId(appraisalId);
        ArgumentNullException.ThrowIfNull(request);

        if (request.AppraisalId != Guid.Empty && request.AppraisalId != appraisalId)
            throw new ArgumentException("Request appraisal id does not match the route appraisal id.", nameof(request));

        request.AppraisalId = appraisalId;

        using var response = await _httpClient.PutAsJsonAsync(
            $"api/appraisals/{appraisalId:D}",
            request,
            _jsonOptions,
            cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var problem = TryParseProblem(payload);
            throw new AppraisalConcurrencyException(
                problem?.ToDisplayMessage()
                ?? "The appraisal was changed by another user. Refresh the workspace and retry your changes.",
                problem);
        }

        await EnsureSuccessAsync(response, payload, cancellationToken);

        // Some update endpoints legitimately return 204 No Content. Reload so the UI
        // receives the server-owned row_version and timestamps after a successful save.
        return string.IsNullOrWhiteSpace(payload)
            ? await GetAsync(appraisalId, cancellationToken)
            : ParseWorkspace(payload, appraisalId);
    }

    private AppraisalWorkspaceDto ParseWorkspace(string payload, Guid expectedAppraisalId)
    {
        if (string.IsNullOrWhiteSpace(payload))
            throw new AppraisalApiException(
                HttpStatusCode.OK,
                "The Nexus API returned an empty appraisal response.");

        try
        {
            using var document = JsonDocument.Parse(payload);
            var root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
                throw new JsonException("Expected a JSON object.");

            AppraisalWorkspaceDto? result = null;

            if (TryGetProperty(root, "appraisal", out var appraisalElement)
                && appraisalElement.ValueKind == JsonValueKind.Object)
            {
                result = appraisalElement.Deserialize<AppraisalWorkspaceDto>(_jsonOptions);
                if (result is not null)
                    MergeEnvelope(root, result);
            }
            else
            {
                result = root.Deserialize<AppraisalWorkspaceDto>(_jsonOptions);
                if (result is not null)
                    MergeAliases(root, result);
            }

            if (result is null)
                throw new JsonException("Could not deserialize appraisal response.");

            if (result.AppraisalId == Guid.Empty)
                result.AppraisalId = expectedAppraisalId;

            if (result.AppraisalId != expectedAppraisalId)
                throw new JsonException("The API response appraisal id does not match the requested appraisal id.");

            return result;
        }
        catch (AppraisalApiException)
        {
            throw;
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            throw new AppraisalApiException(
                HttpStatusCode.OK,
                "The appraisal response could not be parsed. Verify the WinForms DTO contract matches the API response.",
                innerException: ex);
        }
    }

    private void MergeEnvelope(JsonElement root, AppraisalWorkspaceDto target)
    {
        target.Vehicle ??= DeserializeProperty<VehicleSummaryDto>(root, "vehicle")
            ?? DeserializeProperty<VehicleSummaryDto>(root, "subjectVehicle");
        target.Client ??= DeserializeProperty<ClientSummaryDto>(root, "client");
        target.AssignedAppraiser ??= DeserializeProperty<UserSummaryDto>(root, "assignedAppraiser");
        target.AssignedReviewer ??= DeserializeProperty<UserSummaryDto>(root, "assignedReviewer");
        target.LatestInspection ??= DeserializeProperty<InspectionSummaryDto>(root, "latestInspection")
            ?? DeserializeProperty<InspectionSummaryDto>(root, "inspection");
        target.LatestValuation ??= DeserializeProperty<ValuationSummaryDto>(root, "latestValuation")
            ?? DeserializeProperty<ValuationSummaryDto>(root, "valuation");
        target.LatestReport ??= DeserializeProperty<ReportSummaryDto>(root, "latestReport")
            ?? DeserializeProperty<ReportSummaryDto>(root, "report");

        if (target.ComparableCount == 0)
            target.ComparableCount = ReadInt32(root, "comparableCount") ?? ReadInt32(root, "comparablesCount") ?? 0;
    }

    private void MergeAliases(JsonElement root, AppraisalWorkspaceDto target)
    {
        target.Vehicle ??= DeserializeProperty<VehicleSummaryDto>(root, "subjectVehicle");
        target.LatestInspection ??= DeserializeProperty<InspectionSummaryDto>(root, "inspection");
        target.LatestValuation ??= DeserializeProperty<ValuationSummaryDto>(root, "valuation");
        target.LatestReport ??= DeserializeProperty<ReportSummaryDto>(root, "report");
    }

    private T? DeserializeProperty<T>(JsonElement root, string propertyName)
    {
        return TryGetProperty(root, propertyName, out var element)
               && element.ValueKind is not JsonValueKind.Null and not JsonValueKind.Undefined
            ? element.Deserialize<T>(_jsonOptions)
            : default;
    }

    private static int? ReadInt32(JsonElement root, string propertyName)
    {
        if (!TryGetProperty(root, propertyName, out var element))
            return null;

        if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var value))
            return value;

        if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out value))
            return value;

        return null;
    }

    private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        string payload,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var problem = TryParseProblem(payload);
        var message = problem?.ToDisplayMessage()
            ?? $"The Nexus API returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).";

        await Task.CompletedTask.WaitAsync(cancellationToken);
        throw new AppraisalApiException(response.StatusCode, message, problem);
    }

    private ApiProblemDetails? TryParseProblem(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return null;

        try
        {
            return JsonSerializer.Deserialize<ApiProblemDetails>(payload, _jsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static void EnsureAppraisalId(Guid appraisalId)
    {
        if (appraisalId == Guid.Empty)
            throw new ArgumentException("Appraisal id is required.", nameof(appraisalId));
    }
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Services/IAppraisalWorkspaceService.cs`

```csharp
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public interface IAppraisalWorkspaceService
{
    Task<AppraisalWorkspaceDto> GetAsync(Guid appraisalId, CancellationToken cancellationToken = default);

    Task<AppraisalWorkspaceDto> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalWorkspaceRequest request,
        CancellationToken cancellationToken = default);
}
```

## `ONE_Classics_Nexus.WinForms/Features/Appraisals/Styling/AppraisalWorkspaceTheme.cs`

```csharp
using System.Drawing.Drawing2D;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

public static class AppraisalWorkspaceTheme
{
    // ONE Classics Nexus locked primary theme color.
    public static readonly Color Primary = ColorTranslator.FromHtml("#0E2841");
    public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#173D5E");
    public static readonly Color Accent = ColorTranslator.FromHtml("#2F6F9F");
    public static readonly Color Canvas = ColorTranslator.FromHtml("#F4F7FA");
    public static readonly Color Surface = Color.White;
    public static readonly Color Border = ColorTranslator.FromHtml("#D7E0E8");
    public static readonly Color TextPrimary = ColorTranslator.FromHtml("#1E2933");
    public static readonly Color TextSecondary = ColorTranslator.FromHtml("#667482");
    public static readonly Color Success = ColorTranslator.FromHtml("#2E7D32");
    public static readonly Color Warning = ColorTranslator.FromHtml("#A86400");
    public static readonly Color Danger = ColorTranslator.FromHtml("#B3261E");
    public static readonly Color Muted = ColorTranslator.FromHtml("#6B7785");

    public static Font HeaderFont(float size = 18f) => new("Segoe UI Semibold", size, FontStyle.Bold);
    public static Font SectionFont(float size = 12f) => new("Segoe UI Semibold", size, FontStyle.Bold);
    public static Font BodyFont(float size = 9.5f) => new("Segoe UI", size, FontStyle.Regular);

    public static Button CreatePrimaryButton(string text)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(96, 34),
            BackColor = Primary,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = BodyFont(),
            Cursor = Cursors.Hand,
            Padding = new Padding(10, 2, 10, 2)
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = PrimaryHover;
        return button;
    }

    public static Button CreateSecondaryButton(string text)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(96, 34),
            BackColor = Surface,
            ForeColor = Primary,
            FlatStyle = FlatStyle.Flat,
            Font = BodyFont(),
            Cursor = Cursors.Hand,
            Padding = new Padding(10, 2, 10, 2)
        };
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.MouseOverBackColor = Canvas;
        return button;
    }

    public static Panel CreateCard()
    {
        return new Panel
        {
            BackColor = Surface,
            Padding = new Padding(16),
            Margin = new Padding(6)
        };
    }

    public static Color StatusColor(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return Muted;

        var normalized = status.Trim().Replace("_", string.Empty).Replace("-", string.Empty).ToLowerInvariant();
        return normalized switch
        {
            "complete" or "completed" or "approved" or "issued" or "success" => Success,
            "failed" or "rejected" or "cancelled" or "canceled" or "blocked" => Danger,
            "needsreview" or "pendingreview" or "warning" or "inprogress" => Warning,
            _ => Accent
        };
    }

    public static void DrawRoundedRectangle(Graphics graphics, Pen pen, Rectangle bounds, int radius)
    {
        using var path = CreateRoundedPath(bounds, radius);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.DrawPath(pen, path);
    }

    public static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
    {
        var diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }

}
```
