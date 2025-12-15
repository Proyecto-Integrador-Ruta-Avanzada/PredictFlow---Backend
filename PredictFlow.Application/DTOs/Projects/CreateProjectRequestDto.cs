namespace PredictFlow.Application.DTOs.Projects;

public class CreateProjectRequestDto
{
    public Guid TeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}