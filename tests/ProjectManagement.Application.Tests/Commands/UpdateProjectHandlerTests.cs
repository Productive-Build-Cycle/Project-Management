using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.UpdateProject;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class UpdateProjectHandlerTests
{
    [Fact]
    public async Task UpdateProject()
    {
        // Arrange
        var project = new Project(
            "Old Title",
            DateTime.Now.AddDays(5)
        );

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new UpdateProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new UpdateProjectCommand(
            project.Id,
            "New Title",
            "New Description"
        );

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        project.Title.Should().Be("New Title");
        project.Description.Should().Be("New Description");
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectWithInvalidTitle()
    {
        // Arrange
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );

        var repoMock = new Mock<IProjectRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var handler = new UpdateProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new UpdateProjectCommand(
            project.Id,
            " ",
            "Descriptionnnnnnnnnnn"
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();

        project.Title.Should().Be("Test Title");
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
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

        var handler = new UpdateProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new UpdateProjectCommand(
            Guid.NewGuid(),
            "Title",
            "Description"
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>();

        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
