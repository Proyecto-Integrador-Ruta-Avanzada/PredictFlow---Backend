using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class TaskDependencyRepository : ITaskDependencyRepository
{
    private readonly PredictFlowDbContext _context;

    public TaskDependencyRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskDependency>> GetByTaskIdAsync(Guid taskId)
    {
        return await _context.TaskDependencies
            .AsNoTracking()
            .Where(d => d.TaskId == taskId)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid taskId, Guid dependsOnTaskId)
    {
        return await _context.TaskDependencies
            .AnyAsync(d => d.TaskId == taskId && d.DependsOnTaskId == dependsOnTaskId);
    }

    public async Task AddAsync(TaskDependency dependency)
    {
        await _context.TaskDependencies.AddAsync(dependency);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskDependency dependency)
    {
        _context.TaskDependencies.Remove(dependency);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByTaskIdAsync(Guid taskId)
    {
        var deps = await _context.TaskDependencies
            .Where(d => d.TaskId == taskId)
            .ToListAsync();

        if (deps.Count == 0) return;

        _context.TaskDependencies.RemoveRange(deps);
        await _context.SaveChangesAsync();
    }
}