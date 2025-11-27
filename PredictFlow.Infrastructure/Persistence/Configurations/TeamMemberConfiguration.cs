using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("team_members");

        // Composite key: TeamId + UserId
        builder.HasKey(tm => new { tm.TeamId, tm.UserId });

        builder.Property(tm => tm.TeamId)
            .HasColumnName("team_id")
            .IsRequired();

        builder.Property(tm => tm.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(tm => tm.Role)
            .HasColumnName("role")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(tm => tm.Skills)
            .HasColumnName("skills")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(tm => tm.Workload)
            .HasColumnName("workload")
            .IsRequired();

        builder.Property(tm => tm.Availability)
            .HasColumnName("availability")
            .IsRequired();

        builder.Property(tm => tm.PerformanceMetrics)
            .HasColumnName("performance_metrics")
            .HasMaxLength(2000)
            .IsRequired(false);

        // Foreign keys to teams and users (User navigation lives in User entity; we configure FKs here)
        builder.HasOne<Team>()
            .WithMany(t => t.Members)
            .HasForeignKey(tm => tm.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tm => tm.UserId);
    }
}