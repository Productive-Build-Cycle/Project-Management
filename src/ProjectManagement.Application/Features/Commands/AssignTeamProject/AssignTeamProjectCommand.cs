using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.AssignTeamProject
{
    public sealed record AssignTeamToProjectCommand(
     Guid ProjectId,
     string TeamId
 ) : IRequest;
}
