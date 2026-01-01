using FluentValidation;
using ProjectManagement.Application.Features.Commands.ChangeProjectStatus;


public class ChangeProjectStatusValidator
    : AbstractValidator<ChangeProjectStatusCommand>
{
    public ChangeProjectStatusValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid project status");
    }
}