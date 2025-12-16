namespace PredictFlow.Domain.ValueObjects;

public record StoryPoints
{
    public int Value { get; }

    public StoryPoints(int value)
    {
        if (value < 0)
            throw new ArgumentException("Story points cannot be negative");

        if (value > 5)
            throw new ArgumentException("Story points value is unrealistic");

        Value = value;
    }

    public override string ToString() => Value.ToString();
}