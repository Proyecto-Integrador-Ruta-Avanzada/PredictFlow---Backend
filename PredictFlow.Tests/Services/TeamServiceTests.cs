using FluentAssertions;
using Moq;
using PredictFlow.Application.Services;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Tests.Services;

public class TeamServiceTests
{
    private readonly Mock<ITeamRepository> _teamRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly TeamService _service;

    public TeamServiceTests()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _service = new TeamService(
            _teamRepositoryMock.Object,
            _userRepositoryMock.Object
        );
    }
    
    private static User CreateUser()
    {
        return new User(
            "Test User",
            new Email("test@test.com"),
            "hashed-password",
            "Admin"
        );
    }

    [Fact]
    public async Task CreateTeamAsync_WhenOwnerExists_ShouldCreateTeam()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(ownerId))
            .ReturnsAsync(CreateUser());

        // Act
        var team = await _service.CreateTeamAsync("Team Alpha", ownerId);

        // Assert
        team.Should().NotBeNull();
        team.Name.Should().Be("Team Alpha");

        _teamRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Team>()),
            Times.Once
        );

        _teamRepositoryMock.Verify(
            x => x.AddMemberAsync(It.IsAny<TeamMember>()),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateTeamAsync_WhenOwnerDoesNotExist_ShouldThrow()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(ownerId))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> act = () =>
            _service.CreateTeamAsync("Team Beta", ownerId);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("El usuario dueño no existe");
    }

    [Fact]
    public async Task AddMemberAsync_WhenUserAlreadyInTeam_ShouldThrow()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(CreateUser());

        _teamRepositoryMock
            .Setup(x => x.GetMemberAsync(teamId, userId))
            .ReturnsAsync(new TeamMember(
                teamId,
                userId,
                TeamRole.Lead,
                true,
                "",
                80
            ));

        // Act
        Func<Task> act = () =>
            _service.AddMemberAsync(
                teamId,
                userId,
                TeamRole.Lead,
                "C#",
                80
            );

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("El usuario ya pertenece al equipo");
    }

    [Fact]
    public async Task UpdateMemberRoleAsync_ShouldCallRepositoryUpdate()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var member = new TeamMember(
            teamId,
            userId,
            TeamRole.Lead,
            true,
            "",
            100
        );

        _teamRepositoryMock
            .Setup(x => x.GetMemberAsync(teamId, userId))
            .ReturnsAsync(member);

        // Act
        await _service.UpdateMemberRoleAsync(
            teamId,
            userId,
            TeamRole.Lead
        );

        // Assert
        _teamRepositoryMock.Verify(
            x => x.UpdateMemberAsync(member),
            Times.Once
        );
    }
}
