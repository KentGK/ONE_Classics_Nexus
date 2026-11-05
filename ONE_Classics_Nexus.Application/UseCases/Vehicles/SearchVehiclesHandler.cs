using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class SearchVehiclesHandler
{
    private readonly IVehicleRepository _repository;

    public SearchVehiclesHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<VehicleSummaryDto>> HandleAsync(
        SearchVehiclesRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.YearFrom.HasValue && request.YearFrom < 1886)
        {
            throw new ArgumentOutOfRangeException(nameof(request.YearFrom), "YearFrom must be greater than or equal to 1886.");
        }

        if (request.YearTo.HasValue && request.YearTo > 2100)
        {
            throw new ArgumentOutOfRangeException(nameof(request.YearTo), "YearTo must be less than or equal to 2100.");
        }

        if (request.YearFrom.HasValue && request.YearTo.HasValue && request.YearFrom > request.YearTo)
        {
            throw new ArgumentException("YearFrom must be less than or equal to YearTo.");
        }

        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

        request.Page = page;
        request.PageSize = pageSize;

        var (items, totalCount) = await _repository.SearchAsync(request, cancellationToken);
        var dtos = items.Select(v => v.ToSummaryDto()).ToList();

        return new PagedResult<VehicleSummaryDto>(dtos, page, pageSize, totalCount);
    }
}
