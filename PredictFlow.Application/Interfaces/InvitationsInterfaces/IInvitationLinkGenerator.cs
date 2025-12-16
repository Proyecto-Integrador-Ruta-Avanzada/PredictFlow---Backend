namespace PredictFlow.Application.Interfaces.InvitationsInterfaces;

public interface IInvitationLinkGenerator
{
    public string GenerateInvitationLink(string code, string email);
}