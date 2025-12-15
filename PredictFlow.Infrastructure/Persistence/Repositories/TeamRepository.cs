using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly PredictFlowDbContext _context;

    public TeamRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Team?> GetByIdAsync(Guid id)
    {
        return await _context.Teams
            .Include(t => t.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
    {
        return await _context.Teams
            .Include(t => t.Members)
            .ThenInclude(m => m.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Teams
            .Include(t => t.Members)
            .ThenInclude(m => m.User)
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .ToListAsync();
    }
    
    public async Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId)
    {
        return await _context.TeamMembers
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
    }

    public async Task AddAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task AddMemberAsync(TeamMember member)
    {
        await _context.TeamMembers.AddAsync(member);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMemberAsync(TeamMember member)
    {
        _context.TeamMembers.Update(member);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(Guid teamId, Guid userId)
    {
        var entity = await _context.TeamMembers
            .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);

        if (entity != null)
        {
            _context.TeamMembers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
