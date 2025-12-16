namespace PredictFlow.Application.DTOs.Sprints;

public class SprintResponseDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Goal { get; set; } = string.Empty;
}