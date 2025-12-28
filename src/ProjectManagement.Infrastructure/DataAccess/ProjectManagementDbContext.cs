using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Common;
using System.Linq.Expressions;
using System.Reflection;
using ProjectManagement.Infrastructure.Persistence.Interceptors;

namespace ProjectManagement.Infrastructure.DataAccess;

public class ProjectManagementDbContext : DbContext
{
    public ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options)
        : base(options)
    {
    }

    #region DbSets
    public DbSet<Project> Projects => Set<Project>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ApplySoftDeleteQueryFilter(modelBuilder);
    }

    #region Global Query Filter
    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var clrType = entityType.ClrType;

            if (!IsRemovableEntity(clrType))
                continue;

            var parameter = Expression.Parameter(clrType, "e");
            var isRemovedProperty = Expression.Property(parameter, "IsDeleted");

            var filterExpression = Expression.Lambda(
                Expression.Equal(isRemovedProperty, Expression.Constant(false)),
                parameter
            );

            modelBuilder.Entity(clrType).HasQueryFilter(filterExpression);
        }
    }
    private static bool IsRemovableEntity(Type type)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(RemovableEntity<>))
                return true;

            type = type.BaseType!;
        }

        return false;
    }
    #endregion
    
    #region Interceptor
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            new AuditInterceptor(),
            new SoftDeleteInterceptor()
        );
    }

    #endregion
}