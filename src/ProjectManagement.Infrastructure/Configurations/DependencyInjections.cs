using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;
using ProjectManagement.Infrastructure.Persistence;

namespace ProjectManagement.Infrastructure.Configurations;

public static class DependencyInjections
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        #region Database

        services.AddDbContext<ProjectManagementDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("WriteAndReadConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
        });

        #endregion
        
        #region Repositories & UnitOfWork
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region InternalServices

        #endregion

        #region Behaviors

        #endregion

        #region Packages

        #endregion

        return services;
    }
}
