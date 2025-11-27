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
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
    {
        return await _context.Teams
            .Include(t => t.Members)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Teams
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetMemberAsync(Guid teamId)
    {
        return await _context.TeamMembers
            .Where(m => m.TeamId == teamId)
            .Join(
                _context.Users,
                member => member.UserId,
                user => user.Id,
                (member, user) => user
            )
            .AsNoTracking()
            .ToListAsync();
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
}