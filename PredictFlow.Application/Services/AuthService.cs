using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;

    public AuthService(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<(string accessToken, string refreshToken)> GenerateTokens(User user)
    {
        var access = _tokens.GenerateAccessToken(user.Id, user.Email.Value, user.Name);
        var refresh = _tokens.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _users.UpdateAsync(user);

        return (access, refresh);
    }
}