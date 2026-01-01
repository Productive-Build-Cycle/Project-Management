using MediatR;

namespace ProjectManagement.Application.Features.Commands.CreateProject;

public record CreateProjectCommand(
    string Title,
    string Description,
    string? TeamId,
    DateTime DeadlineTime
) : IRequest<Guid>;