using PredictFlow.Application.DTOs.Sprint;
using PredictFlow.Application.DTOs.SprintTask;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class SprintTaskService : ISprintTaskService
{
    private readonly ISprintTaskRepository _repository;

    public SprintTaskService(ISprintTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task AssignTaskToSprintAsync(Guid sprintId, Guid taskId)
    {
        var sprintTask = new SprintTask(sprintId, taskId);
        await _repository.AddAsync(sprintTask);
    }

    public async Task RemoveTaskFromSprintAsync(Guid sprintId, Guid taskId)
    {
        var sprintTasks = await _repository.GetBySprintIdAsync(sprintId);
        var sprintTask = sprintTasks.FirstOrDefault(st => st.TaskId == taskId);
        if (sprintTask != null)
            await _repository.RemoveAsync(sprintTask);
    }

    public async Task<IEnumerable<SprintTaskResponseDto>> GetTasksBySprintIdAsync(Guid sprintId)
    {
        var tasks = await _repository.GetBySprintIdAsync(sprintId);
        return tasks.Select(t => new SprintTaskResponseDto
        {
            SprintId = t.SprintId,
            TaskId = t.TaskId
        });
    }
}