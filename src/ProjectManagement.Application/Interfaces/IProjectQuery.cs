using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.Common.Pagination;

namespace ProjectManagement.Application.Interfaces;

public interface IProjectQuery
{
    Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<ProjectDto>> GetAllAsync(FilterParameters filter, CancellationToken cancellationToken = default);
}
