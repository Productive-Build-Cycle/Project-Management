using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.ChangeProjectDeadline;

public class ChangeProjectDeadlineHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeProjectDeadlineCommand>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(
        ChangeProjectDeadlineCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ProjectNotFoundException(request.Id);

        project.ChangeDeadlineTime(request.DeadlineTime);

        await _unitOfWork.CommitAsync();

        return;
    }
}