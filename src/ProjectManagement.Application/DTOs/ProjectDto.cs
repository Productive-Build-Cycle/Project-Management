using ProjectManagement.Domain.Aggregates.ProjectAggregate;

namespace ProjectManagement.Application.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? TeamId { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime DeadlineTime { get; set; }
}
