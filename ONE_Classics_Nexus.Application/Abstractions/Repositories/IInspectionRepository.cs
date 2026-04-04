using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IInspectionRepository
{
    Task<Inspection> CreateAsync(Inspection inspection, CancellationToken ct);
    Task<Inspection?> GetByIdAsync(Guid inspectionId, CancellationToken ct);
    Task SaveScoresAsync(Guid inspectionId, IReadOnlyList<ConditionScore> scores, CancellationToken ct);
}