using PredictFlow.Domain.Entities;

namespace PredictFlow.Domain.Interfaces;

public interface ISprintTaskRepository
{
    Task AddAsync(SprintTask sprintTask);
    Task RemoveAsync(SprintTask sprintTask);
    Task<IEnumerable<SprintTask>> GetBySprintIdAsync(Guid sprintId);
    Task<IEnumerable<SprintTask>> GetByTaskIdAsync(Guid taskId);
}