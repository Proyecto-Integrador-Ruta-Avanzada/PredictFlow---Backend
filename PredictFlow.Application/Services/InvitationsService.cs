using PredictFlow.Application.DTOs.InvitationDtos;
using PredictFlow.Application.Interfaces;
using PredictFlow.Application.Interfaces.ExternalConnection;
using PredictFlow.Application.Interfaces.InvitationsInterfaces;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Application.Services;

public class InvitationsService : IInvitationService
{
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly ITokenService _tokenService;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IN8nWebhookService _webhookService;
    private readonly IInvitationLinkGenerator _invitationLinkGenerator;

    public InvitationsService(IInvitationsRepository invitationsRepository, 
        ITokenService tokenService,
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        IN8nWebhookService webhookService,
        IInvitationLinkGenerator invitationLinkGenerator)
    {
        _invitationsRepository = invitationsRepository;
        _tokenService = tokenService;
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _webhookService = webhookService;
        _invitationLinkGenerator = invitationLinkGenerator;
    }


    public async Task InviteMember(InviteMemberRequestDto inviteMemberRequestDto)
    {
        var user = await _userRepository.GetByIdAsync(inviteMemberRequestDto.InvitedByUserId);
        if (user == null) throw new Exception("Owner User not found");
        var team = await _teamRepository.GetByIdAsync(inviteMemberRequestDto.InvitedToTeamId);
        if (team == null) throw new Exception("Team not found");
        var code = _tokenService.GenerateInvitationCode();
        var url = _invitationLinkGenerator.GenerateInvitationLink(code, inviteMemberRequestDto.Email);
        await _webhookService.SendInvitationAsync(inviteMemberRequestDto.Email, url, user.Name, team.Name);
        await _invitationsRepository.AddAsync(new TeamInvitation(team.Id, user.Id, code, inviteMemberRequestDto.Email));
    }

    public async Task<InvitationValidateResultDto> ValidateInvitation(string code, string email)
    {
        var invitation = await _invitationsRepository.GetByCodeAsync(code);
        if (invitation == null) return new InvitationValidateResultDto()
        {
            CodeIsValid = false,
            Message = "Invitation with that code was not found"
        };
        if (invitation.ExpiresAtUtc < DateTime.UtcNow)
        { 
            invitation.Expire();
            await _invitationsRepository.SaveChangesAsync();
            return new InvitationValidateResultDto()
            {
                CodeIsValid = true,
                InvitationIsExpired = true,
                Message = "Invitation is already expired" 
            };
        }
        var user = await _userRepository.GetByEmailAsync(new Email(email));
        if (user == null)
            return new InvitationValidateResultDto()
            {
                CodeIsValid = true,
                InvitationIsExpired = false,
                EmailExists = false,
                Message = "A user created with that email was not found"
            };
        var teamId = await _teamRepository.GetByIdAsync(invitation.TeamId);
        var team = await _teamRepository.GetMemberAsync(teamId.Id, user.Id);
        if (team != null) return new InvitationValidateResultDto
        {
            CodeIsValid = true,
            InvitationIsExpired = false,
            EmailExists = true,
            UserAlreadyInTeam = true,
            Message = "The user is already in the team"
        };
        return new InvitationValidateResultDto
        {
            CodeIsValid = true,
            InvitationIsExpired = false,
            EmailExists = true,
            UserAlreadyInTeam = false,
            Message = "Invitation is Ok"
        };
    }

    public async Task<AcceptInvitationResultDto> AcceptInvitation(string code)
    {
        var invitation = await _invitationsRepository.GetByCodeAsync(code);
        invitation.Accept();
        await _invitationsRepository.SaveChangesAsync();
        var invitedUser = await _userRepository.GetByEmailAsync(invitation.InvitedUserEmail);
        var user = await _userRepository.GetByIdAsync(invitation.InvitedByUserId);
        var team = await _teamRepository.GetByIdAsync(invitation.TeamId);
        await _webhookService.SendConfirmationAsync(user.Email.Value, invitedUser.Name, team.Name, user.Name);
        var teamMember = new TeamMember(team.Id, user.Id, TeamRole.Developer, false, "", 1);
        await _teamRepository.AddMemberAsync(teamMember);
        return new AcceptInvitationResultDto()
        {
            Succeeded = true,
            Message = $"User succesfully joined the team {team.Name}"
        };
    }
}