using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class TeamInvitationConfiguration : IEntityTypeConfiguration<TeamInvitation>
{
    public void Configure(EntityTypeBuilder<TeamInvitation> builder)
    {
        builder.ToTable("team_invitations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.TeamId)
            .HasColumnName("team_id")
            .IsRequired();

        builder.Property(x => x.InvitedByUserId)
            .HasColumnName("invited_by_user_id")
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(200)
            .IsRequired();

        //-------------------------
        //     VALUE OBJECT Email
        //-------------------------
        var emailConverter = new ValueConverter<Email, string>(
            v => v.Value,
            v => new Email(v)
        );

        builder.Property(x => x.InvitedUserEmail)
            .HasColumnName("invited_user_email")
            .HasConversion(emailConverter)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ExpiresAtUtc)
            .HasColumnName("expires_at_utc")
            .HasColumnType("datetime(6)")
            .IsRequired();
    }
}