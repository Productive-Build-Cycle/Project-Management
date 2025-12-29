using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Interfaces.Persistence;

// Defines a unit of work for coordinating repositories
public interface IUnitOfWork : IDisposable
{
    // Commit all pending changes to the database
    Task<int> CommitAsync();
}