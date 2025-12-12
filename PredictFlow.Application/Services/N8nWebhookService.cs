using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using PredictFlow.Application.Interfaces.ExternalConnection;

namespace PredictFlow.Application.Services;

public class N8nWebhookService :IN8nWebhookService
{
    private readonly HttpClient _client;
    private readonly string _webhookInvitationUrl;
    private readonly string _webhookAcceptedUrl;

    public N8nWebhookService(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _webhookInvitationUrl = configuration["Urls:WebhookInvitationUrl"] ??
                      Environment.GetEnvironmentVariable("WEBHOOK_INVITED_URL") ??
                      throw new Exception("Webhook url missing");
        _webhookAcceptedUrl = configuration["Urls:WebhookAcceptedUrl"] ??
                              Environment.GetEnvironmentVariable("WEBHOOK_ACCEPTED_URL") ??
                              throw new Exception("Webhook url is missing");
    }
    public async Task SendInvitationAsync(string email, string link, string invitedBy, string teamName)
    {
        var payload = new
        {
            Email = email,
            Name = invitedBy,
            TeamName = teamName,
            InviteUrl = link
        };
        await _client.PostAsJsonAsync(_webhookInvitationUrl, payload);
    }

    public async Task SendConfirmationAsync(string email, string acceptedBy, string teamName, string ownerName)
    {
        var payload = new
        {
            Email = email,
            InvitedUserName = acceptedBy,
            TeamName = teamName,
            OwnerName = ownerName
        };
        await _client.PostAsJsonAsync(_webhookAcceptedUrl, payload);
    }
}