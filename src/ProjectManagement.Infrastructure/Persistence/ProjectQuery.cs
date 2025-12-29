using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.Common.Pagination;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Infrastructure.Common;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Persistence;

public class ProjectQuery(ProjectManagementReadDbContext context) : IProjectQuery
{
    private readonly ProjectManagementReadDbContext _context = context;

    public async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Projects.Where(x => x.Id == id).Select(x => x.Adapt<ProjectDto>()).FirstOrDefaultAsync(cancellationToken);

    public async Task<PagedList<ProjectDto>> GetAllAsync(FilterParameters filter, CancellationToken cancellationToken = default)
        => await _context.Projects.ToPagedList<Project, ProjectDto>(filter, cancellationToken);
}