using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Infrastructure.DataAccess;

namespace ProjectManagement.Infrastructure.Services;

public class ValidationService(ProjectManagementReadDbContext context) : IValidationService
{
    private readonly ProjectManagementReadDbContext _context = context;

    public async Task<bool> ValidateTeamExistsAsync(string teamId)
    {
        var sql = """
              SELECT COUNT(1)
              FROM Teams
              WHERE Id = @TeamId
              """;

        var parameter = new SqlParameter("@TeamId", teamId);

        var count = await _context.Database
            .ExecuteSqlRawAsync(sql, parameter);

        return count > 0;
    }
}