using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectManagement.Domain.Common;

namespace ProjectManagement.Infrastructure.Persistence.Interceptors;

// Interceptor to automatically set audit fields (CreatedAt, ModifiedAt) before saving changes
public class AuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
    {
        UpdateEntries(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntries(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntries(DbContext context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Where(e => ImplementsBaseEntity(e.Entity.GetType()));

        var now = DateTime.Now;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("CreatedAt").CurrentValue = now;
                    break;

                case EntityState.Modified:
                    entry.Property("ModifiedAt").CurrentValue = now;
                    break;
            }
        }
    }

    private static bool ImplementsBaseEntity(Type entityType)
    {
        while (entityType != null && entityType != typeof(object))
        {
            if (entityType.IsGenericType && entityType.GetGenericTypeDefinition() == typeof(BaseEntity<>))
                return true;

            entityType = entityType.BaseType;
        }
        return false;
    }
}