using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.StartProject
{
    public class StartProjectCommand : IRequest
    {
        public Guid ProjectId { get; init; }
        public DateTime StartTime { get; init; }
    }
}
