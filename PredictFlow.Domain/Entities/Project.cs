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
        if (teamId == Guid.Empty) throw new ArgumentException("TeamId is required.", nameof(teamId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));

        Id = Guid.NewGuid();
        TeamId = teamId;
        Name = name.Trim();
        Description = description.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));

        Name = name.Trim();
        Description = description.Trim();
    }

    public Board AddBoard(string name, int position)
    {
        var board = new Board(Id, name, position);
        Boards.Add(board);
        return board;
    }

    public void RemoveBoard(Guid boardId)
    {
        if (boardId == Guid.Empty) throw new ArgumentException("BoardId is required.", nameof(boardId));

        var board = Boards.FirstOrDefault(b => b.Id == boardId);
        if (board is null) throw new InvalidOperationException("Board not found in this project.");

        Boards.Remove(board);
    }
}