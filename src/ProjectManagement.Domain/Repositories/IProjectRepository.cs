using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Domain.Repositories;

// Repository contract for Project aggregate
public interface IProjectRepository
{
    // Get a project by its id
    Task<Project?> GetByIdAsync(Guid id);

    // Get all projects
    Task<List<Project>> GetAllAsync();

    // Add a new project
    Task AddAsync(Project project);

    // Update an existing project
    void Update(Project project);

    // Remove a project using domain logic (soft delete)
    void Remove(Project project);
}