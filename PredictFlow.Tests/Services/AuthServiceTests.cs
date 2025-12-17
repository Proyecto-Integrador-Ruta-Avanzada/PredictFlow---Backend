using FluentAssertions;
using Moq;
using PredictFlow.Application.DTOs;
using PredictFlow.Application.Interfaces;
using PredictFlow.Application.Services;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();

        _service = new AuthService(
            _userRepositoryMock.Object,
            _tokenServiceMock.Object
        );
    }

    private static User CreateUser(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        return new User(
            "Test User",
            new Email("test@test.com"),
            hash,
            "User"
        );
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldThrow()
    {
        var dto = new RegisterDto
        {
            Name = "Test",
            Email = "test@test.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(CreateUser("123456"));

        Func<Task> act = () => _service.RegisterAsync(dto);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("El correo electrónico ya está registrado.");
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ShouldThrow()
    {
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "wrong-password"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(CreateUser("correct-password"));

        Func<Task> act = () => _service.LoginAsync(dto);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Credenciales inválidas.");
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnToken()
    {
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "123456"
        };

        var user = CreateUser("123456");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(user);

        _tokenServiceMock
            .Setup(x => x.GenerateAccessToken(user.Id, user.Email.Value, user.Name))
            .Returns("fake-jwt-token");

        var result = await _service.LoginAsync(dto);

        result.Should().NotBeNull();
        result.Token.Should().Be("fake-jwt-token");
        result.Email.Should().Be(user.Email.Value);
    }
}
