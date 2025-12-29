using ProjectManagement.Application.Services.Interfaces;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Services
{
    public class TeamValidationService : ITeamValidationService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamValidationService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public async Task ValidateTeamExistsAsync(string teamId)
        {
            if (string.IsNullOrWhiteSpace(teamId))
                throw new DomainException("TeamId is required.");

            var exists = await _teamRepository.ExistsAsync(teamId);

            if (!exists)
                throw new DomainException($"Team with id '{teamId}' does not exist.");
        }
    }
}
