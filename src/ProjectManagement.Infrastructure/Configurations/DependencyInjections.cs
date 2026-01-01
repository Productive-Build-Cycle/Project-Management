using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Application.Common.Behaviors;
using ProjectManagement.Application.Features.Queries.GetProjectById;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Repositories;
using ProjectManagement.Infrastructure.DataAccess;
using ProjectManagement.Infrastructure.Persistence;
using ProjectManagement.Infrastructure.Persistence.Interceptors;
using ProjectManagement.Infrastructure.Services;

namespace ProjectManagement.Infrastructure.Configurations;

public static class DependencyInjections
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services,
        IConfiguration configuration)
    {
        #region Database
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<ProjectManagementWriteDbContext>((serviceProvider, options) =>
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

        services.AddDbContext<ProjectManagementReadDbContext>((serviceProvider, options) =>
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

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        #endregion

        #region Internal Services
        services.AddScoped<IProjectQuery, ProjectQuery>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IValidationService, ValidationService>();
        #endregion

        #region Behaviors

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        #endregion

        #region Packages

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProjectByIdQuery).Assembly));
        services.AddValidatorsFromAssembly(typeof(GetProjectByIdQuery).Assembly);
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        #endregion

        return services;
    }
}