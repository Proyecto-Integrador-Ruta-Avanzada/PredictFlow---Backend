using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface IBoardColumnRepository
{
    Task<BoardColumn?> GetByIdAsync(Guid id);
    Task<IEnumerable<BoardColumn>> GetByBoardIdAsync(Guid boardId);
    Task AddAsync(BoardColumn column);
    Task UpdateAsync(BoardColumn column);
    Task DeleteAsync(BoardColumn column);
}