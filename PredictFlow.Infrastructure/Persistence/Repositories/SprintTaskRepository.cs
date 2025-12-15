using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class SprintTaskRepository : ISprintTaskRepository
{
    private readonly PredictFlowDbContext _context;

    public SprintTaskRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SprintTask sprintTask)
    {
        await _context.SprintTasks.AddAsync(sprintTask);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(SprintTask sprintTask)
    {
        _context.SprintTasks.Remove(sprintTask);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SprintTask>> GetBySprintIdAsync(Guid sprintId)
    {
        return await _context.SprintTasks
            .Where(st => st.SprintId == sprintId)
            .ToListAsync();
    }

    public async Task<IEnumerable<SprintTask>> GetByTaskIdAsync(Guid taskId)
    {
        return await _context.SprintTasks
            .Where(st => st.TaskId == taskId)
            .ToListAsync();
    }
}