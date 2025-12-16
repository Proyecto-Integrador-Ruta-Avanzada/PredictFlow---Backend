namespace PredictFlow.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, string email, string fullName);
    string GenerateRefreshToken();
    public string GenerateInvitationCode(int length = 48);
}