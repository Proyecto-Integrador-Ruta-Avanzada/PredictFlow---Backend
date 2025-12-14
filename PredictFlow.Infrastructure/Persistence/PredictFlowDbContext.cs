using Microsoft.EntityFrameworkCore;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Infrastructure.Persistence;

public class PredictFlowDbContext : DbContext
{
    public PredictFlowDbContext(DbContextOptions<PredictFlowDbContext> options) : base(options) { }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Sprint> Sprints => Set<Sprint>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<BoardColumn> BoardColumns => Set<BoardColumn>();
    public DbSet<SprintTask> SprintTasks => Set<SprintTask>();
    public DbSet<TeamInvitation> Invitations => Set<TeamInvitation>();
    public DbSet<TaskDependency> TaskDependencies => Set<TaskDependency>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PredictFlowDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}