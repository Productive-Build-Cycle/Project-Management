using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.EndProject
{
    public class EndProjectHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<EndProjectCommand>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            EndProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
                throw new Exception("Project not found");

            project.Finish(request.EndTime);

            await _unitOfWork.CommitAsync();
        }
    }
}
