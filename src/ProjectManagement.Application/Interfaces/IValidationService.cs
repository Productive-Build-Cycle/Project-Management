using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.Common.Pagination;

namespace ProjectManagement.Application.Interfaces;

public interface IValidationService
{
    Task<bool> ValidateTeamExistsAsync(string teamId);
}
