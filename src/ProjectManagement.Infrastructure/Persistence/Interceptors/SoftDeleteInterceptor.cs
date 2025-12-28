using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Common.Interfaces;

namespace ProjectManagement.Infrastructure.Persistence.Interceptors;

// Interceptor to implement soft delete behavior
public class SoftDeleteInterceptor : SaveChangesInterceptor
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

        // Iterate through tracked entities of type RemovableEntity<IBaseEntity>
        foreach (var entry in context.ChangeTracker.Entries<RemovableEntity<IBaseEntity>>())
        {
            if (entry.State == EntityState.Deleted)
            {
                // Instead of physically deleting, mark the entity as removed and update ModifiedAt
                entry.State = EntityState.Modified;
                entry.Entity.SoftDelete(now);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}