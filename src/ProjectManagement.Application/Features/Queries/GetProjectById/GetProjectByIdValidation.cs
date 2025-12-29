using FluentValidation;

namespace ProjectManagement.Application.Features.Queries.GetProjectById;

public class GetProjectByIdValidator : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdValidator()
    {
        RuleFor(x => x.id)
            .NotEmpty()
            .WithMessage("Project Id is required.");
    }
}
