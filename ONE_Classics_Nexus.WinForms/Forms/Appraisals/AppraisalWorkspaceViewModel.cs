using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Services;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals;

public sealed class AppraisalWorkspaceViewModel
{
    private readonly IAppraisalWorkspaceService _service;

    public AppraisalWorkspaceViewModel(IAppraisalWorkspaceService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public AppraisalWorkspaceState State { get; } = new();

    public event EventHandler? StateChanged;

    public async Task LoadAsync(
        Guid appraisalId, 
        CancellationToken cancellationToken = default)
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

    public Task ReloadAsync(
        CancellationToken cancellationToken = default)
    {
        if (State.Appraisal is null)
            throw new InvalidOperationException("There is no loaded appraisal to refresh.");

        return LoadAsync(State.Appraisal.AppraisalId, cancellationToken);
    }

    public void SetActiveSection(
        AppraisalWorkspaceSection section)
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
