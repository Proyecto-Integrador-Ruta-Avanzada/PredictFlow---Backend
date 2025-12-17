using PredictFlow.Application.DTOs;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects; // <-- NECESARIO para usar 'Email'

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

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        // CORRECCIÓN 1: Convertimos el string a ValueObject Email
        var emailVo = new Email(registerDto.Email);

        // Ahora pasamos el objeto emailVo, no el string
        var existingUser = await _userRepository.GetByEmailAsync(emailVo);
        
        if (existingUser != null)
        {
            throw new Exception("El correo ya está registrado");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        // Reutilizamos emailVo que ya creamos arriba
        var newUser = new User(registerDto.Name, emailVo, passwordHash, "User");

        await _userRepository.AddAsync(newUser);

        return "Usuario registrado exitosamente. Por favor inicia sesión.";
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDto loginDto)
    {
        // CORRECCIÓN 2: Convertimos el string a ValueObject Email aquí también
        var emailVo = new Email(loginDto.Email);

        var user = await _userRepository.GetByEmailAsync(emailVo);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new Exception("Credenciales inválidas");
        }

        // 1. Generar Access Token
        var accessToken = _tokenService.GenerateToken(user);
        
        // 2. Generar Refresh Token
        var refreshToken = _tokenService.GenerateRefreshToken();

        // 3. Guardar en Base de Datos
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
        
        await _userRepository.UpdateAsync(user);

        // 4. Devolver respuesta
        return new AuthResponseDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            UserName = user.Name,
            Email = user.Email.Value, // Aquí extraemos el string del ValueObject
            Id = user.Id
        };
    }
}