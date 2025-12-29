using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Persistence;

// Repository implementation for Project aggregate
public class ProjectRepository(ProjectManagementWriteDbContext context) : IProjectRepository
{
    private readonly ProjectManagementWriteDbContext _context = context;

    // Get a project by its id
    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id,cancellationToken);
    }

    // Add a new project to the context
    public async Task<Guid> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        return project.Id;
    }

    // Mark project as updated
    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }
}