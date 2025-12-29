using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Persistence;

// Repository implementation for Project aggregate
public class ProjectRepository : IProjectRepository
{
    private readonly ProjectManagementDbContext _context;

    // Inject DbContext for data access
    public ProjectRepository(ProjectManagementDbContext context)
    {
        _context = context;
    }

    // Get a project by its id
    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Get all projects
    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    // Add a new project to the context
    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    // Mark project as updated
    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    // Remove project using domain logic (soft delete)
    public void Remove(Project project)
    {
        project.Remove();
    }
}