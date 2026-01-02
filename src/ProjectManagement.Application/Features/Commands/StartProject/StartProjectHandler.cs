using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.StartProject;


public class StartProjectHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<StartProjectCommand>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        StartProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new ProjectNotFoundException(request.ProjectId);

        project.Start(request.StartTime);

        await _unitOfWork.CommitAsync();
    }
}
