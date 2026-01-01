using FluentValidation;
using ProjectManagement.Application.Features.Commands.ChangeProjectDeadline;

public class ChangeProjectDeadlineValidator
    : AbstractValidator<ChangeProjectDeadlineCommand>
{
    public ChangeProjectDeadlineValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.DeadlineTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Deadline must be in the future.");
    }
}