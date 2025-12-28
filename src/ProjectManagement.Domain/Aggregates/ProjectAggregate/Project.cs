using ProjectManagement.Domain.Common;

namespace ProjectManagement.Domain.Aggregates.ProjectAggregate;

public class Project : RemovableEntity<Guid>
{
    public string Title { get;private set; }
    public string Description { get;private set; }
    public string TeamId { get;private set; }
    public ProjectStatus Status { get;private set; }
    public DateTime? StartTime { get;private set; }
    public DateTime? EndTime { get;private set; }
    public DateTime DeadlineTime { get;private set; }

    public Project(
    string title,
    string description,
    string teamId,
    DateTime deadlineTime)
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        SetDescription(description);
        TeamId = teamId;
        DeadlineTime = deadlineTime;
        Status = ProjectStatus.Draft;
    }

    protected Project() { }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Project title cannot be empty");

        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description?.Trim();
    }
    public void ChangeStatus(ProjectStatus status)
    {
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Completed project cannot be changed");

        Status = status;
    }
    public void Start(DateTime startTime)
    {
        if (startTime > DeadlineTime)
            throw new ArgumentException("Start time cannot be after deadline");

        StartTime = startTime;
    }

    public void Finish(DateTime endTime)
    {
        if (StartTime == null)
            throw new InvalidOperationException("Project has not started yet");

        EndTime = endTime;
        Status = ProjectStatus.Completed;
    }
}
