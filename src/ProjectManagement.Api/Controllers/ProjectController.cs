#region Usings
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Models.Requests;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Commands.AssignTeamProject;
using ProjectManagement.Application.Features.Commands.ChangeProjectDeadline;
using ProjectManagement.Application.Features.Commands.CreateProject;
using ProjectManagement.Application.Features.Commands.DeleteProject;
using ProjectManagement.Application.Features.Commands.EndProject;
using ProjectManagement.Application.Features.Commands.StartProject;
using ProjectManagement.Application.Features.Commands.UpdateProject;
using ProjectManagement.Application.Features.Queries.Common.Pagination;
using ProjectManagement.Application.Features.Queries.GetAllProjects;
using ProjectManagement.Application.Features.Queries.GetProjectById;
#endregion
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

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="request">The data required to create a project.</param>
    /// <returns>
    /// The unique identifier (GUID) of the newly created project.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateProjectCommand>();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates the title and description of an existing project.
    /// </summary>
    /// <param name="request">The project content update data.</param>
    [HttpPut("change-content")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectDto request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateProjectCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Updates the deadline of an existing project.
    /// </summary>
    /// <param name="request">Project identifier and new deadline date.</param>
    [HttpPut("change-deadline")]
    public async Task<IActionResult> ChangeDeadlineTime([FromBody] ChangeProjectDeadlineTimeDto request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<ChangeProjectDeadlineCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Assigns a team to a project.
    /// </summary>
    /// <param name="request">Project identifier and team ID.</param>
    [HttpPut("assign-team")]
    public async Task<IActionResult> AssignTeam([FromBody] AssignTeamToProjectDto request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AssignTeamToProjectCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a project by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (GUID) of the project.</param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProjectCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Starts a project.
    /// </summary>
    /// <param name="request">Project identifier and start date.</param>
    [HttpPut("start")]
    public async Task<IActionResult> StartProject([FromBody] StartProjectDto request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<StartProjectCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Ends a project.
    /// </summary>
    /// <param name="request">Project identifier and end date.</param>
    [HttpPut("end")]
    public async Task<IActionResult> EndProject([FromBody] EndProjectDto request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<EndProjectCommand>();
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}