using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Infrastructure.DataAccess;
using System.Data;

namespace ProjectManagement.Infrastructure.Services;

public class ValidationService(ProjectManagementReadDbContext context) : IValidationService
{
    private readonly ProjectManagementReadDbContext _context = context;

    public async Task<bool> ValidateTeamExistsAsync(string teamId)
    {
        if (!Guid.TryParse(teamId, out var teamGuid))
            return false;

        await using var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();

        command.CommandText = """
        SELECT 1
        FROM Teams
        WHERE Id = @TeamId
        """;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@TeamId";
        parameter.DbType = DbType.Guid;
        parameter.Value = teamGuid;
        command.Parameters.Add(parameter);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return result != null;
    }
}