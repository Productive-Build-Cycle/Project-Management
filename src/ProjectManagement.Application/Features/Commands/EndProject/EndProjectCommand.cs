using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.EndProject
{
    public record EndProjectCommand(
    Guid ProjectId,
    DateTime EndTime
) : IRequest;
}
