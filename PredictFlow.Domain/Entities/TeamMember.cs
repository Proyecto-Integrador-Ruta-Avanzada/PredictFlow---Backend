using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Domain.Entities;

public class TeamMember
{
    public Guid TeamId { get; private set; }
    public Guid UserId { get; private set; }
    
    public User User { get; private set; }
    
    public TeamRole Role { get; private set; }
    public string Skills { get; private set; }
    public int Workload { get; private set; }
    public int Availability { get; private set; }
    public string? PerformanceMetrics { get; private set; }
    

    private TeamMember() { }

    public TeamMember(Guid teamId, Guid userId, TeamRole role, string skills, int availability)
    {
        TeamId = teamId;
        UserId = userId;
        Role = role;
        Skills = skills;
        Workload = 0;   
        Availability = availability;
    }

    public void UpdateWorkload(int value)
    {
        Workload = value;
    }
    
    public void UpdateRole(TeamRole newRole)
    {
        Role = newRole;
    }

    public void UpdateSkills(string newSkills)
    {
        Skills = newSkills;
    }

    public void UpdateAvailability(int newAvailability)
    {
        Availability = newAvailability;
    }
}