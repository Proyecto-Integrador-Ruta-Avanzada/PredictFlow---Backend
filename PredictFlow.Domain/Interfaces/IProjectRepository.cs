using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<IEnumerable<Project>> GetByTeamIdAsync(Guid teamId);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(Project project);
}