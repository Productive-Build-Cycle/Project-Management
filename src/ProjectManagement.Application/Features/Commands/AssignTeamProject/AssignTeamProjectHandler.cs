using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;

namespace ProjectManagement.Application.Features.Commands.AssignTeamProject
{

    public sealed class AssignTeamToProjectHandler(
            IProjectRepository projectRepository,
            ValidationService validationService,
            IUnitOfWork unitOfWork) : IRequestHandler<AssignTeamToProjectCommand>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly ValidationService _validationService = validationService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            AssignTeamToProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken) ?? throw new ProjectNotFoundException(request.ProjectId);

            if (!await _validationService.ValidateTeamExistsAsync(request.TeamId))
                throw new Exception(request.TeamId);

            project.AssignToTeam(request.TeamId);

            await _unitOfWork.CommitAsync();
        }
    }
}
