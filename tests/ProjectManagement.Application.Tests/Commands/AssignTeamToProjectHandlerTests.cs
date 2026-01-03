using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.AssignTeamProject;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class AssignTeamToProjectHandlerTests
{
    [Fact]
    public async Task AssignTeam()
    {
        // Arrange
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );
        var teamId = Guid.NewGuid().ToString();
        var repoMock = new Mock<IProjectRepository>();
        var validationMock = new Mock<IValidationService>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        validationMock
            .Setup(v => v.ValidateTeamExistsAsync(teamId))
            .ReturnsAsync(true);

        var handler = new AssignTeamToProjectHandler(
            repoMock.Object,
            validationMock.Object,
            uowMock.Object
        );

        var command = new AssignTeamToProjectCommand(
            project.Id,
            teamId
        );

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        project.TeamId.Should().Be(teamId);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ProjectNotFound()
    {
        // Arrange
        var repoMock = new Mock<IProjectRepository>();
        var validationMock = new Mock<IValidationService>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        var handler = new AssignTeamToProjectHandler(
            repoMock.Object,
            validationMock.Object,
            uowMock.Object
        );

        var command = new AssignTeamToProjectCommand(
            Guid.NewGuid(),
            Guid.NewGuid().ToString()
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>();

        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task TeamNotFound()
    {
        // Arrange
        var project = new Project(
            "Test Title",
            DateTime.Now.AddDays(5)
        );
        var teamId = Guid.NewGuid().ToString();
        var repoMock = new Mock<IProjectRepository>();
        var validationMock = new Mock<IValidationService>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        validationMock
            .Setup(v => v.ValidateTeamExistsAsync(teamId))
            .ReturnsAsync(false);

        var handler = new AssignTeamToProjectHandler(
            repoMock.Object,
            validationMock.Object,
            uowMock.Object
        );

        var command = new AssignTeamToProjectCommand(
            project.Id,
            teamId
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<TeamNotFoundException>();

        project.TeamId.Should().BeNull();
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
