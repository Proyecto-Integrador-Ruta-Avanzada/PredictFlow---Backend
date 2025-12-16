using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class SprintTaskConfiguration : IEntityTypeConfiguration<SprintTask>
{
    public void Configure(EntityTypeBuilder<SprintTask> builder)
    {
        builder.ToTable("sprint_tasks");

        // Composite key
        builder.HasKey(st => new { st.SprintId, st.TaskId });

        builder.Property(st => st.SprintId)
            .HasColumnName("sprint_id")
            .IsRequired();

        builder.Property(st => st.TaskId)
            .HasColumnName("task_id")
            .IsRequired();

        // FK constraints
        builder.HasOne<Sprint>()
            .WithMany(s => s.Tasks)
            .HasForeignKey(st => st.SprintId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<TaskEntity>()
            .WithMany()
            .HasForeignKey(st => st.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(st => st.TaskId);
    }
}