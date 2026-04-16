using ONE_Classics_Nexus.Domain.Valuation.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Valuation.Interfaces;

public interface IValuationRunRepository
{
    Task SaveAsync(Guid appraisalId, ValuationResult result, CancellationToken ct);
}