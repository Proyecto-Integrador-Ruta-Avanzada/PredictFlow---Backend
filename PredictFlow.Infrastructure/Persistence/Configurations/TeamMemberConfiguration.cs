using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PredictFlow.Domain.Entities;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("team_members");

        builder.HasKey(tm => new { tm.TeamId, tm.UserId });

        builder.Property(tm => tm.TeamId)
            .HasColumnName("team_id");

        builder.Property(tm => tm.UserId)
            .HasColumnName("user_id");

        builder.Property(tm => tm.Role)
            .HasColumnName("role")
            .HasConversion<string>() // Importante si Role es enum
            .HasMaxLength(80);

        builder.Property(tm => tm.Skills)
            .HasColumnName("skills")
            .HasMaxLength(1000);

        builder.Property(tm => tm.Workload)
            .HasColumnName("workload");

        builder.Property(tm => tm.Availability)
            .HasColumnName("availability");

        builder.Property(tm => tm.PerformanceMetrics)
            .HasColumnName("performance_metrics")
            .HasMaxLength(2000);

        // TeamMember -> Team (N:1)
        builder.HasOne<Team>()
            .WithMany(t => t.Members)
            .HasForeignKey(tm => tm.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        // TeamMember -> User (N:1)
        builder.HasOne(tm => tm.User)
            .WithMany()
            .HasForeignKey(tm => tm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tm => tm.UserId);
    }
}