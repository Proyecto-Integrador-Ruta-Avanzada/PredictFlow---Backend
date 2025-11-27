using PredictFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace PredictFlow.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}