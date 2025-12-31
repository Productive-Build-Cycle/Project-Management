using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.CreateProject;

public class CreateProjectHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("CreateProjectHandler called!"); 
        var project = new Project(
            request.Title,
            request.DeadlineTime,
            request.Description,
            request.TeamId
        );

        await _projectRepository.AddAsync(project);
        await _unitOfWork.CommitAsync();

        return project.Id;
    }
}