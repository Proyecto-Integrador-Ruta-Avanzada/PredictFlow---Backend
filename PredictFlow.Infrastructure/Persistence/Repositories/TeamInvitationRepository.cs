using Microsoft.EntityFrameworkCore;
using PredictFlow.Application.Interfaces.InvitationsInterfaces;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Infrastructure.Persistence.Repositories;

public class TeamInvitationRepository : IInvitationsRepository
{
    private readonly PredictFlowDbContext _dbContext;
    public TeamInvitationRepository(PredictFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TeamInvitation teamInvitation)
    {
        _dbContext.Invitations.Add(teamInvitation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TeamInvitation?> GetByCodeAsync(string code)
    {
       return await _dbContext.Invitations.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}