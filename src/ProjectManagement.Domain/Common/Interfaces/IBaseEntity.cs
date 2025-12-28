namespace ProjectManagement.Domain.Common.Interfaces;

// Interface defining basic audit properties for entities
public class IBaseEntity
{
    // Timestamp when the entity was created
    DateTime CreatedAt { get; }

    // Timestamp when the entity was last modified (nullable)
    DateTime? ModifiedAt { get; }
}