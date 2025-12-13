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
        if (projectId == Guid.Empty) throw new ArgumentException("ProjectId is required.", nameof(projectId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "Position cannot be negative.");

        Id = Guid.NewGuid();
        ProjectId = projectId;
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

    public BoardColumn AddColumn(string name, int position)
    {
        var column = new BoardColumn(Id, name, position);
        Columns.Add(column);
        return column;
    }

    public void RemoveColumn(Guid columnId)
    {
        if (columnId == Guid.Empty) throw new ArgumentException("ColumnId is required.", nameof(columnId));

        var column = Columns.FirstOrDefault(c => c.Id == columnId);
        if (column is null) throw new InvalidOperationException("Column not found in this board.");

        Columns.Remove(column);
    }
}