using MediatR;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Application.Features.Commands.ChangeProjectDeadline;

public record ChangeProjectDeadlineCommand(
    Guid Id,
    DateTime DeadlineTime
) : IRequest;