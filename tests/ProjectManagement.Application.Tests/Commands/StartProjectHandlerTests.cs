using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.StartProject;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class StartProjectHandlerTests
{
    [Fact]
    public async Task StartProject()
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

        var handler = new StartProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var startTime = DateTime.Now;

        var command = new StartProjectCommand(
            project.Id,
            startTime
        );

        await handler.Handle(command, CancellationToken.None);

        project.Status.Should().Be(ProjectStatus.InProgress);
        project.StartTime.Should().Be(startTime);
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

        var handler = new StartProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new StartProjectCommand(
            Guid.NewGuid(),
            DateTime.Now
        );

        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ProjectNotFoundException>();
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task StartProjectWhenItAlreadyStarted()
    {
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );

        project.Start(DateTime.Now);

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new StartProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new StartProjectCommand(
            project.Id,
            DateTime.Now
        );

        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}