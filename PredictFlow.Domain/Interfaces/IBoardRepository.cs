using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(Guid id);
    Task<IEnumerable<Board>> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(Board board);
    Task UpdateAsync(Board board);
    Task DeleteAsync(Board board);
}