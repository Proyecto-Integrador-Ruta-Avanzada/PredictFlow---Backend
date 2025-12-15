using PredictFlow.Application.DTOs;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    // Método de Registro
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // 1. Validar si el usuario ya existe
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new Exception("El correo electrónico ya está registrado.");
        }

        // 2. Hashear la contraseña
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 3. Crear la entidad User (Usando tu constructor existente en User.cs)
        // Nota: Asignamos un rol por defecto "User"
        var newUser = new User(
            dto.Name, 
            new Email(dto.Email), // Usamos el Value Object Email
            passwordHash, 
            "User" 
        );

        // 4. Guardar en base de datos
        await _userRepository.AddAsync(newUser);

        // 5. Generar Token JWT usando tu TokenService existente
        var token = _tokenService.GenerateAccessToken(newUser.Id, newUser.Email.Value, newUser.Name);

        return new AuthResponseDto
        {
            Token = token,
            Email = newUser.Email.Value,
            Name = newUser.Name
        };
    }

    // Método de Login
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        // 1. Buscar usuario
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new Exception("Credenciales inválidas.");
        }

        // 2. Verificar contraseña (Hash vs Texto plano)
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Credenciales inválidas.");
        }

        // 3. Generar Token
        var token = _tokenService.GenerateAccessToken(user.Id, user.Email.Value, user.Name);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email.Value,
            Name = user.Name
        };
    }

    // Método de Generación de Access Token y Refresh Token
    public async Task<(string accessToken, string refreshToken)> GenerateTokens(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email.Value, user.Name);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        // Actualizamos el usuario con el nuevo RefreshToken
        await _userRepository.UpdateAsync(user);

        return (accessToken, refreshToken);
    }
}
