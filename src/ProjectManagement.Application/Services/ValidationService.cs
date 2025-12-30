
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ProjectManagement.Application.Services
{
    public class ValidationService
    {
        private readonly string _connectionString;

        public ValidationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task ValidateTeamExistsAsync(string teamId)
        {
            const string query =
                "SELECT COUNT(1) FROM Teams WHERE Id = @TeamId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TeamId", teamId);

            await connection.OpenAsync();

            var exists = (int)await command.ExecuteScalarAsync() > 0;

            if (!exists)
                throw new Exception("Team does not exist.");
        }

    }
}

