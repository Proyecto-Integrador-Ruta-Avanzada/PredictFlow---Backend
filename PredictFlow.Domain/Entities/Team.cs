namespace PredictFlow.Domain.Entities;

public class Team
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public List<TeamMember> Members { get; private set; } = new();

    private Team() { }

    public Team(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }
}