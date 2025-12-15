namespace PredictFlow.Application.DTOs.Profile;

public class ProfileResponseDto
{
    public UserProfileDto User { get; set; } = null!;
    public List<TeamProfileDto> Teams { get; set; } = new();
}