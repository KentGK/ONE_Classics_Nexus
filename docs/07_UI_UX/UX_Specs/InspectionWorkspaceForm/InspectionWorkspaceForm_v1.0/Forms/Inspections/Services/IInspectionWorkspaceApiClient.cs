using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.WinForms.Forms.Inspections.Contracts;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections.Services;

public interface IInspectionWorkspaceApiClient : IDisposable
{
    Task<IReadOnlyList<InspectionListItemResponse>> GetInspectionListAsync(
        CancellationToken cancellationToken = default);

    Task<InspectionWorkspaceResponse> GetWorkspaceAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default);

    Task UpdateInspectionAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken = default);

    Task SaveConditionScoresAsync(
        Guid inspectionId,
        SaveConditionScoresRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteInspectionAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default);
}
