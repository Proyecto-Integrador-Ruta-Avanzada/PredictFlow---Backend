using PredictFlow.Application.DTOs.Profile;
using PredictFlow.Application.Interfaces;
using PredictFlow.Domain.Interfaces;

namespace PredictFlow.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IProjectRepository _projectRepository;

    public ProfileService(
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        IProjectRepository projectRepository)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ProfileResponseDto> GetMyProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
                   ?? throw new Exception("User not found");

        var teams = await _teamRepository.GetTeamsByUserIdAsync(userId);

        var teamProfiles = new List<TeamProfileDto>();

        foreach (var team in teams)
        {
            var member = team.Members.FirstOrDefault(m => m.UserId == userId)
                         ?? throw new Exception("User is not a member of this team");

            var projects = await _projectRepository.GetByTeamIdAsync(team.Id);

            teamProfiles.Add(new TeamProfileDto
            {
                TeamId = team.Id,
                TeamName = team.Name,
                Role = member.Role,
                Skills = member.Skills,
                Availability = member.Availability,
                Workload = member.Workload,
                Projects = projects.Select(p => new ProjectProfileDto
                {
                    ProjectId = p.Id,
                    Name = p.Name
                }).ToList()
            });
        }

        return new ProfileResponseDto
        {
            User = new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                CreatedAt = user.CreatedAt
            },
            Teams = teamProfiles
        };
    }
}
