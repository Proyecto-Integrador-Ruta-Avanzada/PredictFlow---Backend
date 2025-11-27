using PredictFlow.Domain.Entities;

namespace PredictFlow.Domain.Interfaces;

public interface ISprintRepository
{
    Task<Sprint?> GetByIdAsync(Guid id);
    Task<IEnumerable<Sprint>> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(Sprint sprint);
    Task UpdateAsync(Sprint sprint);
    Task DeleteAsync(Sprint sprint);
}