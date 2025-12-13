using PredictFlow.Application.DTOs.Tasks;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IBoardColumnRepository _boardColumnRepository;

    public TaskService(
        ITaskRepository taskRepository,
        IBoardColumnRepository boardColumnRepository)
    {
        _taskRepository = taskRepository;
        _boardColumnRepository = boardColumnRepository;
    }

  
    // CREATE TASK

    public async Task<TaskResponseDto> CreateAsync(CreateTaskRequestDto dto)
    {
        if (dto.BoardColumnId == Guid.Empty)
            throw new ArgumentException("BoardColumnId is required.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException("Description is required.");

        if (dto.CreatedBy == Guid.Empty)
            throw new ArgumentException("CreatedBy is required.");

        if (dto.AssignedTo == Guid.Empty)
            throw new ArgumentException("AssignedTo is required.");

        if (dto.EstimatedHours <= 0)
            throw new ArgumentOutOfRangeException(nameof(dto.EstimatedHours));

        var column = await _boardColumnRepository.GetByIdAsync(dto.BoardColumnId);
        if (column is null)
            throw new InvalidOperationException("Board column not found.");

        if (!Enum.IsDefined(typeof(Priority), dto.Priority))
            throw new ArgumentException("Invalid priority value.");

        var priority = (Priority)dto.Priority;
        var storyPoints = new StoryPoints(dto.StoryPoints);

        var task = new TaskEntity(
            dto.BoardColumnId,
            dto.Title.Trim(),
            dto.Description.Trim(),
            dto.CreatedBy,
            dto.AssignedTo,
            priority,
            storyPoints,
            dto.EstimatedHours
        );

        await _taskRepository.AddAsync(task);
        return Map(task);
    }

   
    // MOVE TASK 
    
    public async Task MoveAsync(Guid taskId, MoveTaskRequestDto dto)
    {
        if (taskId == Guid.Empty)
            throw new ArgumentException("TaskId is required.");

        if (dto.NewColumnId == Guid.Empty)
            throw new ArgumentException("NewColumnId is required.");

        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task is null)
            throw new InvalidOperationException("Task not found.");

        var destinationColumn = await _boardColumnRepository.GetByIdAsync(dto.NewColumnId);
        if (destinationColumn is null)
            throw new InvalidOperationException("Destination column not found.");

        var currentColumn = await _boardColumnRepository.GetByIdAsync(task.BoardColumnId);
        if (currentColumn is null)
            throw new InvalidOperationException("Current column not found.");

        if (currentColumn.BoardId != destinationColumn.BoardId)
            throw new InvalidOperationException("Cannot move task to a column in a different board.");

        task.MoveTo(dto.NewColumnId);
        await _taskRepository.UpdateAsync(task);
    }

    
    // UPDATE TASK STATUS
    
    public async Task UpdateStatusAsync(Guid taskId, UpdateTaskStatusRequestDto dto)
    {
        if (taskId == Guid.Empty)
            throw new ArgumentException("TaskId is required.");

        if (string.IsNullOrWhiteSpace(dto.Status))
            throw new ArgumentException("Status is required.");

        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task is null)
            throw new InvalidOperationException("Task not found.");

        if (!Enum.TryParse<TaskState>(dto.Status, true, out var newStatus))
            throw new ArgumentException("Invalid task status.");

        
        task.UpdateStatus(newStatus);
        await _taskRepository.UpdateAsync(task);
    }

    private static TaskResponseDto Map(TaskEntity task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            BoardColumnId = task.BoardColumnId,
            Title = task.Title,
            Description = task.Description,
            CreatedBy = task.CreatedBy,
            AssignedTo = task.AssignedTo,
            Priority = (int)task.Priority,
            StoryPoints = task.StoryPoints.Value,
            State = task.State.ToString(),
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
