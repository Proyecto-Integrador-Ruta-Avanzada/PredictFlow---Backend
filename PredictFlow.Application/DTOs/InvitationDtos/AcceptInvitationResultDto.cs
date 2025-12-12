namespace PredictFlow.Application.DTOs.InvitationDtos;

public class AcceptInvitationResultDto
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
}