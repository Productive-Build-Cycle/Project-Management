// Base entity class with soft delete capability
namespace ProjectManagement.Domain.Common;

public abstract class RemovableEntity<T> : BaseEntity<T>
{
    // Indicates whether the entity is soft-deleted
    public bool IsDeleted { get; private set; }
    // Timestamp when the entity was soft-deleted
    public DateTime? RemovedAt { get; private set; }

    // Marks the entity as deleted without removing it from the database
    public void SoftDelete(DateTime time)
    {
        IsDeleted = true;
        RemovedAt = time;
    }
}
