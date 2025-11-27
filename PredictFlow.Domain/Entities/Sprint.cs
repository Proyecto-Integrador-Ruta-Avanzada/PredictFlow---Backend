namespace PredictFlow.Domain.Entities;

public class Sprint
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Goal { get; private set; }

    public List<SprintTask> Tasks { get; private set; } = new();

    private Sprint() { }

    public Sprint(Guid projectId, string name, DateTime start, DateTime end, string goal)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Name = name;
        StartDate = start;
        EndDate = end;
        Goal = goal;
    }
}