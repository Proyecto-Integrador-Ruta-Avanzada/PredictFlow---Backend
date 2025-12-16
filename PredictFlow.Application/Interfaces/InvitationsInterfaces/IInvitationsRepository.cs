using PredictFlow.Domain.Entities;

namespace PredictFlow.Application.Interfaces.InvitationsInterfaces;

public interface IInvitationsRepository
{
    public Task SaveChangesAsync();
    public Task<TeamInvitation?> GetByCodeAsync(string code);
    public Task AddAsync(TeamInvitation teamInvitation);
}