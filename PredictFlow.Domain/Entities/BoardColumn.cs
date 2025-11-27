using Task = PredictFlow.Domain.Entities.Task;

namespace PredictFlow.Domain.Entities;

public class BoardColumn
{
    public Guid Id { get; private set; }
    public Guid BoardId { get; private set; }
    public string Name { get; private set; }
    public int Position { get; private set; }

    public List<Task> Tasks { get; private set; } = new();

    private BoardColumn() { }

    public BoardColumn(Guid boardId, string name, int position)
    {
        Id = Guid.NewGuid();
        BoardId = boardId;
        Name = name;
        Position = position;
    }
}