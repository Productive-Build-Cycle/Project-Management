using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Infrastructure.Persistence
{
    //public class TeamRepository : ITeamRepository
    //{
    //    //private readonly ProjectManagementDbContext _context;
    //    //public TeamRepository(ProjectManagementDbContext context)
    //    //{
    //    //    _context = context;
    //    //}
    //    //public async Task<bool> ExistsAsync(string teamId)
    //    //{
    //    //    var sql = """
    //    //          SELECT COUNT(1)
    //    //          FROM Teams
    //    //          WHERE Id = @TeamId
    //    //          """;

    //    //    var parameter = new SqlParameter("@TeamId", teamId);

    //    //    var count = await _context.Database
    //    //        .ExecuteSqlRawAsync(sql, parameter);

    //    //    return count > 0;
    //    //}
    //}
    
}
