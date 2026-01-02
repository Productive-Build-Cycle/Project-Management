using FluentAssertions;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using Xunit;

namespace ProjectManagement.Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void CreateProject()
    {
        var deadline = DateTime.Now.AddDays(10);

        var project = new Project("Test Title", deadline);

        project.Status.Should().Be(ProjectStatus.Draft);
        project.Title.Should().Be("Test Title");
        project.DeadlineTime.Should().Be(deadline);
    }

    [Fact]
    public void CreateProjectWithEmptyTitle()
    {
        Action act = () => new Project(" ", DateTime.Now.AddDays(5));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void StartProject()
    {
        var project = new Project("Test Title", DateTime.Now.AddDays(5));

        project.Start(DateTime.Now);

        project.Status.Should().Be(ProjectStatus.InProgress);
        project.StartTime.Should().NotBeNull();
    }

    [Fact]
    public void StartProjectWhenAlreadyStarted()
    {
        var project = new Project("Title", DateTime.Now.AddDays(5));
        project.Start(DateTime.Now);

        Action act = () => project.Start(DateTime.Now);

        act.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void FinishProject()
    {
        var project = new Project("Title", DateTime.Now.AddDays(5));
        project.Start(DateTime.Now);

        project.Finish(DateTime.Now.AddDays(1));

        project.Status.Should().Be(ProjectStatus.Completed);
        project.EndTime.Should().NotBeNull();
    }

    [Fact]
    public void InvalidChangeStatus()
    {
        var project = new Project("Test", DateTime.UtcNow.AddDays(5));

        Action act = () => project.ChangeStatus(ProjectStatus.Completed);

        act.Should().Throw<InvalidOperationException>();
    }
}
