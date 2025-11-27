using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ToTable("sprints");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(s => s.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(s => s.Goal)
            .HasColumnName("goal")
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.HasMany(s => s.Tasks)
            .WithOne()
            .HasForeignKey(st => st.SprintId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProjectId);
    }
}