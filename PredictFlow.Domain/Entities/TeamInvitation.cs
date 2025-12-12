using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Domain.Entities;

public class TeamInvitation
{
    public Guid Id { get; private set; }
    public Guid TeamId { get; private set; }
    public Guid InvitedByUserId { get; private set; }
    
    public string Code { get; private set; }
    public Email InvitedUserEmail { get; private set; }
    public InvitationStatus Status { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    
    private TeamInvitation(){} //Constructor para EF

    public TeamInvitation(Guid teamId, Guid invitedByUserId, string code, Email invitedUserEmail)
    {
        TeamId = teamId;
        InvitedByUserId = invitedByUserId;
        Code = code;
        InvitedUserEmail = invitedUserEmail;
        Status = InvitationStatus.Pending;
        ExpiresAtUtc = DateTime.UtcNow.AddDays(7);
    }

    public void Accept()
    {
        if (Status != InvitationStatus.Pending)
            throw new Exception("Invitation is not pending");
        Status = InvitationStatus.Accepted;
    }
    public void Expire()
    {
        if (Status != InvitationStatus.Pending)
            throw new Exception("Invitation is not pending");
        Status = InvitationStatus.Expired;
    }
}

public enum InvitationStatus
{
    Pending,
    Accepted,
    Expired
}