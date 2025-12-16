using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly PredictFlowDbContext _context;

    public TaskRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<TaskEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskEntity>> GetByColumnIdAsync(Guid boardColumnId)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.BoardColumnId == boardColumnId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskEntity>> GetByAssigneeAsync(Guid userId)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.AssignedTo == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(TaskEntity task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskEntity task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskEntity task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}