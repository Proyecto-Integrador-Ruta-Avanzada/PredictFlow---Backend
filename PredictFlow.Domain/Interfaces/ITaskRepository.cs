using PredictFlow.Domain.Entities;

namespace PredictFlow.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskEntity>> GetByColumnIdAsync(Guid boardColumnId);
    Task<IEnumerable<TaskEntity>> GetByAssigneeAsync(Guid userId);
    Task AddAsync(TaskEntity task);
    Task UpdateAsync(TaskEntity task);
    Task DeleteAsync(TaskEntity task);
}