using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly PredictFlowDbContext _context;

    public ProjectRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Boards)
            .ThenInclude(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .Include(p => p.Sprints)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }


    public async Task<IEnumerable<Project>> GetByTeamIdAsync(Guid teamId)
    {
        return await _context.Projects
            .Where(p => p.TeamId == teamId)
            .Include(p => p.Boards)
            .ThenInclude(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .Include(p => p.Sprints)
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

}