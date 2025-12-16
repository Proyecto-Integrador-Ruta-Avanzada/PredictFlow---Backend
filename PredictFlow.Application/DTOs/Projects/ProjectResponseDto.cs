using PredictFlow.Application.DTOs.Boards;

namespace PredictFlow.Application.DTOs.Projects;

public class ProjectResponseDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<BoardResponseDto> Boards { get; set; } = new();
}