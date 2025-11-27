namespace PredictFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public Guid TeamId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public List<Board> Boards { get; private set; } = new();
    public List<Sprint> Sprints { get; private set; } = new();

    private Project() { }

    public Project(Guid teamId, string name, string description)
    {
        Id = Guid.NewGuid();
        TeamId = teamId;
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}