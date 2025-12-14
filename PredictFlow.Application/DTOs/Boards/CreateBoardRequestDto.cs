namespace PredictFlow.Application.DTOs.Boards;

public class CreateBoardRequestDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}