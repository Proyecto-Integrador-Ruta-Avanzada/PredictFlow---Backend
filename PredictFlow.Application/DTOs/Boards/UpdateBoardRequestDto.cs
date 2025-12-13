namespace PredictFlow.Application.DTOs.Boards;

public class UpdateBoardRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}