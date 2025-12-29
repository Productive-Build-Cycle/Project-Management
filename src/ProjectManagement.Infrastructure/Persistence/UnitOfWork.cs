using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Persistence;

// Unit of Work implementation to manage database transactions
public class UnitOfWork : IUnitOfWork
{
    private readonly ProjectManagementDbContext _context;

    // Exposes repositories that share the same DbContext
    public IProjectRepository Projects { get; }

    // Inject DbContext and repositories
    public UnitOfWork(
        ProjectManagementDbContext context,
        IProjectRepository projectRepository)
    {
        _context = context;
        Projects = projectRepository;
    }

    // Commit all changes made through repositories
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Dispose DbContext when the unit of work is completed
    public void Dispose()
    {
        _context.Dispose();
    }
}