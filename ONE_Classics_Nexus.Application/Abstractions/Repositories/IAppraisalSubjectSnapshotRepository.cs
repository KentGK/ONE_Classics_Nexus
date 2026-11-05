using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IAppraisalSubjectSnapshotRepository
{
    Task<AppraisalSubjectSnapshot?> GetByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);

    Task<AppraisalSubjectSnapshot> InsertAsync(
        AppraisalSubjectSnapshot snapshot,
        CancellationToken cancellationToken);

    Task<AppraisalSubjectSnapshot?> UpdateAsync(
        AppraisalSubjectSnapshot snapshot,
        CancellationToken cancellationToken);

    /// <summary>
    /// Removes the snapshot belonging to an appraisal. Used for compensating cleanup
    /// when appraisal creation must be rolled back.
    /// </summary>
    /// <returns><c>true</c> when a row was deleted.</returns>
    Task<bool> DeleteByAppraisalIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken);
}
