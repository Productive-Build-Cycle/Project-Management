using MediatR;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Exceptions;

namespace ProjectManagement.Application.Features.Queries.GetProjectById;

public class GetProjectByIdHandler(IProjectQuery projectQuery) : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IProjectQuery _projectQuery = projectQuery;

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        => await _projectQuery.GetByIdAsync(request.id , cancellationToken) ?? throw new ProjectNotFoundException(request.id);
}
