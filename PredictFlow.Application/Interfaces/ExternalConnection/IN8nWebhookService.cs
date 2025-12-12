namespace PredictFlow.Application.Interfaces.ExternalConnection;

public interface IN8nWebhookService
{
    public Task SendInvitationAsync(string email, string link, string invitedBy, string teamName);
    public Task SendConfirmationAsync(string email, string acceptedBy, string teamName, string ownerName);
}