using MediatR;

namespace ProjectManagement.Application.Features.Commands.StartProject;

public record StartProjectCommand(Guid ProjectId, DateTime StartTime) : IRequest;
