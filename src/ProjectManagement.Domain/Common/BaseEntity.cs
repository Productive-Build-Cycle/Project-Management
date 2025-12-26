namespace ProjectManagement.Domain.Common;


public abstract class BaseEntity<T>
{
    public T Id { get; protected set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}