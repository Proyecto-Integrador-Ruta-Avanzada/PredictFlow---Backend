using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<IEnumerable<Team>> GetAllAsync();
    Task<IEnumerable<Team>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<User>> GetMemberAsync(Guid teamId);
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
}