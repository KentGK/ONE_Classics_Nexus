using ONE_Classics_Nexus.Application.Valuation.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Valuation.Interfaces;

public interface IValuationApplicationService
{
    Task<ValuationResultDto> RunAsync(Guid appraisalId, CancellationToken ct);
}