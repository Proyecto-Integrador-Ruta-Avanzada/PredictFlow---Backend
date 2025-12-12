using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Domain.Entities;

public class TaskEntity
{
    public Guid Id { get; private set; }
    public Guid BoardColumnId { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }

    public Guid CreatedBy { get; private set; }
    public Guid AssignedTo { get; private set; }

    public Priority Priority { get; private set; }
    public StoryPoints StoryPoints { get; private set; }
    public TaskState State { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public int EstimatedHours { get; private set; }
    public int? ActualHours { get; private set; }

    public List<Guid> Dependencies { get; private set; } = new();

    private TaskEntity() { }

    public TaskEntity(
        Guid boardColumnId,
        string title,
        string description,
        Guid createdBy,
        Guid assignedTo,
        Priority priority,
        StoryPoints storyPoints,
        int estimatedHours
    )
    {
        Id = Guid.NewGuid();
        BoardColumnId = boardColumnId;
        Title = title;
        Description = description;
        CreatedBy = createdBy;
        AssignedTo = assignedTo;
        Priority = priority;
        StoryPoints = storyPoints;
        EstimatedHours = estimatedHours;
        State = TaskState.Todo;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MoveTo(Guid newColumnId)
    {
        BoardColumnId = newColumnId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(TaskState status)
    {
        State = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateActualHours(int hours)
    {
        ActualHours = hours;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddDependency(Guid taskId)
    {
        if (!Dependencies.Contains(taskId))
            Dependencies.Add(taskId);
    }

    public void RemoveDependency(Guid taskId)
    {
        if (Dependencies.Contains(taskId))
            Dependencies.Remove(taskId);
    }
}
