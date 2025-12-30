using MediatR;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.CreateProject;

public class CreateProjectHandler(IProjectRepository projectRepository) : IRequestHandler<CreateProjectQuery, Guid>
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Guid> Handle(CreateProjectQuery request, CancellationToken cancellationToken)
    {
        var existingProject = await _projectRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingProject is not null)
            throw new InvalidOperationException("Project with the same name already exists.");

        var project = new Project(
            title: request.Name,
            deadlineTime: request.DeadlineTime,
            description: request.Description,
            teamId: request.TeamId.ToString()
        );

        await _projectRepository.AddAsync(project, cancellationToken);

        return project.Id;
    }
}