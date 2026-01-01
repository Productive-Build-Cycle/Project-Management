using MediatR;

namespace ProjectManagement.Application.Features.Commands.UpdateProject;

public record UpdateProjectCommand(
    Guid Id,
    string Title,
    string Description
) : IRequest;