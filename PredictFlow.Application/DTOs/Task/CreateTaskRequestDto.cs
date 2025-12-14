namespace PredictFlow.Application.DTOs.Tasks;

public class CreateTaskRequestDto
{
    public Guid BoardColumnId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CreatedBy { get; set; }
    public Guid AssignedTo { get; set; }

    public int Priority { get; set; }           // mapea a ValueObject Priority
    public int StoryPoints { get; set; }        // mapea a ValueObject StoryPoints
    public int EstimatedHours { get; set; }
}