using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<IEnumerable<Team>> GetByUserIdAsync(Guid userId);
    Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId);
    Task AddAsync(Team team);
    Task AddMemberAsync(TeamMember member);
    Task UpdateMemberAsync(TeamMember member);
    Task RemoveMemberAsync(Guid teamId, Guid userId);
}