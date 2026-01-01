using MediatR;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Features.Commands.StartProject
{

    public class StartProjectHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<StartProjectCommand>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            StartProjectCommand request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("StartProjectHandler called!");

            var project = await _projectRepository
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
                throw new Exception("Project not found");

            project.Start(request.StartTime);

            await _unitOfWork.CommitAsync();
        }
    }
}
