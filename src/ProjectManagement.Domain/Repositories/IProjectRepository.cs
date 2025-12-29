using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<List<Project>> GetAllAsync();
    Task AddAsync(Project project);
    void Update(Project project);
    void Remove(Project project);
}