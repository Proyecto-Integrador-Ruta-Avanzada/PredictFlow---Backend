using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PredictFlow.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using PredictFlow.Application.Settings;
using PredictFlow.Domain.Entities; // Necesario para 'User'

namespace PredictFlow.Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwt;

    public TokenService(IOptions<JwtSettings> jwt)
    {
        _jwt = jwt.Value;
    }

    // CORRECCIÓN 1: El método se llama GenerateToken y recibe (User user)
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Para máxima compatibilidad
            
            // CORRECCIÓN 2: Usamos .Value porque Email es un ValueObject
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value), 
            
            new Claim("fullName", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public string GenerateInvitationCode(int length = 48)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(bytes)
            .Replace("/", "").Replace("+", "").Replace("=", "");
    }
}