using PredictFlow.Application.DTOs.Sprint;
using PredictFlow.Application.DTOs.SprintTask;

namespace PredictFlow.Application.Interfaces;

public interface ISprintTaskService
{
    Task AssignTaskToSprintAsync(Guid sprintId, Guid taskId);
    Task RemoveTaskFromSprintAsync(Guid sprintId, Guid taskId);
    Task<IEnumerable<SprintTaskResponseDto>> GetTasksBySprintIdAsync(Guid sprintId);
}