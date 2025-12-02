using PredictFlow.Application.DTOs;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects; // Necesario para el Email VO
using BCrypt.Net;

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
}