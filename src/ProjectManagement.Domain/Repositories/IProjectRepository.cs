using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Domain.Repositories;

// Repository contract for Project aggregate
public interface IProjectRepository
{
    // Get a project by its id
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Get a project by its name
    Task<Project?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    // Add a new project
    Task<Guid> AddAsync(Project project, CancellationToken cancellationToken = default);

    // Update an existing project
    void Update(Project project);
    void Delete(Project project);
}