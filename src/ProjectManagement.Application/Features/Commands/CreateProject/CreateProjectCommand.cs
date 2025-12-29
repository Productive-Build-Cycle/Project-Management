using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.CreateProject
{
    public record CreateProjectCommand(
      string Title,
      string? Description,
      DateTime DeadlineTime,
      string? TeamId
  ) : IRequest<Guid>;
}
