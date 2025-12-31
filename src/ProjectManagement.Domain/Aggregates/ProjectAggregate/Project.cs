using ProjectManagement.Domain.Common;

namespace ProjectManagement.Domain.Aggregates.ProjectAggregate;

public class Project : RemovableEntity<Guid>
{
    #region Props

    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? TeamId { get; private set; }
    public ProjectStatus Status { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public DateTime DeadlineTime { get; private set; }

    #endregion

    #region CTors

    private Project()
    {
    }

    public Project(
        string title,
        DateTime deadlineTime,
        string? description = null,
        string? teamId = null)
    {
        Id = Guid.NewGuid();

        SetTitle(title);
        SetDescription(description);
        ChangeDeadlineTime(deadlineTime);
        AssignToTeam(teamId);

        Status = ProjectStatus.Draft;
    }

    #endregion

    #region Setters

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Project title cannot be empty.", nameof(title));

        Title = title.Trim();
    }

    public void SetDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void ChangeStatus(ProjectStatus newStatus)
    {
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Completed project status cannot be changed.");

        if (!IsValidStatusTransition(Status, newStatus))
            throw new InvalidOperationException($"Invalid status transition from {Status} to {newStatus}.");

        Status = newStatus;
    }

    public void Start(DateTime startTime)
    {
        if (Status != ProjectStatus.Draft)
            throw new InvalidOperationException("Only draft projects can be started.");

        if (startTime > DeadlineTime)
            throw new ArgumentException("Start time cannot be after deadline.");

        StartTime = startTime;
        Status = ProjectStatus.InProgress;
    }

    public void Finish(DateTime endTime)
    {
        if (Status != ProjectStatus.InProgress)
            throw new InvalidOperationException("Only in-progress projects can be completed.");

        if (endTime < StartTime)
            throw new ArgumentException("End time cannot be before start time.");

        EndTime = endTime;
        Status = ProjectStatus.Completed;
    }

    public void ChangeDeadlineTime(DateTime deadlineTime)
    {
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Deadline cannot be changed after completion.");

        DeadlineTime = deadlineTime;
    }

    public void AssignToTeam(string? teamId)
    {
        TeamId = teamId;
    }

    #endregion

    #region Rules

    private static bool IsValidStatusTransition(ProjectStatus current, ProjectStatus next)
    {
        return current switch
        {
            ProjectStatus.Draft => next is ProjectStatus.InProgress,
            ProjectStatus.InProgress => next is ProjectStatus.Completed,
            ProjectStatus.Completed => false,
            _ => false
        };
    }

    #endregion
}