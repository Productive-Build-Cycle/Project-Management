using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.Common.Pagination;
using ProjectManagement.Application.Features.Queries.GetProjectById;

namespace ProjectManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Retrieves a single project by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (GUID) of the project.</param>
    /// <returns>
    /// Returns <see cref="ProjectDto"/> if the project exists.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProjectByIdQuery(id), cancellationToken));


    /// <summary>
    /// Retrieves a paginated list of projects based on filter criteria.
    /// </summary>
    /// <param name="filter">Filter parameters for pagination, sorting, and searching.</param>
    /// <returns>
    /// Returns a <see cref="PagedList{ProjectDto}"/> containing the filtered list of projects.
    /// </returns>
    [HttpPost("get-all")]
    [ProducesResponseType(typeof(PagedList<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromBody] FilterParameters filter, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(filter.Adapt<GetAllProjectsQuery>(), cancellationToken));
}
