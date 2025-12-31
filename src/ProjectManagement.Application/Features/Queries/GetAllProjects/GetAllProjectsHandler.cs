using Mapster;
using MediatR;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.Common.Pagination;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Exceptions;

namespace ProjectManagement.Application.Features.Queries.GetProjectById;

public class GetAllProjectsHandler(IProjectQuery projectQuery) : IRequestHandler<GetAllProjectsQuery, PagedList<ProjectDto>>
{
    private readonly IProjectQuery _projectQuery = projectQuery;

    public async Task<PagedList<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        => await _projectQuery.GetAllAsync(request.Adapt<FilterParameters>(), cancellationToken);
}
