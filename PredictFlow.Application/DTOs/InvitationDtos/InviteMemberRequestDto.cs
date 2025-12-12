namespace PredictFlow.Application.DTOs.InvitationDtos;

public class InviteMemberRequestDto
{
    public string Email { get; set; } = string.Empty;
    public Guid InvitedToTeamId { get; set; }
    public Guid InvitedToUserId { get; set; }
}