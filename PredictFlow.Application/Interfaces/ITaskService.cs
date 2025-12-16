using PredictFlow.Application.DTOs.Tasks;

namespace PredictFlow.Application.Interfaces;

public interface ITaskService
{
    Task MoveAsync(Guid taskId, MoveTaskRequestDto dto);
    Task<TaskResponseDto> CreateAsync(Guid currentUserId, CreateTaskRequestDto dto);
    Task UpdateStatusAsync(Guid taskId, UpdateTaskStatusRequestDto dto);
}