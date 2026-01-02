using FluentAssertions;
using Moq;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Features.Queries.GetProjectById;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Exceptions;
using Xunit;

namespace ProjectManagement.Application.Tests.Queries;

public class GetProjectByIdHandlerTests
{
    [Fact]
    public async Task GetById()
    {
        var dto = new ProjectDto
        {
            Id = Guid.NewGuid(),
            Title = "Test Title"
        };

        var queryMock = new Mock<IProjectQuery>();

        queryMock
            .Setup(q => q.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var handler = new GetProjectByIdHandler(queryMock.Object);

        var result = await handler.Handle(
            new GetProjectByIdQuery(dto.Id),
            CancellationToken.None);

        result.Should().Be(dto);
    }

    [Fact]
    public async Task ProjectNotFound()
    {
        var queryMock = new Mock<IProjectQuery>();

        queryMock
            .Setup(q => q.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProjectDto?)null);

        var handler = new GetProjectByIdHandler(queryMock.Object);

        Func<Task> act = async () =>
            await handler.Handle(
                new GetProjectByIdQuery(Guid.NewGuid()),
                CancellationToken.None);

        await act.Should().ThrowAsync<ProjectNotFoundException>();
    }
}
