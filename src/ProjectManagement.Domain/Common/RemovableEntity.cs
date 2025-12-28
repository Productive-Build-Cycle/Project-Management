namespace ProjectManagement.Domain.Common;

public class RemovableEntity<T> : BaseEntity<T>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
