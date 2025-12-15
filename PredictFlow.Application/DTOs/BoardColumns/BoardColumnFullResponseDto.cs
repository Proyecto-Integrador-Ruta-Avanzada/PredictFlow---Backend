using PredictFlow.Application.DTOs.Tasks;

namespace PredictFlow.Application.DTOs.BoardColumns;

public class BoardColumnFullResponseDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }

    public List<TaskResponseDto> Tasks { get; set; } = new();
}