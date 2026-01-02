using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.DeleteProject;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class DeleteProjectHandlerTests
{
    [Fact]
    public async Task DeleteProject()
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

        var handler = new DeleteProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new DeleteProjectCommand(project.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.Delete(project), Times.Once);
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

        var handler = new DeleteProjectHandler(
            repoMock.Object,
            uowMock.Object
        );

        var command = new DeleteProjectCommand(Guid.NewGuid());

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>();

        repoMock.Verify(r => r.Delete(It.IsAny<Project>()), Times.Never);
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
