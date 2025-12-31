using MediatR;
using ProjectManagement.Application.DTOs;

namespace ProjectManagement.Application.Features.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid id) : IRequest<ProjectDto>;