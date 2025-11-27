using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;


namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class SprintRepository : ISprintRepository
{
    private readonly PredictFlowDbContext _context;

    public SprintRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Sprint?> GetByIdAsync(Guid id)
    {
        return await _context.Sprints
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sprint>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Sprints
            .Where(s => s.ProjectId == projectId)
            .OrderBy(s => s.StartDate)
            .ToListAsync();
    }

    public async Task AddAsync(Sprint sprint)
    {
        await _context.Sprints.AddAsync(sprint);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sprint sprint)
    {
        _context.Sprints.Update(sprint);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Sprint sprint)
    {
        _context.Sprints.Remove(sprint);
        await _context.SaveChangesAsync();
    }
}