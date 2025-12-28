using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Common.Interfaces;

namespace ProjectManagement.Infrastructure.Persistence.Interceptors;

// Interceptor to automatically set audit fields (CreatedAt, ModifiedAt) before saving changes
public class AuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var now = DateTime.UtcNow;

        // Iterate through tracked entities of type BaseEntity<IBaseEntity>
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity<IBaseEntity>>())
        {
            if (entry.State == EntityState.Added)
            {
                // Set CreatedAt timestamp when a new entity is added
                entry.Entity.SetCreatedAt(now);
            }

            else if (entry.State == EntityState.Modified)
            {
                // Update ModifiedAt timestamp when an existing entity is modified
                entry.Entity.SetModifiedAt(now);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}