using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Common.Interfaces;
using ProjectManagement.Infrastructure.Persistence.Interceptors;
using System.Linq.Expressions;
using System.Reflection;

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
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (!IsRemovableEntity(clrType))
                continue;

            var parameter = Expression.Parameter(clrType, "e");

            var isDeletedProperty = Expression.Property(
                parameter,
                nameof(RemovableEntity<IBaseEntity>.IsDeleted)
            );

            var filterBody = Expression.Equal(
                isDeletedProperty,
                Expression.Constant(false)
            );

            var lambda = Expression.Lambda(filterBody, parameter);

            modelBuilder.Entity(clrType).HasQueryFilter(lambda);
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