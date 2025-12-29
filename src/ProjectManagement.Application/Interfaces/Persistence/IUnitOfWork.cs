using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IProjectRepository Projects { get; }
    Task<int> CommitAsync();
}
