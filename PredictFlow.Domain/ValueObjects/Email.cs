using System.Text.RegularExpressions;

namespace PredictFlow.Domain.ValueObjects;

public record Email
{
    public string Value { get; private set; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Email format is invalid");

        Value = value.Trim();
    }

    public override string ToString() => Value;
}