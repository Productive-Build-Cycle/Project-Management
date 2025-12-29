using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProjectManagementDbContext _context;

    public IProjectRepository Projects { get; }

    public UnitOfWork(
        ProjectManagementDbContext context,
        IProjectRepository projectRepository)
    {
        _context = context;
        Projects = projectRepository;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
