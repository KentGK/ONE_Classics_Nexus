using ONE_Classics_Nexus.Application.Abstractions.Paging;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Entities; // <-- Add this using directive

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class ListVehiclesHandler
{
    private readonly IVehicleRepository _repository;

    public ListVehiclesHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<VehicleDto>> HandleAsync(
        ListVehiclesRequest request,
        CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

        throw new NotImplementedException("IVehicleRepository does not support listing vehicles. Implement a ListAsync method in IVehicleRepository and its implementation, or refactor this handler to use available repository methods.");

        // TODO: Implement fetching items from repository, e.g.:
        // var items = await _repository.ListAsync(page, pageSize, cancellationToken);
        // var dtos = items.Select(v => v.ToDto()).ToList();

        return new PagedResult<VehicleDto>(dtos, page, pageSize, totalCount);
    }
}