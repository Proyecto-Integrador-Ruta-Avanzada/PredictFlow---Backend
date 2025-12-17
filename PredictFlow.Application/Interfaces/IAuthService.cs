using PredictFlow.Application.DTOs;

namespace PredictFlow.Application.Interfaces;

public interface IAuthService
{
    // Corrección: Usamos RegisterDto (con 'o' minúscula)
    Task<string> RegisterAsync(RegisterDto registerDto);
    
    // Corrección: Usamos LoginDto (con 'o' minúscula)
    // Nota: AuthResponseDTO sí suele ir en mayúsculas si así lo definiste en el archivo AuthResponseDTO.cs
    Task<AuthResponseDTO> LoginAsync(LoginDto loginDto);
}