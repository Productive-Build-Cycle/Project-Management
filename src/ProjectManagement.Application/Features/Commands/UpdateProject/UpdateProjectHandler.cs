using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
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
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ProjectNotFoundException(request.Id);

        project.SetTitle(request.Title);
        project.SetDescription(request.Description);

        await _unitOfWork.CommitAsync();
    }
}