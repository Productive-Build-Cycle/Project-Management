using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.StartProject
{
    public class StartProjectValidation : AbstractValidator<StartProjectCommand>
    {
        public StartProjectValidation()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("ProjectId is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");
        }
    }
}
