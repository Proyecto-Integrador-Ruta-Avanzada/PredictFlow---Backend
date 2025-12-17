using PredictFlow.Domain.Entities;

namespace PredictFlow.Application.Interfaces;

public interface ITokenService
{
    // Método actualizado para recibir la entidad User completa
    string GenerateToken(User user);
    
    // Método para el Refresh Token (Nuevo)
    string GenerateRefreshToken();
    
    // Método para invitaciones (Existente)
    string GenerateInvitationCode(int length = 48);
}