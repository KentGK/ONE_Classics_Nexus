using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Clients;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/clients")]
public sealed class ClientsController : ControllerBase
{
    private readonly IClientRepository _repository;

    public ClientsController(IClientRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// List clients for the Client List, with search, filtering, sorting and paging.
    /// </summary>
    /// <remarks>
    /// Returns only Active clients unless a status is supplied. Pass
    /// statusCode=All to return every status.
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> List(
        [FromServices] IClientApiService service,
        [FromQuery] string? search,
        [FromQuery] string? clientTypeCode,
        [FromQuery] string? statusCode,
        [FromQuery] ClientSortBy sortBy = ClientSortBy.DisplayName,
        [FromQuery] ClientSortDirection sortDirection = ClientSortDirection.Ascending,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var request = new ListClientsRequest
        {
            Search = search,
            ClientTypeCode = clientTypeCode,
            // An omitted statusCode must keep the Active default, so only
            // overwrite it when the caller actually supplied a value.
            StatusCode = string.IsNullOrWhiteSpace(statusCode)
                ? ClientStatusCodes.Active
                : statusCode,
            SortBy = sortBy,
            SortDirection = sortDirection,
            Page = page,
            PageSize = pageSize
        };

        var result = await service.ListAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new client
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] IClientApiService service,
        [FromBody] CreateClientRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(
            request, 
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { clientId = result.ClientId },
            result);
    }

    /// <summary>
    /// Get a client by ID
    /// </summary>
    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetById(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var client = await _repository.GetByIdAsync(
            clientId,
            cancellationToken);

        return client is null ? NotFound() : Ok(client);
    }

    /// <summary>
    /// Update an existing client
    /// </summary>
    [HttpPut("{clientId:guid}")]
    public async Task<IActionResult> Update(
        [FromServices] IClientApiService service,
        Guid clientId,
        [FromBody] UpdateClientRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await service.UpdateAsync(
            clientId,
            request,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    /// <summary>
    /// Delete a client
    /// </summary>
    [HttpDelete("{clientId:guid}")]
    public async Task<IActionResult> Delete(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(
            clientId,
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Create a contact for a client
    /// </summary>
    [HttpPost("{clientId:guid}/contacts")]
    public async Task<IActionResult> CreateContact(
        Guid clientId,
        [FromBody] ClientContact contact,
        CancellationToken cancellationToken)
    {
        if (contact.ClientId != clientId)
        {
            return BadRequest("The route client ID must match the request body client ID.");
        }

        var result = await _repository.CreateContactAsync(
            contact,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetContactById),
            new { clientContactId = result.ClientContactId },
            result);
    }

    /// <summary>
    /// Get all contacts for a client
    /// </summary>
    [HttpGet("{clientId:guid}/contacts")]
    public async Task<IActionResult> GetContacts(
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var contacts = await _repository.GetContactsAsync(
            clientId,
            cancellationToken);

        return Ok(contacts);
    }

    /// <summary>
    /// Get a specific client contact by ID
    /// </summary>
    [HttpGet("contacts/{clientContactId:guid}")]
    public async Task<IActionResult> GetContactById(
        Guid clientContactId,
        CancellationToken cancellationToken)
    {
        var contact = await _repository.GetContactByIdAsync(
            clientContactId,
            cancellationToken);

        return contact is null ? NotFound() : Ok(contact);
    }

    /// <summary>
    /// Update an existing client contact
    /// </summary>
    [HttpPut("contacts/{clientContactId:guid}")]
    public async Task<IActionResult> UpdateContact(
        Guid clientContactId,
        [FromBody] ClientContact contact,
        CancellationToken cancellationToken)
    {
        if (contact.ClientContactId != clientContactId)
        {
            return BadRequest(
                "The route contact ID must match the request body contact ID.");
        }

        var result = await _repository.UpdateContactAsync(
            contact,
            cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Delete a client contact
    /// </summary>
    [HttpDelete("contacts/{clientContactId:guid}")]
    public async Task<IActionResult> DeleteContact(
        Guid clientContactId,
        CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteContactAsync(
            clientContactId,
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}