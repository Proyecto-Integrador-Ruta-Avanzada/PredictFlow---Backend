using PredictFlow.Domain.Entities;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.Interfaces;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(string name, Guid userId);
    
    Task<Team?> GetTeamAsync(Guid teamId);
    Task<IEnumerable<Team>> GetTeamsForUserAsync(Guid userId);
    
    Task AddMemberAsync(Guid teamId, Guid userId);
    Task RemoveMemberAsync(Guid teamId, Guid userId);
    
    Task UpdateMemberRoleAsync(Guid teamId, Guid userId, TeamRole newRole);
    Task UpdateMemberSkillsAsync(Guid teamId, Guid userId, string newSkills);
    Task UpdateMemberAvailabilityAsync(Guid teamId, Guid userId, int newAvailability);
    
    Task<IEnumerable<TeamMember>> GetMembersAsync(Guid teamId);
    
    Task<bool> UserIsTeamMemberAsync(Guid userId, Guid teamId);
}