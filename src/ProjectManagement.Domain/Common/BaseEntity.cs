using ProjectManagement.Domain.Common.Events;

namespace ProjectManagement.Domain.Common;

// Base entity class providing common properties and domain event support
public abstract class BaseEntity<T>
{
    // Primary key of the entity
    public T Id { get; protected set; }

    // Timestamps for tracking entity changes
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    // List of domain events raised by this entity
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    // Add a domain event to be dispatched later
    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    // Methods for setting audit timestamps
    public void SetCreatedAt(DateTime time)
        => CreatedAt = time;

    public void SetModifiedAt(DateTime time)
        => ModifiedAt = time;
}