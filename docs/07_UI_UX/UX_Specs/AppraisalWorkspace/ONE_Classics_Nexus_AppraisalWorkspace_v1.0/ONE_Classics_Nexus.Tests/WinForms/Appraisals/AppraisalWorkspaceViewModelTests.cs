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
