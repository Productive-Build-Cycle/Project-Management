using MediatR;

namespace ProjectManagement.Application.Features.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : IRequest;