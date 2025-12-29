using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Application.Services.Interfaces
{
    public interface ITeamValidationService
    {
        Task ValidateTeamExistsAsync(string teamId);
    }
}
