using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class BoardColumnRepository : IBoardColumnRepository
{
    private readonly PredictFlowDbContext _context;

    public BoardColumnRepository(PredictFlowDbContext context)
    {
        _context = context;
    }

    public async Task<BoardColumn?> GetByIdAsync(Guid id)
    {
        return await _context.BoardColumns
            .Include(c => c.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<BoardColumn>> GetByBoardIdAsync(Guid boardId)
    {
        return await _context.BoardColumns
            .Where(c => c.BoardId == boardId)
            .OrderBy(c => c.Position)
            .ToListAsync();
    }

    public async Task AddAsync(BoardColumn column)
    {
        await _context.BoardColumns.AddAsync(column);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BoardColumn column)
    {
        _context.BoardColumns.Update(column);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BoardColumn column)
    {
        _context.BoardColumns.Remove(column);
        await _context.SaveChangesAsync();
    }
}