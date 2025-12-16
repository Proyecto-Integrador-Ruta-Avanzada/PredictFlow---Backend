namespace PredictFlow.Domain.Entities;

public class BoardColumn
{
    public Guid Id { get; private set; }
    public Guid BoardId { get; private set; }
    public string Name { get; private set; }
    public int Position { get; private set; }

    public List<TaskEntity> Tasks { get; private set; } = new();

    private BoardColumn() { }

    public BoardColumn(Guid boardId, string name, int position)
    {
        if (boardId == Guid.Empty) throw new ArgumentException("BoardId is required.", nameof(boardId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Position cannot be negative.");

        Id = Guid.NewGuid();
        BoardId = boardId;
        Name = name.Trim();
        Position = position;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        Name = name.Trim();
    }

    public void Reposition(int position)
    {
        if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Position cannot be negative.");
        Position = position;
    }
}