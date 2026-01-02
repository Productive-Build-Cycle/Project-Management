using MediatR;

namespace ProjectManagement.Application.Features.Commands.EndProject;

public record EndProjectCommand(
Guid ProjectId,
DateTime EndTime
) : IRequest;
