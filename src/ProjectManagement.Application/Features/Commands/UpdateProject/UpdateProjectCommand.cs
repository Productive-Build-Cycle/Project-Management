using MediatR;

namespace ProjectManagement.Application.Features.Commands;

public record UpdateProjectCommand(
    Guid Id,
    string Title,
    string Description
) : IRequest;