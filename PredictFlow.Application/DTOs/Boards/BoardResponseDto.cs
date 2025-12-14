using PredictFlow.Application.DTOs.BoardColumns;

namespace PredictFlow.Application.DTOs.Boards;

public class BoardResponseDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }

    public List<BoardColumnResponseDto> Columns { get; set; } = new();
}