using ProjectManagement.Domain.Common;

namespace ProjectManagement.Domain.Aggregates.ProjectAggregate;

public class Project : RemovableEntity<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string TeamId { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime DeadlineTime { get; set; }
}
