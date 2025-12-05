using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;

    public TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
    }

    public async Task<Team> CreateTeamAsync(string name, Guid ownerId)
    {
        var team = new Team(name);
        
        var owner = await _userRepository.GetByIdAsync(ownerId) 
                    ?? throw new Exception("El usuario dueño no existe");

        var ownerMember = new TeamMember(team.Id, ownerId, TeamRole.Lead, "", 100);
        
        await _teamRepository.AddMemberAsync(ownerMember);
        await _teamRepository.AddAsync(team);

        return team;
    }

    public async Task<Team?> GetTeamAsync(Guid teamId)
    {
        return await _teamRepository.GetByIdAsync(teamId);
    }

    public async Task<IEnumerable<Team>> GetTeamsForUserAsync(Guid userId)
    {
        return await _teamRepository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<TeamMember>> GetMembersAsync(Guid teamId)
    {
        var team = await _teamRepository.GetByIdAsync(teamId) 
                   ?? throw new Exception("Team no encontrado");

        return team.Members;
    }

    public async Task AddMemberAsync(Guid teamId, Guid userId, TeamRole role, string skills, int availability)
    {
        var user = await _userRepository.GetByIdAsync(userId)
                   ?? throw new Exception("El usuario no existe");

        // Verificar si ya existe
        var existing = await _teamRepository.GetMemberAsync(teamId, userId);
        if (existing != null)
            throw new Exception("El usuario ya pertenece al equipo");

        var member = new TeamMember(teamId, userId, role, skills, availability);
        await _teamRepository.AddMemberAsync(member);
    }

    public async Task RemoveMemberAsync(Guid teamId, Guid userId)
    {
        await _teamRepository.RemoveMemberAsync(teamId, userId);
    }

    public async Task UpdateMemberRoleAsync(Guid teamId, Guid userId, TeamRole newRole)
    {
        var member = await _teamRepository.GetMemberAsync(teamId, userId) 
                     ?? throw new Exception("Miembro no encontrado");

        member.UpdateRole(newRole);
        await _teamRepository.UpdateMemberAsync(member);
    }

    public async Task UpdateMemberSkillsAsync(Guid teamId, Guid userId, string newSkills)
    {
        var member = await _teamRepository.GetMemberAsync(teamId, userId) 
                     ?? throw new Exception("Miembro no encontrado");

        member.UpdateSkills(newSkills);
        await _teamRepository.UpdateMemberAsync(member);
    }

    public async Task UpdateMemberAvailabilityAsync(Guid teamId, Guid userId, int newAvailability)
    {
        var member = await _teamRepository.GetMemberAsync(teamId, userId) 
                     ?? throw new Exception("Miembro no encontrado");

        member.UpdateAvailability(newAvailability);
        await _teamRepository.UpdateMemberAsync(member);
    }

    public async Task<bool> UserIsTeamMemberAsync(Guid teamId, Guid userId)
    {
        var member = await _teamRepository.GetMemberAsync(teamId, userId);
        return member != null;
    }
}