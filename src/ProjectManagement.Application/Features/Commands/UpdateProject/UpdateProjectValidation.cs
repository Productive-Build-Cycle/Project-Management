using FluentValidation;
using ProjectManagement.Application.Features.Commands;

public class UpdateProjectValidator
    : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}