namespace ProjectManagement.Domain.Common;

// Base entity class providing common properties and domain event support
public abstract class BaseEntity<T>
{
    // Primary key of the entity
    public T Id { get; protected set; }

    // Timestamps for tracking entity changes
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    // Methods for setting audit timestamps
    public void SetCreatedAt(DateTime time)
        => CreatedAt = time;

    public void SetModifiedAt(DateTime time)
        => ModifiedAt = time;
}