using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Common;

public class Project : RemovableEntity<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string TeamId { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime DeadlineTime { get; set; }

    public void Remove()
    {
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Completed project cannot be removed.");

        SoftDelete(DateTime.UtcNow);
    }
}