using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("tasks");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(t => t.BoardColumnId)
            .HasColumnName("board_column_id")
            .IsRequired();

        builder.Property(t => t.Title)
            .HasColumnName("title")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(4000)
            .IsRequired(false);

        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by")
            .IsRequired();

        builder.Property(t => t.AssignedTo)
            .HasColumnName("assigned_to")
            .IsRequired(false);

        // Priority: enum -> int
        builder.Property(t => t.Priority)
            .HasColumnName("priority")
            .HasConversion<int>()
            .IsRequired();

        // StoryPoints: ValueObject -> int
        var spConverter = new ValueConverter<StoryPoints, int>(
            v => v.Value,
            v => new StoryPoints(v)
        );

        builder.Property(t => t.StoryPoints)
            .HasConversion(spConverter)
            .HasColumnName("story_points")
            .IsRequired();

        // TaskState (enum) -> int
        builder.Property(t => t.State)
            .HasColumnName("state")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime(6)")
            .IsRequired();

        // Indexes commonly used
        builder.HasIndex(t => t.BoardColumnId);
        builder.HasIndex(t => t.CreatedBy);
        builder.HasIndex(t => t.AssignedTo);

        // Relations: tasks -> users (created_by, assigned_to) are FKs but Entities lack navigation properties
        // Configure FK constraints to users table
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.AssignedTo)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
