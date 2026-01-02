using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.EndProject
{
    public class EndProjectValidation : AbstractValidator<EndProjectCommand>
    {
        public EndProjectValidation()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("ProjectId is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.");
        }
    }
}
