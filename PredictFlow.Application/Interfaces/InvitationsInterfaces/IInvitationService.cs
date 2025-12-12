using PredictFlow.Application.DTOs.InvitationDtos;

namespace PredictFlow.Application.Interfaces.InvitationsInterfaces;

public interface IInvitationService
{
    public Task InviteMember(InviteMemberRequestDto inviteMemberRequestDto);
    public Task<InvitationValidateResultDto> ValidateInvitation(string code, string email);
    public Task<AcceptInvitationResultDto>  AcceptInvitation(string code, string email);
}