using FluentAssertions;
using Moq;
using ProjectManagement.Application.Features.Commands.CreateProject;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Interfaces.Persistence;
using ProjectManagement.Domain.Aggregates.ProjectAggregate;
using ProjectManagement.Domain.Exceptions;
using ProjectManagement.Domain.Repositories;
using Xunit;

namespace ProjectManagement.Application.Tests.Commands;

public class CreateProjectHandlerTests
{
    [Fact]
    public async Task CreateProject()
    {
        // Arrange
        var repoMock = new Mock<IProjectRepository>();
        var validationMock = new Mock<IValidationService>();
        var uowMock = new Mock<IUnitOfWork>();
        
        repoMock
            .Setup(r => r.TitleExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        validationMock
            .Setup(v => v.ValidateTeamExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var handler = new CreateProjectHandler(
            repoMock.Object,
            validationMock.Object,
            uowMock.Object
        );

        var command = new CreateProjectCommand(
            Title: "Test Title",
            DeadlineTime: DateTime.Now.AddDays(10),
            Description: "Test Project",
            TeamId: Guid.NewGuid().ToString()
        );

        // Act
        var projectId = await handler.Handle(command, CancellationToken.None);

        // Assert
        projectId.Should().NotBe(Guid.Empty);

        repoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateProjectWithInvalidTeamId()
    {
        // Arrange
        var repoMock = new Mock<IProjectRepository>();
        var validationMock = new Mock<IValidationService>();
        var uowMock = new Mock<IUnitOfWork>();
        var teamId = Guid.NewGuid().ToString();

        validationMock
            .Setup(v => v.ValidateTeamExistsAsync(teamId))
            .ReturnsAsync(false);

        var handler = new CreateProjectHandler(
            repoMock.Object,
            validationMock.Object,
            uowMock.Object
        );

        var command = new CreateProjectCommand(
            Title: "Test Title",
            DeadlineTime: DateTime.Now.AddDays(10),
            Description: "Test Project",
            TeamId: teamId
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<TeamNotFoundException>();

        repoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateProjectWithDuplicateTitle()
    {
        // Arrange
        var repoMock = new Mock<IProjectRepository>();
        var validationMock = new Mock<IValidationService>();
        var uowMock = new Mock<IUnitOfWork>();

        repoMock
            .Setup(r => r.TitleExistsAsync("Duplicate", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        validationMock
            .Setup(v => v.ValidateTeamExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var handler = new CreateProjectHandler(
            repoMock.Object,
            validationMock.Object,
            uowMock.Object
        );

        var command = new CreateProjectCommand(
            Title: "Duplicate",
            DeadlineTime: DateTime.Now.AddDays(10),
            Description: "Test Project",
            TeamId: Guid.NewGuid().ToString()
        );

        // Act
        Func<Task> act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateProjectTitleException>();

        repoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
        uowMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
