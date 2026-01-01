using MediatR;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.CreateProject;

public class CreateProjectHandler(
    IProjectRepository projectRepository,
    IValidationService validationService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IValidationService _validationService = validationService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.TeamId) && !await _validationService.ValidateTeamExistsAsync(request.TeamId))
            throw new Exception(request.TeamId);
        else if (await _projectRepository.TitleExistsAsync(request.Title, cancellationToken))        
            throw new DuplicateProjectTitleException(request.Title);
        
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