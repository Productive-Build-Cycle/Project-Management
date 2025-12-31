using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.AssignTeamProject
{

    public sealed class AssignTeamToProjectHandler
    : IRequestHandler<AssignTeamToProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignTeamToProjectHandler(
            IProjectRepository projectRepository,
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AssignTeamToProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
                throw new ProjectNotFoundException(request.ProjectId);

            var teamExists = await _teamRepository
                .ExistsAsync(request.TeamId);

            if (!teamExists)
                throw new Exception(request.TeamId);

            project.AssignToTeam(request.TeamId);

            await _unitOfWork.CommitAsync();
        }
    }
}
