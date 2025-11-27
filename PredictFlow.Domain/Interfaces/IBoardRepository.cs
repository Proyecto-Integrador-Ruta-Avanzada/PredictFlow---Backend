namespace PredictFlow.Domain.Repositories;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(Guid id);
    Task<IEnumerable<Board>> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(Board board);
    Task UpdateAsync(Board board);
}