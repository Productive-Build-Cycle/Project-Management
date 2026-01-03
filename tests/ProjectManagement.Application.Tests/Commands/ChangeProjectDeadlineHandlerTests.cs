using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.ChangeProjectDeadline;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class ChangeProjectDeadlineHandlerTests
{
    [Fact]
    public async Task ChangeDeadline()
    {
        // Arrange
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(10)
        );

        var newDeadline = DateTime.UtcNow.AddDays(20);

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new ChangeProjectDeadlineHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new ChangeProjectDeadlineCommand(
            project.Id,
            newDeadline
        );

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        project.DeadlineTime.Should().Be(newDeadline);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ProjectNotFound()
    {
        // Arrange
        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        var handler = new ChangeProjectDeadlineHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new ChangeProjectDeadlineCommand(
            Guid.NewGuid(),
            DateTime.Now.AddDays(5)
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>();

        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task ChangeDeadlineAfterFinish()
    {
        // Arrange
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );

        project.Start(DateTime.Now);
        project.Finish(DateTime.Now.AddDays(1));

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new ChangeProjectDeadlineHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new ChangeProjectDeadlineCommand(
            project.Id,
            DateTime.Now.AddDays(10)
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
