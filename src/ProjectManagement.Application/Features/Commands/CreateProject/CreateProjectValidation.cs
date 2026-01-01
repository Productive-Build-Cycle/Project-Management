using FluentValidation;
using ProjectManagement.Application.Features.Commands.CreateProject;


public class CreateProjectValidator
    : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DeadlineTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Deadline must be in the future.");
    }
}