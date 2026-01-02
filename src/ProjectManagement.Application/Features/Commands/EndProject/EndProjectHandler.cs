using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.EndProject;

public class EndProjectHandler(
IProjectRepository projectRepository,
IUnitOfWork unitOfWork)
: IRequestHandler<EndProjectCommand>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        EndProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new ProjectNotFoundException(request.ProjectId);

        project.Finish(request.EndTime);

        await _unitOfWork.CommitAsync();
    }
}
