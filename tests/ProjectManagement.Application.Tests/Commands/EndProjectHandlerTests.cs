using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.DeleteProject;
using ProjectManagement.Application.Features.Commands.EndProject;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class EndProjectHandlerTests
{
    [Fact]
    public async Task EndProject()
    {
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );

        var startTime = DateTime.Now;
        var endTime = startTime.AddDays(1);

        project.Start(startTime);

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new EndProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new EndProjectCommand(
            project.Id,
            endTime
        );

        await handler.Handle(command, CancellationToken.None);

        project.Status.Should().Be(ProjectStatus.Completed);
        project.EndTime.Should().Be(endTime);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ProjectNotFound()
    {
        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        var handler = new EndProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new EndProjectCommand(
            Guid.NewGuid(),
            DateTime.Now
        );

        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ProjectNotFoundException>();
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task EndProjectWhenNotStarted()
    {
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new EndProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new EndProjectCommand(
            project.Id,
            DateTime.Now
        );

        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
