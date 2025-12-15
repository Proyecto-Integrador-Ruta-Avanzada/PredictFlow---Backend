namespace PredictFlow.Application.DTOs.Tasks;

public class TaskResponseDto
{
    public Guid Id { get; set; }
    public Guid BoardColumnId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CreatedBy { get; set; }
    public Guid? AssignedTo { get; set; }

    public int Priority { get; set; }
    public int StoryPoints { get; set; }
    public string State { get; set; } = string.Empty;

    public int EstimatedHours { get; set; }
    public int? ActualHours { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}