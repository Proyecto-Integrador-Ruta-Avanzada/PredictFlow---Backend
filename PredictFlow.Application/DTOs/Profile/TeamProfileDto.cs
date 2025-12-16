using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.DTOs.Profile;

public class TeamProfileDto
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public TeamRole Role { get; set; }
    public string Skills { get; set; } = string.Empty;
    public int Availability { get; set; }
    public int Workload { get; set; }
    public List<ProjectProfileDto> Projects { get; set; } = new();
}