using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.UpdateProject;

public class UpdateProjectHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("UpdateProjectHandler called!");

        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);

        if (project is null)
            throw new Exception("Project not found");

        project.SetTitle(request.Title);
        project.SetDescription(request.Description);
        project.ChangeDeadlineTime(request.DeadlineTime);

        await _unitOfWork.CommitAsync();
    }
}