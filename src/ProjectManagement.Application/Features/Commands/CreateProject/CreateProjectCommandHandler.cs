using MediatR;
using ProjectManagement.Application.Services.Interfaces;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.CreateProject
{
    public class CreateProjectCommandHandler:IRequestHandler<CreateProjectCommand,Guid>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamValidationService _teamValidationService;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            ITeamValidationService teamValidationService)
        {
            _projectRepository = projectRepository;
            _teamValidationService = teamValidationService;
        }

        public async Task<Guid> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.TeamId))
            {
                await _teamValidationService
                    .ValidateTeamExistsAsync(request.TeamId);
            }

            var project = new Project(
                title: request.Title,
                deadlineTime: request.DeadlineTime,
                description: request.Description,
                teamId: request.TeamId
            );

            //await _projectRepository.AddAsync(project);

            return project.Id;
        }
    }
}
