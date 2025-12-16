using FluentAssertions;
using Moq;
using PredictFlow.Application.DTOs.Tasks;
using PredictFlow.Application.Services;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IBoardColumnRepository> _columnRepositoryMock;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _columnRepositoryMock = new Mock<IBoardColumnRepository>();

        _service = new TaskService(
            _taskRepositoryMock.Object,
            _columnRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_WhenBoardColumnDoesNotExist_ShouldThrow()
    {
        var dto = new CreateTaskRequestDto
        {
            BoardColumnId = Guid.NewGuid(),
            Title = "Task",
            Description = "Description",
            AssignedTo = Guid.NewGuid(),
            Priority = 1,
            StoryPoints = 3,
            EstimatedHours = 5
        };

        _columnRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.BoardColumnId))
            .ReturnsAsync((BoardColumn?)null);

        Func<Task> act = () => _service.CreateAsync(Guid.NewGuid(), dto);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Board column not found.");
    }

    [Fact]
    public async Task MoveAsync_WhenMovingToDifferentBoard_ShouldThrow()
    {
        var taskId = Guid.NewGuid();
        var columnA = new BoardColumn(Guid.NewGuid(), "ToDo", 1);
        var columnB = new BoardColumn(Guid.NewGuid(), "Done", 1);

        var task = new TaskEntity(
            columnA.Id,
            "Task",
            "Desc",
            Guid.NewGuid(),
            Guid.NewGuid(),
            Priority.Medium,
            new StoryPoints(3),
            5
        );

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        _columnRepositoryMock
            .Setup(x => x.GetByIdAsync(columnA.Id))
            .ReturnsAsync(columnA);

        _columnRepositoryMock
            .Setup(x => x.GetByIdAsync(columnB.Id))
            .ReturnsAsync(columnB);

        Func<Task> act = () =>
            _service.MoveAsync(taskId, new MoveTaskRequestDto
            {
                NewColumnId = columnB.Id
            });

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot move task to a column in a different board.");
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenStatusIsInvalid_ShouldThrow()
    {
        var taskId = Guid.NewGuid();

        var task = new TaskEntity(
            Guid.NewGuid(),           // BoardColumnId
            "Test Task",
            "Description",
            Guid.NewGuid(),           // CreatedBy
            Guid.NewGuid(),           // AssignedTo
            Priority.Medium,
            new StoryPoints(3),
            5
        );

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        Func<Task> act = () =>
            _service.UpdateStatusAsync(taskId, new UpdateTaskStatusRequestDto
            {
                Status = "INVALID_STATUS"
            });

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Invalid task status.");
    }
}
