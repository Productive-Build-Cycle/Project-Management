using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Common;

// Project aggregate root
public class Project : RemovableEntity<Guid>
{
    // Basic project information
    public string Title { get; set; }
    public string Description { get; set; }

    // Reference to the owning team (external service)
    public string TeamId { get; set; }

    // Current project status
    public ProjectStatus Status { get; set; }

    // Project timeline
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime DeadlineTime { get; set; }

    // Domain behavior for removing a project
    public void Remove()
    {
        // Completed projects should not be removed
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Completed project cannot be removed.");

        // Perform soft delete through base entity
        SoftDelete(DateTime.UtcNow);
    }
}