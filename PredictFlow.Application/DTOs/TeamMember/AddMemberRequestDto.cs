using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.DTOs.TeamMember;

public class AddMemberRequestDto
{
    public Guid UserId { get; set; }
    public TeamRole Role { get; set; }
    public string Skills { get; set; }
    public int Availability { get; set; }
}