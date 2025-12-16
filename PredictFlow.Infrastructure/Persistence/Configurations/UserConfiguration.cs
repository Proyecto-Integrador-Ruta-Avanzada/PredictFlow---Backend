using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PredictFlow.Domain.Entities;
using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        // Email is a ValueObject -> store as string
        var emailConverter = new ValueConverter<Email, string>(
            v => v.Value,
            v => new Email(v)
        );

        builder.Property(u => u.Email)
            .HasConversion(emailConverter)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(u => u.RefreshToken)
            .HasColumnName("refresh_token") // (Opcional) Asigna un nombre a la columna
            .HasMaxLength(500); // Esto hace que la columna sea NVARCHAR(500) en la DB

        builder.Property(u => u.RefreshTokenExpiryTime)
            .HasColumnName("refresh_token_expiry_time"); // Usa el tipo de dato por defecto (datetime/datetime2)

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime(6)")
            .IsRequired();
    }
}