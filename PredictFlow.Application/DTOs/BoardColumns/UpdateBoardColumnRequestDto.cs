namespace PredictFlow.Application.DTOs.BoardColumns;

public class UpdateBoardColumnRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}