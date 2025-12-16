using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PredictFlow.Domain.Entities;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumn>
{
    public void Configure(EntityTypeBuilder<BoardColumn> builder)
    {
        builder.ToTable("board_columns");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(c => c.BoardId)
            .HasColumnName("board_id")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Position)
            .HasColumnName("position")
            .IsRequired();

        // Tasks navigation: BoardColumn -> TaskEntity
        builder.HasMany(c => c.Tasks)
            .WithOne()
            .HasForeignKey(t => t.BoardColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.BoardId);
    }
}