using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface ITaskDependencyRepository
{
    Task<IEnumerable<TaskDependency>> GetByTaskIdAsync(Guid taskId);
    Task<bool> ExistsAsync(Guid taskId, Guid dependsOnTaskId);
    Task AddAsync(TaskDependency dependency);
    Task DeleteAsync(TaskDependency dependency);
    Task DeleteByTaskIdAsync(Guid taskId);
}