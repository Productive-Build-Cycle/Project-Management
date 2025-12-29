using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Domain.Repositories
{
    public interface ITeamRepository
    {
        Task<bool> ExistsAsync(string teamId);
    }
}
