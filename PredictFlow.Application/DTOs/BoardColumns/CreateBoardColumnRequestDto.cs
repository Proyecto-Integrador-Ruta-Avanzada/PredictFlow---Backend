namespace PredictFlow.Application.DTOs.BoardColumns;

public class CreateBoardColumnRequestDto
{
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}