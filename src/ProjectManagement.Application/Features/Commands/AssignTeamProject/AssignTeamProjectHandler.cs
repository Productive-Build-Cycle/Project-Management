using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Application.Interfaces;

namespace ProjectManagement.Application.Features.Commands.AssignTeamProject
{

    public sealed class AssignTeamToProjectHandler(
            IProjectRepository projectRepository,
            IValidationService validationService,
            IUnitOfWork unitOfWork) : IRequestHandler<AssignTeamToProjectCommand>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IValidationService _validationService = validationService;
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
