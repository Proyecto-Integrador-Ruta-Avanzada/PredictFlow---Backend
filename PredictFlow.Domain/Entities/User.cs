using PredictFlow.Domain.ValueObjects;

namespace PredictFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { } // EF/ORM

    public User(string name, Email email, string passwordHash, string role)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}
