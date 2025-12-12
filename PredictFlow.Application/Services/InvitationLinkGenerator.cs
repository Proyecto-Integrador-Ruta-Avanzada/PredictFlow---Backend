using Microsoft.Extensions.Configuration;
using PredictFlow.Application.Interfaces.InvitationsInterfaces;

namespace PredictFlow.Application.Services;

public class InvitationLinkGenerator : IInvitationLinkGenerator
{
    private readonly string _frontUrl;

    public InvitationLinkGenerator(IConfiguration configuration)
    {
        _frontUrl = configuration["Urls:FrontendUrl"] ?? 
                    Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ??
                    throw new Exception("Frontend url missing");
    }
    public string GenerateInvitationLink(string code, string email)
    {
        return $"{_frontUrl}/invitation?code={code}&email={email}";
    }
}