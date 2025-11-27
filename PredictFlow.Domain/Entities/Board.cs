namespace PredictFlow.Domain.Entities;

public class Board
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; }
    public int Position { get; private set; }

    public List<BoardColumn> Columns { get; private set; } = new();

    private Board() { }

    public Board(Guid projectId, string name, int position)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Name = name;
        Position = position;
    }
}