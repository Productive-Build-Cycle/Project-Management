using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectManagement.Domain.Common;

namespace ProjectManagement.Infrastructure.Persistence.Interceptors;

// Interceptor to implement soft delete behavior
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
                DbContextEventData eventData,
                InterceptionResult<int> result)
    {
        SoftDeleteEntries(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SoftDeleteEntries(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SoftDeleteEntries(DbContext context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker
            .Entries()
            .Where(e =>
                e.State == EntityState.Deleted &&
                !e.Metadata.IsOwned() &&
                ImplementsRemovableEntity(e.Entity.GetType()));

        foreach (var entry in entries)
        {
            entry.State = EntityState.Unchanged;

            if (entry.Metadata.FindProperty("IsDeleted") is not null)
            {
                entry.Property("IsDeleted").CurrentValue = true;
                entry.Property("IsDeleted").IsModified = true;
            }

            if (entry.Metadata.FindProperty("RemovedAt") is not null)
            {
                entry.Property("RemovedAt").CurrentValue = DateTime.Now;
                entry.Property("RemovedAt").IsModified = true;
            }
        }
    }
    private static bool ImplementsRemovableEntity(Type entityType)
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