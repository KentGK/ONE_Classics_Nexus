using ONE_Classics_Nexus.Application.Valuation.DTOs;

namespace ONE_Classics_Nexus.Application.Valuation.Interfaces;

public interface IValuationApplicationService
{
    Task<ValuationResultDto> RunAsync(Guid appraisalId, CancellationToken ct);
}