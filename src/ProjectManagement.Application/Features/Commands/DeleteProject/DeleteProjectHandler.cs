using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ProjectNotFoundException(request.Id);

        _projectRepository.Delete(project);
        await _unitOfWork.CommitAsync();
        return;
    }
}