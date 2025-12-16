namespace PredictFlow.Domain.Entities;

public class SprintTask
{
    public Guid SprintId { get; private set; }
    public Guid TaskId { get; private set; }

    private SprintTask() { }

    public SprintTask(Guid sprintId, Guid taskId)
    {
        SprintId = sprintId;
        TaskId = taskId;
    }
}