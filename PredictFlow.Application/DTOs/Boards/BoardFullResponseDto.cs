using PredictFlow.Application.DTOs.BoardColumns;

namespace PredictFlow.Application.DTOs.Boards;

public class BoardFullResponseDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }

    public List<BoardColumnFullResponseDto> Columns { get; set; } = new();
}