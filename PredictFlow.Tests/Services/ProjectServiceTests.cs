using FluentAssertions;
using Moq;
using PredictFlow.Application.DTOs.Projects;
using PredictFlow.Application.Services;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _repoMock;
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _repoMock = new Mock<IProjectRepository>();
        _service = new ProjectService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProject()
    {
        // Arrange
        var dto = new CreateProjectRequestDto
        {
            TeamId = Guid.NewGuid(),
            Name = "Project A",
            Description = "Test project"
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
        result.Description.Should().Be(dto.Description);
        result.TeamId.Should().Be(dto.TeamId);

        _repoMock.Verify(x => x.AddAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenProjectDoesNotExist_ShouldThrow()
    {
        // Arrange
        _repoMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Project?)null);

        var dto = new UpdateProjectRequestDto
        {
            Name = "Updated",
            Description = "Updated desc"
        };

        // Act
        Func<Task> act = async () =>
            await _service.UpdateAsync(Guid.NewGuid(), dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Project not found.");
    }
}