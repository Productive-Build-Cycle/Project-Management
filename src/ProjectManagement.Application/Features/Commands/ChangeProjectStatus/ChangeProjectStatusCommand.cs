using MediatR;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Application.Features.Commands.ChangeProjectStatus;

public record ChangeProjectStatusCommand(
    Guid Id,
    ProjectStatus Status
) : IRequest;