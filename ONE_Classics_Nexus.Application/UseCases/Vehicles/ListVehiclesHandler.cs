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

        // ERROR: IVehicleRepository does not have a ListAsync method. You must implement ListAsync in IVehicleRepository and its implementation, or replace this call with available repository methods such as GetByIdAsync or CreateAsync, or refactor the logic accordingly.

        var dtos = items.Select(v => v.ToDto()).ToList();

        return new PagedResult<VehicleDto>(dtos, page, pageSize, totalCount);
    }
}