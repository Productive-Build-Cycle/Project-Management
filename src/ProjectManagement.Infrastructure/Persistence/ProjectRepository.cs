using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Persistence;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectManagementDbContext _context;

    public ProjectRepository(ProjectManagementDbContext context)
    {
        _context = context;
    }
    
    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
    }


    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void Remove(Project project)
    {
        project.Remove();
    }
}
