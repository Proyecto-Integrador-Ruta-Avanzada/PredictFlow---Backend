using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly PredictFlowDbContext _context;

    public BoardRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Board?> GetByIdAsync(Guid id)
    {
        return await _context.Boards
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Board>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Boards
            .Where(b => b.ProjectId == projectId)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Board board)
    {
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Board board)
    {
        _context.Boards.Update(board);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Board board)
    {
        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();
    }
}